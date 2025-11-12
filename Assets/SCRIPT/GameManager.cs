using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    [Header("Number of Clicks")]
    public TMP_Text _clickText;
    public int _clicks;

    [Header("Gauge Progession")]
    public Slider _clickGauge;
    private float _gaugeProgression;
    private int _gaugeMax = 10;
    public int _gaugeMultiplier;

    [Header("Clickable Objects")]
    Transform _clickObjects;
    Vector3 _mousePos;
    RaycastHit2D _raycastHit2D;

    [Header("Animations")]
    public Animator _charaAnimator;


    [Header("Upgrades")]
    public List<UpgradeContent> _upgrades;
    public GameObject _upgradePrefabUI;
    public GameObject _parentUpgrades;
    public Action _onClick;


    public void Start()
    {
        /*_onClick += CharaAnimation;*/
        _clickGauge.maxValue = _gaugeMax;

        foreach (var upgrade in _upgrades)
        {
            GameObject go = GameObject.Instantiate(_upgradePrefabUI, _parentUpgrades.transform, false);
            go.transform.localPosition = Vector3.zero;
            var _boxUpgrades = go.GetComponent<BoxUpgrades>();
            _boxUpgrades.Initialize(upgrade);
        }

    }

    private void Update()
    {
        _mousePos = Input.mousePosition;

        Ray mouseRay = Camera.main.ScreenPointToRay(_mousePos);

        if (Input.GetMouseButtonDown(0))
        {
            _raycastHit2D = Physics2D.Raycast(mouseRay.origin, mouseRay.direction);
            _clickObjects = _raycastHit2D ? _raycastHit2D.collider.transform : null;

            if (_clickObjects)
            {
                AddClicks(1);
                CharaAnimation();
            }
        }
    }

    public void Awake()
    {
        Instance = this;
    }

    public void CharaAnimation()
    {
        Debug.Log("ola");
        /*_charaAnimator.SetTrigger("_isClicked");*/
        _charaAnimator.Play("ANIM_LittleChara_Squidge", 0, 0f);
    }

    public bool PurchaseAction(int cost)
    {
        if (_clicks >= cost)
        {
            _clicks -= cost;
            UpdateCounter();
            return true;
        }
        return false;
    }

    private void UpdateCounter()
    {
        _clickText.text = $"x {_clicks.ToString()}";
    }

    public void AddClicks(int _addedClicks, bool _isPlayerClick = true)
    {
        _clicks = _clicks + _addedClicks;
        UpdateCounter();
        Gauge();
        if (_isPlayerClick)
        {
            _onClick.Invoke();
        }
    }

    private void Gauge()
    {
        if (_gaugeProgression % _gaugeMax == 0 && _gaugeProgression != 0)
        {
            _gaugeMax = _gaugeMax * _gaugeMultiplier;
            _clickGauge.maxValue = _gaugeMax;
            _gaugeProgression = 0;
            _clickGauge.value = _gaugeProgression;
        }
        else
        {
            _gaugeProgression++; //problem cause the value of _clicks does not go back to zero when _gaugeProgression is reinitialized. What to do?
            _clickGauge.value = _gaugeProgression;
        }
    }
}
