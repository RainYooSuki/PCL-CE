using System;
using System.Diagnostics.CodeAnalysis;
using PCL.Core.Utils;

namespace PCL.Core.App.Configuration;

public class ArgConfig<TValue> : ParameterizedProperty<object, TValue>
{
    [SetsRequiredMembers]
    public ArgConfig(Func<object?, TValue> getter, Action<object?, TValue> setter)
    {
        GetValue = getter;
        SetValue = setter;
    }
}
