using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContaiterVisiable : MonoBehaviour
{
    public List<Item> ContanerItemList = new List<Item>();
    public GameObject ContainerUI;
    private int MaxCountItems;
    public GameObject InventoryUI;
    public bool inContainer;
    public GameObject Body;
    public GameObject ShopAccess;
    
    void Start()
    {
        inContainer = false;
        MaxCountItems = ContainerUI.transform.childCount;      


        for(int i = 0; i < MaxCountItems; i++)         
        {
            ContainerUI.transform.GetChild(i).GetComponent<ContainerCurrent>().cellNum = (byte)i;       //нумеруем ячейки
        }
    }

    public void AddItem(Item add, int count)
    {
        for(int i = 0; i < ContanerItemList.Count-1; i++)
        {
            if(ContanerItemList[i].id == add.id)
            {
                ContanerItemList[i].countItem++;
                break;
            }

            if(ContanerItemList[i].id == 0)
            {
                ContanerItemList[i].id = add.id;
                ContanerItemList[i].countItem = (byte)count;
                ContanerItemList[i].Cost = add.Cost;
                ContanerItemList[i].Name = add.Name; 
                ContanerItemList[i].isStackable = add.isStackable; 
                ContanerItemList[i].discription = add.discription; ContanerItemList[i].icon = add.icon; 
                ContanerItemList[i].inventoryList = add.inventoryList;
                break;
            }
        }
        DisplayContainer();
    }

    void Update()
    {
        if (Input.GetKeyDown("i")&& inContainer == true)
        {   
            ContainerHide();
        }
    }

    public void ContainerHide ()
    {
        ShopAccess.SetActive(false);
        ContainerUI.SetActive(false);
        InventoryUI.SetActive(false);
        Cursor.visible = false;
        Body.GetComponent<ControllerBody>().timeFactor = 1;
        inContainer = false;
    }

    public void UpdateContainer(List<Item> UpdateList)
    {
        ContanerItemList = UpdateList;

        for (int i = ContanerItemList.Count; i < MaxCountItems; i++)
        {
            ContanerItemList.Add(new Item());
        }

        InventoryUI.SetActive(true);
        Cursor.visible = true;
        Body.GetComponent<ControllerBody>().timeFactor = 0;
        inContainer = true;
        ContainerUI.SetActive(true);
        DisplayContainer();
    }

    public void DisplayContainer()
    {
        for(int i = 0; i< MaxCountItems; i++)
        {
            Transform cell = ContainerUI.transform.GetChild(i);         //Ее картинку и количество
            Transform icon = cell.GetChild(0);
            Transform count = icon.GetChild(0);
            
            Text txt = count.GetComponent<Text>();
            Image img = icon.GetComponent<Image>();
            if(ContanerItemList[i].countItem != 0 && ContanerItemList[i].id != 0)                            //Если предметов в ячейке больше 0
            {
                img.enabled = true;                               //делаем для неё картику этого предмета(метод additem это определяет)
                img.sprite = ContanerItemList[i].icon; 
                if(ContanerItemList[i].countItem > 1)                         //Если предметов больше 1, то показывает их колтчество
                txt.text = ContanerItemList[i].countItem.ToString();
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
}

