using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Utilities.DotNet.Observables
{
    /// <summary>
    /// Base class for observable objects.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Observable objects generate PropertyChanged events when their properties change their values.
    /// </para>
    /// <para>
    /// When user together with Fody/PropertyChanged (<see href="https://github.com/Fody/PropertyChanged"/>),
    /// the generation of events when a property changes is automatic without any extra boilerplate code.
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
        /// Notifies that a property has changed.
        /// </summary>
        /// <remarks>
        /// This method is intended for manually generating changed events when automatic generation is not suitable.
        /// </remarks>
        /// <param name="propertyName">Name of the property that changed.</param>
        protected void OnPropertyChanged( [CallerMemberName] string propertyName = "" )
        {
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
        }
    }
}
