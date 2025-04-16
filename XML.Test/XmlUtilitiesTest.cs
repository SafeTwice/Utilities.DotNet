/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Globalization;
using System.IO;
using System.Xml.Linq;
using Xunit;

namespace Utilities.DotNet.XML.Test
{
    public class XmlUtilitiesTest
    {
        private readonly Uri m_fileuri;
        private readonly XDocument m_doc;

        public XmlUtilitiesTest()
        {
            CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

            var filename = Path.GetTempFileName();

            string[] contents =
            {
                "<Root>",
                "<String value='foo' empty=''/>",
                "<Int value='-45' empty='' invalid='bar'/>",
                "<UInt value='435' empty='' invalid='bar'/>",
                "<Double value='23.645' empty='' invalid='bar'/>",
                "<Bool true='true' false='false' empty='' invalid='bar'/>",
                "<Enum value1='Option1' value2=' Option2 ' empty='' invalid='bar'/>",
                "<EnumFlags value1='Option1' value2='Option1, Option2' empty='' invalid='bar'/>",
                "<Int value='450'/>",
                "<Text>FooBar</Text>",
                "<Nesting><Text>VoidDoid</Text></Nesting>",
                "<Guid value='089e1f22-b2d0-41a8-ab19-65eac650e589' invalid='foo'/>",
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

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
            {
                element.MandatoryAttribute( "bar" );
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

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
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

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
            {
                element.MandatoryAttributeInt( "invalid" );
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

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
            {
                element.MandatoryAttributeUInt( "invalid" );
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

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
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

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
            {
                element.MandatoryAttributeBool( "invalid" );
            } );

            Assert.Equal( $"XML element 'Bool' attribute 'invalid' has an invalid value 'bar' (expected boolean value)", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 6, exception.Line );
        }

        [Fact]
        public void MandatoryAttributeGuid_Existing()
        {
            var element = GetElement( "Guid" );

            var value = element.MandatoryAttributeGuid( "value" );

            Assert.Equal( Guid.Parse( "089e1f22-b2d0-41a8-ab19-65eac650e589" ), value );
        }

        [Fact]
        public void MandatoryAttributeGuid_Invalid()
        {
            var element = GetElement( "Guid" );

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
            {
                element.MandatoryAttributeGuid( "invalid" );
            } );

            Assert.Equal( $"XML element 'Guid' attribute 'invalid' has an invalid value 'foo' (expected GUID value)", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 12, exception.Line );
        }

        public enum ETest
        {
            Option1,
            Option2,
        }

        [Theory]
        [InlineData( "value1", ETest.Option1 )]
        [InlineData( "value2", ETest.Option2 )]
        public void MandatoryAttributeEnum_Existing( string attribute, ETest expectedValue )
        {
            var element = GetElement( "Enum" );

            var value = element.MandatoryAttributeEnum<ETest>( attribute );

            Assert.Equal( expectedValue, value );
        }

        [Fact]
        public void MandatoryAttributeEnum_Invalid()
        {
            var element = GetElement( "Enum" );

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
            {
                element.MandatoryAttributeEnum<ETest>( "invalid" );
            } );

            Assert.Equal( $"XML element 'Enum' attribute 'invalid' has an invalid value 'bar' (expected one of: Option1, Option2)", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 7, exception.Line );
        }

        [Flags]
        public enum EFlagsTest
        {
            None = 0,
            Option1 = 0x01,
            Option2 = 0x02,
            Option3 = 0x04,
        }

        [Theory]
        [InlineData( "value1", EFlagsTest.Option1 )]
        [InlineData( "value2", EFlagsTest.Option1 | EFlagsTest.Option2 )]
        public void MandatoryAttributeEnumFlags_Existing( string attribute, EFlagsTest expectedValue )
        {
            var element = GetElement( "EnumFlags" );

            var value = element.MandatoryAttributeEnum<EFlagsTest>( attribute );

            Assert.Equal( expectedValue, value );
        }

        [Fact]
        public void MandatoryAttributeEnumFlags_Invalid()
        {
            var element = GetElement( "EnumFlags" );

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
            {
                element.MandatoryAttributeEnum<EFlagsTest>( "invalid" );
            } );

            Assert.Equal( $"XML element 'EnumFlags' attribute 'invalid' has an invalid value 'bar' (expected one of: None, Option1, Option2, Option3)", exception.ShortMessage );
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

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
            {
                element.OptionalAttributeInt( "invalid" );
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

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
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

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
            {
                element.OptionalAttributeDouble( "invalid" );
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
            var element = GetElement( "Bool" );

            var valueTrue = element.OptionalAttributeBool( "bar", true );
            var valueFalse = element.OptionalAttributeBool( "baz", false );

            Assert.True( valueTrue );
            Assert.False( valueFalse );
        }

        [Fact]
        public void OptionalAttributeBool_Invalid()
        {
            var element = GetElement( "Bool" );

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
            {
                element.OptionalAttributeBool( "invalid" );
            } );

            Assert.Equal( $"XML element 'Bool' attribute 'invalid' has an invalid value 'bar' (expected boolean value)", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 6, exception.Line );
        }

        [Fact]
        public void OptionalAttributeGuid_Existing()
        {
            var element = GetElement( "Guid" );

            var value = element.OptionalAttributeGuid( "value" );

            Assert.Equal( Guid.Parse( "089e1f22-b2d0-41a8-ab19-65eac650e589" ), value );
        }

        [Fact]
        public void OptionalAttributeGuid_NotExisting()
        {
            var element = GetElement( "Guid" );

            var value1 = element.OptionalAttributeGuid( "bar" );
            var value2 = element.OptionalAttributeGuid( "bar", Guid.Empty );

            Assert.Null( value1 );
            Assert.Equal( Guid.Empty, value2 );
        }

        [Fact]
        public void OptionalAttributeGuid_Invalid()
        {
            var element = GetElement( "Guid" );

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
            {
                element.OptionalAttributeGuid( "invalid" );
            } );

            Assert.Equal( $"XML element 'Guid' attribute 'invalid' has an invalid value 'foo' (expected GUID value)", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 12, exception.Line );
        }

