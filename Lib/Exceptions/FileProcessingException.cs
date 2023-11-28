/// @file
/// @copyright  Copyright (c) 2020-2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using static Utilities.DotNet.I18N.LibraryLocalizer;

namespace Utilities.DotNet.Exceptions
{
    /// <summary>
    /// Represents an error raised while processing a file.
    /// </summary>
    public class FileProcessingException : RuntimeException
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <value>
        /// Line where the exception was triggered (<c>0</c> if does not apply).
        /// </value>
        public int Line { get; }

        /// <value>
        /// Name of the file where the exception was triggered (<c>null</c> if not specified or implicit).
        /// </value>
        public string? Filename { get; }

        public override string Message
        {
            get
            {
                string header = string.Empty;
                if( Filename?.Length > 0 )
                {
                    if( Line > 0 )
                    {
                        return Localize( $"[{Filename} @ Line {Line}] {ShortMessage}" );
                    }
                    else
                    {
                        return Localize( $"[{Filename}] {ShortMessage}" );
                    }
                }
                else
                {
                    if( Line > 0 )
                    {
                        return Localize( $"[Line {Line}] {ShortMessage}" );
                    }
                    else
                    {
                        return ShortMessage;
                    }
                }
            }
        }

        /// <value>
        /// Message that describes the current exception without file or line adornments.
        /// </value>
        public string ShortMessage
        {
            get => base.Message;
        }

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Message of the exception</param>
        /// <param name="filename">Name of the file that triggered the exception</param>
        /// <param name="line">Line where the exception was triggered (0 if does not apply)</param>
        public FileProcessingException( string message, string filename, int line ) : base( message )
        {
            Filename = filename;
            Line = line;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Message of the exception</param>
        /// <param name="line">Line where the exception was triggered (0 if does not apply)</param>
        public FileProcessingException( string message, int line ) : base( message )
        {
            Line = line;
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Message of the exception</param>
        /// <param name="filename">Name of the file that triggered the exception</param>
        /// <param name="line">Line where the exception was triggered (0 if does not apply)</param>
        /// <param name="innerException">The exception that is the cause of the current exception</param>
        public FileProcessingException( string message, string filename, int line, Exception innerException ) : base( message, innerException )
        {
            Filename = filename;
            Line = line;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Message of the exception</param>
        /// <param name="line">Line where the exception was triggered (0 if does not apply)</param>
        /// <param name="innerException">The exception that is the cause of the current exception</param>
        public FileProcessingException( string message, int line, Exception innerException ) : base( message, innerException )
        {
            Line = line;
        }
    }
}
