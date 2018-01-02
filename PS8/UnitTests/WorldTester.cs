///
/// @authors Tony Diep and Sona Torosyan
///
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using System.Collections.Generic;

/// <summary>
/// Contains a library of unit tests for sprite models 
/// as well as motion mechanic tests
/// </summary>
namespace UnitTests
{
    /// <summary>
    /// Contains tests for the World model
    /// </summary>
    [TestClass]
    public class WorldTester
    {
        /// <summary>
        /// Verifies the dictionary sizes of the empty world constructor;
        /// that is, there should be no ships, projectiles, and stars at the moment
        /// </summary>
        [TestMethod]
        public void VerifyDefaultWorldConstructor()
        {
            World helloWorld = new World();

            Assert.AreEqual(0, helloWorld.Stars().Count);
            Assert.AreEqual(0, helloWorld.Projectiles().Count);
            Assert.AreEqual(0, helloWorld.Ships().Count);
        }

        /// <summary>
        /// Verifies that when creating the world, there should be
        /// no ships yet
        /// </summary>
        [TestMethod]
        public void VerifyNoShipsInWorldYet()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, false);

            Assert.IsTrue(helloWorld.Ships().Count == 0);
        }

        /// <summary>
        /// Verifies that when creating the world, there should be
        /// no stars yet
        /// </summary>
        [TestMethod]
        public void VerifyNoStarsInWorldYet()
        {
            //double projVelocity, double engineStrength, int shipSize, int starSize, int worldSize, 
            //int numOfFrames, double rotationAngle, int respawnDelay, int defaultHP)
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, false);

            Assert.IsTrue(helloWorld.Stars().Count == 0);
        }

        /// <summary>
        /// Verifies that when creating the world, there should be
        /// no projectiles yet
        /// </summary>
        [TestMethod]
        public void VerifyNoProjectilesInWorldYet()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, false);

            Assert.IsTrue(helloWorld.Projectiles().Count == 0);
        }

        /// <summary>
        /// Verify that the actual projectile object is added to the world when
        /// the projectile ID is only there
        /// </summary>        
        [TestMethod]
        public void VerifyProjIDIsInWorldButNotActualProj()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, false);

            Vector2D loc = new Vector2D(0, 0);
            Vector2D dir = new Vector2D(0, 0);
            Vector2D velocity = new Vector2D(0, 0);

            Projectile proj = new Projectile(0, 0, loc, dir, velocity);

            helloWorld.UpdateProjectiles(proj);
            helloWorld.Projectiles().Remove(proj.ID());

            helloWorld.UpdateProjectiles(proj);

            Assert.IsTrue(helloWorld.Projectiles().Count == 2);
        }

        /// <summary>
        /// Verify that there is now one ship in the world after updating
        /// </summary>
        [TestMethod]
        public void VerifyOneShipIsInWorld()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, false);
            helloWorld.UpdateShip(new Ship("Sona", 0, 5));

            Assert.IsTrue(helloWorld.Ships().Count == 1);
        }

        /// <summary>
        /// Verify that the actual ship object is added to the world when
        /// the ship ID is only there
        /// </summary>        
        [TestMethod]
        public void VerifyShipIDIsInWorldButNotActualShip()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, false);

            Ship germainShip = new Ship("Germain", 0, 5);

            helloWorld.UpdateShip(germainShip);
            helloWorld.Ships().Remove(germainShip.ID());

            helloWorld.UpdateShip(germainShip);

            Assert.IsTrue(helloWorld.Ships().Count == 1);
        }

        /// <summary>
        /// Verifies that when the ship location is within the star location,
        /// the ship should have a different location that is outside of the star
        /// radius 
        /// </summary>
        [TestMethod]
        public void VerifyUpdateShipWhenOverlappingTheStar()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, false);

            Ship tonyShip = new Ship("Tony", 0, 5);
            tonyShip.UpdateLocation(new Vector2D(375, 375));

            Star star = new Star(0, new Vector2D(375, 375), 50.25);
            helloWorld.UpdateStars(star);

            helloWorld.UpdateShip(tonyShip);

            Assert.IsFalse(tonyShip.Location().GetX() == star.Location().GetX());
            Assert.IsFalse(tonyShip.Location().GetY() == star.Location().GetY());
        }

        /// <summary>
        /// Verify that there is now one star in the world after updating
        /// </summary>
        [TestMethod]
        public void VerifyOneStarIsInWorld()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, false);
            helloWorld.UpdateStars(new Star(0, new Vector2D(375, 375), 50.25));

            Assert.IsTrue(helloWorld.Stars().Count == 1);
        }

        /// <summary>
        /// Verify that the actual star object is added to the world
        /// when there's only the star ID
        /// </summary>
        [TestMethod]
        public void VerifyStarIDIsInWorldButNotActualStar()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, false);
            
            Star star = new Star(0, new Vector2D(375, 375), 50.25);

            helloWorld.UpdateStars(star);
            helloWorld.UpdateStars(star);

            Assert.IsTrue(helloWorld.Stars().Count == 1);
        }

        /// <summary>
        /// Verifies that there is now one projectile in the world after updating
        /// </summary>
        [TestMethod]
        public void VerifyOneProjectileIsInWorld()
        {
            Vector2D loc = new Vector2D(0, 0);
            Vector2D dir = new Vector2D(0, 0);
            Vector2D velocity = new Vector2D(0, 0);

            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, false);
            helloWorld.UpdateProjectiles(new Projectile(0, 0, loc, dir, velocity));

            Assert.IsTrue(helloWorld.Projectiles().Count == 1);
        }

        /// <summary>
        /// Stress test... verify that 100 projectiles and 100 ships are 
        /// all added 
        /// </summary>
        [TestMethod]
        public void VerifyManyDifferentSpritesInWorld()
        {
            Vector2D loc = new Vector2D(0, 0);
            Vector2D dir = new Vector2D(0, 0);
            Vector2D velocity = new Vector2D(0, 0);

            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, false);

            for(int index = 0; index < 100; index++)
            {
                Vector2D indexVector = new Vector2D(index, index);

                helloWorld.UpdateShip(new Ship("Ship" + index, index, 5));
                helloWorld.UpdateProjectiles(new Projectile(index, index, loc + indexVector, dir + indexVector, velocity + indexVector));
            }

            Assert.IsTrue(helloWorld.Ships().Count == 100);
            Assert.IsTrue(helloWorld.Projectiles().Count == 100);
        }

        /// <summary>
        /// Verifies that the ship thrusted (that is, it started on its 
        /// stationary point and then left out of it)
        /// </summary>
        [TestMethod]
        public void VerifyShipThrust()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, false);

            Ship tonyShip = new Ship("Tony", 0, 5);
            Vector2D currDir = tonyShip.Direction();

            helloWorld.UpdateShip(tonyShip);
            helloWorld.Thrust(tonyShip.ID());

            Assert.IsFalse(currDir != tonyShip.Direction());
        }

        /// <summary>
        /// Verifies that the projectile location has changed from
        /// its stationary location
        /// </summary>
        [TestMethod]
        public void VerifyingUpdateProjectileLocation()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, false);

            Vector2D loc = new Vector2D(750, 750);
            Vector2D dir = new Vector2D(0, 0);
            Vector2D velocity = new Vector2D(15, 15);

            Projectile proj = new Projectile(0, 0, loc, dir, velocity);

            helloWorld.UpdateProjectiles(proj);

            helloWorld.UpdateProjectileLocation(proj.ID());

            Assert.IsFalse(loc.GetX() == proj.Location().GetX());
            Assert.IsFalse(loc.GetY() == proj.Location().GetY());
        }

        /// <summary>
        /// Verifies that the ship has gravity acceleration relative to 
        /// the star
        /// </summary>
        [TestMethod]
        public void VerifyApplyGravityAccelerationToShip()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, false);

            Vector2D loc = new Vector2D(750, 750);
            Vector2D dir = new Vector2D(0, 0);
            Vector2D gravity = new Vector2D(9.8, 9.8);

            Ship sonaPlayer = new Ship("Sona", 0, 5);

            helloWorld.UpdateShip(sonaPlayer);
            helloWorld.ApplyGravityAcceleration(gravity, sonaPlayer.ID());

            Assert.AreEqual(9.8, sonaPlayer.Acceleration().GetX());
            Assert.AreEqual(9.8, sonaPlayer.Acceleration().GetY());
        }

        /// <summary>
        /// Verifies that the ship's velocity has reset to default velocity
        /// </summary>
        [TestMethod]
        public void VerifyResetToDefaultVelocityForShip()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, false);

            Ship sonaTonyPlayer = new Ship("SonaTony", 0, 5);
            sonaTonyPlayer.UpdateVelocity(new Vector2D(5.5, 5.5));

            Assert.AreEqual(new Vector2D(5.5, 5.5), sonaTonyPlayer.Velocity());

            helloWorld.UpdateShip(sonaTonyPlayer);            
            helloWorld.ResetToShipDefaultVelocity(sonaTonyPlayer.ID(), 0);

            Assert.AreEqual(new Vector2D(0, 0), sonaTonyPlayer.Velocity());
        }

        /// <summary>
        /// Verifies that the projectile removes from the world
        /// </summary>
        [TestMethod]
        public void VerifyRemoveProjectileFromWorld()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, false);

            Vector2D loc = new Vector2D(350, 350);
            Vector2D dir = new Vector2D(0, 0);
            Vector2D velocity = new Vector2D(15, 15);

            Projectile proj = new Projectile(0, 0, loc, dir, velocity);

            helloWorld.UpdateProjectiles(proj);

            helloWorld.RemoveProjectile(proj);

            Assert.AreEqual(0, helloWorld.Projectiles().Count);
        }

        /// <summary>
        /// Verifies that when firing, a projectile is created associated with 
        /// the ship
        /// </summary>
        [TestMethod]
        public void VerifyFireProjectileFromShip()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, false);

            Ship tonyPlayer = new Ship("Tony", 0, 5);
            tonyPlayer.UpdateVelocity(new Vector2D(5, 5));

            helloWorld.UpdateShip(tonyPlayer);
            helloWorld.Fire(tonyPlayer.ID());

            Assert.AreEqual(1, helloWorld.Projectiles().Count);
            Assert.IsTrue(helloWorld.Projectiles()[tonyPlayer.ID()].ID() == tonyPlayer.ID());
        }

        /// <summary>
        /// Verifies that the ship's non-zero acceleration should reset to 
        /// zero acceleration
        /// </summary>
        [TestMethod]
        public void VerifyResetToZeroAccelerationForShip()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, false);

            Ship tonyPlayer = new Ship("Tony", 0, 5);
            tonyPlayer.UpdateAcceleration(new Vector2D(2, 2) + new Vector2D(5, 5));

            helloWorld.UpdateShip(tonyPlayer);

            helloWorld.ResetToZeroAcceleration(tonyPlayer.ID());

            Assert.AreEqual(new Vector2D(0, 0), tonyPlayer.Acceleration());
        }

        /// <summary>
        /// Verifies that a projectile is within the ship's radius of collision
        /// </summary>
        [TestMethod]
        public void VerifyProjAndShipAreColliding()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, false);

            Ship tonyPlayer = new Ship("Tony", 0, 5);
            Vector2D tonyLoc = new Vector2D(200, 305);
            tonyPlayer.UpdateLocation(tonyLoc);
            tonyPlayer.UpdateVelocity(new Vector2D(5, 5));
            helloWorld.UpdateShip(tonyPlayer);


            Vector2D projLoc = tonyPlayer.Location();
            Vector2D projDir = new Vector2D(1, 1);
            Vector2D projVelocity = tonyPlayer.Velocity();

            Projectile proj = new Projectile(0, 1, projLoc, projDir, projVelocity);

            Ship sonaPlayer = new Ship("Sona", 4, 0);
            helloWorld.UpdateShip(sonaPlayer);
            helloWorld.UpdateProjectiles(proj);

            helloWorld.CollideShip();

            Assert.AreEqual(4, tonyPlayer.HP());
        }


        /// <summary>
        /// Verifies that the ship's location is within the radius of the star, 
        /// and should therefore, collide with the star
        /// </summary>
        [TestMethod]
        public void VerifyCollideWithStar()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, false);

            Ship tonyPlayer = new Ship("Tony", 0, 5);
            Star star = new Star(0, new Vector2D(375, 375), 35);

            helloWorld.UpdateShip(tonyPlayer);
            helloWorld.Ships().Remove(tonyPlayer.ID());
            helloWorld.UpdateShip(tonyPlayer);

            tonyPlayer.UpdateLocation(new Vector2D(375, 375));

            Ship sonaPlayer = new Ship("Sona", 1, 0);
            helloWorld.UpdateShip(sonaPlayer);

            helloWorld.UpdateStars(star);

            helloWorld.CollideWithStar();

            Assert.AreEqual(0, tonyPlayer.HP());
        }

        /// <summary>
        /// Verifies that Sona's ship did not had the same location as 
        /// the first time it updated its location compared to the second time it updated
        /// </summary>
        [TestMethod]
        public void VerifyUpdateShipLocation()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, false);
            Ship sonaPlayer = new Ship("Sona", 0, 5);

            helloWorld.UpdateShip(sonaPlayer);

            Vector2D firstUpdatedLoc = sonaPlayer.Location();

            helloWorld.UpdateShip(sonaPlayer.ID());

            Vector2D secondUpdatedLoc = sonaPlayer.Location();

            Assert.IsTrue(firstUpdatedLoc != new Vector2D(0, 0));
            Assert.IsTrue(secondUpdatedLoc != new Vector2D(0, 0));
            Assert.IsTrue(firstUpdatedLoc != secondUpdatedLoc);
        }

        /// <summary>
        /// Verifies that when a ship goes out of bounds in the positive x direction,
        /// the ship will be wrapped back to the left bounds of the universe square
        /// </summary>
        [TestMethod]
        public void VerifyWorkAroundXCoordinateOutOfBoundsPositive()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, false);

            Ship tonyGermain = new Ship("TonyGermain", 3, 9);

            helloWorld.UpdateShip(tonyGermain);
            helloWorld.Ships().Remove(tonyGermain.ID());

            tonyGermain.UpdateLocation(new Vector2D(376, 375));

            helloWorld.UpdateShipThrust(tonyGermain.ID(), true);

            helloWorld.UpdateShip(tonyGermain.ID());

            Assert.AreEqual(new Vector2D(-375, 375), tonyGermain.Location());
        }

        /// <summary>
        /// Verifies that when a ship goes out of bounds in the negative x direction,
        /// the ship will wrap around to the right bounds of the universe square
        /// </summary>
        [TestMethod]
        public void VerifyWorkAroundXCoordinateOutOfBoundsNegative()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, false);

            Ship tonyGermain = new Ship("TonyGermain", 3, 9);

            helloWorld.UpdateShip(tonyGermain);
            helloWorld.Ships().Remove(tonyGermain.ID());

            tonyGermain.UpdateLocation(new Vector2D(-376, 375));

            helloWorld.UpdateShipThrust(tonyGermain.ID(), true);

            helloWorld.UpdateShip(tonyGermain.ID());

            Assert.AreEqual(new Vector2D(375, 375), tonyGermain.Location());
        }

        /// <summary>
        /// Verifies that when a ship goes out of bounds in the positive y direction,
        /// the ship will wrap around to the bottom bounds of the universe square
        /// </summary>
        [TestMethod]
        public void VerifyWorkAroundYCoordinateOutOfBoundsPositive()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, false);

            Ship tonyGermain = new Ship("TonyGermain", 3, 9);

            helloWorld.UpdateShip(tonyGermain);
            helloWorld.Ships().Remove(tonyGermain.ID());

            tonyGermain.UpdateLocation(new Vector2D(375, 376));

            helloWorld.UpdateShipThrust(tonyGermain.ID(), true);

            helloWorld.UpdateShip(tonyGermain.ID());

            Assert.AreEqual(new Vector2D(375, -375), tonyGermain.Location());
        }

        /// <summary>
        /// Verifies that when a ship goes out of bounds in the negative y direction,
        /// the ship will wrap around to the top bounds of the universe square
        /// </summary>
        [TestMethod]
        public void VerifyWorkAroundYCoordinateOutOfBoundsNegative()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, false);

            Ship tonyGermain = new Ship("TonyGermain", 3, 9);

            helloWorld.UpdateShip(tonyGermain);
            helloWorld.Ships().Remove(tonyGermain.ID());

            tonyGermain.UpdateLocation(new Vector2D(375, -376));

            helloWorld.UpdateShipThrust(tonyGermain.ID(), true);

            helloWorld.UpdateShip(tonyGermain.ID());

            Assert.AreEqual(new Vector2D(375, 375), tonyGermain.Location());
        }

        /// <summary>
        /// Verifies that the ship turned left from its current direction 
        /// </summary>
        [TestMethod]
        public void VerifyShipTurnLeft()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, false);

            Ship koptaPlayer = new Ship("Kopta", 4, 5);
            Vector2D currDir = koptaPlayer.Direction();

            helloWorld.UpdateShip(koptaPlayer);
            helloWorld.ApplyShipLeftRotation(koptaPlayer.ID());
            
            Assert.IsTrue(koptaPlayer.Direction().GetX() != currDir.GetX());
            Assert.IsTrue(koptaPlayer.Direction().GetY() != currDir.GetY());
        }

        /// <summary>
        /// Verifies that the ship turned right from its current direction
        /// </summary>
        [TestMethod]
        public void VerifyShipTurnRight()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, false);

            Ship koptaPlayer = new Ship("Kopta", 4, 5);
            Vector2D currDir = koptaPlayer.Direction();

            helloWorld.UpdateShip(koptaPlayer);
            helloWorld.ApplyShipRightRotation(koptaPlayer.ID());

            Assert.IsTrue(koptaPlayer.Direction().GetX() != currDir.GetX());
            Assert.IsTrue(koptaPlayer.Direction().GetY() != currDir.GetY());
        }

        /// <summary>
        /// Verifies that the ship collided with a projectile and respawned
        /// to a different random location other than its original location
        /// </summary>
        [TestMethod]
        public void VerifyRespawningShipAfterCollidingWithProjectile()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, false);

            Ship germainShip = new Ship("Germain", 3, 5);
            Vector2D currLoc = germainShip.Location();

            Vector2D projLoc = new Vector2D(germainShip.Location().GetX() - 10, germainShip.Location().GetY() - 10);
            Vector2D projDir = new Vector2D(germainShip.Direction().GetX(), germainShip.Direction().GetY());
            Vector2D projVelocity = new Vector2D(15, 15);

            Projectile proj = new Projectile(1, 1, projLoc, projDir, projVelocity);

            helloWorld.UpdateShip(germainShip);
            helloWorld.UpdateProjectiles(proj);

            helloWorld.CollideShip();

            helloWorld.RespawnShip(germainShip.ID());

            Assert.AreNotEqual(currLoc, germainShip.Location());
        }

        /// <summary>
        /// Verifies that the ship collided with the star and respawned 
        /// to a different random location other than its original location
        /// </summary>
        [TestMethod]
        public void VerifyRespawningShipAfterCollidingWithStar()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, false);

            Ship germainShip = new Ship("Germain", 3, 5);
            Star star = new Star(0, new Vector2D(375, 375), 35);

            helloWorld.UpdateShip(germainShip);
            helloWorld.UpdateStars(star);
            helloWorld.Ships()[germainShip.ID()].UpdateLocation(new Vector2D(370, 370));

            Vector2D currLoc = germainShip.Location();

            helloWorld.CollideWithStar();

            helloWorld.RespawnShip(germainShip.ID());

            Assert.AreNotEqual(currLoc, germainShip.Location());
        }

        /// <summary>
        /// Verifies that the ship respawns successfully while still in a  space not within the star's 
        /// radius or any radii of all current ships
        /// </summary>
        [TestMethod]
        public void VerifyRespawnShipWithOtherShipsAndStarAround()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, false);
          

            Ship tonyPlayer = new Ship("Tony", 1, 1);
            tonyPlayer.UpdateLocation(new Vector2D(100, 100));
            tonyPlayer.UpdateVelocity(new Vector2D(15, 15));

            Vector2D projLoc = tonyPlayer.Location();
            Vector2D projDir = new Vector2D(0, 0);
            Vector2D projVelocity = tonyPlayer.Velocity();

            Projectile proj = new Projectile(tonyPlayer.ID(), tonyPlayer.ID(), projLoc, projDir, projVelocity);

            helloWorld.UpdateShip(tonyPlayer);
            helloWorld.UpdateProjectiles(proj);

            helloWorld.CollideShip();

            helloWorld.RespawnShip(tonyPlayer.ID());

            Assert.IsTrue(tonyPlayer.HP() == 5);           
        }

        /// <summary>
        /// Verifies that the ship is able to respawn even when the default location
        /// vector is a zero vector
        /// </summary>
        [TestMethod]
        public void VerifyRespawningOnZeroVector()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, false);

            Ship tonyPlayer = new Ship("Tony", 1, 1);
            Vector2D currLoc = tonyPlayer.Location();

            Star star = new Star(0, new Vector2D(70, 70), 35);

            Ship sonaPlayer = new Ship("Sona", 0, 1);
            sonaPlayer.UpdateLocation(new Vector2D(5, 5));

            helloWorld.UpdateShip(tonyPlayer);
            helloWorld.UpdateStars(star);
            helloWorld.UpdateShip(sonaPlayer);
            helloWorld.CollideShip();
            helloWorld.CollideWithStar();
            helloWorld.RespawnShip(tonyPlayer.ID());

            Assert.AreNotEqual(currLoc, tonyPlayer.Location());
            Assert.IsTrue(tonyPlayer.HP() == 5);
        }
        
        /// <summary>
        /// Verifies that a ship with 1 HP left and hit by a projectile 
        /// belonging to another ship hits 0 HP, which triggers a projectile
        /// ship collision; this means the ship containing the projectile should 
        /// have a score of 1
        /// </summary>
        [TestMethod]
        public void VerifyCollideProjectileKillsShipWith1HP()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, false);

            Ship tonyPlayer = new Ship("Tony", 0, 1);
            Ship sonaPlayer = new Ship("Sona", 1, 5);

            helloWorld.UpdateShip(sonaPlayer);
            helloWorld.UpdateShip(tonyPlayer);

            Vector2D projLoc = tonyPlayer.Location();
            Vector2D projDir = tonyPlayer.Direction();
            Vector2D projVelocity = new Vector2D(15, 15);

            Projectile proj = new Projectile(1, 1, projLoc, projDir, projVelocity);

            helloWorld.UpdateProjectiles(proj);

            helloWorld.CollideShip();

            Assert.AreEqual(1, sonaPlayer.Score());
        }

        /// <summary>
        /// Verifies that the collision between an adjacent projectile and
        /// an already dead ship cannot happen; thus, score of the ship containing
        /// the projectile should stay 0
        /// </summary>
        [TestMethod]
        public void VerifyShipAlreadyContainingHPProjCollision()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, false);

            Ship tonyPlayer = new Ship("Tony", 0, 0);
            Ship sonaPlayer = new Ship("Sona", 1, 5);

            helloWorld.UpdateShip(sonaPlayer);
            helloWorld.UpdateShip(tonyPlayer);

            Vector2D projLoc = tonyPlayer.Location();
            Vector2D projDir = tonyPlayer.Direction();
            Vector2D projVelocity = new Vector2D(15, 15);

            Projectile proj = new Projectile(1, 1, projLoc, projDir, projVelocity);

            helloWorld.UpdateProjectiles(proj);

            helloWorld.CollideShip();

            Assert.AreEqual(0, sonaPlayer.Score());
        }

        /// <summary>
        /// Verifies that after 10 iterations of updating ship location, 
        /// the location of current starting point should not be the same
        /// as the new random generated location vector after those 10 iterations
        /// </summary>
        [TestMethod]
        public void VerifyGeneratingRandomLocationsInUpdateShips()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, false);

            Ship germainShip = null;
            Vector2D currLoc = null;
            Star star = new Star(0, new Vector2D(375, 375), 35);

            for (int index = 1; index <= 10; index++)
            {
                germainShip = new Ship("Germain" + index, index, 5);
                currLoc = germainShip.Location();
                helloWorld.UpdateShip(germainShip);
                helloWorld.UpdateStars(star);
                Assert.AreNotEqual(currLoc, germainShip.Location());
            }
        }

        /// <summary>
        /// Verifies that out of the many ships in the game, there will be at least one
        /// where either the x coordinate or y coordinate or both are within the radius 
        /// of another ship
        /// </summary>
        [TestMethod]
        public void VerifyUpdateManyShipsWithXOrYCoordInRadiusOfOtherShipAndStar()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, false);

            Ship tonyShip = new Ship("Tony", 0, 5);
            Ship sonaShip = new Ship("Sona", 1, 5);
            Ship germainShip = new Ship("Germain", 2, 3);
            Ship koptaShip = new Ship("Kopta", 3, 4);
            Ship johnsonShip = new Ship("Johnson", 4, 3);

            List<Ship> allShips = new List<Ship>();
            allShips.Add(tonyShip);
            allShips.Add(sonaShip);
            allShips.Add(germainShip);
            allShips.Add(koptaShip);
            allShips.Add(johnsonShip);

            Star star = new Star(0, new Vector2D(180, 180), 35);
            helloWorld.UpdateStars(star);

            foreach (Ship ship in allShips)
            {
                ship.UpdateLocation(star.Location());
                helloWorld.UpdateShip(ship);                
                Assert.AreNotEqual(new Vector2D(0, 0), ship.Location());
            }
        }

        /// <summary>
        /// Verifies that the firing case where the number of projectiles frames
        /// is not the same as the ship's frames
        /// </summary>
        [TestMethod]
        public void VerifyShipFiringIncrementProjFramesWithShipFrame0()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, false);

            Ship tonyPlayer = new Ship("Tony", 0, 5);

            tonyPlayer.IncrementProjNumFrames();

            helloWorld.UpdateShip(tonyPlayer);
            helloWorld.Fire(tonyPlayer.ID());

            Assert.AreEqual(2, tonyPlayer.NumberOfProjFrames());
        }

        /// <summary>
        /// Verifies the firing case where both the projectile and ship frames; should
        /// reset to zero
        /// are the same
        /// </summary>
        [TestMethod]
        public void VerifyShipFiringIncrementProjAndShipFrames()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, false);

            Ship tonyPlayer = new Ship("Tony", 0, 5);

            for(int index = 0; index < 6; index++)
            {
                tonyPlayer.IncrementProjNumFrames();
            }

            tonyPlayer.IncrementShipFrames();

            helloWorld.UpdateShip(tonyPlayer);
            helloWorld.Fire(tonyPlayer.ID());

            Assert.IsTrue(tonyPlayer.NumberOfProjFrames() == 0);
        }

        /// <summary>
        /// Verifies that two ships are indeed colliding since they are within 
        /// one of each other's radii
        /// </summary>
        [TestMethod]
        public void VerifyShipToShipCollision()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, false);

            Ship tonyPlayer = new Ship("Tony", 0, 1);
            Ship sonaPlayer = new Ship("Sona", 1, 1);

            helloWorld.UpdateShip(tonyPlayer);
            helloWorld.UpdateShip(sonaPlayer);

            tonyPlayer.UpdateLocation(new Vector2D(100, 100));
            sonaPlayer.UpdateLocation(new Vector2D(100, 100));

            helloWorld.CollideShipToShip();

            Assert.IsTrue(tonyPlayer.HP() == 0 && sonaPlayer.HP() == 0);
        }

        /// <summary>
        /// Verifies that the projectile has updated its location by adding velocity and acceleration
        /// when extra feature mode is on
        /// </summary>
        [TestMethod]
        public void VerifyUpdatingProjectilesWithExtraModeOn()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, true);

            Vector2D loc = new Vector2D(100, 100);
            Vector2D dir = new Vector2D(1, 1);

            Vector2D velocity = new Vector2D(15, 15);
            Vector2D accel = new Vector2D(5, 5);

            Projectile proj = new Projectile(0, 0, loc, dir, new Vector2D(0, 0));
            proj.UpdateAcceleration(accel);

            helloWorld.UpdateProjectiles(proj);
            helloWorld.UpdateProjectileLocation(proj.ID());

            Assert.AreEqual(new Vector2D(105, 105), proj.Location());
        }

        /// <summary>
        /// Verifies that the projectile's acceleration is reset to zero
        /// </summary>
        [TestMethod]
        public void VerifyResetZeroAccelerationOnProjectile()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, true);

            Vector2D loc = new Vector2D(100, 100);
            Vector2D dir = new Vector2D(1, 1);

            Vector2D velocity = new Vector2D(15, 15);
            Vector2D accel = new Vector2D(5, 5);

            Projectile proj = new Projectile(0, 0, loc, dir, new Vector2D(0, 0));
            helloWorld.UpdateProjectiles(proj);

            helloWorld.ResetToZeroAccelerationProj(proj.ID());

            Assert.AreEqual(new Vector2D(0, 0), proj.Acceleration());
        }

        /// <summary>
        /// Verifies that the ship is killed
        /// </summary>
        [TestMethod]
        public void VerifyKillShip()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, true);

            Ship tonyPlayer = new Ship("Tony", 0, 1);

            helloWorld.UpdateShip(tonyPlayer);
            helloWorld.KillShip(tonyPlayer.ID());

            Assert.AreEqual(0, tonyPlayer.HP());
        }

        /// <summary>
        /// Verifies that the ship respawned at a different location after trying to 
        /// respawn within the star's radius
        /// </summary>
        [TestMethod]
        public void VerifyRespawningOnStar()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, true);

            Ship tonyPlayer = new Ship("Tony", 0, 1);

            Star star = new Star(0, new Vector2D(100, 100), 35);
            helloWorld.UpdateStars(star);

            helloWorld.UpdateShip(tonyPlayer);
            helloWorld.KillShip(tonyPlayer.ID());
            tonyPlayer.UpdateLocation(star.Location());
            helloWorld.RespawnShip(tonyPlayer.ID());

            Assert.IsTrue(tonyPlayer.Location() != star.Location());
        }

        /// <summary>
        /// Verify that the ship has gravity applied to it
        /// </summary>
        [TestMethod]
        public void VerifyApplyGravityToShip()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, true);

            Ship tonyPlayer = new Ship("Tony", 0, 1);

            Star star = new Star(0, new Vector2D(100, 100), 35);
            helloWorld.UpdateStars(star);
            helloWorld.UpdateShip(tonyPlayer);

            helloWorld.ApplyGravityToShip(tonyPlayer.ID());
            
            Assert.IsTrue(tonyPlayer.Location() != star.Location());
        }

        /// <summary>
        /// Verifies that the gravity acceleration was applied to the projectile
        /// with respect to the star
        /// </summary>
        [TestMethod]
        public void VerifyApplyGravityAccelerationToProjectile()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, true);

            Projectile proj = new Projectile(0, 0, new Vector2D(0, 0), new Vector2D(0, 0), new Vector2D(0, 0));

            Star star = new Star(0, new Vector2D(100, 100), 35);
            helloWorld.UpdateStars(star);

            helloWorld.UpdateProjectiles(proj);
            Vector2D gravity = new Vector2D(9.8, 9.8);

            helloWorld.ApplyGravityAccelerationToProjectile(gravity, proj.ID());

            Assert.IsTrue(proj.Location() != star.Location());
        }

        /// <summary>
        /// Verifies that the gravity was applied to the projectile
        /// </summary>
        [TestMethod]
        public void VerifyApplyGravityToProjectile()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, true);

            Projectile proj = new Projectile(0, 0, new Vector2D(0, 0), new Vector2D(0, 0), new Vector2D(0, 0));

            Star star = new Star(0, new Vector2D(100, 100), 35);
            helloWorld.UpdateStars(star);

            helloWorld.UpdateProjectiles(proj);
            Vector2D gravity = new Vector2D(9.8, 9.8);

            helloWorld.ApplyGravityAccelerationToProjectile(gravity, proj.ID());
            helloWorld.ApplyGravityToProjectile(proj.ID());

            Assert.IsTrue(proj.Location() != star.Location());
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void VerifyShipRespawnTwiceAfterOverlappingStar()
        {
            World helloWorld = new World(15.0, .08, 20, 35, 750, 6, 2.0, 300, 5, false);

            Ship tonyPlayer = new Ship("Tony", 1, 1);
            tonyPlayer.UpdateLocation(new Vector2D(100, 100));
            tonyPlayer.UpdateVelocity(new Vector2D(15, 15));

            helloWorld.UpdateShip(tonyPlayer);
            helloWorld.KillShip(tonyPlayer.ID());
          

            helloWorld.RespawnShip(tonyPlayer.ID());

            Assert.IsTrue(tonyPlayer.HP() == 5);
        }
    }
}
