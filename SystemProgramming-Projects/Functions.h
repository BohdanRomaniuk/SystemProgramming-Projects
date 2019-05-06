#ifndef DLLEXPORT
#define DLLEXPORT __declspec(dllexport)
#endif

#pragma once
#include<iostream>

using namespace std;

class Functionality
{
public:
	void sayHelloTo(const char* name)
	{
		cout << "Hello " << name << "!" << endl;
	}

	double calculateHypotenuse(double firstCatet, double secondCatet)
	{
		return sqrt(pow(firstCatet, 2) + pow(secondCatet, 2));
	}

	void solveSquareEquation(double a, double b, double c)
	{
		double discriminant = b * b - 4 * a*c;
		double x1, x2, realPart, imaginaryPart;
		if (discriminant > 0)
		{
			x1 = (-b + sqrt(discriminant)) / (2 * a);
			x2 = (-b - sqrt(discriminant)) / (2 * a);
			cout << "Roots are real and different." << endl;
			cout << "x1 = " << x1 << endl;
			cout << "x2 = " << x2 << endl;
		}
		else if (discriminant == 0)
		{
			cout << "Roots are real and same." << endl;
			x1 = (-b + sqrt(discriminant)) / (2 * a);
			cout << "x1 = x2 =" << x1 << endl;
		}
		else
		{
			realPart = -b / (2 * a);
			imaginaryPart = sqrt(-discriminant) / (2 * a);
			cout << "Roots are complex and different." << endl;
			cout << "x1 = " << realPart << "+" << imaginaryPart << "i" << endl;
			cout << "x2 = " << realPart << "-" << imaginaryPart << "i" << endl;
		}
	}
};

Functionality* funcs = new Functionality();

extern "C" {
	DLLEXPORT void sayHelloTo(const char* name);
	DLLEXPORT double calculateHypotenuse(double firstCatet, double secondCatet);
	DLLEXPORT void solveSquareEquation(double a, double b, double c);
}