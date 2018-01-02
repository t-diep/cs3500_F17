///
/// @authors Tony Diep and Sona Torosyan
///
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Contains the general networking library; utilize for setting up
/// sockets and retrieving data
/// </summary>
namespace NetworkController
{
    /// <summary>
    /// General networking library
    /// </summary>
    public static class Networking
    {
        //Our server for this assignment
        public const int DEFAULT_PORT = 11000;

        /// <summary>
        /// Attempts to connect to a server through a provided hostname
        /// </summary>
        /// <param name="action">a delegate to be called when server is connected</param>
        /// <param name="hostname">the host name</param>
        /// <returns>a socket that is responsible for connecting to a server</returns>
        public static Socket ConnectToServer(callMe action, string hostname)
        {
            Socket socket;
            IPAddress ipAddress;

            MakeSocket(hostname, out socket, out ipAddress);

            //Create a socket state for current socket
            SocketState theServer = new SocketState(socket, -1);
            theServer.SetCallMeDelegate(action);
            theServer.GetSocket().BeginConnect(ipAddress, Networking.DEFAULT_PORT, ConnectedToServer, theServer);

            //Return socket that established the connection to server
            return socket;
        }

        /// <summary>
        /// Helper to make a socket; used inside ConnectToServer; this is directly from the Lec 19
        /// code; this is inspired by Daniel Kopta's recommendation to make this helper method
        /// </summary>
        /// <param name="hostName">provided host name</param>
        /// <param name="socket">socket storage to create the actual socket</param>
        /// <param name="ipAddress">IP address</param>
        private static void MakeSocket(string hostName, out Socket socket, out IPAddress ipAddress)
        {
            ipAddress = IPAddress.None;
            socket = null;

            try
            {
                // Establish the remote endpoint for the socket.
                IPHostEntry ipHostInfo;

                // Determine if the server address is a URL or an IP
                try
                {
                    ipHostInfo = Dns.GetHostEntry(hostName);
                    bool foundIPV4 = false;
                    foreach (IPAddress addr in ipHostInfo.AddressList)
                        if (addr.AddressFamily != AddressFamily.InterNetworkV6)
                        {
                            foundIPV4 = true;
                            ipAddress = addr;
                            break;
                        }
                    // Didn't find any IPV4 addresses
                    if (!foundIPV4)
                    {
                        System.Diagnostics.Debug.WriteLine("Invalid address: " + hostName);
                        throw new Exception("Invalid address");
                    }
                }
                catch (Exception)
                {
                    // see if host name is actually an ipaddress, i.e., 155.99.123.456
                    System.Diagnostics.Debug.WriteLine("using IP");
                    ipAddress = IPAddress.Parse(hostName);
                }

                // Create a TCP/IP socket.
                socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);

                // Disable Nagle's algorithm - can speed things up for tiny messages, 
                // such as for a game
                socket.NoDelay = true;
            }
            catch (Exception)
            {
                throw new Exception("Unable to connect to server");
            }
        }

        /// <summary>
        /// From an asynchronous result, extracts a socket and calls the delegate to start receive data;
        /// this code is taken directly from Lec 19 demo code
        /// </summary>
        /// <param name="stateAsArObject"></param>
        public static void ConnectedToServer(IAsyncResult stateAsArObject)
        {
            SocketState socketState = (SocketState)stateAsArObject.AsyncState;

            try
            {
                // Complete the connection.
                socketState.GetSocket().EndConnect(stateAsArObject);

                //No error on connecting to server, so update flag to true
                socketState.UpdateToErrorNotFound();
            }
            catch (Exception)
            {
                //Found an error when connecting to server, so update flag to false
                socketState.UpdateToErrorFound();
            }
            finally
            {
                //Call the delegate that's between client and server
                socketState.GetCallMeDelegate()(socketState);
            }
        }

