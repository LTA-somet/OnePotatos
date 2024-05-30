
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryDialog : Dialog
{  
    public void BackHome()
    {
        Show(false);

        SceneManager.LoadScene("Home");
    }
    public void ExitGame()
    {

        Show(false);
    //    UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
