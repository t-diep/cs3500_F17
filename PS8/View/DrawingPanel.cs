///
/// @authors Tony Diep and Sona Torosyan
///
using Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// Provides the drawing panel used to put on our Spacewars GUI
/// </summary>
namespace View
{
    /// <summary>
    /// Represents the drawing panel for our Spacewars GUI
    /// </summary>
    internal class DrawingPanel : Panel
    {
        //The current world
        private World theWorld;

        //Holds all of the ship coasting images
        private Image shipCoastBlue;
        private Image shipCoastBrown;
        private Image shipCoastGrey;
        private Image shipCoastGreen;
        private Image shipCoastRed;
        private Image shipCoastViolet;
        private Image shipCoastWhite;
        private Image shipCoastYellow;

        //Holds all of the ship thrusting images
        private Image shipThrustBlue;
        private Image shipThrustBrown;
        private Image shipThrustGrey;
        private Image shipThrustGreen;
        private Image shipThrustRed;
        private Image shipThrustViolet;
        private Image shipThrustWhite;
        private Image shipThrustYellow;

        //Holds all of the projectile images
        private Image projBlue;
        private Image projBrown;
        private Image projGrey;
        private Image projGreen;
        private Image projRed;
        private Image projViolet;
        private Image projWhite;
        private Image projYellow;

        //Holds the star image
        private Image theStar;

        //Provides coordinates for the ship
        private Rectangle shipRect;

        //Provides coordinates for the projectile
        private Rectangle projRect;

        //Provides coordinates for the star
        private Rectangle starRect;

        /// <summary>
        /// Construct the drawing panel 
        /// </summary>
        /// <param name="theWorld">the current world in which to draw</param>
        public DrawingPanel(World theWorld)
        {
            this.theWorld = theWorld;
            this.DoubleBuffered = true;

            //Create ship coast images
            shipCoastBlue = Image.FromFile("../../../Resources/Images/ship-coast-blue.png");
            shipCoastBrown = Image.FromFile("../../../Resources/Images/ship-coast-brown.png");
            shipCoastGrey = Image.FromFile("../../../Resources/Images/ship-coast-grey.png");
            shipCoastGreen = Image.FromFile("../../../Resources/Images/ship-coast-green.png");
            shipCoastRed = Image.FromFile("../../../Resources/Images/ship-coast-red.png");
            shipCoastViolet = Image.FromFile("../../../Resources/Images/ship-coast-violet.png");
            shipCoastWhite = Image.FromFile("../../../Resources/Images/ship-coast-white.png");
            shipCoastYellow = Image.FromFile("../../../Resources/Images/ship-coast-yellow.png");

            //Create ship thrust images
            shipThrustBlue = Image.FromFile("../../../Resources/Images/ship-thrust-blue.png");
            shipThrustBrown = Image.FromFile("../../../Resources/Images/ship-thrust-brown.png");
            shipThrustGrey = Image.FromFile("../../../Resources/Images/ship-thrust-grey.png");
            shipThrustGreen = Image.FromFile("../../../Resources/Images/ship-thrust-green.png");
            shipThrustRed = Image.FromFile("../../../Resources/Images/ship-thrust-red.png");
            shipThrustViolet = Image.FromFile("../../../Resources/Images/ship-thrust-violet.png");
            shipThrustWhite = Image.FromFile("../../../Resources/Images/ship-thrust-white.png");
            shipThrustYellow = Image.FromFile("../../../Resources/Images/ship-thrust-yellow.png");

            //Create star image
            theStar = Image.FromFile("../../../Resources/Images/star.jpg");

            //Create projectile images
            projBlue = Image.FromFile("../../../Resources/Images/shot-blue.png");
            projBrown = Image.FromFile("../../../Resources/Images/shot-brown.png");
            projGrey = Image.FromFile("../../../Resources/Images/shot-grey.png");
            projGreen = Image.FromFile("../../../Resources/Images/shot-green.png");
            projRed = Image.FromFile("../../../Resources/Images/shot-red.png");
            projViolet = Image.FromFile("../../../Resources/Images/shot-violet.png");
            projWhite = Image.FromFile("../../../Resources/Images/shot-white.png");
            projYellow = Image.FromFile("../../../Resources/Images/shot-yellow.png");

            //Create the coordinates for ship
            shipRect = new Rectangle(-15, -15, 30, 30);

            //Create the coordinates for projectile
            projRect = new Rectangle(-10, -10, 20, 20);

            //Create the coordinates for star
            starRect = new Rectangle(-15, -15, 30, 30);
        }

