using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopCurrent : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
{
    public byte cellNum;
    public ShopVisiable ShopPlayer;
    public GameObject InfoPanel;
    public Inventory inventory;

    public GameObject ShopAccess;
    public Slider SliderShopAccess;
    public ButtonAccessrShop ButtonAccessShop;

    int d;


     public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left && ShopPlayer.TraderList[cellNum].Cost <= ShopPlayer.Money && ShopPlayer.TraderList[cellNum].countItem > 0)
        {
            if(Input.GetKey("left shift")&&ShopPlayer.TraderList[cellNum].isStackable == true)
            {
                int finalCountBuy;
                int CountAllMoney = ShopPlayer.Money/ShopPlayer.TraderList[cellNum].Cost;
                if(CountAllMoney<ShopPlayer.TraderList[cellNum].countItem)
                    finalCountBuy = CountAllMoney;
                else
                    finalCountBuy = ShopPlayer.TraderList[cellNum].countItem;

                ShopAccess.SetActive(true);
                SliderShopAccess.maxValue = finalCountBuy;
                ButtonAccessShop.InventoryType = 4;
                ButtonAccessShop.Cell = cellNum;   
                ButtonAccessShop.shop = true;
            }
            else
            {
                inventory.AddItem(ShopPlayer.TraderList[cellNum].id, 1, ShopPlayer.TraderList[cellNum].inventoryList);

                ShopPlayer.TraderList[cellNum].countItem --;
                ShopPlayer.DisplayTrader();

                ShopPlayer.Money -= ShopPlayer.TraderList[cellNum].Cost;
            }
            
        }
    }

    public void AccessShopButton()
    {
        ShopPlayer.Money -= (int)(ShopPlayer.TraderList[ButtonAccessShop.Cell].Cost * ButtonAccessShop.Count);

        ShopPlayer.TraderList[ButtonAccessShop.Cell].countItem -= ButtonAccessShop.Count;
        
        inventory.AddItem(ShopPlayer.TraderList[ButtonAccessShop.Cell].id, ButtonAccessShop.Count, ShopPlayer.TraderList[ButtonAccessShop.Cell].inventoryList);

        if(ShopPlayer.TraderList[ButtonAccessShop.Cell].countItem<=0)
            ShopPlayer.TraderList[ButtonAccessShop.Cell] = new Item();


        ShopPlayer.DisplayTrader();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(ShopPlayer.TraderList[cellNum].id != 0)
        {
            InfoPanel.SetActive(true);
        
            Transform iconInfo = InfoPanel.transform.GetChild(0);
            Image icon_Info = iconInfo.GetComponent<Image>();
            icon_Info.sprite = ShopPlayer.TraderList[cellNum].icon;

            Transform nameInfo = InfoPanel.transform.GetChild(1);
            Text name_Info = nameInfo.GetComponent<Text>();
            name_Info.text = ShopPlayer.TraderList[cellNum].Name;

            Transform discInfo = InfoPanel.transform.GetChild(2);
            Text disc_Info = discInfo.GetComponent<Text>();
            disc_Info.text = ShopPlayer.TraderList[cellNum].discription;
            
            Transform costInfo = InfoPanel.transform.GetChild(3);
            Text cost_Info = costInfo.GetChild(0).GetComponent<Text>();
            cost_Info.text = ShopPlayer.TraderList[cellNum].Cost.ToString();
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        InfoPanel.SetActive(false);
    }
}
