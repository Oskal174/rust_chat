extern crate rusqlite;
extern crate serde;
extern crate serde_derive;
extern crate serde_json;

mod api;

use std::net::{SocketAddrV4, Ipv4Addr, TcpListener, TcpStream};
use std::thread;
use std::time::Duration;

use serde_json::Value;
use rusqlite::{Connection, NO_PARAMS};

use api::messages::*;
use api::user_account::*;
use api::information_exchange::*;

static HANDSHAKE_SECRET: &'static str = "VXp8v8rF7YefA1hqOX51Wl7g";


pub struct Session {
    user_id: i32,
    login: String,
    is_authenticated: bool,
    room_id: i32,
    room_name: String
}

impl Session {
    fn new() -> Session {
        Session {
            user_id: 0,
            login: "none".to_string(), 
            is_authenticated: false, 
            room_id: 0, 
            room_name: "none".to_string()
        }
    }
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
        let mut client_socket;
        let addr;
        match listener.accept() {
            Ok((ts, a)) => {
                client_socket = ts;
                addr = a;
            },
            Err(e) => {
                println!("{}", e); 
                continue 
            }
        } 

        println!("Connection received from {:?}", addr);

        thread::Builder::new().name(addr.to_string()).spawn(move || {
           processing_client(client_socket); 
        }).unwrap();
    
        std::thread::sleep(Duration::from_millis(50));
    }
}


fn processing_client(mut client_socket: TcpStream) {
    let sqlite_handler = Connection::open("chat.db").unwrap();
    // First process HandShake then receiving messages from client
    let mut input_str = recv_string(&mut client_socket, true).unwrap();
    match process_handshake(&mut client_socket, serde_json::from_str(&input_str).unwrap()) {
        Ok(_) => {},
        Err(_e) => panic!("{}", _e)
    }

    let mut session = Session::new();
    loop {
        input_str = recv_string(&mut client_socket, true).unwrap();
        let input_json: Value = serde_json::from_str(&input_str).unwrap();

        let action: String = input_json["action"].to_string();
        match &action as &str {
            "\"authentication\""    => process_authentication(&mut client_socket, &sqlite_handler, &mut session, serde_json::from_str(&input_str).unwrap()),
            "\"logout\""            => process_logout(&mut client_socket, &mut session),
            
            "\"close_connection\""  => break,
            
            "\"registration\""      => process_registration(&mut client_socket, &sqlite_handler, serde_json::from_str(&input_str).unwrap()),
            "\"delete_account\""    => process_delete_account(&mut client_socket, &sqlite_handler, serde_json::from_str(&input_str).unwrap()),
            "\"update_password\""   => process_update_password(&mut client_socket, &sqlite_handler, serde_json::from_str(&input_str).unwrap()),
            
            "\"send_message\""      => process_send_message(&mut client_socket, &sqlite_handler, &mut session, serde_json::from_str(&input_str).unwrap()),
            "\"get_messages\""      => process_get_messages(&mut client_socket, &sqlite_handler, &mut session, serde_json::from_str(&input_str).unwrap()),
            _ => {},
        }

        thread::sleep(Duration::from_millis(100));
    }

    println!("User close connection, thread {} exiting...", thread::current().name().unwrap());
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
                  text            TEXT NOT NULL,
                  timestamp       INTEGER NOT NULL
                )",
        NO_PARAMS,
    ).unwrap();
    sqlite_handler.execute(
        "INSERT INTO room(id, name) VALUES(1, \"common\")",
        NO_PARAMS,
    ).ok();
    sqlite_handler.close().unwrap();
}
