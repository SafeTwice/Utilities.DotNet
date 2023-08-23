/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.IO;
using System.Xml.Linq;
using Utilities.DotNet.Exceptions;
using Utilities.DotNet.XML;
using Xunit;

namespace Utilities.DotNet.Test.XML
{
    public class XmlUtilitiesTest
    {
        private readonly Uri m_fileuri;
        private XDocument m_doc;

        public XmlUtilitiesTest()
        {
            var filename = Path.GetTempFileName();

            string[] contents =
            {
                "<Root>",
                "<String value='foo' empty=''/>",
                "<Int value='-45' empty='' invalid='bar'/>",
                "<UInt value='435' empty='' invalid='bar'/>",
                "<Double value='23.645' empty='' invalid='bar'/>",
                "<Bool true='true' false='false' empty='' invalid='bar'/>",
                "<Enum value1='OPTION1' value2=' OPTION2 ' empty='' invalid='bar'/>",
                "<EnumFlags value1='OPTION1' value2='OPTION1, OPTION2' empty='' invalid='bar'/>",
                "<Int value='450'/>",
                "</Root>",
            };

            File.WriteAllLines( filename, contents );

            m_doc = XDocument.Load( filename, LoadOptions.SetBaseUri | LoadOptions.SetLineInfo );

            File.Delete( filename );

            m_fileuri = new Uri( filename );
        }

        private XElement GetElement( string name )
        {
            return m_doc.Element( "Root" )!.Element( name )!;
        }

        [Fact]
        public void MandatoryAttribute_Existing()
        {
            var element = GetElement( "String" );

            var value = element.MandatoryAttribute( "value" );

            Assert.Equal( "foo", value );
        }

        [Fact]
        public void MandatoryAttribute_NotExisting()
        {
            var element = GetElement( "String" );

            var exception = Assert.Throws<FileProcessingException>( () =>
            {
                var value = element.MandatoryAttribute( "bar" );
            } );

            Assert.Equal( $"XML element 'String' lacks mandatory attribute 'bar'", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 2, exception.Line );
        }

        [Fact]
        public void MandatoryAttribute_Empty_Allowed()
        {
            var element = GetElement( "String" );

            var value = element.MandatoryAttribute( "empty", true );

            Assert.Equal( "", value );
        }

        [Fact]
        public void MandatoryAttribute_Empty_NotAllowed()
        {
            var element = GetElement( "String" );

            var exception = Assert.Throws<FileProcessingException>( () =>
            {
                var value = element.MandatoryAttribute( "empty" );
            } );

            Assert.Equal( $"XML element 'String' mandatory attribute 'empty' is empty", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 2, exception.Line );
        }

        [Fact]
        public void MandatoryAttributeInt_Existing()
        {
            var element = GetElement( "Int" );

            var value = element.MandatoryAttributeInt( "value" );

            Assert.Equal( -45, value );
        }

        [Fact]
        public void MandatoryAttributeInt_Invalid()
        {
            var element = GetElement( "Int" );

            var exception = Assert.Throws<FileProcessingException>( () =>
            {
                var value = element.MandatoryAttributeInt( "invalid" );
            } );

            Assert.Equal( $"XML element 'Int' attribute 'invalid' has an invalid value 'bar' (expected integer value)", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 3, exception.Line );
        }

        [Fact]
        public void MandatoryAttributeUInt_Existing()
        {
            var element = GetElement( "UInt" );

            var value = element.MandatoryAttributeUInt( "value" );

            Assert.Equal( 435U, value );
        }

        [Fact]
        public void MandatoryAttributeUInt_Invalid()
        {
            var element = GetElement( "UInt" );

            var exception = Assert.Throws<FileProcessingException>( () =>
            {
                var value = element.MandatoryAttributeUInt( "invalid" );
            } );

            Assert.Equal( $"XML element 'UInt' attribute 'invalid' has an invalid value 'bar' (expected unsigned integer value)", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 4, exception.Line );
        }

        [Fact]
        public void MandatoryAttributeDouble_Existing()
        {
            var element = GetElement( "Double" );

            var value = element.MandatoryAttributeDouble( "value" );

            Assert.Equal( 23.645, value, 3 );
        }

        [Fact]
        public void MandatoryAttributeDouble_Invalid()
        {
            var element = GetElement( "Double" );

            var exception = Assert.Throws<FileProcessingException>( () =>
            {
                var value = element.MandatoryAttributeDouble( "invalid" );
            } );

            Assert.Equal( $"XML element 'Double' attribute 'invalid' has an invalid value 'bar' (expected real number value)", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 5, exception.Line );
        }

