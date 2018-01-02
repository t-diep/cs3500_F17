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
/// Contains the tcp state blueprint
/// </summary>
namespace NetworkController
{
    /// <summary>
    /// A blueprint to represent a storage state for TcpListener
    /// </summary>
    public class TcpState
    {
        //The tcp listener contained in this state
        private TcpListener tcpListener;

        //The call me delegate contained in this state
        private callMe callMeDelegate;

        /// <summary>
        /// Creates a new tcp state
        /// </summary>
        /// <param name="tcpListener">tcp listener</param>
        /// <param name="callMeDelegate">call me delegate</param>
        public TcpState(TcpListener tcpListener, callMe callMeDelegate)
        {
            this.tcpListener = tcpListener;
            this.callMeDelegate = callMeDelegate;
        }

        /// <summary>
        /// Gets the curent tcp listener contained in this state
        /// </summary>
        /// <returns>tcp listener</returns>
        public TcpListener GetTcpListener()
        {
            return tcpListener;
        }

        /// <summary>
        /// Gets the current call me delegate contained in this state
        /// </summary>
        /// <returns>call me delegate</returns>
        public callMe GetCallMeDelegate()
        {
            return callMeDelegate;
        }

        /// <summary>
        /// Updates current call me delegate in this state to a new call me delegate
        /// </summary>
        /// <param name="newCallMeDelegate">new call me delegate</param>
        public void UpdateCallMeDelegate(callMe newCallMeDelegate)
        {
            callMeDelegate = newCallMeDelegate;
        }

        /// <summary>
        /// Updates current tcp listener in this state to a new tcp listener
        /// </summary>
        /// <param name="newTcpListener">new tcp listener</param>
        public void UpdateTcpListener(TcpListener newTcpListener)
        {
            tcpListener = newTcpListener;
        }
    }
}
