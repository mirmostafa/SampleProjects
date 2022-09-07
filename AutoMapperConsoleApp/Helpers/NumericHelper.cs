namespace AutoMapperConsoleApp.Helpers;

public static class NumericHelper
{
    public static int ToInt(this object? o)
        => Convert.ToInt32(o);
}