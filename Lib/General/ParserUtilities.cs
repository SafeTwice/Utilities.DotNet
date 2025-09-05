using System;
using System.Globalization;

namespace Utilities.DotNet
{
    /// <summary>
    /// Option flags for parsing signed integers from strings.
    /// </summary>
    [Flags]
    public enum EIntParseOptions
    {
        /// <summary>
        /// Indicates that no style elements, such as leading or trailing white space, 
        /// or thousands separators can be present in the parsed string.
        /// The string to be parsed must consist of integral decimal digits only.
        /// </summary>
        None = NumberStyles.None,

        /// <inheritdoc cref="NumberStyles.AllowLeadingWhite"/>
        AllowLeadingWhite = NumberStyles.AllowLeadingWhite,

        /// <inheritdoc cref="NumberStyles.AllowTrailingWhite"/>
        AllowTrailingWhite = NumberStyles.AllowTrailingWhite,

        /// <inheritdoc cref="NumberStyles.AllowLeadingSign"/>
        AllowLeadingSign = NumberStyles.AllowLeadingSign,

        /// <inheritdoc cref="NumberStyles.AllowTrailingSign"/>
        AllowTrailingSign = NumberStyles.AllowTrailingSign,

        /// <inheritdoc cref="NumberStyles.AllowParentheses"/>
        AllowParentheses = NumberStyles.AllowParentheses,

        /// <inheritdoc cref="NumberStyles.AllowThousands"/>
        AllowThousands = NumberStyles.AllowThousands,

        /// <inheritdoc cref="NumberStyles.AllowCurrencySymbol"/>
        AllowCurrencySymbol = NumberStyles.AllowCurrencySymbol,

        /// <inheritdoc cref="NumberStyles.AllowHexSpecifier"/>
        AllowHexSpecifier = NumberStyles.AllowHexSpecifier,

        /// <inheritdoc cref="NumberStyles.HexNumber"/>
        HexNumber = NumberStyles.HexNumber,

#if NET8_0_OR_GREATER
        /// <inheritdoc cref="NumberStyles.AllowBinarySpecifier"/>
        AllowBinarySpecifier = NumberStyles.AllowBinarySpecifier,

        /// <inheritdoc cref="NumberStyles.BinaryNumber"/>
        BinaryNumber = NumberStyles.BinaryNumber,
#endif

        /// <summary>
        /// Indicates that the string can contain a prefix indicating if the number 
        /// has to be interpreted as hexadecimal or binary.
        /// </summary>
        /// <remarks>
        /// Prefixes for hexadecimal numbers are "0x", "0X". Prefixes for binary numbers are "0b", "0B".
        /// </remarks>
        AllowPrefix = 0x00010000,

        /// <summary>
        /// Default options: allows leading and trailing white space, and a leading sign.
        /// </summary>
        Default = AllowLeadingWhite | AllowTrailingWhite | AllowLeadingSign,

        /// <summary>
        /// Allows leading and trailing white spaces, a leading sign, 
        /// and a prefix indicating if the number has to be interpreted as hexadecimal or binary.
        /// </summary>
        AnyNumber = Default | AllowPrefix,
    }

    /// <summary>
    /// Option flags for parsing unsigned integers from strings.
    /// </summary>
    [Flags]
    public enum EUIntParseOptions
    {
        /// <summary>
        /// Indicates that no style elements, such as leading or trailing white space, 
        /// or thousands separators can be present in the parsed string.
        /// The string to be parsed must consist of integral decimal digits only.
        /// </summary>
        None = NumberStyles.None,

        /// <inheritdoc cref="NumberStyles.AllowLeadingWhite"/>
        AllowLeadingWhite = NumberStyles.AllowLeadingWhite,

        /// <inheritdoc cref="NumberStyles.AllowTrailingWhite"/>
        AllowTrailingWhite = NumberStyles.AllowTrailingWhite,

