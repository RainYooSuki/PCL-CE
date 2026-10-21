using System;
using System.Collections.Frozen;

namespace PCL.Core.UI.Animation.Core;

public record AnimationData<T> : IAnimationData
{
    public required IAnimation Animation { get; init; }
    public required FrozenDictionary<int, T> Values { get; init; }
}