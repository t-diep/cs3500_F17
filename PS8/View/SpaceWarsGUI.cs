///
/// @authors Tony Diep and Sona Torosyan
///
using Model;
using NetworkController;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

/// <summary>
/// Contains the SpaceWarsGUI form
/// </summary>
namespace View
{
    /// <summary>
    /// The SpaceWarsGUI form blueprint
    /// </summary>
    public partial class SpaceWarsGUI : Form
    {
        //The current world
        private World theWorld;

        //The server used for the client to connect to
        private Socket theServer;

        //The drawing panel to use for drawing SpaceWars game
        private DrawingPanel drawingPanel;

        //Verifies if the ID and world size is received 
        private bool receivedIDAndSize;

        //Indicates whether the user hit the left arrow key
        private bool leftArrowKeyPressed;

        //Indicates whether the user hit the right arrow key
        private bool rightArrowKeyPressed;

        //Indicates whether the up arrow key
        private bool upArrowKeyPressed;

        //Indicates whether the space bar key is pressed
        private bool spaceBarPressed;

        //Holds the name the user enter in as their player name
        private String playerName;

        //The counter to use to number the guest if user doesn't provide a player name
        private int guestCounter;

        //Holds the current world size
        private int worldSize;

        //Holds the current form size
        private int formSize;

        //Dynamic link library import for watermark feature
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        /// <summary>
        /// Creates the SpaceWars GUI
        /// </summary>
        public SpaceWarsGUI()
        {
            InitializeComponent();

            //Set up current world and configure size for the form
            theWorld = new World();

            //Set up guest counter
            guestCounter = 0;

            //Set default sizes for world size and SpaceWars GUI Form size
            worldSize = 750;
            formSize = 1000;
            Size = new Size(formSize, formSize);

            //Configure settings for the DrawingPanel
            drawingPanel = new DrawingPanel(theWorld);
            drawingPanel.Location = new Point(0, 50);
            drawingPanel.BackColor = System.Drawing.Color.Black;
            drawingPanel.Size = new Size(worldSize, worldSize);
            this.Controls.Add(drawingPanel);

            //Apply watermark feature to the player name text box
            SendMessage(playerNameTextBox.Handle, 0x1501, 1, "Please Enter Name");
        }

        /// <summary>
        /// Listens for the connect button to be triggered; once clicked, allows the user
        /// to connect to a server 
        /// </summary>
        /// <param name="sender">object that represents the event triggered</param>
        /// <param name="e">an event that used to pass in for a response</param>
        private void ConnectButton_Click(object sender, EventArgs e)
        {
            //Check player name and server name before connecting to server
            if (IsValidServerName())
            {
                // Disable the controls and try to connect
                DisableConnectToServerComponents();
                AssignPlayerName();

                //Get the IP address in and attempt to connect to server with that IP address
                string ip = serverNameTextBox.Text;

                //Try connecting to the server
                try
                {
                    theServer = Networking.ConnectToServer(FirstContact, ip);
                }
                //Server name doesn't exist, so reenabled 
                catch (Exception)
                {
                    MessageBox.Show("Server name doesn't exist.  Please try again");
                    ReEnableConnectToServerComponents();
                }
            }
        }

        /// <summary>
        /// Helps verify correct server and player names before proceeding
        /// to connect to server
        /// </summary>
        private bool IsValidServerName()
        {
            //Server name is either blank or null, so return false
            if (serverNameTextBox.Text == "" || serverNameTextBox.Text == null)
            {
                MessageBox.Show("Invalid server name.  Please enter a valid server name");
                return false;
            }

            //Server name is valid, so return true
            return true;
        }

        /// <summary>
        /// Helps set player name credentials to the player sprite depending on whether
        /// the user provided a player name or not
        /// </summary>
        private void AssignPlayerName()
        {
            playerName = playerNameTextBox.Text;

            //Don't allow user to enter a player name more than 16 characters long
            if (playerName.Length > 16)
            {
                MessageBox.Show("Player name must be 16 characters or less!");
            }

            //Store user's entered player name, or store "Guest" plus number if no player name provided
            if (playerNameTextBox.Text != "")
            {
                playerName = playerNameTextBox.Text;
            }
            else
            {
                playerName = "Guest" + ++guestCounter;
            }
        }

