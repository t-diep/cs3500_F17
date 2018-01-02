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
/// Contains the Star class
/// </summary>
namespace Model
{
    /// <summary>
    /// Represents the star sprite
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Star
    {
        //This star's ID number
        [JsonProperty(PropertyName = "star")]
        private int star;

        //Location coordinates
        [JsonProperty]
        private Vector2D loc;

        //This star's mass
        [JsonProperty]
        private double mass;


        /// <summary>
        /// Creates a new star, given an unique ID, location vector, and a mass
        /// </summary>
        /// <param name="star">the unique ID to assign to this star</param>
        /// <param name="loc">location vector</param>
        /// <param name="mass">mass of the star</param>
        public Star(int star, Vector2D loc, double mass)
        {
            //Set ID, location vector, and mass quantity for this star
            this.star = star;
            this.loc = loc;
            this.mass = mass;
        }


        /// <summary>
        /// Gets this star's ID
        /// </summary>
        /// <returns>star's ID</returns>
        public int ID()
        {
            return star;
        }

        /// <summary>
        /// Gets the current location of this star
        /// </summary>
        /// <returns>the current location of this star</returns>
        public Vector2D Location()
        {
            return loc;
        }

        /// <summary>
        /// Gets the current mass of this star
        /// </summary>
        /// <returns>current mass of this star</returns>
        public double Mass()
        {
            return mass;
        }
    }
}
