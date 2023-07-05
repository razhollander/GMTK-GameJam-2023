using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public static class AnimationExtensions
{
    public static UniTask PlayAndWait(this Animation animation, string clipName)
    {
        try
        {
            animation.Play(clipName);
            var animationTime = animation[clipName].length * (1 / Mathf.Abs(animation[clipName].speed));

            return UniTask.Delay(TimeSpan.FromSeconds(animationTime));
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to play clip: {clipName}, {e}");

            return UniTask.CompletedTask;
        }
    }

    public static UniTask PlayAndWait(this Animation animation, string clipName, float percentageToAwait)
    {
        try
        {
            animation.Play(clipName);
            var animationTime = animation[clipName].length * (1 / Mathf.Abs(animation[clipName].speed));
            var timeToWait = animationTime * percentageToAwait;

            return UniTask.Delay(TimeSpan.FromSeconds(timeToWait));
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to play clip: {clipName}, {e.ToString()}");

            return UniTask.CompletedTask;
        }
    }
}