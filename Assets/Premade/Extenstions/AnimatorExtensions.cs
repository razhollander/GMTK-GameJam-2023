using System.Collections.Generic;
using UnityEngine;

public static class AnimatorExtensions
{
    private static readonly Dictionary<string, HashSet<int>> _animatorCachedParametersHashDictionary = new Dictionary<string, HashSet<int>>();

    public static bool HasParameterOfType(this Animator self, int hash, AnimatorControllerParameterType type)
    {
        if (self == null || !self.isActiveAndEnabled || !self.gameObject.activeInHierarchy)
        {
            return false;
        }

        // Get animator controller unique name
        var animatorControllerName = self.runtimeAnimatorController.name;

        HashSet<int> cachedHashSet;

        // Try get the int hashset of the target animator
        if (!_animatorCachedParametersHashDictionary.TryGetValue(animatorControllerName, out cachedHashSet))
        {
            // If doesn't exist yet, initialize the cached hash set
            cachedHashSet = new HashSet<int>();

            // Add it to the dictionary
            _animatorCachedParametersHashDictionary.Add(animatorControllerName, cachedHashSet);
        }

        // If this hash is not yet cached, check if it's part of this animator parameters
        if (!cachedHashSet.Contains(hash))
        {
            var parameters = self.parameters;
            var parametersCount = parameters.Length;

            // Iterate over each parameter and try to find this hash
            for (int parameterIndex = 0; parameterIndex < parametersCount; parameterIndex++)
            {
                if (parameters[parameterIndex].type == type && parameters[parameterIndex].nameHash == hash)
                {
                    // If found, add it and return true
                    cachedHashSet.Add(hash);

                    return true;
                }
            }
        }

        // Else, this has is already contained, immediately return true
        else
        {
            return true;
        }

        return false;
    }

    public static bool TrySetTrigger(this Animator self, int triggerHash)
    {
        if (self == null || !self.isActiveAndEnabled || !self.gameObject.activeInHierarchy)
        {
            return false;
        }

        if (!self.HasParameterOfType(triggerHash, AnimatorControllerParameterType.Trigger))
        {
            return false;
        }

        self.SetTrigger(triggerHash);

        return true;
    }

    public static bool TryResetTrigger(this Animator self, int triggerHash)
    {
        if (!self.isActiveAndEnabled || !self.gameObject.activeInHierarchy)
        {
            return false;
        }

        if (!self.HasParameterOfType(triggerHash, AnimatorControllerParameterType.Trigger))
        {
            return false;
        }

        self.ResetTrigger(triggerHash);

        return true;
    }

    public static bool TrySetFloat(this Animator self, int floatHash, float value)
    {
        if (self == null || !self.isActiveAndEnabled || !self.gameObject.activeInHierarchy)
        {
            return false;
        }

        if (!self.HasParameterOfType(floatHash, AnimatorControllerParameterType.Float))
        {
            return false;
        }

        self.SetFloat(floatHash, value);

        return true;
    }

    public static bool TrySetBool(this Animator self, int boolHash, bool value)
    {
        if (self == null || !self.isActiveAndEnabled || !self.gameObject.activeInHierarchy)
        {
            return false;
        }

        if (!self.HasParameterOfType(boolHash, AnimatorControllerParameterType.Bool))
        {
            return false;
        }

        self.SetBool(boolHash, value);

        return true;
    }
}