using System.Collections.Generic;
using UnityEngine;
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
        DontDestroyOnLoad(gameObject);
    }

    public float TimeRemaining = 120.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TimeRemaining -= Time.deltaTime;

    }
}
