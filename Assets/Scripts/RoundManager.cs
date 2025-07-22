using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Video;
using static GameManager;

public class RoundManager : MonoBehaviour
{
    private static RoundManager instance;
    public static RoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindFirstObjectByType<RoundManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
    }

    public GameObject GameoverMenu;

    public static event Action OnInningChange;

    public float TimeRemaining = 120.0f;
    public float InningTimeRemaining;
    int innings = 9;

    float timePerInning;

    public int CurrentInning = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameoverMenu.SetActive(false);
        timePerInning = TimeRemaining / (float)innings;
        InningTimeRemaining = timePerInning;
    }

    // Update is called once per frame
    void Update()
    {
        TimeRemaining -= Time.deltaTime;
        if (TimeRemaining <= 0.0f)
        {
            GameOver();
        }

        InningTimeRemaining -= Time.deltaTime;

        if (InningTimeRemaining <= 0.0f)
        {
            CurrentInning = Mathf.Min(CurrentInning + 1, 9);
            if (CurrentInning % 2 == 0)
            {
                GameManager.Instance.AwardTopping(Topping.HotSauce);
                OnInningChange?.Invoke();
            }

            InningTimeRemaining = timePerInning;
        }
    }

    void GameOver()
    {
        Time.timeScale = 0.0f;
        GameoverMenu.SetActive(true);
    }
}

