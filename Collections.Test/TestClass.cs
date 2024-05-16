/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.ComponentModel;

namespace Utilities.DotNet.Test.Collections
{
    public class TestClass : INotifyPropertyChanged, IComparable
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

        public TestClass( string name, int value )
        {
            m_name = name;
            m_value = value;
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

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
