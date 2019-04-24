extern crate rusqlite;
extern crate serde;
extern crate serde_derive;
extern crate serde_json;

use std::net::{SocketAddrV4, Ipv4Addr, TcpListener, TcpStream};
use std::io::Read;
use std::io::Write;
use std::thread;
use std::time::Duration;

use serde_derive::{Serialize, Deserialize};
use serde_json::Value;
use rusqlite::{Connection, NO_PARAMS};

static HANDSHAKE_SECRET: &'static str = "VXp8v8rF7YefA1hqOX51Wl7g";

#[derive(Deserialize, Serialize)]
struct ServerResponse {
    action: String,
    code: i32,
    message: String
}

#[derive(Deserialize, Serialize)]
struct HandshakeMessage {
    action: String,
    secret: String
}

#[derive(Deserialize, Serialize)]
struct AuthenticationMessage {
    action: String,
    login: String,
    password_hash: String
}

#[derive(Deserialize, Serialize)]
struct RegistrationMessage {
    action: String,
    login: String,
    password_hash: String
}

#[derive(Deserialize, Serialize)]
struct DeleteAccountMessage {
    action: String,
    login: String,
    password_hash: String
}

#[derive(Deserialize, Serialize)]
struct UpdatePasswordMessage {
    action: String,
    login: String,
    password_hash: String,
    new_password_hash: String
}


fn main() {
    init_database();

    let loopback = Ipv4Addr::new(127, 0, 0, 1);
    let socket = SocketAddrV4::new(loopback, 9090);
    
    let listener;
    match TcpListener::bind(socket) {
        Ok(l) => listener = l,
        Err(e) => panic!("{}", e)
    }

    let port;
    match listener.local_addr() {
        Ok(p) => {
            port = p; 
            println!("Listening on {}", port);
        },
        Err(e) => println!("{}", e)
    }
    
    loop {
        let mut tcp_stream;
        let addr;
        match listener.accept() {
            Ok((ts, a)) => {
                tcp_stream = ts;
                addr = a;
            },
            Err(e) => {
                println!("{}", e); 
                continue 
            }
        } 

        println!("Connection received from {:?}", addr);

        thread::Builder::new().name(addr.to_string()).spawn(move || {
           processing_client(tcp_stream); 
        }).unwrap();
        std::thread::sleep(Duration::from_millis(50));
    }
}


fn processing_client(mut client_socket: TcpStream) {
    let sqlite_handler = Connection::open("chat.db").unwrap();
    // First process HandShake then receiving messages from client
    let mut input_str = recv_message(&mut client_socket, true).unwrap();
    match process_handshake(&mut client_socket, serde_json::from_str(&input_str).unwrap()) {
        Ok(_) => {},
        Err(_e) => panic!("{}", _e)
    }

    loop {
        input_str = recv_message(&mut client_socket, true).unwrap();
        let input_json: Value = serde_json::from_str(&input_str).unwrap();
        
        let action: String = input_json["action"].to_string();
        match &action as &str {
            "\"authentication\""    => process_authentication(&mut client_socket, &sqlite_handler, serde_json::from_str(&input_str).unwrap()),
            "\"registration\""      => process_registration(&mut client_socket, &sqlite_handler, serde_json::from_str(&input_str).unwrap()),
            "\"delete_account\""    => process_delete_account(&mut client_socket, &sqlite_handler, serde_json::from_str(&input_str).unwrap()),
            "\"update_password\""   => process_update_password(&mut client_socket, &sqlite_handler, serde_json::from_str(&input_str).unwrap()),
            _ => {},
        }

        thread::sleep(Duration::from_millis(100));
    }
}


/**
 * Docs for function
 */
fn recv_message(socket: &mut TcpStream, is_logging: bool) -> Result<String, std::io::Error> {
    let mut input_array: [u8; 2048] = [0; 2048];
    match socket.read(&mut input_array) {
        Ok(_n) => {
            if _n == 0 {
                return Ok(String::new())
            }

            if _n > 2048 { 
                println!("Error recv too much bytes ({})", _n); 
                return Err(std::io::Error::new(std::io::ErrorKind::InvalidInput, "Too long message"))
            }

            match String::from_utf8(input_array[0.._n].to_vec()) {
                Ok(r) => {
                    if is_logging { 
                        println!("Recv ({}): {}", _n, r)
                    }
                    Ok(r)
                }
                Err(_e) => { 
                    println!("{}", _e);
                    Err(std::io::Error::new(std::io::ErrorKind::InvalidInput, _e.to_string()))
                }
            }
        },
        
        Err(_e) => { 
            println!("{}", _e);
            Err(_e)
        }
    }
}


fn send_message(socket: &mut TcpStream, msg: String, is_logging: bool) -> Result<usize, std::io::Error> {
    match socket.write(msg.as_bytes()) {
        Ok(_n) => {
            if is_logging {println!("Send ({}): {}", _n, msg)}
            return Ok(_n)
        },
        Err(_e) => {
            println!("{}", _e);
            return Err(_e)
        }
    }
}