        /// <summary>
        /// Helps send incoming user information to the socket and retrieve 
        /// incoming data
        /// </summary>
        /// <param name="state">socket state</param>
        private void FirstContact(SocketState state)
        {
            //Server cannot connect
            if (state.CantConnectToServer())
            {
                MessageBox.Show("Cannot connect to server.  Please try again!");

                //Renable components after failing to connnect to server
                try
                {
                    this.Invoke(new MethodInvoker(() => ReEnableConnectToServerComponents()));
                }
                //Close the form safely
                catch (ObjectDisposedException)
                {

                }
            }
            else
            {
                //Send and get data from server
                state.SetCallMeDelegate(ReceiveStartup);
                Networking.Send(state.GetSocket(), playerName + "\n");
                Networking.GetData(state);
            }
        }

        /// <summary>
        /// Receive data at the time the user starts up the game by connecting
        /// to the server; this method will be called once
        /// </summary>
        /// <param name="state">socket state</param>
        private void ReceiveStartup(SocketState state)
        {
            lock (theWorld)
            {
                Dictionary<int, Ship> oldShips = theWorld.Ships();
                //At this point, we are receiving the ID and world size, so update this flag to true
                receivedIDAndSize = true;

                //Split long message text into individual parts
                string totalData = state.GetStringBuilder().ToString();
                string[] parts = Regex.Split(totalData, @"(?<=[\n])");

                //Retrieve the ID and world size in numerical data form
                int idOfPlayer;
                int.TryParse(parts[0], out idOfPlayer);
                int currentWorldSize = int.Parse(parts[1]);

                //Create a ship player to represent this unique ID as well as the player name
                theWorld.UpdateShip(new Ship(playerName, idOfPlayer));

                //Update the sizes between world and form to avoid improper formatting of the GUI
                UpdateNewWorldAndFormSizes(currentWorldSize);

                try
                {
                    //Draw player credentials on a separate panel
                    MethodInvoker scoreboardDisplay = new MethodInvoker(() => ShowScoreboardPanelAndItsComponents());
                    this.Invoke(scoreboardDisplay);
                }
                catch (ObjectDisposedException)
                {

                }

                //No longer need the ID and world size, so remove them
                state.GetStringBuilder().Remove(0, parts[0].Length);
                state.GetStringBuilder().Remove(0, parts[1].Length);

                //Retrieve data from current world and get data from current socket state
                state.SetCallMeDelegate(ReceiveWorld);
                Networking.GetData(state);

                //Redraw based on incoming data without having to use a timer
                NoTimerRedraw();
            }

            DisplayPlayerStats();
        }

        /// <summary>
        /// Helps update non-default sizes for the world and SpaceWarsGUI Form
        /// </summary>
        /// <param name="currentWorldSize">the world size as of now</param>
        private void UpdateNewWorldAndFormSizes(int currentWorldSize)
        {
            //Current world size is bigger than the form size and the menu strip's height combined
            if ((currentWorldSize > formSize - clientMenuStrip.Height) || currentWorldSize > formSize - scoreboardFlowPanel.Width)
            {
                int sumWorldSizeClientStrip = currentWorldSize + clientMenuStrip.Height;
                int sumWorldSizeScoreboard = currentWorldSize + scoreboardFlowPanel.Width;

                //Form should have the biggest size from combining size of world and menu strip, so update the form size
                if (sumWorldSizeClientStrip > sumWorldSizeScoreboard)
                {
                    formSize = sumWorldSizeClientStrip;
                }
                //Form should have the biggest size from the scoreboard and world size combined, so update the size
                else
                {
                    formSize = sumWorldSizeScoreboard;
                }
            }
            //Current world size is smaller than the form size and the menu strip's height combined
            else
            {
                worldSize = currentWorldSize;
            }

            try
            {
                //Update world size
                MethodInvoker updateWorldSizes = new MethodInvoker(() => ClientSize = new Size(formSize, formSize));
                this.Invoke(updateWorldSizes);

                //Update drawing panel size
                MethodInvoker updateDrawingPanelSize = new MethodInvoker(() => drawingPanel.Size = new Size(worldSize, worldSize));
                this.Invoke(updateDrawingPanelSize);

                //Invalidate form and its children after updating world size and drawing panel size
                MethodInvoker invalidate = new MethodInvoker(() => this.Invalidate(true));
                this.Invoke(invalidate);
            }
            //Close form safely
            catch (ObjectDisposedException)
            {

            }
        }

