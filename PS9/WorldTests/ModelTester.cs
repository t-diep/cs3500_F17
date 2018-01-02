using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Model;
using System.Linq;

namespace WorldTests
{
    [TestClass]
    public class ModelTester
    {
        [TestMethod]
        public void TestConstructor()
        {
            int world_size = 120;
            List<Star> stars = new List<Star>();
            int respawnrate = 7;
            bool movestars = false;
            int firerate = 4;
            World world = new World(world_size, stars, respawnrate, firerate, movestars);

            Assert.AreEqual(world.GetAllShips().GetEnumerator().Current, null);
        }

        [TestMethod]
        public void TestGetAllStars()
        {
            int world_size = 120;
            List<Star> stars = new List<Star>();
            stars.Add(new Star());
            int respawnrate = 7;
            bool movestars = false;
            int firerate = 4;
            World world = new World(world_size, stars, respawnrate, firerate, movestars);

            Assert.AreEqual(world.GetAllShips().GetEnumerator().Current, null);
        }
        [TestMethod]
        public void TestAddShip()
        {
            int world_size = 120;
            List<Star> stars = new List<Star>();
            stars.Add(new Star());
            int respawnrate = 7;
            bool movestars = false;
            int firerate = 4;
            World world = new World(world_size, stars, respawnrate, firerate, movestars);
            world.AddShip(4, null);

            Assert.AreEqual(world.GetAllShips().GetEnumerator().MoveNext(), false);
        }

        [TestMethod]
        public void TestValidAddShip()
        {
            int world_size = 120;
            List<Star> stars = new List<Star>();
            stars.Add(new Star());
            int respawnrate = 7;
            bool movestars = false;
            int firerate = 4;
            World world = new World(world_size, stars, respawnrate, firerate, movestars);
            world.AddShip(4, new Ship());

            Assert.AreEqual(world.GetAllShips().GetEnumerator().MoveNext(), true);
        }


        [TestMethod]
        public void TestAddProj()
        {
            int world_size = 120;
            List<Star> stars = new List<Star>();
            stars.Add(new Star());
            int respawnrate = 7;
            bool movestars = false;
            int firerate = 4;
            World world = new World(world_size, stars, respawnrate, firerate, movestars);
            world.AddProj(4, null);

            Assert.AreEqual(world.GetAllProjectiles().GetEnumerator().MoveNext(), false);
        }

        [TestMethod]
        public void TestValidAddProj()
        {
            int world_size = 120;
            List<Star> stars = new List<Star>();
            stars.Add(new Star());
            int respawnrate = 7;
            bool movestars = false;
            int firerate = 4;
            World world = new World(world_size, stars, respawnrate, firerate, movestars);
            world.AddProj(4, new Projectile());

            Assert.AreEqual(world.GetAllProjectiles().GetEnumerator().MoveNext(), true);
        }
        [TestMethod]
        public void TestAddStar()
        {
            int world_size = 120;
            List<Star> stars = new List<Star>();
            stars.Add(new Star());
            int respawnrate = 7;
            bool movestars = false;
            int firerate = 4;
            World world = new World(world_size, stars, respawnrate, firerate, movestars);
            world.AddStar(4, null);

            Assert.AreEqual(world.GetAllStars().GetEnumerator().MoveNext(), true);
        }

        [TestMethod]
        public void TestValidAddStar()
        {
            int world_size = 120;
            List<Star> stars = new List<Star>();
            stars.Add(new Star());
            int respawnrate = 7;
            bool movestars = false;
            int firerate = 4;
            World world = new World(world_size, stars, respawnrate, firerate, movestars);
            world.AddStar(4, new Star());

            Assert.AreEqual(world.GetAllStars().GetEnumerator().MoveNext(), true);
        }

        [TestMethod]
        public void TestValidRemoveProj()
        {
            int world_size = 120;
            List<Star> stars = new List<Star>();
            stars.Add(new Star());
            int respawnrate = 7;
            bool movestars = false;
            int firerate = 4;
            World world = new World(world_size, stars, respawnrate, firerate, movestars);
            world.AddProj(4, new Projectile());
            world.RemoveProj(4);

            Assert.AreEqual(world.GetAllProjectiles().GetEnumerator().MoveNext(), false);
        }

        [TestMethod]
        public void TestValidRemoveShip()
        {
            int world_size = 120;
            List<Star> stars = new List<Star>();
            stars.Add(new Star());
            int respawnrate = 7;
            bool movestars = false;
            int firerate = 4;
            World world = new World(world_size, stars, respawnrate, firerate, movestars);
            world.AddShip(4, new Ship());
            world.RemoveShip(4);
          

            Assert.AreEqual(world.GetAllShips().Count(), 0);
        }

