using System;
using static I18N.Net.Global;

namespace Utilities.Net.Exceptions
{
    public class InvalidCastExceptionEx : InvalidCastException
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        public object Object { get; }

        public Type TargetType { get; }

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        public InvalidCastExceptionEx( object obj, Type targetType )
            : base( Localize( $"Cannot cast object '{obj}' of type {obj.GetType().FullName} to type {targetType.FullName}." ) )
        {
            Object = obj;
            TargetType = targetType;
        }
    }
}
