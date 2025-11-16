using UnityEngine;
using DG.Tweening;

public class NewItems : MonoBehaviour
{
    public float _fadeTime;
    public CanvasGroup _canvasGroup;
    public RectTransform _panelTransform;
    public Vector3 _panelPos;


    public void PanelFadeIn()
    {
        _canvasGroup.alpha = 0f;
        _panelTransform.transform.localPosition = _panelPos;
        _panelTransform.DOAnchorPos(new Vector2(343.0065f, -120.1556f), _fadeTime, false).SetEase(Ease.OutElastic);
        _canvasGroup.DOFade(1,_fadeTime);
    }

    public void PanelFadeOut()
    {
        _canvasGroup.alpha = 1f;
        _panelTransform.transform.localPosition = _panelPos;
        _panelTransform.DOAnchorPos(new Vector2(_panelPos.x, _panelPos.y), _fadeTime, false).SetEase(Ease.InOutQuint);
        _canvasGroup.DOFade(0, _fadeTime);
    }
}