        [Theory]
        [InlineData( "value1", ETest.Option2, ETest.Option1 )]
        [InlineData( "value2", ETest.Option1, ETest.Option2 )]
        public void OptionalAttributeEnum_Existing( string attribute, ETest defaultValue, ETest expectedValue )
        {

            var element = GetElement( "Enum" );

            var value = element.OptionalAttributeEnum<ETest>( attribute, defaultValue );

            Assert.Equal( expectedValue, value );
        }

        [Fact]
        public void OptionalAttributeEnum_NotExisting()
        {

            var element = GetElement( "Enum" );

            var value = element.OptionalAttributeEnum<ETest>( "bar", ETest.Option2 );

            Assert.Equal( ETest.Option2, value );
        }

        [Fact]
        public void OptionalAttributeEnum_Invalid()
        {
            var element = GetElement( "Enum" );

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
            {
                element.OptionalAttributeEnum<ETest>( "invalid" );
            } );

            Assert.Equal( $"XML element 'Enum' attribute 'invalid' has an invalid value 'bar' (expected one of: Option1, Option2)", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 7, exception.Line );
        }

        [Theory]
        [InlineData( "value1", EFlagsTest.Option2, EFlagsTest.Option1 )]
        [InlineData( "value2", EFlagsTest.None, EFlagsTest.Option1 | EFlagsTest.Option2 )]
        public void OptionalAttributeEnumFlags_Existing( string attribute, EFlagsTest defaultValue, EFlagsTest expectedValue )
        {

            var element = GetElement( "EnumFlags" );

            var value = element.OptionalAttributeEnum<EFlagsTest>( attribute, defaultValue );

            Assert.Equal( expectedValue, value );
        }

        [Fact]
        public void OptionalAttributeEnumFlags_NotExisting()
        {

            var element = GetElement( "EnumFlags" );

            var value = element.OptionalAttributeEnum<EFlagsTest>( "bar", EFlagsTest.Option2 );

            Assert.Equal( EFlagsTest.Option2, value );
        }

