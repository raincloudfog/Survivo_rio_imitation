using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class Game_Timer : MonoBehaviour
{
    public TMP_Text timerText;
    public float timeRemaining = 0.0f; // 초기 타이머 설정 ( 초 단위) // 60초로하고 -= 하면 타이머 += 스톱워치

    private bool isPaused = false;
    

    // Start is called before the first frame update
    void Start()
    {
        UpdateTimerDisplay();
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isPaused)
        {
            timeRemaining += Time.deltaTime ;
           
            UpdateTimerDisplay();
        }
    }

    public void PauseTimer()
    {
        isPaused = true;
    }

    public void ResumeTimer()
    {
        isPaused = false;
    }

    public int GetMinutes()
    {
        return Mathf.FloorToInt(timeRemaining / 60);
    }

    public int GetSeconds()
    {
        return Mathf .FloorToInt(timeRemaining % 60);
    }
}