        /// <inheritdoc cref="NumberStyles.AllowThousands"/>
        AllowThousands = NumberStyles.AllowThousands,

        /// <inheritdoc cref="NumberStyles.AllowCurrencySymbol"/>
        AllowCurrencySymbol = NumberStyles.AllowCurrencySymbol,

        /// <inheritdoc cref="NumberStyles.AllowHexSpecifier"/>
        AllowHexSpecifier = NumberStyles.AllowHexSpecifier,

        /// <inheritdoc cref="NumberStyles.HexNumber"/>
        HexNumber = NumberStyles.HexNumber,

#if NET8_0_OR_GREATER
        /// <inheritdoc cref="NumberStyles.AllowBinarySpecifier"/>
        AllowBinarySpecifier = NumberStyles.AllowBinarySpecifier,

        /// <inheritdoc cref="NumberStyles.BinaryNumber"/>
        BinaryNumber = NumberStyles.BinaryNumber,
#endif

        /// <summary>
        /// Indicates that the string can contain a prefix indicating if the number
        /// has to be interpreted as hexadecimal or binary.
        /// </summary>
        /// <remarks>
        /// Prefixes for hexadecimal numbers are "0x", "0X". Prefixes for binary numbers are "0b", "0B".
        /// </remarks>
        AllowPrefix = 0x00010000,

        /// <summary>
        /// Default options: allows leading and trailing white spaces.
        /// </summary>
        Default = AllowLeadingWhite | AllowTrailingWhite,

        /// <summary>
        /// Allows leading and trailing white spaces, 
        /// and a prefix indicating if the number has to be interpreted as hexadecimal or binary.
        /// </summary>
        AnyNumber = Default | AllowPrefix,
    }

    /// <summary>
    /// Utilities to parse <see cref="String"/>s.
    /// </summary>
    public static partial class ParserUtilities
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        //***************************************************************************
        //                               Int32
        //***************************************************************************

        /// <summary>
        /// Converts the string representation of a number to its 32-bit signed integer equivalent using the current culture. 
        /// </summary>
        /// <param name="input">A string that represents the number to convert.</param>
        /// <param name="parseOptions">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="input"/>.</param>
        /// <returns>A 32-bit signed integer equivalent to the number contained in <paramref name="input"/>.</returns>
        /// <exception cref="FormatException">Thrown when <paramref name="input"/> is not in a compliant format.</exception>
        /// <exception cref="OverflowException">Thrown when <paramref name="input"/> represents a number that is less than <see cref="Int32.MinValue"/> 
        ///                                     or greater than <see cref="Int32.MaxValue"/>.</exception>"
        public static int ParseInt( this string input, EIntParseOptions parseOptions )
        {
            return ParseInt( input, parseOptions, CultureInfo.CurrentCulture );
        }

        /// <summary>
        /// Converts the string representation of a number to its 32-bit signed integer equivalent. 
        /// </summary>
        /// <param name="input">A string that represents the number to convert.</param>
        /// <param name="parseOptions">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="input"/>.</param>
        /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A 32-bit signed integer equivalent to the number contained in <paramref name="input"/>.</returns>
        /// <exception cref="FormatException">Thrown when <paramref name="input"/> is not in a compliant format.</exception>
        /// <exception cref="OverflowException">Thrown when <paramref name="input"/> represents a number that is less than <see cref="Int32.MinValue"/> 
        ///                                     or greater than <see cref="Int32.MaxValue"/>.</exception>"
        public static int ParseInt( this string input, EIntParseOptions parseOptions, IFormatProvider? formatProvider )
        {
            if( parseOptions.HasFlag( EIntParseOptions.AllowPrefix ) )
            {
                if( parseOptions.HasFlag( EIntParseOptions.AllowLeadingWhite ) )
                {
                    input = input.TrimStart();
                }

                if( parseOptions.HasFlag( EIntParseOptions.AllowTrailingWhite ) )
                {
                    input = input.TrimEnd();
                }

                if( input.StartsWith( "0x", StringComparison.OrdinalIgnoreCase ) )
                {
                    input = input.Substring( 2 );
                    return int.Parse( input, NumberStyles.AllowHexSpecifier, formatProvider );
                }
#if NET8_0_OR_GREATER
                else if( input.StartsWith( "0b", StringComparison.OrdinalIgnoreCase ) )
                {
                    input = input.Substring( 2 );
                    return int.Parse( input, NumberStyles.AllowBinarySpecifier, formatProvider );
                }
#endif
                else
                {
                    const EIntParseOptions styleMask = ~( EIntParseOptions.AllowPrefix | EIntParseOptions.AllowLeadingWhite | EIntParseOptions.AllowTrailingWhite );
                    var numberStyles = (NumberStyles) ( parseOptions & styleMask );
                    return int.Parse( input, numberStyles, formatProvider );
                }
            }
            else
            {
                return int.Parse( input, (NumberStyles) parseOptions, formatProvider );
            }
        }

