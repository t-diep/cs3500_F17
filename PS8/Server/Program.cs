///
/// @authors Tony Diep and Sona Torosyan
///
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkController;
using Model;
using Newtonsoft.Json;
using System.Xml;
using System.IO;
using System.Diagnostics;

/// <summary>
/// Contains our own server program, which is an executable
/// </summary>
namespace Server
{
    /// <summary>
    /// The server blueprint; controls what incoming clients do
    /// </summary>
    public class Server
    {
        //Holds all of the current clients connected to this server
        private Dictionary<int, SocketState> clients;
        
        //The current world containing all sprites at the moment
        private World theWorld;

        //The number of units on each side of the square universe; default value is 750
        private int universeSize = 750;

        //The current hp for all ships; default value is 5
        private int defaultHP = 5;

        //The speed for the projectiles to travel; default value is 15
        private double projSpeed = 15;

        //The amount of acceleration the ship's engine apply; default value is .08
        private double engineStrength = .08;

        //The amount of degrees the ship can rotate per frame; default value is 2 degrees
        private double turnRate = 2;

        //The amount of area the ship occupies; default value is 20
        private int shipSize = 20;

        //The amount of area the star occupies; default value is 35
        private int starSize = 35;

        //The amount of frames a ship waits between firing projectiles; default value is 6 frames
        private int projFireDelay = 6;

        //The amount of frames a ship waits before respawning; default value is 300 frames
        private int respawnDelay = 300;

        //How often the server updates the world; default value is 16 frames per millisecond
        private double timePerFrame = 10;

        //Holds all of the locations for all of the currently existing stars
        private List<Vector2D> starLocations;

        //Holds all of the masses for all of the currently existing stars
        private List<double> starMasses;

        //Holds all of the left commands associated with a ship player
        private Dictionary<int, bool> leftCommands;

        //Holds all of the right commands associated with a ship player
        private Dictionary<int, bool> rightCommands;

        //Holds all of the thrusting commands associated with a ship player
        private Dictionary<int, bool> thrustCommands;

        //Holds all of the fire commands associated with a ship player
        private Dictionary<int, bool> fireCommands;

        //Indicates whether we are on the extra game feature mode or not
        private bool isOnExtraFeatureMode = false;

        /// <summary>
        /// Creates a new server with a given file name that gates
        /// to the settings xml file
        /// </summary>
        /// <param name="fileName">the name of the settings xml file</param>
        public Server(String fileName)
        {
            //Set up our list of clients, star locations, and star masses
            clients = new Dictionary<int, SocketState>();           
            starLocations = new List<Vector2D>();
            starMasses = new List<double>();

            //Set up our different command dictionaries
            leftCommands = new Dictionary<int, bool>();
            rightCommands = new Dictionary<int, bool>();
            thrustCommands = new Dictionary<int, bool>();
            fireCommands = new Dictionary<int, bool>();
            
            //Read in the configured settings from the xml file
            ReadXMLFile(fileName);

            //Create a new world with the configured settings from xml file
            theWorld = new World(projSpeed, engineStrength, shipSize, starSize, universeSize, 
                projFireDelay, turnRate, respawnDelay, defaultHP, isOnExtraFeatureMode);

            //Place all the stars onto the world
            foreach (Vector2D location in starLocations)
            {
                int starCounter = 0;
                Star star = new Star(starCounter, location, starMasses[starCounter]);
                starCounter++;
                theWorld.UpdateStars(star);
            }
        }

