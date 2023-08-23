/// @file
/// @copyright  Copyright (c) 2019-2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.IO;

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

        public static string SanitizeFilename( string filename )
        {
            string ret = filename;

            foreach( var invalidChar in Path.GetInvalidFileNameChars() )
            {
                ret = ret.Replace( $"{invalidChar}", "" );
            }

            return ret;
        }
    }
}
