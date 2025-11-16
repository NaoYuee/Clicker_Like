using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class AnimationHandler
{
    public Animator _animator;
    public string _animationTransitionName;
    public bool _rndAnimation = false;
    public List<string> _animationNames;
}
