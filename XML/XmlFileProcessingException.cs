/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Xml;
using System.Xml.Linq;
using Utilities.DotNet.Exceptions;

namespace Utilities.DotNet.XML
{
    /// <summary>
    /// Represents an error raised while processing an XML file.
    /// </summary>
    public class XmlFileProcessingException : FileProcessingException
    {
        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Message of the exception.</param>
        /// <param name="element">XML element where the exception was triggered.</param>
        public XmlFileProcessingException( string message, XElement element )
            : base( message, element.BaseUri, ( (IXmlLineInfo) element ).LineNumber )
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Message of the exception.</param>
        /// <param name="element">XML element where the exception was triggered.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public XmlFileProcessingException( string message, XElement element, Exception innerException )
            : base( message, element.BaseUri, ( (IXmlLineInfo) element ).LineNumber, innerException )
        {
        }
    }
}
