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

    [SerializeField] GameObject AffectAreaPrefab;
    GameObject AffectArea;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnEnable()
    {
        GameManager.OnToppingChange += UpdateToppings;
    }

    private void OnDisable()
    {
        GameManager.OnToppingChange -= UpdateToppings;
        if (AffectArea)
        {
            Destroy(AffectArea);
        }
    }

    void Start()
    {
        UpdateToppings(Topping.Ketchup, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateToppings(Topping ToppingChanged, int amount)
    {
        Relish.interactable = GameManager.Instance.PlayerInventory[Topping.Relish] > 0;
        HotSauce.interactable = GameManager.Instance.PlayerInventory[Topping.HotSauce] > 0;
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

    public void OnHotSauceHovered()
    {
        if (!AffectArea)
        {
            AffectArea = Instantiate(AffectAreaPrefab, myPopup.myFan.transform.position, Quaternion.identity);
            AffectArea.transform.localScale = Vector3.one;
        }

    }

    public void OnHotSacueUnHovered()
    {
        if (AffectArea)
        {
            Destroy(AffectArea);
            AffectArea = null;
        }
    }
}
