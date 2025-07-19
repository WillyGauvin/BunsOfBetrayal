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
        DontDestroyOnLoad(gameObject);

        PlayerInventory = new Dictionary<Topping, int>();

        PlayerInventory.Add(Topping.Ketchup, 10);
        PlayerInventory.Add(Topping.Mustard, 10);
        PlayerInventory.Add(Topping.Relish, 2);
        PlayerInventory.Add(Topping.HotSauce, 3);

        MaxAmountOfItem = new Dictionary<Topping, int>();

        MaxAmountOfItem.Add(Topping.Ketchup, 10);
        MaxAmountOfItem.Add(Topping.Mustard, 10);
        MaxAmountOfItem.Add(Topping.Relish, 2);
        MaxAmountOfItem.Add(Topping.HotSauce, 4);
    }

    public Controller Player;

    Seat[] Seats;

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
    public Dictionary<Topping, int> MaxAmountOfItem;

    public static event Action OnToppingChange;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
                Seats[seatIndex].AddFan(UnityEngine.Random.Range(-5, 0));

            else
                Seats[seatIndex].AddFan(UnityEngine.Random.Range(1, 6));

        }
        foreach (Seat seat in Seats)
        {
            seat.UpdateFanScore();
        }

        HotDogRequest();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        PlayerInventory[toppingType] -= 1;
        OnToppingChange?.Invoke();

        switch(toppingType)
        {
            case Topping.Ketchup:
                affectedFan.FanScore += 1;
                break;

            case Topping.Mustard:
                affectedFan.FanScore -= 1;
                break;

            case Topping.Relish:
                affectedFan.FanScore *= -1;
                break;

            case Topping.HotSauce:
                affectedFan.FanScore *= 2;
                break;
                
        }

        HotDogRequest();
    }
}
