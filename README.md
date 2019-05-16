# Simple tcp-chat
Client-server communication is based on [JSON-API](https://github.com/Oskal174/rust_chat/blob/master/api.md). Frontend side is based on C# because it`s easiest way.

Project architecture: new thread for every connected client. This is not the best architecture, so there is another server-side project on Rust with async-io (link to another project). Despite the problems with input/output this solution has the right to exist.

## Released features
* User registration 
* Updating user password
* Deleting user
* User authentication
* ...
