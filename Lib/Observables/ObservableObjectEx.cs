/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Utilities.DotNet.Observables
{
    /// <summary>
    /// Base class for observable objects that implement the <see cref="INotifyPropertyChangedEx"/> interface.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Observable objects generate <see cref="PropertyChangedEx"/> events when their properties change their values.
    /// </para>
    /// <para>
    /// For convenience, this class also implements the <see cref="INotifyPropertyChanged"/> interface.
    /// </para>
    /// <para>
    /// Derived classes must, for properties that notify their changes using the <see cref="PropertyChangedEx"/> event, either:
    /// <list type="bullet">
    ///   <item>Use the <see cref="SetProperty{T}(ref T, T, string)"/> method to manage the property using a backing field.</item>
    ///   <item>Call the <see cref="OnPropertyChanged{T}(T, T, string)"/> method from the property setter <i>after</i> the property value has been updated. </item>
    /// </list>
    /// </para>
    /// </remarks>
    public class ObservableObjectEx : INotifyPropertyChangedEx, INotifyPropertyChanged
    {
        //===========================================================================
        //                             PUBLIC EVENTS
        //===========================================================================

        /// <inheritdoc/>
        public event PropertyChangedExEventHandler? PropertyChangedEx;

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <summary>
        /// Helper method to set a property using a backing field and raise the <see cref="PropertyChangedEx"/> and <see cref="ObservableObject.PropertyChanged"/> events.
        /// </summary>
        /// <example>
        /// <code>
        /// public object MyProperty
        /// {
        ///     get => m_myProperty;
        ///     set => SetProperty( ref m_myProperty, value );
        /// }
        /// </code>
        /// </example>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="field">Backing field for the property.</param>
        /// <param name="value">New value for the property.</param>
        /// <param name="propertyName">Name of the property (can be omitted if called from the property's setter).</param>
        protected void SetProperty<T>( ref T field, T value, [CallerMemberName] string propertyName = "" )
        {
            if( !Equals( field, value ) )
            {
                T oldValue = field;
                field = value;
                OnPropertyChanged( oldValue, value, propertyName );
            }
        }

        /// <summary>
        /// Raises the <see cref="PropertyChangedEx"/> and <see cref="ObservableObject.PropertyChanged"/> events.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="oldValue">Previous value of the property.</param>
        /// <param name="newValue">New value of the property.</param>
        /// <param name="propertyName">Name of the property that changed (can be omitted if called from the property's setter).</param>
        protected void OnPropertyChanged<T>( T oldValue, T newValue, [CallerMemberName] string propertyName = "" )
        {
            PropertyChangedEx?.Invoke( this, new PropertyChangedExEventArgs<T>( propertyName, oldValue, newValue ) );
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
        }

#pragma warning disable S1133
        /// <summary>
        /// This function shall only be called by Fody.PropertyChanged injected code.
        /// </summary>
        /// <param name="propertyName"></param>
        [Obsolete( "Use OnPropertyChanged<T>(T, T, string) instead." )]
        protected void OnPropertyChanged( string propertyName )
        {
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
        }
#pragma warning restore S1133
    }
}
