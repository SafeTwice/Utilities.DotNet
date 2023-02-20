/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using Utilities.Net.Logs;
using Xunit;

namespace Utilities.Net.Test.Logs
{
    public class FileLogTest : IDisposable
    {
        [Flags]
        public enum ELogEntryType
        {
            NONE = 0,
            ONE = 0x01,
            TWO = 0x02,
        }

        private string m_filename = Path.GetTempPath() + $"/{typeof(FileLogTest).FullName}-{DateTime.Now:yyyy-MM-dd-HH-mm-ss-fff}";

        public void Dispose()
        {
            if( File.Exists( m_filename ) )
            {
                File.Delete( m_filename );
            }
        }

        private string ReadFileContents( string filename )
        {
            using var fileStream = new FileStream( filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite );
            using var reader = new StreamReader( fileStream );

            return reader.ReadToEnd();
        }

        [Theory]
        [InlineData( ELogEntryType.NONE, false, false )]
        [InlineData( ELogEntryType.ONE, true, false )]
        [InlineData( ELogEntryType.TWO, false, true )]
        [InlineData( ELogEntryType.ONE | ELogEntryType.TWO, true, true )]
        public void EntryTypesEnabling( ELogEntryType enabledEntryTypes, bool oneValue, bool twoValue )
        {
            // Prepare

            using var fileLog = new FileLog<ELogEntryType>();

            // Execute

            fileLog.EnabledEntryTypes = enabledEntryTypes;

            // CHeck

            Assert.Equal( enabledEntryTypes, fileLog.EnabledEntryTypes );
            Assert.Equal( oneValue, fileLog.IsEntryTypeEnabled( ELogEntryType.ONE ) );
            Assert.Equal( twoValue, fileLog.IsEntryTypeEnabled( ELogEntryType.TWO ) );
        }

        [Fact]
        public void Constructor_OpenFile_EntryTypesEnabled()
        {
            // Execute

            using var fileLog = new FileLog<ELogEntryType>( m_filename, ELogEntryType.TWO );

            // Check

            Assert.True( File.Exists( m_filename ) );

            var fileContents = ReadFileContents( m_filename );

            Assert.Equal( "TIME,TYPE,CATEGORY,MESSAGE" + Environment.NewLine, fileContents );

            // Cleanup

            fileLog.Close();
        }

        [Fact]
        public void Constructor_OpenFile_AllEntryTypesDisabled()
        {
            // Execute

            using var fileLog = new FileLog<ELogEntryType>( m_filename );

            // Check

            Assert.False( File.Exists( m_filename ) );

            // Cleanup

            fileLog.Close();
        }

        [Fact]
        public void Open_EntryTypesEnabled()
        {
            // Prepare

            using var fileLog = new FileLog<ELogEntryType>();
            fileLog.EnabledEntryTypes = ELogEntryType.TWO;

            // Execute

            fileLog.Open( m_filename );

            // Check

            Assert.True( File.Exists( m_filename ) );

            var fileContents = ReadFileContents( m_filename );

            Assert.Equal( "TIME,TYPE,CATEGORY,MESSAGE" + Environment.NewLine, fileContents );

            // Cleanup

            fileLog.Close();
        }

        [Fact]
        public void Open_AllEntryTypesDisabled()
        {
            // Prepare

            using var fileLog = new FileLog<ELogEntryType>();

            // Execute

            fileLog.Open( m_filename );

            // Check

            Assert.False( File.Exists( m_filename ) );

            // Cleanup

            fileLog.Close();
        }

        [Fact]
        public void AddLogEntry_StringMessage_EntryTypeEnabled()
        {
            // Prepare

            using var fileLog = new FileLog<ELogEntryType>( m_filename, ELogEntryType.ONE );

            // Execute

            var currentTime = DateTime.Now;
            fileLog.AddLogEntry( ELogEntryType.ONE, "Category X", "Message Y" );

            // Check

            Assert.True( File.Exists( m_filename ) );

            var fileContents = ReadFileContents( m_filename );

            var matches = Regex.Match( fileContents, "^TIME,TYPE,CATEGORY,MESSAGE" + Environment.NewLine +
                            "(\\d{4}-\\d{2}-\\d{2} \\d{2}:\\d{2}:\\d{2}\\.\\d{3}),ONE,Category X,Message Y" + Environment.NewLine + "$" );

            Assert.True( matches.Success );

            var loggedTime = DateTime.ParseExact( matches.Groups[ 1 ].Value, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture );

            Assert.True( ( loggedTime - currentTime ).TotalMilliseconds < 1000 );

            // Cleanup

            fileLog.Close();
        }

