using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Controller
{
    /// <summary>
    /// Static class used to update the values of objects in the game world
    /// </summary>
    public static class ClientUpdater
    {
        /// <summary>
        /// Analyzes message received from the server and update the objects in the game
        /// </summary>
        /// <param name="rData">JSON message from server</param>
        /// <param name="theWorld">The reference to the world object</param>
        public static void ConvertData(string rData, ref World theWorld)
        {
            //Parse the received data
            JObject gameObj = JObject.Parse(rData);

            //Grab the JSON element type which will be used to check the type of object
            JToken posShip = gameObj["ship"];
            JToken posStar = gameObj["star"];
            JToken posProj = gameObj["proj"];

            //Check the type of object
            if (posShip != null)
            {
                Ship deShip = JsonConvert.DeserializeObject<Ship>(rData);

                //Retreive the Ship's ID which is used for storage in the dictionary
                int objID = deShip.GetID();

                //Add the ship to the dictionary
                theWorld.AddShip(objID, deShip);

            }
            else if (posProj != null)
            {
                //Deserialize the Json string and add/update the projectile object to the dictionary
                Projectile deProj = JsonConvert.DeserializeObject<Projectile>(rData);

                //Retreive the projectile's ID which is used for storage in the dictionary
                int objID = deProj.GetID();
                bool isAlive = deProj.GetAlive();

                //Check if the projectilve is alive. If it is not alive, try to remove it from dictionary. Update or add projectile otherwise
                if (!isAlive)
                {
                    theWorld.RemoveProj(objID);
                }
                else
                {
                    theWorld.AddProj(objID, deProj);
                }
            }
            else if (posStar != null)
            {
                //Deserialize the Json string and add/update the star object to the dictionary
                Star deStar = JsonConvert.DeserializeObject<Star>(rData);

                //Retreive the star's ID which is used for storage in the dictionary then add/update the value in the dictionary
                int objID = deStar.GetID();
                theWorld.AddStar(objID, deStar);
            }
        }
    }
}
