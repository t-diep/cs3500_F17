using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using System.Xml;

namespace Server
{
    /// <summary>
    /// This class contains the settings of the server
    /// </summary>
    public class GameSettings
    {
        /// <summary>
        /// The list of all of the stars in the file
        /// </summary>
        public List<Star> starList { get; protected set; }

        /// <summary>
        /// The size of the World
        /// </summary>
        public int UniverseSize { get; protected set; }

        /// <summary>
        /// The number of frames between each ship's shot (fire rate)
        /// </summary>
        public int FramesPerShot { get; protected set; }

        /// <summary>
        /// The respawn rate of a Ship when its health reaches zero
        /// </summary>
        public int RespawnRate { get; protected set; }

        /// <summary>
        /// The number of milliseconds between each frame update
        /// </summary>
        public int MSPerFrame { get; protected set; }

        /// <summary>
        /// Whether to enable moving stars (extra game mode)
        /// </summary>
        public bool MovingStars { get; protected set; }

   

        private GameSettings()
        {
            //Initialize to default values if not specified
            //by XML document
            starList = new List<Star>();
            UniverseSize = 750;
            FramesPerShot = 16;
            RespawnRate = 300;
            MSPerFrame = 16;
            MovingStars = false;
        }

        /// <summary>
        /// This method reads the settings.xml and sets the config.
        /// to the settings that will be used in the server.
        /// If the file cannot be read then the program will exit gracefully.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static GameSettings ReadSettingsFile(string filePath)
        {
            GameSettings servSettings = new GameSettings();
            //Use try/catch to get any exceptions that are thrown
            try
            {
                //Make sure the reader ignores whitespace
                XmlReaderSettings settings = new XmlReaderSettings { IgnoreWhitespace = true };

                using (XmlReader settingsReader = XmlReader.Create(filePath, settings))
                {
                    //Loop through all of the XML file elements
                    while (settingsReader.Read())
                    {
                        //Read the starting elements of the XML
                        if (settingsReader.IsStartElement())
                        {
                            //Get the element name and perform the proper actions
                            switch (settingsReader.Name)
                            {
                                case "UniverseSize":
                                    settingsReader.Read();
                                    servSettings.UniverseSize = int.Parse(settingsReader.Value);
                                    break;

                                case "MSPerFrame":
                                    settingsReader.Read();
                                    servSettings.MSPerFrame = int.Parse(settingsReader.Value);
                                    break;

                                case "FramesPerShot":
                                    settingsReader.Read();
                                    servSettings.FramesPerShot = int.Parse(settingsReader.Value);
                                    break;

                                case "RespawnRate":
                                    settingsReader.Read();
                                    servSettings.RespawnRate = int.Parse(settingsReader.Value);
                                    break;

                                case "MovingStars":
                                    settingsReader.Read();
                                    servSettings.MovingStars = bool.Parse(settingsReader.Value);
                                    break;

                                case "Star":
                                    //Create Xml only containing the contents of the particular Star
                                    XmlReader innerXml = settingsReader.ReadSubtree();

                                    double starX = 0;
                                    double starY = 0;
                                    double starMass = 0;
                                    
                                    //Process the contents inside of the Star
                                    while (innerXml.Read())
                                    {
                                        if (innerXml.ReadToFollowing("x"))
                                        {
                                            innerXml.Read();
                                            starX = double.Parse(innerXml.Value);
                                        }

                                        if (innerXml.ReadToFollowing("y"))
                                        {
                                            innerXml.Read();
                                            starY = double.Parse(innerXml.Value);
                                        }

                                        if (innerXml.ReadToFollowing("mass"))
                                        {
                                            innerXml.Read();
                                            starMass = double.Parse(innerXml.Value);
                                        }
                                    }
                                    //Create and add the Star to the StarList
                                    Star parsedStar = new Star(new Vector2D(starX, starY), starMass);
                                    servSettings.starList.Add(parsedStar);
                                    break;

                                default:
                                    break;
                            }
                        }

                    }
                }
            }
            catch (Exception)
            {
                SpaceWarsServer.Exit("Unable to read file " + filePath);
            }

            return servSettings;
        }
    }
}