        [Fact]
        public void AddLogEntry_StringMessage_EntryTypeDisabled()
        {
            // Prepare

            using var fileLog = new FileLog<ELogEntryType>( m_filename, ELogEntryType.ONE );

            // Execute

            fileLog.AddLogEntry( ELogEntryType.TWO, "Category X", "Message Y" );

            // Check

            Assert.True( File.Exists( m_filename ) );

            var fileContents = ReadFileContents( m_filename );

            Assert.Equal( "TIME,TYPE,CATEGORY,MESSAGE" + Environment.NewLine, fileContents );

            // Cleanup

            fileLog.Close();
        }

        [Fact]
        public void AddLogEntry_StringMessage_FileClosed()
        {
            // Prepare

            using var fileLog = new FileLog<ELogEntryType>();
            fileLog.EnabledEntryTypes = ELogEntryType.ONE;

            // Execute

            fileLog.AddLogEntry( ELogEntryType.ONE, "Category X", "Message Y" );

            // Check

            Assert.False( File.Exists( m_filename ) );

            // Cleanup

            fileLog.Close();
        }

        [Fact]
        public void AddLogEntry_NoMessage()
        {
            // Prepare

            using var fileLog = new FileLog<ELogEntryType>( m_filename, ELogEntryType.ONE );

            // Execute

            var currentTime = DateTime.Now;
            fileLog.AddLogEntry( ELogEntryType.ONE, "Category X" );

            // Check

            Assert.True( File.Exists( m_filename ) );

            var fileContents = ReadFileContents( m_filename );

            var matches = Regex.Match( fileContents, "^TIME,TYPE,CATEGORY,MESSAGE" + Environment.NewLine +
                            "(\\d{4}-\\d{2}-\\d{2} \\d{2}:\\d{2}:\\d{2}\\.\\d{3}),ONE,Category X," + Environment.NewLine + "$" );

            Assert.True( matches.Success );

            var loggedTime = DateTime.ParseExact( matches.Groups[ 1 ].Value, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture );

            Assert.True( ( loggedTime - currentTime ).TotalMilliseconds < 1000 );

            // Cleanup

            fileLog.Close();
        }

        [Fact]
        public void AddLogEntry_ActionMessage_EntryTypeEnabled()
        {
            // Prepare

            using var fileLog = new FileLog<ELogEntryType>( m_filename, ELogEntryType.ONE );

            int i = 33;

            // Execute

            var currentTime = DateTime.Now;
            fileLog.AddLogEntry( ELogEntryType.ONE, "Category X", () => $"Message {i++}" );

            // Check

            Assert.True( File.Exists( m_filename ) );

            var fileContents = ReadFileContents( m_filename );

            var matches = Regex.Match( fileContents, "^TIME,TYPE,CATEGORY,MESSAGE" + Environment.NewLine +
                            "(\\d{4}-\\d{2}-\\d{2} \\d{2}:\\d{2}:\\d{2}\\.\\d{3}),ONE,Category X,Message 33" + Environment.NewLine + "$" );

            Assert.True( matches.Success );

            var loggedTime = DateTime.ParseExact( matches.Groups[ 1 ].Value, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture );

            Assert.True( ( loggedTime - currentTime ).TotalMilliseconds < 1000 );

            Assert.Equal( 34, i );

            // Cleanup

            fileLog.Close();
        }

        [Fact]
        public void AddLogEntry_ActionMessage_EntryTypeDisabled()
        {
            // Prepare

            using var fileLog = new FileLog<ELogEntryType>( m_filename, ELogEntryType.ONE );

            int i = 33;

            // Execute

            fileLog.AddLogEntry( ELogEntryType.TWO, "Category X", () => $"Message {i++}" );

            // Check

            Assert.True( File.Exists( m_filename ) );

            var fileContents = ReadFileContents( m_filename );

            Assert.Equal( "TIME,TYPE,CATEGORY,MESSAGE" + Environment.NewLine, fileContents );

            Assert.Equal( 33, i );

            // Cleanup

            fileLog.Close();
        }

        [Fact]
        public void AddLogEntry_ActionMessage_FileClosed()
        {
            // Prepare

            using var fileLog = new FileLog<ELogEntryType>();
            fileLog.EnabledEntryTypes = ELogEntryType.ONE;

            int i = 33;

            // Execute

            fileLog.AddLogEntry( ELogEntryType.ONE, "Category X", () => $"Message {i++}" );

            // Check

            Assert.False( File.Exists( m_filename ) );

            Assert.Equal( 33, i );

            // Cleanup

            fileLog.Close();
        }
    }
}
