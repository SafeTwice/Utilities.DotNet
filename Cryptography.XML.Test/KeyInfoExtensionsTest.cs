/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using Xunit;

namespace Utilities.DotNet.Cryptography.XML.Test
{
    public class KeyInfoExtensionsTest
    {
        private readonly X509Certificate2 m_signingKey;

        public KeyInfoExtensionsTest()
        {
            m_signingKey = TestHelper.GenerateSigningKey();
        }

        [Fact]
        public void SubjectKeyIdentifier()
        {
            // Arrange

            var xmlSignedText = Test.TestHelper.GetSignedXmlText( m_signingKey );

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml( xmlSignedText );

            var signedXml = new SignedXml( xmlDoc );
            signedXml.LoadXml( xmlDoc.DocumentElement![ "Signature" ]! );

            Assert.True( signedXml.CheckSignature( m_signingKey.GetRSAPublicKey()! ) );

            var keyInfo = signedXml.KeyInfo;

            // Act

            var subjectKeyIdentifier = keyInfo.GetSubjectKeyIdentifier();

            var subjectKeyIdentifierBytes = keyInfo.GetSubjectKeyIdentifierBytes();

            // Assert

            Assert.NotNull( subjectKeyIdentifier );
            Assert.NotNull( subjectKeyIdentifierBytes );

            Assert.Equal( subjectKeyIdentifier, subjectKeyIdentifierBytes.Value.ToArray().ToHexString() );

            Assert.Equal( m_signingKey.GetSubjectKeyIdentifier(), subjectKeyIdentifier );
        }
    }
}
