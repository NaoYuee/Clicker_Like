using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemsCollection : MonoBehaviour
{
    [Header("Items Collection")]
    private List<ItemSceneHandler> _itemSceneHandler;
    /*private List<Sprite> _itemsCollection = new List<Sprite>();*/

    public GameObject _itemBox;
    private GameObject _itemBoxChild;
    private Image _itemImageComponent;
    private ItemSceneHandler _selectedListElement;
    private RectTransform _itemPos;
    private int _rndSelection;
    public TMP_Text _apologies;
    private Color _apologiesColor;

    public ItemChange _itemChange;

    [Header("Feedback")]
    private RectTransform _itemBoxTransform;
    public float _strength;
    public float _duration;
    public int _vibrato;

    private void Start()
    {
        _itemSceneHandler = _itemChange._itemSceneHandler;

        _itemBoxChild = _itemBox.transform.GetChild(0).gameObject;
        _itemImageComponent = _itemBoxChild.GetComponent<Image>();
        _apologiesColor = _apologies.color;
        _apologiesColor.a = 0f;
        _apologies.color = _apologiesColor;
        _itemBox.SetActive(true);
    }

    public ItemSceneHandler ChooseRnd()
    {
        _rndSelection = Random.Range(0, _itemSceneHandler.Count);
        if (_rndSelection < 0 || _rndSelection >= _itemSceneHandler.Count)
        {
            _apologiesColor.a = 1f;
            _apologies.color = _apologiesColor;
            _itemBox.SetActive(false);
        }
        else
        {
            _selectedListElement = _itemSceneHandler[_rndSelection];

            _itemSceneHandler.RemoveAt(_rndSelection);
        }
        return _selectedListElement;
    }

    public void AddSprite()
    {
        _itemImageComponent.sprite = ChooseRnd()._thumbnailSprite;
        _itemChange.ReplaceItem(_selectedListElement);
    }

    public void BounceItemBox()
    {
        _itemBoxTransform = _itemBox.GetComponent<RectTransform>();
        Sequence _sequence = DOTween.Sequence();
        _sequence.Join(_itemBoxTransform.DOShakeScale(_duration, _strength, _vibrato, 1f, true, ShakeRandomnessMode.Harmonic).SetEase(Ease.InOutSine));
    }
}
