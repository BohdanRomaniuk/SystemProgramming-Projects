using DllUsage.Libraries;
using System;

namespace DllUsage
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("CppDllUsageExample");
            Console.WriteLine("----------------------------");
            CppFunctions cpp = new CppFunctions();
            cpp.SayHello("Bohdan");
            Console.WriteLine("Hypotenuse: " + cpp.CalculateHypotenuse(3, 4));
            cpp.SolveSquareEquation(2, 7, 3);

            Console.WriteLine("\n\n----------------------------\n\n");

            Console.WriteLine("CSharpDllUsageExample");
            Console.WriteLine("----------------------------");
            CsFuntions cs = new CsFuntions();
            cs.SayHello("Bohdan");
            Console.WriteLine("Hypotenuse: " + cs.CalculateHypotenuse(3, 4));
            cs.SolveSquareEquation(2, 7, 3);

            Console.ReadKey();
        }
    }
}
