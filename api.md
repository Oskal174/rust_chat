Application programming interface for rust_messages.

Server responce:<br>
return_code [integer] - 200 for ok, 300 for warnings, 400 for errors.<br>
message [string] - what().<br>
Universal server response.

Hash - md5.

# User account
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

## User logout
### Client side
{"action": "logout"}

### Server side
{"action": "logout", "code": return_code, "message": "error_message"}

## Close connection
### Client side
{"action": "close_connection"}

### Server side
*closing connection*

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
{"action": "send_message", "text": "message_text",  "timestamp": unix_timestamp}

### Server side
*saving new message into database*

#### Response to sender
*not really sure* {"action": "send_message", "code": return_code, "message": "error_message"}

#### Response to all users in updated room
*not really sure* {"action": "get_message", "author": "author_login", "text": "message_text", "timestamp": unix_timestamp}

## Get messages (in current room)
### Client side
{"action": "get_messages", "number_of_messages": number}

*number_of_messages* - get last N messages or less.

### Server side
{"action": "get_messages", messages: [
    {"author": "message_author", "text": "message_text", "timestamp": unix_timestamp},
    ...
]}

# Rooms
## Create new room
### Client side
{"action": "create_room", "name": "room_name"}

### Server side
{"action": "create_room", "code": return_code, "message": "error_message"}

## Delete room
### Client side
{"action": "delete_room", "name": "room_name"}

### Server side
{"action": "delete_room", "code": return_code, "message": "error_message"}

## Change user`s room
### Client side
{"action": "change_room", "old_room_name": "old_room_name", "new_room_name": "new_room_name"}

### Server side
{"action": "change_room", "code": return_code, "message": "error_message"}
