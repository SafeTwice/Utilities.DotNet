/// @file
/// @copyright  Copyright (c) 2024-2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.ComponentModel;
using Xunit.Abstractions;

namespace Utilities.DotNet.Collections.Test
{
    public class TestClass : INotifyPropertyChanged, IComparable, IXunitSerializable
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        public string Name
        {
            get => m_name;
            set
            {
                if( m_name != value )
                {
                    m_name = value;
                    OnPropertyChanged( nameof( Name ) );
                }
            }
        }

        public int Value
        {
            get => m_value;
            set
            {
                if( m_value != value )
                {
                    m_value = value;
                    OnPropertyChanged( nameof( Value ) );
                }
            }
        }

        //===========================================================================
        //                             PUBLIC EVENTS
        //===========================================================================

        public event PropertyChangedEventHandler? PropertyChanged;

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        public TestClass()
        {
            m_name = string.Empty;
            m_value = 0;
        }

        public TestClass( string name, int value )
        {
            m_name = name;
            m_value = value;
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        public override string ToString()
        {
            return $"<{m_name} / {m_value}>";
        }

        public int CompareTo( object? obj )
        {
            if( obj == null )
            {
                return 1;
            }
            else if( obj is TestClass other )
            {
                return m_name.CompareTo( other.m_name );
            }
            else
            {
                throw new ArgumentException( "Object is not a TestClass" );
            }
        }

        public override bool Equals( object? obj ) => ( CompareTo( obj ) == 0 );

#pragma warning disable S2328 // "GetHashCode" should not reference mutable fields
        public override int GetHashCode() => m_name.GetHashCode();
#pragma warning restore S2328 // "GetHashCode" should not reference mutable fields

        public void Serialize( IXunitSerializationInfo info )
        {
            info.AddValue( nameof( Name ), m_name );
            info.AddValue( nameof( Value ), m_value );
        }

        public void Deserialize( IXunitSerializationInfo info )
        {
            m_name = info.GetValue<string>( nameof( Name ) );
            m_value = info.GetValue<int>( nameof( Value ) );
        }

        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        private void OnPropertyChanged( string propertyName )
        {
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
        }

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private string m_name;
        private int m_value;
    }
}
