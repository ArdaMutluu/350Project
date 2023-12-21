using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Levels : MonoBehaviour
{

    public Button[] buttons;

    public void Awake()
    {
        int unlocked = PlayerPrefs.GetInt("UnlockedLevel", 1);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
        for (int i = 0; i < unlocked; i++)
        {
            buttons[i].interactable = true;
        }
    }

    public void LoadLevel(int levelID)
    {
        string level = "Level" + levelID;
        SceneManager.LoadScene(level);
    }
    
}
