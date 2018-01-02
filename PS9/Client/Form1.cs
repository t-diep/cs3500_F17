using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Timers;
using Controller;
using Model;
using System.Text.RegularExpressions;
using View;

namespace Client
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// The socket used to communicate with the server
        /// </summary>
        private Socket theServer;

        /// <summary>
        /// Stores all of the objects contained inside of the game
        /// </summary>
        private World theWorld;

        /// <summary>
        /// Stores the two values that are given when first connected
        /// to the server: the player's ID (index 0) and the world size (index 1)
        /// </summary>
        private List<int> startupValues;

        /// <summary>
        /// Used to draw the world
        /// </summary>
        private DrawingPanel drawingPanel;

        /// <summary>
        /// Used to display the player leaderboard
        /// </summary>
        private ScoreBoardPanel scoreBoardPanel;

        /// <summary>
        /// Whether the client has successfully connected to the server
        /// </summary>
        private bool isConnected = false;

        public Form1()
        {
            InitializeComponent();

            theWorld = new World();
            startupValues = new List<int>();
            drawingPanel = new DrawingPanel(theWorld);
            scoreBoardPanel = new ScoreBoardPanel(theWorld);
            this.Controls.Add(drawingPanel);
            this.Controls.Add(scoreBoardPanel);
            drawingPanel.BackColor = System.Drawing.Color.Black;
            scoreBoardPanel.BackColor = System.Drawing.Color.White;
            AdjustWorldSize();
            

            // Start a new timer that will redraw the game every 15 milliseconds 
            // This should correspond to about 67 frames per second.
            System.Timers.Timer frameTimer = new System.Timers.Timer();
            frameTimer.Interval = 15;
            frameTimer.Elapsed += Redraw;
            frameTimer.Start();
        }

        /// <summary>
        /// Redraw the game. This method is invoked every time the "frameTimer"
        /// above ticks.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Redraw(object sender, ElapsedEventArgs e)
        {
            //Use try catch to avoid exception being thrown when exiting the program
            try
            {
                // Invalidate this form and all its children (true)
                // This will cause the form to redraw as soon as it can
                this.Invoke(new MethodInvoker(() => Invalidate(true)));
            }
            catch (Exception) { }
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            //Make sure that the server or name fields are not left empty
            if (serverAddress.Text == "")
            {
                MessageBox.Show("Please enter a server address");
                return;
            }

            if(nameBox.Text == "")
            {
                MessageBox.Show("Please enter a name");
                return;
            }


            // Disable the controls and try to connect
            connectButton.Enabled = false;
            serverAddress.Enabled = false;
            nameBox.Enabled = false;

            //Try to connect to the server, if unsuccessful, display a message and
            //let the user try again
            try
            {
                //Connect to the server, then go to the first contact
                theServer = Networking.ConnectToServer(FirstContact, serverAddress.Text);
            }
            catch(Exception z)
            {
                MessageBox.Show(z.Message + ", please try again");

                //Enable the controls again
                connectButton.Enabled = true;
                serverAddress.Enabled = true;
                nameBox.Enabled = true;
            }
        }

        /// <summary>
        /// The final step in setting up the connection between the server and the 
        /// client
        /// </summary>
        /// <param name="state"></param>
        private void ReceiveStartup(SocketState state)
        {
            //Convert and store the startup data into an array
            string totalData = state.sb.ToString();
            string[] parts = Regex.Split(totalData, @"(?<=[\n])");

            //Iterate through all of the startup data
            foreach (string p in parts)
            {
                // Ignore empty strings added by the regex splitter
                if (p.Length == 0)
                    continue;

                // The regex splitter will include the last string even if it doesn't end with a '\n',
                // So we need to ignore it if this happens. 
                if (p[p.Length - 1] != '\n')
                    break;

                //If the startupValues list is equal to 2, we have received all of the startup data
                if (startupValues.Count >= 2)
                {
                    //Change to callback to start the "event loop"
                    state.callMe = ProcessMessage;

                    //Now that we received the world size, adjust the windows form and
                    //DrawingPanel to the appropiate demensions.
                    this.Invoke(new MethodInvoker(() => this.AdjustWorldSize()));

                    //The client has successfully connected to the server
                    isConnected = true;

                    break;
                }
                
                //Make sure that the startup values are numbers
                if(Regex.IsMatch(p, @"^\d+$"))
                {
                    startupValues.Add(int.Parse(p));
                }

                // Then remove it from the SocketState's growable buffer
                state.sb.Remove(0, p.Length);
            }

            Networking.GetData(state);
        }

        /// <summary>
        /// Used for the handshake between the server and the client
        /// </summary>
        /// <param name="state"></param>
        private void FirstContact(SocketState state)
        {
            //Change the callMe to receive the startup values
            state.callMe = ReceiveStartup;

            //Since we are now connected to the server, send the player's name
           
            Networking.SendData(state.theSocket, nameBox.Text);

            Networking.GetData(state);
        }

        /// <summary>
        /// Used to process messages received from the server
        /// </summary>
        /// <param name="state"></param>
        private void ProcessMessage(SocketState state)
        {
            string totalData = state.sb.ToString();
            string[] parts = Regex.Split(totalData, @"(?<=[\n])");

            // Loop until we have processed all messages.
            // We may have received more than one.
            foreach (string p in parts)
            {
                // Ignore empty strings added by the regex splitter
                if (p.Length == 0)
                    continue;

                // The regex splitter will include the last string even if it doesn't end with a '\n',
                // So we need to ignore it if this happens. 
                if (p[p.Length - 1] != '\n')
                    break;

                //Convert the data retreived from the message into the proper objects
                //and then update the model
                ClientUpdater.ConvertData(p, ref theWorld);
                
                // Then remove it from the SocketState's growable buffer
                state.sb.Remove(0, p.Length);
            }

            Networking.GetData(state);
        }

        /// <summary>
        /// Helper method to adjust the sizing of the windows form
        /// </summary>
        private void AdjustWorldSize()
        {
            //Before the client connects to the server, the default location and
            //sizes are selected
            if (startupValues.Count < 2)
            {
                //Starting values for location and size 
                ClientSize = new Size(1050, 800);
                drawingPanel.Size = new Size(750, 750);
                drawingPanel.Location = new Point(0, 50);
                scoreBoardPanel.Location = new Point(750, 50);
                scoreBoardPanel.Size = new Size(300, 750);
            }
            //Once the client receives the world size from the server, resize the panels accordingly to
            //fit the server world size
            else
            {
                //Make client bigger than the drawingPanel to fit in the menu and scoreboard
                int adjustedHeight = startupValues[1] + 50;
                int adjustedWidth = startupValues[1] + 300;

                //Change the client and drawingPanel size to fit with the world size
                ClientSize = new Size(adjustedWidth, adjustedHeight);
                drawingPanel.Size = new Size(startupValues[1], startupValues[1]);

                //Adjust the scoreboard panel so it fits between the drawingPanel and the windows form border
                scoreBoardPanel.Location = new Point(startupValues[1], 50);
                scoreBoardPanel.Size = new Size(adjustedWidth - startupValues[1], startupValues[1]);
            }

        }

        //Key Press Mapping

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //If the client has not connected to the server, do not fire any key press events
            if (!isConnected)
                return;

            e.SuppressKeyPress = true;

            //Use StringBuilder to combine all user input
            StringBuilder userKeys = new StringBuilder();
            userKeys.Append("(");

            //Look for whether the user presses the W, A, S, D, or space key
            if (e.KeyCode == Keys.W)
                userKeys.Append("T");

            if (e.KeyCode == Keys.A)
                userKeys.Append("L");

            if (e.KeyCode == Keys.D)
                userKeys.Append("R");

            if (e.KeyCode == Keys.Space)
                userKeys.Append("F");

            userKeys.Append(")\n");

            //Send off the user input to the server
        
            Networking.SendData(theServer, userKeys.ToString());
        }
    }
}
