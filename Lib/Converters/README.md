# Utilities.DotNet.Converters

## About

The _Utilities.DotNet.Converters_ package provides converters between representations of numbers (signed and unsigned integers, and floating point) stored in byte arrays and the primitive types that represent them.

## Usage

### Simple Usage

Two classes provide static methods to perform conversion between numbers stored in byte arrays to primitive types.

| Class                    | Description                                                               |
|--------------------------|---------------------------------------------------------------------------|
| BigEndianBitConverter    | Provides conversion functions for numbers stored in big-endian format.    |
| LittleEndianBitConverter | Provides conversion functions for numbers stored in little-endian format. |

| Method   | Description                                                                              |
|--------------------------|--------------------------------------------------------------------------|
| ToUInt16 | Converts a value stored using 2 bytes in a byte array to a 16-bit unsigned integer.      |
| ToUInt32 | Converts a value stored using 4 bytes in a byte array to a 32-bit unsigned integer.      |
| ToUInt64 | Converts a value stored using 8 bytes in a byte array to a 64-bit unsigned integer.      |
| ToInt16  | Converts a value stored using 2 bytes in a byte array to a 16-bit signed integer.        |
| ToInt32  | Converts a value stored using 4 bytes in a byte array to a 32-bit signed integer.        |
| ToInt64  | Converts a value stored using 8 bytes in a byte array to a 64-bit signed integer.        |
| ToSingle | Converts a value stored using 4 bytes in a byte array to a 32-bit floating-point number. |
| ToDouble | Converts a value stored using 8 bytes in a byte array to a 64-bit floating-point number. |

#### Example

``` CS
byte[] storedValue = { 0xC0, 0xFF, 0xEE, 0x11 };

UInt32 primitiveValueBE = BigEndianBitConverter.ToUInt32( storedValue );
UInt32 primitiveValueLE = LittleEndianBitConverter.ToUInt32( storedValue );

Assert.Equal( 0xC0FFEE11, primitiveValueBE );
Assert.Equal( 0x11EEFFC0, primitiveValueLE );
```

### Advanced Usage

The classes `BigEndianBitConverter` and `LittleEndianBitConverter` are subclasses of `BitConverter`, which provides instance methods with the same signature than the static methods listed above.

In cases where the conversion type must be decided at runtime, a `BitConverter` object can be passed around, initialized with the needed type of converter.

For convenience, both `BigEndianBitConverter` and `LittleEndianBitConverter` have an `Instance` static field initialized with its corresponding type of object.

#### Example

``` CS
class AClass
{
	public ACLass( BitConverter converter )
    {
    	m_converter = converter;
    	...
    }

    ...

    private BitConverter m_converter;
}

class Program
{
	static void Main(string[] args)
    {
    	var objA = new AClass( BigEndianBitConverter.Instance );

    	...
    }
}
```

## Full API Documentation

You can browse the full API documentation for:
 - [The last release (stable)](https://safetwice.github.io/Utilities.DotNet/stable/namespace_utilities_1_1_dot_net_1_1_converters)
 - [Main branch (unstable)](https://safetwice.github.io/Utilities.DotNet/main/namespace_utilities_1_1_dot_net_1_1_converters)
