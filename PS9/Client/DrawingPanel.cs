using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Model;

namespace View
{ 
    public class DrawingPanel : Panel
    {
        private World theWorld = new World();
        private static object myLock = new object();

        public DrawingPanel(World w)
        {
            DoubleBuffered = true;
            theWorld = w;
        }

        private static int WorldSpaceToImageSpace(int size, double w)
        {
            return (int)w + size / 2;
        }

        /// <summary>
        /// A delegate for DrawObjectsWithTransform
        /// Methods matching this delegate can draw whatever they want using e
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        public delegate void ObjectDrawer(object o, PaintEventArgs e);


        /// <summary>
        /// This method performs a translation and rotation to drawn an object in the world.
        /// </summary>
        /// <param name="e">PaintEventArgs to access the graphics (for drawing)</param>
        /// <param name="o">The object to draw</param>
        /// <param name="worldSize">The size of one edge of the world (assuming the world is square)</param>
        /// <param name="worldX">The X coordinate of the object in world space</param>
        /// <param name="worldY">The Y coordinate of the object in world space</param>
        /// <param name="angle">The orientation of the objec, measured in degrees clockwise from "up"</param>
        /// <param name="drawer">The drawer delegate. After the transformation is applied, the delegate is invoked to draw whatever it wants</param>
        private void DrawObjectWithTransform(PaintEventArgs e, object o, int worldSize, double worldX, double worldY, double angle, ObjectDrawer drawer)
        {
            // Perform the transformation
            int x = WorldSpaceToImageSpace(worldSize, worldX);
            int y = WorldSpaceToImageSpace(worldSize, worldY);
            e.Graphics.TranslateTransform(x, y);
            e.Graphics.RotateTransform((float)angle);
            // Draw the object 
            drawer(o, e);
            // Then undo the transformation
            e.Graphics.ResetTransform();
        }

        /// <summary>
        /// Used to draw a Ship object. Used as delegate for DrawOjectWithTransform
        /// </summary>
        /// <param name="o">The Ship to draw</param>
        /// <param name="e">The PaintEventArgs to access the graphics</param>
        private void ShipDrawer(object o, PaintEventArgs e)
        {
            Ship pShip = o as Ship;

            //Do not draw the ship if it has zero health
            if (pShip.GetHP() == 0)
                return;

            //Retrieve the ship's image
            Image shipImage = RetrieveImage(pShip);

            //The desired demensions of the ship
            int width = 40;
            int height = 40;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            //Use rectangle to represent the Ship's location when drawing it
            Rectangle r = new Rectangle(-(width / 2), -(height / 2), width, height);

            //Draw the ship given its image
            e.Graphics.DrawImage(shipImage, r);
        }

        /// <summary>
        /// Used to draw a Projectile object. Used as delegate for DrawOjectWithTransform
        /// </summary>
        /// <param name="o">The Projectile to draw</param>
        /// <param name="e">The PaintEventArgs to access the graphics</param>
        private void ProjectileDrawer(object o, PaintEventArgs e)
        {
            Projectile pProj = o as Projectile;

            //Retrieve the projectile's image
            Image projImage = RetrieveImage(pProj);

            //The desired demensions of the projectile
            int width = 10;
            int height = 20;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            //Use rectangle to represent the projectile's location when drawing it
            Rectangle r = new Rectangle(-(width / 2), -(height / 2), width, height);

            //Draw the projectile given its image
            e.Graphics.DrawImage(projImage, r);
        }

        /// <summary>
        /// Used to draw a Star object. Used as delegate for DrawOjectWithTransform
        /// </summary>
        /// <param name="o">The Star to draw</param>
        /// <param name="e">The PaintEventArgs to access the graphics</param>
        private void StarDrawer(object o, PaintEventArgs e)
        {
            Star pStar = o as Star;

            //Retrieve the star's image
            Image starImage = RetrieveImage(pStar);

            //The desired demensions of the star
            int width = 55;
            int height = 55;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            //Use rectangle to represent the Stars's location when drawing it
            Rectangle r = new Rectangle(-(width / 2), -(height / 2), width, height);

            //Draw the star given its image
            e.Graphics.DrawImage(starImage, r);
        }

