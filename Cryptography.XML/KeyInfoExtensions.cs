/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Linq;
using System.Security.Cryptography.Xml;

namespace Utilities.DotNet.Cryptography.XML
{
    /// <summary>
    /// Extension methods for <see cref="KeyInfo"/>.
    /// </summary>
    public static class KeyInfoExtensions
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <summary>
        /// Gets the subject key identifier from the key info.
        /// </summary>
        /// <param name="keyInfo">A KeyInfo instance.</param>
        /// <returns>Subject key identifier or <c>null</c> if not found.</returns>
        public static string? GetSubjectKeyIdentifier( this KeyInfo keyInfo )
        {
            var keyIdArray = (byte[]?) keyInfo.OfType<KeyInfoX509Data>().FirstOrDefault()?.SubjectKeyIds?[ 0 ];
            return ( keyIdArray == null ) ? null : keyIdArray.ToHexString();
        }

        /// <summary>
        /// Gets the subject key identifier bytes from the key info.
        /// </summary>
        /// <param name="keyInfo">A KeyInfo instance.</param>
        /// <returns>Subject key identifier bytes or <c>null</c> if not found.</returns>
        public static ReadOnlyMemory<byte>? GetSubjectKeyIdentifierBytes( this KeyInfo keyInfo )
        {
            byte[]? keyIdArray = (byte[]?) keyInfo.OfType<KeyInfoX509Data>().FirstOrDefault()?.SubjectKeyIds?[ 0 ];
            return ( keyIdArray == null ) ? (ReadOnlyMemory<byte>?) null : new ReadOnlyMemory<byte>( keyIdArray );
        }
    }
}
