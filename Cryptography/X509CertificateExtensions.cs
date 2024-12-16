/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Utilities.DotNet.Cryptography
{
    /// <summary>
    /// Extension methods for <see cref="X509Certificate2"/>.
    /// </summary>
    public static class X509CertificateExtensions
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <summary>
        /// Gets the basic constraints extension of the certificate.
        /// </summary>
        /// <param name="certificate">A certificate.</param>
        /// <returns>Basic constraints extension or <c>null</c> if not found.</returns>
        public static X509BasicConstraintsExtension? GetBasicConstraints( this X509Certificate2 certificate )
        {
            return certificate.Extensions.OfType<X509BasicConstraintsExtension>().FirstOrDefault();
        }

        /// <summary>
        /// Indicates whether the certificate is a certificate authority.
        /// </summary>
        /// <param name="certificate">A certificate.</param>
        /// <returns><c>true</c> if the certificate is a certificate authority; otherwise, <c>false</c>.</returns>
        public static bool IsCertificateAuthority( this X509Certificate2 certificate )
        {
            return certificate.GetBasicConstraints()?.CertificateAuthority ?? false;
        }

        /// <summary>
        /// Gets the path length constraint of the certificate.
        /// </summary>
        /// <param name="certificate">A certificate.</param>
        /// <returns>Path length constraint or <c>null</c> if unconstrained.</returns>
        public static int? PathLengthConstraint( this X509Certificate2 certificate )
        {
            var basicConstraints = certificate.GetBasicConstraints();
            if( basicConstraints?.HasPathLengthConstraint == true )
            {
                return basicConstraints.PathLengthConstraint;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the key usage extension of the certificate.
        /// </summary>
        /// <param name="certificate">A certificate.</param>
        /// <returns>Key usage extension or <c>null</c> if not found.</returns>
        public static X509KeyUsageExtension? GetKeyUsage( this X509Certificate2 certificate )
        {
            return certificate.Extensions.OfType<X509KeyUsageExtension>().FirstOrDefault();
        }

        /// <summary>
        /// Gets the key usage flags of the certificate.
        /// </summary>
        /// <param name="certificate">A certificate.</param>
        /// <returns>Key usages or <c>null</c> if not found.</returns>
        public static X509KeyUsageFlags? GetKeyUsageFlags( this X509Certificate2 certificate )
        {
            return certificate.GetKeyUsage()?.KeyUsages;
        }

        /// <summary>
        /// Gets the subject key identifier of the certificate.
        /// </summary>
        /// <param name="certificate">A certificate.</param>
        /// <returns>Subject key identifier or <c>null</c> if not found.</returns>
        public static string? GetSubjectKeyIdentifier( this X509Certificate2 certificate )
        {
            var skidExtension = certificate.Extensions.OfType<X509SubjectKeyIdentifierExtension>().FirstOrDefault();

            return skidExtension?.SubjectKeyIdentifier;
        }

        /// <summary>
        /// Gets the subject key identifier bytes of the certificate.
        /// </summary>
        /// <param name="certificate">A certificate.</param>
        /// <returns>Subject key identifier bytes or <c>null</c> if not found.</returns>
        public static ReadOnlyMemory<byte>? GetSubjectKeyIdentifierBytes( this X509Certificate2 certificate )
        {
#if NET7_0_OR_GREATER
            var skidExtension = certificate.Extensions.OfType<X509SubjectKeyIdentifierExtension>().FirstOrDefault();

            return skidExtension?.SubjectKeyIdentifierBytes;
#else
            var bytes = certificate.GetSubjectKeyIdentifier()?.ParseHexString();

            return ( bytes == null ) ? null : new( bytes );
#endif
        }

#if NET7_0_OR_GREATER
        /// <summary>
        /// Gets the authority key identifier of the certificate.
        /// </summary>
        /// <param name="certificate">A certificate.</param>
        /// <returns>Authority key identifier or <c>null</c> if not found.</returns>
        public static string? GetAuthorityKeyIdentifier( this X509Certificate2 certificate )
        {
            var akidBytes = certificate.GetAuthorityKeyIdentifierBytes();

            return akidBytes?.ToArray().ToHexString();
        }

        /// <summary>
        /// Gets the authority key identifier bytes of the certificate.
        /// </summary>
        /// <param name="certificate">A certificate.</param>
        /// <returns>Authority key identifier bytes or <c>null</c> if not found.</returns>
        public static ReadOnlyMemory<byte>? GetAuthorityKeyIdentifierBytes( this X509Certificate2 certificate )
        {
            var akidExtension = certificate.Extensions.OfType<X509AuthorityKeyIdentifierExtension>().FirstOrDefault();

            return akidExtension?.KeyIdentifier;
        }

#endif
    }
}