        /// <summary>
        /// Retrieve data from the socket contained in the current socket state; taken directly 
        /// from Lec 19 code
        /// </summary>
        /// <param name="socketState">current socket state</param>
        public static void GetData(SocketState socketState)
        {
            socketState.GetSocket().BeginReceive(socketState.GetMessageBuffer(), 0, socketState.GetMessageBuffer().Length, SocketFlags.None, ReceiveCallback, socketState);
        }

        /// <summary>
        /// Called by the OS when new data arrives; taken directly from Lec 19 code
        /// </summary>
        /// <param name="stateAsArObject">asynchronous result</param>
        public static void ReceiveCallback(IAsyncResult stateAsArObject)
        {
            try
            {
                SocketState socketState = (SocketState)stateAsArObject.AsyncState;

                //Disconnect exception happened in "bytesRead"
                int bytesRead = socketState.GetSocket().EndReceive(stateAsArObject);

                // If the socket is still open
                if (bytesRead > 0)
                {
                    string theMessage = Encoding.UTF8.GetString(socketState.GetMessageBuffer(), 0, bytesRead);
                    // Append the received data to the growable buffer.
                    // It may be an incomplete message, so we need to start building it up piece by piece
                    socketState.GetStringBuilder().Append(theMessage);

                    //Activate delegate from current socket state
                    socketState.GetCallMeDelegate()(socketState);
                }
            }
            catch(Exception)
            {
                //Don't call the SocketState's callMe delegate
            }          
        }

        /// <summary>
        /// Sends data in byte form to the socket; taken directly from Lec 19 code
        /// </summary>
        /// <param name="socket">socket</param>
        /// <param name="data">message text in which to convert to byte form</param>
        public static bool Send(Socket socket, String data)
        {

            try
            {
                byte[] messageBytes = Encoding.UTF8.GetBytes(data);
                socket.BeginSend(messageBytes, 0, messageBytes.Length, SocketFlags.None, SendCallback, socket);
                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// When data comes out, extracts a socket from an asynchronous event; taken directly 
        /// from Lec 19 code
        /// </summary>
        /// <param name="ar">asynchronous result</param>
        public static void SendCallback(IAsyncResult ar)
        {
            Socket s = (Socket)ar.AsyncState;
            s.EndSend(ar);
        }

        /// <summary>
        /// Allows the server to continuously wait for new clients that want to 
        /// join that server
        /// </summary>
        /// <param name="callMeDelegate">the delegate that connects from incoming clients to this server</param>
        public static void ServerAwaitingClientLoop(callMe callMeDelegate)
        {
            //Construct a connection listener on any IPAddress with the localhost port
            TcpListener tcpListener = new TcpListener(IPAddress.Any, Networking.DEFAULT_PORT);
            
            //Start listening for connections
            tcpListener.Start();

            //Create a storage state to keep track of current connection listener
            TcpState tcpState = new TcpState(tcpListener, callMeDelegate);

            //Activates callback parameter, which allows new clients to connect to this server
            tcpListener.BeginAcceptSocket(AcceptNewClient, tcpState);
        }

        /// <summary>
        /// Accepts new clients based on a TcpListener and a socket state
        /// </summary>
        /// <param name="asyncRes">the asychronous result</param>
        public static void AcceptNewClient(IAsyncResult asyncRes)
        {
            //Extract a tcp state from the asynchronous result
            TcpState tcpStateRes = (TcpState)asyncRes.AsyncState;

            //Create socket from the tcp state
            Socket socket = tcpStateRes.GetTcpListener().EndAcceptSocket(asyncRes);

            //Store this socket into a socket state
            SocketState socketState = new SocketState(socket, -1);

            //Update callback delegate from tcp state to the socket state
            socketState.SetCallMeDelegate(tcpStateRes.GetCallMeDelegate());
            
            //Start invoking the callback delegate from the current socket state
            socketState.GetCallMeDelegate()(socketState);

            //Continue to accept new connections coming from new clients
            tcpStateRes.GetTcpListener().BeginAcceptSocket(AcceptNewClient, tcpStateRes);
        }
    }
}