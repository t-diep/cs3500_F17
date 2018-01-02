///
///@authors Tony Diep and Sona Torosyan
///
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

/// <summary>
/// Contains the ship class
/// </summary>
namespace Model
{
    /// <summary>
    /// Represents the ship sprite
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Ship
    {
        //This ship's ID number
        [JsonProperty(PropertyName = "ship")]
        private int ship;

        //Location coordinates
        [JsonProperty]
        private Vector2D loc;

        //Direction coordinates
        [JsonProperty]
        private Vector2D dir;

        //State of the ship thrusting or not
        [JsonProperty]
        private bool thrust;

        //This ship's name (player name)
        [JsonProperty]
        private String name;
            
        //Ship's hit points
        [JsonProperty]
        private int hp;
        
        //Ship's score
        [JsonProperty]      
        private int score;

        //Velocity vector
        private Vector2D velocity;

        //Acceleration vector
        private Vector2D acceleration;

        //Keeps track of the number of frames of this ship's projectile
        private int projNumFrames;

        //Keeps track of the number of frames of this ship
        private int shipNumFrames;


        /// <summary>
        /// Creates a new ship (a.k.a a new player)
        /// </summary>
        /// <param name="name">name of ship</param>
        /// <param name="ship">ID number</param>
        /// <param name="hp">ship's hit points</param>
        public Ship(String name, int ship, int hp)
        {
            //Set up vectors
            loc = new Vector2D(0, 0);
            dir = new Vector2D(0, 0);
            velocity = new Vector2D(0, 0);
            acceleration = new Vector2D(0, 0);

            //Save name, ID, and HP information to this ship
            this.name = name;
            this.ship = ship;
            this.hp = hp;

            //Ship is by default stationary
            thrust = false;
        }

        /// <summary>
        /// Creates a new ship with a given name of player and their ID
        /// </summary>
        /// <param name="ship"></param>
        public Ship(String name, int ship)
        {
            //Set ID of this ship
            this.ship = ship;

            //Set name of this ship
            this.name = name;

            //Set location vector for this ship
            loc = new Vector2D();

            //Set default velocity vector to be 0
            velocity = new Vector2D(0, 0);

            //Set default acceleration to be 0
            acceleration = new Vector2D(0, 0);

            //Generate random orientation coordinates for this ship
            Random randOrient = new Random();
            double rangeX = randOrient.NextDouble() * (1 - (-1)) + (-1);
            double rangeY = randOrient.NextDouble() * (1 - (-1)) + (-1);

            //Set up an orientation vector containing the randomly generated coordinates
            dir = new Vector2D(rangeX, rangeY);
            
            //Normalize the direction vector
            dir.Normalize();

            //Ship is by default not currently thrusting
            thrust = false;

            //Stationary, so no acceleration at the moment
            acceleration = new Vector2D(0, 0);

            //Set starting HP to 5
            this.hp = 5;
        }


        /// <summary>
        /// Reports whether the ship is thrusting (moving) or not
        /// </summary>
        /// <returns>true if thrusting and false otherwise</returns>
        public bool IsThrusting()
        {
            return thrust;
        }

        /// <summary>
        /// Updates the current thrust state of this ship, whether it thrusted or not
        /// </summary>
        /// <param name="newStatus">true if the ship thrusted; false if the ship no longer thrusted</param>
        public void UpdateThrust(bool newStatus)
        {
            thrust = newStatus;
        }

        /// <summary>
        /// Increments the score of this ship's score when the ship's 
        /// projectile comes in contact with another ship
        /// </summary>
        public void UpdateScore()
        {
            score++;
        }

        /// <summary>
        /// Gets the ID of this ship
        /// </summary>
        /// <returns>this ship's ID</returns>
        public int ID()
        {
            return ship;
        }

        /// <summary>
        /// Gets the name of this ship
        /// </summary>
        /// <returns>the ship's name</returns>
        public String Name()
        {
            return name;
        }

        /// <summary>
        /// Gets the current location of this ship
        /// </summary>
        /// <returns>the current ship's location </returns>
        public Vector2D Location()
        {
            return loc;
        }

        /// <summary>
        /// Updates location coordinates of ship when moving around
        /// </summary>
        /// <param name="newLocation">the new location vector to update to this ship</param>
        public void UpdateLocation(Vector2D newLocation)
        {
            loc = newLocation;
        }

        /// <summary>
        /// Gets the current direction as a vector component of this ship
        /// </summary>
        /// <returns>current direction of this ship</returns>
        public Vector2D Direction()
        {
            return dir;
        }

        /// <summary>
        /// Changes the direction of this ship 
        /// </summary>
        /// <param name="newDirVector">new vector to change direction</param>
        public void ChangeDirection(Vector2D newDirVector)
        {
            dir = newDirVector;
        }

        /// <summary>
        /// Gets the velocity vector of this ship
        /// </summary>
        /// <returns>the velocity as a vector</returns>
        public Vector2D Velocity()
        {
            return velocity;
        }

        /// <summary>
        /// Gets the number of frames in respect to this ship
        /// </summary>
        /// <returns></returns>
        public int ShipNumberOfFrames()
        {
            return shipNumFrames;
        }

        /// <summary>
        /// Resets the number of ship frames to zero
        /// </summary>
        public void ResetShipNumFrames()
        {
            shipNumFrames = 0;
        }

        /// <summary>
        /// Increments the number of ship frames
        /// </summary>
        public void IncrementShipFrames()
        {
            shipNumFrames++;
        }

        /// <summary>
        /// Gets the acceleration vector of this ship
        /// </summary>
        /// <returns>the acceleration vector</returns>
        public Vector2D Acceleration()
        {
            return acceleration;
        }

        /// <summary>
        /// Updates the acceleration of this ship to help enable the ship to move
        /// </summary>
        /// <param name="newAccelVec">the vector to update as the new accerlation vector for this ship</param>
        public void UpdateAcceleration(Vector2D newAccelVec)
        {
            acceleration = newAccelVec;
        }

        /// <summary>
        /// Updates the velocity of this ship to help enable the ship to move
        /// </summary>
        /// <param name="newVector">the vector to update as the new velocity vector for this ship</param>
        public void UpdateVelocity(Vector2D newVector)
        {
            velocity = newVector;
        }

        /// <summary>
        /// Gets the number of frames of this ship
        /// </summary>
        /// <returns>number of frames</returns>
        public int NumberOfProjFrames()
        {
            return projNumFrames;
        }

        /// <summary>
        /// Updates the number of frames counter
        /// </summary>
        public void IncrementProjNumFrames()
        {
            projNumFrames++;
        }

        /// <summary>
        /// Resets the number of frames to zero 
        /// </summary>
        public void ResetProjNumFrames()
        {
            projNumFrames = 0;
        }

        /// <summary>
        /// Gets the current HP of this ship
        /// </summary>
        /// <returns>current HP of this ship</returns>
        public int HP()
        {
            return hp;
        }

        /// <summary>
        /// Updates the current HP for this ship
        /// </summary>
        /// <param name="newHP">the new ship's HP</param>
        public void UpdateHP(int newHP)
        {
            hp = newHP;
        }

        /// <summary>
        /// Helps decrease the hp when a projectile hits this ship
        /// </summary>
        public void DecrementHP()
        {
            hp--;
        }

        /// <summary>
        /// Allows the ship to rotate at a given angle (in degrees)
        /// </summary>
        /// <param name="degree"></param>
        public void Rotate(double degree)
        {
            dir.Normalize();
            dir.Rotate(degree);
        }

        /// <summary>
        /// Gets the current score of this ship
        /// </summary>
        /// <returns>the current score of this ship</returns>
        public int Score()
        {
            return score;
        }

        /// <summary>
        /// Updates score of this ship to a new score
        /// </summary>
        /// <param name="newScore">the new score to override current score</param>
        public void SetScore(int newScore)
        {
            score = newScore;
        }

        /// <summary>
        /// Reports whether the ship has no more HP left
        /// </summary>
        /// <returns>true if no hp left and false otherwise</returns>
        public bool IsDead()
        {
            return hp == 0;
        }
    }
}