        /// <summary>
        /// A helper to the RetrieveImage method. This retrieves the 
        /// color of the player given the player's ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private string RetrieveSpriteName(int ID)
        {
            string fileName;

            //If all images are already being used, pick a random image
            if(ID > 8)
            {
                Random randNum = new Random();
                ID = randNum.Next(1, 9);
            }

            //Decide on the color of the object based on its ID
            switch (ID)
            {
                case 1:
                    fileName = "blue.png";
                    break;
                case 2:
                    fileName = "brown.png";
                    break;
                case 3:
                    fileName = "green.png";
                    break;
                case 4:
                    fileName = "grey.png";
                    break;
                case 5:
                    fileName = "red.png";
                    break;
                case 6:
                    fileName = "violet.png";
                    break;
                case 7:
                    fileName = "white.png";
                    break;
                case 8:
                    fileName = "yellow.png";
                    break;
                default:
                    fileName = "white.png";
                    break;

            }
            return fileName;
        }

        /// <summary>
        /// Finds and returns image from given filepath for the game
        /// </summary>
        /// <param name="playerShip"></param>
        /// <returns></returns>
        private Image RetrieveImage(Ship playerShip)
        {
            string filePath = "..\\..\\..\\Resources\\Sprites\\";
            string fileName = "";
            string color = RetrieveSpriteName(playerShip.GetID());

            //Pick the image depending whether the player is using the ship's thrusters
            if (!playerShip.GetThrust())
            {
                fileName = string.Concat("ship-coast-", color);
            }
            else
            {
                fileName = string.Concat("ship-thrust-", color);
            }

            //Construct the full filepath and retreive the image
            string fullFilePath = string.Concat(filePath, fileName);

            Image shipImage = Image.FromFile(fullFilePath);

            return shipImage;

        }

        /// <summary>
        /// Finds and returns image from given filepath for the game
        /// </summary>
        /// <param name="playerProjectile"></param>
        /// <returns></returns>
        private Image RetrieveImage(Projectile playerProjectile)
        {
            string filePath = "..\\..\\..\\Resources\\Sprites\\";
            string color = RetrieveSpriteName(playerProjectile.GetOwner());
            string fileName = string.Concat("shot-", color);

            //Contruct the full filepath and retreive the image
            string fullFilePath = string.Concat(filePath, fileName);

            Image projImage = Image.FromFile(fullFilePath);

            return projImage;

        }

        /// <summary>
        /// Finds and returns image from given filepath for the game
        /// </summary>
        /// <param name="playerStar"></param>
        /// <returns></returns>
        private Image RetrieveImage(Star playerStar)
        {
            //There is only one star, so we can just specify its filepath
            string fullFilePath = "..\\..\\..\\Resources\\Sprites\\star.jpg";

            Image starImage = Image.FromFile(fullFilePath);

            return starImage;

        }

        // This method is invoked when the DrawingPanel needs to be re-drawn
        protected override void OnPaint(PaintEventArgs e)
        {
            
            //Use try-catch to prevent exception when user moves window
            try
            {
                //Iterate through all the players and draw them
                foreach (Ship play in theWorld.GetAllShips())
                {
                    lock (myLock)
                    {
                        DrawObjectWithTransform(e, play, this.Size.Width, play.GetLocation().GetX(),
                            play.GetLocation().GetY(), play.GetDirection().ToAngle(), ShipDrawer);
                    }
                }

                //Iterate through all the projectiles and draw them
                foreach (Projectile proj in theWorld.GetAllProjectiles())
                {
                    lock (myLock)
                    {
                        DrawObjectWithTransform(e, proj, this.Size.Width, proj.GetLocation().GetX(),
                            proj.GetLocation().GetY(), proj.GetDirection().ToAngle(), ProjectileDrawer);
                    }
                }

                //Iterate through all the Stars and draw them
                foreach (Star pStar in theWorld.GetAllStars())
                {
                    lock (myLock)
                    {
                        DrawObjectWithTransform(e, pStar, this.Size.Width, pStar.GetLocation().GetX(),
                            pStar.GetLocation().GetY(), 0.0F, StarDrawer);
                    }
                }
            }
            catch (Exception) { }

            // Do anything that Panel (from which we inherit) needs to do
            base.OnPaint(e);
            
        }
        

    }
}