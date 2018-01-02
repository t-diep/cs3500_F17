Created by Michael Valentine and Stephen Ancajas CS3500


------------------- Client README --------------------------

The game can be controlled using the W, A, D, and SPACEBAR keys.

W --------- Activates thrusters
A --------- Rotates the ship counter-clockwise
D --------- Rotates the ship clockwise
SPACEBAR -- Fires a projectile

The goal is to destroy all other player's ships while trying to avoid colliding with the sun.



------------------- Server README --------------------------

Server processes and maintains all connections for the SpaceWars game. The server detects
all collisions and updates the health and score of ships. 

EXTRA FEATURE:
If "MovingStars" is enabled in the settings, all stars will move to a random location every
700 frames.

Update log.
----------------------------------------------------------------
Thursday, November 30, 2017 by Stephen

- * Updated access modifier of the SpaceWarsServer class to static since the server will perserve its own instance
and the client instance of the world that will be shared to all clients.
- * Started designing method headers in SpaceWarsServer.

Total hours put in : 2
--------------------------------------------------------------------
Friday, November 31, 2017 by Stephen
- * New AcceptNewClient and AwaitingNewClientLoop implemented within the Networking class within the Controller project
** (Unstable) Not yet tested yet **
- Implemented some of the design document, several methods are there.
- * Refresh method is going to take some time just realizing that.
- * Made a GameSettings class just for reading the xml file and setting the server up yaas.

Total hours put in : 6
-----------------------------------------------------------------------------------
Saturday, December 2, 2017 by Michael
- Moved all methods to its proper file location and combined the two README files.
- Adjusted some networking methods
- Worked on the setup of the connection (send the world size and player ID for startup data)
- Created storage for connected clients and to update the connection's ID
- Started working on methods that are used to process commands received from a client

Total hours put in : 5
-----------------------------------------------------------------------------------
Sunday, December 3, 2017 by Michael
- Added methods to the Ship class (RotateLeft, RotateRight, SetHP, etc.)
- Added other methods the game objects such as SetLocation
- Added a method to the World class that updates Ships given a player's commands
- Fixed issues regarding the storage of connections

Total hours put in : 4
-----------------------------------------------------------------------------------

Sunday/Monday, December 4, 2017 by Stephen
- * Made a condition out of SendData if the socket is empty(connection) would remove that client and remove the world.
- * Fully implemented SpaceWarsServer
- * Refresh in Ship and Projectile, along with several other new methods including funky physics.
- * TODO: CLIENT IS UNABLE TO SEE BULLETS, NEEDs COMMENTING, EXITING GRACEFULLY OTHERWISE WE'RE DONE!

Total hours put in : 9
-----------------------------------------------------------------------------------
Monday/Tuesday, December 4/5, 2017 by Michael
- Added comments to a few methods
- Changed the way the game updates, it updates using a busy loop instead of a timer
- Fixed projectiles so that they are created displayed 6 units in front of ship

Total hours put in : 2
-----------------------------------------------------------------------------------
Wednesday, December 6, 2017 by Michael
- Completed the collision detection for projectiles (collision with stars and player object)
- Completed the score/heath updates
- Completed the respawn delay for ships
- Fixed issue where only one star being displayed (multiple stars set in settings)
- Fixed issues regarding race conditions
- Cleaned up code in all files (specific list below)
- * Added comments to undocumented code
- * Combined methods that were seperated and added helper methods for repeated operations
- * Simplified code and removed unused methods
- Fixed issues regarding client disconnects (keep sending dead ships of disconnected clients)
- Fixed issue reading settings file
- TODO: ADD EXTRA FEATURES AND COMPLETE UNIT TESTING OF THE MODEL

Total hours put in : 10+
-----------------------------------------------------------------------------------