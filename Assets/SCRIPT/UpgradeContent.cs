using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UpgradeContent
{
    [Header("UI")]
    public string _title;
    public string _priceText;
    public string _description;
    public Sprite _objectIcon;

    [Header("Upgrade Type")]
    public UpgradeType _upgradeType;

    [Header("Objects")]
    public GameObject _object;

    [Header("Values")]
    public float _timeGoal;
    public int _giftedClicksTimer;
    public int _numberMax;
    public int _giftedClicks;
}
