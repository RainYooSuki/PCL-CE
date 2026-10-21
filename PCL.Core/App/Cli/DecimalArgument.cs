using System;
using System.Diagnostics.CodeAnalysis;

namespace PCL.Core.App.Cli;

public class DecimalArgument : CommandArgument<decimal>
{
    public override ArgumentValueKind ValueKind => ArgumentValueKind.Decimal;

    protected override decimal ParseValueText() => decimal.Parse(ValueText);

    public new decimal Value
    {
        get => base.Value;
        init => base.Value = value;
    }

    public override bool TryCastValue<T>([NotNullWhen(true)] out T? value)
    {
        if (base.TryCastValue(out value)) return true;
        var type = typeof(T);
        try
        {
            if (type == typeof(int)) value = (T)(object)Convert.ToInt32(Value);
            else if (type == typeof(long)) value = (T)(object)Convert.ToInt64(Value);
            else if (type == typeof(double)) value = (T)(object)Convert.ToDouble(Value);
            else if (type == typeof(float)) value = (T)(object)Convert.ToSingle(Value);
            else if (type == typeof(short)) value = (T)(object)Convert.ToInt16(Value);
            else if (type == typeof(sbyte)) value = (T)(object)Convert.ToSByte(Value);
            else if (type == typeof(ulong)) value = (T)(object)Convert.ToUInt64(Value);
            else if (type == typeof(uint)) value = (T)(object)Convert.ToUInt32(Value);
            else if (type == typeof(ushort)) value = (T)(object)Convert.ToUInt16(Value);
            else if (type == typeof(byte)) value = (T)(object)Convert.ToByte(Value);
            else if (type == typeof(nint)) value = (T)(object)checked((nint)Convert.ToInt64(Value));
            else if (type == typeof(nuint)) value = (T)(object)checked((nuint)Convert.ToUInt64(Value));
            else return false;
            return true;
        }
        catch (Exception ex) when (ex is OverflowException or InvalidCastException or FormatException)
        {
            return false;
        }
    }
}