        /// <summary>
        /// Represents what object specifically to draw
        /// </summary>
        /// <param name="obj">an object we would like to draw</param>
        /// <param name="paintEventArgs">paint event argument</param>
        public delegate void ObjectDrawer(object obj, PaintEventArgs paintEventArgs);

        /// <summary>
        /// Helps convert from world coordinates to image coordinates, 
        /// in which we can see the sprites on view
        /// </summary>
        /// <param name="size">size of the current world</param>
        /// <param name="coordinate">the current coordinate from that world</param>
        /// <returns></returns>
        private static int WorldSpaceToImageSpace(int size, double coordinate)
        {
            return (int)coordinate + size / 2;
        }

        /// <summary>
        /// Helps draw an object while giving some transforming properties
        /// </summary>
        /// <param name="paintEventArgs">paint event argument</param>
        /// <param name="obj">object in which we want to draw</param>
        /// <param name="worldSize">size of the current world</param>
        /// <param name="worldX">x coordinate of world</param>
        /// <param name="worldY">y coordinate of world</param>
        /// <param name="angle">angle in which to draw</param>
        /// <param name="drawer">delegate that draws an object we want to draw</param>
        private void DrawObjectWithTransform(PaintEventArgs paintEventArgs, object obj, int worldSize,
            double worldX, double worldY, double angle, ObjectDrawer drawer)
        {
            //Get the converted x and y image coordinates
            int x = WorldSpaceToImageSpace(worldSize, worldX);
            int y = WorldSpaceToImageSpace(worldSize, worldY);

            //Apply transformations and rotations to give object an edge
            paintEventArgs.Graphics.TranslateTransform(x, y);
            paintEventArgs.Graphics.RotateTransform((float)angle);

            //Activate object drawer delegate
            drawer(obj, paintEventArgs);

            //Discard recent transformations and prepare for next object to apply transformations
            paintEventArgs.Graphics.ResetTransform();
        }


        /// <summary>
        /// Helps draw a ship player
        /// </summary>
        /// <param name="o">The object to draw</param>
        /// <param name="e">The PaintEventArgs to access the graphics</param>
        private void ShipPlayerDrawer(object o, PaintEventArgs e)
        {
            //Convert object to a ship
            Ship ship = o as Ship;

            if (ship.IsDead())
            {
                return;
            }

            //Generate unique ID for drawing different ships
            int shipID = ship.ID() % 8;

            //Draw thrusting ship pictures
            if (ship.IsThrusting())
            {
                switch (shipID)
                {
                    case 0:
                        e.Graphics.DrawImage(shipThrustBlue, shipRect);
                        break;
                    case 1:
                        e.Graphics.DrawImage(shipThrustBrown, shipRect);
                        break;
                    case 2:
                        e.Graphics.DrawImage(shipThrustGrey, shipRect);
                        break;
                    case 3:
                        e.Graphics.DrawImage(shipThrustGreen, shipRect);
                        break;
                    case 4:
                        e.Graphics.DrawImage(shipThrustRed, shipRect);
                        break;
                    case 5:
                        e.Graphics.DrawImage(shipThrustViolet, shipRect);
                        break;
                    case 6:
                        e.Graphics.DrawImage(shipThrustWhite, shipRect);
                        break;
                    case 7:
                        e.Graphics.DrawImage(shipThrustYellow, shipRect);
                        break;
                }
            }
            //Draw coasting ship images
            else
            {
                switch (shipID)
                {
                    case 0:
                        e.Graphics.DrawImage(shipCoastBlue, shipRect);
                        break;
                    case 1:
                        e.Graphics.DrawImage(shipCoastBrown, shipRect);
                        break;
                    case 2:
                        e.Graphics.DrawImage(shipCoastGrey, shipRect);
                        break;
                    case 3:
                        e.Graphics.DrawImage(shipCoastGreen, shipRect);
                        break;
                    case 4:
                        e.Graphics.DrawImage(shipCoastRed, shipRect);
                        break;
                    case 5:
                        e.Graphics.DrawImage(shipCoastViolet, shipRect);
                        break;
                    case 6:
                        e.Graphics.DrawImage(shipCoastWhite, shipRect);
                        break;
                    case 7:
                        e.Graphics.DrawImage(shipCoastYellow, shipRect);
                        break;
                }
            }
        }