        /// <summary>
        /// Converts the string representation of a number to its 32-bit signed integer equivalent using the invariant culture. 
        /// </summary>
        /// <param name="input">A string that represents the number to convert.</param>
        /// <param name="parseOptions">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="input"/>.</param>
        /// <returns>A 32-bit signed integer equivalent to the number contained in <paramref name="input"/>.</returns>
        /// <exception cref="FormatException">Thrown when <paramref name="input"/> is not in a compliant format.</exception>
        /// <exception cref="OverflowException">Thrown when <paramref name="input"/> represents a number that is less than <see cref="Int32.MinValue"/> 
        ///                                     or greater than <see cref="Int32.MaxValue"/>.</exception>"
        public static int ParseInvariantInt( this string input, EIntParseOptions parseOptions )
        {
            return ParseInt( input, parseOptions, CultureInfo.InvariantCulture );
        }

        /// <summary>
        /// Tries to convert the string representation of a number to its 32-bit signed integer equivalent using the current culture. 
        /// </summary>
        /// <remarks>
        /// The conversion fails if the <paramref name="input"/> parameter is null or empty, is not in a compliant format,
        /// or represents a number that is less than <see cref="Int32.MinValue"/> or greater than <see cref="Int32.MaxValue"/>.
        /// </remarks>
        /// <param name="input">A string that represents the number to convert.</param>
        /// <param name="parseOptions">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="input"/>.</param>
        /// <param name="result">
        /// When this method returns, contains the 32-bit signed integer value equivalent to the number contained in <paramref name="input"/>, 
        /// if the conversion succeeded, or zero if the conversion failed.
        /// This parameter is passed uninitialized; any value originally supplied in result will be overwritten.
        /// </param>
        /// <returns><see langword="true"/> if <paramref name="input"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        public static bool TryParseInt( this string input, EIntParseOptions parseOptions, out int result )
        {
            return TryParseInt( input, parseOptions, CultureInfo.CurrentCulture, out result );
        }

