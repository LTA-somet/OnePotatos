
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameoverDialog : Dialog
{
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            ExitGame();
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            Replay();
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            BackHome();
        }
    }
    public void Replay()
    {
        Show(false);
        ReloadCurrentScene();
    }
    public void BackHome()
    {
         Show(false );
     
        SceneManager.LoadScene("Home");
    }
    public void ExitGame()
    {
     
        Show(false);
  //      UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
    private void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
