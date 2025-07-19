using UnityEngine;
using System.Collections.Generic;
using System;

public class Fan : MonoBehaviour
{
    [SerializeField] GameObject hotdogPopup_Prefab;
    GameObject hotdogPopup;

    public GameObject hotdogPopupLocation;

    public Seat mySeat;
    public SpriteRenderer FanSprite;
    public List<Sprite> HomeFan;
    public List<Sprite> AwayFan;

    int fanScore = 0;

    public static event Action OnChangedTeam;
    public int FanScore
    {
        get { return fanScore; }
        set
        {
            if ((fanScore < 0 && value > -1) || (fanScore > 0 && value < 1))
            {
                OnChangedTeam?.Invoke();
            }
            fanScore = Mathf.Clamp(value, -5, 5);
            UpdateFan();
        }
    }
    Controller Player;

    public void Initialize(int fanScore, Seat seat)
    {
        mySeat = seat;
        FanScore = fanScore;

        UpdateFan();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = FindFirstObjectByType<Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hotdogPopup != null)
        {
            Vector3 direction = Player.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 270f);
        }
    }

    public void UpdateFan()
    {
        if (FanScore < 0)
        {
            FanSprite.sprite = HomeFan[UnityEngine.Random.Range(0, HomeFan.Count)];
        }
        else
        {
            FanSprite.sprite = AwayFan[UnityEngine.Random.Range(0, AwayFan.Count)];
        }
        mySeat.UpdateFanScore();
    }

    public bool TryHotDogEvent()
    {
        if (hotdogPopup != null)
            return false;

        hotdogPopup = UnityEngine.Object.Instantiate(hotdogPopup_Prefab, hotdogPopupLocation.transform.position, Quaternion.identity);
        hotdogPopup.GetComponent<HotDogPopup>().Initialize(this);
        return true;
    }
}
