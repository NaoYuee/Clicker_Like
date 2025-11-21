using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class RoomManager : MonoBehaviour
{
    [SerializeField] private Animator _transitionAnimator;

    public void PlayAnimationEnd(string _sceneName)
    {
        _transitionAnimator.Play("ANIM_Transition_Close");
        StartCoroutine(SceneChange(_sceneName));
    }

    private IEnumerator SceneChange (string _sceneName)
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(_sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Pressed");
    }
}
