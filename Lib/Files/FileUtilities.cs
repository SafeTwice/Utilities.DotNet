/// @file
/// @copyright  Copyright (c) 2019-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.IO;
using System.Linq;

namespace Utilities.DotNet.Files
{
    /// <summary>
    /// File utilities.
    /// </summary>
    public static class FileUtilities
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <summary>
        /// Creates the directory and subdirectories where the file would be stored if they don't exist.
        /// </summary>
        /// <param name="path">Path to a file.</param>
        /// <exception cref="ArgumentException">Thrown when the path does not have directory information</exception>
        public static void EnsureFilePathIsAvailable( string path )
        {
            var dirName = Path.GetDirectoryName( path );
            if( dirName?.Length > 0 )
            {
                Directory.CreateDirectory( dirName );
            }
            else
            {
                throw new ArgumentException( "The specified path does not have directory information", nameof( path ) );
            }
        }

        /// <summary>
        /// Sanitizes the given filename, replacing characters invalid for file names with the given replacement character.
        /// </summary>
        /// <param name="filename">Name of a file.</param>
        /// <param name="replacement">Character to use to replace invalid file name characters, or <c>null</c> to delete them.</param>
        /// <returns>Sanitized filename.</returns>
        /// <exception cref="ArgumentException">Thrown when the replacement character is invalid for file names</exception>
        public static string SanitizeFilename( string filename, char? replacement = null )
        {
            string ret = filename;

            char[] invalidChars = Path.GetInvalidFileNameChars();

            string replacementStr = string.Empty;

            if( replacement != null )
            {
                if( invalidChars.Contains( replacement.Value ) )
                {
                    throw new ArgumentException( "Invalid replacement character", nameof( replacement ) );
                }

                replacementStr = $"{replacement}";
            }

            foreach( var invalidChar in invalidChars )
            {
                ret = ret.Replace( $"{invalidChar}", replacementStr );
            }

            return ret;
        }
    }
}
