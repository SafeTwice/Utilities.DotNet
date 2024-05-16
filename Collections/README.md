# Utilities.DotNet.Collections

## About

The _Utilities.DotNet.Collections_ package provides implementations of the interfaces defined in `System.Collections` and  `System.Collections.Generic`.

## Usage

The following interfaces are defined:

| Interface                      | Description                                                                      |
|--------------------------------|----------------------------------------------------------------------------------|
| IObservableCollection&lt;T&gt; | Represents a collection that provides notifications when the collection changes. |
| IObservableList&lt;T&gt;       | Represents a list that provides notifications when the list changes.             |

The following classes are provided:

| Class                               | Description                                                              |
|-------------------------------------|---------------------------------------------|
| ObservableList&lt;T&gt;             | Implements an observable list.              |
| SortedObservableCollection&lt;T&gt; | Implements an observable sorted collection. |
| SortedObservableList&lt;T&gt;       | Implements an observable sorted list.       |

### IObservableCollection&lt;T&gt;

The `IObservableCollection<T>` interface inherits [`System.Collections.Generic.ICollection<>`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.icollection-1) and [`System.Collections.Specialized.INotifyCollectionChanged`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged).

It represents a collection of objects that provides notifications when items are added or removed, or when the whole collection is cleared.

### IObservableList&lt;T&gt;

The `IObservableList<T>` interface inherits [`System.Collections.Generic.IList<>`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ilist-1) and [`IObservableCollection<T>`](#iobservablecollectiont).

It represents a collection of objects which can be individually accessed by index, and that provides notifications when items are added, removed, moved or replaced, or when the whole collection is cleared.

### ObservableList&lt;T&gt;

The `ObservableList<T>` class implements [`IObservableList<T>`](#iobservablelistt).

This class if functionally equivalent to [`System.Collections.ObjectModel.ObservableCollection<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.observablecollection-1), but with the advance of allowing abstraction as an observable list through the `IObservableList<T>` interface.

### SortedObservableCollection&lt;T&gt;

The `SortedObservableCollection<T>` class implements [`IObservableCollection<T>`]((#iobservablecollectiont)).

In order to enable sorting of items, collection items should implement  [`System.IComparable<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable-1) or [`System.IComparable`](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable). If collection items do not implement either interface, then a [`System.Collections.Generic.IComparer<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.icomparer-1) instance must be passed to the collection constructor.

The items added to the collection are automatically inserted to maintain the sort order.

In order to automatically update the sort order when the collection items change, they should implement [`System.ComponentModel.INotifyPropertyChanged`](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged). Otherwise, the method `UpdateSortOrder()` must be called each time that a collection item changes.

### SortedObservableList&lt;T&gt;

The `SortedObservableList<T>` class implements [`IObservableList<T>`]((#iobservablelistt)).

This class extends [`SortedObservableCollection<T>`](#sortedobservablecollectiont), adding access to the collection items by index.

## Full API Documentation

You can browse the full API documentation for:
 - [The last release (stable)](https://safetwice.github.io/Utilities.DotNet/stable/namespace_utilities_1_1_dot_net_1_1_collections)
 - [Main branch (unstable)](https://safetwice.github.io/Utilities.DotNet/main/namespace_utilities_1_1_dot_net_1_1_collections)
