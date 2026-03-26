/// @file
/// @license Public Domain

// Based on https://stackoverflow.com/a/59127914/27871372

#if NET5_0_OR_GREATER

using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Utilities.DotNet.Threading
{
    /// <summary>
    /// Utility class that provides atomic operations for enum types.
    /// </summary>
    public static class InterlockedEx
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <summary>
        /// Compares two instances of the specified enum <typeparamref name="TEnum"/> for equality and,
        /// if they're equal, replaces the first value, as an atomic operation.
        /// </summary>
        /// <param name="location">The destination, whose value is compared with <paramref name="comparand"/> and possibly replaced.</param>
        /// <param name="value">The value that replaces the <paramref name="location"/> value if the comparison results in equality.</param>
        /// <param name="comparand">The value that is compared to the value at <paramref name="location"/>.</param>
        /// <returns>The original value in <paramref name="location"/>.</returns>
        public static TEnum CompareExchange<TEnum>( ref TEnum location, TEnum value, TEnum comparand )
            where TEnum : struct, Enum
        {
            return Unsafe.SizeOf<TEnum>() switch
            {
#if NET9_0_OR_GREATER
                1 => CompareExchangeByte( ref location, value, comparand ),
                2 => CompareExchangeShort( ref location, value, comparand ),
#endif
                4 => CompareExchangeInt( ref location, value, comparand ),
                8 => CompareExchangeLong( ref location, value, comparand ),
                _ => throw new NotSupportedException( "Unsupported enum size" )
            };

#if NET9_0_OR_GREATER
            static TEnum CompareExchangeByte( ref TEnum location, TEnum value, TEnum comparand )
            {
                var comparandRaw = Unsafe.As<TEnum, byte>( ref comparand );
                var valueRaw = Unsafe.As<TEnum, byte>( ref value );
                ref var locationRaw = ref Unsafe.As<TEnum, byte>( ref location );
                var returnRaw = Interlocked.CompareExchange( ref locationRaw, valueRaw, comparandRaw );
                return Unsafe.As<byte, TEnum>( ref returnRaw );
            }

            static TEnum CompareExchangeShort( ref TEnum location, TEnum value, TEnum comparand )
            {
                var comparandRaw = Unsafe.As<TEnum, short>( ref comparand );
                var valueRaw = Unsafe.As<TEnum, short>( ref value );
                ref var locationRaw = ref Unsafe.As<TEnum, short>( ref location );
                var returnRaw = Interlocked.CompareExchange( ref locationRaw, valueRaw, comparandRaw );
                return Unsafe.As<short, TEnum>( ref returnRaw );
            }
#endif

            static TEnum CompareExchangeInt( ref TEnum location, TEnum value, TEnum comparand )
            {
                int comparandRaw = Unsafe.As<TEnum, int>( ref comparand );
                int valueRaw = Unsafe.As<TEnum, int>( ref value );
                ref int locationRaw = ref Unsafe.As<TEnum, int>( ref location );
                int returnRaw = Interlocked.CompareExchange( ref locationRaw, valueRaw, comparandRaw );
                return Unsafe.As<int, TEnum>( ref returnRaw );
            }

            static TEnum CompareExchangeLong( ref TEnum location, TEnum value, TEnum comparand )
            {
                long comparandRaw = Unsafe.As<TEnum, long>( ref comparand );
                long valueRaw = Unsafe.As<TEnum, long>( ref value );
                ref long locationRaw = ref Unsafe.As<TEnum, long>( ref location );
                long returnRaw = Interlocked.CompareExchange( ref locationRaw, valueRaw, comparandRaw );
                return Unsafe.As<long, TEnum>( ref returnRaw );
            }
        }

        /// <summary>
        /// Sets an enum <typeparamref name="TEnum"/> to a specified value and returns the original value, as an atomic operation.
        /// </summary>
        /// <param name="location">The variable to set to the specified value.</param>
        /// <param name="value">The value to which the <paramref name="location"/> parameter is set.</param>
        /// <returns>The original value of <paramref name="location"/>.</returns>
        public static TEnum Exchange<TEnum>( ref TEnum location, TEnum value )
            where TEnum : struct, Enum
        {
            return Unsafe.SizeOf<TEnum>() switch
            {
#if NET9_0_OR_GREATER
                1 => ExchangeByte( ref location, value ),
                2 => ExchangeShort( ref location, value ),
#endif
                4 => ExchangeInt( ref location, value ),
                8 => ExchangeLong( ref location, value ),
                _ => throw new NotSupportedException( "Unsupported enum size" )
            };

#if NET9_0_OR_GREATER
            static TEnum ExchangeByte( ref TEnum location, TEnum value )
            {
                var valueRaw = Unsafe.As<TEnum, byte>( ref value );
                ref var locationRaw = ref Unsafe.As<TEnum, byte>( ref location );
                var returnRaw = Interlocked.Exchange( ref locationRaw, valueRaw );
                return Unsafe.As<byte, TEnum>( ref returnRaw );
            }

            static TEnum ExchangeShort( ref TEnum location, TEnum value )
            {
                var valueRaw = Unsafe.As<TEnum, short>( ref value );
                ref var locationRaw = ref Unsafe.As<TEnum, short>( ref location );
                var returnRaw = Interlocked.Exchange( ref locationRaw, valueRaw );
                return Unsafe.As<short, TEnum>( ref returnRaw );
            }
#endif

            static TEnum ExchangeInt( ref TEnum location, TEnum value )
            {
                var valueRaw = Unsafe.As<TEnum, int>( ref value );
                ref var locationRaw = ref Unsafe.As<TEnum, int>( ref location );
                var returnRaw = Interlocked.Exchange( ref locationRaw, valueRaw );
                return Unsafe.As<int, TEnum>( ref returnRaw );
            }

            static TEnum ExchangeLong( ref TEnum location, TEnum value )
            {
                var valueRaw = Unsafe.As<TEnum, long>( ref value );
                ref var locationRaw = ref Unsafe.As<TEnum, long>( ref location );
                var returnRaw = Interlocked.Exchange( ref locationRaw, valueRaw );
                return Unsafe.As<long, TEnum>( ref returnRaw );
            }
        }

        /// <summary>
        /// Bitwise "ands" two enums <typeparamref name="TEnum"/> and replaces the first value with the result, as an atomic operation.
        /// </summary>
        /// <param name="location">A variable containing the first value to be combined and where the result is stored.</param>
        /// <param name="value">The value to be combined with the enum at <paramref name="location"/>.</param>
        /// <returns>The original value of <paramref name="location"/>.</returns>
        public static TEnum And<TEnum>( ref TEnum location, TEnum value )
            where TEnum : struct, Enum
        {
            return Unsafe.SizeOf<TEnum>() switch
            {
                4 => AndInt( ref location, value ),
                8 => AndLong( ref location, value ),
                _ => throw new NotSupportedException( "Unsupported enum size" )
            };

            static TEnum AndInt( ref TEnum location, TEnum value )
            {
                var valueRaw = Unsafe.As<TEnum, int>( ref value );
                ref var locationRaw = ref Unsafe.As<TEnum, int>( ref location );
                var returnRaw = Interlocked.And( ref locationRaw, valueRaw );
                return Unsafe.As<int, TEnum>( ref returnRaw );
            }

            static TEnum AndLong( ref TEnum location, TEnum value )
            {
                var valueRaw = Unsafe.As<TEnum, long>( ref value );
                ref var locationRaw = ref Unsafe.As<TEnum, long>( ref location );
                var returnRaw = Interlocked.And( ref locationRaw, valueRaw );
                return Unsafe.As<long, TEnum>( ref returnRaw );
            }
        }

        /// <summary>
        /// Bitwise "ors" two enums <typeparamref name="TEnum"/> and replaces the first value with the result, as an atomic operation.
        /// </summary>
        /// <param name="location">A variable containing the first value to be combined and where the result is stored.</param>
        /// <param name="value">The value to be combined with the enum at <paramref name="location"/>.</param>
        /// <returns>The original value of <paramref name="location"/>.</returns>
        public static TEnum Or<TEnum>( ref TEnum location, TEnum value )
            where TEnum : struct, Enum
        {
            return Unsafe.SizeOf<TEnum>() switch
            {
                4 => OrInt( ref location, value ),
                8 => OrLong( ref location, value ),
                _ => throw new NotSupportedException( "Unsupported enum size" )
            };

            static TEnum OrInt( ref TEnum location, TEnum value )
            {
                var valueRaw = Unsafe.As<TEnum, int>( ref value );
                ref var locationRaw = ref Unsafe.As<TEnum, int>( ref location );
                var returnRaw = Interlocked.Or( ref locationRaw, valueRaw );
                return Unsafe.As<int, TEnum>( ref returnRaw );
            }

            static TEnum OrLong( ref TEnum location, TEnum value )
            {
                var valueRaw = Unsafe.As<TEnum, long>( ref value );
                ref var locationRaw = ref Unsafe.As<TEnum, long>( ref location );
                var returnRaw = Interlocked.Or( ref locationRaw, valueRaw );
                return Unsafe.As<long, TEnum>( ref returnRaw );
            }
        }
    }
}

#endif