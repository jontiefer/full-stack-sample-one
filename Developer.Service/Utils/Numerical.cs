using System;

namespace Developer.Service.Utils;

public static class Numerical
{
    public static bool AreNearlyEqual(double a, double b, double epsilon)
    {
        var absA = Math.Abs(a);
        var absB = Math.Abs(b);
        var diff = Math.Abs(a - b);

        if(a == b)
            return true;
        
        if (a == 0 || b == 0 || diff < double.Epsilon)
            return diff < epsilon * double.Epsilon;

        if(diff / (absA + absB) < epsilon)
            return true;

        return false;
    }
}
