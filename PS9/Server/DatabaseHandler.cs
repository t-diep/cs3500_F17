using Model;
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Server
{
    /// <summary>
    /// Database PS9 Extra Credit.
    /// </summary>
    public static class DatabaseHandler
    {
        private const string connectionBase = "server=atr.eng.utah.edu;" +
            "database=cs3500_u0934661;" +
            "uid=cs3500_u0934661;" +
            "password=tonytheTIGER96";

        /// <summary>
        /// Sends game data of the current world with the total duration seconds
        /// </summary>
        /// <param name="durationSeconds"></param>
        /// <param name="world"></param>
        public static void SendGame(long durationSeconds, World world)
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(connectionBase))
            {
                try
                {                  
                    mySqlConnection.Open();
                    MySqlCommand cmd = mySqlConnection.CreateCommand();
                    cmd.CommandText = "insert into GameDurations (Duration) values (" + durationSeconds.ToString() + ");";
                    Console.WriteLine("adding game");
                    cmd.ExecuteNonQuery();
                    MySqlCommand command2 = mySqlConnection.CreateCommand();
                    command2.CommandText = "select GameID from GameDurations;";
                    int num1 = -1;
                    Console.WriteLine("getting game ID");
                    using (MySqlDataReader mySqlDataReader = command2.ExecuteReader())
                    {
                        mySqlDataReader.Read();
                        num1 = mySqlDataReader.GetInt32(0);
                    }                   
                    
                    foreach (Ship ship in world.GetAllShips())
                    {
                        MySqlCommand command3 = mySqlConnection.CreateCommand();

                        double num2 = ((double)ship.GetHits() / (double)ship.GetTotalShots() * 100.0);

                        if(num2.ToString() == "NaN")
                        {
                            num2 = 0;
                        }

                        command3.CommandText = "insert into PlayersByGame (Player, Score, Accuracy, Duration) values (\"" + ship.GetName() + "\", " + ship.GetScore() + ", " + num2 + ", " + durationSeconds.ToString() + ");";
                        Console.WriteLine("adding player by game");
                        command3.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Gets the games associated with a specified player
        /// </summary>
        /// <param name="player">the player to look up game data</param>
        /// <returns>The list of data associated with the specified player</returns>
        public static List<List<string>> GetPlayersGames(string player)
        {
            List<List<string>> stringListList = new List<List<string>>();
            Console.WriteLine("connecting to database");
            using (MySqlConnection mySqlConnection = new MySqlConnection(connectionBase))
            {
                try
                {
                    Console.WriteLine("connecting");
                    mySqlConnection.Open();
                    MySqlCommand command = mySqlConnection.CreateCommand();
                    stringListList.Add(new List<string>());

                    stringListList[0].Add("Player");
                    stringListList[0].Add("GameID");                  
                    stringListList[0].Add("Score");
                    stringListList[0].Add("Accuracy");
                    stringListList[0].Add("Duration");

                    string str = "select PlayersByGame.Player, GameDurations.GameID, PlayersByGame.Score, PlayersByGame.Accuracy, PlayersByGame.Duration from PlayersByGame join GameDurations where PlayersByGame.Player =\'" + player+ "\'";

                    command.CommandText = str;
                    Console.WriteLine("getting games for player " + player);
                    using (MySqlDataReader mySqlDataReader = command.ExecuteReader())
                    { 
                        while (mySqlDataReader.Read())
                        {
                            stringListList.Add(new List<string>());
                            for (int index = 0; index < mySqlDataReader.FieldCount; ++index)
                                stringListList[stringListList.Count - 1].Add(mySqlDataReader[index].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            return stringListList;
        }

        /// <summary>
        /// Gets all of the data present without any restriction ordered by scores in descending order
        /// </summary>
        /// <returns>the list of all data with no restrictions</returns>
        public static List<List<string>> GetOverallData()
        {
            List<List<string>> stringListList = new List<List<string>>();
            
            Console.WriteLine("connecting to database");
            using (MySqlConnection mySqlConnection = new MySqlConnection(connectionBase))
            {
                try
                {
                    Console.WriteLine("connecting");
                    mySqlConnection.Open();
                    MySqlCommand command = mySqlConnection.CreateCommand();
                    stringListList.Add(new List<string>());

                    stringListList[0].Add("GameID");
                    stringListList[0].Add("Player");
                    stringListList[0].Add("Score");
                    stringListList[0].Add("Accuracy");
                    stringListList[0].Add("Duration");
                    
                    string str = "select GameDurations.GameID, PlayersByGame.Player, PlayersByGame.Score, PlayersByGame.Accuracy, PlayersByGame.Duration from PlayersByGame join GameDurations order by Score desc;";
                    command.CommandText = str;
                    Console.WriteLine("getting all data");
                    using (MySqlDataReader mySqlDataReader = command.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            stringListList.Add(new List<string>());
                            for (int index = 0; index < mySqlDataReader.FieldCount; ++index)
                                stringListList[stringListList.Count - 1].Add(mySqlDataReader[index].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            return stringListList;
        }

        /// <summary>
        /// Encapsulates some string content into a html
        /// </summary>
        /// <param name="content">the content to add the html to display on browser</param>
        /// <returns></returns>
        public static string WrapHtml(string content)
        {
            return "<!DOCTYPE html><html><head><title>SpaceWars</title></head><body>" + content + "</body></html>";
        }

        /// <summary>
        /// Gets all of the current information about a particular game 
        /// </summary>
        /// <param name="id">the specific id mapping to a game number</param>
        /// <returns></returns>
        public static List<List<string>> GetGame(string id)
        {
            List<List<string>> stringListList = new List<List<string>>();
            
            Console.WriteLine("connecting to database");
            using (MySqlConnection mySqlConnection = new MySqlConnection(connectionBase))
            {
                try
                {
                    Console.WriteLine("connecting");
                    mySqlConnection.Open();
                    MySqlCommand command = mySqlConnection.CreateCommand();
                    stringListList.Add(new List<string>());
                    stringListList[0].Add("GameID");
                    stringListList[0].Add("Player");
                    stringListList[0].Add("Score");
                    stringListList[0].Add("Accuracy");
                    stringListList[0].Add("Duration");

                    string str = "select GameDurations.GameID, PlayersByGame.Player, PlayersByGame.Score, PlayersByGame.Accuracy, PlayersByGame.Duration from GameDurations, PlayersByGame where GameDurations.GameID = " + id + " order by Score desc;";
                    command.CommandText = str;
                    Console.WriteLine("getting game ID " + id);
                    using (MySqlDataReader mySqlDataReader = command.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            stringListList.Add(new List<string>());
                            for (int index = 0; index < mySqlDataReader.FieldCount; ++index)
                                stringListList[stringListList.Count - 1].Add(mySqlDataReader[index].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            return stringListList;
        }

       
    }

}

