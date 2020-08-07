using System;
using System.Reflection;

namespace Application.Infrastructure.EventBus
{
    internal class EventHandlerHolder
    {
        public EventHandlerHolder(object handler, MethodInfo methodInfo, Type parameterType)
        {
            Handler = handler;
            MethodInfo = methodInfo;
            ParameterType = parameterType;
        }

        public object Handler { get; }

        public MethodInfo MethodInfo { get; }

        public Type ParameterType { get; }
    }
}
