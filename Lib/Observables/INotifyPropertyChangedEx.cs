/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;

namespace Utilities.DotNet.Observables
{
    /// <summary>
    /// Data for the <see cref="INotifyPropertyChangedEx.PropertyChangedEx"/> event.
    /// </summary>
    public interface IPropertyChangedExEventArgs
    {
        /// <summary>
        /// Name of the property that changed.
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// Previous value of the property.
        /// </summary>
        object? OldValue { get; }

        /// <summary>
        /// Current value of the property.
        /// </summary>
        object? NewValue { get; }
    }

    /// <summary>
    /// Data for the <see cref="INotifyPropertyChangedEx.PropertyChangedEx"/> event.
    /// </summary>
    /// <typeparam name="T">Type of the property.</typeparam>
    public interface IPropertyChangedExEventArgs<out T> : IPropertyChangedExEventArgs
    {
        /// <summary>
        /// Previous value of the property.
        /// </summary>
        new T OldValue { get; }

        /// <summary>
        /// New value of the property.
        /// </summary>
        new T NewValue { get; }
    }

    /// <summary>
    /// Implementation of data for the <see cref="INotifyPropertyChangedEx.PropertyChangedEx"/> event.
    /// </summary>
    public class PropertyChangedExEventArgs<T> : EventArgs, IPropertyChangedExEventArgs<T>
    {
        /// <inheritdoc/>
        public virtual string PropertyName { get; }

        /// <inheritdoc/>
        public virtual T OldValue { get; }

        /// <inheritdoc/>
        public virtual T NewValue { get; }

        object? IPropertyChangedExEventArgs.OldValue => OldValue;

        object? IPropertyChangedExEventArgs.NewValue => NewValue;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="propertyName">Name of the property that changed.</param>
        /// <param name="oldValue">Previous value of the property.</param>
        /// <param name="newValue">New value of the property.</param>
        public PropertyChangedExEventArgs( string propertyName, T oldValue, T newValue )
        {
            PropertyName = propertyName;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }

    /// <summary>
    /// Represents the method that will handle the <see cref="INotifyPropertyChangedEx.PropertyChangedEx"/> event
    /// raised when a property is changed on a component.
    /// </summary>
    /// <param name="sender">Source of the event.</param>
    /// <param name="e">Event data.</param>
    public delegate void PropertyChangedExEventHandler( object sender, IPropertyChangedExEventArgs e );

    /// <summary>
    /// Notifies clients that a property value has changed, indicating the old and the new values.
    /// </summary>
    public interface INotifyPropertyChangedEx
    {
        //===========================================================================
        //                                  EVENTS
        //===========================================================================

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        event PropertyChangedExEventHandler PropertyChangedEx;
    }
}
