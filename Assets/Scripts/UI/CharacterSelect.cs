using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
    private int index;
    [SerializeField] GameObject[] character;
    [SerializeField] Text charaterName;
    void Start()
    {
        index = 0;
        SelectCharacter();
    }
    public void OnPlayBtnClick()
    {
        if (charaterName.text == "Luffy") { SceneManager.LoadScene("Luffy"); }
        if (charaterName.text == "Ushop") { SceneManager.LoadScene("Ushop"); }
        if (charaterName.text == "Zoro") { SceneManager.LoadScene("Zoro"); }
    }
    public void ClickPrevBtn()
    {
        if (index > 0)
        {
            index--;
        }
        SelectCharacter();
    }

   
    public void ClickNextBtn()
    {
        if(index < character.Length-1)
        {
            index++;
        }
        SelectCharacter();
    }
    private void SelectCharacter()
    {
        for (int i = 0; i < character.Length; i++)
        {
            if (i == index)
            {
                character[i].GetComponent<Image>().color = Color.white;
                charaterName.text = character[i].name;
            }
            else
            {
                character[i].GetComponent<Image>().color= Color.black;
                
            }
        }
    }

}
