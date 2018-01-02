using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Model
{
    /// <summary>
    /// This class represents a star in the SpaceWars game
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Star
    {
        /// <summary>
        /// The star's ID
        /// </summary>
        [JsonProperty(PropertyName = "star")]
        private int _ID;

        /// <summary>
        /// The location of the star
        /// </summary>
        [JsonProperty(PropertyName = "loc")]
        private Vector2D _location;

        /// <summary>
        /// The mass of the star
        /// </summary>
        [JsonProperty(PropertyName = "mass")]
        private double _mass;
        
        /// <summary>
        /// Monitors which ID is for a given Star.
        /// The ID is incremented every time a Star
        /// is created as it is a static class member
        /// </summary>
        private static int _nextStarID;

        /// <summary>
        /// Counter used for when a star will be
        /// moved to a new, random location (extra game mode)
        /// </summary>
        private int _moveStarCounter;

        /// <summary>
        /// Default constructor for JSON serialization
        /// </summary>
        public Star()
        {
            _ID = -1;
            _location = new Vector2D();
            _mass = 0;
        }

        /// <summary>
        /// Constructor for a Star given two arguments
        /// </summary>
        /// <param name="location"></param>
        /// <param name="mass"></param>
        public Star(Vector2D location, double mass)
        {
            _ID = _nextStarID++;
            _location = location;
            _mass = mass;
        }

        /// <summary>
        /// Retreive the star's ID
        /// </summary>
        /// <returns></returns>
        public int GetID()
        {
            return _ID;
        }

        /// <summary>
        /// Retreive the Vector2D object representing the star's location
        /// </summary>
        /// <returns></returns>
        public Vector2D GetLocation()
        {
            return _location;
        }

        /// <summary>
        /// Retreive the star's mass
        /// </summary>
        /// <returns></returns>
        public double GetMass()
        {
            return _mass;
        }

        /// <summary>
        /// Retreive the move star counter
        /// </summary>
        /// <returns></returns>
        public int GetCounter()
        {
            return _moveStarCounter;
        }

        /// <summary>
        /// Update the location of the Star
        /// </summary>
        /// <param name="newLoc"></param>
        public void SetLocation(Vector2D newLoc)
        {
            if (newLoc != null)
                _location = newLoc;
        }

        /// <summary>
        /// Update the move star counter of the Star
        /// </summary>
        /// <param name="newCount"></param>
        public void SetCounter(int newCount)
        {
            _moveStarCounter = newCount;
        }

        /// <summary>
        /// Decrements the move star counter by 1
        /// </summary>
        public void DecrementCounter()
        {
            if (_moveStarCounter > 0)
                _moveStarCounter--;
        }

        /// <summary>
        /// Overriden ToString command that converts a Star object
        /// into a JSON string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
