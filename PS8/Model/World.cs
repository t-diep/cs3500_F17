///
/// @authors Tony Diep and Sona Torosyan
///
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Contains the World class
/// </summary>
namespace Model
{
    /// <summary>
    /// An encapsulation that consists of all of the different sprites in SpaceWars
    /// </summary>
    public class World
    {
        //Holds all of the ship sprites
        private Dictionary<int, Ship> theShips;

        //Holds all of the projectile sprites
        private Dictionary<int, Projectile> theProjectiles;

        //Holds all of the star sprites
        private Dictionary<int, Star> theStars;

        //Helps to update all existing projectiles to have the same velocities as each other
        private double projVelocity;

        //The scalar constant that is used to multiply by the thrust vector
        private double engineStrength;

        //Holds the configured ship size after reading settings file
        private int shipSize;

        //Holds the configured star size after reading settings file
        private int starSize;

        //Holds the configured world size after reading settings file
        private int worldSize;

        //Holds the configured number of ship frames after reading settings file
        private int numberOfShipFrames;

        //Holds the configured rotation angle for the ship to rotate
        private double rotationAngle;

        //Holds the configured respawn delay rate after reading settings file
        private int respawnDelay;

        //Holds the configured default HP after reading settings file
        private int defaultHP;

        //Flag to indicate whether we are on extra feature mode or not
        private bool onExtraFeatureMode;

        /// <summary>
        /// The default constructor that contains default stat values
        /// </summary>
        public World()
        {
            //Set up our storages for our different sprites
            theShips = new Dictionary<int, Ship>();
            theProjectiles = new Dictionary<int, Projectile>();
            theStars = new Dictionary<int, Star>();

            starSize = 35;
            shipSize = 20;
            defaultHP = 5;
            respawnDelay = 300;
            rotationAngle = 2;
            worldSize = 750;
            numberOfShipFrames = 0;
            engineStrength = .08;
            projVelocity = 15;
            onExtraFeatureMode = false;
        }

        /// <summary>
        /// Creates a new world based on the configured settings.xml file
        /// </summary>
        /// <param name="projVelocity">the velocity value for the projectile</param>
        /// <param name="engineStrength">the value used with thrusting power</param>
        /// <param name="shipSize">size of ship</param>
        /// <param name="starSize">size of star</param>
        /// <param name="worldSize">the square size of the world</param>
        /// <param name="numOfFrames">the number of frames for the ship</param>
        /// <param name="rotationAngle">the degrees of rotation for a ship to rotate</param>
        /// <param name="respawnDelay">the time delay it takes for a ship to respawn after dying</param>
        /// <param name="defaultHP">the default HP to set the ships to</param>
        /// <param name="isOnExtraFeatureMode"></param>
        public World(double projVelocity, double engineStrength, int shipSize, int starSize, int worldSize,
            int numOfFrames, double rotationAngle, int respawnDelay, int defaultHP, bool isOnExtraFeatureMode)
        {
            //Set up our storages for our different sprites
            theShips = new Dictionary<int, Ship>();
            theProjectiles = new Dictionary<int, Projectile>();
            theStars = new Dictionary<int, Star>();

            //Store configured values to this world
            this.projVelocity = projVelocity;
            this.engineStrength = engineStrength;
            this.shipSize = shipSize;
            this.starSize = starSize;
            this.worldSize = worldSize;
            this.numberOfShipFrames = numOfFrames;
            this.rotationAngle = rotationAngle;
            this.respawnDelay = respawnDelay;
            this.defaultHP = defaultHP;
            this.onExtraFeatureMode = isOnExtraFeatureMode;
        }

