using System;

namespace RishUI.RDS
{
    public interface IInternalState<S> where S : struct, IEquatable<S>
    {
        void SetDefaultState(uint style, ref S defaultState);
    }
}