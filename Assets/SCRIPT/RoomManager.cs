using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private Animator _transitionAnimator;
    [SerializeField] private bool _isPlayingTransition;
    [SerializeField] private Button _interactableButton;

    public void PlayAnimationEnd(string _sceneName)
    {
        AudioManager.Instance.StopMusic();
        _transitionAnimator.Play("ANIM_Transition_Close");
        AudioManager.Instance.PlaySFX("Door Shut");
       _isPlayingTransition= true;
        StartCoroutine(SceneChange(_sceneName));
    }

    private IEnumerator SceneChange (string _sceneName)
    {
        yield return new WaitForSeconds(0.7f);
        SceneManager.LoadScene(_sceneName);
        _isPlayingTransition = false;
        AudioManager.Instance.PlayMusic();
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Pressed");
    }

    public void NotInteractble()
    {
        if (_isPlayingTransition == false)
        {
            _interactableButton.interactable = false;
        }
        else
        {
            _interactableButton.interactable= true;
        }
    }
}