        /// <summary>
        /// Gets the list of ships and their ID's that exist in the current world
        /// </summary>
        /// <returns>the list of ships and their ID's</returns>
        public Dictionary<int, Ship> Ships()
        {
            return new Dictionary<int, Ship>(theShips);
        }
        /// <summary>
        /// Updates the ship in the world or adds it if it does not exist. 
        /// </summary>
        /// <param name="newShip"></param>
        public void UpdateShip(Ship newShip)
        {
            //ID exists but not actual ship, so add the actual ship to this world
            if (theShips.ContainsKey(newShip.ID()))
            {
                theShips[newShip.ID()] = newShip;
            }
            //Add the new ID and the actual ship to this world
            else
            {
                theShips.Add(newShip.ID(), newShip);

                
                //Apply the vector with randomly generated coordinates to the newly added ship
                newShip.UpdateLocation(GenerateRandomLocation());
            }
        }
        private Vector2D GenerateRandomLocation()
        {
            //Generate a random number generator for random coordinates
            Random randShipLoc = new Random();

            //Assume the new ship will overlap with some other existing ship in this world
            bool isOverlapping = true;

            //Create a vector that will have randomly generated location coordinates 
            Vector2D randShipLocVec = null;

            //As long as a ship is overlapping with either another ship or star...
            while (isOverlapping)
            {
                //
                isOverlapping = false;

                //Generate new random vector
                randShipLocVec = new Vector2D((double)randShipLoc.Next(-worldSize / 2 + 2 * shipSize, worldSize / 2 - 2 * shipSize), (double)randShipLoc.Next(-worldSize / 2 + 2 * shipSize, worldSize / 2 - 2 * shipSize));

                //Check current ship location if it's within or on another ship's location                
                foreach (Ship ship in theShips.Values)
                {
                    //Check location relative to other ships with our current random vector
                    if (Math.Abs(ship.Location().GetX() - randShipLocVec.GetX()) < 2 * shipSize
                        && Math.Abs(ship.Location().GetY() - randShipLocVec.GetY()) < 2 * shipSize)
                    {
                        isOverlapping = true;
                        break;
                    }
                }

                //Move on to another ship that may or may not be overlapping with another ship
                if (isOverlapping)
                {
                    continue;
                }

                //Check if current ship's location is within or on the star's location
                foreach (Star star in theStars.Values)
                {
                    if (Math.Abs(star.Location().GetX() - randShipLocVec.GetX()) < 3 * starSize
                        && Math.Abs(star.Location().GetY() - randShipLocVec.GetY()) < 3 * starSize)
                    {
                        isOverlapping = true;
                        break;
                    }
                }
            }
            return randShipLocVec;
        }
        /// <summary>
        /// Allows a particular ship to respawn after hit by a star or a projectile 
        /// </summary>
        /// <param name="shipID">the unique ID that maps to a particular ship</param>
        public void RespawnShip(int shipID)
        {
            theShips[shipID].UpdateHP(defaultHP);

            //the ships intial velocity should be 0
            theShips[shipID].UpdateVelocity(new Vector2D(0, 0));

           //Apply the vector with randomly generated coordinates to the respawned ship
            theShips[shipID].UpdateLocation(GenerateRandomLocation());

            //Ship number of frames set to zero
            theShips[shipID].ResetShipNumFrames();
        }

        /// <summary>
        /// Gets the list of projectiles and their ID's that exist in the current world
        /// </summary>
        /// <returns>the list of projectiles and their ID's</returns>
        public Dictionary<int, Projectile> Projectiles()
        {
            return new Dictionary<int, Projectile>(theProjectiles);
        }

        /// <summary>
        /// Updates the projectiles in the world or adds the newProj if it does not exist. 
        /// </summary>
        /// <param name="newProj"></param>
        public void UpdateProjectiles(Projectile newProj)
        {
            //Projectile ID already exists, so add the actual projectile that is associated with that ID
            while(theProjectiles.ContainsKey(newProj.ID()))
            {
                //Give another ID
                newProj.IncrementID();              
            }

            //Projectile doesn't already exist, so add both the ID and the actual projectile associated to the world           
            theProjectiles.Add(newProj.ID(), newProj);          
        }

        /// <summary>
        /// Removes projectile from the world
        /// </summary>
        /// <param name="deletingProj">the projectile to remove</param>
        public void RemoveProjectile(Projectile deletingProj)
        {
            theProjectiles.Remove(deletingProj.ID());
        }

        /// <summary>
        /// Makes a particular ship dead
        /// </summary>
        /// <param name="shipID">the unique ID that corresponds to a particular ship</param>
        public void KillShip(int shipID)
        {
            theShips[shipID].UpdateHP(0);
        }

        /// <summary>
        /// Gets the list of stars and their ID's that exist in the current world
        /// </summary>
        /// <returns>the list of stars and their ID's</returns>
        public Dictionary<int, Star> Stars()
        {
            return theStars;
        }

        /// <summary>
        /// Adds a star to the world
        /// </summary>
        /// <param name="star">the star to add to the dictionary</param>
        public void UpdateStars(Star star)
        {
            //Star ID already exists, so add the actual star object associated with this ID
            if (theStars.ContainsKey(star.ID()))
            {
                theStars[star.ID()] = star;
            }
            //Star ID doesn't already exist, so add the ID and the actual star object to the world
            else
            {
                theStars.Add(star.ID(), star);
            }
        }

