﻿/// @file
/// @copyright  Copyright (c) 2019-2023 SafeTwice S.L. All rights reserved.
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

        public static void EnsureDirectoryExists( string dirPath )
        {
            if( !Directory.Exists( dirPath ) )
            {
                Directory.CreateDirectory( dirPath );
            }
        }

        public static void EnsureFilePathIsAvailable( string path )
        {
            var dirName = Path.GetDirectoryName( path );
            if( dirName?.Length > 0 )
            {
                EnsureDirectoryExists( dirName );
            }
            else
            {
                throw new ArgumentException( "The specified path does not have directory information" );
            }
        }

        /// <summary>
        /// Sanitizes the given filename, replacing charactars invalid for file names with the given replacement character.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="replacement">Character to use to replace invalid file name characters, or <c>null</c> to delete them</param>
        /// <returns>Sanitized filename</returns>
        /// <exception cref="Exception">Thrown when the replacement character is invalid for file names</exception>
        public static string SanitizeFilename( string filename, char? replacement = null )
        {
            string ret = filename;

            char[] invalidChars = Path.GetInvalidFileNameChars();

            string replacementStr = string.Empty;

            if( replacement != null )
            {
                if( invalidChars.Contains( replacement.Value ) )
                {
                    throw new Exception( "Invalid replacement character" );
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
