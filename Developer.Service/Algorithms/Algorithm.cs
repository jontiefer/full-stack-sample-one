using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Developer.Service.Algorithms;

public static class Algorithm
{
    public static int GetFactorial(int n)
    {
        try
        {
            var factorialResult = Enumerable.Range(1, n).Aggregate((x, y) => checked(x * y));

            return factorialResult;
        }
        catch (OverflowException ex)
        {
            Console.Write($"The value {n} is invalid and results an invalid result.");
            throw;
        }
    }

    public static double GetFactorialDouble(int n)
    {
        var factorialResult = 
            Enumerable.Range(1, n).Select(Convert.ToDouble)
            .Aggregate((x, y) => checked(x * y));

        return factorialResult;
    } 

    public static string FormatSeparators(params string[] items)
    {
        if (items.Length == 1) return items[0];

        var formattedItems = string.Join(", ", items[..^1]);

        return string.Concat(formattedItems, $" and {items[^1]}");
    }
}