        /// <summary>
        /// Helps read in a given file name that maps to a xml file
        /// </summary>
        /// <param name="fileName">the xml file</param>
        private void ReadXMLFile(String fileName)
        {
            //Holds x and y location configured values
            int xLoc = 0;
            int yLoc = 0;

            //Prevent any null or empty file names from being read from XMLReader
            if(fileName == null || fileName == "")
            {
                throw new Exception("Hey, your filname cannot be null or empty! -_______-");
            }

            //Set up XmlReader disposable; read in the different attributes from settings.xml file
            try
            {
                using (XmlReader xmlReader = XmlReader.Create(fileName))
                {
                    while(xmlReader.Read())
                    {
                        if(xmlReader.IsStartElement())
                        {
                            switch (xmlReader.Name)
                            {
                                case "SpaceSettings":
                                    break;

                                case "UniverseSize":
                                    xmlReader.Read();
                                    universeSize = int.Parse(xmlReader.Value);
                                    break;

                                case "FramesPerShot":
                                    xmlReader.Read();
                                    projFireDelay = int.Parse(xmlReader.Value);
                                    break;

                                case "MSPerFrame":
                                    xmlReader.Read();
                                    timePerFrame = int.Parse(xmlReader.Value);
                                    break;

                                case "RespawnRate":
                                    xmlReader.Read();
                                    respawnDelay = int.Parse(xmlReader.Value);
                                    break;

                                case "HP":
                                    xmlReader.Read();
                                    defaultHP = int.Parse(xmlReader.Value);
                                    break;

                                case "projSpeed":
                                    xmlReader.Read();
                                    projSpeed = int.Parse(xmlReader.Value);
                                    break;

                                case "engineStrength":
                                    xmlReader.Read();
                                    engineStrength = Double.Parse(xmlReader.Value);
                                    break;

                                case "turnRate":
                                    xmlReader.Read();
                                    turnRate = Double.Parse(xmlReader.Value);
                                    break;

                                case "shipSize":
                                    xmlReader.Read();
                                    shipSize = int.Parse(xmlReader.Value);
                                    break;

                                case "starSize":
                                    xmlReader.Read();
                                    starSize = int.Parse(xmlReader.Value);
                                    break;

                                case "Star":
                                    break;

                                case "x":
                                    xmlReader.Read();
                                    xLoc = int.Parse(xmlReader.Value);
                                    break;
                                case "y":
                                    xmlReader.Read();
                                    yLoc = int.Parse(xmlReader.Value);
                                    starLocations.Add(new Vector2D(xLoc, yLoc));
                                    break;

                                case "mass":
                                    xmlReader.Read();
                                    starMasses.Add(Double.Parse(xmlReader.Value));
                                    break;

                                case "ExtraGameMode":
                                    xmlReader.Read();
                                    bool.TryParse(xmlReader.Value, out isOnExtraFeatureMode);
                                    break;
                            }
                        }
                    }
                }
            }
            catch(XmlException)
            {
                Console.WriteLine("Unable to write xml file");
            }
            catch(IOException)
            {
                Console.WriteLine("Hmm... can't find the file. Try again.");
            }
        }

        /// <summary>
        /// Main program
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //Make a server that takes in file name mapping to the xml file for configuring settings
            Server theServer = new Server("../../../Resources/settings.xml");

            //Continue to accept one or more clients that want to connect to server
            Networking.ServerAwaitingClientLoop(theServer.HandleNewClient);

            //Update server infinitisemally
            while(true)
            {
                theServer.Update();
            }
        }

        /// <summary>
        /// In the case of a new client connecting to our server, this 
        /// method will get information/data about the new client 
        /// </summary>
        /// <param name="socketState"></param>
        public void HandleNewClient(SocketState socketState)
        {
            socketState.SetCallMeDelegate(ReceiveName);
            Networking.GetData(socketState);
        }

        /// <summary>
        /// Gets the name of the existing client from the given socket state, and sends
        /// that data to the network; this is used as the given socket state's callback
        /// delegate
        /// </summary>
        /// <param name="socketState">socket state</param>
        public void ReceiveName(SocketState socketState)
        {
            //Don't want to include \n character at the end, so we do message buffer's length - 1
            //The ID of the new ship is configured by getting the number of ships in the world currently
            Ship ship = new Ship(socketState.GetStringBuilder().ToString().Substring(0, socketState.GetStringBuilder().Length - 1), theWorld.Ships().Count);

            //Save this ship's ID into the socket state storage
            socketState.SetID(ship.ID());

            //Update the call me delegate to allow more command data to retrieve
            socketState.SetCallMeDelegate(ReceiveCommands);

            //Update the message builder to retrieve name of client to send to server
            socketState.UpdateStringBuilder(0, socketState.GetStringBuilder().Length);

            bool sendCondition = Networking.Send(socketState.GetSocket(), ship.ID() + "\n" + universeSize + "\n");

            //When sending data, we need to append "\n" to indicate end
            if (sendCondition)
            {
                //Allow only one thread to add clients to our list of clients
                lock (clients)
                {
                    //Add this socket state to the list of clients
                    clients.Add(socketState.GetID(), socketState);
                }

                //Allow only one thread to update the ships in current world
                lock (theWorld)
                {
                    //Add this newly created ship to 
                    theWorld.UpdateShip(ship);
                }

                //Ask for data from client (like commands)
                Networking.GetData(socketState);
            }            
        }