        [Fact]
        public void MandatoryAttributeBool_Existing()
        {
            var element = GetElement( "Bool" );

            var valueTrue = element.MandatoryAttributeBool( "true" );
            var valueFalse = element.MandatoryAttributeBool( "false" );

            Assert.True( valueTrue );
            Assert.False( valueFalse );
        }

        [Fact]
        public void MandatoryAttributeBool_Invalid()
        {
            var element = GetElement( "Bool" );

            var exception = Assert.Throws<FileProcessingException>( () =>
            {
                var value = element.MandatoryAttributeBool( "invalid" );
            } );

            Assert.Equal( $"XML element 'Bool' attribute 'invalid' has an invalid value 'bar' (expected boolean value)", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 6, exception.Line );
        }

        public enum TestEnum
        {
            OPTION1,
            OPTION2,
        }

        [Theory]
        [InlineData( "value1", TestEnum.OPTION1 )]
        [InlineData( "value2", TestEnum.OPTION2 )]
        public void MandatoryAttributeEnum_Existing( string attribute, TestEnum expectedValue )
        {
            var element = GetElement( "Enum" );

            var value = element.MandatoryAttributeEnum<TestEnum>( attribute );

            Assert.Equal( expectedValue, value );
        }

        [Fact]
        public void MandatoryAttributeEnum_Invalid()
        {
            var element = GetElement( "Enum" );

            var exception = Assert.Throws<FileProcessingException>( () =>
            {
                var value = element.MandatoryAttributeEnum<TestEnum>( "invalid" );
            } );

            Assert.Equal( $"XML element 'Enum' attribute 'invalid' has an invalid value 'bar' (expected one of: OPTION1, OPTION2)", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 7, exception.Line );
        }

        [Flags]
        public enum TestEnumFlags
        {
            NONE = 0,
            OPTION1 = 0x01,
            OPTION2 = 0x02,
            OPTION3 = 0x04,
        }

        [Theory]
        [InlineData( "value1", TestEnumFlags.OPTION1 )]
        [InlineData( "value2", TestEnumFlags.OPTION1 | TestEnumFlags.OPTION2 )]
        public void MandatoryAttributeEnumFlags_Existing( string attribute, TestEnumFlags expectedValue )
        {
            var element = GetElement( "EnumFlags" );

            var value = element.MandatoryAttributeEnum<TestEnumFlags>( attribute );

            Assert.Equal( expectedValue, value );
        }

        [Fact]
        public void MandatoryAttributeEnumFlags_Invalid()
        {
            var element = GetElement( "EnumFlags" );

            var exception = Assert.Throws<FileProcessingException>( () =>
            {
                var value = element.MandatoryAttributeEnum<TestEnumFlags>( "invalid" );
            } );

            Assert.Equal( $"XML element 'EnumFlags' attribute 'invalid' has an invalid value 'bar' (expected one of: NONE, OPTION1, OPTION2, OPTION3)", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 8, exception.Line );
        }

        [Fact]
        public void OptionalAttribute_Existing()
        {
            var element = GetElement( "String" );

            var value = element.OptionalAttribute( "value", "bar" );

            Assert.Equal( "foo", value );
        }

        [Fact]
        public void OptionalAttribute_NotExisting()
        {
            var element = GetElement( "String" );

            var value = element.OptionalAttribute( "bar", "fizz" );

            Assert.Equal( "fizz", value );
        }

        [Fact]
        public void OptionalAttributeInt_Existing()
        {
            var element = GetElement( "Int" );

            var value = element.OptionalAttributeInt( "value", 76645 );

            Assert.Equal( -45, value );
        }

        [Fact]
        public void OptionalAttributeInt_NotExisting()
        {
            var element = GetElement( "Int" );

            var value = element.OptionalAttributeInt( "bar", 7687 );

            Assert.Equal( 7687, value );
        }

        [Fact]
        public void OptionalAttributeInt_Invalid()
        {
            var element = GetElement( "Int" );

            var exception = Assert.Throws<FileProcessingException>( () =>
            {
                var value = element.OptionalAttributeInt( "invalid" );
            } );

            Assert.Equal( $"XML element 'Int' attribute 'invalid' has an invalid value 'bar' (expected integer value)", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 3, exception.Line );
        }

        [Fact]
        public void OptionalAttributeUInt_Existing()
        {
            var element = GetElement( "UInt" );

            var value = element.OptionalAttributeUInt( "value", 5454U );

            Assert.Equal( 435U, value );
        }

        [Fact]
        public void OptionalAttributeUInt_NotExisting()
        {
            var element = GetElement( "UInt" );

            var value = element.OptionalAttributeUInt( "bar", 775U );

            Assert.Equal( 775U, value );
        }

