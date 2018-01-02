using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Model
{
    /// <summary>
    /// This class represents a projectile shot by a player in the SpaceWars game
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Projectile
    {
        /// <summary>
        /// The projectile's ID
        /// </summary>
        [JsonProperty(PropertyName = "proj")]
        private int _ID;

        /// <summary>
        /// The location of the projectile represented in a Vector2D object
        /// </summary>
        [JsonProperty(PropertyName = "loc")]
        private Vector2D _location;

        /// <summary>
        /// The direction of the projectile represented in a Vector2D object
        /// </summary>
        [JsonProperty(PropertyName = "dir")]
        private Vector2D _direction;
        
        /// <summary>
        /// Whether the projectile is alive or not
        /// </summary>
        [JsonProperty(PropertyName = "alive")]
        private bool _alive;

        /// <summary>
        /// The ID of the ship which the projectile orginated from
        /// </summary>
        [JsonProperty(PropertyName = "owner")]
        private int _owner;
        
        /// <summary>
        /// Monitors which ID is for a given projectile.
        /// The ID is incremented every time a projectile
        /// is created as it is a static class memeber
        /// </summary>
        private static int _nextProjID;

        /// <summary>
        /// Default constructor for JSON serialization
        /// </summary>
        public Projectile()
        {
            _ID = 0;
            _location = new Vector2D();
            _direction = new Vector2D();
            _alive = true;
            _owner = -1;
        }

        /// <summary>
        /// Creates projectile given four arguments
        /// </summary>
        /// <param name="owner">The owner of the projectile</param>
        /// <param name="loc">The location of the projectile</param>
        /// <param name="dir">The direction of the projectile</param>
        public Projectile(int owner, Vector2D loc, Vector2D dir)
        {
            _ID = _nextProjID++;
            _owner = owner;
            _location = loc;
            _direction = dir;
            _alive = true;
        }

        /// <summary>
        /// Retreive the projectile's ID
        /// </summary>
        /// <returns></returns>
        public int GetID()
        {
            return _ID;
        }

        /// <summary>
        /// Return whether the projectile is alive or not
        /// </summary>
        /// <returns></returns>
        public bool GetAlive()
        {
            return _alive;
        }

        /// <summary>
        /// Retreive the Vector2D object representing the projectiles's location
        /// </summary>
        /// <returns></returns>
        public Vector2D GetLocation()
        {
            return _location;
        }

        /// <summary>
        /// Retreive the Vector2D object representing the projectile's direction
        /// </summary>
        /// <returns></returns>
        public Vector2D GetDirection()
        {
            return _direction;
        }
        
        /// <summary>
        /// Retreive the ID of the owner of the projectile
        /// </summary>
        /// <returns></returns>
        public int GetOwner()
        {
            return _owner;
        }
        
        /// <summary>
        /// Update the location of the Projectile
        /// </summary>
        /// <param name="newVect"></param>
        public void SetLocation(Vector2D newVect)
        {
            if (newVect != null)
                _location = newVect;
        }

        /// <summary>
        /// Update the direction of the Projectile
        /// </summary>
        /// <param name="newVect"></param>
        public void SetDirection(Vector2D newVect)
        {
            if (newVect != null)
                _location = newVect;
        }
        
        /// <summary>
        /// Sets the projectile's alive state to false
        /// </summary>
        public void Death()
        {
          _alive = false;
        }

        /// <summary>
        /// Overriden ToString command that converts a Projectile object
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
