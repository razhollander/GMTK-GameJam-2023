using UnityEngine;

namespace Core.Extensions
{
    public class WaitForAnimationEnd : CustomYieldInstruction
    {
        private const float MaxAnimationNormalizedTime = 1;
        private const int DefaultAnimationLayerIndex = 0;

        private readonly Animator _animator;
        private readonly int _layerIndex;
        private readonly string _animationTag;
        
        public override bool keepWaiting => _animator != null && CorrectAnimationIsPlaying && !AnimationIsDone;

        private AnimatorStateInfo StateInfo => _animator != null ? _animator.GetCurrentAnimatorStateInfo(_layerIndex) : default;
        private bool AnimationIsDone => StateInfo.normalizedTime >= MaxAnimationNormalizedTime;
        private bool CorrectAnimationIsPlaying => StateInfo.IsName(_animationTag);

        public WaitForAnimationEnd(Animator animator, string animationTag, int layerIndex = DefaultAnimationLayerIndex)
        {
            _animator = animator;
            _animationTag = animationTag;
            _layerIndex = layerIndex;
        }
    }
}