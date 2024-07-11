using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class Game_Timer : MonoBehaviour
{
    public TMP_Text timerText;
    public float timeRemaining = 0.0f; // �ʱ� Ÿ�̸� ���� ( �� ����) // 60�ʷ��ϰ� -= �ϸ� Ÿ�̸� += �����ġ

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
