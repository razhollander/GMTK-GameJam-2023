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
	
	public static async UniTask CrossFade(this Animation animation, string animationName, float crossFadeDuration)
        {
            try
            {
                while (crossFadeDuration > 0)
                {
                    animation.CrossFade(animationName, crossFadeDuration, PlayMode.StopSameLayer);
                    crossFadeDuration -= Time.deltaTime;
                    await UniTask.Yield();
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to play animation: {animationName}, {e}");
            }
        }

        public static float GetAnimationLength(this Animation animation, string animationName)
        {
            var animationState = animation[animationName];
            return animationState.length * (1 / Mathf.Abs(animationState.speed));
        }
        
        public static float GetClipLength(this Animation animation, string clipName)
        {
            return animation.GetClip(clipName).length;
        }
}