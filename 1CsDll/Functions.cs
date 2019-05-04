using System;

namespace CsDll
{
    public class Functions
    {
        public static void SayHello(string name)
        {
            Console.WriteLine("Hello " + name + "!");
        }

        public static double CalculateHypotenuse(double a, double b)
        {
            return Math.Sqrt(a * a + b * b);
        }

        public static void SolveSquareEquation(double a, double b, double c)
        {
            double discriminant = b * b - 4 * a * c;
            double x1, x2, realPart, imaginaryPart;
            if (discriminant > 0)
            {
                x1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
                x2 = (-b - Math.Sqrt(discriminant)) / (2 * a);
                Console.WriteLine("Roots are real and different.");
                Console.WriteLine("x1 = {0}", x1);
                Console.WriteLine("x2 = {0}", x2);
            }
            else if (discriminant == 0)
            {
                Console.WriteLine("Roots are real and same.");
                x1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
                Console.WriteLine("x1 = x2 = {0}", x1);
            }
            else
            {
                realPart = -b / (2 * a);
                imaginaryPart = Math.Sqrt(-discriminant) / (2 * a);
                Console.WriteLine("Roots are complex and different.");
                Console.WriteLine("x1 = {0} + {1}i", realPart, imaginaryPart);
                Console.WriteLine("x2 = {0} - {1}i", realPart, imaginaryPart);
            }
        }
    }
}
