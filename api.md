Application programming interface for rust_messages.

Server responce:<br>
return_code [integer] - 200 for ok, 400 for error.<br>
message [string] - what().<br>
Universal server response.

Hash - md5.

# Connection
## Handshake procedure
### Client side
{"action": "handshake", "secret": "handshake_secret"}

### Server side
{"action": "handshake", "code": return_code, "message": "error_message"}

## Authentication
### Client side
{"action":"authentication", "login": "client_login", "password": "client_password_hash"}

### Server side
{"action": "authentication", "code": return_code, "message": "error_message"}

## User registration
### Client side
{"action": "registration", "login": "client_login",  "password": "client_password_hash"}

### Server side
{"action": "registration", "code": return_code, "message": "error_message"}

## Change user password
### Client side
{"action": "change_password", "login": "client_login", "new_password": "new_client_password_hash"}

### Server side
{"action": "change_password", "code": return_code, "message": "error_message"}

## Delete user account
### Client side
{"action": "delete_account", "login": "client_login", "password": "client_password_hash"}

### Server side
{"action": "delete_account", "code": return_code, "message": "error_message"}

# Information exchange
## Send message
### Client side
{"action": "send_message", "room": room_id, "header": "message_header", "text": "message_text",  "date": "message_date"}

### Server side
{"action": "get_messages", "code": return_code, "message": "error_message"}

## Get all messages (in current room)
### Client side
{"action": "get_messages", "room": room_id}

### Server side
{"action": "get_messages", "code": return_code, "message": "error_message"}