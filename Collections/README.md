# Utilities.DotNet.Collections

## About

The _Utilities.DotNet.Collections_ package provides useful extensions of the interfaces defined in `System.Collections.Generic` to amend the lack of properties and methods that should have been defined in these interfaces, and also provides the corresponding implementations of these extended interfaces, including a comprehensive set of observable collections.

## Usage

The following interfaces are defined:

| Interface                      | Description                                                                      |
|--------------------------------|----------------------------------------------------------------------------------|
| IReadOnlyCollectionEx&lt;T&gt; | Represents a read-only collection of objects.                                    |
| IReadOnlyListEx&lt;T&gt;       | Represents a read-only list of objects.                                          |
| ICollectionEx&lt;T&gt;         | Represents a collection of objects.                                              |
| IListEx&lt;T&gt;               | Represents a list of objects.                                                    |
| IObservableReadOnlyCollection&lt;T&gt; | Represents a read-only collection of objects that provides notifications when the collection changes.|
| IObservableReadOnlyList&lt;T&gt;       | Represents a read-only list of objects that provides notifications when the list changes.|
| IObservableCollection&lt;T&gt; | Represents a collection of objects that provides notifications when the collection changes. |
| IObservableList&lt;T&gt;       | Represents a list of objects that provides notifications when the list changes.  |

The following classes are provided:

| Class                                 | Description                                               |
|---------------------------------------|-----------------------------------------------------------|
| ListEx&lt;T&gt;                       | Implements a list of objects.                             |
| ReadOnlyCollectionEx&lt;T&gt;         | Implements a read-only collection of objects.             |
| ReadOnlyListEx&lt;T&gt;               | Implements a read-only list of objects.                   |
| ObservableCollection&lt;T&gt;         | Implements an observable collection of objects.           |
| ObservableList&lt;T&gt;               | Implements an observable list of objects.                 |
| ObservableSortedCollection&lt;T&gt;   | Implements an observable sorted collection of objects.    |
| ObservableSortedList&lt;T&gt;         | Implements an observable sorted list of objects.          |
| ObservableReadOnlyCollection&lt;T&gt; | Implements an observable read-only collection of objects. |
| ObservableReadOnlyList&lt;T&gt;       | Implements an observable read-only list of objects.       |

### IReadOnlyCollectionEx&lt;T&gt;

The `IReadOnlyCollectionEx<T>` interface represents a read-only collection of items. This interface provides methods to access the collection items, but does not provide any mechanism to modify the collection.

The `IReadOnlyCollectionEx<T>` interface extends [`System.Collections.Generic.IReadOnlyCollection<>`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlycollection-1) with the following additional methods:

| Method                   | Description                                              |
|--------------------------|----------------------------------------------------------|
| Contains                 | Determines whether the collection contains an item.      |
| CopyTo                   | Copies the elements of the collection to an array.       |

> **Note:** This interface does not provide methods to modify the collection, but this does not mean that the collection is immutable, the collection can still be modified by other means (e.g., it can be a read-only view of a modifiable collection).

### IReadOnlyListEx&lt;T&gt;

The `IReadOnlyListEx<T>` interface represents a read-only list of items. This interface provides methods to access the collection items, which can be individually accessed by index, but does not provide any mechanism to modify the list.

