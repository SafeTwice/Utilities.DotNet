/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using Xunit;

namespace Utilities.DotNet.Cryptography.XML.Test
{
    public class SignedXmlExtensionsTest
    {
        private readonly X509Certificate2 m_signingKey;

        public SignedXmlExtensionsTest()
        {
            m_signingKey = TestHelper.GenerateSigningKey();
        }

        [Fact]
        public void SubjectKeyIdentifier_Present()
        {
            // Arrange

            var xmlSignedText = TestHelper.GetSignedXmlText( m_signingKey, true );

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml( xmlSignedText );

            var signedXml = new SignedXml( xmlDoc );
            signedXml.LoadXml( xmlDoc.DocumentElement![ "Signature" ]! );

            Assert.True( signedXml.CheckSignature( m_signingKey.GetRSAPublicKey()! ) );

            // Act

            var subjectKeyIdentifier = signedXml.GetSubjectKeyIdentifier();

            var subjectKeyIdentifierBytes = signedXml.GetSubjectKeyIdentifierBytes();

            // Assert

            Assert.NotNull( subjectKeyIdentifier );
            Assert.NotNull( subjectKeyIdentifierBytes );

            Assert.Equal( subjectKeyIdentifier, subjectKeyIdentifierBytes.Value.ToArray().ToHexString() );

            Assert.Equal( m_signingKey.GetSubjectKeyIdentifier(), subjectKeyIdentifier );
        }

        [Fact]
        public void SubjectKeyIdentifier_NotPresent()
        {
            // Arrange

            var xmlSignedText = TestHelper.GetSignedXmlText( m_signingKey, false );

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml( xmlSignedText );

            var signedXml = new SignedXml( xmlDoc );
            signedXml.LoadXml( xmlDoc.DocumentElement![ "Signature" ]! );

            Assert.True( signedXml.CheckSignature( m_signingKey.GetRSAPublicKey()! ) );

            // Act

            var subjectKeyIdentifier = signedXml.GetSubjectKeyIdentifier();

            var subjectKeyIdentifierBytes = signedXml.GetSubjectKeyIdentifierBytes();

            // Assert

            Assert.Null( subjectKeyIdentifier );
            Assert.Null( subjectKeyIdentifierBytes );
        }
    }
}
