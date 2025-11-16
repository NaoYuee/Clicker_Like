using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class ItemChange : MonoBehaviour
{
    [Header("Item Description")]
    public List<ItemSceneHandler> _itemSceneHandler;
    public List<ItemSceneHandler> _stockedItem;
    private Sprite _itemSprite;
    private Sprite _thumbnailSprite;
    /*private Sprite _thumbnailExpression;*/
    private RuntimeAnimatorController _itemController;
    private bool _isUnlocked;


    public ItemsCollection _itemCollectionScript;

    [Header("Object Components")]
    [SerializeField] private SpriteRenderer _itemSpriteRenderer;
    [SerializeField] private Animator _itemAnimator;
    private Sprite _spriteToReplace;
    private RuntimeAnimatorController _animatorToReplace;
    private Color _spriteColor;
    public float _chooseAlpha;
    private int _indexItem = 0;

    private void Start()
    {

        _spriteToReplace = _itemSpriteRenderer.sprite;
        _animatorToReplace = _itemAnimator.runtimeAnimatorController;

        _spriteColor = _itemSpriteRenderer.color;

        foreach (var _accesory in _itemSceneHandler)
        {
            _itemSprite = _accesory._itemSprite;
            _itemController = _accesory._itemController;

            _thumbnailSprite = _accesory._thumbnailSprite;

            _spriteColor.a = _chooseAlpha;
            _itemSpriteRenderer.color = _spriteColor;
        }
    }

    #region trrash
    /*private bool VerifyUnlocked()
    {
        if (_itemCollectionScript.ChooseRnd() == _itemSprite)
        {
            _isUnlocked = true;
            Debug.Log("GameObject true");
        }
        return _isUnlocked;
    }

    private void ActivateSprite()
    {
        if(VerifyUnlocked() == true)
        {
            _spriteColor.a = 1f;
            this.GetComponent<SpriteRenderer>().color = _spriteColor;
        }
    }

    public void ReplaceItem(ItemSceneHandler _itemSceneHandler)
    {

        if (VerifyUnlocked())
        {
            ActivateSprite();
            Debug.Log("Replaceeeeee");
            *//*_spriteToReplace = _itemSprite;
            _animatorToReplace = _itemAnimator;*//*
        }
    }*/
    #endregion

    public void ReplaceItem(ItemSceneHandler _itemSceneHandler, bool _addToList = true)
    {
        if (_addToList)
        {
            _stockedItem.Add(_itemSceneHandler);
        }
        _spriteColor.a = 1f;
        _itemSpriteRenderer.color = _spriteColor;
        _itemSpriteRenderer.sprite = _itemSceneHandler._itemSprite;
        _itemAnimator.runtimeAnimatorController = _itemSceneHandler._itemController;

    }

    public void ChangeToNextItem()
    {
        _indexItem++;
        _indexItem %= _stockedItem.Count;

        ReplaceItem(_stockedItem[_indexItem], false);   
    }
}
