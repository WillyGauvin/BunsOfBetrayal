using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections;
using DG.Tweening;
public class ScoreBoardUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI TimerText;

    [SerializeField] TextMeshProUGUI[] Innings;

    [SerializeField] Image HomeFill;

    bool showingMessage = false;

    private void OnEnable()
    {
        Fan.OnScoreChange += UpdateScore;
        RoundManager.OnInningChange += UpdateInning;
        GameManager.OnShowScoreBoardMessage += ShowMessage;
    }
    private void OnDisable()
    {
        Fan.OnScoreChange -= UpdateScore;
        RoundManager.OnInningChange -= UpdateInning;
        GameManager.OnShowScoreBoardMessage -= ShowMessage;
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
        if (!showingMessage)
        {
            float remaining = Mathf.Max(0, RoundManager.Instance.TimeRemaining);
            TimerText.text = TimeSpan.FromSeconds(remaining).ToString(@"mm\:ss");
        }
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

    public void ShowMessage(String messageToShow)
    {
        StartCoroutine(ShowMessageAnimation(messageToShow));
    }

    IEnumerator ShowMessageAnimation(String messageToShow)
    {
        showingMessage = true;

        TimerText.text = messageToShow;

        yield return new WaitForSeconds(1.0f);

        Tween ScreenFlash = TimerText.DOFade(0.0f, 1.0f).SetLoops(6, LoopType.Yoyo);

        yield return ScreenFlash.WaitForCompletion();
        
        showingMessage = false;

    }
}
