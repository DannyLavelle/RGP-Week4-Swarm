using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiController : MonoBehaviour
{
    public TMP_Text Money;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        if (Money!=null)
        {
            string money = Convert.ToString(Currency.amount);
            Money.text = ("Money : " + money);
        }
     
    }
    public void Retry()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Reload the current scene
        SceneManager.LoadScene(currentSceneIndex);
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void Play()
    {
        SceneManager.LoadScene("Level");
    }
}
