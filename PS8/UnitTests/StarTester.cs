///
/// @authors Tony Diep and Sona Torosyan
///
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

/// <summary>
/// Contains a library of unit tests for sprite models 
/// as well as motion mechanic tests
/// </summary>
namespace UnitTests
{
    /// <summary>
    /// Contains tests for the ship model
    /// </summary>
    [TestClass]
    public class StarTester
    {
        /// <summary>
        /// Verifies the provided unique ID is passed in successfully
        /// </summary>
        [TestMethod]
        public void VerifyStarID()
        {
            Star star = new Star(1, new Vector2D(375, 375), 50.25);
            Assert.AreEqual(1, star.ID());
        }

        /// <summary>
        /// Verifies the provided location vector is passed in successfully
        /// </summary>
        [TestMethod]
        public void VerifyStarLocation()
        {
            Star star = new Star(1, new Vector2D(375, 375), 50.25);
            Assert.AreEqual(new Vector2D(375, 375), star.Location());          
        }

        /// <summary>
        /// Verifies the provided mass is passed in successfully
        /// </summary>
        [TestMethod]
        public void VerifyStarMass()
        {
            Star star = new Star(1, new Vector2D(375, 375), 50.25);
            Assert.AreEqual(50.25, star.Mass());
        }
    }
}
