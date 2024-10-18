/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Utilities.DotNet.Types;
using Xunit;

namespace Utilities.DotNet.Test.Exceptions
{
    internal interface IBaseType
    {
    }

    internal abstract class AbstractType : IBaseType
    {
        public static DateTime Initialized { get; private set; }

        static AbstractType()
        {
            Initialized = DateTime.Now;
        }
    }

    internal class ConcreteType : AbstractType
    {
        public static DateTime Initialized2 { get; private set; }

        static ConcreteType()
        {
            Initialized2 = DateTime.Now;
        }
    }

    internal abstract class AbstractType2 : AbstractType
    {
    }

    internal class AnotherType : IBaseType
    {
        public static DateTime Initialized { get; private set; }

        static AnotherType()
        {
            Initialized = DateTime.Now;
        }
    }

    internal class YetAnotherType : IBaseType
    {
        public static DateTime Initialized { get; private set; }

        static YetAnotherType()
        {
            Initialized = DateTime.Now;
        }
    }

    public class TypeUtilitiesTest
    {
        [Fact]
        public void FindSubclasses_SingleAssembly_ForInterface_All()
        {
            var types = TypeUtilities.FindSubclasses<IBaseType>( typeof( TypeUtilitiesTest ).Assembly, false );

            Assert.Collection( types,
                t => Assert.Equal( typeof( AbstractType ), t ),
                t => Assert.Equal( typeof( ConcreteType ), t ),
                t => Assert.Equal( typeof( AbstractType2 ), t ),
                t => Assert.Equal( typeof( AnotherType ), t ),
                t => Assert.Equal( typeof( YetAnotherType ), t )
            );
        }

        [Fact]
        public void FindSubclasses_SingleAssembly_ForInterface_NonAbstract()
        {
            var types = TypeUtilities.FindSubclasses<IBaseType>( typeof( TypeUtilitiesTest ).Assembly, true );

            Assert.Collection( types,
                t => Assert.Equal( typeof( ConcreteType ), t ),
                t => Assert.Equal( typeof( AnotherType ), t ),
                t => Assert.Equal( typeof( YetAnotherType ), t )
            );
        }

        [Fact]
        public void FindSubclasses_SingleAssembly_ForClass_All()
        {
            var types = TypeUtilities.FindSubclasses<AbstractType>( typeof( TypeUtilitiesTest ).Assembly, false );

            Assert.Collection( types,
                t => Assert.Equal( typeof( ConcreteType ), t ),
                t => Assert.Equal( typeof( AbstractType2 ), t )
            );
        }

        [Fact]
        public void FindSubclasses_SingleAssembly_ForClass_NonAbstract()
        {
            var types = TypeUtilities.FindSubclasses<AbstractType>( typeof( TypeUtilitiesTest ).Assembly, true );

            Assert.Collection( types,
                t => Assert.Equal( typeof( ConcreteType ), t )
            );
        }

        [Fact]
        public void FindSubclasses_MultipleAssemblies()
        {
            var types = TypeUtilities.FindSubclasses<IBaseType>( AppDomain.CurrentDomain.GetAssemblies(), false );

            Assert.Collection( types,
                t => Assert.Equal( typeof( AbstractType ), t ),
                t => Assert.Equal( typeof( ConcreteType ), t ),
                t => Assert.Equal( typeof( AbstractType2 ), t ),
                t => Assert.Equal( typeof( AnotherType ), t ),
                t => Assert.Equal( typeof( YetAnotherType ), t )
            );
        }

        [Fact]
        public void FindSubclasses_AllAssemblies()
        {
            var types = TypeUtilities.FindSubclasses<IBaseType>( false );

            Assert.Collection( types,
                t => Assert.Equal( typeof( AbstractType ), t ),
                t => Assert.Equal( typeof( ConcreteType ), t ),
                t => Assert.Equal( typeof( AbstractType2 ), t ),
                t => Assert.Equal( typeof( AnotherType ), t ),
                t => Assert.Equal( typeof( YetAnotherType ), t )
            );
        }

        [Fact]
        public void RunClassConstructors()
        {
            TypeUtilities.RunClassConstructors<AbstractType>( typeof( TypeUtilitiesTest ).Assembly );

            var referenceTime = DateTime.Now;

            Thread.Sleep( 100 );

            Assert.True( AbstractType.Initialized <= referenceTime );
            Assert.True( ConcreteType.Initialized2 <= referenceTime );
            Assert.False( AnotherType.Initialized <= referenceTime );

            var referenceTime2 = DateTime.Now;

            Thread.Sleep( 100 );

            TypeUtilities.RunClassConstructors<IBaseType>( typeof( TypeUtilitiesTest ).Assembly );

            var referenceTime3 = DateTime.Now;

            Thread.Sleep( 100 );

            Assert.False( YetAnotherType.Initialized <= referenceTime2 );
            Assert.True( YetAnotherType.Initialized <= referenceTime3 );

            TypeUtilities.RunClassConstructors<IBaseType>( AppDomain.CurrentDomain.GetAssemblies() );
            TypeUtilities.RunClassConstructors<IBaseType>();

            Assert.True( AbstractType.Initialized <= referenceTime );
            Assert.True( ConcreteType.Initialized2 <= referenceTime );
            Assert.False( AnotherType.Initialized <= referenceTime );
            Assert.True( AnotherType.Initialized <= referenceTime2 );
            Assert.False( YetAnotherType.Initialized <= referenceTime2 );
            Assert.True( YetAnotherType.Initialized <= referenceTime3 );
        }

        [Theory]
        [InlineData( "IList", typeof( IList ) )]
        [InlineData( "ArrayList", typeof( ArrayList ) )]
        [InlineData( "List<Double>", typeof( List<double> ) )]
        [InlineData( "LinkedList<IEnumerable<Int32>>", typeof( LinkedList<IEnumerable<int>> ) )]
        public void GetPrettyName( string expected, Type type )
        {
            Assert.Equal( expected, TypeUtilities.GetPrettyName( type ) );
        }
    }
}
