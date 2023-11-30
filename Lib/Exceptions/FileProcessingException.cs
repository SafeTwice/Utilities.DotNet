/// @file
/// @copyright  Copyright (c) 2020 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using static Utilities.Net.I18N.LibraryLocalizer;

namespace Utilities.Net.Exceptions
{
    /// <summary>
    /// Represents an error raised while processing a file.
    /// </summary>
    public class FileProcessingException : RuntimeException
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        public int Line { get; private set; }

        public string? Filename { get; private set; }

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

        public string ShortMessage
        {
            get => base.Message;
        }

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        public FileProcessingException( string message, string? filename, int line ) : base( message )
        {
            Filename = filename;
            Line = line;
        }

        public FileProcessingException( string message, string? filename, int line, Exception innerException ) : base( message, innerException )
        {
            Filename = filename;
            Line = line;
        }
    }
}
