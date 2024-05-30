using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSetting : MonoBehaviour
{
    public AudioSource audioSource;
    public Text volumeText; 
    public Button increaseButton; 
    public Button decreaseButton; 
    public float step = 0.1f; 

    void Start()
    {
       
        //if (increaseButton != null)
        //    increaseButton.onClick.AddListener(IncreaseVolume);

        //if (decreaseButton != null)
        //    decreaseButton.onClick.AddListener(DecreaseVolume);

       
        UpdateVolumeText();
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
