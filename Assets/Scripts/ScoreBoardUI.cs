using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
public class ScoreBoardUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI TimerText;

    [SerializeField] TextMeshProUGUI[] Innings;

    [SerializeField] Image HomeFill;

    private void OnEnable()
    {
        Fan.OnScoreChange += UpdateScore;
        RoundManager.OnInningChange += UpdateInning;
    }
    private void OnDisable()
    {
        Fan.OnScoreChange -= UpdateScore;
        RoundManager.OnInningChange -= UpdateInning;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateScore();
        UpdateInning();
    }


    // Update is called once per frame
    void Update()
    {
        float remaining = Mathf.Max(0, RoundManager.Instance.TimeRemaining);
        TimerText.text = TimeSpan.FromSeconds(remaining).ToString(@"mm\:ss");
    }
    void UpdateScore()
    {
        HomeFill.fillAmount = GameManager.Instance.GetHomeFanPercent();
    }

    public void UpdateInning()
    {
        foreach (TextMeshProUGUI text in Innings)
        {
            text.color = Color.white;
        }
        Innings[RoundManager.Instance.CurrentInning - 1].color = Color.red;
    }
}
