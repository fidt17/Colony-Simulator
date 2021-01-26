using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class TraversabilityArgs : EventArgs
    {
        public bool IsTraversable;
    }
    
    public interface ITraversable
    {
        event EventHandler OnTraversabilityChange;
        
        bool IsTraversable { get; }

        void SetTraversability(bool value);
    }
}
