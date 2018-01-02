///
/// @authors Tony Diep and Sona Torosyan
///
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Contains the SocketState class
/// </summary>
namespace NetworkController
{
    /// <summary>
    /// A delegate to be called when a connection is made
    /// </summary>
    /// <param name="state"></param>
    public delegate void callMe(SocketState state);

    /// <summary>
    /// A storage blueprint for keeping track of current data and socket
    /// </summary>
    public class SocketState
    {
        //Current socket
        private Socket theSocket;

        //This will be used in PS8, but for now it's set to -1 for placeholding
        private int ID;

        //Stores data in byte form
        private byte[] messageBuffer;

        //Appends incoming data to strings
        private StringBuilder messageBuilder;

        //Delegate to be called when connection is made
        private callMe callMeDelegate;

        //Keeps track if a server can be found to connect
        private bool NoServerFound;

        //Keeps track of whether we the server name retrieved is valid
        private bool IsValidServerName;

        /// <summary>
        /// Creates a new storage for the current socket and ID
        /// </summary>
        /// <param name="socket">the socket to save in this storage</param>
        /// <param name="id">useful for server in PS8 but for now it's -1</param>
        public SocketState(Socket socket, int id)
        {
            messageBuffer = new Byte[1000];
            messageBuilder = new StringBuilder();
            theSocket = socket;
            ID = id;
            NoServerFound = false;
            IsValidServerName = true;
        }

        /// <summary>
        /// Gets the socket contained in this state
        /// </summary>
        /// <returns>socket</returns>
        public Socket GetSocket()
        {
            return theSocket;
        }

        /// <summary>
        /// Gets the message buffer contained in this state
        /// </summary>
        /// <returns>the message buffer</returns>
        public byte[] GetMessageBuffer()
        {
            return messageBuffer;
        }

        /// <summary>
        /// Gets the message builder contained in this state
        /// </summary>
        /// <returns>the message builder</returns>
        public StringBuilder GetStringBuilder()
        {
            return messageBuilder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        public void UpdateStringBuilder(int startIndex, int endIndex)
        {
            messageBuilder.Remove(startIndex, endIndex);
        }

        /// <summary>
        /// Gets the delegate contained in this state
        /// </summary>
        /// <returns>the delegate</returns>
        public callMe GetCallMeDelegate()
        {
            return callMeDelegate;
        }

        /// <summary>
        /// Updates the delegate contained in this state to a different method
        /// to represent the delegate
        /// </summary>
        /// <param name="newDelegate">the new method to represent as the delegate</param>
        public void SetCallMeDelegate(callMe newDelegate)
        {
            callMeDelegate = newDelegate;
        }

        /// <summary>
        /// Gets the current status of connecting to server errors 
        /// </summary>
        /// <returns>the server error flag</returns>
        public bool CantConnectToServer()
        {
            return NoServerFound;
        }

        /// <summary>
        /// Updates the flag to true when error connecting to server happened
        /// </summary>
        public void UpdateToErrorFound()
        {
            NoServerFound = true;
        }

        /// <summary>
        /// Updates the flag to false when error connecting to server didn't happen
        /// </summary>
        public void UpdateToErrorNotFound()
        {
            NoServerFound = false;
        }

        /// <summary>
        /// Reports whether the server name retrieved is valid or not
        /// </summary>
        /// <returns>true if valid and false otherwise</returns>
        public bool ValidServerName()
        {
            return IsValidServerName;
        }

        /// <summary>
        /// Updates the server name flag to false when we find that the
        /// server name is invalid
        /// </summary>
        public void UpdateToNotValidServerName()
        {
            IsValidServerName = false;
        }

        /// <summary>
        /// Gets the ID of this socket state
        /// </summary>
        /// <returns>the ID</returns>
        public int GetID()
        {
            return ID;
        }

        /// <summary>
        /// Sets the current ID to a new ID
        /// </summary>
        /// <param name="newID">the new ID to update to</param>
        public void SetID(int newID)
        {
            ID = newID;
        }
    }
}