        /// <summary>
        /// Tries to convert the string representation of a number to its 32-bit signed integer equivalent. 
        /// </summary>
        /// <remarks>
        /// The conversion fails if the <paramref name="input"/> parameter is null or empty, is not in a compliant format,
        /// or represents a number that is less than <see cref="Int32.MinValue"/> or greater than <see cref="Int32.MaxValue"/>.
        /// </remarks>
        /// <param name="input">A string that represents the number to convert.</param>
        /// <param name="parseOptions">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="input"/>.</param>
        /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
        /// <param name="result">
        /// When this method returns, contains the 32-bit signed integer value equivalent to the number contained in <paramref name="input"/>, 
        /// if the conversion succeeded, or zero if the conversion failed.
        /// This parameter is passed uninitialized; any value originally supplied in result will be overwritten.
        /// </param>
        /// <returns><see langword="true"/> if <paramref name="input"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        public static bool TryParseInt( this string input, EIntParseOptions parseOptions, IFormatProvider? formatProvider, out int result )
        {
            if( parseOptions.HasFlag( EIntParseOptions.AllowPrefix ) )
            {
                if( parseOptions.HasFlag( EIntParseOptions.AllowLeadingWhite ) )
                {
                    input = input.TrimStart();
                }

                if( parseOptions.HasFlag( EIntParseOptions.AllowTrailingWhite ) )
                {
                    input = input.TrimEnd();
                }

                if( input.StartsWith( "0x", StringComparison.OrdinalIgnoreCase ) )
                {
                    input = input.Substring( 2 );
                    return int.TryParse( input, NumberStyles.AllowHexSpecifier, formatProvider, out result );
                }
#if NET8_0_OR_GREATER
                else if( input.StartsWith( "0b", StringComparison.OrdinalIgnoreCase ) )
                {
                    input = input.Substring( 2 );
                    return int.TryParse( input, NumberStyles.AllowBinarySpecifier, formatProvider, out result );
                }
#endif
                else
                {
                    const EIntParseOptions styleMask = ~( EIntParseOptions.AllowPrefix | EIntParseOptions.AllowLeadingWhite | EIntParseOptions.AllowTrailingWhite );
                    var numberStyles = (NumberStyles) ( parseOptions & styleMask );
                    return int.TryParse( input, numberStyles, formatProvider, out result );
                }
            }
            else
            {
                return int.TryParse( input, (NumberStyles) parseOptions, formatProvider, out result );
            }
        }

        /// <summary>
        /// Tries to convert the string representation of a number to its 32-bit signed integer equivalent using the invariant culture. 
        /// </summary>
        /// <remarks>
        /// The conversion fails if the <paramref name="input"/> parameter is null or empty, is not in a compliant format,
        /// or represents a number that is less than <see cref="Int32.MinValue"/> or greater than <see cref="Int32.MaxValue"/>.
        /// </remarks>
        /// <param name="input">A string that represents the number to convert.</param>
        /// <param name="parseOptions">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="input"/>.</param>
        /// <param name="result">
        /// When this method returns, contains the 32-bit signed integer value equivalent to the number contained in <paramref name="input"/>, 
        /// if the conversion succeeded, or zero if the conversion failed.
        /// This parameter is passed uninitialized; any value originally supplied in result will be overwritten.
        /// </param>
        /// <returns><see langword="true"/> if <paramref name="input"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        public static bool TryParseInvariantInt( this string input, EIntParseOptions parseOptions, out int result )
        {
            return TryParseInt( input, parseOptions, CultureInfo.InvariantCulture, out result );
        }

        //***************************************************************************
        //                                UInt32
        //***************************************************************************

        /// <summary>
        /// Converts the string representation of a number to its 32-bit unsigned integer equivalent using the current culture. 
        /// </summary>
        /// <param name="input">A string that represents the number to convert.</param>
        /// <param name="parseOptions">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="input"/>.</param>
        /// <returns>A 32-bit unsigned integer equivalent to the number contained in <paramref name="input"/>.</returns>
        /// <exception cref="FormatException">Thrown when <paramref name="input"/> is not in a compliant format.</exception>
        /// <exception cref="OverflowException">Thrown when <paramref name="input"/> represents a number that is less than <see cref="UInt32.MinValue"/> 
        ///                                     or greater than <see cref="UInt32.MaxValue"/>.</exception>"
        public static uint ParseUInt( this string input, EUIntParseOptions parseOptions )
        {
            return ParseUInt( input, parseOptions, CultureInfo.CurrentCulture );
        }

