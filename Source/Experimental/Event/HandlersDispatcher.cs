using System;

namespace Omega.Tools.Experimental.Events
{
    internal static class HandlersDispatcher<TEvent>
    {
        private static IHandlersProvider<TEvent> _provider;
        public static IHandlersProvider<TEvent> Provider => _provider ?? CreateDefault(); 

        public static void SetProvider(IHandlersProvider<TEvent> provider)
        {
            throw new NotImplementedException();
            _provider = provider;
        }

        private static IHandlersProvider<TEvent> CreateDefault()
            => new DefaultHandlersProvider<TEvent>();
    }
}