        [Fact]
        public void OptionalAttributeUInt_Invalid()
        {
            var element = GetElement( "UInt" );

            var exception = Assert.Throws<FileProcessingException>( () =>
            {
                var value = element.OptionalAttributeUInt( "invalid" );
            } );

            Assert.Equal( $"XML element 'UInt' attribute 'invalid' has an invalid value 'bar' (expected unsigned integer value)", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 4, exception.Line );
        }

        [Fact]
        public void OptionalAttributeDouble_Existing()
        {
            var element = GetElement( "Double" );

            var value = element.OptionalAttributeDouble( "value", 456.3 );

            Assert.Equal( 23.645, value, 3 );
        }

        [Fact]
        public void OptionalAttributeDouble_NotExisting()
        {
            var element = GetElement( "Double" );

            var value = element.OptionalAttributeDouble( "bar", 77434.2432 );

            Assert.Equal( 77434.2432, value, 4 );
        }

        [Fact]
        public void OptionalAttributeDouble_Invalid()
        {
            var element = GetElement( "Double" );

            var exception = Assert.Throws<FileProcessingException>( () =>
            {
                var value = element.OptionalAttributeDouble( "invalid" );
            } );

            Assert.Equal( $"XML element 'Double' attribute 'invalid' has an invalid value 'bar' (expected real number value)", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 5, exception.Line );
        }

        [Fact]
        public void OptionalAttributeBool_Existing()
        {
            var element = GetElement( "Bool" );

            var valueTrue = element.OptionalAttributeBool( "true", false );
            var valueFalse = element.OptionalAttributeBool( "false", true );

            Assert.True( valueTrue );
            Assert.False( valueFalse );
        }

        [Fact]
        public void OptionalAttributeBool_NotExisting()
        {
            var element = GetElement( "Double" );

            var valueTrue = element.OptionalAttributeBool( "bar", true );
            var valueFalse = element.OptionalAttributeBool( "baz", false );

            Assert.True( valueTrue );
            Assert.False( valueFalse );
        }

        [Fact]
        public void OptionalAttributeBool_Invalid()
        {
            var element = GetElement( "Bool" );

            var exception = Assert.Throws<FileProcessingException>( () =>
            {
                var value = element.OptionalAttributeBool( "invalid" );
            } );

            Assert.Equal( $"XML element 'Bool' attribute 'invalid' has an invalid value 'bar' (expected boolean value)", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 6, exception.Line );
        }

        [Theory]
        [InlineData( "value1", TestEnum.OPTION2, TestEnum.OPTION1 )]
        [InlineData( "value2", TestEnum.OPTION1, TestEnum.OPTION2 )]
        public void OptionalAttributeEnum_Existing( string attribute, TestEnum defaultValue, TestEnum expectedValue )
        {

            var element = GetElement( "Enum" );

            var value = element.OptionalAttributeEnum<TestEnum>( attribute, defaultValue );

            Assert.Equal( expectedValue, value );
        }

        [Fact]
        public void OptionalAttributeEnum_NotExisting()
        {

            var element = GetElement( "Enum" );

            var value = element.OptionalAttributeEnum<TestEnum>( "bar", TestEnum.OPTION2 );

            Assert.Equal( TestEnum.OPTION2, value );
        }

        [Fact]
        public void OptionalAttributeEnum_Invalid()
        {
            var element = GetElement( "Enum" );

            var exception = Assert.Throws<FileProcessingException>( () =>
            {
                var value = element.OptionalAttributeEnum<TestEnum>( "invalid" );
            } );

            Assert.Equal( $"XML element 'Enum' attribute 'invalid' has an invalid value 'bar' (expected one of: OPTION1, OPTION2)", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 7, exception.Line );
        }

        [Theory]
        [InlineData( "value1", TestEnumFlags.OPTION2, TestEnumFlags.OPTION1 )]
        [InlineData( "value2", TestEnumFlags.NONE, TestEnumFlags.OPTION1 | TestEnumFlags.OPTION2 )]
        public void OptionalAttributeEnumFlags_Existing( string attribute, TestEnumFlags defaultValue, TestEnumFlags expectedValue )
        {

            var element = GetElement( "EnumFlags" );

            var value = element.OptionalAttributeEnum<TestEnumFlags>( attribute, defaultValue );

            Assert.Equal( expectedValue, value );
        }

        [Fact]
        public void OptionalAttributeEnumFlags_NotExisting()
        {

            var element = GetElement( "EnumFlags" );

            var value = element.OptionalAttributeEnum<TestEnumFlags>( "bar", TestEnumFlags.OPTION2 );

            Assert.Equal( TestEnumFlags.OPTION2, value );
        }

