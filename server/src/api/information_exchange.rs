use std::net::TcpStream;
use rusqlite::{Connection, ToSql};
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
