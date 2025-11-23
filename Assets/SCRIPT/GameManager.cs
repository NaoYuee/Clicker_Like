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
    [SerializeField] private float _gaugeProgression;
    [SerializeField] private int _gaugeMax = 10;
    public int _gaugeMultiplier;

    [Header("Clickable Objects")]
    private Transform _clickObjects;
    private Vector3 _mousePos;
    private RaycastHit2D _raycastHit2D;
    [SerializeField] private LayerMask _objectsLayerMask;
    [SerializeField] private LayerMask _itemLayerMask;

    [Header("Animations")]
    public List<AnimationHandler> _animations;
    private int _rndSelectionAnim;
    public Animator _mouseAnimatorLeft;
    public Animator _mouseAnimatorRight;
    private AnimationClip _mouseClip;
    public GameObject _clickLeft;
    public GameObject _clickRight;

    [Header("Upgrades")]
    public List<UpgradeContent> _upgrades;
    public GameObject _upgradePrefabUI;
    public GameObject _parentUpgrades;
    public Action _onClick;

    /*    [Header("Items")]
        public List<>*/

    [Header("Feedbacks")]
    public NewItemsAnim _newItems;
    public ItemsCollection[] _itemsCollections;

    public ParticleSystem _riceParticle;
    public ParticleSystem _achievementParticle;
    public ParticleSystem _clickParticle;
    public ParticleSystem _changeParticle;
    private bool _particleUnlocked = false;

    public RectTransform _boardTransform;
    private Vector3 _originalBoardScale;
    private Vector3 _boardScaleTo;

    public RectTransform _onigiriTransform;
    private Vector3 _originalScale;
    private Vector3 _onigiriScaleTo;
    private Vector3 _onigiriRotation;
    private Vector3 _originalRotation;
    private float _onigiriRotateTo = -10f;
    private float _scaleMultiplier = 1.5f;

    public RectTransform _scrollArrow;


    [Header("Sound")]
    [HideInInspector] public List<string> _audioList = new List<string>();

    public void Start()
    {

        Screen.SetResolution(1920, 1080, true);

        BounceArrow();

        _clickGauge.maxValue = _gaugeMax;

        foreach (var upgrade in _upgrades)
        {
            GameObject go = GameObject.Instantiate(_upgradePrefabUI, _parentUpgrades.transform, false);
            go.transform.localPosition = Vector3.zero;
            var _boxUpgrades = go.GetComponent<BoxUpgrades>();
            _boxUpgrades.Initialize(upgrade);
            _boxUpgrades.InitializeObjects();
        }

        _mouseAnimatorLeft.Play("ANIM_Mouse_Left");
        _mouseAnimatorLeft.SetBool("_stopLeft", false);
        _mouseAnimatorRight.SetBool("_stopRight", false);
        _clickRight.SetActive(false);

        _changeParticle.Stop();


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

            _clickParticle.transform.position = _point;
            _clickParticle.Play();

            RaycastHit2D hit = Physics2D.Raycast(_point, Vector2.zero, 0f, _objectsLayerMask);

            if (hit.collider != null)
            {
                _clickParticle.Stop();
                StopMouseAnim(_mouseAnimatorLeft, _clickLeft, 10f, "_stopLeft", true);
                AddClicks(1);
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 _worldPos = Camera.main.ScreenToWorldPoint(_mousePos);
            Vector2 _point = new Vector2(_worldPos.x, _worldPos.y);


            RaycastHit2D hit = Physics2D.Raycast(_point, Vector2.zero, 0f, _itemLayerMask);

            if (hit.collider != null)
            {
                ItemChange _scriptFound = hit.collider.GetComponent<ItemChange>();
                if (_scriptFound)
                {
                    StopMouseAnim(_mouseAnimatorRight, _clickRight, 3f, "_stopRight", true);
                    _scriptFound.ChangeToNextItem();
                    if (_particleUnlocked == true)
                    {
                        _changeParticle.transform.position = _point;
                        _changeParticle.Play();
                    }
                }
            }
        }
    }

    public void Awake()
    {
        Instance = this;
    }

    #region Animation Mouse
    private void StopMouseAnim(Animator _mouseAnimator, GameObject _clickObj, float _time, string _condition, bool _state)
    {
        StartCoroutine(StopWait(_time));
        _mouseAnimator.SetBool(_condition, _state);
        _clickObj.SetActive(false);
    }

    private IEnumerator StopWait(float _time)
    {
        yield return new WaitForSeconds(_time);
    }
    #endregion

    public void AddAudioToList()
    {
        _audioList.Add("Bloop Character");
        _audioList.Add("Chopping Board");
    }

    public void ClickAnimation()
    {
        foreach (var _animations in _animations)
        {
            if (_animations._rndAnimation == true)
            {
                _rndSelectionAnim = UnityEngine.Random.Range(0, _animations._animationNames.Count);
                _animations._animator.Play(_animations._animationNames[_rndSelectionAnim]);
            }
            if (_animations._rndAnimation == false)
            {
                _animations._animator.Play(_animations._animationTransitionName);
            }
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

    private void TutoRightClick()
    {
        if (_gaugeProgression == 29)
        {
            _clickRight.SetActive(true);
            _mouseAnimatorRight.SetBool("_isRight", true);
            _particleUnlocked = true;
        }
    }

    private void Gauge()
    {
        if (_gaugeProgression == _gaugeMax - 1)
        {
            AudioManager.Instance.PlaySFX("Bell Finish");
            _achievementParticle.Play();
            TutoRightClick();
        }
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

    private void BounceArrow()
    {

        Sequence _sequenceBounce = DOTween.Sequence().SetLoops(-1, LoopType.Yoyo);
        _sequenceBounce.Join(_scrollArrow.DOAnchorPosY(_scrollArrow.anchoredPosition.y + 8, 0.4f).SetEase(Ease.InOutSine));
        _sequenceBounce.Join(_scrollArrow.DOShakeScale(0.2f, 0.2f, 1, 0.2f, true, ShakeRandomnessMode.Harmonic).SetEase(Ease.InOutSine));
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
            _itemsCollections[0].AddSprite();
            _itemsCollections[1].AddSprite();
            _newItems.PanelFadeIn();
            StartCoroutine(Fade());
        }
    }

    private IEnumerator Fade()
    {
        yield return new WaitForSeconds(2.7f);
        _newItems.PanelFadeOut();
    }
}
