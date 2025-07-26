using System;
using System.Collections;
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

        PlayerInventory.Add(Topping.Relish, 1);
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

    public delegate void OnToppingChangeEvent(Topping ToppingChanged, int amount);
    public static event OnToppingChangeEvent OnToppingChange;


    public delegate void OnShowScoreBoardEvent(String MessageToShow);
    public static event OnShowScoreBoardEvent OnShowScoreBoardMessage;

    public static event Action UpdateRelish;

    public int KetchupsToServedForRelish = 3;
    public int KetchupsServed = 0;

    float TimePerRequest = 2.0f;
    float TimePerRequestRemaining;

    [SerializeField] LayerMask TargetLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HotDogRequest();
        TimePerRequestRemaining = TimePerRequest;
        StartCoroutine(GameEvents());
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
                UpdateRelish?.Invoke();
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
                OnToppingChange?.Invoke(toppingType, -1);

                break;

            case Topping.HotSauce:
                PlayerInventory[toppingType] -= 1;
                affectedFan.FanScore *= 2;
                OnToppingChange?.Invoke(toppingType, -1);


                Collider2D[] hits = Physics2D.OverlapCircleAll(affectedFan.transform.position, 1.0f, TargetLayer);

                foreach (Collider2D hit in hits)
                {
                    if (hit.GetComponent<Fan>() != affectedFan)
                    {
                        hit.GetComponent<Fan>().FanScore -= 1;
                    }
                }
                break;
                
        }

        //if (UnityEngine.Random.Range(0, 101) < 80)
        //{
        //    HotDogRequest();
        //}
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
        OnToppingChange?.Invoke(toppingToAward, 1);
    }



    IEnumerator GameEvents()
    {
        yield return new WaitForSeconds(1.0f);
        while(true)
        {
            yield return new WaitForSeconds(30.0f);
            float TimeRemaining = RoundManager.Instance.TimeRemaining;
            if (TimeRemaining <= 30.0f)
            {
                StartCoroutine(HotDogFrenzy());
            }
            else if (TimeRemaining <= 60.0f)
            {
                StartCoroutine(AwayTeamScore());
            }
            else if (TimeRemaining <= 90.0f)
            {
                StartCoroutine(HomeTeamHomeRun());
            }
        }
    }
    IEnumerator HomeTeamHomeRun()
    {
        OnShowScoreBoardMessage?.Invoke("BLUE HOME RUN");
        foreach (Fan fan in FanList)
        {
            if (UnityEngine.Random.Range(0, 101) < 30)
            {
                fan.FanScore += 1;
                yield return new WaitForSeconds(UnityEngine.Random.Range(0.01f, 0.05f));
            }
        }
    }

    IEnumerator AwayTeamScore()
    {
        OnShowScoreBoardMessage?.Invoke("GREEN RUN");
        foreach (Fan fan in FanList)
        {
            if (UnityEngine.Random.Range(0, 101) < 20)
            {
                fan.FanScore += -1;
                yield return new WaitForSeconds(UnityEngine.Random.Range(0.01f, 0.05f));
            }
        }
    }
    IEnumerator HotDogFrenzy()
    {
        OnShowScoreBoardMessage?.Invoke("HALF OFF DOGS");
        for (int i = 0; i < 10; i++)
        {
            HotDogRequest();
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1.0f));
        }
    }
}
