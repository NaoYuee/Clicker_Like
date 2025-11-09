using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    [Header("Number of Clicks")]
    public TMP_Text _clickText;
    public int _clicks;

    [Header("Gauge Progession")]
    public Slider _clickGauge;
    public float _gaugeProgression;
    public int _gaugeMax = 10;
    public int _gaugeMultiplier;

    [Header("Clickable Objects")]
    Transform _clickObjects;
    Vector3 _mousePos;
    RaycastHit2D _raycastHit2D;

    [Header("Upgrades")]
    public List<UpgradeContent> _upgrades;
    public GameObject _upgradePrefabUI;
    public GameObject _parentUpgrades;


    public void Start()
    {
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
                addClicks();
            }
        }
    }

    public void Awake()
    {
        Instance = this;
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

    public void UpdateCounter()
    {
        _clickText.text = _clicks.ToString();
    }

    public void addClicks()
    {
        _clicks++;    
        UpdateCounter();
        Gauge();
    }

    public void Gauge()
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