        [Fact]
        public void OptionalAttributeEnumFlags_Invalid()
        {
            var element = GetElement( "EnumFlags" );

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
            {
                var value = element.OptionalAttributeEnum<EFlagsTest>( "invalid" );
            } );

            Assert.Equal( $"XML element 'EnumFlags' attribute 'invalid' has an invalid value 'bar' (expected one of: None, Option1, Option2, Option3)", exception.ShortMessage );
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

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
            {
                string attributeName;
                element.MandatoryAttribute( new string[] { "bar", "baz" }, out attributeName );
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

            var exception = Assert.Throws<XmlFileProcessingException>( () => rootElement.MandatoryUniqueElement( "Foo" ) );

            Assert.Equal( $"XML element 'Root' lacks mandatory child element 'Foo'", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 1, exception.Line );
        }

        [Fact]
        public void MandatoryUniqueElement_NotUnique()
        {
            var rootElement = m_doc!.Root!;

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
            {
                rootElement.MandatoryUniqueElement( "Int" );
            } );

            Assert.Equal( $"XML element 'Root' has more than 1 child element 'Int'", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 3, exception.Line );
        }

        [Fact]
        public void MandatoryUniqueElementText_NotEmpty()
        {
            var rootElement = m_doc!.Root!;

            var elementText = rootElement.MandatoryUniqueElementText( "Text" );

            Assert.Equal( "FooBar", elementText );
        }

        [Fact]
        public void MandatoryUniqueElementText_Empty_Allowed()
        {
            var rootElement = m_doc!.Root!;

            var elementText = rootElement.MandatoryUniqueElementText( "String", true );

            Assert.Equal( "", elementText );
        }

        [Fact]
        public void MandatoryUniqueElementText_Empty_NotAllowed()
        {
            var rootElement = m_doc!.Root!;

            var exception = Assert.Throws<XmlFileProcessingException>( () => rootElement.MandatoryUniqueElementText( "String" ) );

            Assert.Equal( $"XML element 'String' is empty", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 2, exception.Line );
        }

        [Fact]
        public void MandatoryUniqueElementText_WithChildElements()
        {
            var rootElement = m_doc!.Root!;

            var exception = Assert.Throws<XmlFileProcessingException>( () => rootElement.MandatoryUniqueElementText( "Nesting" ) );

            Assert.Equal( $"XML element 'Nesting' has child elements", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 11, exception.Line );
        }

        [Fact]
        public void MandatoryUniqueElementText_NotExisting()
        {
            var rootElement = m_doc!.Root!;

            var exception = Assert.Throws<XmlFileProcessingException>( () => rootElement.MandatoryUniqueElementText( "Foo" ) );

            Assert.Equal( $"XML element 'Root' lacks mandatory child element 'Foo'", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 1, exception.Line );
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

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
            {
                var element = rootElement.OptionalUniqueElement( "Int" );
            } );

            Assert.Equal( $"XML element 'Root' has more than 1 child element 'Int'", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 3, exception.Line );
        }

        [Fact]
        public void OptionalUniqueElementText_Existing_NotEmpty()
        {
            var rootElement = m_doc!.Root!;

            var elementText = rootElement.OptionalUniqueElementText( "Text" );

            Assert.Equal( "FooBar", elementText );
        }

        [Fact]
        public void OptionalUniqueElementText_Existing_Empty()
        {
            var rootElement = m_doc!.Root!;

            var elementText = rootElement.OptionalUniqueElementText( "String" );

            Assert.Equal( "", elementText );
        }

        [Fact]
        public void OptionalUniqueElementText_WithChildElements()
        {
            var rootElement = m_doc!.Root!;

            var exception = Assert.Throws<XmlFileProcessingException>( () => rootElement.OptionalUniqueElementText( "Nesting" ) );

            Assert.Equal( $"XML element 'Nesting' has child elements", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 11, exception.Line );
        }

        [Fact]
        public void OptionalUniqueElementText_NotExisting()
        {
            var rootElement = m_doc!.Root!;

            var elementText = rootElement.OptionalUniqueElementText( "Foo" );

            Assert.Null( elementText );
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
        public void AddUnique_AlreadyExisting()
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