        /// <summary>
        /// Receives commands in which to move the sprites; the server will arbritarily
        /// choose which command to perform
        /// </summary>
        /// <param name="socketState">the socket state</param>
        public void ReceiveCommands(SocketState socketState)
        {
            //Retrieve the command from the socket state's message buffer
            //Don't include '/n' and ')' at the end
            //Don't include '(' in the beginning
            String command = socketState.GetStringBuilder().ToString().Substring(1, socketState.GetStringBuilder().Length - 3);

            //Go through each character in the command string 
            for(int index = 0; index < command.Length; index++)
            {
                //Check for valid commands (that is, thrust, left, right, up, and terminating character)
                if(command[index] == 'T')
                {
                    continue;
                }
                else if(command[index] == 'R')
                {
                    continue;
                }
                else if (command[index] == 'F')
                {
                    continue;
                }
                else if(command[index] == 'L')
                {
                    continue;
                }      
                //Bad data in command string, so terminate connection of client
                else
                {
                    lock(clients)
                    {
                        clients.Remove(socketState.GetID());
                        theWorld.KillShip(socketState.GetID());
                    }

                    //Done removing client from connection
                    return;
                }
            }

            //Override current message builder so we can avoid keeping old messages and receive new ones
            socketState.UpdateStringBuilder(0, socketState.GetStringBuilder().Length);
           
            //Generate a random index for server to arbitrarily pick a command
            //if it received conflicting commands (e.g. (LR))
            Random generator = new Random();
            int randCommandIndex = generator.Next(command.Length);

            //Get a random command from our message
            char singleCommand = command[randCommandIndex];

            //Handle non-conflicting command case
            //Thrust command is triggered
            if(command.Contains("T"))
            {
                AddCommand(thrustCommands, socketState);
                
            }
            //Fire command is triggered
            if(command.Contains("F"))
            {
                AddCommand(fireCommands, socketState);
                
            }
            //Left and right commands both pressed, so choose either or
            if (command.Contains("L") && command.Contains("R"))
            {               
                int leftOrRight = generator.Next(2);

                if (leftOrRight == 0)
                {
                    AddCommand(leftCommands, socketState);
                    
                }
                else
                {
                    AddCommand(rightCommands, socketState);
                    
                }                
            }
            //Right command only pressed
            else if (command.Contains("R"))
            {
                AddCommand(rightCommands, socketState);
            }
            //Left command only pressed
            else if(command.Contains("L"))
            {
                AddCommand(leftCommands, socketState);
                
            }

            //After receiving commands, ask for more data
            Networking.GetData(socketState);
        }

        /// <summary>
        /// Helps add all of the flag commands for a ship depending on which command we want to add to
        /// </summary>
        /// <param name="commandDictionary">the dictionary of a particular command to add</param>
        /// <param name="socketState">holds the ID of the ship to recognize its commands</param>
        private void AddCommand(Dictionary<int, bool> commandDictionary, SocketState socketState)
        {
            
            //Only allow one thread to add commands to the command dictionary
            lock(commandDictionary)
            {
                if (commandDictionary.ContainsKey(socketState.GetID()))
                {
                    commandDictionary[socketState.GetID()] = true;
                }
                else
                {
                    commandDictionary.Add(socketState.GetID(), true);
                }
            }           
        }

