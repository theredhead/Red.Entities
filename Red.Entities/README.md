# Red.Entities 

This is the beginnings of the backend library that will allow me to bounce Entities (database records) 
back and forth between clients and server while tracking changes and updating clients in realtime.

The backend server will keep track of all entities currently in use on any client and update all 
clients that have an entity in their scope the moment it's modifications are persisted.

This ties in with my entity database designer plans as currently functionally implemented in a rough 
draft within the separate TypedUI project. The designer currently only creates an init script for the
database but that will be expanded to create Entity implementations as well and eventually should be
able to generate an entire tiered web application from an entity design. That should alow getting an
application up and running almost instantaneous and allow for one-off code generation to aid in rapid
application development for complicated trees of data.
