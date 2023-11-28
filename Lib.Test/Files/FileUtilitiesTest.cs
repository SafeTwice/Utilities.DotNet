/// @file
/// @copyright  Copyright (c) 2019-2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.IO;
using Utilities.DotNet.Files;
using Xunit;

namespace Utilities.DotNet.Test.Files
{
    public class FileUtilitiesTest
    {
        [Fact]
        public void EnsureDirectoryExists()
        {
            // Arrange

            var testDirPath = Path.GetTempPath() + $"/{nameof( FileUtilitiesTest )}";

            if( Directory.Exists( testDirPath ) )
            {
                Directory.Delete( testDirPath, true );
            }

            // Act

            FileUtilities.EnsureDirectoryExists( testDirPath );

            // Assert

            Assert.True( Directory.Exists( testDirPath ) );

            // Cleanup

            Directory.Delete( testDirPath, true );
        }

        [Fact]
        public void EnsureFilePathIsAvailable()
        {
            // Arrange

            var testDirPath = Path.GetTempPath() + $"/{nameof( FileUtilitiesTest )}";
            var tempFilePath = testDirPath + $"/{nameof( FileUtilitiesTest )}.txt";

            if( Directory.Exists( testDirPath ) )
            {
                Directory.Delete( testDirPath, true );
            }

            // Act

            FileUtilities.EnsureFilePathIsAvailable( tempFilePath );

            // Assert

            Assert.True( Directory.Exists( testDirPath ) );

            // Cleanup

            Directory.Delete( testDirPath, true );
        }

        [Fact]
        public void EnsureFilePathIsAvailable_Exception()
        {
            // Act

            var exception = Assert.Throws<ArgumentException>( () => FileUtilities.EnsureFilePathIsAvailable( "temp.txt" ) );

            // Assert

            Assert.Equal( "The specified path does not have directory information", exception.Message );
        }

        [Fact]
        public void SanitizeFilename_DeleteInvalidCharacters()
        {
            // Act

            var result = FileUtilities.SanitizeFilename( "F|i\\l/e:name@$" );

            // Assert

            Assert.Equal( "Filename@$", result );
        }

        [Fact]
        public void SanitizeFilename_ReplaceInvalidCharacters()
        {
            // Act

            var result = FileUtilities.SanitizeFilename( "F|i\\l/e:name@$", '-' );

            // Assert

            Assert.Equal( "F-i-l-e-name@$", result );
        }

        [Fact]
        public void SanitizeFilename_InvalidReplacementCharacter()
        {
            // Act

            var exception = Assert.Throws<Exception>( () =>
            {
                var result = FileUtilities.SanitizeFilename( "F|i\\l/e:name@$", '|' );
            } );

            // Assert

            Assert.Equal( "Invalid replacement character", exception.Message );
        }
    }
}