fn process_handshake(socket: &mut TcpStream, handshake: HandshakeMessage) -> Result<i32, String> {
    let mut response = ServerResponse {action: "handshake".to_string(), code: 200, message: "OK".to_string()};

    if handshake.secret != HANDSHAKE_SECRET {
        response = ServerResponse {action: "handshake".to_string(), code: 400, message: "Wrong secret phrase".to_string()};        
        send_message(socket, serde_json::to_string(&response).unwrap(), true).unwrap();
        return Err(response.message);
    }

    send_message(socket, serde_json::to_string(&response).unwrap(), true).unwrap();
    Ok(response.code)
}


/**
 * For now it`s just yes/no. In future it will return role of user. And of cource need reading database...
*/
fn process_authentication(socket: &mut TcpStream, sqlite_handler: &Connection, authentication_data: AuthenticationMessage) {
    let sql = format!("SELECT password FROM user WHERE login = \"{}\"", authentication_data.login);
    let mut stmt: rusqlite::Statement;
    match sqlite_handler.prepare(&sql) {
        Ok(_s) => stmt = _s,
        Err(_e) => {
            let msg = ServerResponse {action: "authentication".to_string(), code: 400, message: _e.to_string()};
            send_message(socket, serde_json::to_string(&msg).unwrap(), true).unwrap();
            return;
        }
    };
    
    let password_hash: String;
    let mut rows = stmt.query(NO_PARAMS).unwrap();
    match rows.next() {
        Ok(Some(_row)) => { 
            password_hash = _row.get(0).unwrap();
        },
        Ok(None) => {
            let msg = ServerResponse {action: "authentication".to_string(), code: 400, message: "Login does not exist".to_string()};
            send_message(socket, serde_json::to_string(&msg).unwrap(), true).unwrap();
            return;
        },
        Err(_e) => {
            let msg = ServerResponse {action: "authentication".to_string(), code: 400, message: _e.to_string()};
            send_message(socket, serde_json::to_string(&msg).unwrap(), true).unwrap();
            return;
        }
    }
    
    let mut response = ServerResponse {action: "authentication".to_string(), code: 200, message: "OK".to_string()};
    if authentication_data.password_hash != password_hash {
        response = ServerResponse {action: "authentication".to_string(), code: 400, message: "Wrong login or password".to_string()};
    }

    send_message(socket, serde_json::to_string(&response).unwrap(), true).unwrap();
}


fn process_registration(socket: &mut TcpStream, sqlite_handler: &Connection, registration_data: RegistrationMessage) {
    let sql = format!("SELECT id FROM user WHERE login = \"{}\"", registration_data.login);
    let mut stmt: rusqlite::Statement;
    match sqlite_handler.prepare(&sql) {
        Ok(_s) => stmt = _s,
        Err(_e) => {
            let msg = ServerResponse {action: "authentication".to_string(), code: 400, message: _e.to_string()};
            send_message(socket, serde_json::to_string(&msg).unwrap(), true).unwrap();
            return;
        }
    };

    let mut rows = stmt.query(NO_PARAMS).unwrap();
    match rows.next() {
        Ok(Some(_row)) => { 
            let msg = ServerResponse {action: "authentication".to_string(), code: 400, message: "Login already exists".to_string()};
            send_message(socket, serde_json::to_string(&msg).unwrap(), true).unwrap();
            return;
        },
        Ok(None) => {},
        Err(_e) => {
            let msg = ServerResponse {action: "authentication".to_string(), code: 400, message: _e.to_string()};
            send_message(socket, serde_json::to_string(&msg).unwrap(), true).unwrap();
            return;
        }
    }

    
    match sqlite_handler.execute("INSERT INTO user (login, password, room_id) VALUES (?1, ?2, 1)", &[registration_data.login, registration_data.password_hash]) {
        Ok(_u) => {
            let response = ServerResponse {action: "authentication".to_string(), code: 200, message: "OK".to_string()};
            send_message(socket, serde_json::to_string(&response).unwrap(), true).unwrap();
        }
        Err(_e) => {
            let msg = ServerResponse {action: "authentication".to_string(), code: 400, message: _e.to_string()};
            send_message(socket, serde_json::to_string(&msg).unwrap(), true).unwrap();
        }
    }
}


