using System;
using System.Collections.Generic;

namespace Omega.Package
{
    public struct ListPoolHandler<T> : IDisposable
    {
        private List<T> _list;

        public List<T> List => _list;

        public ListPoolHandler(List<T> listFromHandler)
        {
            _list = listFromHandler;
        }

        public void Dispose()
        {
            if(_list != null)
                ListPool<T>.ReturnInternal(_list);
        }
    }
}