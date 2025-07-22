using UnityEngine;
using TMPro;

using static GameManager;
public class InventoryUI : MonoBehaviour
{
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
        RelishText.text = GameManager.Instance.PlayerInventory[Topping.Relish].ToString();
        HotSauceText.text = GameManager.Instance.PlayerInventory[Topping.HotSauce].ToString();
    }
}
