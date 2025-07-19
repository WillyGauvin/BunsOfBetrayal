using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
public class ScoreBoardUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI TimerText;

    [SerializeField] Image[] HomeLights;
    [SerializeField] Image[] AwayLights;

    [SerializeField] Sprite OffSprite;
    [SerializeField] Sprite HomeSprite;
    [SerializeField] Sprite AwaySprite;

    private void OnEnable()
    {
        Fan.OnChangedTeam += UpdateTeams;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateTeams();
    }

    // Update is called once per frame
    void Update()
    {
        float remaining = Mathf.Max(0, RoundManager.Instance.TimeRemaining);
        TimerText.text = TimeSpan.FromSeconds(remaining).ToString(@"mm:ss");
    }
    void UpdateTeams()
    {
        Tuple<int, int> TotalToHomeFansCount = GameManager.Instance.GetTotalToHomeFans();

        int totalFans = TotalToHomeFansCount.Item1;
        int homeFans = TotalToHomeFansCount.Item2;
        int awayFans = totalFans - homeFans;

        float homeScoreBoardLights = (int)(((float) homeFans / (float) totalFans) * 9);
        int awayScoreBoardLights = (int)(((float)awayFans / (float)totalFans) * 9);

        for (int i = 0; i < HomeLights.Length; i++)
        {
            HomeLights[i].sprite = (i < homeScoreBoardLights)? HomeSprite : OffSprite;
            AwayLights[i].sprite = (i < awayScoreBoardLights) ? AwaySprite : OffSprite;
        }
    }
}
