/// @file
/// @copyright  Copyright (c) 2018-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Utilities.Net.Network
{
    /// <summary>
    /// Utility class for network-related operations.
    /// </summary>
    public static class NetworkUtilities
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <summary>
        /// Gets the IPv4 address of a host.
        /// </summary>
        /// <param name="hostname">Name of the host (or its IPv4 address).</param>
        /// <returns>The IPv4 address of the host, or <see langword="null"/> if it could not be resolved.</returns>
        public static IPAddress? GetIP4Address( string hostname )
        {
            if( IPAddress.TryParse( hostname, out var ipAddress ) )
            {
                return ipAddress;
            }
            else
            {
                var hostEntry = Dns.GetHostEntry( hostname );

                return hostEntry.AddressList.Where( ipAddress => ( ipAddress.AddressFamily == AddressFamily.InterNetwork ) ).FirstOrDefault();
            }
        }
    }
}
