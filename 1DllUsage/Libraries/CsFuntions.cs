using DllUsage.Interfaces;
using CsDll;

namespace DllUsage.Libraries
{
    public class CsFuntions : IFunctions
    {
        public void SayHello(string name)
        {
            Functions.SayHello(name);
        }

        public double CalculateHypotenuse(double a, double b)
        {
            return Functions.CalculateHypotenuse(a, b);
        }

        public void SolveSquareEquation(double a, double b, double c)
        {
            Functions.SolveSquareEquation(a, b, c);
        }
    }
}
