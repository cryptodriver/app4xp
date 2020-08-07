using Application.Core;

namespace Application
{
    public class ServiceProvider
    {
        public static IAutofacLocator Instance { get; private set; }

        public static void RegisterServiceLocator(IAutofacLocator s)
        {
            Instance = s;
        }
    }
}
