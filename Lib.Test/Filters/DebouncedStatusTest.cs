/// @file
/// @copyright  Copyright (c) 2020 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using Utilities.Net.Filters;
using Xunit;

namespace Utilities.Net.Test.Filters
{
    public class DebouncedStatusTest
    {
        private bool m_activated;
        private bool m_deactivated;

        private void Check_OneActivation( DebouncedStatus filter, bool expectActive, bool expectActivation )
        {
            m_activated = false;
            m_deactivated = false;

            filter.UpdateStatus( true );

            Assert.Equal( expectActive, filter.Active );
            Assert.Equal( expectActivation, m_activated );
            Assert.False( m_deactivated );
        }

        private void Check_OneDeactivation( DebouncedStatus filter, bool expectActive, bool expectDeactivation )
        {
            m_activated = false;
            m_deactivated = false;

            filter.UpdateStatus( false );

            Assert.Equal( expectActive, filter.Active );
            Assert.False( m_activated );
            Assert.Equal( expectDeactivation, m_deactivated );
        }

        private void Check_Activation( DebouncedStatus filter )
        {
            Check_OneActivation( filter, false, false );

            Check_OneActivation( filter, false, false );

            Check_OneActivation( filter, true, true );
        }

        private void Check_Deactivation( DebouncedStatus filter )
        {
            Check_OneDeactivation( filter, true, false );

            Check_OneDeactivation( filter, true, false );

            Check_OneDeactivation( filter, false, true );
        }

        [Fact]
        public void Activation_Simple()
        {
            var filter = new DebouncedStatus( 3 );

            filter.Activated += delegate
            {
                m_activated = true;
            };

            filter.Deactivated += delegate
            {
                m_deactivated = true;
            };

            Check_Activation( filter );
        }

        [Fact]
        public void Activation_Complex()
        {
            var filter = new DebouncedStatus( 3 );

            filter.Activated += delegate
            {
                m_activated = true;
            };

            filter.Deactivated += delegate
            {
                m_deactivated = true;
            };

            Check_OneActivation( filter, false, false );

            Check_OneDeactivation( filter, false, false );

            Check_OneActivation( filter, false, false );

            Check_OneActivation( filter, false, false );

            Check_OneDeactivation( filter, false, false );

            Check_OneActivation( filter, false, false );

            Check_OneActivation( filter, true, true );
        }

        [Fact]
        public void Deactivation_Simple()
        {
            var filter = new DebouncedStatus( 3 );

            filter.Activated += delegate
            {
                m_activated = true;
            };

            filter.Deactivated += delegate
            {
                m_deactivated = true;
            };

            Check_Activation( filter );

            Check_Deactivation( filter );
        }

        [Fact]
        public void Deactivation_Complex()
        {
            var filter = new DebouncedStatus( 3 );

            filter.Activated += delegate
            {
                m_activated = true;
            };

            filter.Deactivated += delegate
            {
                m_deactivated = true;
            };

            Check_Activation( filter );

            Check_OneDeactivation( filter, true, false );

            Check_OneActivation( filter, true, false );

            Check_OneDeactivation( filter, true, false );

            Check_OneDeactivation( filter, true, false );

            Check_OneActivation( filter, true, false );

            Check_OneDeactivation( filter, true, false );

            Check_OneDeactivation( filter, false, true );
        }

        [Fact]
        public void Multiple()
        {
            var filter = new DebouncedStatus( 3 );

            filter.Activated += delegate
            {
                m_activated = true;
            };

            filter.Deactivated += delegate
            {
                m_deactivated = true;
            };

            for( int i = 0; i < 10; i++ )
            {
                Check_Activation( filter );

                Check_Deactivation( filter );
            }
        }
    }
}
