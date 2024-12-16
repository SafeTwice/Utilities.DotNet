/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Xunit;

namespace Utilities.DotNet.Cryptography.Test
{
    public class X509CertificateExtensionsTest
    {
        private readonly X509Certificate2 m_caCertificate;

        private readonly X509Certificate2 m_endEntity1Certificate;
        private readonly X509Certificate2 m_endEntity2Certificate;

        private static X509Certificate2 GenerateCaCertificate()
        {
            var rsa = RSA.Create( 2048 );

            var certificateRequest = new CertificateRequest( "CN=Test CA", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1 );

            certificateRequest.CertificateExtensions.Add( new X509BasicConstraintsExtension( true, true, 0, true ) );
            certificateRequest.CertificateExtensions.Add( new X509KeyUsageExtension( X509KeyUsageFlags.KeyCertSign, true ) );
            certificateRequest.CertificateExtensions.Add( new X509SubjectKeyIdentifierExtension( certificateRequest.PublicKey, false ) );

            var startDate = DateTime.UtcNow.Date.AddDays( -1 );
            var endDate = startDate.AddDays( 2 );

            return certificateRequest.CreateSelfSigned( startDate, endDate );
        }

        private X509Certificate2 GenerateEndEntity1Certificate()
        {
            var rsa = RSA.Create( 2048 );

            var certificateRequest = new CertificateRequest( "CN=Test 1", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1 );

            certificateRequest.CertificateExtensions.Add( new X509BasicConstraintsExtension( false, false, 0, true ) );

            certificateRequest.CertificateExtensions.Add( new X509KeyUsageExtension( X509KeyUsageFlags.DigitalSignature, true ) );
            certificateRequest.CertificateExtensions.Add( new X509SubjectKeyIdentifierExtension( certificateRequest.PublicKey, false ) );
#if NET7_0_OR_GREATER
            certificateRequest.CertificateExtensions.Add( X509AuthorityKeyIdentifierExtension.CreateFromCertificate( m_caCertificate, true, false ) );
#endif

            var startDate = DateTime.UtcNow.Date.AddDays( -1 );
            var endDate = startDate.AddDays( 2 );

            return certificateRequest.Create( m_caCertificate, startDate, endDate, Guid.NewGuid().ToByteArray() );
        }

        private X509Certificate2 GenerateEndEntity2Certificate()
        {
            var rsa = RSA.Create( 2048 );

            var certificateRequest = new CertificateRequest( "CN=Test 2", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1 );

            var startDate = DateTime.UtcNow.Date.AddDays( -1 );
            var endDate = startDate.AddDays( 2 );

            return certificateRequest.Create( m_caCertificate, startDate, endDate, Guid.NewGuid().ToByteArray() );
        }

        public X509CertificateExtensionsTest()
        {
            m_caCertificate = GenerateCaCertificate();
            m_endEntity1Certificate = GenerateEndEntity1Certificate();
            m_endEntity2Certificate = GenerateEndEntity2Certificate();
        }

        [Fact]
        public void GetBasicConstraints()
        {
            // Act

            var caBasicContraints = m_caCertificate.GetBasicConstraints();
            var endEntity1BasicContraints = m_endEntity1Certificate.GetBasicConstraints();
            var endEntity2BasicContraints = m_endEntity2Certificate.GetBasicConstraints();

            // Assert

            Assert.NotNull( caBasicContraints );
            Assert.True( caBasicContraints.CertificateAuthority );
            Assert.True( caBasicContraints.HasPathLengthConstraint );
            Assert.Equal( 0, caBasicContraints.PathLengthConstraint );

            Assert.NotNull( endEntity1BasicContraints );
            Assert.False( endEntity1BasicContraints.CertificateAuthority );
            Assert.False( endEntity1BasicContraints.HasPathLengthConstraint );

            Assert.Null( endEntity2BasicContraints );
        }

        [Fact]
        public void IsCertificateAuthority()
        {
            // Act & Assert

            Assert.True( m_caCertificate.IsCertificateAuthority() );

            Assert.False( m_endEntity1Certificate.IsCertificateAuthority() );

            Assert.False( m_endEntity2Certificate.IsCertificateAuthority() );
        }

        [Fact]
        public void PathLengthConstraint()
        {
            // Act & Assert

            Assert.Equal( 0, m_caCertificate.PathLengthConstraint() );

            Assert.Null( m_endEntity1Certificate.PathLengthConstraint() );

            Assert.Null( m_endEntity2Certificate.PathLengthConstraint() );
        }

