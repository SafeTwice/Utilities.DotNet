/// @file
/// @copyright  Copyright (c) 2023-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.IO;
using Utilities.DotNet.Files;

namespace Utilities.DotNet.Logs
{
    /// <summary>
    /// Log that writes log entries to a file.
    /// </summary>
    /// <remarks>
    /// The type parameter <typeparamref name="TLogEntryType"/> must be a bit-field enum (i.e., declared using the <see cref="System.FlagsAttribute">Flags</see> attribute).
    /// </remarks>
    /// <typeparam name="TLogEntryType">Type of log entries.</typeparam>
    public class FileLog<TLogEntryType> : ILog<TLogEntryType>, IDisposable where TLogEntryType : struct, Enum
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <inheritdoc/>
        public TLogEntryType EnabledEntryTypes
        {
            get => m_enabledEntryTypes;
            set
            {
                m_enabledEntryTypes = value;

                TryOpenFile();
            }
        }

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Default constructor.
        /// </summary>
        public FileLog()
        {
        }

        /// <summary>
        /// Constructor that sets the enabled entry types and opens an output file (see <see cref="Open(string)"/>).
        /// </summary>
        /// <param name="filename">File path for the log output file.</param>
        /// <param name="enabledEntryTypes">Type of entries to enable.</param>
        public FileLog( string filename, TLogEntryType enabledEntryTypes = default )
        {
            m_enabledEntryTypes = enabledEntryTypes;

            Open( filename );
        }

        //===========================================================================
        //                               FINALIZER
        //===========================================================================

        ~FileLog()
        {
            Dispose();
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <inheritdoc/>
        public bool IsEntryTypeEnabled( TLogEntryType entryType )
        {
            return m_enabledEntryTypes.HasFlag( entryType );
        }

        /// <summary>
        /// Open an output file to write logged entries to.
        /// </summary>
        /// <remarks>
        /// If <see cref="EnabledEntryTypes"/> has its default value (i.e., all log entry types disabled) then
        /// file opening is delayed until at least one log entry type is enabled.
        /// </remarks>
        /// <param name="filename">File path for the log output file.</param>
        public void Open( string filename )
        {
            Close();

            m_filename = filename;

            TryOpenFile();
        }

        /// <summary>
        /// Closes the log output file.
        /// </summary>
        public void Close()
        {
            lock( m_fileLock )
            {
                m_fileWriter?.Close();
                m_fileStream?.Close();

                m_fileWriter?.Dispose();
                m_fileStream?.Dispose();

                m_fileWriter = null;
                m_fileStream = null;
                m_filename = null;
            }
        }

        /// <inheritdoc/>
        public void AddLogEntry( TLogEntryType entryType, string category, string? message = null )
        {
            if( !IsWriteEntryEnabled( entryType ) )
            {
                return;
            }

            WriteLogEntry( entryType, category, message ?? string.Empty );
        }

        /// <inheritdoc/>
        public void AddLogEntry( TLogEntryType entryType, string category, Func<string> messageGenerator )
        {
            if( !IsWriteEntryEnabled( entryType ) )
            {
                return;
            }

            string message = messageGenerator();

            WriteLogEntry( entryType, category, message );

        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Close();
        }

        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        private bool IsWriteEntryEnabled( TLogEntryType entryType )
        {
            if( ( m_fileStream == null ) || ( m_fileWriter == null ) )
            {
                return false;
            }

            return IsEntryTypeEnabled( entryType );
        }

        private void WriteLogEntry( TLogEntryType logEntryType, string category, string message )
        {
            var dateTime = DateTime.Now.ToString( "yyyy-MM-dd HH:mm:ss.fff" );

            lock( m_fileLock )
            {
                m_fileWriter!.Write( dateTime );
                m_fileWriter.Write( ',' );
                m_fileWriter.Write( logEntryType.ToString() );
                m_fileWriter.Write( "," );
                m_fileWriter.Write( EscapeMessage( category ) );
                m_fileWriter.Write( "," );
                m_fileWriter.WriteLine( EscapeMessage( message ) );
                m_fileWriter.Flush();

                m_fileStream!.Flush( true );
            }
        }

        private string EscapeMessage( string message )
        {
            if( message.Contains( "," ) )
            {
                return "\"" + message.Replace( "\"", "\"\"" ) + "\"";
            }
            else
            {
                return message;
            }
        }

        private void TryOpenFile()
        {
            if( ( m_filename == null ) || ( m_fileStream != null ) || ( m_fileWriter != null ) || m_enabledEntryTypes.Equals( default( TLogEntryType ) ) )
            {
                return;
            }

            FileUtilities.EnsureFilePathIsAvailable( m_filename );

            lock( m_fileLock )
            {
                m_fileStream = new FileStream( m_filename, FileMode.Append, FileAccess.Write, FileShare.Read );
                m_fileWriter = new StreamWriter( m_fileStream );

                m_fileWriter.WriteLine( "TIME,TYPE,CATEGORY,MESSAGE" );
                m_fileWriter.Flush();
                m_fileStream.Flush( true );
            }
        }

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private FileStream? m_fileStream;
        private StreamWriter? m_fileWriter;
        private object m_fileLock = new();

        private string? m_filename;
        private TLogEntryType m_enabledEntryTypes = default;
    }
}
