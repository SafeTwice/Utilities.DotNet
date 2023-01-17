/// @file
/// @copyright  Copyright (c) 2020-2021 SafeTwice S.L. All rights reserved.
/// @license    MIT (https://opensource.org/licenses/MIT)

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using Utilities.Net.Exceptions;
using static Utilities.Net.I18N.LibraryLocalizer;

namespace Utilities.Net.XML
{
    public static class XmlUtilities
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        public static string MandatoryAttribute( this XElement element, string attributeName, bool allowEmpty = false )
        {
            var attrStr = element.Attribute( attributeName )?.Value;

            if( attrStr == null )
            {
                throw new FileProcessingException( Localize( $"XML element '{element.Name}' lacks mandatory attribute '{attributeName}'" ),
                                                   element.BaseUri, ( (IXmlLineInfo) element ).LineNumber );
            }
            else if( !allowEmpty && ( attrStr.Length == 0 ) )
            {
                throw new FileProcessingException( Localize( $"XML element '{element.Name}' mandatory attribute '{attributeName}' is empty" ),
                                                   element.BaseUri, ( (IXmlLineInfo) element ).LineNumber );
            }

            return attrStr;
        }

        public static int MandatoryAttributeInt( this XElement element, string attributeName )
        {
            string attrStr = element.MandatoryAttribute( attributeName );

            int attrValue;
            if( !int.TryParse( attrStr, out attrValue ) )
            {
                throw new FileProcessingException( Localize( $"XML element '{element.Name}' attribute '{attributeName}' has an invalid value '{attrStr}' (expected integer value)" ),
                                                   element.BaseUri, ( (IXmlLineInfo) element ).LineNumber );
            }

            return attrValue;
        }

        public static uint MandatoryAttributeUInt( this XElement element, string attributeName )
        {
            string attrStr = element.MandatoryAttribute( attributeName );

            uint attrValue;
            if( !uint.TryParse( attrStr, out attrValue ) )
            {
                throw new FileProcessingException( Localize( $"XML element '{element.Name}' attribute '{attributeName}' has an invalid value '{attrStr}' (expected unsigned integer value)" ),
                                                   element.BaseUri, ( (IXmlLineInfo) element ).LineNumber );
            }

            return attrValue;
        }

        public static double MandatoryAttributeDouble( this XElement element, string attributeName )
        {
            string attrStr = element.MandatoryAttribute( attributeName );

            double attrValue;
            if( !double.TryParse( attrStr, NumberStyles.Float, CultureInfo.InvariantCulture, out attrValue ) )
            {
                throw new FileProcessingException( Localize( $"XML element '{element.Name}' attribute '{attributeName}' has an invalid value '{attrStr}' (expected real number value)" ),
                                                   element.BaseUri, ( (IXmlLineInfo) element ).LineNumber );
            }

            return attrValue;
        }

        public static bool MandatoryAttributeBool( this XElement element, string attributeName )
        {
            string attrStr = element.MandatoryAttribute( attributeName );

            bool attrValue;
            if( !bool.TryParse( attrStr, out attrValue ) )
            {
                throw new FileProcessingException( Localize( $"XML element '{element.Name}' attribute '{attributeName}' has an invalid value '{attrStr}' (expected boolean value)" ),
                                                   element.BaseUri, ( (IXmlLineInfo) element ).LineNumber );
            }

            return attrValue;
        }

        public static T MandatoryAttributeEnum<T>( this XElement element, string attributeName ) where T : struct, Enum
        {
            var attrStr = element.MandatoryAttribute( attributeName );

            T attrValue;
            if( !Enum.TryParse( attrStr, out attrValue ) )
            {
                var enumValues = string.Join( ", ", Enum.GetNames( typeof( T ) ) );
                throw new FileProcessingException( Localize( $"XML element '{element.Name}' attribute '{attributeName}' has an invalid value '{attrStr}' (expected one of: {enumValues})" ),
                                                   element.BaseUri, ( (IXmlLineInfo) element ).LineNumber );
            }

            return attrValue;
        }

        public static string? OptionalAttribute( this XElement element, string attributeName )
        {
            return element.Attribute( attributeName )?.Value;
        }

