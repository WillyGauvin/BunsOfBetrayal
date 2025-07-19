using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class HotDogPopup : MonoBehaviour
{
    [SerializeField] GameObject ToppingsPopup;
    [SerializeField] Button HotDogButton;

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
    }

    public void OnHotDogClicked()
    {
        ToppingsPopup.SetActive(!ToppingsPopup.activeSelf);
    }

    public void CompletedRequest()
    {
        Destroy(gameObject);
    }

}
