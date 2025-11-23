using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoxUpgrades : MonoBehaviour
{

    [Header("UI")]
    public TMP_Text _title;
    public string _hiddenTitle;
    private string _titleStored;

    public TMP_Text _description;
    public string _hiddenDescription;
    private string _descriptionStored;
    public TMP_Text _canUpgradeText;
    [SerializeField] private Color _boxOverlayColor;
    [SerializeField] private Image _boxOverlay;

    public TMP_Text _priceText;
    public Image _objectIcon;
    [SerializeField] private Image _upgradeBoxUI;
    [SerializeField] private Color _hiddenColor;

    [Header("Objects")]
    private GameObject _object;
    private SpriteRenderer _objectSpriteRenderer;
    public float _scaleFactor;

    [Header("Feedback")]
    private RectTransform _uiBoxUpgrade;
    private Button _uiBoxButton;
    public Color _uiBoxColor;

    public float _strength;
    public float _duration;
    public int _vibrato;

    [Header("Price")]
    private int _startPrice;
    int _price;
    public float _priceMultiplier;
    private int _upgradeLvl = 0;
    private int _startUpgradeLvl = 0;

    [Header("Upgrade")]
    private bool _isPurchased;

    private UpgradeContent _allUpgrades;

    [SerializeField] private UpgradeType _upgradeType;
    [SerializeField] private UpgradeObject _upgradeObject;

    private int _currentClickNum;
    private int _numberMax = 3;
    private int _giftedClicks = 2;


    private int _giftedClicksTimer = 2;
    private float _timer;
    private float _timeGoal = 3f;


    private void Start()
    {
        UpdateUI();


        _uiBoxUpgrade = this.GetComponent<RectTransform>();
        _uiBoxButton = this.GetComponent<Button>();

        _canUpgradeText.color = _uiBoxColor;
        _boxOverlayColor = _boxOverlay.color;
        _boxOverlayColor.a = 0f;
        _boxOverlay.color = _boxOverlayColor;

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


        _objectIcon.sprite = upgrade._objectIcon;
        _upgradeType = upgrade._upgradeType;
        _upgradeObject = upgrade._upgradeObject;

        _descriptionStored = DescriptionInitialize();
        _description.text = _descriptionStored;

        //Upgrade Objects
        _object = upgrade._object;

        _numberMax = upgrade._valueList[0]._numberMax;
        _giftedClicks = upgrade._valueList[0]._giftedClicks;
        _timeGoal = upgrade._valueList[0]._timeGoal;
        _giftedClicksTimer = upgrade._valueList[0]._giftedClicksTimer;

        _allUpgrades = upgrade;
    }

    public string DescriptionInitialize()
    {
        if (_upgradeType == UpgradeType.UpgradePerClick)
        {
            return $"Eat {_numberMax} onigiri to get {_giftedClicks} free";
        }
        if (_upgradeType == UpgradeType.UpgradeTimer)
        {
            return $"{_giftedClicksTimer} onigiris every {_timeGoal} seconds";
        }
        return "ERROR";
    }

    public void UpdateDescription(UpgradeContent upgrade)
    {
        if (upgrade._valueList.Count >= _upgradeLvl)
        {
            _numberMax = upgrade._valueList[_upgradeLvl - 1]._numberMax;
            _giftedClicks = upgrade._valueList[_upgradeLvl - 1]._giftedClicks;
            _timeGoal = upgrade._valueList[_upgradeLvl - 1]._timeGoal;
            _giftedClicksTimer = upgrade._valueList[_upgradeLvl - 1]._giftedClicksTimer;

            _descriptionStored = DescriptionInitialize();
            _description.text = _descriptionStored;
            _canUpgradeText.gameObject.SetActive(true);
        }
        else
        {
            _boxOverlayColor.a = 0.5f;
            _boxOverlay.color = _boxOverlayColor;
            _canUpgradeText.gameObject.SetActive(false);
            _uiBoxButton.interactable = false;
        }
    }

    private void UpdateUI()
    {
        _priceText.text = CalculatePrice().ToString();
        _isPurchased = _upgradeLvl > _startUpgradeLvl;
        if (_isPurchased == false)
        {
            _canUpgradeText.gameObject.SetActive(false);
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
            AudioManager.Instance.PlaySFX("Plate");
            _upgradeLvl++;
            UpdateDescription(_allUpgrades);
            UpdateUI();
            UIFeedback();
        }
    }

    private void ObjectAppear()
    {

        Sequence _sequence = DOTween.Sequence();

        _sequence.Join(_objectSpriteRenderer.DOFade(1f, 0.5f));
        _sequence.Join(_object.transform.DOScale(_scaleFactor, 0.3f).SetEase(Ease.InOutBounce));

        AddSFXList(UpgradeObject.Wasabi, "Wasabi");
        AddSFXList(UpgradeObject.Ginger, "Ginger");
        AddSFXList(UpgradeObject.SoySauce, "Soy Sauce");
    }


    private void AddSFXList(UpgradeObject _object, string _sfxName)
    {
        if (_upgradeObject == _object)
        {
            GameManager.Instance._audioList.Add(_sfxName);
        }
    }
    private void UIFeedback()
    {
        _uiBoxUpgrade.DOComplete();
        _uiBoxUpgrade.DOShakeScale(_duration, _strength, _vibrato, 0.2f, true, ShakeRandomnessMode.Harmonic).SetEase(Ease.InOutSine);
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

    public void UpgradePerObjects()
    {

    }

    #endregion
}
