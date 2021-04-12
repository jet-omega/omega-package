using System.Collections;

namespace Omega.Package
{
    public interface IPool<T>
    {
        T Get();
        void Return(T value);
    }
}