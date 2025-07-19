using UnityEngine;
using TMPro;

using static GameManager;
public class InventoryUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI KetchupText;
    [SerializeField] TextMeshProUGUI MustardText;
    [SerializeField] TextMeshProUGUI RelishText;
    [SerializeField] TextMeshProUGUI HotSauceText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateText();
    }

    private void OnEnable()
    {   
        GameManager.OnToppingChange += UpdateText;
    }

    private void OnDestroy()
    {
        GameManager.OnToppingChange -= UpdateText;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateText()
    {
        KetchupText.text = GameManager.Instance.PlayerInventory[Topping.Ketchup] + "/" + GameManager.Instance.MaxAmountOfItem[Topping.Ketchup];
        MustardText.text = GameManager.Instance.PlayerInventory[Topping.Mustard] + "/" + GameManager.Instance.MaxAmountOfItem[Topping.Mustard];
        RelishText.text = GameManager.Instance.PlayerInventory[Topping.Relish] + "/" + GameManager.Instance.MaxAmountOfItem[Topping.Relish];
        HotSauceText.text = GameManager.Instance.PlayerInventory[Topping.HotSauce] + "/" + GameManager.Instance.MaxAmountOfItem[Topping.HotSauce];
    }
}
