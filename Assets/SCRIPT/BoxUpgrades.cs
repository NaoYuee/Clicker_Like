using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class BoxUpgrades : MonoBehaviour
{

    [Header("UI")]
    public TMP_Text _title;
    public TMP_Text _priceText;
    public TMP_Text _description;
    public Image _objectIcon;
    [SerializeField] private Image _upgradeBoxUI;
    public string _hiddenTitle;
    public string _hiddenDescription;
    private string _titleStored;
    private string _descriptionStored;
    [SerializeField] private Color _hiddenColor;

    [Header("Objects")]
    private GameObject _object;
    private SpriteRenderer _objectSpriteRenderer;
    public float _scaleFactor;

    [Header("Price")]
    private int _startPrice;
    int _price;
    public float _priceMultiplier;
    private int _upgradeLvl = 0;

    [Header("Upgrade")]
    private bool _isPurchased;

    public UpgradeType _upgradeType;

    private int _currentClickNum;
    private int _numberMax = 3;
    private int _addedClicks;
    private int _giftedClicks = 2;
    private int _giftedClicksTimer = 2;


    private float _timer;
    private float _timeGoal = 3f;


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
    Reset to 0 (numberAdd) once it has reached it's maximum (numberAddMax))
    */

    /* Dans update faire en sorte qu'il y est un temps à atteindre (_timeGoal) et une variable de timer en privée (_timer). */

    #endregion

    private void Start()
    {
        UpdateUI();
        if (_upgradeType == UpgradeType.UpgradePerClick)
        {
            GameManager.Instance._onClick += UpgradePerClicks;
        }
    }

    private void Update()
    {
        if (_isPurchased == true)
        {
            if (_upgradeType == UpgradeType.UpgradeTimer)
            {
                UpgradeWithTimer();
                ObjectAppear();
            }
            if (_upgradeType == UpgradeType.UpgradePerClick)
            {
                ObjectAppear();
            }
        }
    }

    public void InitializeObjects()
    {
        if (_object != null)
        {
            _objectSpriteRenderer = _object.GetComponent<SpriteRenderer>();
            Color _objectSpriteColor = _object.GetComponent<SpriteRenderer>().color;

            _objectSpriteColor.a = 0f;
            _objectSpriteRenderer.color = _objectSpriteColor;

        }
    }

    public void Initialize(UpgradeContent upgrade)
    {
        //Upgrade Content
        _title.text = upgrade._title;
        _titleStored = _title.text;

        _priceText.text = upgrade._priceText;
        _startPrice = int.Parse(_priceText.text);
        _description.text = upgrade._description;
        _descriptionStored = _description.text;

        _objectIcon.sprite = upgrade._objectIcon;
        _upgradeType = upgrade._upgradeType;

        //Upgrade Objects
        _object = upgrade._object;

        //Upgrade Type Values
        _timeGoal = upgrade._timeGoal;
        _giftedClicksTimer = upgrade._giftedClicksTimer;

        _numberMax = upgrade._numberMax;
        _giftedClicks = upgrade._giftedClicks;
    }

    private void UpdateUI()
    {
        _priceText.text = CalculatePrice().ToString();
        _isPurchased = _upgradeLvl > 0;
        if (_isPurchased == false)
        {
            _objectIcon.color = Color.black;
            _upgradeBoxUI.color = _hiddenColor;
            _title.text = _hiddenTitle;
            _description.text = _hiddenDescription;
        }
        else
        {
            _objectIcon.color = Color.white;
            _upgradeBoxUI.color = Color.white;
            _title.text = _titleStored;
            _description.text = _descriptionStored;
        }
    }

    private int CalculatePrice()
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

    private void ObjectAppear()
    {

        Sequence _sequence = DOTween.Sequence();

        _sequence.Join(_objectSpriteRenderer.DOFade(1f, 0.5f));
        _sequence.Join(_object.transform.DOScale(_scaleFactor, 0.3f).SetEase(Ease.InOutBounce));


    }

    #region Upgrades
    private void UpgradeWithTimer()
    {
        _timer += Time.deltaTime;
        if (_timer >= _timeGoal)
        {
            _timer = 0;
            GameManager.Instance.AddClicks(_giftedClicksTimer, false);
        }
    }

    public void UpgradePerClicks()
    {
        if (_isPurchased == true)
        {
            _currentClickNum++;

            if (_currentClickNum >= _numberMax)
            {
                _currentClickNum = 0;
                GameManager.Instance.AddClicks(_giftedClicks, false);
            }
        }
    }

    #endregion
}
