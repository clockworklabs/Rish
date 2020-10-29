using System;

namespace RishUI.RDS
{
    public interface IStyleSheet
    {
        void Get<T>(ref T result) where T : struct, IEquatable<T>;
    }
}
