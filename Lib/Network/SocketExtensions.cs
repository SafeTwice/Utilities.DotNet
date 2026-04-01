/// @file
/// @copyright  Copyright (c) 2026 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Utilities.DotNet.Network
{
    /// <summary>
    /// Utility class providing extension methods for the <see cref="Socket"/> class.
    /// </summary>
    public static class SocketExtensions
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <summary>
        /// Establishes a connection to a remote host.
        /// </summary>
        /// <param name="socket">Socket used to establish connection.</param>
        /// <param name="address">The IPAddress of the remote host to connect to.</param>
        /// <param name="port">The port on the remote host to connect to.</param>
        /// <param name="timeout">The number of milliseconds to wait for connection to be established,
        ///                       or <see cref="Timeout.Infinite"/> to wait indefinitely.</param>
        /// <exception cref="SocketException">An error occurred when accessing the socket.</exception>
        /// <exception cref="System.ObjectDisposedException">The <see cref="Socket"/> has been disposed.</exception>
        /// <exception cref="System.Security.SecurityException">A caller higher in the call stack does not have permission for the requested operation.</exception>
        /// <exception cref="System.InvalidOperationException">The <see cref="Socket"/> has been placed in a listening state by calling <see cref="Socket.Listen(int)"/>.</exception>
        public static void Connect( this Socket socket, IPAddress address, int port, int timeout )
        {
            if( timeout == Timeout.Infinite )
            {
                socket.Connect( address, port );
            }
            else
            {
                Task connectTask;

                try
                {
                    connectTask = socket.ConnectAsync( address, port );

                    if( !connectTask.Wait( timeout ) )
                    {
                        socket.Close();
                        throw new SocketException( (int) SocketError.TimedOut );
                    }
                }
                catch( AggregateException ex )
                {
                    socket.Close();
                    throw ex.InnerException ?? ex;
                }
                catch( Exception )
                {
                    socket.Close();
                    throw;
                }

                Debug.Assert( !connectTask.IsFaulted );
                Debug.Assert( socket.Connected );
            }
        }
    }
}