        [Fact]
        public void OptionalAttributeEnumFlags_Invalid()
        {
            var element = GetElement( "EnumFlags" );

            var exception = Assert.Throws<FileProcessingException>( () =>
            {
                var value = element.OptionalAttributeEnum<TestEnumFlags>( "invalid" );
            } );

            Assert.Equal( $"XML element 'EnumFlags' attribute 'invalid' has an invalid value 'bar' (expected one of: NONE, OPTION1, OPTION2, OPTION3)", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 8, exception.Line );
        }

        [Fact]
        public void MandatoryAttribute_Multiple_Existing()
        {
            var element = GetElement( "String" );

            string attributeName;
            var value = element.MandatoryAttribute( new string[] { "bar", "value" }, out attributeName );

            Assert.Equal( "foo", value );
            Assert.Equal( "value", attributeName );
        }

        [Fact]
        public void MandatoryAttribute_Multiple_NotExisting()
        {
            var element = GetElement( "String" );

            var exception = Assert.Throws<FileProcessingException>( () =>
            {
                string attributeName;
                var value = element.MandatoryAttribute( new string[] { "bar", "baz" }, out attributeName );
            } );

            Assert.Equal( $"XML element 'String' lacks one of mandatory attributes ('bar', 'baz') or are empty", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 2, exception.Line );
        }

        [Fact]
        public void MandatoryUniqueElement_Existing()
        {
            var rootElement = m_doc!.Root!;

            var element = rootElement.MandatoryUniqueElement( "String" );

            Assert.Equal( "String", element.Name );
            Assert.Equal( "foo", element.MandatoryAttribute( "value" ) );
        }

        [Fact]
        public void MandatoryUniqueElement_NotExisting()
        {
            var rootElement = m_doc!.Root!;

            var exception = Assert.Throws<FileProcessingException>( () =>
            {
                var element = rootElement.MandatoryUniqueElement( "Foo" );
            } );

            Assert.Equal( $"XML element 'Root' lacks mandatory child element 'Foo'", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 1, exception.Line );
        }

        [Fact]
        public void MandatoryUniqueElement_NotUnique()
        {
            var rootElement = m_doc!.Root!;

            var exception = Assert.Throws<FileProcessingException>( () =>
            {
                var element = rootElement.MandatoryUniqueElement( "Int" );
            } );

            Assert.Equal( $"XML element 'Root' has more than 1 child element 'Int'", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 3, exception.Line );
        }

        [Fact]
        public void OptionalUniqueElement_Existing()
        {
            var rootElement = m_doc!.Root!;

            var element = rootElement.OptionalUniqueElement( "String" );

            Assert.NotNull( element );
            Assert.Equal( "String", element.Name );
            Assert.Equal( "foo", element.MandatoryAttribute( "value" ) );
        }

        [Fact]
        public void OptionalUniqueElement_NotExisting()
        {
            var rootElement = m_doc!.Root!;

            var element = rootElement.OptionalUniqueElement( "Foo" );

            Assert.Null( element );
        }

        [Fact]
        public void OptionalUniqueElement_NotUnique()
        {
            var rootElement = m_doc!.Root!;

            var exception = Assert.Throws<FileProcessingException>( () =>
            {
                var element = rootElement.OptionalUniqueElement( "Int" );
            } );

            Assert.Equal( $"XML element 'Root' has more than 1 child element 'Int'", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 3, exception.Line );
        }

        [Fact]
        public void AddUnique_NotExisting()
        {
            var container = new XElement( "Container" );

            var child = new XElement( "Child" );

            container.AddUnique( child );

            Assert.Equal( container, child.Parent );
            Assert.Collection( container.Elements(), new Action<XElement>[]
            {
                element => Assert.Equal( child, element )
            } );
        }

        [Fact]
        public void AddUnique_AlredyExisting()
        {
            var container = new XElement( "Container" );

            var child = new XElement( "Child" );

            container.Add( child );

            container.AddUnique( child );

            Assert.Equal( container, child.Parent );
            Assert.Collection( container.Elements(), new Action<XElement>[]
            {
                element => Assert.Equal( child, element )
            } );
        }

        [Fact]
        public void AddUnique_Move()
        {
            var container1 = new XElement( "Container1" );
            var container2 = new XElement( "Container2" );

            var child = new XElement( "Child" );

            container1.Add( child );

            container2.AddUnique( child );

            Assert.Equal( container2, child.Parent );
            Assert.Empty( container1.Elements() );
            Assert.Collection( container2.Elements(), new Action<XElement>[]
            {
                element => Assert.Equal( child, element )
            } );
        }
    }
}
