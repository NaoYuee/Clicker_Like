using System;
using System.Collections.Generic;
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
    public UpgradeObject _upgradeObject;

    [Header("Objects")]
    public GameObject _object;

    [Header("Values")]
    public List<ValueUpgrades> _valueList;
}
