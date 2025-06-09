
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class MainMenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartGame()
    {
        UnityEngine.Debug.Log("Starting Game...");
        SceneManager.LoadScene("Divided Village");
    }

    // Update is called once per frame
    public void QuitGame()
    {
        UnityEngine.Debug.Log("Quiting Game...");
        Application.Quit();

#if UNITY_EDITOR
UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
