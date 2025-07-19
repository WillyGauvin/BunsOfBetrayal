using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;
public class ToppingsPopup : MonoBehaviour
{
    [SerializeField] HotDogPopup myPopup;
    [SerializeField] Button Ketchup;
    [SerializeField] Button Mustard;
    [SerializeField] Button Relish;
    [SerializeField] Button HotSauce;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnEnable()
    {
        GameManager.OnToppingChange += UpdateToppings;
        UpdateToppings();
    }

    private void OnDisable()
    {
        GameManager.OnToppingChange -= UpdateToppings;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateToppings()
    {
        Ketchup.interactable = GameManager.Instance.PlayerInventory[Topping.Ketchup] > 0;
        Mustard.interactable = GameManager.Instance.PlayerInventory[Topping.Mustard] > 0;
        Relish.interactable = GameManager.Instance.PlayerInventory[Topping.Relish] > 0;
        HotSauce.interactable = GameManager.Instance.PlayerInventory[Topping.HotSauce] > 0;

        if (GameManager.Instance.PlayerInventory[Topping.Relish] == -1)
        {
            Relish.gameObject.SetActive(false);
        }
        else
        {
            Relish.gameObject.SetActive(true);
        }

        if (GameManager.Instance.PlayerInventory[Topping.HotSauce] == -1)
        {
            HotSauce.gameObject.SetActive(false);
        }
        else
        {
            HotSauce.gameObject.SetActive(true);
        }
    }

    public void OnKetchupClicked()
    {
        GameManager.Instance.UseTopping(Topping.Ketchup, myPopup.myFan);
        myPopup.CompletedRequest();
    }

    public void OnMustardClicked()
    {
        GameManager.Instance.UseTopping(Topping.Mustard, myPopup.myFan);
        myPopup.CompletedRequest();
    }

    public void OnRelishClicked()
    {
        GameManager.Instance.UseTopping(Topping.Relish, myPopup.myFan);
        myPopup.CompletedRequest();
    }

    public void OnHotSauceClicked()
    {
        GameManager.Instance.UseTopping(Topping.HotSauce, myPopup.myFan);
        myPopup.CompletedRequest();
    }
}
