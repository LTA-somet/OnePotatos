
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingDialog : Dialog
{
    public AudioSource audioSource;
    public Text volumeText;
    public float step = 0.1f;
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.C))
        {
           Close();
        }
        if (Input.GetKeyUp(KeyCode.H))
        {
            BackHome();
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            Exit();
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
           IncreaseVolume();
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            DecreaseVolume();   
        }
        UpdateVolumeText();
    }
    public void Exit()
    {

        Show(false);
    //    UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
    public void BackHome()
    {
        Show(false);

        SceneManager.LoadScene("Home");
    }
    public override void Close()
    {
        base.Close();
        Time.timeScale = 1.0f;
    }
    void IncreaseVolume()
    {
        audioSource.volume = Mathf.Clamp(audioSource.volume + step, 0f, 1f);
        UpdateVolumeText();
    }


    void DecreaseVolume()
    {
        audioSource.volume = Mathf.Clamp(audioSource.volume - step, 0f, 1f);
        UpdateVolumeText();
    }


    void UpdateVolumeText()
    {
        if (volumeText != null)
        {
            volumeText.text = (audioSource.volume * 100).ToString("F0") + "%";
        }
    }
}
