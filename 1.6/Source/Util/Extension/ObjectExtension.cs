namespace Rhynia.Baseline.Util;

public static class ObjectExtension
{
    /// <summary>
    /// Checks if the object is not equal to any of the provided values.
    /// </summary>
    public static bool NotEqualToAnyOf(this object obj, params object[] values) =>
        values.Any(value => obj != value);

    /// <summary>
    /// Checks if the object is not equal to all of the provided values.
    /// </summary>
    public static bool NotEqualToAllOf(this object obj, params object[] values) =>
        values.All(value => obj != value);
}
