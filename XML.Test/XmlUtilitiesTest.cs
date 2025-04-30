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
                "<Enum value1='Option1' value2=' Option2 ' value3='option1' value4='OPTiON2' empty='' invalid='bar'/>",
                "<EnumFlags value1='Option1' value2='Option1, Option2' empty='' invalid='bar'/>",
                "<Int value='450'/>",
                "<Text>FooBar</Text>",
                "<Nesting><Text>VoidDoid</Text></Nesting>",
                "<Guid value='089e1f22-b2d0-41a8-ab19-65eac650e589' invalid='foo'/>",
                "<DateTime invariant='12/06/2028' fr='11/08/2025' ja='2027/09/07' local='2029-02-15T12:25:36.0000000-07:00' " +
                          "utc='2039-11-30T23:45:22.1900000Z' invalid='foo'/>",
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
            // Arrange

            var element = GetElement( "String" );

            // Act

            var value = element.MandatoryAttribute( "value" );

            // Assert

            Assert.Equal( "foo", value );
        }

        [Fact]
        public void MandatoryAttribute_NotExisting()
        {
            // Arrange

            var element = GetElement( "String" );

            // Act

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
            {
                element.MandatoryAttribute( "bar" );
            } );

            // Assert

            Assert.Equal( $"XML element 'String' lacks mandatory attribute 'bar'", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 2, exception.Line );
        }

        [Fact]
        public void MandatoryAttribute_Empty_Allowed()
        {
            // Arrange

            var element = GetElement( "String" );

            // Act

            var value = element.MandatoryAttribute( "empty", true );

            // Assert

            Assert.Equal( "", value );
        }

        [Fact]
        public void MandatoryAttribute_Empty_NotAllowed()
        {
            // Arrange

            var element = GetElement( "String" );

            // Act

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
            {
                element.MandatoryAttribute( "empty" );
            } );

            // Assert

            Assert.Equal( $"XML element 'String' mandatory attribute 'empty' is empty", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 2, exception.Line );
        }

        [Fact]
        public void MandatoryAttributeInt_Existing()
        {
            // Arrange

            var element = GetElement( "Int" );

            // Act

            var value = element.MandatoryAttributeInt( "value" );

            // Assert

            Assert.Equal( -45, value );
        }

        [Fact]
        public void MandatoryAttributeInt_Invalid()
        {
            // Arrange

            var element = GetElement( "Int" );

            // Act

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
            {
                element.MandatoryAttributeInt( "invalid" );
            } );

            // Assert

            Assert.Equal( $"XML element 'Int' attribute 'invalid' has an invalid value 'bar' (expected integer value)", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 3, exception.Line );
        }

        [Fact]
        public void MandatoryAttributeUInt_Existing()
        {
            // Arrange

            var element = GetElement( "UInt" );

            // Act

            var value = element.MandatoryAttributeUInt( "value" );

            // Assert

            Assert.Equal( 435U, value );
        }

        [Fact]
        public void MandatoryAttributeUInt_Invalid()
        {
            // Arrange

            var element = GetElement( "UInt" );

            // Act

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
            {
                element.MandatoryAttributeUInt( "invalid" );
            } );

            // Assert

            Assert.Equal( $"XML element 'UInt' attribute 'invalid' has an invalid value 'bar' (expected unsigned integer value)", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 4, exception.Line );
        }

        [Fact]
        public void MandatoryAttributeDouble_Existing()
        {
            // Arrange

            var element = GetElement( "Double" );

            // Act

            var value = element.MandatoryAttributeDouble( "value" );

            // Assert

            Assert.Equal( 23.645, value, 3 );
        }

        [Fact]
        public void MandatoryAttributeDouble_Invalid()
        {
            // Arrange

            var element = GetElement( "Double" );

            // Act

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
            {
                element.MandatoryAttributeDouble( "invalid" );
            } );

            // Assert

            Assert.Equal( $"XML element 'Double' attribute 'invalid' has an invalid value 'bar' (expected real number value)", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 5, exception.Line );
        }

        [Fact]
        public void MandatoryAttributeBool_Existing()
        {
            // Arrange

            var element = GetElement( "Bool" );

            // Act

            var valueTrue = element.MandatoryAttributeBool( "true" );
            var valueFalse = element.MandatoryAttributeBool( "false" );

            // Assert

            Assert.True( valueTrue );
            Assert.False( valueFalse );
        }

        [Fact]
        public void MandatoryAttributeBool_Invalid()
        {
            // Arrange

            var element = GetElement( "Bool" );

            // Act

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
            {
                element.MandatoryAttributeBool( "invalid" );
            } );

            // Assert

            Assert.Equal( $"XML element 'Bool' attribute 'invalid' has an invalid value 'bar' (expected boolean value)", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 6, exception.Line );
        }

        [Fact]
        public void MandatoryAttributeGuid_Existing()
        {
            // Arrange

            var element = GetElement( "Guid" );

            // Act

            var value = element.MandatoryAttributeGuid( "value" );

            // Assert

            Assert.Equal( Guid.Parse( "089e1f22-b2d0-41a8-ab19-65eac650e589" ), value );
        }

        [Fact]
        public void MandatoryAttributeGuid_Invalid()
        {
            // Arrange

            var element = GetElement( "Guid" );

            // Act

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
            {
                element.MandatoryAttributeGuid( "invalid" );
            } );

            // Assert

            Assert.Equal( $"XML element 'Guid' attribute 'invalid' has an invalid value 'foo' (expected GUID value)", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 12, exception.Line );
        }

        [Theory]
        [InlineData( "invariant", 2028, 12, 6, null )]
        [InlineData( "fr", 2025, 8, 11, "fr-FR" )]
        [InlineData( "ja", 2027, 9, 7, "ja-JP" )]
        public void MandatoryAttributeDateTime_Existing( string attribute, int year, int month, int day, string? culture )
        {
            // Arrange

            var element = GetElement( "DateTime" );

            var cultureInfo = ( culture != null ) ? new CultureInfo( culture ) : null;

            // Act

            var value = element.MandatoryAttributeDateTime( attribute, cultureInfo );

            // Assert

            Assert.Equal( year, value.Year );
            Assert.Equal( month, value.Month );
            Assert.Equal( day, value.Day );
            Assert.Equal( 0, value.Hour );
            Assert.Equal( 0, value.Minute );
            Assert.Equal( 0, value.Second );
            Assert.Equal( 0, value.Millisecond );
            Assert.Equal( DateTimeKind.Unspecified, value.Kind );
        }

        //"<DateTime local='2029-02-15T12:25:36.0000000-07:00' utc='2039-11-30T23:45:22.1900000Z' invalid='foo'/>",

        [Theory]
        [InlineData( "local", 2029, 2, 15, 20, 25, 36, 0, DateTimeKind.Local )]
        [InlineData( "utc", 2039, 11, 30, 23, 45, 22, 190, DateTimeKind.Utc )]
        public void MandatoryAttributeDateTime_WithFormat_Existing( string attribute, int year, int month, int day,
                                                                    int hour, int minute, int second, int milliseconds, DateTimeKind kind )
        {
            // Arrange

            var element = GetElement( "DateTime" );

            // Act

            var value = element.MandatoryAttributeDateTime( attribute, "o", styles: DateTimeStyles.RoundtripKind );

            // Assert

            Assert.Equal( year, value.Year );
            Assert.Equal( month, value.Month );
            Assert.Equal( day, value.Day );
            Assert.Equal( hour, value.Hour );
            Assert.Equal( minute, value.Minute );
            Assert.Equal( second, value.Second );
            Assert.Equal( milliseconds, value.Millisecond );
            Assert.Equal( kind, value.Kind );
        }

        [Fact]
        public void MandatoryAttributeDataTime_Invalid()
        {
            // Arrange

            var element = GetElement( "DateTime" );

            // Act

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
            {
                element.MandatoryAttributeDateTime( "invalid" );
            } );

            // Assert

            Assert.Equal( $"XML element 'DateTime' attribute 'invalid' has an invalid value 'foo' (expected date/time value)", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 13, exception.Line );
        }

        [Fact]
        public void MandatoryAttributeDataTime_WithFormat_Invalid()
        {
            // Arrange

            var element = GetElement( "DateTime" );

            // Act

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
            {
                element.MandatoryAttributeDateTime( "invalid", "s" );
            } );

            // Assert

            Assert.Equal( $"XML element 'DateTime' attribute 'invalid' has an invalid value 'foo' (expected date/time value)", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 13, exception.Line );
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
            // Arrange

            var element = GetElement( "Enum" );

            // Act

            var value = element.MandatoryAttributeEnum<ETest>( attribute );

            // Assert

            Assert.Equal( expectedValue, value );
        }

        [Theory]
        [InlineData( "value1", ETest.Option1 )]
        [InlineData( "value2", ETest.Option2 )]
        [InlineData( "value3", ETest.Option1 )]
        [InlineData( "value4", ETest.Option2 )]
        public void MandatoryAttributeEnum_Existing_CaseInsensitive( string attribute, ETest expectedValue )
        {
            // Arrange

            var element = GetElement( "Enum" );

            // Act

            var value = element.MandatoryAttributeEnum<ETest>( attribute, true );

            // Assert

            Assert.Equal( expectedValue, value );
        }

        [Theory]
        [InlineData( "invalid" )]
        [InlineData( "value3" )]
        [InlineData( "value4" )]
        public void MandatoryAttributeEnum_Invalid( string attribute )
        {
            // Arrange

            var element = GetElement( "Enum" );

            // Act

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
            {
                element.MandatoryAttributeEnum<ETest>( attribute );
            } );

            // Assert

            Assert.Equal( $"XML element 'Enum' attribute '{attribute}' has an invalid value '{element.Attribute( attribute )?.Value}' (expected one of: Option1, Option2)", exception.ShortMessage );
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
            // Arrange

            var element = GetElement( "EnumFlags" );

            // Act

            var value = element.MandatoryAttributeEnum<EFlagsTest>( attribute );

            // Assert

            Assert.Equal( expectedValue, value );
        }

        [Fact]
        public void MandatoryAttributeEnumFlags_Invalid()
        {
            // Arrange

            var element = GetElement( "EnumFlags" );

            // Act

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
            {
                element.MandatoryAttributeEnum<EFlagsTest>( "invalid" );
            } );

            // Assert

            Assert.Equal( $"XML element 'EnumFlags' attribute 'invalid' has an invalid value 'bar' (expected one of: None, Option1, Option2, Option3)", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 8, exception.Line );
        }

        [Fact]
        public void OptionalAttribute_Existing()
        {
            // Arrange

            var element = GetElement( "String" );

            // Act

            var value = element.OptionalAttribute( "value", "bar" );

            // Assert

            Assert.Equal( "foo", value );
        }

        [Fact]
        public void OptionalAttribute_NotExisting()
        {
            // Arrange

            var element = GetElement( "String" );

            // Act

            var value = element.OptionalAttribute( "bar", "fizz" );

            // Assert

            Assert.Equal( "fizz", value );
        }

        [Fact]
        public void OptionalAttributeInt_Existing()
        {
            // Arrange

            var element = GetElement( "Int" );

            // Act

            var value = element.OptionalAttributeInt( "value", 76645 );

            // Assert

            Assert.Equal( -45, value );
        }

        [Fact]
        public void OptionalAttributeInt_NotExisting()
        {
            // Arrange

            var element = GetElement( "Int" );

            // Act

            var value = element.OptionalAttributeInt( "bar", 7687 );

            // Assert

            Assert.Equal( 7687, value );
        }

        [Fact]
        public void OptionalAttributeInt_Invalid()
        {
            // Arrange

            var element = GetElement( "Int" );

            // Act

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
            {
                element.OptionalAttributeInt( "invalid" );
            } );

            // Assert

            Assert.Equal( $"XML element 'Int' attribute 'invalid' has an invalid value 'bar' (expected integer value)", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 3, exception.Line );
        }

        [Fact]
        public void OptionalAttributeUInt_Existing()
        {
            // Arrange

            var element = GetElement( "UInt" );

            // Act

            var value = element.OptionalAttributeUInt( "value", 5454U );

            // Assert

            Assert.Equal( 435U, value );
        }

        [Fact]
        public void OptionalAttributeUInt_NotExisting()
        {
            // Arrange

            var element = GetElement( "UInt" );

            // Act

            var value = element.OptionalAttributeUInt( "bar", 775U );

            // Assert

            Assert.Equal( 775U, value );
        }

        [Fact]
        public void OptionalAttributeUInt_Invalid()
        {
            // Arrange

            var element = GetElement( "UInt" );

            // Act

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
            {
                element.OptionalAttributeUInt( "invalid" );
            } );

            // Assert

            Assert.Equal( $"XML element 'UInt' attribute 'invalid' has an invalid value 'bar' (expected unsigned integer value)", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 4, exception.Line );
        }

        [Fact]
        public void OptionalAttributeDouble_Existing()
        {
            // Arrange

            var element = GetElement( "Double" );

            // Act

            var value = element.OptionalAttributeDouble( "value", 456.3 );

            // Assert

            Assert.Equal( 23.645, value, 3 );
        }

        [Fact]
        public void OptionalAttributeDouble_NotExisting()
        {
            // Arrange

            var element = GetElement( "Double" );

            // Act

            var value = element.OptionalAttributeDouble( "bar", 77434.2432 );

            // Assert

            Assert.Equal( 77434.2432, value, 4 );
        }

        [Fact]
        public void OptionalAttributeDouble_Invalid()
        {
            // Arrange

            var element = GetElement( "Double" );

            // Act

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
            {
                element.OptionalAttributeDouble( "invalid" );
            } );

            // Assert

            Assert.Equal( $"XML element 'Double' attribute 'invalid' has an invalid value 'bar' (expected real number value)", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 5, exception.Line );
        }

        [Fact]
        public void OptionalAttributeBool_Existing()
        {
            // Arrange

            var element = GetElement( "Bool" );

            // Act

            var valueTrue = element.OptionalAttributeBool( "true", false );
            var valueFalse = element.OptionalAttributeBool( "false", true );

            // Assert

            Assert.True( valueTrue );
            Assert.False( valueFalse );
        }

        [Fact]
        public void OptionalAttributeBool_NotExisting()
        {
            // Arrange

            var element = GetElement( "Bool" );

            // Act

            var valueTrue = element.OptionalAttributeBool( "bar", true );
            var valueFalse = element.OptionalAttributeBool( "baz", false );

            // Assert

            Assert.True( valueTrue );
            Assert.False( valueFalse );
        }

        [Fact]
        public void OptionalAttributeBool_Invalid()
        {
            // Arrange

            var element = GetElement( "Bool" );

            // Act

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
            {
                element.OptionalAttributeBool( "invalid" );
            } );

            // Assert

            Assert.Equal( $"XML element 'Bool' attribute 'invalid' has an invalid value 'bar' (expected boolean value)", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 6, exception.Line );
        }

        [Fact]
        public void OptionalAttributeGuid_Existing()
        {
            // Arrange

            var element = GetElement( "Guid" );

            // Act

            var value = element.OptionalAttributeGuid( "value" );

            // Assert

            Assert.Equal( Guid.Parse( "089e1f22-b2d0-41a8-ab19-65eac650e589" ), value );
        }

        [Fact]
        public void OptionalAttributeGuid_NotExisting()
        {
            // Arrange

            var element = GetElement( "Guid" );

            // Act

            var value1 = element.OptionalAttributeGuid( "bar" );
            var value2 = element.OptionalAttributeGuid( "bar", Guid.Empty );

            // Assert

            Assert.Null( value1 );
            Assert.Equal( Guid.Empty, value2 );
        }

        [Fact]
        public void OptionalAttributeGuid_Invalid()
        {
            // Arrange

            var element = GetElement( "Guid" );

            // Act

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
            {
                element.OptionalAttributeGuid( "invalid" );
            } );

            // Assert

            Assert.Equal( $"XML element 'Guid' attribute 'invalid' has an invalid value 'foo' (expected GUID value)", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 12, exception.Line );
        }

        [Theory]
        [InlineData( "invariant", 2028, 12, 6, null )]
        [InlineData( "fr", 2025, 8, 11, "fr-FR" )]
        [InlineData( "ja", 2027, 9, 7, "ja-JP" )]
        public void OptionalAttributeDateTime_Existing( string attribute, int year, int month, int day, string? culture )
        {
            // Arrange

            var element = GetElement( "DateTime" );

            var cultureInfo = ( culture != null ) ? new CultureInfo( culture ) : null;

            // Act

            var value = element.OptionalAttributeDateTime( attribute, cultureInfo );

            // Assert

            Assert.NotNull( value );
            Assert.Equal( year, value.Value.Year );
            Assert.Equal( month, value.Value.Month );
            Assert.Equal( day, value.Value.Day );
            Assert.Equal( 0, value.Value.Hour );
            Assert.Equal( 0, value.Value.Minute );
            Assert.Equal( 0, value.Value.Second );
            Assert.Equal( 0, value.Value.Millisecond );
            Assert.Equal( DateTimeKind.Unspecified, value.Value.Kind );
        }

        //"<DateTime local='2029-02-15T12:25:36.0000000-07:00' utc='2039-11-30T23:45:22.1900000Z' invalid='foo'/>",

        [Theory]
        [InlineData( "local", 2029, 2, 15, 20, 25, 36, 0, DateTimeKind.Local )]
        [InlineData( "utc", 2039, 11, 30, 23, 45, 22, 190, DateTimeKind.Utc )]
        public void OptionalAttributeDateTime_WithFormat_Existing( string attribute, int year, int month, int day,
                                                                    int hour, int minute, int second, int milliseconds, DateTimeKind kind )
        {
            // Arrange

            var element = GetElement( "DateTime" );

            // Act

            var value = element.OptionalAttributeDateTime( attribute, "o", styles: DateTimeStyles.RoundtripKind );

            // Assert

            Assert.NotNull( value );
            Assert.Equal( year, value.Value.Year );
            Assert.Equal( month, value.Value.Month );
            Assert.Equal( day, value.Value.Day );
            Assert.Equal( hour, value.Value.Hour );
            Assert.Equal( minute, value.Value.Minute );
            Assert.Equal( second, value.Value.Second );
            Assert.Equal( milliseconds, value.Value.Millisecond );
            Assert.Equal( kind, value.Value.Kind );
        }

        [Fact]
        public void OptionalAttributeDateTime_NotExisting()
        {
            // Arrange

            var element = GetElement( "DateTime" );

            // Act

            var value = element.OptionalAttributeDateTime( "bar" );

            // Assert

            Assert.Null( value );
        }

        [Fact]
        public void OptionalAttributeDateTime_WithFormat_NotExisting()
        {
            // Arrange

            var element = GetElement( "DateTime" );

            // Act

            var value = element.OptionalAttributeDateTime( "bar", "g" );

            // Assert

            Assert.Null( value );
        }

        [Fact]
        public void OptionalAttributeDataTime_Invalid()
        {
            // Arrange

            var element = GetElement( "DateTime" );

            // Act

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
            {
                element.OptionalAttributeDateTime( "invalid" );
            } );

            // Assert

            Assert.Equal( $"XML element 'DateTime' attribute 'invalid' has an invalid value 'foo' (expected date/time value)", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 13, exception.Line );
        }

        [Fact]
        public void OptionalAttributeDataTime_WithFormat_Invalid()
        {
            // Arrange

            var element = GetElement( "DateTime" );

            // Act

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
            {
                element.OptionalAttributeDateTime( "invalid", "s" );
            } );

            // Assert

            Assert.Equal( $"XML element 'DateTime' attribute 'invalid' has an invalid value 'foo' (expected date/time value)", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 13, exception.Line );
        }

        [Theory]
        [InlineData( "value1", ETest.Option2, ETest.Option1 )]
        [InlineData( "value2", ETest.Option1, ETest.Option2 )]
        public void OptionalAttributeEnum_Existing( string attribute, ETest defaultValue, ETest expectedValue )
        {
            // Arrange

            var element = GetElement( "Enum" );

            // Act

            var value = element.OptionalAttributeEnum<ETest>( attribute, defaultValue );

            // Assert

            Assert.Equal( expectedValue, value );
        }

        [Theory]
        [InlineData( "value1", ETest.Option2, ETest.Option1 )]
        [InlineData( "value2", ETest.Option1, ETest.Option2 )]
        [InlineData( "value3", ETest.Option2, ETest.Option1 )]
        [InlineData( "value4", ETest.Option1, ETest.Option2 )]
        public void OptionalAttributeEnum_Existing_CaseInsensitive( string attribute, ETest defaultValue, ETest expectedValue )
        {
            // Arrange

            var element = GetElement( "Enum" );

            // Act

            var value = element.OptionalAttributeEnum( attribute, defaultValue, true );

            // Assert

            Assert.Equal( expectedValue, value );
        }

        [Fact]
        public void OptionalAttributeEnum_NotExisting()
        {
            // Arrange

            var element = GetElement( "Enum" );

            // Act

            var value = element.OptionalAttributeEnum( "bar", ETest.Option2 );

            // Assert

            Assert.Equal( ETest.Option2, value );
        }

        [Theory]
        [InlineData( "invalid" )]
        [InlineData( "value3" )]
        [InlineData( "value4" )]
        public void OptionalAttributeEnum_Invalid( string attribute )
        {
            // Arrange

            var element = GetElement( "Enum" );

            // Act

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
            {
                element.OptionalAttributeEnum<ETest>( attribute );
            } );

            // Assert

            Assert.Equal( $"XML element 'Enum' attribute '{attribute}' has an invalid value '{element.Attribute( attribute )?.Value}' (expected one of: Option1, Option2)", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 7, exception.Line );
        }

        [Theory]
        [InlineData( "value1", EFlagsTest.Option2, EFlagsTest.Option1 )]
        [InlineData( "value2", EFlagsTest.None, EFlagsTest.Option1 | EFlagsTest.Option2 )]
        public void OptionalAttributeEnumFlags_Existing( string attribute, EFlagsTest defaultValue, EFlagsTest expectedValue )
        {
            // Arrange

            var element = GetElement( "EnumFlags" );

            // Act

            var value = element.OptionalAttributeEnum<EFlagsTest>( attribute, defaultValue );

            // Assert

            Assert.Equal( expectedValue, value );
        }

        [Fact]
        public void OptionalAttributeEnumFlags_NotExisting()
        {
            // Arrange

            var element = GetElement( "EnumFlags" );

            // Act

            var value = element.OptionalAttributeEnum<EFlagsTest>( "bar", EFlagsTest.Option2 );

            // Assert

            Assert.Equal( EFlagsTest.Option2, value );
        }

        [Fact]
        public void OptionalAttributeEnumFlags_Invalid()
        {
            // Arrange

            var element = GetElement( "EnumFlags" );

            // Act

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
            {
                element.OptionalAttributeEnum<EFlagsTest>( "invalid" );
            } );

            // Assert

            Assert.Equal( $"XML element 'EnumFlags' attribute 'invalid' has an invalid value 'bar' (expected one of: None, Option1, Option2, Option3)", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 8, exception.Line );
        }

        [Fact]
        public void MandatoryAttribute_Multiple_Existing()
        {
            // Arrange

            var element = GetElement( "String" );
            var attributeNames = new string[] { "bar", "value" };

            // Act

            var value = element.MandatoryAttribute( attributeNames, out var attributeName );

            // Assert

            Assert.Equal( "foo", value );
            Assert.Equal( "value", attributeName );
        }

        [Fact]
        public void MandatoryAttribute_Multiple_NotExisting()
        {
            // Arrange

            var element = GetElement( "String" );

            string[] attributeNames = new string[] { "bar", "baz" };

            // Act

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
            {
                element.MandatoryAttribute( attributeNames, out var _ );
            } );

            // Assert

            Assert.Equal( $"XML element 'String' lacks one of mandatory attributes ('bar', 'baz') or are empty", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 2, exception.Line );
        }

        [Fact]
        public void MandatoryUniqueElement_Existing()
        {
            // Arrange

            var rootElement = m_doc!.Root!;

            // Act

            var element = rootElement.MandatoryUniqueElement( "String" );

            // Assert

            Assert.Equal( "String", element.Name );
            Assert.Equal( "foo", element.MandatoryAttribute( "value" ) );
        }

        [Fact]
        public void MandatoryUniqueElement_NotExisting()
        {
            // Arrange

            var rootElement = m_doc!.Root!;

            // Act

            var exception = Assert.Throws<XmlFileProcessingException>( () => rootElement.MandatoryUniqueElement( "Foo" ) );

            // Assert

            Assert.Equal( $"XML element 'Root' lacks mandatory child element 'Foo'", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 1, exception.Line );
        }

        [Fact]
        public void MandatoryUniqueElement_NotUnique()
        {
            // Arrange

            var rootElement = m_doc!.Root!;

            // Act

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
            {
                rootElement.MandatoryUniqueElement( "Int" );
            } );

            // Assert

            Assert.Equal( $"XML element 'Root' has more than 1 child element 'Int'", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 3, exception.Line );
        }

        [Fact]
        public void MandatoryUniqueElementText_NotEmpty()
        {
            // Arrange

            var rootElement = m_doc!.Root!;

            // Act

            var elementText = rootElement.MandatoryUniqueElementText( "Text" );

            // Assert

            Assert.Equal( "FooBar", elementText );
        }

        [Fact]
        public void MandatoryUniqueElementText_Empty_Allowed()
        {
            // Arrange

            var rootElement = m_doc!.Root!;

            // Act

            var elementText = rootElement.MandatoryUniqueElementText( "String", true );

            // Assert

            Assert.Equal( "", elementText );
        }

        [Fact]
        public void MandatoryUniqueElementText_Empty_NotAllowed()
        {
            // Arrange

            var rootElement = m_doc!.Root!;

            // Act

            var exception = Assert.Throws<XmlFileProcessingException>( () => rootElement.MandatoryUniqueElementText( "String" ) );

            // Assert

            Assert.Equal( $"XML element 'String' is empty", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 2, exception.Line );
        }

        [Fact]
        public void MandatoryUniqueElementText_WithChildElements()
        {
            // Arrange

            var rootElement = m_doc!.Root!;

            // Act

            var exception = Assert.Throws<XmlFileProcessingException>( () => rootElement.MandatoryUniqueElementText( "Nesting" ) );

            // Assert

            Assert.Equal( $"XML element 'Nesting' has child elements", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 11, exception.Line );
        }

        [Fact]
        public void MandatoryUniqueElementText_NotExisting()
        {
            // Arrange

            var rootElement = m_doc!.Root!;

            // Act

            var exception = Assert.Throws<XmlFileProcessingException>( () => rootElement.MandatoryUniqueElementText( "Foo" ) );

            // Assert

            Assert.Equal( $"XML element 'Root' lacks mandatory child element 'Foo'", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 1, exception.Line );
        }

        [Fact]
        public void OptionalUniqueElement_Existing()
        {
            // Arrange

            var rootElement = m_doc!.Root!;

            // Act

            var element = rootElement.OptionalUniqueElement( "String" );

            // Assert

            Assert.NotNull( element );
            Assert.Equal( "String", element.Name );
            Assert.Equal( "foo", element.MandatoryAttribute( "value" ) );
        }

        [Fact]
        public void OptionalUniqueElement_NotExisting()
        {
            // Arrange

            var rootElement = m_doc!.Root!;

            // Act

            var element = rootElement.OptionalUniqueElement( "Foo" );

            // Assert

            Assert.Null( element );
        }

        [Fact]
        public void OptionalUniqueElement_NotUnique()
        {
            // Arrange

            var rootElement = m_doc!.Root!;

            // Act

            var exception = Assert.Throws<XmlFileProcessingException>( () =>
            {
                rootElement.OptionalUniqueElement( "Int" );
            } );

            // Assert

            Assert.Equal( $"XML element 'Root' has more than 1 child element 'Int'", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 3, exception.Line );
        }

        [Fact]
        public void OptionalUniqueElementText_Existing_NotEmpty()
        {
            // Arrange

            var rootElement = m_doc!.Root!;

            // Act

            var elementText = rootElement.OptionalUniqueElementText( "Text" );

            // Assert

            Assert.Equal( "FooBar", elementText );
        }

        [Fact]
        public void OptionalUniqueElementText_Existing_Empty()
        {
            // Arrange

            var rootElement = m_doc!.Root!;

            // Act

            var elementText = rootElement.OptionalUniqueElementText( "String" );

            // Assert

            Assert.Equal( "", elementText );
        }

        [Fact]
        public void OptionalUniqueElementText_WithChildElements()
        {
            // Arrange

            var rootElement = m_doc!.Root!;

            // Act

            var exception = Assert.Throws<XmlFileProcessingException>( () => rootElement.OptionalUniqueElementText( "Nesting" ) );

            // Assert

            Assert.Equal( $"XML element 'Nesting' has child elements", exception.ShortMessage );
            Assert.Equal( m_fileuri.ToString(), exception.Filename );
            Assert.Equal( 11, exception.Line );
        }

        [Fact]
        public void OptionalUniqueElementText_NotExisting()
        {
            // Arrange

            var rootElement = m_doc!.Root!;

            // Act

            var elementText = rootElement.OptionalUniqueElementText( "Foo" );

            // Assert

            Assert.Null( elementText );
        }

        [Fact]
        public void AddUnique_NotExisting()
        {
            // Arrange

            var container = new XElement( "Container" );

            var child = new XElement( "Child" );

            // Act

            container.AddUnique( child );

            // Assert

            Assert.Equal( container, child.Parent );
            Assert.Single( container.Elements(), child );
        }

        [Fact]
        public void AddUnique_AlreadyExisting()
        {
            // Arrange

            var container = new XElement( "Container" );

            var child = new XElement( "Child" );

            container.Add( child );

            // Act

            container.AddUnique( child );

            // Assert

            Assert.Equal( container, child.Parent );
            Assert.Single( container.Elements(), child );
        }

        [Fact]
        public void AddUnique_Move()
        {
            // Arrange

            var container1 = new XElement( "Container1" );
            var container2 = new XElement( "Container2" );
            var child = new XElement( "Child" );

            container1.Add( child );

            // Act

            container2.AddUnique( child );

            // Assert

            Assert.Equal( container2, child.Parent );
            Assert.Empty( container1.Elements() );
            Assert.Single( container2.Elements(), child );
        }
    }
}
