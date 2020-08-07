using Application.Core;

namespace Common.Dispachers
{
    [Autofac(true)]
    public class ErrorDispacher : IErrorDispacher
    {
        public bool Dispach(dynamic data)
        {

            return true;
        }
    }
}
