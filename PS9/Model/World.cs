using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace Model
{
    /// <summary>
    /// Class that stores all of the objects being used in the playing field using the object's ID as 
    /// dictionary keys
    /// </summary>
    public class World
    {
        /// <summary>
        /// ConcurrentDictionary to store the ships
        /// </summary>
        private ConcurrentDictionary<int, Ship> _shipDict;

        /// <summary>
        /// ConcurrentDictionary to store projectiles
        /// </summary>
        private ConcurrentDictionary<int, Projectile> _projDict;

        /// <summary>
        /// ConcurrentDictionary to store stars
        /// </summary>
        private ConcurrentDictionary<int, Star> _starDict;

        /// <summary>
        /// The height and width of the world
        /// </summary>
        private int _universeSize;

        /// <summary>
        /// The number of frames required before a Ship can
        /// respawn
        /// </summary>
        private int _respawnRate;

        /// <summary>
        /// The number of frames required between when a 
        /// Ship shoots a projectile (fire rate)
        /// </summary>
        private int _framesPerShot;

        /// <summary>
        /// Whether to enable the moving star
        /// extra feature
        /// </summary>
        private bool _movingStar;

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, Ship> _deadships;
        
        /// <summary>
        /// Default constructor for the World class
        /// </summary>
        public World()
        {

            _shipDict = new ConcurrentDictionary<int, Ship>();
            _projDict = new ConcurrentDictionary<int, Projectile>();
            _starDict = new ConcurrentDictionary<int, Star>();
            _deadships = new Dictionary<int, Ship>();
        }

        /// <summary>
        /// Constructor used to initialized a world given the four parameters
        /// listed below
        /// </summary>
        /// <param name="worldsize">Size of the world</param>
        /// <param name="stars">An IEnumerable containing all stars in the world</param>
        /// <param name="firerate">The desired firerate</param>
        /// <param name="respawnrate">The desired respawn rate</param>
        public World(int worldsize, IEnumerable<Star> stars, int firerate, int respawnrate, bool movestars) : this()
        {
            //Initialize the properties of the world
            this._universeSize = worldsize;
            this._respawnRate = respawnrate;
            this._framesPerShot = firerate;
            this._movingStar = movestars;

            //Add all of the stars (from the game settings) to the Star dictionary
            foreach(Star pStar in stars)
            {
                _starDict[pStar.GetID()] = pStar;
            }
        }

        /// <summary>
        /// Adds ship object to its dictionary
        /// </summary>
        /// <param name="shipId"></param>
        /// <param name="playerShip"></param>
        public void AddShip(int shipId, Ship playerShip)
        {
            if (playerShip != null)
                _shipDict[shipId] = playerShip;
        }
        
        /// <summary>
        /// Adds projectile object to its dictionary
        /// </summary>
        /// <param name="projId"></param>
        /// <param name="playerProj"></param>
        public void AddProj(int projId, Projectile playerProj)
        {
            if (playerProj != null)
                _projDict[projId] = playerProj;
        }

        /// <summary>
        /// Adds star object to its dictionary
        /// </summary>
        /// <param name="starId"></param>
        /// <param name="playerStar"></param>
        public void AddStar(int starId, Star playerStar)
        {
            if (playerStar != null)
                _starDict[starId] = playerStar;
        }

        /// <summary>
        /// Method to remove a ship given the ships's ID
        /// </summary>
        /// <param name="shipId">The ship's ID</param>
        public void RemoveShip(int shipId)
        {
            if (_shipDict.ContainsKey(shipId))
                _shipDict.TryRemove(shipId, out Ship s);
        }

        /// <summary>
        /// Method to remove a projectile given the projectile's ID
        /// </summary>
        /// <param name="projId">The projectile's ID</param>
        public void RemoveProj(int projId)
        {
            if (_projDict.ContainsKey(projId))
                _projDict.TryRemove(projId, out Projectile p);
        }

        /// <summary>
        /// Adds a new Ship in a random location
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int AddRandomShip(string name)
        {
            //Create the needed Vector2D objects for the Ship
            Vector2D velocity = new Vector2D(0.0, 0.0);
            Vector2D direction = new Vector2D(0.0, -1.0);
            Vector2D position = LocationRandom();

            //Create the Ship and add it to the dictionary
            Ship newShip = new Ship(name, position, velocity, direction);
            AddShip(newShip.GetID(), newShip);

            return newShip.GetID();
        }

        /// <summary>
        /// Kills a ship and doesn't allow for it
        /// to respawn
        /// </summary>
        /// <param name="playerID"></param>
        public void KillShipForever(int playerID)
        {
            //Set the Ship's to -1 to prevent it from satisfying respawn requirements
            //and set the last death counter to 5 to send the death data to clients
            if (_shipDict.ContainsKey(playerID))
            {
                _shipDict[playerID].SetHP(-1);
                _shipDict[playerID].SetLastDeath(5);
            }
        }

        /// <summary>
        /// Creates a Vector2D object in a random location in the world
        /// </summary>
        /// <returns></returns>
        private Vector2D LocationRandom()
        {
            Random rand = new Random();
            Vector2D randomLocation;
            bool flag;

            do
            {
                //Create a new random location
                randomLocation = new Vector2D(rand.NextDouble() * 2.0 - 1.0, rand.NextDouble() * 2.0 - 1.0) * (double)(this._universeSize / 2);
                flag = true;

                //Iterate through all of the stars in the world
                foreach (Star star in _starDict.Values)
                {
                    //If the random location is located inside a star, do not have it spawn there
                    if ((randomLocation - star.GetLocation()).Length() < 35.0)
                    {
                        flag = false;
                        break;
                    }
                }
            }
            //Keep looking for a random location that is not contained inside
            //of a star
            while (!flag);

            return randomLocation;
        }

        /// <summary>
        /// Moves a star object to a random location in the world
        /// </summary>
        /// <returns></returns>
        private void RandomStarLocation(Star pStar)
        {
            //Make sure that enough time has passed before moving
            //the star to a different location
            if (pStar.GetCounter() > 0)
            {
                pStar.DecrementCounter();
                return;
            }

            //Create a random location for the star
            Vector2D newStarLocation = LocationRandom();

            //Update the star's location to a random spot in the world
            pStar.SetLocation(newStarLocation);
            pStar.SetCounter(700);
        }

        /// <summary>
        /// Interprets a player's commands and updates the player's
        /// Ship accoordingly
        /// </summary>
        /// <param name="playerID"></param>
        /// <param name="cmd"></param>
        public void ProcessCommand(int playerID, string cmd)
        {
            //Check whether the dictionary contains the ship
            if (!_shipDict.ContainsKey(playerID))
                return;

            //See what command to use to update the player's ship
            if (cmd.Contains("F"))
                this.CreateProjectile(playerID, _shipDict[playerID]);

            if (cmd.Contains("T"))
                _shipDict[playerID].RequestThrust();

            if (cmd.Contains("L"))
                _shipDict[playerID].RequestLeftTurn();

            if (cmd.Contains("R"))
                _shipDict[playerID].RequestRightTurn();
        }

        /// <summary>
        /// Creates new projectile when a Ship fires
        /// </summary>
        /// <param name="playerID"></param>
        /// <param name="pShip"></param>
        private void CreateProjectile(int playerID, Ship pShip)
        {
            //Controls the fire rate of a player's ship or do not fire 
            //if the ship is dead
            if (pShip.GetLastFired() != 0 || pShip.GetHP() == 0)
                return;

            //Normalize the player's direction and multiply the direction vector by
            //6 to start the projectile 6 units in front of ship
            Vector2D playerDir = pShip.GetDirection();
            playerDir.Normalize();

            Vector2D projDir = playerDir * 6;

            //Add the direction and location together to find the projectile's start location
            Vector2D projLoc = projDir + pShip.GetLocation();
            Projectile newProj = new Projectile(playerID, projLoc, projDir);

            //If fired, reset the LastFiredTimer to the given fire rate in the settings
            _shipDict[playerID].SetLastFired(_framesPerShot);
            AddProj(newProj.GetID(), newProj);

            //Increment the ship's shot counter
            _shipDict[playerID].IncrementTotalShots();
        }

        /// <summary>
        /// Helper method used to update the location of projectiles
        /// </summary>
        /// <param name="proj"></param>
        private void UpdateProjectileValues(Projectile proj)
        {
            //Update the location of the projectile
            Vector2D newDir = proj.GetDirection();
            newDir.Normalize();
            newDir = newDir * 15d;
            proj.SetLocation(newDir + proj.GetLocation());

            //Check for collisions and update scores
            CheckProjectileCollision(proj);
        }

        /// <summary>
        /// A helper method which checks whether a projectile has collided
        /// with an object
        /// </summary>
        /// <param name="proj"></param>
        private void CheckProjectileCollision(Projectile proj)
        {
            //Check if the projectile is still alive
            if (proj.GetAlive())
            {
                //Calculate the world's edges plus an offset to make sure projectiles are
                //completely out of sight before being removed
                int half_space = (_universeSize / 2) + 10;

                //Check if the projectile is still within the play area
                if (proj.GetLocation().GetX() > half_space || proj.GetLocation().GetX() < -half_space)
                    proj.Death();
                if (proj.GetLocation().GetY() > half_space || proj.GetLocation().GetY() < -half_space)
                    proj.Death();
                
                
                //Check for collisions with stars
                foreach (Star pStar in _starDict.Values)
                {
                    if (((proj.GetLocation() - pStar.GetLocation()).Length() < 35.0))
                        proj.Death();
                }

                //Check if the projectile has collided with a Ship
                foreach (Ship pShip in _shipDict.Values)
                {
                    //If the projectile belongs to the ship, skip it
                    if (pShip.GetID() == proj.GetOwner())
                        continue;

                    //Check if the projectile has collided with the ship
                    if (((proj.GetLocation() - pShip.GetLocation()).Length() < 20.0) && pShip.GetHP() > 0)
                    {
                        //Perform the necessary operations when a projectile collides with a ship
                        pShip.DecrementHP();
                        _shipDict[proj.GetOwner()].IncrementHits();

                        //Check if the ship has been killed by the projectile
                        if (pShip.GetHP() == 0)
                        {
                            _shipDict[proj.GetOwner()].IncrementScore();
                            pShip.SetLastDeath(_respawnRate);
                        }
                        proj.Death();
                    }
                }
            }
            //If projectile is not alive, remove it from the dictionary
            else
            {
                RemoveProj(proj.GetID());
            }
        }

        /// <summary>
        /// Helper method used to update the location of ships and whether they
        /// collide with a Star
        /// </summary>
        /// <param name="pShip"></param>
        private void UpdateShipValues(Ship pShip)
        {
            Vector2D vectorThrust = new Vector2D(0.0, 0.0);

            //Check if the player has the thrust engaged
            if (pShip.GetThrust())
                vectorThrust = pShip.GetDirection() * 0.08;
            
            //Iterate through all stars for collision detection and 
            //updating a Ship's location according to the pull of a star's gravity
            foreach (Star star in _starDict.Values)
            {
                //If the extra game mode is enabled, see if we need
                //to move the star
                if (_movingStar)
                    RandomStarLocation(star);

                Vector2D vectorLocation = star.GetLocation() - pShip.GetLocation();

                //If the Ship collides with a Star, set the player's HP to zero
                if (vectorLocation.Length() < 35.0 && pShip.GetHP() > 0)
                {
                    pShip.SetHP(0);
                    pShip.SetLastDeath(_respawnRate);
                    return;
                }
                //Update the thrust vector given the mass of the sun
                double starMass = star.GetMass();
                vectorLocation.Normalize();
                vectorThrust += (vectorLocation * starMass);
            }
            //Update the Ship's location and velocity
            pShip.SetVelocity(pShip.GetVelocity() + vectorThrust);
            pShip.SetLocation(pShip.GetLocation() + pShip.GetVelocity());

            int half_space = _universeSize / 2;

            //Check whether the Ship is outside of the game area. If so, wrap the ship around
            if (pShip.GetLocation().GetX() > half_space || pShip.GetLocation().GetX() < -half_space)
                pShip.WrapAroundX();

            if (pShip.GetLocation().GetY() > half_space || pShip.GetLocation().GetY() < -half_space)
                pShip.WrapAroundY();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Ship> GetDeadShips()
        {
            return new Dictionary<int, Ship>(this._deadships);
        }

        /// <summary>
        /// Updates the whole world
        /// </summary>
        public void Refresh()
        {
            foreach (int k in _shipDict.Keys)
            {
                if (_shipDict[k].GetHP() == -1 && _shipDict[k].GetLastDeath() == 0)
                    _deadships.Add(k, _shipDict[k]);

            }
            
            //Iterate through all ships
            foreach (Ship ship in _shipDict.Values)
            {
                //Each frame, decrement the counters
                ship.DecrementLastFired();
                ship.DecrementLastDeath();

                //If the ship has been killed permanently, remove it from storage
                if (ship.GetHP() == -1 && ship.GetLastDeath() == 0)
                    RemoveShip(ship.GetID());
                    

                //Check if a dead ship can respawn, otherwise update the ships values
                else if (ship.GetHP() == 0 && ship.GetLastDeath() == 0)
                    ship.Respawn(LocationRandom());
                else
                {
                    ship.SetControls();
                    UpdateShipValues(ship);
                }
            }

            //Iterate through all projectiles to update loctions and check for collisions
            foreach (Projectile projectile in _projDict.Values)
                UpdateProjectileValues(projectile);
        }

        /// <summary>
        /// Retreives all active ships stored in its dictionary
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Ship> GetAllShips()
        {
            return new List<Ship>(_shipDict.Values);
        }

        /// <summary>
        /// Retreives all active projectiles stored in its dictionary
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Projectile> GetAllProjectiles()
        {
            return _projDict.Values;
        }

        /// <summary>
        /// Retreives all active stars stored in its dictionary
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Star> GetAllStars()
        {
            return _starDict.Values;
        }
    }
}
