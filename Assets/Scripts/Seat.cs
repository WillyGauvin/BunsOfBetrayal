using TMPro;
using UnityEngine;

public class Seat : MonoBehaviour
{
    public Fan FanPrefab;
    public Fan myFan;

    public GameObject SeatLocation;

    public TextMeshPro FanScoreText;

    [SerializeField] SpriteRenderer Floor;

    [SerializeField] Sprite DefaultFloor;
    [SerializeField] Sprite YellowFloor;
    [SerializeField] Sprite BlueFloor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Fan AddFan(int FanScore)
    {
        myFan = GameObject.Instantiate(FanPrefab, SeatLocation.transform.position, FanPrefab.transform.rotation);
        myFan.Initialize(FanScore, this);

        UpdateFanScore();

        return myFan;
    }

    public void UpdateFanScore()
    {
        if (myFan != null)
        {
            FanScoreText.text = $"<b>{myFan.FanScore}</b>";
            FanScoreText.color = (myFan.FanScore > 0) ? Color.white : Color.white;
            Floor.sprite = (myFan.FanScore > 0) ? BlueFloor : YellowFloor;
            if (myFan.FanScore == 0) Floor.sprite = DefaultFloor;
        }
        else
        {
            FanScoreText.text = "";
            Floor.sprite = DefaultFloor;
        }

        //FanScoreText.fontSize = Mathf.Abs(myFan.FanScore * 0.5f) + 0.5f;
    }

    public bool TryHotDogEvent()
    {
        if (myFan == null) return false;
        return myFan.TryHotDogEvent();
    }
}