        /// <summary>
        /// Converts the string representation of a number to its 32-bit unsigned integer equivalent. 
        /// </summary>
        /// <param name="input">A string that represents the number to convert.</param>
        /// <param name="parseOptions">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="input"/>.</param>
        /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A 32-bit unsigned integer equivalent to the number contained in <paramref name="input"/>.</returns>
        /// <exception cref="FormatException">Thrown when <paramref name="input"/> is not in a compliant format.</exception>
        /// <exception cref="OverflowException">Thrown when <paramref name="input"/> represents a number that is less than <see cref="UInt32.MinValue"/> 
        ///                                     or greater than <see cref="UInt32.MaxValue"/>.</exception>"
        public static uint ParseUInt( this string input, EUIntParseOptions parseOptions, IFormatProvider? formatProvider )
        {
            if( parseOptions.HasFlag( EUIntParseOptions.AllowPrefix ) )
            {
                if( parseOptions.HasFlag( EUIntParseOptions.AllowLeadingWhite ) )
                {
                    input = input.TrimStart();
                }

                if( parseOptions.HasFlag( EUIntParseOptions.AllowTrailingWhite ) )
                {
                    input = input.TrimEnd();
                }

                if( input.StartsWith( "0x", StringComparison.OrdinalIgnoreCase ) )
                {
                    input = input.Substring( 2 );
                    return uint.Parse( input, NumberStyles.AllowHexSpecifier, formatProvider );
                }
#if NET8_0_OR_GREATER
                else if( input.StartsWith( "0b", StringComparison.OrdinalIgnoreCase ) )
                {
                    input = input.Substring( 2 );
                    return uint.Parse( input, NumberStyles.AllowBinarySpecifier, formatProvider );
                }
#endif
                else
                {
                    const EUIntParseOptions styleMask = ~( EUIntParseOptions.AllowPrefix | EUIntParseOptions.AllowLeadingWhite | EUIntParseOptions.AllowTrailingWhite );
                    var numberStyles = (NumberStyles) ( parseOptions & styleMask );
                    return uint.Parse( input, numberStyles, formatProvider );
                }
            }
            else
            {
                return uint.Parse( input, (NumberStyles) parseOptions, formatProvider );
            }
        }

        /// <summary>
        /// Converts the string representation of a number to its 32-bit unsigned integer equivalent using the invariant culture. 
        /// </summary>
        /// <param name="input">A string that represents the number to convert.</param>
        /// <param name="parseOptions">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="input"/>.</param>
        /// <returns>A 32-bit unsigned integer equivalent to the number contained in <paramref name="input"/>.</returns>
        /// <exception cref="FormatException">Thrown when <paramref name="input"/> is not in a compliant format.</exception>
        /// <exception cref="OverflowException">Thrown when <paramref name="input"/> represents a number that is less than <see cref="UInt32.MinValue"/> 
        ///                                     or greater than <see cref="UInt32.MaxValue"/>.</exception>"
        public static uint ParseInvariantUInt( this string input, EUIntParseOptions parseOptions )
        {
            return ParseUInt( input, parseOptions, CultureInfo.InvariantCulture );
        }

        /// <summary>
        /// Tries to convert the string representation of a number to its 32-bit unsigned integer equivalent using the current culture. 
        /// </summary>
        /// <remarks>
        /// The conversion fails if the <paramref name="input"/> parameter is null or empty, is not in a compliant format,
        /// or represents a number that is less than <see cref="UInt32.MinValue"/> or greater than <see cref="UInt32.MaxValue"/>.
        /// </remarks>
        /// <param name="input">A string that represents the number to convert.</param>
        /// <param name="parseOptions">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="input"/>.</param>
        /// <param name="result">
        /// When this method returns, contains the 32-bit unsigned integer value equivalent to the number contained in <paramref name="input"/>, 
        /// if the conversion succeeded, or zero if the conversion failed.
        /// This parameter is passed uninitialized; any value originally supplied in result will be overwritten.
        /// </param>
        /// <returns><see langword="true"/> if <paramref name="input"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        public static bool TryParseUInt( this string input, EUIntParseOptions parseOptions, out uint result )
        {
            return TryParseUInt( input, parseOptions, CultureInfo.CurrentCulture, out result );
        }

