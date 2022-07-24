namespace Wpf.ModernColorPicker.Common;

public static class MathHelpers
{
    public static double Clamp(double value, double minBound, double maxBound)
    {
#if NETCOREAPP3_0_OR_GREATER
        return System.Math.Clamp(value, minBound, maxBound);
#else
        return value < minBound ? minBound : value > maxBound ? maxBound : value;
#endif
    }
}
