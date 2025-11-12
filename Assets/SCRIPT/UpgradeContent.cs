using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UpgradeContent
{
    [Header("Visuals")]
    public string _title;
    public string _priceText;
    public string _description;
    public Sprite _objectIcon;
    public UpgradeType _upgradeType;
}
