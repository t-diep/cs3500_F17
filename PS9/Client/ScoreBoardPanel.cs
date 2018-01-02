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
    /// <summary>
    /// The panel that displays the score, HP, and name of the player
    /// </summary>
    public class ScoreBoardPanel : Panel
    {
        private World theWorld = new World();
        private static object myLock = new object();

        public ScoreBoardPanel(World w)
        {
            DoubleBuffered = true;
            theWorld = w;
        }

        /// <summary>
        /// Used to draw a string inside of the panel
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        /// <param name="y"></param>
        private void StringDrawer(object o, PaintEventArgs e, int y)
        {
            Ship p = o as Ship;

            //The text that will be displayed on the scoreboard panel
            string drawString = string.Format("{0} -- Score: {1}  HP: {2}", p.GetName(), p.GetScore(), p.GetHP());

            // Create font and brush.
            Font drawFont = new Font("Arial", 12);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            Point drawPoint = new Point(10, y);

            e.Graphics.DrawString(drawString, drawFont, drawBrush, drawPoint);
        }

        /// <summary>
        /// Invoked when Scoreboard needs to be redrawn
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            //Used to add spacing between each player's stats
            int yCounter = 15;

            //Use try-catch to prevent exception when user moves window
            try
            {
                //Iterate through all the players and draw them
                foreach (Ship play in theWorld.GetAllShips())
                {
                    lock (myLock)
                    {
                        StringDrawer(play, e, yCounter);

                        //Increase the counter every time a player is added
                        yCounter += 30;
                    }
                }
            }
            catch (InvalidOperationException) { }

            // Do anything that Panel (from which we inherit) needs to do
            base.OnPaint(e);

        }
    }
}
