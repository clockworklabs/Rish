using System;

namespace RishUI.RDS
{
    public interface IInternalProps<P> where P : struct, IEquatable<P>
    {
        void SetDefaultProps(uint style, ref P defaultProps);
    }
}