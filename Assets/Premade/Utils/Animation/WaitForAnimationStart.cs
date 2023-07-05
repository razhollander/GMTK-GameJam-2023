using UnityEngine;

namespace Core.Extensions
{
    public class WaitForAnimationStart : CustomYieldInstruction
    {
        private const int DefaultAnimationLayerIndex = 0;
        private readonly Animator _animator;
        private readonly int _layerIndex;

        private readonly string _animationTag;

        public override bool keepWaiting => _animator != null && !CorrectAnimationIsPlaying;

        private AnimatorStateInfo StateInfo => _animator.GetCurrentAnimatorStateInfo(_layerIndex);
        private bool CorrectAnimationIsPlaying => StateInfo.IsName(_animationTag);

        public WaitForAnimationStart(Animator animator, string animationTag, int layerIndex = DefaultAnimationLayerIndex)
        {
            _animator = animator;
            _animationTag = animationTag;
            _layerIndex = layerIndex;
        }
    }
}