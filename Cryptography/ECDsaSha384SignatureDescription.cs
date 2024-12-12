/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Security.Cryptography;

namespace Utilities.DotNet.Cryptography
{
    /// <summary>
    /// Contains the description of the ECDSA-SHA384 signature.
    /// </summary>
    public class ECDsaSha384SignatureDescription : ECDsaShaSignatureDescription
    {
        //===========================================================================
        //                           PUBLIC CONSTANTS
        //===========================================================================

        /// <summary>
        /// The name of the signature algorithm.
        /// </summary>
        public const string Name = "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha384";

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor.
        /// </summary>
        public ECDsaSha384SignatureDescription() : base( 257, 384, () => SHA384.Create() )
        {
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <summary>
        /// Registers the signature algorithm.
        /// </summary>
        public static void Register()
        {
            if( !g_registered )
            {
                CryptoConfig.AddAlgorithm( typeof( ECDsaSha384SignatureDescription ), Name );
                g_registered = true;
            }
        }

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private static bool g_registered = false;
    }
}
