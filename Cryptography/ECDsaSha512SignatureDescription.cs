/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Security.Cryptography;

namespace Utilities.DotNet.Cryptography
{
    /// <summary>
    /// Contains the description of the ECDSA-SHA512 signature.
    /// </summary>
    public class ECDsaSha512SignatureDescription : ECDsaShaSignatureDescription
    {
        //===========================================================================
        //                           PUBLIC CONSTANTS
        //===========================================================================

        /// <summary>
        /// The name of the signature algorithm.
        /// </summary>
        public const string Name = "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha512";

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor.
        /// </summary>
        public ECDsaSha512SignatureDescription() : base( 385, int.MaxValue, () => SHA512.Create() )
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
                CryptoConfig.AddAlgorithm( typeof( ECDsaSha512SignatureDescription ), Name );
                g_registered = true;
            }
        }

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private static bool g_registered = false;
    }
}
