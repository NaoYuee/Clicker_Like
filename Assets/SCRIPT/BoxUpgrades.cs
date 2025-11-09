using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class BoxUpgrades : MonoBehaviour
{

    [Header("Visuals")]
    public TMP_Text _title;
    public TMP_Text _priceText;
    public TMP_Text _description;
    public Image _objectIcon;
    public Image _upgradeBoxUI;
    public string _hiddenTitle;
    public string _hiddenDescription;

    [Header("Price")]
    public int _startPrice;
    int _price;
    public float _priceMultiplier;
    int _upgradeLvl = 0;

    /*    [Header("Upgrade")]
        public TMP_Text _clickCount;
        public int _currentClickNum;
        public int _numberAdd;
        public int _addedClicks;*/
    bool _isPurchased;


    #region to do 
    /*
    starting price (+ text visual)
    When bought lvl goes up by one and the price goes up by price multiplier (+ text visual)
    each upgrade has a different functionnality:
    1 click per second -- will have to create a counter that keeps track of the time
    2 clicks after 3 clicks -- will have to keep track of the number of clicks since the moment of the upgrade (number of clicks +3) and then once 3 clicks is obtained set it back to 0
    5 clicks once every 10 clicks -- same as previous
    20 clicks once every 50 clicks
    */

    /*
    Once the upgrade is bought start counting (numberAdd) from (currentClickNum) and once that numberAdd reaches the max add (addClicks).
    Reset to 0 (numberAdd) once it has reached it's maximum)
     */
    #endregion

    private void Start()
    {
        UpdateUI();
    }

    public void Initialize(UpgradeContent upgrade)
    {
        _title.text = upgrade._title;
        _priceText.text = upgrade._priceText;
        _startPrice = int.Parse(_priceText.text);
        _description.text = upgrade._description;
        _objectIcon.sprite = upgrade._objectIcon;
    }

    public void UpdateUI()
    {
        _priceText.text = CalculatePrice().ToString();
        bool _isPurchased = _upgradeLvl> 0;
        if (_isPurchased == false)
        {
            _objectIcon.color = Color.black;
            _upgradeBoxUI.color = new Color(0x8A,0x58,0x58,0xFF) ; //Color won't change
            _hiddenTitle = "???";
            _hiddenDescription = "Not yet available";
        }
        else
        {
            _objectIcon.color = Color.white;
            _upgradeBoxUI.color = Color.white;
        }
    }

    int CalculatePrice()
    {
        _price = Mathf.RoundToInt(_startPrice * Mathf.Pow(_priceMultiplier, _upgradeLvl));
        return _price;
    }

    public void ClickAction()
    {
        _price = CalculatePrice();
        bool purchaseSuccess = GameManager.Instance.PurchaseAction(_price);
        if (purchaseSuccess)
        {
            _upgradeLvl++;
            UpdateUI();
        }
    }

    /*    public void UpgradePerClicks(int _numberAdd, int _addedClicks)
        {
            if (_isPurchased == true)
            {
                _isPurchased = false;
                //Change visuals to new upgrade
                int.TryParse(_clickCount.text, out _currentClickNum);
                while (_isPurchased = false)
                {
                    for (int i = 0; i < _numberAdd; i++)
                    {
                        _currentClickNum++;
                    }
                    _currentClickNum = _currentClickNum + _addedClicks;
                    //change tmp_text into new value
                }
            }
        }*/

    /* public void BuyBox()
     {
         int _price = 
     }*/
}
