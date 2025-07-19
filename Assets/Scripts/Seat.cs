using TMPro;
using UnityEngine;

public class Seat : MonoBehaviour
{
    public Fan FanPrefab;
    public Fan myFan;

    public GameObject SeatLocation;

    public TextMeshPro FanScoreText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddFan(int FanScore)
    {
        myFan = GameObject.Instantiate(FanPrefab, SeatLocation.transform.position, FanPrefab.transform.rotation);
        myFan.Initialize(FanScore, this);

        UpdateFanScore();
    }

    public void UpdateFanScore()
    {
        if (myFan != null)
        {
            FanScoreText.text = $"<b>{myFan.FanScore}</b>";
            if (myFan.FanScore < 0)
                FanScoreText.color = Color.blue;
            else
                FanScoreText.color = Color.red;
        }
        else
        {
            FanScoreText.text = "";
        }

        FanScoreText.fontSize = Mathf.Abs(myFan.FanScore * 0.5f) + 0.5f;
    }

    public bool TryHotDogEvent()
    {
        if (myFan == null) return false;
        return myFan.TryHotDogEvent();
    }
}