        [Fact]
        public void GetKeyUsage()
        {
            // Act

            var caKeyUsage = m_caCertificate.GetKeyUsage();

            var endEntity1KeyUsage = m_endEntity1Certificate.GetKeyUsage();

            var endEntity2KeyUsage = m_endEntity2Certificate.GetKeyUsage();

            // Assert

            Assert.NotNull( caKeyUsage );
            Assert.Equal( X509KeyUsageFlags.KeyCertSign, caKeyUsage.KeyUsages );

            Assert.NotNull( endEntity1KeyUsage );
            Assert.Equal( X509KeyUsageFlags.DigitalSignature, endEntity1KeyUsage.KeyUsages );

            Assert.Null( endEntity2KeyUsage );
        }

        [Fact]
        public void GetKeyUsages()
        {
            // Act & Assert

            Assert.Equal( X509KeyUsageFlags.KeyCertSign, m_caCertificate.GetKeyUsageFlags() );

            Assert.Equal( X509KeyUsageFlags.DigitalSignature, m_endEntity1Certificate.GetKeyUsageFlags() );

            Assert.Null( m_endEntity2Certificate.GetKeyUsageFlags() );
        }

        [Fact]
        public void SubjectKeyIdentifier()
        {
            // Act

            var caSubjectKeyIdentifier = m_caCertificate.GetSubjectKeyIdentifier();
            var endEntity1SubjectKeyIdentifier = m_endEntity1Certificate.GetSubjectKeyIdentifier();
            var endEntity2SubjectKeyIdentifier = m_endEntity2Certificate.GetSubjectKeyIdentifier();

            var caSubjectKeyIdentifierBytes = m_caCertificate.GetSubjectKeyIdentifierBytes();
            var endEntity1SubjectKeyIdentifierBytes = m_endEntity1Certificate.GetSubjectKeyIdentifierBytes();
            var endEntity2SubjectKeyIdentifierBytes = m_endEntity2Certificate.GetSubjectKeyIdentifierBytes();

            // Assert

            Assert.NotNull( caSubjectKeyIdentifier );
            Assert.NotNull( endEntity1SubjectKeyIdentifier );
            Assert.Null( endEntity2SubjectKeyIdentifier );

            Assert.NotNull( caSubjectKeyIdentifierBytes );
            Assert.NotNull( endEntity1SubjectKeyIdentifierBytes );
            Assert.Null( endEntity2SubjectKeyIdentifierBytes );

            Assert.Equal( caSubjectKeyIdentifier, caSubjectKeyIdentifierBytes.Value.ToArray().ToHexString() );
            Assert.Equal( endEntity1SubjectKeyIdentifier, endEntity1SubjectKeyIdentifierBytes.Value.ToArray().ToHexString() );
        }

#if NET7_0_OR_GREATER

        [Fact]
        public void AuthorityKeyIdentifier()
        {
            // Act

            var caAuthorityKeyIdentifier = m_caCertificate.GetAuthorityKeyIdentifier();
            var endEntity1AuthorityKeyIdentifier = m_endEntity1Certificate.GetAuthorityKeyIdentifier();
            var endEntity2AuthorityKeyIdentifier = m_endEntity2Certificate.GetAuthorityKeyIdentifier();

            var caAuthorityKeyIdentifierBytes = m_caCertificate.GetAuthorityKeyIdentifierBytes();
            var endEntity1AuthorityKeyIdentifierBytes = m_endEntity1Certificate.GetAuthorityKeyIdentifierBytes();
            var endEntity2AuthorityKeyIdentifierBytes = m_endEntity2Certificate.GetAuthorityKeyIdentifierBytes();

            // Assert

            Assert.Null( caAuthorityKeyIdentifier );
            Assert.NotNull( endEntity1AuthorityKeyIdentifier );
            Assert.Null( endEntity2AuthorityKeyIdentifier );

            Assert.Null( caAuthorityKeyIdentifierBytes );
            Assert.NotNull( endEntity1AuthorityKeyIdentifierBytes );
            Assert.Null( endEntity2AuthorityKeyIdentifierBytes );

            Assert.Equal( endEntity1AuthorityKeyIdentifier, endEntity1AuthorityKeyIdentifierBytes.Value.ToArray().ToHexString() );

            Assert.Equal( m_caCertificate.GetSubjectKeyIdentifier(), endEntity1AuthorityKeyIdentifier );
        }

#endif
    }
}