        /// <summary>
        /// Redraws from the incoming data without using a timer event
        /// </summary>
        private void NoTimerRedraw()
        {
            try
            {
                //Redraw by invalidating form and its children
                MethodInvoker redraw = new MethodInvoker(() => this.Invalidate(true));
                Invoke(redraw);

                //Take in keyboard inputs that have been triggered and send respective commands for each
                MethodInvoker sendCommands = new MethodInvoker(() => SendCommandKeyMessage());
                Invoke(sendCommands);
            }
            //Close form safely
            catch (ObjectDisposedException)
            {

            }
        }

        /// <summary>
        /// Helps retrieve all of the data contained about the current World
        /// </summary>
        /// <param name="state">socket state</param>
        private void ReceiveWorld(SocketState state)
        {
            string totalData = state.GetStringBuilder().ToString();
            Console.WriteLine("Total Data: " + totalData);
            string[] parts = Regex.Split(totalData, @"(?<=[\n])");
            Dictionary<int, Ship> oldShips = new Dictionary<int, Ship>();
            //Allow one thread to access this critical section at a time
            lock (theWorld)
            {
                oldShips = new Dictionary<int, Ship>(theWorld.Ships());
                //Process all incoming messages; possible to receive more than one at a time
                foreach (string stringTextObject in parts)
                {
                    // Ignore empty strings added by the regex splitter
                    if (stringTextObject.Length == 0)
                        continue;

                    // The regex splitter will include the last string even if it doesn't end with a '\n',
                    // So we need to ignore it if this happens. 
                    if (stringTextObject[stringTextObject.Length - 1] != '\n')
                        break;

                    //Produce an JSON object in which to parse into different sprite tokens
                    JObject obj = JObject.Parse(stringTextObject);
                    JToken shipToken = obj["ship"];
                    JToken projectileToken = obj["proj"];
                    JToken starToken = obj["star"];
                 

                    //The JSON token contains a ship, so make a ship
                    if (shipToken != null)
                    {
                        Ship ship = JsonConvert.DeserializeObject<Ship>(stringTextObject);
                        theWorld.UpdateShip(ship);
                        state.GetStringBuilder().Remove(0, stringTextObject.Length);
                    }
                    //The JSON token contains a projectile, so make a projectile
                    else if (projectileToken != null)
                    {
                        Projectile projectile = JsonConvert.DeserializeObject<Projectile>(stringTextObject);
                        theWorld.UpdateProjectiles(projectile);
                        state.GetStringBuilder().Remove(0, stringTextObject.Length);

                        if (!projectile.Alive())
                        {
                            theWorld.RemoveProjectile(projectile);
                        }
                    }
                    //The JSON token contains a star, so make a star
                    else if (starToken != null)
                    {
                        Star star = JsonConvert.DeserializeObject<Star>(stringTextObject);
                        theWorld.UpdateStars(star);
                        state.GetStringBuilder().Remove(0, stringTextObject.Length);
                    }
                    //Neither a star, projectile, or a ship, so proceed
                    else
                        continue;
                }

            }

            //The number of previous ships is the same as the current number of ships
            if (oldShips.Count != theWorld.Ships().Count)
            {
                DisplayPlayerStats(oldShips);
            }

            //Retrieve data from the current socket state
            Networking.GetData(state);

            //Data has changed, so update them for the score board
            UpdateScoreboard();

            //Redraw without using timer
            NoTimerRedraw();
        }

