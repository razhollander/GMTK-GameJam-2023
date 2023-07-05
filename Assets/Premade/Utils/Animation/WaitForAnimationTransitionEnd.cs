using UnityEngine;

namespace Utils.Animation
{
    public class WaitForAnimationTransitionEnd : CustomYieldInstruction
    {
        private const int DefaultAnimationLayerIndex = 0;
        private readonly Animator _animator;
        private readonly int _layerIndex;
        private readonly string _transitionName;

        public override bool keepWaiting =>  _animator != null && IsCorrectTransitionNamePlaying && IsInTransition;

        private AnimatorTransitionInfo TransitionInfo => _animator.GetAnimatorTransitionInfo(_layerIndex);
        private bool IsCorrectTransitionNamePlaying => _transitionName == null || TransitionInfo.IsName(_transitionName);
        private bool IsInTransition => _animator.IsInTransition(_layerIndex);

        public WaitForAnimationTransitionEnd(Animator animator, string transitionName = null, int layerIndex = DefaultAnimationLayerIndex)
        {
            _animator = animator;
            _transitionName = transitionName;
            _layerIndex = layerIndex;
        }
    }
}