        [TestMethod]
        public void TestAddRandomShip()
        {
            int world_size = 120;
            List<Star> stars = new List<Star>();
            stars.Add(new Star());
            int respawnrate = 7;
            bool movestars = false;
            int firerate = 4;
            World world = new World(world_size, stars, respawnrate, firerate, movestars);
            for (int i = 0; i < 10; i++)
            {
                world.AddRandomShip("sam" + i);
            }

            Assert.AreEqual(world.GetAllShips().GetEnumerator().MoveNext(), true);
        }

        [TestMethod]
        public void TestKillShips()
        {
            int world_size = 120;
            List<Star> stars = new List<Star>();
            stars.Add(new Star());
            int respawnrate = 7;
            bool movestars = false;
            int firerate = 4;
            World world = new World(world_size, stars, respawnrate, firerate, movestars);
            for (int i = 0; i < 10; i++)
            {
                world.AddRandomShip("sam" + i);
            }

            for (int i = 0; i < 10; i++)
            {
                world.KillShipForever(i);
            }

            world.GetAllShips().GetEnumerator().MoveNext(); 

            Assert.AreEqual(world.GetAllShips().GetEnumerator().Current, null);
        }

        [TestMethod]
        public void TestProjID()
        {
            Random rand = new Random();
            Projectile proj = new Projectile(1, new Vector2D(), new Vector2D());
            Assert.AreEqual(proj.GetID(), 0);
        }

        [TestMethod]
        public void TestProjOwner()
        {
            Random rand = new Random();
            Projectile proj = new Projectile(1, new Vector2D(), new Vector2D());
            Assert.AreEqual(proj.GetID(), 1);
        }

        [TestMethod]
        public void TestProjDefaultOwner()
        {
            Random rand = new Random();
            Projectile proj = new Projectile();
            Assert.AreEqual(proj.GetID(), 0);
        }

        [TestMethod]
        public void TestProjDirection()
        {
            Random rand = new Random();
            Projectile proj = new Projectile(1, new Vector2D(), new Vector2D());
            Assert.AreEqual(proj.GetAlive(), true);
        }

        [TestMethod]
        public void TestProjGetDirection()
        {
            Random rand = new Random();
            Projectile proj = new Projectile(1, new Vector2D(), new Vector2D());
            Assert.AreEqual(proj.GetDirection(), new Vector2D());
        }
        [TestMethod]
        public void TestProjGetLocation()
        {
            Random rand = new Random();
            Projectile proj = new Projectile(1, new Vector2D(), new Vector2D());
            Assert.AreEqual(proj.GetLocation(), new Vector2D());
        }

        [TestMethod]
        public void TestGetOwner()
        {
            Projectile proj = new Projectile();
            Assert.AreEqual(proj.GetOwner(), -1);
        }

        [TestMethod]
        public void TestProjSetDirection()
        {
            Projectile proj = new Projectile(1, new Vector2D(), new Vector2D());
            proj.SetLocation(null);
            Vector2D v = new Vector2D();
            
            Assert.AreEqual(proj.GetLocation().GetX(), -1);
        }

        [TestMethod]
        public void TestProjSetLoc()
        {
            
            Projectile proj = new Projectile(1, new Vector2D(), new Vector2D());
            proj.SetDirection(null);
            Assert.AreEqual(proj.GetDirection(), new Vector2D());
        }
        [TestMethod]
        public void TestProjDeath()
        {
            Projectile p = new Projectile();
            p.Death();
            Assert.AreEqual(p.GetAlive(), false);
        }

        [TestMethod]
        public void TestKillShipForever()
        {
            World world = new World();
            Ship ship = new Ship();
            int ID = ship.GetID();
            world.AddShip(ID, ship);
            world.KillShipForever(ID);

            foreach (Ship pShip in world.GetAllShips())
                Assert.AreEqual(pShip.GetHP(), -1);
        }
        [TestMethod]
        public void TestShipConstructor()
        {
            Ship s = new Ship("test", 1);
            Assert.AreNotEqual(s.GetName(), null);
        }

        [TestMethod]
        public void TestShipGetHP()
        {
            Ship s = new Ship("test", 1);
            Assert.AreEqual(s.GetHP(), 5);
        }
        [TestMethod]
        public void TestShipGetScore()
        {
            Ship s = new Ship("test", 1);
            Assert.AreNotEqual(s.GetScore(), 1);
        }
        [TestMethod]
        public void TestShipGetLocation()
        {
            Ship s = new Ship("test", 1);
            Assert.AreEqual(s.GetLocation(), new Vector2D(-1, -1));
        }
        [TestMethod]
        public void TestShipGetDirection()
        {
            Ship s = new Ship("test", 1);
            Assert.AreEqual(s.GetDirection(), new Vector2D(-1, -1));
        }
        [TestMethod]
        public void TestShipGetVelocity()
        {
            Ship s = new Ship("test", 1);
            Assert.AreNotEqual(s.GetVelocity(), new Vector2D());
        }

        [TestMethod]
        public void TestShipGetThrust()
        {
            Ship s = new Ship("test", 1);
            Assert.AreEqual(s.GetThrust(), false);
        }

