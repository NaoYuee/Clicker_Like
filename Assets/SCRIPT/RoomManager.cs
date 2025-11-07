using UnityEngine;
using UnityEngine.SceneManagement; 

public class RoomManager : MonoBehaviour
{
    public void SceneChange (string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Pressed");
    }
}
