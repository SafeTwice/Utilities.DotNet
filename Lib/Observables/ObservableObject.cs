using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Utilities.DotNet.Observables
{
    /// <summary>
    /// Base class for observable objects.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Observable objects generate  <see cref="PropertyChanged"/> events when their properties change their values.
    /// </para>
    /// <para>
    /// When user together with Fody/PropertyChanged (<see href="https://github.com/Fody/PropertyChanged"/>),
    /// the generation of events when a property changes is automatic without any extra boilerplate code.
    /// </para>
    /// <para>
    /// If not using Fody/PropertyChanged, derived classes must, for properties that notify their changes using the <see cref="PropertyChanged"/> event, either:
    /// <list type="bullet">
    ///   <item>Use the <see cref="SetProperty{T}(ref T, T, string)"/> method to manage the property using a backing field.</item>
    ///   <item>Call the <see cref="OnPropertyChanged(string)"/> method from the property setter <i>after</i> the property value has been updated. </item>
    /// </list>
    /// </para>
    /// </remarks>
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        //===========================================================================
        //                             PUBLIC EVENTS
        //===========================================================================

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <summary>
        /// Helper method to set a property using a backing field and raise the <see cref="PropertyChanged"/> event.
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
                field = value;
                OnPropertyChanged( propertyName );
            }
        }

        /// <summary>
        /// Notifies that a property has changed.
        /// </summary>
        /// <remarks>
        /// This method is intended for manually generating changed events when automatic generation is not suitable.
        /// </remarks>
        /// <param name="propertyName">Name of the property that changed (can be omitted if called from the property's setter).</param>
        protected void OnPropertyChanged( [CallerMemberName] string propertyName = "" )
        {
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
        }
    }
}