        /// <summary>
        /// Updates the world depending on a time per frame value; invoked on every iteration and sent to every existing client
        /// </summary>
        public void Update()
        {
            //Create stopwatch
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            //Let stopwatch run infinitisemally while updating every time per frame value
            while (stopWatch.ElapsedMilliseconds < timePerFrame)
            {
                //DO NOTHING!!! O__O
            }

            //Reset stopwatch
            stopWatch.Reset();

            //~Update~//

            //Create message
            String message = "";

            //Only allow one thread to access the world and update it
            lock(theWorld)
            {
                //Update every ship in the current world
                foreach (Ship ship in theWorld.Ships().Values)
                {
                    message += JsonConvert.SerializeObject(ship) + "\n";

                    
                    //Ship is dead and the client is still connected to the server
                    if (ship.IsDead() && clients.ContainsKey(ship.ID()))
                    {
                        if (ship.ShipNumberOfFrames() != respawnDelay)
                        {
                            ship.IncrementShipFrames();
                            continue;
                        }
                        else
                        {
                            //Respawn the ship
                            theWorld.RespawnShip(ship.ID());
                        }
                    }

                    //Apply the gravity for all the stars on the current ship
                    theWorld.ApplyGravityToShip(ship.ID());
                    
                    //Thrust commands

                    //Check for thrust flag in the thrustCommand Dictionary
                    if (thrustCommands.ContainsKey(ship.ID()))
                    {
                        theWorld.UpdateShipThrust(ship.ID(), thrustCommands[ship.ID()]);
                        //set the flag back to false because it has been processed already
                        thrustCommands[ship.ID()] = false;
                    }
                    else
                    {
                        theWorld.UpdateShipThrust(ship.ID(), false);
                    }

                    //If in the thrusting dictionary the flag for the ship
                    //is set to true then the ship is thrusting;
                    //so, apply thrusting accelaration to the ship
                    if (theWorld.Ships()[ship.ID()].IsThrusting())
                    {

                        theWorld.Thrust(ship.ID());
                    }


                    //Right commands
                    //check in the dictionary if the ship has received a command
                    //for turning right
                    if (rightCommands.ContainsKey(ship.ID()) && rightCommands[ship.ID()])
                    {
                        theWorld.ApplyShipRightRotation(ship.ID());
                        //set the flag back to false after it has been processed
                        rightCommands[ship.ID()] = false;

                    }
                    //if it contains the flag 
                    //set it to false
                    else if (rightCommands.ContainsKey(ship.ID()))
                    {
                        rightCommands[ship.ID()] = false;
                    }

                    //Left commands
                    //check in the dictionary if the ship has received a command
                    //for turning left
                    if (leftCommands.ContainsKey(ship.ID()) && leftCommands[ship.ID()])
                    {
                        theWorld.ApplyShipLeftRotation(ship.ID());
                        //set the flag back to false after it has been processed
                        leftCommands[ship.ID()] = false;

                    }
                    else if (leftCommands.ContainsKey(ship.ID()))
                    {
                        leftCommands[ship.ID()] = false;
                    }


                    //Firing commands
                    //Check for server's thrust command flag
                    if (fireCommands.ContainsKey(ship.ID()) && fireCommands[ship.ID()])
                    {
                        theWorld.Fire(ship.ID());
                        //set the flag back to false because 
                        //it has been processed
                        fireCommands[ship.ID()] = false;
                    }
                    else if (fireCommands.ContainsKey(ship.ID()))
                    {
                        fireCommands[ship.ID()] = false;
                    }
                    
                    //Update ship's velocity and position
                    theWorld.UpdateShip(ship.ID());

                    

                    //theWorld.ApplyGravityToShip(ship.ID());
                    theWorld.CollideShip();
                    //if is on extra game mode
                    //apply ship collision 
                    if (isOnExtraFeatureMode)
                    {
                        theWorld.CollideShipToShip();
                    }                
                }

                

                //Update every projectile in the current world
                foreach (Projectile proj in theWorld.Projectiles().Values)
                {                
                    message += JsonConvert.SerializeObject(proj) + "\n";
                    
                    //Apply gravity to the projectiles if it is on 
                    //extra game mode
                    if(isOnExtraFeatureMode)
                    {
                        theWorld.ApplyGravityToProjectile(proj.ID());
                    }

                    theWorld.UpdateProjectileLocation(proj.ID());
                }

                

                //Update every star in the current world
                foreach (Star star in theWorld.Stars().Values)
                {
                    message += JsonConvert.SerializeObject(star) + "\n";
                }

                
            }

            //Only allow one thread to send the updated world 
            lock(clients)
            {
                //list that will hold the indexes of the clients that
                //are disconnected
                List<int> socketIndexes = new List<int>();

                foreach (SocketState client in clients.Values)
                {
                    //if Send returned false
                    //exception happened so add the index into the list
                    bool sendCondition = Networking.Send(client.GetSocket(), message);

                    if (!sendCondition)
                    {
                        socketIndexes.Add(client.GetID());
                    }
                }
                //remove the disconnected clients from 
                //the clients list and set the hp of the ship 
                //associated with that client to 0
                foreach (int index in socketIndexes)
                {
                    clients.Remove(index);
                    theWorld.KillShip(index);
                }               
            }       
        }
    }
}