        /// <summary>
        /// Enables a particular ship to thrust on command
        /// </summary>
        /// <param name="shipID">used to choose which ship to thrust</param>
        public void Thrust(int shipID)
        {
            Vector2D thrustVector = new Vector2D(theShips[shipID].Direction());

            thrustVector = thrustVector * engineStrength;

            //Acceleration changes as the ship thrusts
            theShips[shipID].UpdateAcceleration(thrustVector);

        }

        /// <summary>
        /// Generates a gravity vector and applies it to a particular ship
        /// </summary>
        /// <param name="shipID"></param>
        public void ApplyGravityToShip(int shipID)
        {
            //Have the ship start stationary first
            ResetToZeroAcceleration(shipID);

            foreach (Star star in Stars().Values)
            {
                //Apply gravity acceleration
                Vector2D gravity = star.Location() - theShips[shipID].Location();
                //Normalize gravity vector
                gravity.Normalize();
                //Apply the star mass scalar to the gravity vector
                gravity = gravity * star.Mass();
                //Apply gravity acceleration to current gravity vector
                ApplyGravityAcceleration(gravity, shipID);
            }
        }

        /// <summary>
        /// Applies gravity to a particular projectile
        /// </summary>
        /// <param name="projID">the unique ID corresponding to a particular projectile</param>
        public void ApplyGravityToProjectile(int projID)
        {
            //Have the ship start stationary first
            ResetToZeroAccelerationProj(projID);

            //Only apply gravity to projectile during extra feature mode
            if (onExtraFeatureMode)
            {
                foreach (Star star in Stars().Values)
                {

                    //Apply gravity acceleration
                    Vector2D gravity = star.Location() - theProjectiles[projID].Location();
                    //Normalize gravity vector
                    gravity.Normalize();
                    //Apply the star mass scalar to the gravity vector
                    gravity = gravity * star.Mass();
                    //Apply gravity acceleration to current gravity vector
                    ApplyGravityAccelerationToProjectile(gravity, projID);
                }
            }
        }

        /// <summary>
        /// Enables a particular ship to start firing on command
        /// </summary>
        /// <param name="shipID">used to choose which ship to thrust</param>
        public void Fire(int shipID)
        {
            //Ship is not dead yet, so now check for projectile frames
            if(!theShips[shipID].IsDead())
            {
                //No projectiles fired yet, so create a projectile as well as give it velocity
                if (theShips[shipID].NumberOfProjFrames() == 0)
                {
                    //Retrieve the ship's location vector
                    Vector2D loc = new Vector2D(theShips[shipID].Location());

                    //Match the projectile's direction vector to be the same as the ship's direction vector
                    Vector2D dirVelocity = new Vector2D(theShips[shipID].Direction());

                    //Compute velocity in terms of direction
                    dirVelocity = dirVelocity * projVelocity;

                    //Create the projectile with the calculated direction velocity vector
                    Projectile projectile = new Projectile(theProjectiles.Count, shipID, loc, theShips[shipID].Direction(), dirVelocity);

                    //The newly created projectile is now alive
                    projectile.NoLongerDead();

                    //Add it to this world
                    UpdateProjectiles(projectile);

                    //Update number of frames
                    theShips[shipID].IncrementProjNumFrames();
                }
                //The number of projectile frames matches the ship frames, so reset 
                else if (theShips[shipID].NumberOfProjFrames() == numberOfShipFrames)
                {
                    theShips[shipID].ResetProjNumFrames();
                }
                //Projectile frames neither 0 or the same as the ship frames
                else
                {
                    //Update number of frames
                    theShips[shipID].IncrementProjNumFrames();
                }
            }          
        }

        /// <summary>
        /// Computes the new location of a particular ship when moving around
        /// </summary>
        /// <param name="shipID">the unique ID corresponding to a ship we want to update the location of</param>
        public void UpdateShip(int shipID)
        {
            //Update velocity of ship
            theShips[shipID].UpdateVelocity(theShips[shipID].Acceleration() + theShips[shipID].Velocity());
            
            //Update position of ship
            theShips[shipID].UpdateLocation(theShips[shipID].Location() + theShips[shipID].Velocity());

           
            //Check for collisions with the star
            CollideWithStar();

            //Apply wrap around
            WrapAroundShipLocation(shipID);
        }

