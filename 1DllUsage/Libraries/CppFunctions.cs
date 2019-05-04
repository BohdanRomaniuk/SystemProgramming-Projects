using DllUsage.Interfaces;
using System.Runtime.InteropServices;

namespace DllUsage.Libraries
{
    public class CppFunctions : IFunctions
    {
        [DllImport("1CppDll.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void sayHelloTo(string name);

        [DllImport("1CppDll.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern double calculateHypotenuse(double a, double b);

        [DllImport("1CppDll.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void solveSquareEquation(double a, double b, double c);

        public void SayHello(string name)
        {
            sayHelloTo(name);
        }

        public double CalculateHypotenuse(double a, double b)
        {
            return calculateHypotenuse(a, b);
        }

        public void SolveSquareEquation(double a, double b, double c)
        {
            solveSquareEquation(a, b, c);
        }
    }
}
