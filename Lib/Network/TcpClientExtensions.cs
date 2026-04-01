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
    /// Utility class providing extension methods for the <see cref="TcpClient"/> class.
    /// </summary>
    public static class TcpClientExtensions
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <summary>
        /// Establishes a connection to a remote TCP host.
        /// </summary>
        /// <param name="client">TCP client used to establish connection.</param>
        /// <param name="address">The IPAddress of the remote host to connect to.</param>
        /// <param name="port">The port on the remote host to connect to.</param>
        /// <param name="timeout">The number of milliseconds to wait for connection to be established,
        ///                       or <see cref="Timeout.Infinite"/> to wait indefinitely.</param>
        /// <exception cref="SocketException">An error occurred when accessing the socket.</exception>
        /// <exception cref="ObjectDisposedException">The <see cref="TcpClient"/> has been disposed.</exception>
        public static void Connect( this TcpClient client, IPAddress address, int port, int timeout )
        {
            if( timeout == Timeout.Infinite )
            {
                client.Connect( address, port );
            }
            else
            {
                Task connectTask;

                try
                {
                    connectTask = client.ConnectAsync( address, port );

                    if( !connectTask.Wait( timeout ) )
                    {
                        client.Close();
                        throw new SocketException( (int) SocketError.TimedOut );
                    }
                }
                catch( AggregateException ex )
                {
                    client.Close();
                    throw ex.InnerException ?? ex;
                }
                catch( Exception )
                {
                    client.Close();
                    throw;
                }

                Debug.Assert( !connectTask.IsFaulted );
                Debug.Assert( client.Connected );
            }
        }
    }
}
