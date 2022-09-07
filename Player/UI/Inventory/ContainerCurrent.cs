using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ContainerCurrent : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
{
    public byte cellNum;
    public ContaiterVisiable ContainerPlayer;
    public GameObject InfoPanel;
    public Inventory inventory;

    public GameObject ShopAccess;
    public Slider SliderShopAccess;
    public ButtonAccessrShop ButtonAccessShop;

    int d;


     public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left && ContainerPlayer.ContanerItemList[cellNum].countItem > 0)
        {
            if(Input.GetKey("left shift") && ContainerPlayer.ContanerItemList[cellNum].isStackable == true)
            {
                ShopAccess.SetActive(true);
                SliderShopAccess.maxValue = ContainerPlayer.ContanerItemList[cellNum].countItem;
                ButtonAccessShop.InventoryType = 5;
                ButtonAccessShop.Cell = cellNum; 
                ButtonAccessShop.shop = true;  
            }
            else
            {
                inventory.AddItem(ContainerPlayer.ContanerItemList[cellNum].id, 1, ContainerPlayer.ContanerItemList[cellNum].inventoryList);

                ContainerPlayer.ContanerItemList[cellNum].countItem--;
                ContainerPlayer.DisplayContainer();
            }
        }
    }

    public void AccessShopButton()
    {
        ContainerPlayer.ContanerItemList[ButtonAccessShop.Cell].countItem -= ButtonAccessShop.Count;
        
        inventory.AddItem(ContainerPlayer.ContanerItemList[ButtonAccessShop.Cell].id, ButtonAccessShop.Count, ContainerPlayer.ContanerItemList[ButtonAccessShop.Cell].inventoryList);

        if(ContainerPlayer.ContanerItemList[ButtonAccessShop.Cell].countItem<=0)
            ContainerPlayer.ContanerItemList[ButtonAccessShop.Cell] = new Item();


        ContainerPlayer.DisplayContainer();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(ContainerPlayer.ContanerItemList[cellNum].id != 0)
        {
            InfoPanel.SetActive(true);
        
            Transform iconInfo = InfoPanel.transform.GetChild(0);
            Image icon_Info = iconInfo.GetComponent<Image>();
            icon_Info.sprite = ContainerPlayer.ContanerItemList[cellNum].icon;

            Transform nameInfo = InfoPanel.transform.GetChild(1);
            Text name_Info = nameInfo.GetComponent<Text>();
            name_Info.text = ContainerPlayer.ContanerItemList[cellNum].Name;

            Transform discInfo = InfoPanel.transform.GetChild(2);
            Text disc_Info = discInfo.GetComponent<Text>();
            disc_Info.text = ContainerPlayer.ContanerItemList[cellNum].discription;
            
            Transform costInfo = InfoPanel.transform.GetChild(3);
            Text cost_Info = costInfo.GetChild(0).GetComponent<Text>();
            cost_Info.text = ContainerPlayer.ContanerItemList[cellNum].Cost.ToString();
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        InfoPanel.SetActive(false);
    }
}
