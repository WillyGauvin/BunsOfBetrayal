using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindFirstObjectByType<GameManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);

        FanList = new List<Fan>();

        PlayerInventory = new Dictionary<Topping, int>();

        PlayerInventory.Add(Topping.Relish, 0);
        PlayerInventory.Add(Topping.HotSauce, 1);

        Player = FindFirstObjectByType<Controller>();

        Seats = FindObjectsByType<Seat>(FindObjectsSortMode.None);

        int filledSeats = Mathf.RoundToInt((Seats.Length) * filledSeatsPercentage);
        int homeFans = Mathf.RoundToInt(filledSeats * homeFanPercentage);

        List<int> seatIndices = Enumerable.Range(0, Seats.Length).ToList();

        ListExtensions.Shuffle(seatIndices); // Randomize seat order

        for (int i = 0; i < filledSeats; i++)
        {
            int seatIndex = seatIndices[i];

            if (i < homeFans)
                FanList.Add(Seats[seatIndex].AddFan(UnityEngine.Random.Range(1, 4)));

            else
                FanList.Add(Seats[seatIndex].AddFan(UnityEngine.Random.Range(-3, 0)));

        }
        foreach (Seat seat in Seats)
        {
            seat.UpdateFanScore();
        }
    }

    public Controller Player;

    Seat[] Seats;
    List<Fan> FanList;

    [Range(0f, 1f)] public float filledSeatsPercentage = 0.75f;
    [Range(0f, 1f)] public float homeFanPercentage = 0.6f;

    public enum Topping
    {
        Ketchup,
        Mustard,
        Relish,
        HotSauce,
    }

    public Dictionary<Topping, int> PlayerInventory;

    public static event Action OnToppingChange;

    public int KetchupsToServedForRelish = 3;
    public int KetchupsServed = 0;

    float TimePerRequest = 7.0f;
    float TimePerRequestRemaining;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HotDogRequest();
        TimePerRequestRemaining = TimePerRequest;
    }

    // Update is called once per frame
    void Update()
    {
        TimePerRequestRemaining -= Time.deltaTime;
        if (TimePerRequestRemaining <= 0.0f)
        {
            HotDogRequest();
            TimePerRequestRemaining = TimePerRequest;
        }
    }

    void HotDogRequest()
    {
        bool completedRequest = false;

        while (!completedRequest)
        {
            completedRequest = Seats[UnityEngine.Random.Range(0, Seats.Length)].TryHotDogEvent();
        }
    }

    public void UseTopping(Topping toppingType, Fan affectedFan)
    {
        switch(toppingType)
        {
            case Topping.Ketchup:
                affectedFan.FanScore += 1;
                KetchupsServed += 1;
                if (KetchupsServed >= KetchupsToServedForRelish)
                {
                    AwardTopping(Topping.Relish);
                    KetchupsServed = 0;
                }
                break;

            case Topping.Mustard:
                affectedFan.FanScore -= 1;
                break;

            case Topping.Relish:
                PlayerInventory[toppingType] -= 1;
                affectedFan.FanScore *= -1;
                break;

            case Topping.HotSauce:
                PlayerInventory[toppingType] -= 1;
                affectedFan.FanScore *= 2;
                break;
                
        }
        OnToppingChange?.Invoke();

        if (UnityEngine.Random.Range(0, 101) < 50)
        {
            HotDogRequest();
        }
    }

    public float GetHomeFanPercent()
    {
        int HomeFanCheerLevel = 0;
        int AwayFanCheerLevel = 0;
        int totalCheerlevel = 0;
        foreach(Fan fan in FanList)
        {
            if (fan.FanScore > 0) HomeFanCheerLevel += fan.FanScore;
            if (fan.FanScore < 0) AwayFanCheerLevel += fan.FanScore;
        }
        totalCheerlevel = HomeFanCheerLevel + Mathf.Abs(AwayFanCheerLevel);

        return (float)HomeFanCheerLevel / (float)totalCheerlevel;
        
    }

    public void AwardTopping(Topping toppingToAward)
    {
        PlayerInventory[toppingToAward] += 1;
        OnToppingChange?.Invoke();
    }
}
