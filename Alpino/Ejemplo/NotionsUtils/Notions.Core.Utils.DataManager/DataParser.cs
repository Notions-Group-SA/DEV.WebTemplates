using System;
using System.Data;

namespace Notions.Core.Utils.DataManager;

public static class DataParser
{
    public static int ToInt(object? value)
    {
        if (int.TryParse(value?.ToString(), out int result))
            return result;

        return 0;
    }

    public static int? ToIntNullable(object? value)
    {
        if (int.TryParse(value?.ToString(), out int result))
            return result;

        return null;
    }

    public static short ToShort(object? value)
    {
        if (short.TryParse(value?.ToString(), out short result))
            return result;

        return 0;
    }
    
    public static short? ToShortNullable(object? value)
    {
        if (short.TryParse(value?.ToString(), out short result))
            return result;

        return null;
    }

    public static long ToLong(object? value)
    {
        if (long.TryParse(value?.ToString(), out long result))
            return result;
        return 0L;
    }

    public static long? ToLongNullable(object? value)
    {
        if (long.TryParse(value?.ToString(), out long result))
            return result;
        return null;
    }
    public static string ToString(object? value)
    {
        return value?.ToString() ?? string.Empty;
    }

    public static string? ToStringNullable(object? value)
    {
        if (value == null || value == DBNull.Value)
            return null;

        return value.ToString();
    }

    public static DateTime ToDateTime(object? value)
    {
        if (DateTime.TryParse(value?.ToString(), out DateTime result))
            return result;

        return DateTime.Parse("1900-01-01");
    }

    public static DateTime? ToDateTimeNullable(object? value)
    {
        if (DateTime.TryParse(value?.ToString(), out DateTime result))
            return result;

        return null;
    }

    public static bool ToBool(object? value)
    {
        if (bool.TryParse(value?.ToString(), out bool result))
            return result;

        return false;
    }
    public static bool? ToBoolNullable(object? value)
    {
        if (bool.TryParse(value?.ToString(), out bool result))
            return result;

        return null;
    }

    public static decimal ToDecimal(object? value)
    {
        if (decimal.TryParse(value?.ToString(), out decimal result))
            return result;

        return 0m;
    }

    public static decimal? ToDecimalNullable(object? value)
    {
        if (decimal.TryParse(value?.ToString(), out decimal result))
            return result;

        return null;
    }

    public static float ToFloat(object? value)
    {
        if (float.TryParse(value?.ToString(), out float result))
            return result;

        return 0f;
    }

    public static float? ToFloatNullable(object? value)
    {
        if (float.TryParse(value?.ToString(), out float result))
            return result;

        return null;
    }

    public static Guid ToGuid(object? value)
    {
        if (Guid.TryParse(value?.ToString(), out Guid result))
            return result;

        return Guid.Empty;
    }

    public static Guid? ToGuidNullable(object? value)
    {
        if (Guid.TryParse(value?.ToString(), out Guid result))
            return result;

        return null;
    }
    public static byte[] ToByteArray(object? value)
    {
        if (value is byte[] byteArray)
            return byteArray;

        if (value is string strValue && !string.IsNullOrEmpty(strValue))
            return Convert.FromBase64String(strValue);

        return Array.Empty<byte>();
    }

}
