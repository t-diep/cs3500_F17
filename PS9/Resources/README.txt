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

While reading the XML settings file, if a star is missing one of its attributes (X, Y, Mass) the 
missing attribute will be initialized to the default value for that particular attribute.

EXTRA FEATURE:
If "MovingStars" is enabled in the settings, all stars will move to a random location every
700 frames.

TESTING:
All of model where not tested since most of them were private helper methods and were straight forward.
They were private methods and it would have been redundant testing them.

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


--------------Database Description-----------------------------------------------

-There are two tables created:

1) PlayersByGame
-Contains the following columns:
    .GameID - the ID that will be tied to the unique ID of a game session
	.Player - holds the name of the players in the game
	.Score - holds the score for each of the players
	.Accuracy - holds the percentage that they hit opponents accurately (the numbers are integers that are implied to be a percentage)
	.Duration - the time for each of the players that remained in the game before disconnecting

2) GameDurations
-Contains the following columns:
    .GameID - the ID associated to a single game session
	.Duration - the total time of the entire game before server is stopped

We decided to have two GameID columns for both tables to correlate the duration times whenever the user wants to look up
the duration time of a single game session as well as the duration times for each of the individual players.

DESIGN
-We created a separate static class called DatabaseHandler.  This will take care of MySQL connections and commands to the tables already
made in the MySQL workbench. The static class contains static methods that are for general purpose, and is used to get current world
data and insert them to the tables.  

-There are a couple of methods we added to the SpaceWarsServer class to add in http protocol listeners.  We added an extra parameter to 
the "ServerAwaitingClientLoop" method to take in any specific port (whether being the default port or the 80 port for http protocol).
We then also created methods UploadServer (which invokes a delegate called ProcessHTTPRequest) that asks for http protocols.  
When ProcessHTTPRequest is invoked, it sends messages that are displayed onto the browser depending on whether a http protocol command
is recognized as valid or invalid.  

Example SQL Code The Server Performs

1) Getting all data with no restriction:
"select GameDurations.GameID, PlayersByGame.Player, PlayersByGame.Score, PlayersByGame.Accuracy, PlayersByGame.Duration from PlayersByGame join GameDurations order by Score desc;"

2) Getting data based on specific game ID:
"select GameDurations.GameID, PlayersByGame.Player, PlayersByGame.Score, PlayersByGame.Accuracy, PlayersByGame.Duration from GameDurations, PlayersByGame where GameDurations.GameID = " + id + " order by Score desc;"

3) Getting data based on specific player name:
"select PlayersByGame.Player, GameDurations.GameID, PlayersByGame.Score, PlayersByGame.Accuracy, PlayersByGame.Duration from PlayersByGame join GameDurations where PlayersByGame.Player =\'" + player+ "\'"

4) Inserting Data to "PlayersByGame" table:
"insert into PlayersByGame (Player, Score, Accuracy, Duration) values (\"" + ship.GetName() + "\", " + ship.GetScore() + ", " + num2 + ", " + durationSeconds.ToString() + ");"
NOTE that gameID is not inserted here because it is configured to be auto-incremented

5) Inserting Data to "GameDurations" table:
"insert into GameDurations (Duration) values (" + durationSeconds.ToString() + ");"
NOTE that gameID is not inserted here because it is configured to be auto-incremented

WARNINGS
-Everything seems to work except that the table will sometimes show multiple rows of the same player's data (i.e. the table
may print a row for one player's data multiple times).  This is likely because we decided to use a list of lists to append row 
data for each player.  Because of short time, we couldn't really fix this bug.  Nonetheless, again, the data is displayed correctly.