        public static string OptionalAttribute( this XElement element, string attributeName, string defaultValue )
        {
            var attrStr = element.Attribute( attributeName )?.Value;

            if( attrStr == null )
            {
                attrStr = defaultValue;
            }

            return attrStr;
        }

        public static int OptionalAttributeInt( this XElement element, string attributeName, int defaultValue = 0 )
        {
            var attrStr = element.OptionalAttribute( attributeName );

            int attrValue = defaultValue;

            if( ( attrStr != null ) && !int.TryParse( attrStr, out attrValue ) )
            {
                throw new FileProcessingException( Localize( $"XML element '{element.Name}' attribute '{attributeName}' has an invalid value '{attrStr}' (expected integer value)" ),
                                                   element.BaseUri, ( (IXmlLineInfo) element ).LineNumber );
            }

            return attrValue;
        }

        public static uint OptionalAttributeUInt( this XElement element, string attributeName, uint defaultValue = 0 )
        {
            var attrStr = element.OptionalAttribute( attributeName );

            uint attrValue = defaultValue;

            if( ( attrStr != null ) && !uint.TryParse( attrStr, out attrValue ) )
            {
                throw new FileProcessingException( Localize( $"XML element '{element.Name}' attribute '{attributeName}' has an invalid value '{attrStr}' (expected unsigned integer value)" ),
                                                   element.BaseUri, ( (IXmlLineInfo) element ).LineNumber );
            }

            return attrValue;
        }

        public static double OptionalAttributeDouble( this XElement element, string attributeName, double defaultValue = 0.0 )
        {
            var attrStr = element.OptionalAttribute( attributeName );

            double attrValue = defaultValue;

            if( ( attrStr != null ) && !double.TryParse( attrStr, NumberStyles.Float, CultureInfo.InvariantCulture, out attrValue ) )
            {
                throw new FileProcessingException( Localize( $"XML element '{element.Name}' attribute '{attributeName}' has an invalid value '{attrStr}' (expected real number value)" ),
                                                   element.BaseUri, ( (IXmlLineInfo) element ).LineNumber );
            }

            return attrValue;
        }

        public static bool OptionalAttributeBool( this XElement element, string attributeName, bool defaultValue = false )
        {
            var attrStr = element.OptionalAttribute( attributeName );

            bool attrValue = defaultValue;

            if( ( attrStr != null ) && !bool.TryParse( attrStr, out attrValue ) )
            {
                throw new FileProcessingException( Localize( $"XML element '{element.Name}' attribute '{attributeName}' has an invalid value '{attrStr}' (expected boolean value)" ),
                                                   element.BaseUri, ( (IXmlLineInfo) element ).LineNumber );
            }

            return attrValue;
        }

        public static T OptionalAttributeEnum<T>( this XElement element, string attributeName, T defaultValue = default ) where T : struct, Enum
        {
            var attrStr = element.OptionalAttribute( attributeName );

            T attrValue = defaultValue;

            if( ( attrStr != null ) && !Enum.TryParse( attrStr, out attrValue ) )
            {
                var enumValues = string.Join( ", ", Enum.GetNames( typeof( T ) ) );
                throw new FileProcessingException( Localize( $"XML element '{element.Name}' attribute '{attributeName}' has an invalid value '{attrStr}' (expected one of: {enumValues})" ),
                                                   element.BaseUri, ( (IXmlLineInfo) element ).LineNumber );
            }

            return attrValue;
        }

        public static string MandatoryAttribute( this XElement element, IEnumerable<string> attributeNames, out string usedAttributeName )
        {
            foreach( var attributeName in attributeNames )
            {
                var attrValue = element.OptionalAttribute( attributeName );
                if( attrValue != null && attrValue.Length > 0 )
                {
                    usedAttributeName = attributeName;
                    return attrValue;
                }
            }

            var attrNameList = string.Join( "', '", attributeNames );
            throw new FileProcessingException( Localize( $"XML element '{element.Name}' lacks one of mandatory attributes ('{attrNameList}') or are empty" ),
                                               element.BaseUri, ( (IXmlLineInfo) element ).LineNumber );
        }
    }
}
