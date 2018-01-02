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
    public class ShipTester
    {
        /// <summary>
        /// Verifies the name and ID is correct 
        /// after creating ship object
        /// </summary>
        [TestMethod]
        public void VerifyShipInformationNameAndID()
        {
            Ship sonaPlayer = new Ship("Sona", 0);

            Assert.AreEqual("Sona", sonaPlayer.Name());
            Assert.AreEqual(0, sonaPlayer.ID());
        }

        /// <summary>
        /// Verifies data passed in is correct after
        /// creating the ship object
        /// </summary>
        [TestMethod]
        public void VerifyShipInformationNameIDAndHP()
        {
            Ship tonyPlayer = new Ship("Tony", 1, 5);

            Assert.AreEqual("Tony", tonyPlayer.Name());
            Assert.AreEqual(1, tonyPlayer.ID());
            Assert.AreEqual(5, tonyPlayer.HP());
        }

        /// <summary>
        /// Verifies if the ship is stationary (not thrusting)
        /// when created from the start
        /// </summary>
        [TestMethod]
        public void VerifyShipIsNotThrusting()
        {
            Ship sonaPlayer = new Ship("Sona", 2, 100);
            Assert.IsFalse(sonaPlayer.IsThrusting());
        }

        /// <summary>
        /// Verifies if the ship's location is at the origin
        /// </summary>
        [TestMethod]
        public void VerifyShipLocationVector()
        {
            Ship tonyPlayer = new Ship("Tony", 2, 5);
            Assert.AreEqual(new Vector2D(0, 0), tonyPlayer.Location());
        }

        /// <summary>
        /// Verifies if the ship's direction vector is by default
        /// <0, 0>
        /// </summary>
        [TestMethod]
        public void VerifyShipDirectionVector()
        {
            Ship danielKoptaPlayer = new Ship("Daniel Kopta", 10, 999);           
            Assert.AreEqual(new Vector2D(0, 0), danielKoptaPlayer.Direction());
        }

        /// <summary>
        /// Verifies if the ship's velocity vector is <0, 0> by default
        /// </summary>
        [TestMethod]
        public void VerifyShipVelocityVector()
        {
            Ship germainPlayer = new Ship("Germain", 10, 999);
            Assert.AreEqual(new Vector2D(0, 0), germainPlayer.Velocity());
        }

        /// <summary>
        /// Verifies that the ship's velocity vector updated correctly
        /// </summary>
        [TestMethod]
        public void VerifyNewVelocityAfterUpdating()
        {
            Ship tonyAndGermainPlayer = new Ship("TonyGermain?!", 1, 22);
            tonyAndGermainPlayer.UpdateVelocity(new Vector2D(3, 3));
            Assert.AreEqual(new Vector2D(3, 3), tonyAndGermainPlayer.Velocity());
        }

        /// <summary>
        /// Verifies if the ship's acceleration vector is <0, 0> by default 
        /// </summary>
        [TestMethod]
        public void VerifyShipAccerlationVector()
        {
            Ship dejohnsoPlayer = new Ship("DeJohnSo", 10, 999);
            Assert.AreEqual(new Vector2D(0, 0), dejohnsoPlayer.Acceleration());
        }

        /// <summary>
        /// Verifies that the acceleration vector is changed correctly
        /// </summary>
        [TestMethod]
        public void VerifyNewAccelrationAfterUpdating()
        {
            Ship tonySonaPlayer = new Ship("TonyAndSona", 6, 5);
            tonySonaPlayer.UpdateAcceleration(new Vector2D(5, 5));
            Assert.AreEqual(new Vector2D(5, 5), tonySonaPlayer.Acceleration());
        }

        /// <summary>
        /// Verifies that a ship player's HP changed continuously such that
        /// its hp reaches zero
        /// </summary>
        [TestMethod]
        public void VerifyShipIsDead()
        {
            Ship hitSonasShip = new Ship("Sona", 4, 5);

            for(int index = 0; index < 5; index++)
            {
                hitSonasShip.DecrementHP();
            }

            Assert.IsTrue(hitSonasShip.IsDead());
        }

        /// <summary>
        /// Verifies that the score successfully changed
        /// </summary>
        [TestMethod]
        public void VerifyUpdateScore()
        {
            Ship tonyPlayer = new Ship("Tony", 2, 5);
            tonyPlayer.UpdateScore();

            Assert.AreEqual(1, tonyPlayer.Score());
        }

        /// <summary>
        /// Verifies that the thrusting status successfully updated
        /// </summary>
        [TestMethod]
        public void VerifyUpdatingThrust()
        {
            Ship sonaPlayer = new Ship("Sona", 4, 5);
            sonaPlayer.UpdateThrust(true);

            Assert.IsTrue(sonaPlayer.IsThrusting());
        }

        /// <summary>
        /// Verifies that after killing the ship, we restore 
        /// the HP to double its original HP
        /// </summary>
        [TestMethod]
        public void VerifyUpdateHP()
        {
            Ship tonyPlayer = new Ship("Tony", 0, 5);

            for(int index = 0; index < 5; index++)
            {
                tonyPlayer.DecrementHP();
            }

            Assert.AreEqual(0, tonyPlayer.HP());

            tonyPlayer.UpdateHP(10);

            Assert.AreEqual(10, tonyPlayer.HP());
        }

        /// <summary>
        /// Verifies that we set the score of Sona's ship to 5 
        /// after Tony's ship is dead
        /// </summary>
        [TestMethod]
        public void VerifySettingScore()
        {
            Ship sonaPlayer = new Ship("Sona", 1, 5);
            Ship tonyPlayer = new Ship("Tony", 0, 5);

            for(int index = 0; index < 5; index++)
            {
                tonyPlayer.DecrementHP();
            }

            sonaPlayer.SetScore(5);

            Assert.AreEqual(5, sonaPlayer.Score());
        }

        /// <summary>
        /// Verifies that the number of frames incremented successfully
        /// </summary>
        [TestMethod]
        public void VerifyIncrementingNumShipFrames()
        {
            Ship sonaPlayer = new Ship("Sona", 0);
            sonaPlayer.IncrementShipFrames();

            Assert.AreEqual(1, sonaPlayer.ShipNumberOfFrames());
        }

        /// <summary>
        /// Verifies that the resetting of ship frames was done successfully
        /// </summary>
        [TestMethod]
        public void VerifyResettingNumShipFrames()
        {
            Ship tonyPlayer = new Ship("Tony", 2);
            
            for(int index = 0; index < 5; index++)
            {
                tonyPlayer.IncrementShipFrames();
            }

            tonyPlayer.ResetShipNumFrames();

            Assert.AreEqual(0, tonyPlayer.ShipNumberOfFrames());
        }

        /// <summary>
        /// Verifies that the incrementing of number of projectile frames
        /// was done successfully
        /// </summary>
        [TestMethod]
        public void VerifyIncrementingProjNumFrames()
        {
            Ship sonaPlayer = new Ship("Sona", 4);

            sonaPlayer.IncrementProjNumFrames();

            Assert.AreEqual(1, sonaPlayer.NumberOfProjFrames());
        }

        /// <summary>
        /// Verifies that the resetting of projectile number of frames 
        /// was done successfully
        /// </summary>
        [TestMethod]
        public void VerifyResettingProjNumFrames()
        {
            Ship tonyPlayer = new Ship("Tony", 3);
            
            for(int index = 0; index < 5; index++)
            {
                tonyPlayer.IncrementProjNumFrames();
            }

            tonyPlayer.ResetProjNumFrames();

            Assert.AreEqual(0, tonyPlayer.NumberOfProjFrames());
        }

        /// <summary>
        /// Verifies that the ship did rotate at the specified angle in degrees 
        /// (in this case, 180 degrees)
        /// </summary>
        [TestMethod]
        public void VerifyRotatingShip()
        {
            Ship rotateSonaShip = new Ship("Sona", 3);
            rotateSonaShip.Rotate(180);

            Assert.AreNotEqual(new Vector2D(0, 0), rotateSonaShip.Direction());
        }

        /// <summary>
        /// Verifies that no two ships will ever have the same location as each other
        /// when displayed onto the game
        /// </summary>
        [TestMethod]
        public void VerifyTwoShipsAreNotEqual()
        {
            Ship tonyPlayer = new Ship("Tony", 3, 5);
            tonyPlayer.UpdateLocation(new Vector2D(200, 100));

            Ship sonaPlayer = new Ship("Sona", 2, 5);
            sonaPlayer.UpdateLocation(new Vector2D(-100, -200));

            Assert.IsFalse(tonyPlayer.Location().Equals(sonaPlayer.Location()));
            Assert.IsFalse(tonyPlayer.Location().GetHashCode() == sonaPlayer.Location().GetHashCode());
        }

        /// <summary>
        /// Verifies that one ship's location vector is never equal to a ship's null vector
        /// </summary>
        [TestMethod]
        public void VerifyOneNotNullShipAndOneNullShipAreNotEqual()
        {
            Ship tonyPlayer = new Ship("Tony", 3, 5);
            tonyPlayer.UpdateLocation(new Vector2D(200, 100));

            Ship sonaPlayer = new Ship("Sona", 2, 5);
            sonaPlayer.UpdateLocation(null);

            Assert.IsFalse(tonyPlayer.Location().Equals(sonaPlayer.Location()));
        }

        /// <summary>
        /// Verifies that comparing a ship's location vector with the ship itself
        /// will never be equal to each other
        /// </summary>
        [TestMethod]
        public void VerifyCompareOneShipLocationVectorToAShipObject()
        {
            Ship tonyPlayer = new Ship("Tony", 1, 5);

            Assert.IsFalse(tonyPlayer.Location().Equals(tonyPlayer));
        }

        /// <summary>
        /// Verifies that the ship's direction vector changed after applying
        /// left rotation
        /// </summary>
        [TestMethod]
        public void VerifyShipDirectionRotatingLeft()
        {
            Ship tonyPlayer = new Ship("Tony", 0, 5);          
            Vector2D currDir = tonyPlayer.Direction();

            tonyPlayer.ChangeDirection(new Vector2D(100, 300));

            Assert.AreNotEqual(currDir, tonyPlayer.Direction());
        }

        /// <summary>
        /// Verifies that the clamping for positive x coordinate and negative y coordinate
        /// </summary>
        [TestMethod]
        public void VerifyShipDirectionVectorClampPosXNegY()
        {
            Ship tonyPlayer = new Ship("Tony", 0, 5);
            tonyPlayer.ChangeDirection(new Vector2D(1.1, -2.0));
            tonyPlayer.Direction().Clamp();

            Assert.IsTrue(tonyPlayer.Direction().GetX() == 1.0);
            Assert.IsTrue(tonyPlayer.Direction().GetY() == -1.0);
        }

        /// <summary>
        /// Verifies that the clamping for negative x coordinate and positive y coordinate
        /// </summary>
        [TestMethod]
        public void VerifyShipDirectionVectorClampNegXPosY()
        {
            Ship sonaPlayer = new Ship("Sona", 1, 5);
            sonaPlayer.ChangeDirection(new Vector2D(-1.1, 1.1));
            sonaPlayer.Direction().Clamp();

            Assert.IsTrue(sonaPlayer.Direction().GetX() == -1.0);
            Assert.IsTrue(sonaPlayer.Direction().GetY() == 1.0);
        }

        /// <summary>
        /// Verifies the ship's direction vectors' correct angle conversion
        /// </summary>
        [TestMethod]
        public void VerifyShipVectorToAngle()
        {
            Ship sonaPlayer = new Ship("Sona", 0, 5);
            sonaPlayer.ChangeDirection(new Vector2D(-50, -50));
            sonaPlayer.Direction().Normalize();

            Assert.AreEqual(-45, sonaPlayer.Direction().ToAngle());
        }
    }
}
