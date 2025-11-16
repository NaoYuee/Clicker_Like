using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using DG.Tweening;

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
    private Transform _clickObjects;
    private Vector3 _mousePos;
    private RaycastHit2D _raycastHit2D;
    [SerializeField] private LayerMask _objectsLayerMask;

    [Header("Animations")]
    public List<AnimationHandler> _animations;

    [Header("Upgrades")]
    public List<UpgradeContent> _upgrades;
    public GameObject _upgradePrefabUI;
    public GameObject _parentUpgrades;
    public Action _onClick;

    [Header("Feedbacks")]
    public NewItems _newItems;

    public ParticleSystem _riceParticle;

    public RectTransform _boardTransform;
    private Vector3 _originalBoardScale;
    private Vector3 _boardScaleTo;

    public RectTransform _onigiriTransform;
    private Vector3 _originalScale;
    private Vector3 _onigiriScaleTo;
    private Vector3 _onigiriRotation;
    private Vector3 _originalRotation;
    public float _onigiriRotateTo;
    public float _scaleMultiplier;

    [Header("Sound")]
    private List<string> _audioList = new List<string>();

    public void Start()
    {
        _clickGauge.maxValue = _gaugeMax;

        foreach (var upgrade in _upgrades)
        {
            GameObject go = GameObject.Instantiate(_upgradePrefabUI, _parentUpgrades.transform, false);
            go.transform.localPosition = Vector3.zero;
            var _boxUpgrades = go.GetComponent<BoxUpgrades>();
            _boxUpgrades.Initialize(upgrade);
            _boxUpgrades.InitializeObjects();
        }

        #region Onigiri Anim
        _originalScale = _onigiriTransform.localScale;
        _onigiriScaleTo = _originalScale * _scaleMultiplier;

        _originalBoardScale = _boardTransform.localScale;
        _boardScaleTo = _originalBoardScale * 1.2f;

        _originalRotation = _onigiriTransform.rotation.eulerAngles;
        _onigiriRotation = new Vector3(0f, 0f, _onigiriRotateTo);
        #endregion

        AddAudioToList();
    }

    private void Update()
    {
        _mousePos = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 _worldPos = Camera.main.ScreenToWorldPoint(_mousePos);
            Vector2 _point = new Vector2(_worldPos.x, _worldPos.y);

            RaycastHit2D hit = Physics2D.Raycast(_point, Vector2.zero, 0f, _objectsLayerMask);

            if (hit.collider != null)
            {
                AddClicks(1);
            }
        }
    }

    public void Awake()
    {
        Instance = this;
    }

    public void AddAudioToList()
    {
        _audioList.Add("Bloop Character");
        _audioList.Add("Chopping Board");
    }

    public void ClickAnimation()
    {
        foreach (var animations in _animations)
        {
            animations._animator.Play(animations._animationClip);
        }
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
        FeedbackItem();
        ClickAnimation();
        AudioManager.Instance.PlayGroupSFX(_audioList);
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
            _gaugeProgression++;
            _clickGauge.value = _gaugeProgression;
        }
    }

    private void FeedbackItem()
    {
        Sequence _sequence = DOTween.Sequence();
        _sequence.Append(_onigiriTransform.DOScale(_onigiriScaleTo, 0.1f).SetEase(Ease.InOutFlash));
        _sequence.Join(_onigiriTransform.DORotate(_onigiriRotation, 0.1f, RotateMode.Fast).SetEase(Ease.OutQuad));
        _sequence.Join(_boardTransform.DOScale(_boardScaleTo, 0.1f).SetEase(Ease.OutFlash));

        _sequence.Append(_onigiriTransform.DOScale(_originalScale, 0.1f).SetEase(Ease.InOutFlash));
        _sequence.Join(_onigiriTransform.DORotate(_originalRotation, 0.1f, RotateMode.Fast).SetEase(Ease.OutQuad));
        _sequence.Join(_boardTransform.DOScale(_originalBoardScale, 0.1f).SetEase(Ease.OutFlash));

        _riceParticle.Play();


        if (_gaugeProgression == _gaugeMax)
        {
            _newItems.PanelFadeIn();
            StartCoroutine(Fade());
        }
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(2f);
        _newItems.PanelFadeOut();
    }
}