        /// <summary>
        /// Helps wrap a ship back to where it is still within the universe square
        /// </summary>
        /// <param name="shipID">the unique ID associated with a particular ship</param>
        private void WrapAroundShipLocation(int shipID)
        {
            //Retrieve ship's current location
            Vector2D location = theShips[shipID].Location();

            //X coordinate is past the square width rightwise, so wrap back to the left side of square
            if(location.GetX() > worldSize / 2)
            {
                location = new Vector2D(-worldSize / 2, location.GetY()); 
            }
            //X coordinate is past the square width leftwise, so wrap back to the right side of square
            else if(location.GetX() < -worldSize / 2)
            {
                location = new Vector2D(worldSize / 2, location.GetY());
            }

            //Y coordinate is past the square width rightwise, so wrap back to the left side of square
            if (location.GetY() > worldSize / 2)
            {
                location = new Vector2D(location.GetX(), -worldSize / 2);
            }
            //Y coordinate is past the square width leftwise, so wrap back to the right side of square
            else if (location.GetY() < -worldSize / 2)
            {
                location = new Vector2D(location.GetX(), worldSize / 2);
            }

            //Apply wrap around with new location vector to ship
            theShips[shipID].UpdateLocation(location);
        }

        /// <summary>
        /// Resets ship velocity to the default velocity; used when ship is no longer thrusting
        /// </summary>
        /// <param name="defaultVal">the default value recorded on the settings.xml file</param>
        public void ResetToShipDefaultVelocity(int shipID, double defaultVal)
        {
            theShips[shipID].UpdateVelocity(new Vector2D(defaultVal, defaultVal));
        }

        /// <summary>
        /// Updates the location of a particular projectile
        /// </summary>
        /// <param name="projID">the unique ID corresponding to a projectile we want to update its location</param>
        public void UpdateProjectileLocation(int projID)
        {
            
            //Apply gravity to the projectile if it is on extra game mode
            if (onExtraFeatureMode)
            {
                theProjectiles[projID].UpdateVelocity(theProjectiles[projID].Acceleration() + theProjectiles[projID].Velocity());
            }
            //Update the location based on the current velocity
            theProjectiles[projID].UpdateLocation(theProjectiles[projID].Location() + theProjectiles[projID].Velocity());

            //If it is too far from the center of the universe
            //remove from the dictionary
            if(theProjectiles[projID].Location().Length() > worldSize)
            {
                theProjectiles.Remove(projID);
            }
        }

        /// <summary>
        /// Resets a particular ship to have zero acceleration; this is the initial step
        /// prior to thrusting a ship
        /// </summary>
        /// <param name="shipID">the unique ID corresponding to a ship we want to reset its acceleration</param>
        public void ResetToZeroAcceleration(int shipID)
        {
            theShips[shipID].UpdateAcceleration(new Vector2D(0, 0));
        }

        /// <summary>
        /// Updates a particular ship's thrusting vector 
        /// </summary>
        /// <param name="shipID">the unique ID corresponding to a ship we want to update its thrusting vector</param>
        public void UpdateShipThrust(int shipID, bool newStatus)
        {
            theShips[shipID].UpdateThrust(newStatus);
        }

        /// <summary>
        /// Updates a particular ship's acceleration due to gravity
        /// </summary>
        /// <param name="gravity">the gravity vector</param>
        /// <param name="shipID">the unique ID corresponding to a ship we want to update its acceleration due to gravity</param>
        public void ApplyGravityAcceleration(Vector2D gravity, int shipID)
        {
            theShips[shipID].UpdateAcceleration(theShips[shipID].Acceleration() + gravity);
        }

        /// <summary>
        /// Updates a particular ship's acceleration due to gravity
        /// </summary>
        /// <param name="gravity">the gravity vector</param>
        /// <param name="shipID">the unique ID corresponding to a ship we want to update its acceleration due to gravity</param>
        public void ApplyGravityAccelerationToProjectile(Vector2D gravity, int projID)
        {
            if(onExtraFeatureMode)
            {
                theProjectiles[projID].UpdateAcceleration(theProjectiles[projID].Acceleration() + gravity);
            }
        }
        /// <summary>
        /// Allows a ship to turn left 
        /// </summary>
        /// <param name="shipID">the unique ID mapping to a particular ship</param>
        public void ApplyShipLeftRotation(int shipID)
        {
            theShips[shipID].Rotate(-rotationAngle);
        }

        /// <summary>
        /// Allows a ship to turn right
        /// </summary>
        /// <param name="shipID">the unique ID mapping to a particular ship</param>
        public void ApplyShipRightRotation(int shipID)
        {
            theShips[shipID].Rotate(rotationAngle);
        }