        /// <summary>
        /// Helps draw the star
        /// </summary>
        /// <param name="o">The object to draw</param>
        /// <param name="e">The PaintEventArgs to access the graphics</param>
        private void StarDrawer(object o, PaintEventArgs e)
        {
            //Convert object to a star
            Star star = o as Star;

            e.Graphics.DrawImage(theStar, starRect);
        }

        /// <summary>
        /// Helps draw the star
        /// </summary>
        /// <param name="o">The object to draw</param>
        /// <param name="e">The PaintEventArgs to access the graphics</param>
        private void ProjectileDrawer(object o, PaintEventArgs e)
        {
            //Convert object to a Projectile
            Projectile projectile = o as Projectile;

            //Generate unique ID to draw specific color for the projectile
            int projectileID = projectile.Owner() % 8;

            //Projectile is alive, so proceed to draw it with its respective color
            if (projectile.Alive())
            {
                //Draw the projectiles with respective color depending on modulus result
                switch (projectileID)
                {
                    case 0:
                        e.Graphics.DrawImage(projBlue, projRect);
                        break;
                    case 1:
                        e.Graphics.DrawImage(projBrown, projRect);
                        break;
                    case 2:
                        e.Graphics.DrawImage(projGrey, projRect);
                        break;
                    case 3:
                        e.Graphics.DrawImage(projGreen, projRect);
                        break;
                    case 4:
                        e.Graphics.DrawImage(projRed, projRect);
                        break;
                    case 5:
                        e.Graphics.DrawImage(projViolet, projRect);
                        break;
                    case 6:
                        e.Graphics.DrawImage(projWhite, projRect);
                        break;
                    case 7:
                        e.Graphics.DrawImage(projYellow, projRect);
                        break;
                }
            }
        }

        /// <summary>
        /// This method is invoked when the DrawingPanel needs to be re-drawn
        /// </summary>
        /// <param name="e">the paint argument that will automatically call this method</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            lock (theWorld)
            {
                // Draw the ships
                foreach (Ship ship in theWorld.Ships().Values)
                {
                    //System.Diagnostics.Debug.WriteLine("drawing player at " + play.GetLocation());
                    DrawObjectWithTransform(e, ship, this.Size.Width, ship.Location().GetX(), ship.Location().GetY(), ship.Direction().ToAngle(), ShipPlayerDrawer);
                }

                // Draw the stars
                foreach (Star star in theWorld.Stars().Values)
                {
                    //System.Diagnostics.Debug.WriteLine("drawing powerup at " + p.GetLocation());
                    DrawObjectWithTransform(e, star, this.Size.Width, star.Location().GetX(), star.Location().GetY(), 0, StarDrawer);
                }

                // Draw the projectiles
                foreach (Model.Projectile projectile in theWorld.Projectiles().Values)
                {
                    //System.Diagnostics.Debug.WriteLine("drawing powerup at " + p.GetLocation());
                    DrawObjectWithTransform(e, projectile, this.Size.Width, projectile.Location().GetX(), projectile.Location().GetY(), 0, ProjectileDrawer);
                }
            }

            // Do anything that Panel (from which we inherit) needs to do
            base.OnPaint(e);
        }
    }
}