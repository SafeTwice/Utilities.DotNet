/// @file
/// @copyright  Copyright (c) 2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Utilities.DotNet.Collections.Observables.Test
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class CollectionChangedEventData
    {
        public CollectionChangedEventData( NotifyCollectionChangedEventArgs eventArgs )
        {
            m_action = eventArgs.Action;
            m_newItems = eventArgs.NewItems?.Cast<object?>();
            m_oldItems = eventArgs.OldItems?.Cast<object?>();
            m_newStartingIndex = eventArgs.NewStartingIndex;
            m_oldStartingIndex = eventArgs.OldStartingIndex;
        }

        public CollectionChangedEventData( NotifyCollectionChangedAction action,
                                           IEnumerable<object?>? newItems,
                                           IEnumerable<object?>? oldItems,
                                           int newStartingIndex,
                                           int oldStartingIndex )
        {
            m_action = action;
            m_newItems = newItems;
            m_oldItems = oldItems;
            m_newStartingIndex = newStartingIndex;
            m_oldStartingIndex = oldStartingIndex;
        }

        public override bool Equals( object? obj )
        {
            if( obj is CollectionChangedEventData other )
            {
                return ( m_action == other.m_action ) &&
                       ( ( m_newItems == other.m_newItems ) ||
                         ( ( m_newItems != null ) && ( other.m_newItems != null ) && Enumerable.SequenceEqual( m_newItems, other.m_newItems ) ) ) &&
                       ( ( m_oldItems == other.m_oldItems ) ||
                         ( ( m_oldItems != null ) && ( other.m_oldItems != null ) && Enumerable.SequenceEqual( m_oldItems, other.m_oldItems ) ) ) &&
                       ( m_newStartingIndex == other.m_newStartingIndex ) &&
                       ( m_oldStartingIndex == other.m_oldStartingIndex );
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return $"{{Action: {m_action}, NewItems: [{string.Join( ", ", m_newItems ?? Enumerable.Empty<object?>() )}], " +
                   $"OldItems: [{string.Join( ", ", m_oldItems ?? Enumerable.Empty<object?>() )}], " +
                   $"NewStartingIndex: {m_newStartingIndex}, OldStartingIndex: {m_oldStartingIndex}}}";
        }

        private readonly NotifyCollectionChangedAction m_action;
        private readonly IEnumerable<object?>? m_newItems;
        private readonly IEnumerable<object?>? m_oldItems;
        private readonly int m_newStartingIndex;
        private readonly int m_oldStartingIndex;
    }
#pragma warning restore CS0659
}
