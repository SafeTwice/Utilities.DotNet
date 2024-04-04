/// @file
/// @copyright  Copyright (c) 2018-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Utilities.Net.Network
{
    /// <summary>
    /// {Enter brief class description here...}
    /// </summary>
    public static class NetworkUtilities
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        public static IPAddress? GetIP4Address( string hostname )
        {
            try
            {
                return IPAddress.Parse( hostname );
            }
            catch
            {
                var hostEntry = Dns.GetHostEntry( hostname );

                return hostEntry.AddressList.Where( ipAddress => ( ipAddress.AddressFamily == AddressFamily.InterNetwork ) ).FirstOrDefault();
            }
        }
    }
}
