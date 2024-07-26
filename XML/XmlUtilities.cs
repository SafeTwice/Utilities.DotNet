/// @file
/// @copyright  Copyright (c) 2020-2021 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using static Utilities.DotNet.XML.I18N.LibraryLocalizer;

#if NET6_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace Utilities.DotNet.XML
{
    /// <summary>
    /// XML utilities.
    /// </summary>
    public static class XmlUtilities
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <summary>
        /// Gets a mandatory attribute value as a <c>string</c>.
        /// </summary>
        /// <param name="element">This element.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="allowEmpty">Indicates if empty values are valid.</param>
        /// <returns>The value of the attribute.</returns>
        /// <exception cref="XmlFileProcessingException">Thrown when the attribute is not present or its value is invalid.</exception>
        public static string MandatoryAttribute( this XElement element, string attributeName, bool allowEmpty = false )
        {
            var attrStr = element.Attribute( attributeName )?.Value;

            if( attrStr == null )
            {
                throw new XmlFileProcessingException( Localize( $"XML element '{element.Name}' lacks mandatory attribute '{attributeName}'" ), element );
            }
            else if( !allowEmpty && ( attrStr.Length == 0 ) )
            {
                throw new XmlFileProcessingException( Localize( $"XML element '{element.Name}' mandatory attribute '{attributeName}' is empty" ), element );
            }

            return attrStr;
        }

        /// <summary>
        /// Gets a mandatory attribute value as an <c>int</c>.
        /// </summary>
        /// <param name="element">This element.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns>The value of the attribute.</returns>
        /// <exception cref="XmlFileProcessingException">Thrown when the attribute is not present or its value is invalid.</exception>
        public static int MandatoryAttributeInt( this XElement element, string attributeName )
        {
            string attrStr = element.MandatoryAttribute( attributeName );

            if( !int.TryParse( attrStr, out var attrValue ) )
            {
                throw new XmlFileProcessingException( Localize( $"XML element '{element.Name}' attribute '{attributeName}' has an invalid value '{attrStr}' (expected integer value)" ),
                                                      element );
            }

            return attrValue;
        }

        /// <summary>
        /// Gets a mandatory attribute value as an <c>uint</c>.
        /// </summary>
        /// <param name="element">This element.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns>The value of the attribute.</returns>
        /// <exception cref="XmlFileProcessingException">Thrown when the attribute is not present or its value is invalid.</exception>
        public static uint MandatoryAttributeUInt( this XElement element, string attributeName )
        {
            string attrStr = element.MandatoryAttribute( attributeName );

            if( !uint.TryParse( attrStr, out var attrValue ) )
            {
                throw new XmlFileProcessingException( Localize( $"XML element '{element.Name}' attribute '{attributeName}' has an invalid value '{attrStr}' (expected unsigned integer value)" ),
                                                      element );
            }

            return attrValue;
        }

        /// <summary>
        /// Gets a mandatory attribute value as a <c>double</c>.
        /// </summary>
        /// <param name="element">This element.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns>The value of the attribute.</returns>
        /// <exception cref="XmlFileProcessingException">Thrown when the attribute is not present or its value is invalid.</exception>
        public static double MandatoryAttributeDouble( this XElement element, string attributeName )
        {
            string attrStr = element.MandatoryAttribute( attributeName );

            if( !double.TryParse( attrStr, NumberStyles.Float, CultureInfo.InvariantCulture, out var attrValue ) )
            {
                throw new XmlFileProcessingException( Localize( $"XML element '{element.Name}' attribute '{attributeName}' has an invalid value '{attrStr}' (expected real number value)" ),
                                                      element );
            }

            return attrValue;
        }

        /// <summary>
        /// Gets a mandatory attribute value as a <c>bool</c>.
        /// </summary>
        /// <param name="element">This element.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns>The value of the attribute.</returns>
        /// <exception cref="XmlFileProcessingException">Thrown when the attribute is not present or its value is invalid.</exception>
        public static bool MandatoryAttributeBool( this XElement element, string attributeName )
        {
            string attrStr = element.MandatoryAttribute( attributeName );

            if( !bool.TryParse( attrStr, out var attrValue ) )
            {
                throw new XmlFileProcessingException( Localize( $"XML element '{element.Name}' attribute '{attributeName}' has an invalid value '{attrStr}' (expected boolean value)" ),
                                                      element );
            }

            return attrValue;
        }

        /// <summary>
        /// Gets a mandatory attribute value as an enum of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="element">This element.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns>The value of the attribute.</returns>
        /// <exception cref="XmlFileProcessingException">Thrown when the attribute is not present or its value is invalid.</exception>
        public static T MandatoryAttributeEnum<T>( this XElement element, string attributeName ) where T : struct, Enum
        {
            var attrStr = element.MandatoryAttribute( attributeName );

            if( !Enum.TryParse( attrStr, out T attrValue ) )
            {
                var enumValues = string.Join( ", ", Enum.GetNames( typeof( T ) ) );
                throw new XmlFileProcessingException( Localize( $"XML element '{element.Name}' attribute '{attributeName}' has an invalid value '{attrStr}' (expected one of: {enumValues})" ),
                                                      element );
            }

            return attrValue;
        }

        /// <summary>
        /// Gets an optional attribute value as a <c>string</c>.
        /// </summary>
        /// <param name="element">This element.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns>The value of the attribute, or <c>null</c> if the attribute is not present.</returns>
        public static string? OptionalAttribute( this XElement element, string attributeName )
        {
            return element.Attribute( attributeName )?.Value;
        }

        /// <summary>
        /// Gets an optional attribute value as a <c>string</c>.
        /// </summary>
        /// <param name="element">This element.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="defaultValue">Value returned when the attribute is not present.</param>
        /// <returns>The value of the attribute.</returns>
#if NET6_0_OR_GREATER
        [return: NotNullIfNotNull( nameof( defaultValue ) )]
#endif
        public static string? OptionalAttribute( this XElement element, string attributeName, string? defaultValue )
        {
            return element.Attribute( attributeName )?.Value ?? defaultValue;
        }

        /// <summary>
        /// Gets an optional attribute value as an <c>int</c>.
        /// </summary>
        /// <param name="element">This element.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="defaultValue">Value returned when the attribute is not present.</param>
        /// <returns>The value of the attribute.</returns>
        /// <exception cref="XmlFileProcessingException">Thrown when the attribute value is invalid.</exception>
        public static int OptionalAttributeInt( this XElement element, string attributeName, int defaultValue = 0 )
        {
            var attrStr = element.OptionalAttribute( attributeName );

            int attrValue = defaultValue;

            if( ( attrStr != null ) && !int.TryParse( attrStr, out attrValue ) )
            {
                throw new XmlFileProcessingException( Localize( $"XML element '{element.Name}' attribute '{attributeName}' has an invalid value '{attrStr}' (expected integer value)" ),
                                                      element );
            }

            return attrValue;
        }

        /// <summary>
        /// Gets an optional attribute value as an <c>uint</c>.
        /// </summary>
        /// <param name="element">This element.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="defaultValue">Value returned when the attribute is not present.</param>
        /// <returns>The value of the attribute.</returns>
        /// <exception cref="XmlFileProcessingException">Thrown when the attribute value is invalid.</exception>
        public static uint OptionalAttributeUInt( this XElement element, string attributeName, uint defaultValue = 0 )
        {
            var attrStr = element.OptionalAttribute( attributeName );

            uint attrValue = defaultValue;

            if( ( attrStr != null ) && !uint.TryParse( attrStr, out attrValue ) )
            {
                throw new XmlFileProcessingException( Localize( $"XML element '{element.Name}' attribute '{attributeName}' has an invalid value '{attrStr}' (expected unsigned integer value)" ),
                                                      element );
            }

            return attrValue;
        }

        /// <summary>
        /// Gets an optional attribute value as a <c>double</c>.
        /// </summary>
        /// <param name="element">This element.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="defaultValue">Value returned when the attribute is not present.</param>
        /// <returns>The value of the attribute.</returns>
        /// <exception cref="XmlFileProcessingException">Thrown when the attribute value is invalid.</exception>
        public static double OptionalAttributeDouble( this XElement element, string attributeName, double defaultValue = 0.0 )
        {
            var attrStr = element.OptionalAttribute( attributeName );

            double attrValue = defaultValue;

            if( ( attrStr != null ) && !double.TryParse( attrStr, NumberStyles.Float, CultureInfo.InvariantCulture, out attrValue ) )
            {
                throw new XmlFileProcessingException( Localize( $"XML element '{element.Name}' attribute '{attributeName}' has an invalid value '{attrStr}' (expected real number value)" ),
                                                      element );
            }

            return attrValue;
        }

        /// <summary>
        /// Gets an optional attribute value as a <c>bool</c>.
        /// </summary>
        /// <param name="element">This element.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="defaultValue">Value returned when the attribute is not present.</param>
        /// <returns>The value of the attribute.</returns>
        /// <exception cref="XmlFileProcessingException">Thrown when the attribute value is invalid.</exception>
        public static bool OptionalAttributeBool( this XElement element, string attributeName, bool defaultValue = false )
        {
            var attrStr = element.OptionalAttribute( attributeName );

            bool attrValue = defaultValue;

            if( ( attrStr != null ) && !bool.TryParse( attrStr, out attrValue ) )
            {
                throw new XmlFileProcessingException( Localize( $"XML element '{element.Name}' attribute '{attributeName}' has an invalid value '{attrStr}' (expected boolean value)" ),
                                                      element );
            }

            return attrValue;
        }

        /// <summary>
        /// Gets an optional attribute value as an enum of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="element">This element.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="defaultValue">Value returned when the attribute is not present.</param>
        /// <returns>The value of the attribute.</returns>
        /// <exception cref="XmlFileProcessingException">Thrown when the attribute value is invalid.</exception>
        public static T OptionalAttributeEnum<T>( this XElement element, string attributeName, T defaultValue = default ) where T : struct, Enum
        {
            var attrStr = element.OptionalAttribute( attributeName );

            T attrValue = defaultValue;

            if( ( attrStr != null ) && !Enum.TryParse( attrStr, out attrValue ) )
            {
                var enumValues = string.Join( ", ", Enum.GetNames( typeof( T ) ) );
                throw new XmlFileProcessingException( Localize( $"XML element '{element.Name}' attribute '{attributeName}' has an invalid value '{attrStr}' (expected one of: {enumValues})" ),
                                                      element );
            }

            return attrValue;
        }

        /// <summary>
        /// Gets a mandatory attribute value from a set of attributes as a <c>string</c>.
        /// </summary>
        /// <param name="element">This element.</param>
        /// <param name="attributeNames">Names of the attributes to search (in search order).</param>
        /// <param name="usedAttributeName">Name of the attribute that was found.</param>
        /// <returns>The value of the found attribute.</returns>
        /// <exception cref="XmlFileProcessingException">Thrown when the none of the attributes is present.</exception>
        public static string MandatoryAttribute( this XElement element, IEnumerable<string> attributeNames, out string usedAttributeName )
        {
            foreach( var attributeName in attributeNames )
            {
                var attrValue = element.OptionalAttribute( attributeName );
                if( ( attrValue != null ) && ( attrValue.Length > 0 ) )
                {
                    usedAttributeName = attributeName;
                    return attrValue;
                }
            }

            var attrNameList = string.Join( "', '", attributeNames );
            throw new XmlFileProcessingException( Localize( $"XML element '{element.Name}' lacks one of mandatory attributes ('{attrNameList}') or are empty" ), element );
        }

        /// <summary>
        /// Gets a mandatory element that must be unique.
        /// </summary>
        /// <param name="element">This element.</param>
        /// <param name="elementName">Name of the element.</param>
        /// <returns>An element.</returns>
        /// <exception cref="XmlFileProcessingException">Thrown when the number of matching elements is different from 1.</exception>
        public static XElement MandatoryUniqueElement( this XElement element, string elementName )
        {
            var childElement = element.OptionalUniqueElement( elementName );

            if( childElement == null )
            {
                throw new XmlFileProcessingException( Localize( $"XML element '{element.Name}' lacks mandatory child element '{elementName}'" ), element );
            }

            return childElement;
        }

        /// <summary>
        /// Gets the contents of a mandatory element (that must be unique) as a <c>string</c>.
        /// </summary>
        /// <param name="element">This element.</param>
        /// <param name="elementName">Name of the element.</param>
        /// <param name="allowEmpty">Indicates if empty values are valid.</param>
        /// <returns>The value of the contents of the element.</returns>
        /// <exception cref="XmlFileProcessingException">Thrown when the number of matching elements is different from 1,
        ///                                              or when the present element value is invalid.</exception>
        public static string MandatoryUniqueElementText( this XElement element, string elementName, bool allowEmpty = false )
        {
            var childElement = element.MandatoryUniqueElement( elementName );

            if( childElement.HasElements )
            {
                throw new XmlFileProcessingException( Localize( $"XML element '{elementName}' has child elements" ), childElement );
            }

            var value = childElement.Value;

            if( !allowEmpty && ( value.Length == 0 ) )
            {
                throw new XmlFileProcessingException( Localize( $"XML element '{elementName}' is empty" ), childElement );
            }

            return value;
        }

        /// <summary>
        /// Gets an optional element that must be unique.
        /// </summary>
        /// <param name="element">This element.</param>
        /// <param name="elementName">Name of the element.</param>
        /// <returns>An element, or <c>null</c> if the element is not present.</returns>
        /// <exception cref="XmlFileProcessingException">Thrown when the number of matching elements is higher than 1.</exception>
        public static XElement? OptionalUniqueElement( this XElement element, string elementName )
        {
            var elements = element.Elements( elementName );

            if( elements.Count() > 1 )
            {
                var childElement = elements.First();
                throw new XmlFileProcessingException( Localize( $"XML element '{element.Name}' has more than 1 child element '{childElement.Name}'" ), childElement );
            }
            else if( elements.Count() == 1 )
            {
                return elements.First();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the contents of an optional element (that must be unique) as a <c>string</c>.
        /// </summary>
        /// <param name="element">This element.</param>
        /// <param name="elementName">Name of the element.</param>
        /// <param name="defaultValue">Value returned when the element is not present.</param>
        /// <returns>The value of the element.</returns>
        /// <exception cref="XmlFileProcessingException">Thrown when the number of matching elements is higher than 1,
        ///                                              or when the element is not present or its value is invalid.</exception>
#if NET6_0_OR_GREATER
        [return: NotNullIfNotNull( nameof( defaultValue ) )]
#endif
        public static string? OptionalUniqueElementText( this XElement element, string elementName, string? defaultValue = null )
        {
            var childElement = element.OptionalUniqueElement( elementName );

            if( childElement == null )
            {
                return defaultValue;
            }
            
            if( childElement.HasElements )
            {
                throw new XmlFileProcessingException( Localize( $"XML element '{elementName}' has child elements" ), childElement );
            }

            return childElement.Value;
        }

        /// <summary>
        /// Adds the specified <paramref name="node"/> as child of this <paramref name="container"/> if it not already added.
        /// </summary>
        /// <remarks>
        /// If the node already has a parent (different from this container), then it is removed from its previous parent before being added to the new parent.
        /// </remarks>
        /// <param name="container">This container.</param>
        /// <param name="node">Node to be added.</param>
        public static void AddUnique( this XContainer container, XNode node )
        {
            if( ( node.Parent != null ) && ( node.Parent != container ) )
            {
                node.Remove();
            }

            if( node.Parent == null )
            {
                container.Add( node );
            }
        }
    }
}
