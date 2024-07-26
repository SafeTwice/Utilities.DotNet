/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Globalization;
using System.IO;
using System.Xml.Linq;
using Utilities.DotNet.XML;
using Xunit;

namespace Utilities.DotNet.Test.XML
{
    public class XmlFileProcessingExceptionTest
    {
        private readonly Uri m_fileuri;
        private XDocument m_doc;

        public XmlFileProcessingExceptionTest()
        {
            CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;

            var filename = Path.GetTempFileName();

            string[] contents =
            {
                "<Root>",
                "<Element1/>",
                "<Element2/>",
                "</Root>",
            };

            File.WriteAllLines( filename, contents );

            m_doc = XDocument.Load( filename, LoadOptions.SetBaseUri | LoadOptions.SetLineInfo );

            File.Delete( filename );

            m_fileuri = new Uri( filename );
        }

        [Fact]
        public void NoInnerException()
        {
            var element = m_doc.Root!.Element( "Element2" )!;

            var exception = new XmlFileProcessingException( "foo11", element );

            Assert.Equal( "foo11", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 3, exception.Line );

            Assert.Null( exception.InnerException );

            Assert.Equal( $"[{exception.Filename} @ Line 3] foo11", exception.Message );
        }

        [Fact]
        public void InnerException()
        {
            var innerException = new Exception( "chazz" );
            var element = m_doc.Root!.Element( "Element1" )!;

            var exception = new XmlFileProcessingException( "foo90", element, innerException );

            Assert.Equal( "foo90", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 2, exception.Line );

            Assert.Same( innerException, exception.InnerException );

            Assert.Equal( $"[{exception.Filename} @ Line 2] foo90", exception.Message );
        }
    }
}
