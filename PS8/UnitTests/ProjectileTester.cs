///
/// @authors Tony Diep and Sona Torosyan
///
using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

/// <summary>
/// Contains a library of unit tests for sprite models 
/// as well as motion mechanic tests
/// </summary>
namespace UnitTests
{
    /// <summary>
    /// Contains tests for the projectile model
    /// </summary>
    [TestClass]
    public class ProjectileTester
    {
        /// <summary>
        /// Verifies that the projectile ID is correct
        /// </summary>
        [TestMethod]
        public void VerifyProjectileID()
        {
            Vector2D loc = new Vector2D(0, 0);
            Vector2D dir = new Vector2D(0, 0);
            Vector2D velocity = new Vector2D(0, 0);

            Projectile proj = new Projectile(0, 0, loc, dir, velocity);

            Assert.AreEqual(0, proj.ID());
        }

        /// <summary>
        /// Verifies that the projectile's owner's ID is correct
        /// </summary>
        [TestMethod]
        public void VerifyProjectileOwnerID()
        {
            Vector2D loc = new Vector2D(0, 0);
            Vector2D dir = new Vector2D(0, 0);
            Vector2D velocity = new Vector2D(0, 0);

            Projectile proj = new Projectile(0, 0, loc, dir, velocity);

            Assert.AreEqual(0, proj.Owner());
        }

        /// <summary>
        /// Verifies that the projectile's location vector is correct
        /// </summary>
        [TestMethod]
        public void VerifyProjectileLocationVector()
        {
            Vector2D loc = new Vector2D(0, 0);
            Vector2D dir = new Vector2D(0, 0);
            Vector2D velocity = new Vector2D(0, 0);

            Projectile proj = new Projectile(0, 0, loc, dir, velocity);

            Assert.AreEqual(new Vector2D(0, 0), proj.Location());
        }

        /// <summary>
        /// Verifies that the projectile's location vector successfully updated
        /// </summary>
        public void VerifyLocationVectorAfterUpdating()
        {
            Vector2D loc = new Vector2D(0, 0);
            Vector2D dir = new Vector2D(0, 0);
            Vector2D velocity = new Vector2D(0, 0);

            Projectile proj = new Projectile(0, 0, loc, dir, velocity);
            proj.UpdateLocation(new Vector2D(15, 15));

            Assert.AreEqual(new Vector2D(15, 15), proj.Location());
        }

        /// <summary>
        /// Verifies that the projectile's direction vector is correct
        /// </summary>
        [TestMethod]
        public void VerifyProjectileDirectionVector()
        {
            Vector2D loc = new Vector2D(0, 0);
            Vector2D dir = new Vector2D(1, 1);
            Vector2D velocity = new Vector2D(0, 0);

            Projectile proj = new Projectile(0, 0, loc, dir, velocity);

            Assert.AreEqual(new Vector2D(0.707106781186547, 0.707106781186547), proj.Direction());
        }

        /// <summary>
        /// Verifies that the projectile's velocity vector is correct
        /// </summary>
        [TestMethod]
        public void VerifyProjectileVelocityVector()
        {
            Vector2D loc = new Vector2D(0, 0);
            Vector2D dir = new Vector2D(0, 0);
            Vector2D velocity = new Vector2D(0, 0);

            Projectile proj = new Projectile(0, 0, loc, dir, velocity);

            Assert.AreEqual(new Vector2D(0, 0), proj.Velocity());
        }

        /// <summary>
        /// Verifies the alive status of a newly created projectile; should
        /// be true to begin with
        /// </summary>
        [TestMethod]
        public void VerifyAliveStatus()
        {
            Vector2D loc = new Vector2D(50, 50);
            Vector2D dir = new Vector2D(50, 50);
            Vector2D velocity = new Vector2D(15, 15);

            Projectile proj = new Projectile(0, 0, loc, dir, velocity);

            Assert.IsTrue(proj.Alive());
        }

        /// <summary>
        /// Verifies that the projectile is no longer dead 
        /// </summary>
        [TestMethod]
        public void VerifyProjectileNoLongerDead()
        {
            Vector2D loc = new Vector2D(50, 50);
            Vector2D dir = new Vector2D(50, 50);
            Vector2D velocity = new Vector2D(15, 15);

            Projectile proj = new Projectile(0, 0, loc, dir, velocity);

            proj.NoLongerDead();

            Assert.IsTrue(proj.Alive());
        }

        /// <summary>
        /// Verifies that the projectile has initial constant velocity (thus no acceleration), then
        /// check if that acceleration vector changed after setting
        /// </summary>
        [TestMethod]
        public void VerifyUpdateProjectileAcceleration()
        {
            Projectile proj = new Projectile(0, 0, new Vector2D(0, 0), new Vector2D(0, 0), new Vector2D(0, 0));

            Assert.AreEqual(new Vector2D(0, 0), proj.Acceleration());

            proj.UpdateAcceleration(new Vector2D(2, 2));

            Assert.AreEqual(new Vector2D(2, 2), proj.Acceleration());
        }

        /// <summary>
        /// Verifies that the projectile no longer has zero velocity after changing its 
        /// velocity vector
        /// </summary>
        [TestMethod]
        public void VerifyUpdateVelocityOnProjectile()
        {
            Projectile proj = new Projectile(0, 0, new Vector2D(0, 0), new Vector2D(0, 0), new Vector2D(0, 0));

            Assert.AreEqual(new Vector2D(0, 0), proj.Acceleration());

            proj.UpdateVelocity(new Vector2D(15, 15));

            Assert.AreEqual(new Vector2D(15, 15), proj.Velocity());
        }
    }
}
