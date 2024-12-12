/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using Xunit;

namespace Utilities.DotNet.Cryptography.Test
{
    public class ECDsaShaSignatureDescriptionTest
    {
        static ECDsaShaSignatureDescriptionTest()
        {
            ECDsaShaSignatureDescription.RegisterAll();
        }

        public static TheoryData<ECCurve, HashAlgorithmName, string> SignatureAndVerificationData => new()
        {
            { ECCurve.NamedCurves.brainpoolP160r1, HashAlgorithmName.SHA256, "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha1" },
            { ECCurve.NamedCurves.nistP256, HashAlgorithmName.SHA256, "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha256" },
            { ECCurve.NamedCurves.nistP384, HashAlgorithmName.SHA384, "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha384" },
            { ECCurve.NamedCurves.nistP521, HashAlgorithmName.SHA512, "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha512" }
        };

        [Theory]
        [MemberData( nameof( SignatureAndVerificationData ) )]
        public void SignatureAndVerification( ECCurve curve, HashAlgorithmName hashName, string dsigName )
        {
            // Arrange

            var ecdsa = ECDsa.Create( curve );

            var certificateRequest = new CertificateRequest( "CN=test", ecdsa, hashName );
            certificateRequest.CertificateExtensions.Add( new X509BasicConstraintsExtension( true, true, 0, true ) );
            certificateRequest.CertificateExtensions.Add( new X509KeyUsageExtension( X509KeyUsageFlags.DigitalSignature, false ) );

            var certificate = certificateRequest.CreateSelfSigned( DateTimeOffset.UtcNow.AddDays( -2 ), DateTimeOffset.UtcNow.AddDays( +2 ) );

            var xmlText = "<Root><Data>value</Data></Root>";

            // Act & Assert

            var result = CheckSignatureCreation( xmlText, certificate, dsigName );

            CheckSignatureVerification( result, certificate );
        }

        private static string CheckSignatureCreation( string xmlText, X509Certificate2 certificate, string signatureMethod )
        {
            // Arrange

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml( xmlText );

            var signedXml = new SignedXml( xmlDoc )
            {
                SigningKey = certificate.GetECDsaPrivateKey()!
            };

            signedXml.SignedInfo!.SignatureMethod = signatureMethod;
            signedXml.SignedInfo.CanonicalizationMethod = "http://www.w3.org/2001/10/xml-exc-c14n#";

            var reference = new Reference( string.Empty );
            reference.AddTransform( new XmlDsigEnvelopedSignatureTransform() );
            reference.AddTransform( new XmlDsigExcC14NTransform() );
            signedXml.AddReference( reference );

            // Act

            signedXml.ComputeSignature();

            var xmlSignature = signedXml.GetXml();

            // Assert

            Assert.NotNull( xmlSignature );

            // Arrange

            xmlDoc.DocumentElement!.AppendChild( xmlSignature );
            return xmlDoc.OuterXml;
        }

        private static void CheckSignatureVerification( string xmlText, X509Certificate2 certificate )
        {
            // Arrange

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml( xmlText );

            var signedXml = new SignedXml( xmlDoc );
            var signatureNode = xmlDoc.DocumentElement!.GetElementsByTagName( "Signature" )[ 0 ]!;

            signedXml.LoadXml( (XmlElement) signatureNode );

            // Act

            var result = signedXml.CheckSignature( certificate.GetECDsaPublicKey()! );

            // Assert

            Assert.True( result );
        }

        public static TheoryData<AsymmetricAlgorithm> InvalidKeyData => new()
        {
            { RSA.Create() },
            { ECDsa.Create( ECCurve.NamedCurves.brainpoolP160r1 ) },
            { ECDsa.Create( ECCurve.NamedCurves.nistP384 ) },
        };

        [Theory]
        [MemberData( nameof( InvalidKeyData ) )]
        public void CreateFormatter_InvalidKey( AsymmetricAlgorithm algorithm )
        {
            // Arrange

            var description = new ECDsaSha256SignatureDescription();

            // Act & Assert

            Assert.Throws<ArgumentException>( () => description.CreateFormatter( algorithm ) );
        }

        [Theory]
        [MemberData( nameof( InvalidKeyData ) )]
        public void CreateDeformatter_InvalidKey( AsymmetricAlgorithm algorithm )
        {
            // Arrange

            var description = new ECDsaSha256SignatureDescription();

            // Act & Assert

            Assert.Throws<ArgumentException>( () => description.CreateDeformatter( algorithm ) );
        }

        [Fact]
        public void SetKey()
        {
            // Arrange

            var description = new ECDsaSha256SignatureDescription();

            var ecdsa1 = ECDsa.Create( ECCurve.NamedCurves.nistP256 );
            var ecdsa2 = ECDsa.Create( ECCurve.NamedCurves.nistP256 );

            var formatter = description.CreateFormatter( ecdsa1 );
            var deformatter = description.CreateDeformatter( ecdsa1 );

            var dataToSign = new byte[ 32 ];
            RandomNumberGenerator.Create().GetBytes( dataToSign );

            // Act

            formatter.SetKey( ecdsa2 );
            formatter.SetHashAlgorithm( "foo" );

            var signedData = formatter.CreateSignature( dataToSign );

            deformatter.SetKey( ecdsa2 );
            deformatter.SetHashAlgorithm( "bar" );

            var result = deformatter.VerifySignature( dataToSign, signedData );

            // Assert

            Assert.True( result );
        }
    }
}
