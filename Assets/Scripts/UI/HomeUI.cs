using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeUI : MonoBehaviour
{
    [SerializeField] GameObject homeUI;
    [SerializeField] GameObject characterSelectionUI;
   public void OnPlayBtnClick()
    {
        homeUI.SetActive(false);
        characterSelectionUI.SetActive(true);

    }
}