        /// <summary>
        /// Out of all of the ships and projectiles in this world, this method
        /// will check if one particular ship collides with a particular projectile 
        /// and applies collision effect if so
        /// </summary>
        public void CollideShip()
        {         
            //Iterate all ships in the world
            foreach (Ship ship in theShips.Values)
            {
                //Ship is already dead
                if (ship.IsDead())
                {
                    continue;
                }

                //Check for all projectiles that may have contacted with current ship
                foreach (Projectile projectile in theProjectiles.Values)
                {
                    

                    //Proj owner cannot match with the collided ship and alive, and must be within ship radius
                    if (projectile.Owner() != ship.ID() && projectile.Alive() && ProjectileCollidedWithShip(projectile, ship))
                    {
                        //Take down one hit point for the ship
                        ship.DecrementHP();
                        
                        //Projectile took HP of ship from 1 to 0 
                        if (ship.IsDead())
                        {
                            //Give one point to the ship responsible for the projectile
                            theShips[projectile.Owner()].UpdateScore();
                        }

                        //Projectile dead after hitting ship, so remove it from world
                        projectile.KillProjectile();
                    }
                }
            } 
            
        }

        /// <summary>
        /// Resets a particular projectile to zero acceleration
        /// </summary>
        /// <param name="projID">the ID that maps to a particular projectile</param>
        public void ResetToZeroAccelerationProj(int projID)
        {
            theProjectiles[projID].UpdateAcceleration(new Vector2D(0, 0));
        }

        /// <summary>
        /// Out of all of the ships in this world, this method checks if 
        /// any particular ship collides with the star in the middle, and applies
        /// the collision affect if so
        /// </summary>
        public void CollideWithStar()
        {           
            //Iterate through all ships in the world
            foreach (Ship ship in theShips.Values)
            {
                //Ship is already dead
                if (ship.IsDead())
                {
                    continue;
                }

                //Check for all projectiles that may have contacted with current ship
                foreach (Star star in theStars.Values)
                {
                    if (ShipCollidedWithStar(ship, star))
                    {
                        //Automatic KO for ship when it hits star
                        ship.UpdateHP(0);
                    }
                }
            }           
        }

        /// <summary>
        /// Determines whether any one ship collides within the star's radius
        /// </summary>
        /// <param name="ship">the ship to check</param>
        /// <param name="star">the star to check</param>
        /// <returns>true if ship overlaps within the star's radius and false otherwise</returns>
        private bool ShipCollidedWithStar(Ship ship, Star star)
        {
            return (ship.Location() - star.Location()).Length() <= starSize;
        }

        /// <summary>
        /// Determines whether any one projectile and any one ship collide with 
        /// each other among that ship's radius
        /// </summary>
        /// <returns>true if within ship's radius and false otherwise</returns>
        private bool ProjectileCollidedWithShip(Projectile proj, Ship ship)
        {
            return (proj.Location() - ship.Location()).Length() <= shipSize;
        }

        /// <summary>
        /// Determines whether any one ship and any one other ship collide with 
        /// each other among each other's ship radii
        /// </summary>
        /// <param name="firstShip">first ship to check</param>
        /// <param name="secondShip">second ship to check</param>
        /// <returns></returns>
        private bool ShipCollidedWithShip(Ship firstShip, Ship secondShip)
        {
            return (firstShip.Location() - secondShip.Location()).Length() <= shipSize;
        }

        /// <summary>
        /// Performs the calculations of ship to ship collision; if so, the two ships that 
        /// collided will both be KO'ed
        /// </summary>
        public void CollideShipToShip()
        {
            //Iterate all ships in the current world
            foreach (Ship firstShip in theShips.Values)
            {
                //Ship is already dead
                if (firstShip.IsDead())
                {
                    continue;
                }

                //Check for all projectiles that may have contacted with current ship
                foreach (Ship secondShip in theShips.Values)
                {
                    //The two ships must have unique ID's and must be alive, so continue
                    if(firstShip.ID() == secondShip.ID() || secondShip.IsDead())
                    {
                        continue;
                    }  

                    //Calculate distance between first and second ship for ship collision
                    if (ShipCollidedWithShip(firstShip, secondShip))
                    {
                        //Automatic death for both ships when it hits star
                        firstShip.UpdateHP(0);
                        secondShip.UpdateHP(0);
                    }
                }
            }
        }
    }
}