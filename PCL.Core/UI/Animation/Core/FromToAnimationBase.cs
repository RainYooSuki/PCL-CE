using System;
using System.Threading;
using System.Threading.Tasks;
using PCL.Core.UI.Animation.Animatable;
using PCL.Core.UI.Animation.Easings;
using PCL.Core.UI.Animation.ValueProcessor;

namespace PCL.Core.UI.Animation.Core;

public class FromToAnimationBase<T> : AnimationBase, IFromToAnimation
{
    public IEasing Easing { get; set; } = new LinearEasing();

    // default(T) is the intentional "not provided" sentinel consumed by _RunCore.
    public T From { get; set; } = default!;

    public T? To { get; set; }
    public AnimationValueType ValueType { get; set; } = AnimationValueType.Absolute;
    public TimeSpan Duration { get; set; }
    public TimeSpan Delay { get; set; }
    public T? CurrentValue { get; internal set; }

    object? IFromToAnimation.CurrentValue
    {
        get => CurrentValue;
        set
        {
            if (value is not T typed)
                throw new InvalidCastException(
                    $"无法将 {(value is null ? "null" : value.GetType().ToString())} 转换为 {typeof(T)}");
            CurrentValue = typed;
        }
    }

    public int TotalFrames { get; private set; }
    
    private int _currentFrame;

    public override int CurrentFrame
    {
        get => Interlocked.CompareExchange(ref _currentFrame, 0, 0);
        set => Interlocked.Exchange(ref _currentFrame, value);
    }
    
    private T? _startValue;

    public override async Task<IAnimation> RunAsync(IAnimatable target)
    {
        _RunCore(target);
        var clone = (FromToAnimationBase<T>)MemberwiseClone();

        // 延迟
        await Task.Delay(Delay);

        // 将该动画推送到动画服务
        await AnimationService.PushAnimationAsync(clone, target);

        return clone;
    }

    public override IAnimation RunFireAndForget(IAnimatable target)
    {
        _RunCore(target);
        var clone = (FromToAnimationBase<T>)MemberwiseClone();

        _ = Task.Run(async () =>
        {
            // 延迟
            await Task.Delay(Delay);

            // 将该动画推送到动画服务
            AnimationService.PushAnimationFireAndForget(clone, target);
        });
        
        return clone;
    }

    private void _RunCore(IAnimatable target)
    {
        // 重置当前帧
        _currentFrame = 0;

        // 空值检查
        ArgumentNullException.ThrowIfNull(To);

        // 记录初始值
        var targetValue = target.GetValue();
        if (targetValue is not T startValue)
            throw new InvalidCastException(
                $"无法将 {(targetValue is null ? "null" : targetValue.GetType().ToString())} 转换为 {typeof(T)}");
        _startValue = startValue;

        // 如果 From 为空，则根据动画值类型设置初始值
        if (!ValueProcessorManager.Equal(startValue, From))
        {
           From = ValueType == AnimationValueType.Relative ? ValueProcessorManager.DefaultValue<T>() : startValue;
        }

        // 计算总帧数
        TotalFrames = (int)Math.Round(Duration.TotalSeconds * AnimationService.Fps / AnimationService.Scale);

        // 进行初始赋值
        // target.SetValue(
        //     ValueType == AnimationValueType.Relative ? ValueProcessorManager.Add(From, _startValue)! : From!);
        
        // 设置状态
        Status = AnimationStatus.Running;
    }

    public override void Cancel()
    {
        // 确保正常结束
        Interlocked.Exchange(ref _currentFrame, TotalFrames);

        Status = AnimationStatus.Canceled;
    }

    public override IAnimationFrame? ComputeNextFrame(IAnimatable target)
    {
        if (_currentFrame >= TotalFrames)
        {
            Status = AnimationStatus.Completed;
            return null;
        }
        
        if (CurrentValue is not { } currentValue)
            throw new InvalidOperationException("The current animation value is not initialized.");
        if (From is not { } fromValue)
            throw new InvalidOperationException("The animation source value is not initialized.");
        if (_startValue is not { } startValue)
            throw new InvalidOperationException("The animation start value is not initialized.");

        return new FromToAnimationFrame<T>
        {
            Target = target,
            Value = ValueType == AnimationValueType.Relative
                ? currentValue
                : ValueProcessorManager.Subtract(currentValue, fromValue),
            StartValue = ValueType == AnimationValueType.Relative ? startValue : fromValue
        };
    }
}