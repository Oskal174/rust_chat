use std::net::TcpStream;
use rusqlite::{Connection, NO_PARAMS};
use serde_json::to_string;

use super::messages::*;
use super::super::Session;


pub fn process_authentication(socket: &mut TcpStream, sqlite_handler: &Connection, session: &mut Session, authentication_data: AuthenticationMessage) {
    if session.is_authenticated == true {
        let responce = ServerResponse {action: "authentication".to_string(), code: 300, message: "User already authenticated".to_string()};
        send_message(socket, to_string(&responce).unwrap(), true).unwrap();
        return;
    }
    
    session.is_authenticated = false;
    session.login = "none".to_string();

    let sql = format!("SELECT id, room_id, password FROM user WHERE login = \"{}\"", authentication_data.login);
    let mut stmt: rusqlite::Statement;
    match sqlite_handler.prepare(&sql) {
        Ok(_s) => stmt = _s,
        Err(_e) => {
            let responce = ServerResponse {action: "authentication".to_string(), code: 400, message: _e.to_string()};
            send_message(socket, to_string(&responce).unwrap(), true).unwrap();
            return;
        }
    };
    
    let password_hash: String;
    let user_id: i32;
    let room_id: i32;
    let mut rows = stmt.query(NO_PARAMS).unwrap();
    match rows.next() {
        Ok(Some(_row)) => { 
            user_id = _row.get(0).unwrap();
            room_id = _row.get(1).unwrap();
            password_hash = _row.get(2).unwrap();
        },
        Ok(None) => {
            let msg = ServerResponse {action: "authentication".to_string(), code: 400, message: "Login does not exist".to_string()};
            send_message(socket, to_string(&msg).unwrap(), true).unwrap();
            return;
        },
        Err(_e) => {
            let msg = ServerResponse {action: "authentication".to_string(), code: 400, message: _e.to_string()};
            send_message(socket, to_string(&msg).unwrap(), true).unwrap();
            return;
        }
    } 
    
    if authentication_data.password_hash != password_hash {
        let response = ServerResponse {action: "authentication".to_string(), code: 400, message: "Wrong login or password".to_string()};
        send_message(socket, to_string(&response).unwrap(), true).unwrap();
    }
    else {
        session.is_authenticated = true;
        session.login = authentication_data.login;
        session.user_id = user_id;
        session.room_id = room_id;
        let response = ServerResponse {action: "authentication".to_string(), code: 200, message: "OK".to_string()};
        send_message(socket, to_string(&response).unwrap(), true).unwrap();
    }
}


pub fn process_logout(socket: &mut TcpStream, session: &mut Session) {
    if session.is_authenticated == false {
        let responce = ServerResponse {action: "logout".to_string(), code: 400, message: "User is not authenticated".to_string()};
        send_message(socket, to_string(&responce).unwrap(), true).unwrap();
        return;
    }

    session.is_authenticated = false;
    let responce = ServerResponse {action: "logout".to_string(), code: 200, message: "OK".to_string()};
    send_message(socket, to_string(&responce).unwrap(), true).unwrap();
}


pub fn process_registration(socket: &mut TcpStream, sqlite_handler: &Connection, registration_data: RegistrationMessage) {
    let sql = format!("SELECT id FROM user WHERE login = \"{}\"", registration_data.login);
    let mut stmt: rusqlite::Statement;
    match sqlite_handler.prepare(&sql) {
        Ok(_s) => stmt = _s,
        Err(_e) => {
            let msg = ServerResponse {action: "authentication".to_string(), code: 400, message: _e.to_string()};
            send_message(socket, to_string(&msg).unwrap(), true).unwrap();
            return;
        }
    };

    let mut rows = stmt.query(NO_PARAMS).unwrap();
    match rows.next() {
        Ok(Some(_row)) => { 
            let msg = ServerResponse {action: "authentication".to_string(), code: 400, message: "Login already exists".to_string()};
            send_message(socket, to_string(&msg).unwrap(), true).unwrap();
            return;
        },
        Ok(None) => {},
        Err(_e) => {
            let msg = ServerResponse {action: "authentication".to_string(), code: 400, message: _e.to_string()};
            send_message(socket, to_string(&msg).unwrap(), true).unwrap();
            return;
        }
    }

    
    match sqlite_handler.execute("INSERT INTO user (login, password, room_id) VALUES (?1, ?2, 1)", &[registration_data.login, registration_data.password_hash]) {
        Ok(_u) => {
            let response = ServerResponse {action: "authentication".to_string(), code: 200, message: "OK".to_string()};
            send_message(socket, to_string(&response).unwrap(), true).unwrap();
        }
        Err(_e) => {
            let msg = ServerResponse {action: "authentication".to_string(), code: 400, message: _e.to_string()};
            send_message(socket, to_string(&msg).unwrap(), true).unwrap();
        }
    }
}


