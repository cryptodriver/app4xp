using Application.Core;
using System;
using System.Reflection;

namespace Common.Dispachers
{
    [Autofac(true)]
    public class TaskDispacher : ITaskDispacher
    {
        public bool Dispach(dynamic context)
        {
            if (context.Payload.TaskName == null)
            {
                return false;
            }

            Type type = Type.GetType((string)context.Payload.TaskName);
            MethodInfo method = type.GetMethod("Run");

            method.Invoke(Activator.CreateInstance(type), parameters: new object[] { context });

            return true;
        }
    }
}
