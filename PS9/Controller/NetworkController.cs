using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace Controller
{
    public delegate void NetworkAction(SocketState state);

    
    /// <summary>
    /// This class holds all the necessary state to represent a socket connection
    /// Note that all of its fields are public because we are using it like a "struct"
    /// It is a simple collection of fields
    /// </summary>
    public class SocketState
    {
        public Socket theSocket;
        public long ID = -1;

        // This is the buffer where we will receive data from the socket
        public byte[] messageBuffer = new byte[1024];

        // This is a larger (growable) buffer, in case a single receive does not contain the full message.
        public StringBuilder sb = new StringBuilder();

        // This is how the networking library will "notify" users when a connection is made, or when data is received.
        public NetworkAction callMe;

        public SocketState(Socket s, int id)
        {
            theSocket = s;
            ID = id;
        }
    }

    /// <summary>
    /// This class holds a TcpListener plus the delegate.
    /// </summary>
    public class ConnectionState
    {
        /// <summary>
        /// Used for the callback after performing networking operations
        /// </summary>
        public NetworkAction callMe;

        /// <summary>
        /// Used for receiving and maintaining connections to the server
        /// </summary>
        public TcpListener listener;

        /// <summary>
        /// Set the Socketstate and TcpListener.
        /// 
        /// </summary>
        /// <param name="listen"></param>
        /// <param name="action"></param>
        public ConnectionState(TcpListener listen, NetworkAction action)
        {
            this.listener = listen;
            this.callMe = action;
        }
    }

    public static class Networking
    {

        public const int DEFAULT_PORT = 11000;


        /// <summary>
        /// Creates a Socket object for the given host string
        /// </summary>
        /// <param name="hostName">The host name or IP address</param>
        /// <param name="socket">The created Socket</param>
        /// <param name="ipAddress">The created IPAddress</param>
        public static void MakeSocket(string hostName, out Socket socket, out IPAddress ipAddress)
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
                        System.Diagnostics.Debug.WriteLine("Invalid addres: " + hostName);
                        throw new ArgumentException("Invalid address");
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
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Unable to create socket. Error occured: " + e);
                throw new ArgumentException("Invalid address");
            }
        }

        /// <summary>
        /// Used to initialize the connection to the server
        /// </summary>
        /// <param name="callMe"></param>
        /// <param name="hostName"></param>
        /// <returns></returns>
        public static Socket ConnectToServer(NetworkAction callMe, string hostName)
        {
            System.Diagnostics.Debug.WriteLine("connecting  to " + hostName);

            // Create a TCP/IP socket.
            Socket socket;
            IPAddress ipAddress;

            Networking.MakeSocket(hostName, out socket, out ipAddress);

            SocketState state = new SocketState(socket, -1);

            state.callMe = callMe;
            state.theSocket.BeginConnect(ipAddress, Networking.DEFAULT_PORT, ConnectedCallback, state);

            return state.theSocket;
        }

        /// <summary>
        /// Method used to send data to the server with a Socket object
        /// </summary>
        /// <param name="state"></param>
        /// <param name="messageBytes"></param>
        public static bool SendData(Socket state, string data)
        {
            //If unable to send data, end the connection
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                state.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, SendCallback, state);
                return true;
            }
            catch (Exception)
            {
               state.Shutdown(SocketShutdown.Both);
               state.Close();
               return false;
            }
        }
        
        /// <summary>
        /// A callback invoked when a send operation completes
        /// </summary>
        /// <param name="ar"></param>
        private static void SendCallback(IAsyncResult ar)
        {
            Socket state = (Socket)ar.AsyncState;

            //Catch any disconnect exceptions
            try
            {
                // Nothing much to do here, just conclude the send operation so the socket is happy.
                state.EndSend(ar);
            }
            catch (Exception)
            {
                state.Shutdown(SocketShutdown.Both);
                state.Close();
            }
        }

        /// <summary>
        /// This function is "called" by the operating system when the remote site acknowledges connect request
        /// </summary>
        /// <param name="ar"></param>
        private static void ConnectedCallback(IAsyncResult ar)
        {
            SocketState state = (SocketState)ar.AsyncState;

            try
            {
                // Complete the connection.
                state.theSocket.EndConnect(ar);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Unable to connect to server. Error occured: " + e);
                return;
            }

            //Invoke client's delegate for it to decide what to do next
            state.callMe(state);
        }

        /// <summary>
        /// Default callback for receiving data and is used in the "event loop"
        /// </summary>
        /// <param name="ar"></param>
        private static void ReceiveCallback(IAsyncResult ar)
        {
            
            SocketState state = (SocketState)ar.AsyncState;

            //Use try catch to get any errors when disconnecting from server
            try
            {
                int bytesRead = state.theSocket.EndReceive(ar);

                // If the socket is still open
                if (bytesRead > 0)
                {
                    string theMessage = Encoding.UTF8.GetString(state.messageBuffer, 0, bytesRead);

                    // Append the received data to the growable buffer.
                    // It may be an incomplete message, so we need to start building it up piece by piece
                    state.sb.Append(theMessage);

                }
                state.callMe(state);
            }
            catch (Exception) { }
        }
        
        /// <summary>
        /// GetData is just a wrapper for BeginReceive.
        /// This is the public entry point for asking for data.
        /// Necessary so that we can separate networking concerns from client concerns.
        /// </summary>
        /// <param name="state"></param>
        public static void GetData(SocketState state)
        {
            state.theSocket.BeginReceive(state.messageBuffer, 0, state.messageBuffer.Length, SocketFlags.None, ReceiveCallback, state);
        }


        //************************* Server Methods *************************//


        /// <summary>
        /// This waits for the client to establish a connection
        /// with the server in which we create a new connection via
        /// TCPListener.
        /// 
        /// </summary>
        /// <param name=""></param>
        public static void ServerAwaitingClientLoop(NetworkAction callMe, int port)
        {
            Console.WriteLine("Server is up. Awaiting first client");
            TcpListener tcp = new TcpListener(IPAddress.Any, port);
           

            // try to create a new connection and begin the connection handshake
            try
            {
                tcp.Start();
                ConnectionState state = new ConnectionState(tcp, callMe);
                tcp.BeginAcceptSocket(new AsyncCallback(AcceptNewClient), state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            
        }

        /// <summary>
        /// BeginAcceptSocket should use. 
        /// This code will be invoked by the OS when a connection
        /// request comes in. 
        /// 
        /// </summary>
        /// <param name="ar"></param>
        public static void AcceptNewClient(IAsyncResult ar)
        {
            // New client... yay create a socket, EndAcceptSocket then begin connection .
            Console.WriteLine("A new client has contacted the Server.");
            ConnectionState connection_state = (ConnectionState)ar.AsyncState;
            Socket socket = null;
            SocketState state = new SocketState(socket, -1);

            //Attempt to accept the new client, catch any errors that occur
            try
            {
                socket = connection_state.listener.EndAcceptSocket(ar);
                socket.NoDelay = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                connection_state.listener.BeginAcceptSocket(new AsyncCallback(AcceptNewClient), connection_state);
                return;
            }

            state.callMe = connection_state.callMe;
            state.theSocket = socket;
            state.callMe(state);
            connection_state.listener.BeginAcceptSocket(new AsyncCallback(AcceptNewClient), connection_state);


        }
    }
}

