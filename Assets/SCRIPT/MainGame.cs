using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;


public class MainGame : MonoBehaviour
{
    [Header("Number of Likes")]
    public TMP_Text _likeText;
    public int _likes;

    [Header("Gauge Progession")]
    public Slider _likesGauge;
    public float _gaugeProgression;
    public int _gaugeMax = 10;
    public int _gaugeMultiplier;

    public void Start()
    {
        _likesGauge.maxValue = _gaugeMax;
    }

    public void addLikes()
    {
        _likes++;
        _likeText.text = _likes.ToString();
        Gauge();
    }

    public void Gauge()
    {
        if (_gaugeProgression % _gaugeMax == 0 && _gaugeProgression != 0)
        {
            _gaugeMax = _gaugeMax * _gaugeMultiplier;
            _likesGauge.maxValue = _gaugeMax;
            _gaugeProgression = 0;
            _likesGauge.value = _gaugeProgression;
        }
        else
        {
            _gaugeProgression++; //problem cause the value of _likes does not go back to zero when _gaugeProgression is reinitialized. What to do?
            _likesGauge.value = _gaugeProgression;
        }
    }
}
