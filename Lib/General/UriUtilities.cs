/// @file
/// @copyright  Copyright (c) 2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Text.RegularExpressions;

#if NET6_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace Utilities.DotNet
{
    /// <summary>
    /// <see cref="Uri"/> utilities.
    /// </summary>
    public static partial class UriUtilities
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <summary>
        /// Parses a URI string.
        /// </summary>
        /// <param name="uriString">URI string to be parsed.</param>
        /// <param name="defaultScheme">Scheme to use if <paramref name="uriString"/> does not define one
        ///                             ('https' when <see langword="null"/>).</param>
        /// <param name="defaultPort">Port to use if <paramref name="uriString"/> does not define one
        ///                             (will use the default port for the scheme when set to -1).</param>
        /// <returns>The parsed URI.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="uriString"/> is not a valid URI.</exception>
        public static Uri ParseUri( this string uriString, string? defaultScheme = null, int defaultPort = -1 )
        {
            if( !TryParseUri( uriString, out var result, defaultScheme, defaultPort ) )
            {
                throw new ArgumentException( "Invalid URI", nameof( uriString ) );
            }

            return result!;
        }

        /// <summary>
        /// Tries to parse a URI string.
        /// </summary>
        /// <param name="uriString">URI string to be parsed.</param>
        /// <param name="result">The parsed URI (or <see langword="null"/> if the URI could not be parsed).</param>
        /// <param name="defaultScheme">Scheme to use if <paramref name="uriString"/> does not define one
        ///                             ('https' when <see langword="null"/>).</param>
        /// <param name="defaultPort">Port to use if <paramref name="uriString"/> does not define one
        ///                             (will use the default port for the scheme when set to -1).</param>
        /// <returns><see langword="true"/> if the URI was parsed successfully; <see langword="false"/> otherwise.</returns>
        public static bool TryParseUri( this string uriString,
#if NET6_0_OR_GREATER
            [NotNullWhen( true )]
#endif
            out Uri? result,
            string? defaultScheme = null, int defaultPort = -1 )
        {
            var match = UrlRegex().Match( uriString );
            if( !match.Success )
            {
                result = null;
                return false;
            }

            var schemeGroup = match.Groups[ 1 ];
            var portGroup = match.Groups[ 3 ];

            var scheme = schemeGroup.Success ? schemeGroup.Value : ( defaultScheme ?? Uri.UriSchemeHttps );
            var host = match.Groups[ 2 ].Value;
            var port = portGroup.Success ? int.Parse( portGroup.Value ) : defaultPort;
            var path = match.Groups[ 4 ].Value;

            result = new UriBuilder( scheme, host, port, path ).Uri;
            return true;
        }

        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        private const string URL_REGEX_PATTERN = @"^(?:([^:]*)://)??([^:/]+)(?::(\d{1,5}))?(?:/(.*))?$";

#if NET7_0_OR_GREATER
        [GeneratedRegex( URL_REGEX_PATTERN )]
        private static partial Regex UrlRegex();
#else
        private static Regex UrlRegex() => URL_REGEX;

        private static readonly Regex URL_REGEX = new( URL_REGEX_PATTERN );
#endif
    }
}
