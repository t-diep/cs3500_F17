using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Server
{
    /// <summary>
    /// A class used to run a server for the SpaceWars game
    /// </summary>
    public static class SpaceWarsServer
    {
        /// <summary>
        /// Contains the Server's desired Settings
        /// </summary>
        private static GameSettings gameSettings;

        /// <summary>
        /// Contains all of the objects in the world
        /// </summary>
        private static World world;

        /// <summary>
        /// Used to time the updates to the world
        /// </summary>
        private static Stopwatch updateWatch;

        /// <summary>
        /// Stores the SocketStates for all of the connected clients
        /// </summary>
        private static LinkedList<SocketState> connections;

        /// <summary>
        /// Stores the start time of the game to figure out the
        /// total duration of the game
        /// </summary>
        private static DateTime startTime;

        /// <summary>
        /// Launches console application that looks for the settings 
        /// file if provided and initializes the server.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            string file = "../../../Resources/settings.xml";

            // Check for the file if it exists then assign to file
            if (!(File.Exists(file)))
                file = "settings.xml";

            //Retreive game settings from file or use default settings if a
            //settings file unavailable
            gameSettings = GameSettings.ReadSettingsFile(file);
            
            //Initialize the objects and start the server
            connections = new LinkedList<SocketState>();

            updateWatch = new Stopwatch();
            LaunchServer();

            //Save the start time
            startTime = DateTime.Now;
            
            //Create thread which will be used to update the world
            Thread thread1 = new Thread(UpdateLoop);
            thread1.Start();

            string endCommand = "";

            world = new World(gameSettings.UniverseSize, gameSettings.starList, gameSettings.FramesPerShot, gameSettings.RespawnRate, gameSettings.MovingStars);

            //Wait for the user to enter 'stop' into
            //the console window.
            while (endCommand != "stop")
            {               
                endCommand = Console.ReadLine();
            }

            long secondsElapsed = updateWatch.ElapsedMilliseconds;
            DatabaseHandler.SendGame(secondsElapsed, world);

            //Once the user enters 'stop', upload all
            //the game and player stats to the server
            UploadToServer();
        }

        /// <summary>
        /// Uploads the game and player stats to the SQL
        /// database and then closes the server application
        /// </summary>
        private static void UploadToServer()
        {
            Networking.ServerAwaitingClientLoop(new NetworkAction(HandleHTTPConnection), 80);  
        }
        
        /// <summary>
        /// This method intializes the server
        /// </summary>
        private static void LaunchServer()
        {
            Networking.ServerAwaitingClientLoop(new NetworkAction(HandleClientConnection), 11000);
        }

        /// <summary>
        /// Helper method to the Update method which
        /// allows for the world to be updated indefinitely
        /// </summary>
        private static void UpdateLoop()
        {
            while (true)
            {
                Update();          
            }
        }

        /// <summary>
        /// Updates the world according to the update timer
        /// </summary>
        private static void Update()
        {
            updateWatch.Start();

            //Do nothing if not enough time has elapsed according to the time in GameSettings
            while (updateWatch.ElapsedMilliseconds < gameSettings.MSPerFrame) { }
            updateWatch.Reset();

            //Update the world
            world.Refresh();

            //Create the client message
            string clientData = CreateClientMessage();

            lock (connections)
            {
                LinkedListNode<SocketState> node = connections.First;
                
                //Iterate through all connections and send the updated data
                while (node != null)
                {
                    // When someone leaves the server, severe their existence in the server.
                    if (!(Networking.SendData(node.Value.theSocket, clientData)))
                    {
                        int clientID = (int)node.Value.ID;

                        //Kill that client's Ship indefinitely
                        world.KillShipForever(clientID);
                        
                        //Remove the connection from the list
                        LinkedListNode<SocketState> next = node.Next;
                        connections.Remove(node);
                        node = next;

                        //Display disconnect message
                        Console.WriteLine("Client " + clientID + " disconnected");
                    }
                    else
                        node = node.Next;
                }
            }
        }

        /// <summary>
        /// Helper method that creates a JSON message containing
        /// the world data that will be sent to the client
        /// </summary>
        /// <returns></returns>
        private static string CreateClientMessage()
        {
            StringBuilder clientData = new StringBuilder();

            //Retrieve the current world state, then send to the client
            foreach (Ship ship in world.GetAllShips())
                clientData.Append(ship.ToString() + "\n");

            foreach (Projectile projectile in world.GetAllProjectiles())
                clientData.Append(projectile.ToString() + "\n");

            foreach (Star star in world.GetAllStars())
                clientData.Append(star.ToString() + "\n");

            return clientData.ToString();
        }

        /// <summary>
        /// Initialize call me and add the server name as a new SocketState.
        /// Kindly ask for some more data.
        /// </summary>
        /// <param name="socketState"></param>
        private static void HandleClientConnection(SocketState socketState)
        {
            socketState.callMe = new NetworkAction(ReceivePlayerName);
            Networking.GetData(socketState);
        }

        /// <summary>
        /// Initialize call me and add the http connection as a new SocketState
        /// Kindly ask for some data regarding http protocol
        /// </summary>
        /// <param name="socketState"></param>
        private static void HandleHTTPConnection(SocketState socketState)
        {
            socketState.callMe = new NetworkAction(ProcessHttpRequest);
            Networking.GetData(socketState);
        }

        /// <summary>
        /// Get the id of the client and add a new ship of the client 
        /// to the server.
        /// </summary>
        /// <param name="socketState"></param>
        private static void ReceivePlayerName(SocketState socketState)
        {         
            //Retrieve the name of the player
            string name = Regex.Replace(socketState.sb.ToString().Trim(), "\\n|\\t|\\r", "");
            Socket socket = socketState.theSocket;

            socketState.callMe = new NetworkAction(HandleClientData);
            socketState.sb.Clear();

            //Create a new player object and add it to the world
            int newPlayerID = world.AddRandomShip(name);

            //Set the client's ID to the player's ID
            socketState.ID = newPlayerID;

            //Send the player's ID and the world size to the client
            PushStartUpData(socket, newPlayerID);

            //Add the client to the list of connections
            lock (connections)
                connections.AddLast(socketState);

            Networking.GetData(socketState);

        }

        /// <summary>
        /// Helps act as the http connection delegate to handle http protocols from 
        /// the client browser
        /// </summary>
        /// <param name="socketState"></param>
        private static void ProcessHttpRequest(SocketState socketState)
        {
            string httpProtocol = socketState.sb.ToString();
            
            //HTTP protocol is too short and likely invalid, so send error message on browser
            if (httpProtocol.IndexOf('\n') < 0)
            {
                Send404Response(socketState);
            }
            else
            {
                string str2 = httpProtocol.Trim();
                int num1 = str2.IndexOf("GET /games?player=");
                int num2 = str2.IndexOf("HTTP/1.1");
                if (num1 >= 0 && num2 > num1)
                {
                    //Getting a player name
                    int startIndex = num1 + "GET /games?player=".Length;
                    string player = str2.Substring(startIndex, num2 - startIndex - 1);
                    SendGamesForPlayer(socketState, player);
                }
                else
                {
                    //Getting game ID
                    int num3 = str2.IndexOf("GET /game?id=");
                    if (num3 >= 0 && num2 > num3)
                    {
                        int startIndex = num3 + "GET /game?id=".Length;
                        string id = str2.Substring(startIndex, num2 - startIndex - 1);
                        SendGame(socketState, id);
                    }
                    else
                    {
                        //Getting all data
                        int num4 = str2.IndexOf("GET /scores");
                        if (num4 >= 0 && num2 > num4)
                        {
                            int startIndex = num4 + "GET /scores".Length;
                            str2.Substring(startIndex, num2 - startIndex - 1);
                            SendAllData(socketState);
                        }
                        else
                            SendHomePage(socketState);
                    }
                }
            }
        }

        /// <summary>
        /// Helps send all of the data about the current state
        /// </summary>
        /// <param name="state">socket state</param>
        private static void SendAllData(SocketState state)
        {
            List<List<string>> allData = DatabaseHandler.GetOverallData();
            string str1 = "<table border=\"1\">";
            foreach (List<string> stringList in allData)
            {
                str1 += "<tr>";
                foreach (string str2 in stringList)
                    str1 = str1 + "<td>" + str2 + "</td>";
                str1 += "</tr>";
            }
            string content = str1 + "</table>";
            Networking.SendData(state.theSocket, "HTTP/1.1 200 OK\r\nConnection: close\r\nContent-Type: text/html; charset=UTF-8\r\n\r\n" + DatabaseHandler.WrapHtml(content));
        }

        /// <summary>
        /// Helps send information about of the players present in the current state
        /// </summary>
        /// <param name="state">current socket state</param>
        /// <param name="player">the specific player to send data</param>
        private static void SendGamesForPlayer(SocketState state, string player)
        {
            List<List<string>> playersGames = DatabaseHandler.GetPlayersGames(player);
            if (playersGames.Count <= 1)
            {
                Networking.SendData(state.theSocket, "HTTP/1.1 200 OK\r\nConnection: close\r\nContent-Type: text/html; charset=UTF-8\r\n\r\n" + DatabaseHandler.WrapHtml("<h2>No such data for \"" + player + "\"</h2>"));
            }
            else
            {
                string str1 = "<table border=\"1\">";
                foreach (List<string> stringList in playersGames)
                {
                    str1 += "<tr>";
                    foreach (string str2 in stringList)
                        str1 = str1 + "<td>" + str2 + "</td>";
                    str1 += "</tr>";
                }
                string content = str1 + "</table>";
                Networking.SendData(state.theSocket, "HTTP/1.1 200 OK\r\nConnection: close\r\nContent-Type: text/html; charset=UTF-8\r\n\r\n" + DatabaseHandler.WrapHtml(content));
            }
        }

        /// <summary>
        /// Sends all information regarding the current game in the current state
        /// </summary>
        /// <param name="state">current socket state</param>
        /// <param name="id">unique id that associates with the game ID</param>
        private static void SendGame(SocketState state, string id)
        {
            List<List<string>> game = DatabaseHandler.GetGame(id);
            if (game.Count <= 1)
            {
                Networking.SendData(state.theSocket, "HTTP/1.1 200 OK\r\nConnection: close\r\nContent-Type: text/html; charset=UTF-8\r\n\r\n" + DatabaseHandler.WrapHtml("<h2>No such game</h2>"));
            }
            else
            {
                string str1 = "<h3>Stats for game " + id + "</h3>" + "<table border=\"1\">";
                foreach (List<string> stringList in game)
                {
                    str1 += "<tr>";
                    foreach (string str2 in stringList)
                        str1 = str1 + "<td>" + str2 + "</td>";
                    str1 += "</tr>";
                }
                string content = str1 + "</table>";
                Networking.SendData(state.theSocket, "HTTP/1.1 200 OK\r\nConnection: close\r\nContent-Type: text/html; charset=UTF-8\r\n\r\n" + DatabaseHandler.WrapHtml(content));
            }
        }

        /// <summary>
        /// Sends home page message if receiving bad request
        /// </summary>
        /// <param name="state">the current socket state</param>
        private static void SendHomePage(SocketState state)
        {
            Networking.SendData(state.theSocket, "HTTP/1.1 200 OK\r\nConnection: close\r\nContent-Type: text/html; charset=UTF-8\r\n\r\nHi");
        }

        /// <summary>
        /// Sends error message if receiving bad request
        /// </summary>
        /// <param name="state">the current socket state</param>
        private static void Send404Response(SocketState state)
        {
            Networking.SendData(state.theSocket, "HTTP/1.1 404 Not Found\r\nConnection: close\r\nContent-Type: text/html; charset=UTF-8\r\n\r\nBad http request");
        }

        /// <summary>
        /// Private helper method for sending the startup information data.
        /// </summary>
        /// <param name="socket">the current socket</param>
        /// <param name="newPlayerID">the unique ID associated with a new player</param>
        private static void PushStartUpData(Socket socket, int newPlayerID)
        {
            Networking.SendData(socket, newPlayerID.ToString() + "\n" + gameSettings.UniverseSize + "\n");
        }

        /// <summary>
        /// This method parses out the command requests sent from the client and 
        /// adds them to the server.
        /// </summary>
        /// <param name="socketState">the current socket state associated with the client</param>
        private static void HandleClientData(SocketState socketState)
        {
            //Check if the Socketstate is valid
            int id = (int)socketState.ID;
            if (id == -1)
                return;

            //Split the commands according to the newline character
            char[] split = new char[1] { '\n' };
            String[] cmds = socketState.sb.ToString().Split(split, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                //Iterate through all commands
                foreach (String item in cmds)
                {
                    if (item.Length > 1 && item[0] == 40 && item[item.Length - 1] == 41)
                    {
                        string cmd = item.Substring(1, item.Length - 2);

                        world.ProcessCommand(id, cmd);
                    }
                    socketState.sb.Remove(0, item.Length + 1);
                }
                Networking.GetData(socketState);
            }
             catch(Exception e)
            {
                Console.WriteLine("Error receiving command from client: " + (object)e);
            }
           
        }

        /// <summary>
        /// Safe way to exit if something goes wrong.
        /// </summary>
        /// <param name="error"></param>
        public static void Exit(String error)
        {
            Console.WriteLine("error " + error);
            Console.Read();
            Environment.Exit(2);
        }
    }
}
