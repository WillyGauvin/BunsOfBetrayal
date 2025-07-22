using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class HotDogPopup : MonoBehaviour
{
    [SerializeField] GameObject ToppingsPopup;
    [SerializeField] Button HotDogButton;

    float RequestTime = 30.0f;
    float RequestTimeElapsed = 0.0f;

    [SerializeField] Color EnabledColorRed;
    [SerializeField] Color EnabledColorGreen;
    [SerializeField] Color DisabledColorRed;
    [SerializeField] Color DisabledColorGreen;
    [SerializeField] Color HighLightedColorRed;
    [SerializeField] Color HighLightedColorGreen;

    public Fan myFan;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ToppingsPopup.SetActive(false);
        HotDogButton.interactable = false;
    }

    public void Initialize(Fan fan)
    {
        myFan = fan;
    }

    // Update is called once per frame
    void Update()
    {
        float yOffset = Mathf.Abs(myFan.gameObject.transform.position.y - GameManager.Instance.Player.transform.position.y);
        float xOffset = Mathf.Abs(myFan.gameObject.transform.position.x - GameManager.Instance.Player.transform.position.x);
        if (yOffset > 0.64f || xOffset > 0.64f * 4.5f)
        {
            ToppingsPopup.SetActive(false);
            HotDogButton.interactable = false;
        }
        else
        {
            HotDogButton.interactable = true;
        }

        if (!ToppingsPopup.activeSelf)
        {
            RequestTimeElapsed += Time.deltaTime;
            float t = RequestTimeElapsed / RequestTime;
            ColorBlock cb = HotDogButton.colors;
            cb.disabledColor = Color.Lerp(DisabledColorGreen, DisabledColorRed, t);
            cb.normalColor = Color.Lerp(EnabledColorGreen, EnabledColorRed, t);
            cb.highlightedColor = Color.Lerp(HighLightedColorGreen, HighLightedColorRed, t);
            cb.selectedColor = Color.Lerp(HighLightedColorGreen, HighLightedColorRed, t);
            HotDogButton.colors = cb;

            if (RequestTimeElapsed >= RequestTime)
            {
                FailedRequest();
                Destroy(gameObject);
            }
        }
    }

    public void OnHotDogClicked()
    {
        ToppingsPopup.SetActive(!ToppingsPopup.activeSelf);
    }

    public void CompletedRequest()
    {
        Destroy(gameObject);
    }

    public void FailedRequest()
    {
        myFan.FailedHotDogEvent();
    }

}