The `IReadOnlyListEx<T>` interface inherits from [`IReadOnlyCollectionEx<T>`]((#ireadonlycollectionext)) and also extends [`System.Collections.Generic.IReadOnlyList<>`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1) with the following additional methods:

| Method                    | Description                                                                        |
|---------------------------|------------------------------------------------------------------------------------|
| GetRange                  | Creates a shallow copy of a range of elements in the source list.                  |
| IndexOf                   | Searches for the first occurrence of an item in the entire list or in a sub-range. |
| LastIndexOf               | Searches for the last occurrence of an item in the entire list or in a sub-range.  |
| Slice                     | Creates a shallow copy of a range of elements in the source list.                  |

> **Note:** This interface does not provide methods to modify the list, but this does not mean that the list is immutable, the list can still be modified by other means (e.g., it can be a read-only view of a modifiable list).

> **Note:** The `Slice()` method allows accessing a sub-range of elements of the list using [C# 8.0 ranges and indices](https://learn.microsoft.com/en-us/dotnet/csharp/tutorials/ranges-indexes).

### ICollectionEx&lt;T&gt;

The `ICollectionEx<T>` interface represents a modifiable collection of items.

The `ICollectionEx<T>` interface inherits from [`IReadOnlyCollectionEx<T>`]((#ireadonlycollectionext)) and also extends [`System.Collections.Generic.ICollection<>`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.icollection-1) with the following additional methods:

| Method                   | Description                                                     |
|--------------------------|-----------------------------------------------------------------|
| AddRange                 | Adds the elements of a collection to the end of the collection. |
| RemoveRange              | Removes the elements of a collection from the collection.       |

### IListEx&lt;T&gt;

The `IListEx<T>` interface represents a modifiable list of items.

The `IListEx<T>` interface inherits from [`IReadOnlyListEx<T>`]((#ireadonlylistext)) and [`ICollectionEx<T>`]((#icollectionext)), and also extends [`System.Collections.Generic.IList<>`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ilist-1) with the following additional methods:

| Method                   | Description                                                     |
|--------------------------|-----------------------------------------------------------------|
| InsertRange              | Inserts the elements of a collection at a given index.          |
| RemoveRange              | Removes a range of elements from the list.                      |

### IObservableReadOnlyCollection&lt;T&gt;

The `IObservableReadOnlyCollection<T>` interface represents a read-only view of an observable collection of items that provides notifications when items are added or removed, or when the whole collection is cleared.

The `IObservableReadOnlyCollection<T>` interface inherits from [`IReadOnlyCollectionEx<T>`]((#ireadonlycollectionext)) and also from [`System.Collections.Specialized.INotifyCollectionChanged`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged).

### IObservableReadOnlyList&lt;T&gt;

The `IObservableReadOnlyList<T>` interface represents a read-only view of an observable list of items that provides notifications when items are added or removed, or when the whole list is cleared.

The `IObservableReadOnlyList<T>` interface inherits from [`IReadOnlyListEx<T>`]((#ireadonlylistext)) and [`IObservableReadOnlyCollectionEx<T>`]((#iobservablereadonlycollectionext))).

### IObservableCollection&lt;T&gt;

The `IObservableCollection<T>` interface represents a modifiable collection of items that provides notifications when items are added or removed, or when the whole collection is cleared.

The `IObservableCollection<T>` interface inherits from [`IObservableReadOnlyCollection<T>`]((#iobservablereadonlycollectiont))) and [`ICollectionEx<T>`]((#icollectionext)).

### IObservableList&lt;T&gt;

The `IObservableList<T>` interface represents a modifiable list of objects which can be individually accessed by index, and that provides notifications when items are added, removed, moved or replaced, or when the whole list is cleared.

The `IObservableList<T>` interface inherits from [`IObservableReadOnlyList<T>`]((#iobservablereadonlylistt)), [`IObservableCollection<T>`]((#iobservablecollectiont)) and [`IListEx<T>`]((#ilistext)).

### ListEx&lt;T&gt;

This class is functionally equivalent to [`System.Collections.Generic.List<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1), but with the advantage of implementing the [`IListEx<T>`](#ilistext) interface which defines more methods than [`System.Collections.Generic.IList<>`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ilist-1).

This class implements [`IListEx<T>`](#ilistext).

### ReadOnlyCollection&lt;T&gt;

The `ReadOnlyCollection<T>` class is used to create a read-only view of a collection.

This class implements [`IReadOnlyCollectionEx<T>`](#ireadonlycollectionext).

##### Example

``` CS
class ExampleClass
{
  public IReadOnlyCollectionEx<int> Items { get; } = new ReadOnlyCollection( m_items );
  ...
  private ICollectionEx<int> m_items;
}
```

### ReadOnlyList&lt;T&gt;

The `ReadOnlyList<T>` class is used to create a read-only view of a list.

This class implements [`IReadOnlyListEx<T>`](#ireadonlylistext).

##### Example

``` CS
class ExampleClass
{
  public IReadOnlyListEx<int> Items { get; } = new ReadOnlyList( m_items );
  ...
  private ListEx<int> m_items;
}
```

### ObservableCollection&lt;T&gt;

The `ObservableCollection<T>` class is functionally equivalent to [`System.Collections.ObjectModel.ObservableCollection<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.observablecollection-1), but without index-based access to items, and with the advantage of allowing abstraction as an observable collection through the `IObservableCollection<T>` interface.

This class implements [`IObservableCollection<T>`](#iobservablecollectiont).

### ObservableList&lt;T&gt;

The `ObservableList<T>` class is functionally equivalent to [`System.Collections.ObjectModel.ObservableCollection<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.observablecollection-1), but with the advantage of allowing abstraction as an observable list through the `IObservableList<T>` interface.

This class inherits from [`ObservableCollection<T>`](#observablecollectiont) and implements [`IObservableList<T>`](#iobservablelistt), adding access to the collection items by index..

### ObservableSortedCollection&lt;T&gt;

The `ObservableSortedCollection<T>` implements an observable sorted collection.

This class implements [`IObservableCollection<T>`]((#iobservablecollectiont)).

In order to enable sorting of items, collection items should implement  [`System.IComparable<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable-1) or [`System.IComparable`](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable). If collection items do not implement either interface, then a [`System.Collections.Generic.IComparer<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.icomparer-1) instance must be passed to the collection constructor.

The items added to the collection are automatically inserted to maintain the sort order.

In order to automatically update the sort order when the collection items change, they should implement [`System.ComponentModel.INotifyPropertyChanged`](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged). Otherwise, the method `UpdateSortOrder()` must be called each time that a collection item changes.

### ObservableSortedList&lt;T&gt;

The `ObservableSortedList<T>` class implements an observable sorted list.

This class inherits from [`ObservableSortedCollection<T>`](#observablesortedcollectiont) and implements [`IObservableList<T>`]((#iobservablelistt)), adding access to the collection items by index.

### ObservableReadOnlyCollection&lt;T&gt;

The `ObservableReadOnlyCollection<T>` class is used to create an observable read-only view of an observable collection.

This class implements [`IObservableReadOnlyCollection<T>`](#iobservablereadonlycollectiont).

##### Example

``` CS
class ExampleClass
{
  public IObservableReadOnlyCollection<ItemsClass> Items { get; } = new ObservableReadOnlyCollection( m_items );
  ...
  private IObservableCollection<ItemsClass> m_items;
}
```

### ObservableReadOnlyList&lt;T&gt;

The `ObservableReadOnlyList<T>` class is used to create an observable read-only view of an observable list.

This class inherits from [`ObservableReadOnlyCollection<T>`](#observablereadonlycollectiont) and implements [`IObservableReadOnlyList<T>`](#iobservablereadonlylistt), adding access to the collection items by index..

##### Example

``` CS
class ExampleClass
{
  public IObservableReadOnlyList<ItemsClass> Items { get; } = new ObservableReadOnlyList( m_items );
  ...
  private ObservableList<ItemsClass> m_items;
}
```

## Full API Documentation

You can browse the full API documentation for:
 - [The last release (stable)](https://safetwice.github.io/Utilities.DotNet/stable/namespace_utilities_1_1_dot_net_1_1_collections)
 - [Main branch (unstable)](https://safetwice.github.io/Utilities.DotNet/main/namespace_utilities_1_1_dot_net_1_1_collections)