pub fn process_delete_account(socket: &mut TcpStream, sqlite_handler: &Connection, delete_data: DeleteAccountMessage) {
    let sql = format!("SELECT password FROM user WHERE login = \"{}\"", delete_data.login);
    let mut stmt: rusqlite::Statement;
    match sqlite_handler.prepare(&sql) {
        Ok(_s) => stmt = _s,
        Err(_e) => {
            let msg = ServerResponse {action: "delete_account".to_string(), code: 400, message: _e.to_string()};
            send_message(socket, to_string(&msg).unwrap(), true).unwrap();
            return;
        }
    };

    let mut rows = stmt.query(NO_PARAMS).unwrap();
    match rows.next() {
        Ok(Some(_row)) => { 
            let password_hash: String = _row.get(0).unwrap();
            if delete_data.password_hash != password_hash {
                let msg = ServerResponse {action: "delete_account".to_string(), code: 400, message: "Wrong user password".to_string()};
                send_message(socket, to_string(&msg).unwrap(), true).unwrap();
                return;
            }
        },
        Ok(None) => {},
        Err(_e) => {
            let msg = ServerResponse {action: "delete_account".to_string(), code: 400, message: _e.to_string()};
            send_message(socket, to_string(&msg).unwrap(), true).unwrap();
            return;
        }
    }

    match sqlite_handler.execute("DELETE FROM user WHERE login = ?1", &[delete_data.login]) {
        Ok(_u) => {
            let response = ServerResponse {action: "delete_account".to_string(), code: 200, message: "OK".to_string()};
            send_message(socket, to_string(&response).unwrap(), true).unwrap();
        }
        Err(_e) => {
            let msg = ServerResponse {action: "delete_account".to_string(), code: 400, message: _e.to_string()};
            send_message(socket, to_string(&msg).unwrap(), true).unwrap();
        }
    }
}


pub fn process_update_password(socket: &mut TcpStream, sqlite_handler: &Connection, update_data: UpdatePasswordMessage) {
    let sql = format!("SELECT password FROM user WHERE login = \"{}\"", update_data.login);
    let mut stmt: rusqlite::Statement;
    match sqlite_handler.prepare(&sql) {
        Ok(_s) => stmt = _s,
        Err(_e) => {
            let msg = ServerResponse {action: "update_password".to_string(), code: 400, message: _e.to_string()};
            send_message(socket, to_string(&msg).unwrap(), true).unwrap();
            return;
        }
    };

    let mut rows = stmt.query(NO_PARAMS).unwrap();
    match rows.next() {
        Ok(Some(_row)) => { 
            let password_hash: String = _row.get(0).unwrap();
            if update_data.password_hash != password_hash {
                let msg = ServerResponse {action: "update_password".to_string(), code: 400, message: "Wrong user password".to_string()};
                send_message(socket, to_string(&msg).unwrap(), true).unwrap();
                return;
            }

            if update_data.new_password_hash == password_hash {
                let msg = ServerResponse {action: "update_password".to_string(), code: 400, message: "The new password should not be the same as the old one".to_string()};
                send_message(socket, to_string(&msg).unwrap(), true).unwrap();
                return;
            }
        },
        Ok(None) => {},
        Err(_e) => {
            let msg = ServerResponse {action: "update_password".to_string(), code: 400, message: _e.to_string()};
            send_message(socket, to_string(&msg).unwrap(), true).unwrap();
            return;
        }
    }

    match sqlite_handler.execute("UPDATE user SET password = ?1 WHERE login = ?2", &[update_data.new_password_hash, update_data.login]) {
        Ok(_u) => {
            let response = ServerResponse {action: "update_password".to_string(), code: 200, message: "OK".to_string()};
            send_message(socket, to_string(&response).unwrap(), true).unwrap();
        }
        Err(_e) => {
            let msg = ServerResponse {action: "update_password".to_string(), code: 400, message: _e.to_string()};
            send_message(socket, to_string(&msg).unwrap(), true).unwrap();
        }
    }
}
