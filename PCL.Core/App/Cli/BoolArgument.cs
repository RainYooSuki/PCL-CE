using System.Diagnostics.CodeAnalysis;

namespace PCL.Core.App.Cli;

public class BoolArgument : CommandArgument<bool>
{
    public override ArgumentValueKind ValueKind => ArgumentValueKind.Bool;

    protected override bool ParseValueText()
    {
        var text = ValueText.ToLowerInvariant().Trim();
        return text is not ("0" or "false");
    }

    public override bool TryCastValue<T>([NotNullWhen(true), MaybeNullWhen(false)] out T value)
    {
        if (base.TryCastValue<T>(out value)) return true;
        var type = typeof(T);
        if (type != typeof(sbyte) &&
            type != typeof(byte) &&
            type != typeof(short) &&
            type != typeof(ushort) &&
            type != typeof(int) &&
            type != typeof(uint) &&
            type != typeof(long) &&
            type != typeof(ulong) &&
            type != typeof(nint) &&
            type != typeof(nuint)) return false;
        var numericValue = Value ? 1L : 0L;
        if (type == typeof(sbyte)) value = (T)(object)(sbyte)numericValue;
        else if (type == typeof(byte)) value = (T)(object)(byte)numericValue;
        else if (type == typeof(short)) value = (T)(object)(short)numericValue;
        else if (type == typeof(ushort)) value = (T)(object)(ushort)numericValue;
        else if (type == typeof(int)) value = (T)(object)(int)numericValue;
        else if (type == typeof(uint)) value = (T)(object)(uint)numericValue;
        else if (type == typeof(long)) value = (T)(object)numericValue;
        else if (type == typeof(ulong)) value = (T)(object)(ulong)numericValue;
        else if (type == typeof(nint)) value = (T)(object)(nint)numericValue;
        else value = (T)(object)(nuint)numericValue;
        return true;
    }
}
