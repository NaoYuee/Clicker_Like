using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("SFX")]
    public List<AudioContent> _sfxCollection = new List<AudioContent>();
    private AudioSource _audioSource;

    private string _sfxName;
    private AudioClip _sfxClip;

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySFX(string _title)
    {
        foreach (var _entry in _sfxCollection)
        {
            if (_entry._clipTitle == _title)
            {
                _audioSource.PlayOneShot(_entry._clip);
                return;
            }
        }
    }

    public void PlayGroupSFX(List<string> _title)
    {
        foreach (var _entry in _sfxCollection)
        {
            foreach (var _individualTitle in _title)
            {
                if (_entry._clipTitle == _individualTitle)
                {
                    _audioSource.PlayOneShot(_entry._clip);
                }
            }
        }
    }
}