        /// <summary>
        /// Tries to convert the string representation of a number to its 32-bit unsigned integer equivalent. 
        /// </summary>
        /// <remarks>
        /// The conversion fails if the <paramref name="input"/> parameter is null or empty, is not in a compliant format,
        /// or represents a number that is less than <see cref="UInt32.MinValue"/> or greater than <see cref="UInt32.MaxValue"/>.
        /// </remarks>
        /// <param name="input">A string that represents the number to convert.</param>
        /// <param name="parseOptions">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="input"/>.</param>
        /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
        /// <param name="result">
        /// When this method returns, contains the 32-bit unsigned integer value equivalent to the number contained in <paramref name="input"/>, 
        /// if the conversion succeeded, or zero if the conversion failed.
        /// This parameter is passed uninitialized; any value originally supplied in result will be overwritten.
        /// </param>
        /// <returns><see langword="true"/> if <paramref name="input"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        public static bool TryParseUInt( this string input, EUIntParseOptions parseOptions, IFormatProvider? formatProvider, out uint result )
        {
            if( parseOptions.HasFlag( EUIntParseOptions.AllowPrefix ) )
            {
                if( parseOptions.HasFlag( EUIntParseOptions.AllowLeadingWhite ) )
                {
                    input = input.TrimStart();
                }

                if( parseOptions.HasFlag( EUIntParseOptions.AllowTrailingWhite ) )
                {
                    input = input.TrimEnd();
                }

                if( input.StartsWith( "0x", StringComparison.OrdinalIgnoreCase ) )
                {
                    input = input.Substring( 2 );
                    return uint.TryParse( input, NumberStyles.AllowHexSpecifier, formatProvider, out result );
                }
#if NET8_0_OR_GREATER
                else if( input.StartsWith( "0b", StringComparison.OrdinalIgnoreCase ) )
                {
                    input = input.Substring( 2 );
                    return uint.TryParse( input, NumberStyles.AllowBinarySpecifier, formatProvider, out result );
                }
#endif
                else
                {
                    const EUIntParseOptions styleMask = ~( EUIntParseOptions.AllowPrefix | EUIntParseOptions.AllowLeadingWhite | EUIntParseOptions.AllowTrailingWhite );
                    var numberStyles = (NumberStyles) ( parseOptions & styleMask );
                    return uint.TryParse( input, numberStyles, formatProvider, out result );
                }
            }
            else
            {
                return uint.TryParse( input, (NumberStyles) parseOptions, formatProvider, out result );
            }
        }

        /// <summary>
        /// Tries to convert the string representation of a number to its 32-bit unsigned integer equivalent using the invariant culture. 
        /// </summary>
        /// <remarks>
        /// The conversion fails if the <paramref name="input"/> parameter is null or empty, is not in a compliant format,
        /// or represents a number that is less than <see cref="UInt32.MinValue"/> or greater than <see cref="UInt32.MaxValue"/>.
        /// </remarks>
        /// <param name="input">A string that represents the number to convert.</param>
        /// <param name="parseOptions">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="input"/>.</param>
        /// <param name="result">
        /// When this method returns, contains the 32-bit unsigned integer value equivalent to the number contained in <paramref name="input"/>, 
        /// if the conversion succeeded, or zero if the conversion failed.
        /// This parameter is passed uninitialized; any value originally supplied in result will be overwritten.
        /// </param>
        /// <returns><see langword="true"/> if <paramref name="input"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        public static bool TryParseInvariantUInt( this string input, EUIntParseOptions parseOptions, out uint result )
        {
            return TryParseUInt( input, parseOptions, CultureInfo.InvariantCulture, out result );
        }
    }
}
