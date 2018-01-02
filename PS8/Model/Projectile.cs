///
/// @authors Tony Diep and Sona Torosyan
///
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

/// <summary>
/// Contains the projectile class
/// </summary>
namespace Model
{
    /// <summary>
    /// Represents a projectile (i.e. bullet) 
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Projectile
    {
        //This projectile's ID number
        [JsonProperty(PropertyName = "proj")]
        private int proj;

        //Location coordinates
        [JsonProperty]
        private Vector2D loc;

        //Direction coordinates
        [JsonProperty]
        private Vector2D dir;

        //Keeps track of whether the projectile is alive or not
        [JsonProperty]
        private bool alive;

        //The ID that refers to who possesses this projectile
        [JsonProperty]
        private int owner;

        //The velocity vector for this projectile
        private Vector2D velocity;

        //The accelaration vector for this projectile
        private Vector2D acceleration;
      

        /// <summary>
        /// Creates a projectile given an unique ID, ship's unique ID, and location, direction, and
        /// velocity vectors
        /// </summary>
        /// <param name="proj">unique ID assigned to this projectile</param>
        /// <param name="owner">the ship's unique ID that correspond to this projectile</param>
        /// <param name="loc">location vector for this projectile</param>
        /// <param name="dir">direction vector for this projectile</param>
        /// <param name="velocity">velocity vector for this projectile</param>
        public Projectile(int proj, int owner, Vector2D loc, Vector2D dir, Vector2D velocity)
        {
            this.loc = loc;           
            this.proj = proj;
            this.owner = owner;
            alive = true;
            // this.velocity = new Vector2D(5, 5);
            this.velocity = velocity;
            this.dir = dir;
            this.acceleration = new Vector2D(0,0);
            dir.Normalize();
        }

        /// <summary>
        /// Gets the current owner who used this projectile
        /// </summary>
        /// <returns>current owner in terms of an ID</returns>
        public int Owner()
        {
            return owner;
        }

        /// <summary>
        /// Gets the current ID of this star
        /// </summary>
        /// <returns>the current ID</returns>
        public int ID()
        {
            return proj;
        }

        /// <summary>
        /// Increments the ID of this projectile
        /// </summary>
        public void IncrementID()
        {
            proj++;
        }

        /// <summary>
        /// Gets the velocity vector of this projectile
        /// </summary>
        /// <returns>the velocity vector</returns>
        public Vector2D Velocity()
        {
            return velocity;
        }

        /// <summary>
        /// Reports whether the projectile is still on the drawing board
        /// (Used to determine when to still draw the projectile and when to stop
        /// after a certain amount of milliseconds passes by)
        /// </summary>
        /// <returns>true if the projectile is still there and false otherwise</returns>
        public bool Alive()
        {
            return alive;
        }

        /// <summary>
        /// Helps update the alive status of this projectile; used to help determine when
        /// to draw and not draw this projectile onto the screen
        /// </summary>
        public void NoLongerDead()
        {
            alive = true;
        }

        /// <summary>
        /// Removes this projectile from the world
        /// </summary>
        public void KillProjectile()
        {
            alive = false;
        }

        /// <summary>
        /// Gets the current location of this projectile
        /// </summary>
        /// <returns>the current location of this projectile</returns>
        public Vector2D Location()
        {
            return loc;
        }

        /// <summary>
        /// Updates the location of this projectile
        /// </summary>
        /// <param name="newLocation">other vector that will be used to update the current location vector</param>
        public void UpdateLocation(Vector2D newLocation)
        {
            loc = newLocation;
        }

        /// <summary>
        /// Gets the direction coordinates of this projectile
        /// </summary>
        /// <returns>the direction coordinates of this projectile</returns>
        public Vector2D Direction()
        {
            return dir;
        }

        /// <summary>
        /// Updates the current acceleration vector to a new accerlation vector
        /// </summary>
        /// <param name="newAccelVec">the new acceleration vector to override current acceleration vector</param>
        public void UpdateAcceleration(Vector2D newAccelVec)
        {
            acceleration = newAccelVec;
        }

        /// <summary>
        /// Gets the acceleration vector from this projectile
        /// </summary>
        /// <returns>the acceleration vector</returns>
        public Vector2D Acceleration()
        {
            return acceleration;
        }

        /// <summary>
        /// Updates the velocity vector of this projectile to a new velocity vector
        /// </summary>
        /// <param name="newVel">the new velocity vector to set this as the current velocity vector</param>
        public void UpdateVelocity(Vector2D newVel)
        {
           velocity = newVel;
        }
    }
}