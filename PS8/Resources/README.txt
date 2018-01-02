README

PS7

~DESIGN DECISIONS~


DRAWING PANEL
-In the DrawingPanel class, we decided to create lots of member fields
pertaining to sprite images.  Then we initialize them in the DrawingPanel constructor.
In addition to this, we decided to create Rectangle member fields to give offset 
coordinates for the sprites Ship, Star, and Projectile.  Then on each of the sprite
drawer methods, we simply called e.Graphics.DrawImage() where we passed in the 
image objects and the rectangle objects.  This would theoretically make the 
drawing frames less laggy and more smooth.  

SENDING MESSAGES TO SERVER
-When sending command messages to server, we created four bool flags that represented
each valid key input (that is in the case, the left, right, and up arrow keys, and spacebar key).
We first validated these keys to be valid input on the PreviewKeyDown method, then updated
flags to true if the keys are pressed in the PreviewKeyDown method, and updated the flag to 
false if the keys are not pressed at the time in the PreviewKeyUp method.  This way, 
the client will be better at sending messages.  Plus, it is simple to implement.

PROJECT DESIGNS
-We have created four different projects: Model, NetworkingController, Resources, and View.

Model - Contains Ship, Projectile, Star, World, and Vector2D
     .In each of these classes (excluding Vector2D since it's already done for us), 
	 we constructed getters and setters for almost all of the member variables to 
	 follow data protection rule.
	 .Each class also has some extra constructors (such as Projectile, Star, and Ship) that
	 are not used.  We are going to leave them in case they are needed in PS8, even if we 
	 found a way to create objects without using these constructors
	 .In the World class, the method called "Ships" contains a lock.  This is because
	 there was an exception.

NetworkingController - Contains two classes:  one static class Networking and the non-static class
SocketState

Resources - Empty project containing the Images folder, holding the images for the sprites

View - Contains the DrawingPanel in which to draw SpaceWars game and the Form + Designer classes
to construct the form

LOTS OF HELPER METHODS
-We have created lots of helper methods in our form.  It is a lot of code to read, so
just be careful.

~EXTRA POLISHES~

WATERMARK
-We added a watermark feature to the player name.  The watermark text will initially show
"Enter Player Name."  When clicking inside the player name text box, and the user enters text,
the watermark text will disappear.  

SCOREBOARD PANEL
-Our scoringpanel is a flow layout panel.  We chose this over the regular standard
Panel in order to allow the controls contained in the flow-layout panel to show when
showing the flow-layout panel after hitting the "Connect" button.  
    
-Each existing ship (player) in the game contains two controls that are displayed on
the scoringboard panel:  the player name label and the progress bar control.  Although
we did use the progress bar to display the HP of players, we are unable to change the
default color (which is green) according to the MSDN.  In addition to the player name
text label, we set the BackColor of the player name label to black.  This is because
we also changed the color of the player name label text font to the same color respective
to the color of the ship and its projectiles.  By providing black BackColor, it gives 
a good look on the player name labels.  

TEXT LABELS FOR SERVER AND PLAYER NAME
-We added text labels that clearly indicate what each text box on the top of the form
represent.  The first text box has a default name as "localhost" which is the server
name text box with the text label "Server Name".  Similarly, there is another text box
next to the server text box, which is the Player name text box.  


~PS7 Log~ 

11/7/17
-Setup and implement the different projects in relation to "separation of concerns"

~Empty Project
    .README
    .Folder containing all of the necessary .dlls for this solution
~Controller Project
	.Focuses on data and networking logic 
~View Project
	.Displays the game screen with the different sprites shown, containing Windows Form GUI
~Model Project
	.Constructs the blueprints of each sprite in the game
	.For easiness, we decided to create multiple different classes in one project instead of
	having separate projects for each sprite
	.Also includes the provided Vector2D class

-Had to delete old solution and create a new solution since we kept mixing up our namespace names
(we tried making all namespaces for each project named "SpaceWars", but that made things a lot 
complicated than necessary...)

11/8/17
-Created the SpaceWars GUI 
	.Added server name text box, player name box, and connect button
	.Still gotta add in the drawing panel
-Added implementation for Networking
-Added getters/setters for each of the different sprite classes
-Added XML comments 

11/9/17
-Tweaked some networking code; networking code working

11/13/17
-Implemented and finished drawing the ship and star; still working on drawing projectile
-Started implementing keyboarding event handlers; part of the thrusting is working
-Still working on multiple command recognition... we'll consult Kopta's office hours for assistance

11/14/17
-Solved multiple command issues and displayed the scoreboard
-Had some deadlock issues in our code
-Realized we tried fixing this and took too long to solve so we didn't solve it and
we spent too much time on it, so we decided to revert back to working code before
fixing the deadlock code... and we had trouble with it...

11/15/17
-Created a separate solution in master branch called PS7

11/16/17
-Clean up code and finish README

11/17/17
-I finally moved all changes made on a separate branch to this solution in the "master" branch.  After so many unnecessary
hours of trying to revert changes, we got that issue fixed.  

11/19/17
-Program was in a deadlock, so I looked into it and realized there was an extra lock in a method that was called
in another method that contained a lock.  So I deleted the inner lock, and fixed it.  Thank god for the extension!

PS8 

~DESIGN DECISIONS~

SERVER CLASS
-Our server will have contain lots of member variables.  This is because the server controls all of the 
game mechanics for the existing clients.
-We decided to make Server class a non-static class as opposed to static.  We wanted to be able to use
member variables to allow information accessing from different classes.  Part of it is also because 
we work very well with this design.


SETTINGS.XML
-We decided to use the provided settings.xml file included in the NewSpaceWarsLibrary zip folder to
avoid rewriting it ourselves from scratch.  This is to get a starting point when writing our own 
settings.xml file, especially when we want to allow more configurations for the user to play around with.
-The settings.xml file is attached under the Resources project.  This way, you can easily configure
the settings contained in that file.  Just enter in the new values you want on that file, then 
rebuild the solution and execute the server again, and those new settings will apply to the 
SpaceWars game.

UNIT TESTING STRATEGIES
-For the Model class, I started testing basic methods that get information regarding the different
models.  The testing in the Ship, Projectile, Star, and Vector2D classes don't have a lot of variety
in terms of unit testing, so those can simply be unit tested just by checking whether the vector
math is correct as well as information passed onto the constructors of each sprite is correct.  
-For the World class in particular, I unit tested each of the different "updating" methods (such as
CollideWithShip and CollideWithStar) by checking if the vector of the ship's original location is different
than the vector that left the starting point of that same ship.  This is because ship's locations are 
randomly generated, so it is harder to get the vector with the exact x and y coordinates when entering the 
game. 


~EXTRA FEATURES~

PROJECTILE AFFECTED BY GRAVITY
-We have applied gravity to the projectiles, similar to the gravity applied to the ships!  However, this is really noticeable ONLY
when the initial velocity of projectile (which you can configure on the settings.xml) is set to 0.  Otherwise, the velocity will be 
too fast for gravity to take in effect, and thus will make this extra feature unnoticeable.  

SHIP TO SHIP COLLISION
-We have also added ship collisions (that is, when one ship collides with another ship, both of the ships will be dead).  However, 
this will only apply as long as the two ships that are about to collide DO NOT contain the same ID as each other.  When turning on this mode,
it will activate just as how you would play the game without the extra game feature.  

SPACING BETWEEN SHIPS
-We designed the ships such that their locations are spaces away from any other ships just to give
each ships some distance.  It kinda looks weird just having ships being created where they are 
sometimes close to other ships.

~PS8 Log~

11/22/17
-Began implementing and fixing networking code

11/27/17
-Got star and ships drawing; got ship to draw at a rotated angle while moving at constant velocity
-Finished implementing comments on methods and member variables

11/28/17
-Got ship to move at constant velocity 
-Got projectile to show per frame
-Got ship to thrust and not thrust based on command, but it is flickery

11/30/17
-Got flickering thrusting of ship fixed!

12/1/17
-Implemented collisions and projectile cleanup

12/2/17
-Implemented a unit tests project that tests for the different models in the model project

12/4/17
-Got rotations and projectile clean up to work!
-Got unit test to yield 97% code coverage

12/5/17
-Need to fix thrust bug
-Updated Extra Features section of this README
-Created more unit tests

12/6/17
-Cleaned up code and updated README.  
-Fixed thrusting :D
-Pushed final commit