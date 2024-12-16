/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Security.Cryptography.Xml;

namespace Utilities.DotNet.Cryptography.XML
{
    /// <summary>
    /// Extension methods for <see cref="SignedXml"/>.
    /// </summary>
    public static class SignedXmlExtensions
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <summary>
        /// Gets the subject key identifier from the signed XML.
        /// </summary>
        /// <param name="signedXml">A SignedXml instance.</param>
        /// <returns>Subject key identifier or <c>null</c> if not found.</returns>
        public static string? GetSubjectKeyIdentifier( this SignedXml signedXml )
        {
            return signedXml.KeyInfo.GetSubjectKeyIdentifier();
        }

        /// <summary>
        /// Gets the subject key identifier bytes from the signed XML.
        /// </summary>
        /// <param name="signedXml">A SignedXml instance.</param>
        /// <returns>Subject key identifier bytes or <c>null</c> if not found.</returns>
        public static ReadOnlyMemory<byte>? GetSubjectKeyIdentifierBytes( this SignedXml signedXml )
        {
            return signedXml.KeyInfo.GetSubjectKeyIdentifierBytes();
        }
    }
}
