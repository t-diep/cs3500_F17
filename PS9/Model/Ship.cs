using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Model
{
    /// <summary>
    /// This class represents a player-controlled ship in the SpaceWars game
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Ship
    {
        /// <summary>
        /// The ship's ID
        /// </summary>
        [JsonProperty(PropertyName = "ship")]
        private int _ID;

        /// <summary>
        /// The user-specified name that represents the ship
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        private string _name;

        /// <summary>
        /// Whether the ship's thrusters are engaged
        /// </summary>
        [JsonProperty(PropertyName = "thrust")]
        private bool _thrust;

        /// <summary>
        /// The current HP of the ship
        /// </summary>
        [JsonProperty(PropertyName = "hp")]
        private int _hp;

        /// <summary>
        /// The player's current score
        /// </summary>
        [JsonProperty(PropertyName = "score")]
        private int _score;

        /// <summary>
        /// The location of the ship represented in a Vector2D object
        /// </summary>
        [JsonProperty(PropertyName = "loc")]
        private Vector2D _location;

        /// <summary>
        /// The direction of the ship represented in a Vector2D object
        /// </summary>
        [JsonProperty(PropertyName = "dir")]
        private Vector2D _direction;
        
        /// <summary>
        /// The velocity of the ship represented in a Vector2D object
        /// </summary>
        private Vector2D _velocity;

        /// <summary>
        /// A count of frames since a ship last fired a projectile.
        /// Used to control the firing rate
        /// </summary>
        private int _lastFired;

        /// <summary>
        /// A count of frames since a ship last died.
        /// Used to control the respawn rate
        /// </summary>
        private int _lastDeath;

        /// <summary>
        /// The number of times a player's projectile
        /// hits an opponent
        /// </summary>
        private int _playerHits;

        /// <summary>
        /// The total number of times a player fires
        /// a projectile
        /// </summary>
        private int _totalShots;
        
        /// <summary>
        /// Monitors which ID is for a given Ship.
        /// The ID is incremented every time a Ship
        /// is created as it is a static class member
        /// </summary>
        private static int _nextShipID;

        /// <summary>
        /// A struct used to control requests that are
        /// received to change a Ship's thrusters and 
        /// orientation
        /// </summary>
        private CommandsRequest controlrequests;


        /// <summary>
        /// Default constructor for JSON serialization
        /// </summary>
        public Ship()
        {
            _ID = 0;
            _name = "";
            _thrust = false;
            _hp = 1;
            _score = 0;
            _lastFired = 0;
            _playerHits = 0;
            _totalShots = 0;
            _location = new Vector2D();
            _direction = new Vector2D();
        }

        /// <summary>
        /// Constructor of ship with set properties.
        /// </summary>
        public Ship(string name, Vector2D location, Vector2D velocity, Vector2D direction)
        {
            _ID = _nextShipID++;
            _name = name;
            _location = new Vector2D(location);
            _velocity = new Vector2D(velocity);
            _direction = new Vector2D(direction);
             controlrequests.Clear();
            _hp = 5;
            _score = 0;
            _playerHits = 0;
            _totalShots = 0;
        }

        /// <summary>
        /// Constructor for creating a Ship given a name and ID
        /// </summary>
        public Ship(string playerName, int id)
        {
            _ID = id;
            _name = playerName;
            _thrust = false;
            _hp = 5;
            _score = 0;
            _playerHits = 0;
            _totalShots = 0;
            _location = new Vector2D();
            _direction = new Vector2D();
        }

        /// <summary>
        /// A struct used to wrap requests to update the Ship
        /// </summary>
        private struct CommandsRequest
        {
            /// <summary>
            /// The request to rotate the Ship in a certain
            /// direction
            /// </summary>
            private int _turning;

            /// <summary>
            /// The request to enable thrust
            /// </summary>
            private bool _thrusting;

            /// <summary>
            /// Getter and setter for turning request
            /// </summary>
            public int requestTurning
            {
                get
                {
                    return this._turning;
                }
                set
                {
                    //Make sure the passed in values are valid
                    if (value != 4 && value != 2)
                    {
                        this._turning = 0;
                        return;
                    }

                    this._turning = value;
                }
            }

            /// <summary>
            /// Getter and setter for thrusting
            /// </summary>
            public bool requestThrusting
            {
                get
                {
                    return this._thrusting;
                }
                set
                {
                    this._thrusting = value;
                }
            }

            /// <summary>
            /// Helper method to clear all command requests
            /// </summary>
            public void Clear()
            {
                this.requestTurning = 0;
                this.requestThrusting = false;
            }
        }


        //************* Methods outside of the CommandRequests struct *************//


        /// <summary>
        /// Requests a left turn into the CommandRequests struct
        /// </summary>
        public void RequestLeftTurn()
        {
            controlrequests.requestTurning = 4;
        }

        /// <summary>
        /// Requests a right turn into the CommandRequests struct
        /// </summary>
        public void RequestRightTurn()
        {
            controlrequests.requestTurning = 2;
        }

        /// <summary>
        /// Requests to enable thrust into the CommandRequests struct
        /// </summary>
        public void RequestThrust()
        {
            controlrequests.requestThrusting = true;
        }

        /// <summary>
        /// Updates the Ship according to the command controls
        /// </summary>
        public void SetControls()
        {
            if (this.controlrequests.requestTurning == 4)
                this._direction.Rotate(-2.0);

            if (this.controlrequests.requestTurning == 2)
                this._direction.Rotate(2.0);

            if (this.controlrequests.requestThrusting)
            {
                this._thrust = true;
            }
            else
                this._thrust = false;

            //After updating the Ship, clear all previous requests
            this.controlrequests.Clear();
        }

        /// <summary>
        /// Retreive the Ship's ID
        /// </summary>
        /// <returns></returns>
        public int GetID()
        {
            return _ID;
        }

        /// <summary>
        /// Retreive the Ship's assigned name
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return _name;
        }

        /// <summary>
        /// Return whether the Ship's thrust is engaged
        /// </summary>
        /// <returns></returns>
        public bool GetThrust()
        {
            return _thrust;
        }

        /// <summary>
        /// Retreive the Ship's hp
        /// </summary>
        /// <returns></returns>
        public int GetHP()
        {
            return _hp;
        }

        /// <summary>
        /// Retreive the Ship's score
        /// </summary>
        /// <returns></returns>
        public int GetScore()
        {
            return _score;
        }

        /// <summary>
        /// Retreive the Ship's total number
        /// of shots fired
        /// </summary>
        /// <returns></returns>
        public int GetTotalShots()
        {
            return _totalShots;
        }

        /// <summary>
        /// Retreive the Ship's total number
        /// of shots that hit an opponent
        /// </summary>
        /// <returns></returns>
        public int GetHits()
        {
            return _playerHits;
        }

        /// <summary>
        /// Retreive the Vector2D object representing the Ship's location
        /// </summary>
        /// <returns></returns>
        public Vector2D GetLocation()
        {
            return _location;
        }

        /// <summary>
        /// Retreive the Vector2D object representing the Ship's direction
        /// </summary>
        /// <returns></returns>
        public Vector2D GetDirection()
        {
            return _direction;
        }

        /// <summary>
        /// Retreive the Vector2D object representing the Ship's velocity
        /// </summary>
        /// <returns></returns>
        public Vector2D GetVelocity()
        {
            return _velocity;
        }

        /// <summary>
        /// Retreive the ship's last fired counter
        /// </summary>
        /// <returns></returns>
        public int GetLastFired()
        {
            return _lastFired;
        }

        /// <summary>
        /// Retreive the ship's last death counter
        /// </summary>
        /// <returns></returns>
        public int GetLastDeath()
        {
            return _lastDeath;
        }

        /// <summary>
        /// Update the HP of the Ship
        /// </summary>
        /// <param name="newHP"></param>
        public void SetHP(int newHP)
        {
            _hp = newHP;
        }

        /// <summary>
        /// Update the Location of the Ship
        /// </summary>
        /// <param name="newLocation"></param>
        public void SetLocation(Vector2D newLocation)
        {
            _location = newLocation;
        }

        /// <summary>
        /// Update the velocity of the Ship
        /// </summary>
        /// <param name="newVelocity"></param>
        public void SetVelocity(Vector2D newVelocity)
        {
            _velocity = newVelocity;
        }

        /// <summary>
        /// Update the direction of the Ship
        /// </summary>
        /// <param name="newDirection"></param>
        public void SetDirection(Vector2D newDirection)
        {
            _direction = newDirection;
        }

        public void SetLastFired(int newLastFired)
        {
            if (newLastFired >= 0)
                _lastFired = newLastFired;
        }

        /// <summary>
        /// Updates the last death of the Ship
        /// </summary>
        /// <param name="newLastDeath"></param>
        public void SetLastDeath(int newLastDeath)
        {
            if (newLastDeath >= 0)
                _lastDeath = newLastDeath;
        }

        /// <summary>
        /// Increments the Ship's score by 1
        /// </summary>
        public void IncrementScore()
        {
            this._score++;
        }

        /// <summary>
        /// Increments the Ship's total shot counter
        /// by 1
        /// </summary>
        public void IncrementTotalShots()
        {
            this._totalShots++;
        }

        /// <summary>
        /// Increments the Ship's hit counter by 1
        /// </summary>
        public void IncrementHits()
        {
            this._playerHits++;
        }

        /// <summary>
        /// Decrements the Ship's HP by 1
        /// </summary>
        public void DecrementHP()
        {
            if (this._hp > 0)
                this._hp--;
        }

        public void DecrementLastFired()
        {
            if (_lastFired > 0)
                _lastFired--;
        }

        /// <summary>
        /// Decrements the Ship's last death counter by 1
        /// </summary>
        public void DecrementLastDeath()
        {
            if (_lastDeath > 0)
                _lastDeath--;
        }

        /// <summary>
        /// Used to wrap a Ship's X value around a world
        /// </summary>
        public void WrapAroundX()
        {
           _location = new Vector2D(_location.GetX() * -1.0, _location.GetY());
        }

        /// <summary>
        /// Used to wrap a Ship's Y value around a world
        /// </summary>
        public void WrapAroundY()
        {
            _location = new Vector2D(_location.GetX(), _location.GetY() * -1.0);
        }

        /// <summary>
        /// Called when it is time for a Ship to respawn.
        /// Sets the respawn location according to the parameter
        /// </summary>
        /// <param name="newLocation">The new respawn location</param>
        public void Respawn(Vector2D newLocation)
        {
            this._location = new Vector2D(newLocation);
            this._velocity = new Vector2D(0.0, 0.0);
            this._direction = new Vector2D(0.0, -1.0);
            this._lastFired = 0;
            this._hp = 5;
        }

        /// <summary>
        /// Overriden ToString command that converts a Ship object
        /// into a JSON string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            _direction.Normalize();
            return JsonConvert.SerializeObject((object)this);
        }

    }
}
