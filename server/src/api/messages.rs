use std::io::{Read, Write};
use std::net::TcpStream;
use serde_derive::{Serialize, Deserialize};

#[derive(Deserialize, Serialize)]
pub struct ServerResponse {
    pub action: String,
    pub code: i32,
    pub message: String
}

#[derive(Deserialize, Serialize)]
pub struct HandshakeMessage {
    pub action: String,
    pub secret: String
}

#[derive(Deserialize, Serialize)]
pub struct AuthenticationMessage {
    pub action: String,
    pub login: String,
    pub password_hash: String
}

#[derive(Deserialize, Serialize)]
pub struct RegistrationMessage {
    pub action: String,
    pub login: String,
    pub password_hash: String
}

#[derive(Deserialize, Serialize)]
pub struct DeleteAccountMessage {
    pub action: String,
    pub login: String,
    pub password_hash: String
}

#[derive(Deserialize, Serialize)]
pub struct UpdatePasswordMessage {
    pub action: String,
    pub login: String,
    pub password_hash: String,
    pub new_password_hash: String
}


#[derive(Deserialize, Serialize)]
pub struct SendMessage {
    pub action: String,
    pub text: String,
    pub timestamp: i32
}


pub fn recv_string(socket: &mut TcpStream, is_logging: bool) -> Result<String, std::io::Error> {
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


pub fn send_message(socket: &mut TcpStream, msg: String, is_logging: bool) -> Result<usize, std::io::Error> {
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
