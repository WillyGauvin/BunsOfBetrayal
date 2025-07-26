using UnityEngine;
using System.Collections.Generic;
using System;
using DG.Tweening;
using TMPro;

public class Fan : MonoBehaviour
{
    [SerializeField] GameObject hotdogPopup_Prefab;
    [SerializeField] GameObject FloatingTextPrefab;
    GameObject hotdogPopup;

    public GameObject hotdogPopupLocation;

    public Seat mySeat;
    public SpriteRenderer FanSprite;
    public List<Sprite> HomeFan;
    public List<Sprite> AwayFan;
    public List<Sprite> NeutralFan;

    int fanScore = 0;

    int fanSprite;

    public static event Action OnScoreChange;
    public int FanScore
    {
        get { return fanScore; }
        set
        {
            int change = Mathf.Clamp(value, -5, 5) - fanScore;
            ShowFloatingText(transform, change);
            fanScore += change;
            UpdateFan();
            OnScoreChange?.Invoke();
        }
    }
    Controller Player;

    public void Initialize(int fanScore, Seat seat)
    {
        mySeat = seat;
        FanScore = fanScore;

        fanSprite = UnityEngine.Random.Range(0, HomeFan.Count);
        UpdateFan();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Player = FindFirstObjectByType<Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (hotdogPopup != null)
        //{
        //    Vector3 direction = Player.transform.position - transform.position;
        //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //    transform.rotation = Quaternion.Euler(0f, 0f, angle);
        //}
        //else
        //{
        //    transform.rotation = Quaternion.Euler(0f, 0f, 270f);
        //}
    }

    public void UpdateFan()
    {
        if (FanScore > 0)
        {
            FanSprite.sprite = HomeFan[fanSprite];
        }
        else if (FanScore < 0)
        {
            FanSprite.sprite = AwayFan[fanSprite];
        }
        else if (FanScore == 0)
        {
            FanSprite.sprite = NeutralFan[fanSprite];
        }
        mySeat.UpdateFanScore();
    }

    public bool TryHotDogEvent()
    {
        if (hotdogPopup != null)
            return false;
        else if (FanScore == -5 || FanScore == 5)
            return false;

        hotdogPopup = UnityEngine.Object.Instantiate(hotdogPopup_Prefab, hotdogPopupLocation.transform.position, Quaternion.identity);
        hotdogPopup.GetComponent<HotDogPopup>().Initialize(this);
        return true;
    }

    public void FailedHotDogEvent()
    {
        if (FanScore < 0)
        {
            FanScore *= -1;
        }
    }

    void ShowFloatingText(Transform parent, int amount)
    {
        if (amount == 0)
        {
            return;
        }
        GameObject floatingText = UnityEngine.Object.Instantiate(FloatingTextPrefab, parent.position, Quaternion.identity);
        floatingText.GetComponent<TextMeshPro>().text = (amount > 0) ? "+" + amount : amount.ToString();
        floatingText.GetComponent<TextMeshPro>().color = (amount > 0) ? Color.green : Color.red;

        floatingText.transform.DOMoveY(transform.position.y + 0.5f, 3.0f);
        floatingText.GetComponent<TextMeshPro>().DOFade(0.0f, 3.0f).OnComplete(() => Destroy(floatingText));
    }
}