fn process_delete_account(socket: &mut TcpStream, sqlite_handler: &Connection, delete_data: DeleteAccountMessage) {
    let sql = format!("SELECT password FROM user WHERE login = \"{}\"", delete_data.login);
    let mut stmt: rusqlite::Statement;
    match sqlite_handler.prepare(&sql) {
        Ok(_s) => stmt = _s,
        Err(_e) => {
            let msg = ServerResponse {action: "delete_account".to_string(), code: 400, message: _e.to_string()};
            send_message(socket, serde_json::to_string(&msg).unwrap(), true).unwrap();
            return;
        }
    };

    let mut rows = stmt.query(NO_PARAMS).unwrap();
    match rows.next() {
        Ok(Some(_row)) => { 
            let password_hash: String = _row.get(0).unwrap();
            if delete_data.password_hash != password_hash {
                let msg = ServerResponse {action: "delete_account".to_string(), code: 400, message: "Wrong user password".to_string()};
                send_message(socket, serde_json::to_string(&msg).unwrap(), true).unwrap();
                return;
            }
        },
        Ok(None) => {},
        Err(_e) => {
            let msg = ServerResponse {action: "delete_account".to_string(), code: 400, message: _e.to_string()};
            send_message(socket, serde_json::to_string(&msg).unwrap(), true).unwrap();
            return;
        }
    }

    match sqlite_handler.execute("DELETE FROM user WHERE login = ?1", &[delete_data.login]) {
        Ok(_u) => {
            let response = ServerResponse {action: "delete_account".to_string(), code: 200, message: "OK".to_string()};
            send_message(socket, serde_json::to_string(&response).unwrap(), true).unwrap();
        }
        Err(_e) => {
            let msg = ServerResponse {action: "delete_account".to_string(), code: 400, message: _e.to_string()};
            send_message(socket, serde_json::to_string(&msg).unwrap(), true).unwrap();
        }
    }
}


fn process_update_password(socket: &mut TcpStream, sqlite_handler: &Connection, update_data: UpdatePasswordMessage) {
    let sql = format!("SELECT password FROM user WHERE login = \"{}\"", update_data.login);
    let mut stmt: rusqlite::Statement;
    match sqlite_handler.prepare(&sql) {
        Ok(_s) => stmt = _s,
        Err(_e) => {
            let msg = ServerResponse {action: "update_password".to_string(), code: 400, message: _e.to_string()};
            send_message(socket, serde_json::to_string(&msg).unwrap(), true).unwrap();
            return;
        }
    };

    let mut rows = stmt.query(NO_PARAMS).unwrap();
    match rows.next() {
        Ok(Some(_row)) => { 
            let password_hash: String = _row.get(0).unwrap();
            if update_data.password_hash != password_hash {
                let msg = ServerResponse {action: "update_password".to_string(), code: 400, message: "Wrong user password".to_string()};
                send_message(socket, serde_json::to_string(&msg).unwrap(), true).unwrap();
                return;
            }

            if update_data.new_password_hash == password_hash {
                let msg = ServerResponse {action: "update_password".to_string(), code: 400, message: "The new password should not be the same as the old one".to_string()};
                send_message(socket, serde_json::to_string(&msg).unwrap(), true).unwrap();
                return;
            }
        },
        Ok(None) => {},
        Err(_e) => {
            let msg = ServerResponse {action: "update_password".to_string(), code: 400, message: _e.to_string()};
            send_message(socket, serde_json::to_string(&msg).unwrap(), true).unwrap();
            return;
        }
    }

    match sqlite_handler.execute("UPDATE user SET password = ?1 WHERE login = ?2", &[update_data.new_password_hash, update_data.login]) {
        Ok(_u) => {
            let response = ServerResponse {action: "update_password".to_string(), code: 200, message: "OK".to_string()};
            send_message(socket, serde_json::to_string(&response).unwrap(), true).unwrap();
        }
        Err(_e) => {
            let msg = ServerResponse {action: "update_password".to_string(), code: 400, message: _e.to_string()};
            send_message(socket, serde_json::to_string(&msg).unwrap(), true).unwrap();
        }
    }
}


fn init_database() {
    let sqlite_handler: Connection = Connection::open("chat.db").unwrap();
    sqlite_handler.execute(
        "CREATE TABLE IF NOT EXISTS user (
                  id              INTEGER PRIMARY KEY,
                  login           TEXT NOT NULL UNIQUE,
                  password        TEXT NOT NULL,
                  room_id         INTEGER REFERENCES room (id) NOT NULL
                )",
        NO_PARAMS,
    ).unwrap();
    sqlite_handler.execute(
        "CREATE TABLE IF NOT EXISTS room (
                  id              INTEGER PRIMARY KEY,
                  name            TEXT NOT NULL UNIQUE
                )",
        NO_PARAMS,
    ).unwrap();
    sqlite_handler.execute(
        "CREATE TABLE IF NOT EXISTS messages (
                  id              INTEGER PRIMARY KEY,
                  user_id         INTEGER REFERENCES user (id) NOT NULL,
                  room_id         INTEGER REFERENCES room (id) NOT NULL,
                  header          TEXT NOT NULL,
                  text            TEXT NOT NULL,
                  date            TEXT NOT NULL
                )",
        NO_PARAMS,
    ).unwrap();
    sqlite_handler.execute(
        "INSERT INTO room(id, name) VALUES(1, \"common\")",
        NO_PARAMS,
    ).ok();
    sqlite_handler.close().unwrap();
}
