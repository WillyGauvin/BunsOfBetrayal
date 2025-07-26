using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using static GameManager;
using System.Collections;
using static UnityEngine.GraphicsBuffer;
public class InventoryUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI RelishText;
    [SerializeField] TextMeshProUGUI HotSauceText;
    [SerializeField] Image HotSauceImage;
    [SerializeField] Image RelishImage;

    [SerializeField] GameObject FloatingTextPrefab;

    float TimeToGetHotSauce = 20.0f;
    float HotSauceTimeElapsed = 0.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateHotSauce();
        UpdateRelish();
    }

    private void OnEnable()
    {
        GameManager.OnToppingChange += UpdateText;
        GameManager.UpdateRelish += UpdateRelish;
    }

    private void OnDestroy()
    {
        GameManager.OnToppingChange -= UpdateText;
        GameManager.UpdateRelish -= UpdateRelish;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHotSauce();
    }

    void UpdateText(Topping toppingType, int amount)
    {
        RelishText.text = GameManager.Instance.PlayerInventory[Topping.Relish].ToString();
        HotSauceText.text = GameManager.Instance.PlayerInventory[Topping.HotSauce].ToString();
        StartCoroutine(UpdateTextAnimation(toppingType, amount));
    }

    void UpdateHotSauce()
    {
        HotSauceTimeElapsed += Time.deltaTime;
        if (HotSauceTimeElapsed > TimeToGetHotSauce)
        {
            GameManager.Instance.AwardTopping(Topping.HotSauce);
            HotSauceTimeElapsed = 0.0f;
        }

        HotSauceImage.fillAmount = HotSauceTimeElapsed / TimeToGetHotSauce;
    }

    public void UpdateRelish()
    {
        RelishImage.fillAmount = (float)GameManager.Instance.KetchupsServed / 3.0f;

        StartCoroutine(RelishAnimation());
    }

    IEnumerator RelishAnimation()
    {
        Tween BounceTween = RelishImage.rectTransform.DOPunchScale(new Vector3(0.5f,0.5f,0.0f), 1.0f);
        yield return BounceTween.WaitForCompletion();

        if (RelishImage.fillAmount >= 1.0f)
        {
            RelishImage.fillAmount = 0.0f;
        }
    }

    IEnumerator UpdateTextAnimation(Topping ToppingTextToChange, int amount)
    {
        if(ToppingTextToChange == Topping.Relish)
        {
            Vector3 originalScale = RelishText.rectTransform.localScale;
            RelishText.rectTransform.localScale = Vector3.zero; // Optional: start from 0 scale
            Tween RelishTween = RelishText.rectTransform.DOScale(originalScale, 0.5f)
                .SetEase(Ease.OutBounce);

            RelishText.DOColor(Color.yellow, 0.5f).OnComplete(() => RelishText.DOColor(Color.white, 0.2f));

            ShowFloatingText(RelishText.rectTransform, amount);

            yield return RelishTween.WaitForCompletion();
        }
        else if (ToppingTextToChange == Topping.HotSauce)
        {
            Vector3 originalScale = HotSauceText.rectTransform.localScale;
            HotSauceText.rectTransform.localScale = Vector3.zero; // Optional: start from 0 scale
            Tween HotSauceTween = HotSauceText.rectTransform.DOScale(originalScale, 0.5f)
                .SetEase(Ease.OutBounce);

            HotSauceText.DOColor(Color.yellow, 0.5f).OnComplete(() => HotSauceText.DOColor(Color.white, 0.2f));

            ShowFloatingText(HotSauceText.rectTransform, amount);

            yield return HotSauceTween.WaitForCompletion();
        }

        yield return new WaitForEndOfFrame();
    }

    void ShowFloatingText(Transform parent, int amount)
    {
        GameObject floatingText = Object.Instantiate(FloatingTextPrefab, parent);
        floatingText.GetComponent<TextMeshProUGUI>().text = (amount > 0) ? "+1" : "-1";
        floatingText.GetComponent<TextMeshProUGUI>().color = (amount > 0) ? Color.green : Color.red;
        floatingText.GetComponent<RectTransform>().anchoredPosition = Vector2.zero + new Vector2(0.0f, 10.0f);
        floatingText.GetComponent<RectTransform>().DOAnchorPosY(floatingText.GetComponent<RectTransform>().anchoredPosition.y + 30.0f, 1.0f);
        floatingText.GetComponent<TextMeshProUGUI>().DOFade(0.0f, 1.0f).OnComplete(() => Destroy(floatingText));
    }
}