        [TestMethod]
        public void TestShipSetHP()
        {
            Ship s = new Ship("test", 1);
            s.SetHP(4);
            Assert.AreNotEqual(s.GetHP(), 5);
        }

        [TestMethod]
        public void TestShipSetLocation()
        {
            Ship s = new Ship("test", 1);
            s.SetLocation(new Vector2D(0, -1));
            Assert.AreNotEqual(s.GetLocation(), new Vector2D(-1, -1));
        }
        [TestMethod]
        public void TestShipSetDirection()
        {
            Ship s = new Ship("test", 1);
            s.SetDirection(new Vector2D(1, 1));
            Assert.AreNotEqual(s.GetDirection(), new Vector2D(0, 0));
        }

        [TestMethod]
        public void TestShipSetVelocity()
        {
            Ship s = new Ship("test", 4);
            s.SetVelocity(new Vector2D(1, 3));
            Assert.AreEqual(s.GetDirection(), new Vector2D(-1, -1));
        }

        [TestMethod]
        public void TestShipSetLastFired()
        {
            Ship s = new Ship("test", 12);
            s.SetLastFired(100);
            Assert.AreEqual(s.GetLastDeath(), 0);
        }
        [TestMethod]
        public void TestSetLastDeath()
        {
            Ship s = new Ship("test", 4);
            s.SetLastFired(12);
            Assert.AreEqual(s.GetLastDeath(), 0);
        }
        [TestMethod]
        public void TestShipIncrementScore()
        {
            Ship s = new Ship("test", 12);
            s.IncrementScore();
            Assert.AreEqual(1, s.GetScore());
        }
        [TestMethod]
        public void TestShipDecrementHP()
        {
            Ship s = new Ship("test", 12);
            s.DecrementHP();
            Assert.AreEqual(4, s.GetHP());
        }

        [TestMethod]
        public void TestShipDecrementLastFired()
        {
            Ship s = new Ship("test", 12);
            s.SetLastFired(1);
            s.DecrementLastFired();
            Assert.AreEqual(0, s.GetLastFired());
        }

        [TestMethod]
        public void TestShipDecrementLastDeath()
        {
            Ship s = new Ship("test", 12);
            s.SetLastDeath(1);
            s.DecrementLastDeath();
            Assert.AreEqual(0, s.GetLastDeath());
        }

        [TestMethod]
        public void TestShipWrapAroundX()
        {
            Ship s = new Ship("test", 1);
            s.SetLocation(new Vector2D(1,1));
            s.WrapAroundX();
            Assert.AreEqual(s.GetLocation(), new Vector2D(-1, 1));

        }

        [TestMethod]
        public void TestShipWrapAroundY()
        {
            Ship s = new Ship("test", 1);
            s.SetLocation(new Vector2D(1, 1));
            s.WrapAroundY();
            Assert.AreEqual(s.GetLocation(), new Vector2D(1, -1));
        }

        [TestMethod]
        public void TestShipRespawn()
        {
            Vector2D v = new Vector2D(1, 1);
            Ship s = new Ship("test", 1);
            s.Respawn(v);
            Assert.AreEqual(v, s.GetLocation());
        }

        [TestMethod]
        public void TestShipToString()
        {
            Ship s = new Ship("test", 1);
            // { "ship":1,"name":"test","thrust":false,"hp":5,"score":0,"loc":{ "x":-1.0,"y":-1.0},"dir":{ "x":-1.0,"y":-1.0} }
            
            string output = "{\"ship\":1,\"name\":\"test\",thrust\":false,\"hp\":5,\"score\":0,\"loc\":{\"x\":-1.0,\"y\":-1.0},\"dir\":{\"x\":-1.0,\"y\":-1.0}";
            Assert.AreNotEqual(s.ToString(), output);
        }

        [TestMethod]
        public void TestStarGetMass()
        {
            Star s = new Star(new Vector2D(1.0, 0.0), 100d);
            Assert.AreEqual(s.GetMass(), 100d);
        }

        [TestMethod]
        public void TestCounter()
        {
            Star s = new Star(new Vector2D(1.0, 0.0), 100d);
            s.SetCounter(20);
            s.DecrementCounter();
            Assert.AreEqual(s.GetCounter(), 19);
        }

        [TestMethod]
        public void TestStarSetLocation()
        {
            Star s = new Star(new Vector2D(1.0, 0.0), 100d);
            s.SetLocation(new Vector2D(3.0, 4.0));
            Assert.AreEqual(s.GetLocation().ToString(), "(3,4)");
        }

        [TestMethod]
        public void TestStarToString()
        {
            Star s = new Star(new Vector2D(1.0, 0.0), 100d);
            string output = "{\"star\":0,\"loc\":{\"x\":1.0,\"y\":0.0},\"mass\":100.0}";

            Assert.AreEqual(s.ToString(), output);
        }

    }
}
