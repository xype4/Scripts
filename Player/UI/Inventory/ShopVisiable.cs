using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopVisiable : MonoBehaviour
{
    public List<Item> TraderList = new List<Item>();
    public GameObject ShopList;
    private int MaxCountItems;
    public GameObject Inventory;
    public int Money;
    public GameObject MoneyMenu;
    public bool inShop;
    public GameObject Body;
    public GameObject ShopAccess;
    
    void Start()
    {
        inShop = false;
        MaxCountItems = ShopList.transform.childCount;      


        for(int i = 0; i < MaxCountItems; i++)         
        {
            ShopList.transform.GetChild(i).GetComponent<ShopCurrent>().cellNum = (byte)i;       //нумеруем ячейки ShopCurrent
        }
    }
    void Update()
    {
        MoneyMenu.GetComponent<Text>().text = Money.ToString();


        if (Input.GetKeyDown("i")&& inShop == true)
        {
            ShopHide();
        }
    }

    public void ShopHide()
    {
        ShopAccess.SetActive(false);
        ShopList.SetActive(false);
        Inventory.SetActive(false);
        Cursor.visible = false;
        Body.GetComponent<ControllerBody>().timeFactor = 1;
        inShop = false;
    }

    public void updateTrigger(List<Item> UpdateList)
    {
        TraderList = UpdateList;

        for (int i = TraderList.Count; i < MaxCountItems; i++)
        {
            TraderList.Add(new Item());
        }

        Inventory.SetActive(true);
        Cursor.visible = true;
        Body.GetComponent<ControllerBody>().timeFactor = 0;
        inShop = true;
        ShopList.SetActive(true);
        DisplayTrader();
    }
    public void DisplayTrader()
    {
        for(int i = 0; i< MaxCountItems; i++)
        {
            Transform cell = ShopList.transform.GetChild(i);         //Ее картинку и количество
            Transform icon = cell.GetChild(0);
            Transform count = icon.GetChild(0);
            
            Text txt = count.GetComponent<Text>();
            Image img = icon.GetComponent<Image>();
            if(TraderList[i].countItem != 0 && TraderList[i].id != 0)                            //Если предметов в ячейке больше 0
            {
                img.enabled = true;                               //делаем для неё картику этого предмета(метод additem это определяет)
                img.sprite = TraderList[i].icon; 
                if(TraderList[i].countItem > 1)                         //Если предметов больше 1, то показывает их колтчество
                txt.text = TraderList[i].countItem.ToString();
                else
                txt.text = "";
            }
            else                                         //Если 0, то удаляет картинку
            {
                img.enabled = false;
                img.sprite = null;
                txt.text = "";
            }
        }
    }
    public void LoadData(Save.PlayerSaveData save)
    {
        Money = save.Money;
    }
}