        /// <summary>
        /// Helps send command messages to the server once one of the arrow 
        /// keys or space bar key is pressed... but ONLY if the ID and world 
        /// size of the ship is received
        /// </summary>
        /// <param name="sender">object that represents the event that triggered</param>
        /// <param name="e">the argument that listens for the keys being pressed</param>
        private void SpaceWarsGUI_KeyDown(object sender, KeyEventArgs e)
        {
            //Ship ID and world size data received, so process valid keyboard keys
            if (receivedIDAndSize)
            {
                //Update flags of the specific keys pressed to true
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        upArrowKeyPressed = true;
                        break;

                    case Keys.Left:
                        leftArrowKeyPressed = true;
                        break;

                    case Keys.Right:
                        rightArrowKeyPressed = true;
                        break;

                    case Keys.Space:
                        spaceBarPressed = true;
                        e.SuppressKeyPress = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Helps send different command messages depending on which
        /// arrow keys are pressed
        /// </summary>
        private void SendCommandKeyMessage()
        {
            //Start taking commands by opening with an opening parenthesis
            String commands = "(";

            //Check for the different keys pressed, so add commands for each key pressed
            if (leftArrowKeyPressed)
            {
                commands += "L";
            }
            if (rightArrowKeyPressed)
            {
                commands += "R";
            }
            if (upArrowKeyPressed)
            {
                commands += "T";
            }
            if (spaceBarPressed)
            {
                commands += "F";
            }

            //As long as there's at least key pressed, append closing message, the send them to server
            if (commands != "(")
            {
                commands += ")\n";

                Networking.Send(theServer, commands);
            }
        }

        /// <summary>
        /// Helps listen for any of the arrow keys or the space bar key
        /// being pressed; then saves those keys as valid input
        /// </summary>
        /// <param name="sender">object that represents an event that happened</param>
        /// <param name="e">the argument that listens for certain keys triggered</param>
        private void SpaceWarsGUI_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //Accept arrow keys and space bar key as valid input
            switch (e.KeyCode)
            {
                case Keys.Up:
                case Keys.Left:
                case Keys.Right:
                case Keys.Space:
                    e.IsInputKey = true;
                    break;
            }
        }

        /// <summary>
        /// Displays the player stats onto the scoreboard panel, located to the 
        /// right of the drawing panel
        /// </summary>
        private void DisplayPlayerStats()
        {
            foreach (Ship ship in theWorld.Ships().Values)
            {
                //Make ProgressBar for this ship with some properties configured
                ProgressBar shipBar = new ProgressBar();
                shipBar.Minimum = 0;
                shipBar.Maximum = 5;
                shipBar.Value = 5;
                shipBar.BackColor = Color.Green;
                shipBar.Width = 170;
                shipBar.Name = ship.ID().ToString();

                //Make label for this ship with some properties configured
                Label shipLabel = new Label();
                shipLabel.Width = 700;
                shipLabel.Text = ship.Name() + ", Score: " + ship.Score();
                shipLabel.Name = ship.ID().ToString();
                shipLabel.BackColor = Color.Black;
                ChangePlayerNameFontColor(shipLabel, ship.ID());

                //Add an empty panel for spacing between current player labels and other ones
                Panel spacingPanel = new Panel();

                //Try to modify scoreboard panel
                try
                {
                    //Allow the thread responsible for this form to modify the scoreboard
                    this.Invoke(new MethodInvoker(() => scoreboardFlowPanel.Controls.Add(shipLabel)));
                    this.Invoke(new MethodInvoker(() => scoreboardFlowPanel.Controls.Add(shipBar)));
                    this.Invoke(new MethodInvoker(() => scoreboardFlowPanel.Controls.Add(spacingPanel)));
                }
                //Close form safely
                catch (ObjectDisposedException)
                {

                }
            }          
        }

        /// <summary>
        /// Helps display the scoreboard that will show to the right of the form
        /// once the user connects to the server; this overloaded method
        /// helps create the exact number of progress bars and player name labels
        /// </summary>
        /// <param name="oldShips">holds the previous number of ships</param>
        private void DisplayPlayerStats(Dictionary<int, Ship> oldShips)
        {
            lock (theWorld)
            {
                foreach (Ship ship in theWorld.Ships().Values)
                {
                    if (oldShips.ContainsKey(ship.ID()))
                        continue;

                    //Make ProgressBar for this ship with some properties configured
                    ProgressBar shipBar = new ProgressBar();
                    shipBar.Minimum = 0;
                    shipBar.Maximum = 5;
                    shipBar.Value = 5;
                    shipBar.BackColor = Color.Green;
                    shipBar.Width = 170;
                    shipBar.Name = ship.ID().ToString();

                    //Make label for this ship with some properties configured
                    Label shipLabel = new Label();
                    shipLabel.Width = 700;
                    shipLabel.Text = ship.Name() + ", Score: " + ship.Score();
                    shipLabel.Name = ship.ID().ToString();
                    shipLabel.BackColor = Color.Black;
                    ChangePlayerNameFontColor(shipLabel, ship.ID());

                    //Add an empty panel for spacing between current player labels and other ones
                    Panel spacingPanel = new Panel();

                    //Try modifying scoreboard panel
                    try
                    {
                        //Allow the thread responsible for this form to modify the scoreboard
                        this.Invoke(new MethodInvoker(() => scoreboardFlowPanel.Controls.Add(shipLabel)));
                        this.Invoke(new MethodInvoker(() => scoreboardFlowPanel.Controls.Add(shipBar)));
                        this.Invoke(new MethodInvoker(() => scoreboardFlowPanel.Controls.Add(spacingPanel)));
                    }
                    //Close form safely
                    catch (ObjectDisposedException)
                    {

                    }
                }
            }
        }

        /// <summary>
        /// Helps update information from the current world and displays them on the
        /// scoreboard panel
        /// </summary>
        private void UpdateScoreboard()
        {
            int ID = 0;
            //Go through all of the controls in the scoreboard panel
            foreach (Control control in scoreboardFlowPanel.Controls)
            {
                //Control is a label, so get the ID of the control 
                if (control is Label)
                {
                    ID = int.Parse(control.Name);

                    //Try displaying ID of this player to this label
                    try
                    {
                        Invoke(new MethodInvoker(() => control.Text = theWorld.Ships()[ID].Name() + " " + theWorld.Ships()[ID].Score()));
                    }
                    //Close form safely
                    catch (ObjectDisposedException)
                    {

                    }
                }
                //Control is a progress bar, so update the value contained depending on the ship's HP
                else if (control is ProgressBar)
                {
                    ProgressBar progressBar = control as ProgressBar;

                    //Try to update progress bar value
                    try
                    {
                        MethodInvoker updateProgressValue = new MethodInvoker(() => progressBar.Value = theWorld.Ships()[ID].HP());
                        this.Invoke(updateProgressValue);
                    }
                    //Close form safely
                    catch (ObjectDisposedException)
                    {

                    }
                }
            }
        }

        /// <summary>
        /// Helps show the scoreboard panel and all of the current players' 
        /// labels and progress bars; activated when a user attempts to connect 
        /// to the server
        /// </summary>
        private void ShowScoreboardPanelAndItsComponents()
        {
            //Show the scoreboard
            scoreboardFlowPanel.Visible = true;

            //Allow every control in scoreboard to also be visible
            foreach (Control control in scoreboardFlowPanel.Controls)
            {
                control.Visible = true;
            }

            //Update the location of the scoreboard panel relative to the drawing panel and menu strip
            scoreboardFlowPanel.Location = new Point(drawingPanel.Width, clientMenuStrip.Height);
        }

        /// <summary>
        /// Helps disable the components such as the player name text box,
        /// the server name text box, and the connect button disabled 
        /// once the user attempts to connect to the game
        /// </summary>
        private void DisableConnectToServerComponents()
        {
            playerNameTextBox.Enabled = false;
            connectButton.Enabled = false;
            serverNameTextBox.Enabled = false;
        }

        /// <summary>
        /// Helps enable the components in the menu strip after attempting
        /// to connect to the server failed.
        /// </summary>
        private void ReEnableConnectToServerComponents()
        {
            playerNameTextBox.Enabled = true;
            connectButton.Enabled = true;
            serverNameTextBox.Enabled = true;
        }

        /// <summary>
        /// Helps update the flags of the different keyboard keys to false, 
        /// indicating that the keys were no longer pressed at the time
        /// </summary>
        /// <param name="sender">the object that represented the event that triggered</param>
        /// <param name="e">the listener for valid key inputs</param>
        private void SpaceWarsGUI_KeyUp(object sender, KeyEventArgs e)
        {
            //Update flags for each key input not pressed at the moment
            switch (e.KeyCode)
            {
                case Keys.Up:
                    upArrowKeyPressed = false;
                    break;

                case Keys.Left:
                    leftArrowKeyPressed = false;
                    break;

                case Keys.Right:
                    rightArrowKeyPressed = false;
                    break;

                case Keys.Space:
                    spaceBarPressed = false;
                    break;
            }
        }

        /// <summary>
        /// Helps change the font color of the player name label to the 
        /// same color as the player
        /// </summary>
        /// <param name="shipLabel">the ship player name label</param>
        /// <param name="ID">the ship's ID which corresponds to a particular color</param>
        private void ChangePlayerNameFontColor(Label shipLabel, int ID)
        {
            //Assign colors based on modulo result
            switch (ID % 8)
            {
                case 0:
                    shipLabel.ForeColor = Color.Blue;
                    break;
                case 1:
                    shipLabel.ForeColor = Color.RosyBrown;
                    break;
                case 2:
                    shipLabel.ForeColor = Color.Green;
                    break;
                case 3:
                    shipLabel.ForeColor = Color.Gray;
                    break;
                case 4:
                    shipLabel.ForeColor = Color.Red;
                    break;
                case 5:
                    shipLabel.ForeColor = Color.Violet;
                    break;
                case 6:
                    shipLabel.ForeColor = Color.White;
                    break;
                case 7:
                    shipLabel.ForeColor = Color.LightGoldenrodYellow;
                    break;
            }
        }
    }
}