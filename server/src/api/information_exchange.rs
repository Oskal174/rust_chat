use std::net::TcpStream;
use rusqlite::{Connection, ToSql, NO_PARAMS};
use serde_json::to_string;

use super::messages::*;
use super::super::Session;


pub fn process_send_message(socket: &mut TcpStream, sqlite_handler: &Connection, session: &mut Session, send_message_data: SendMessage) {
    if session.is_authenticated == false {
        let responce = ServerResponse {action: "send_message".to_string(), code: 400, message: "User dont authenticated".to_string()};
        send_message(socket, to_string(&responce).unwrap(), true).unwrap();
        return;
    }

    sqlite_handler.execute("INSERT INTO messages (user_id, room_id, text, timestamp) VALUES (?1, ?2, ?3, ?4)", &[&session.user_id, &session.room_id, &send_message_data.text as &ToSql, &send_message_data.timestamp]).unwrap();
}


pub fn process_get_messages(socket: &mut TcpStream, sqlite_handler: &Connection, session: &mut Session, get_message_data: GetMessages) {
    if session.is_authenticated == false {
        let responce = ServerResponse {action: "get_messages".to_string(), code: 400, message: "User dont authenticated".to_string()};
        send_message(socket, to_string(&responce).unwrap(), true).unwrap();
        return;
    }

    let sql = format!("SELECT u.login, m.text, m.timestamp FROM messages as m INNER JOIN user as u ON m.user_id = u.id WHERE m.room_id = {}", session.room_id);
    let mut stmt: rusqlite::Statement;
    match sqlite_handler.prepare(&sql) {
        Ok(_s) => stmt = _s,
        Err(_e) => {
            let responce = ServerResponse {action: "get_messages".to_string(), code: 400, message: _e.to_string()};
            send_message(socket, to_string(&responce).unwrap(), true).unwrap();
            return;
        }
    };

    let mut responce = GetMessagesResponce::default();
    let mut rows = stmt.query(NO_PARAMS).unwrap();
    while let Some(row) = rows.next().unwrap() {
        let msg = Message {author: row.get(0).unwrap(), text: row.get(1).unwrap(), timestamp: row.get(2).unwrap()};
        responce.messages.push(msg);
    }

    send_message(socket, to_string(&responce).unwrap(), true).unwrap();
}
