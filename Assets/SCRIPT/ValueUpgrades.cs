using System;
using UnityEngine;

[Serializable]

public class ValueUpgrades 
{
    [Header("Time")]
    public float _timeGoal;
    public int _giftedClicksTimer;

    [Header("Click")]
    public int _numberMax;
    public int _giftedClicks;
}
