// See https://aka.ms/new-console-template for more information
using AutoMapperConsoleApp.Validations;

Console.WriteLine("Hello, World!");


void Test(string a)
{
    Check.IfArgumentNotNull(a);
}