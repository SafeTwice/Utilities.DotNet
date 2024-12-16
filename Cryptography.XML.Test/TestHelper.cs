/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace Utilities.DotNet.Cryptography.XML.Test
{
    /// <summary>
    /// Helper class for tests.
    /// </summary>
    public static class TestHelper
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        public static X509Certificate2 GenerateSigningKey()
        {
            var rsa = RSA.Create( 2048 );

            var certificateRequest = new CertificateRequest( "CN=Test CA", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1 );

            certificateRequest.CertificateExtensions.Add( new X509BasicConstraintsExtension( true, true, 0, true ) );
            certificateRequest.CertificateExtensions.Add( new X509KeyUsageExtension( X509KeyUsageFlags.KeyCertSign | X509KeyUsageFlags.DigitalSignature, true ) );
            certificateRequest.CertificateExtensions.Add( new X509SubjectKeyIdentifierExtension( certificateRequest.PublicKey, false ) );

            var startDate = DateTime.UtcNow.Date.AddDays( -1 );
            var endDate = startDate.AddDays( 2 );

            return certificateRequest.CreateSelfSigned( startDate, endDate );
        }

        public static string GetSignedXmlText( X509Certificate2 key )
        {
            var xmlText = "<Root><Data>Value</Data></Root>";

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml( xmlText );

            var signedXml = new SignedXml( xmlDoc );
            signedXml.SigningKey = key.GetRSAPrivateKey();

            var reference = new Reference( "" );
            reference.AddTransform( new XmlDsigEnvelopedSignatureTransform() );
            signedXml.AddReference( reference );

            var skidBytes = key.GetSubjectKeyIdentifierBytes()!.Value;
            var keyInfoData = new KeyInfoX509Data();
            keyInfoData.AddSubjectKeyId( skidBytes.ToArray() );

            var keyInfo = new KeyInfo();
            keyInfo.AddClause( keyInfoData );
            signedXml.KeyInfo = keyInfo;

            signedXml.ComputeSignature();

            var signatureElement = signedXml.GetXml();

            xmlDoc.DocumentElement!.AppendChild( signatureElement );

            return xmlDoc.OuterXml;
        }
    }
}
