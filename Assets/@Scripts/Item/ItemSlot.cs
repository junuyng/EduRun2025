using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private ItemDataSO itemData;
    [SerializeField] private Button buyButton;
    [SerializeField] private TextMeshProUGUI price;

    private void Start()
    {
        buyButton.onClick.AddListener(OnClickBuy);
    }

    private void OnEnable()
    {
        CheckItemOwnership();
    }
    private void CheckItemOwnership()
    {
        bool alreadyOwned = false;

        foreach (UserItem item in GameManager.Instance.userItems)
        {
            if (item.itemName == itemData.itemName)
            {
                alreadyOwned = true;
                break;
            }
        }
        
        if (alreadyOwned)
        {
            buyButton.interactable = !alreadyOwned;
            price.text = "구매 완료";
        }
        else
        {
        }
    }

    private void OnClickBuy()
    {
        var player = GameManager.Instance.playerData;


        if (player.money >= itemData.price)
        {   
            buyButton.interactable = false;  
            StartCoroutine(GameManager.Instance.BuyItem(
                player.userId,
                itemData.itemName,
                result =>
                {
                    if (result != null)
                    {
                        buyButton.interactable = false;
                        price.text = "구매 완료";

                        GameManager.Instance.CallOnBuy();  
                    }

                    else
                    {
                    }
                }));
        }
        else
        {
        }
    }
}
