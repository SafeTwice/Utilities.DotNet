/// @file
/// @copyright  Copyright (c) 2021-2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security;

namespace Utilities.DotNet
{
    /// <summary>
    /// <see cref="SecureString"/> utilities.
    /// </summary>
    public static class SecureStringUtilities
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <summary>
        /// Gets the string representation of a <see cref="SecureString"/>.
        /// </summary>
        /// <param name="value">A secure string.</param>
        /// <returns>The plain text representation of the secure string.</returns>
        public static string ToPlainString( this SecureString value )
        {
            IntPtr valuePtr = IntPtr.Zero;
            string? plainText;

            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode( value );
                plainText = Marshal.PtrToStringUni( valuePtr );
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode( valuePtr );
            }

            return CheckPlainText( plainText );
        }

        /// <summary>
        /// Compares two <see cref="SecureString"/>.
        /// </summary>
        /// <param name="value">A secure string.</param>
        /// <param name="other">Another secure string.</param>
        /// <returns><see langword="true"/> if the current instance and <paramref name="other"/> have the same string representation;
        ///          <see langword="false"/> otherwise.</returns>
        public static bool EqualTo( this SecureString value, SecureString other )
        {
            var s1 = value.ToPlainString();
            var s2 = other.ToPlainString();
            return s1.Equals( s2 );
        }

        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        [ExcludeFromCodeCoverage]
        private static string CheckPlainText( string? plainText )
        {
            if( plainText == null )
            {
                throw new InvalidOperationException( "Failed to convert the secure string to plain text." );
            }

            return plainText;
        }
    }
}
