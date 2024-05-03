# Utilities.DotNet.Converters

## About

The _Utilities.DotNet.XML_ package provides utility extensions to `System.Xml.Linq` classes.

## Usage

### Extensions to XElement

The following extension methods for `System.Xml.Linq.XElement` are provided:

| Method                   | Description                                              |
|--------------------------|----------------------------------------------------------|
| MandatoryAttribute       | Gets a mandatory attribute value as a `string`           |
| MandatoryAttributeInt    | Gets a mandatory attribute value as an `int`             |
| MandatoryAttributeUInt   | Gets a mandatory attribute value as an `uint`            |
| MandatoryAttributeDouble | Gets a mandatory attribute value as a `double`           |
| MandatoryAttributeBool   | Gets a mandatory attribute value as a `bool`             |
| MandatoryAttributeEnum   | Gets a mandatory attribute value as an `enum`            |
| OptionalAttribute        | Gets an optional attribute value as a `string`           |
| OptionalAttributeInt     | Gets an optional attribute value as an `int`             |
| OptionalAttributeUInt    | Gets an optional attribute value as an `uint`            |
| OptionalAttributeDouble  | Gets an optional attribute value as a `double`           |
| OptionalAttributeBool    | Gets an optional attribute value as a `bool`             |
| OptionalAttributeEnum    | Gets an optional attribute value as an `enum`            |
| MandatoryUniqueElement   | Gets a mandatory element, checking that it is unique     |
| OptionalUniqueElement    | Gets an optional element, checking that it is unique     |

#### Mandatory Attributes

The `MandatoryAttribute*` methods try to obtain an attribute from the `XElement` and convert it to the returned value type.

If the attribute does not exist, or its value is invalid (i.e., it cannot be converted to the returned value type), then a `Utilities.DotNet.Exceptions.FileProcessingException` is thrown.

#### Optional Attributes

The `OptionalAttribute*` methods try to obtain an attribute from the `XElement` and convert it to the returned value type.

If the attribute does not exist, a default value (which can be supplied by the caller) is returned.

However, if it exists but its value is invalid (i.e., it cannot be converted to the returned value type), then a `Utilities.DotNet.Exceptions.FileProcessingException` is thrown.

#### Mandatory Elements

The `MandatoryUniqueElement` method tries to obtain an element from the parent `XElement`.

If the element does not exist, or if it is not unique (i.e., many elements with that same name are present), then a `Utilities.DotNet.Exceptions.FileProcessingException` is thrown.

#### Optional Elements

The `OptionalUniqueElement` method tries to obtain an element from the parent `XElement`.

If the element does not exist, then `null` is returned.

If the element is not unique (i.e., many elements with that same name are present), then a `Utilities.DotNet.Exceptions.FileProcessingException` is thrown.


#### Example

``` CS
try
{
  var List<Item> itemList = new();

  var doc = XDocument.Load( filename, LoadOptions.SetBaseUri | LoadOptions.SetLineInfo );

  foreach( var itemElement in doc.Root )
  {
    string name = element.MandatoryAttribute( "name" );
    uint count = element.MandatoryAttributeUInt( "count" );
    double price = element.MandatoryAttributeDouble( "price" );
    bool inStock = element.OptionalAttributeBool( "in-stock" );
    ItemCategory category = element.MandatoryAttributeEnum<ItemCategory>( "category" );

    itemList.Add( new Item( name, category, count, price, inStock ) );
  }

  ...
}
catch( FileProcessingException e )
{
  MessageBox.Show( e.Message, "Error loading items" );
}
```

> **NOTE**: In order to have `FileProcessingException` exceptions properly filled with the information regarding the file and line where the error was found, when loading the `XDocument` use the options `LoadOptions.SetLineInfo` (to get line info) and `LoadOptions.SetBaseUri` (to get file info).

### Extensions to XContainer

The following extension methods for `System.Xml.Linq.XContainer` are provided:

| Method                   | Description                                                            |
|--------------------------|------------------------------------------------------------------------|
| AddUnique                | Adds a node as child of a container if it not already added.           |

The `AddUnique` method adds an `XNode` as a child of an `XContainer` if it's not already added, removing it from its
previous parent if necessary.

## Full API Documentation

You can browse the full API documentation for:
 - [The last release (stable)](https://safetwice.github.io/Utilities.DotNet/stable/namespace_utilities_1_1_dot_net_1_1_converters)
 - [Main branch (unstable)](https://safetwice.github.io/Utilities.DotNet/main/namespace_utilities_1_1_dot_net_1_1_converters)
