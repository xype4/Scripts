using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    // [HideInInspector]
    public List<Item> item;             //Создаём лист, для хранения предметов(в инвентаре, у каждого предмета есть характеристика из Item)
    public List<Item> quest;             //Создаём лист, для хранения предметов квестов(в инвентаре, у каждого предмета есть характеристика из Item)
    public List<Item> equip;             //Создаём лист, для хранения предметов экипировки(в инвентаре, у каждого предмета есть характеристика из Item)
    public GameMenu Menu;
    public GameObject MainInventory;        //Основное меню инвентаря
    public GameObject cellContainerItem;    //Меню инвентаря, в котором дочерние ячейки
    public GameObject cellContainerQuest;    //Меню инвентаря, в котором дочерние ячейки
    public GameObject cellContainerEquip;    //Меню инвентаря, в котором дочерние ячейки
    public GameObject Alchemy;
    private byte cellCount;          //количество ячеек для цикла, присваиваются след значения
    private byte cellCountitem;          //Количество предметов
    private byte cellCountquest;          //Количество предметов квеста
    private byte cellCountequip;          //Количество предметов экипировки
    public GameObject dataBase;               //Объект с всеми предметами в игре
    public GameObject F;
    //private bool AddIn = false;
    public bool inInv = false;
    public GameObject Body;
    void Start()                                                                                       //По порядку заполняем страницы инвентаря нулевыми предметами для листа             
    {
        cellCountitem = (byte)cellContainerItem.transform.childCount;                     
        item = new List<Item>();                                                        
        for(byte i = 0; i < cellCountitem; i++)                                           
        {
            item.Add(new Item());
            cellContainerItem.transform.GetChild(i).GetComponent<CurrentItem>().cell = i;      
        }

        cellCountquest = (byte)cellContainerQuest.transform.childCount;                                
        quest = new List<Item>();                                    
        for(byte i = 0; i < cellCountquest; i++)                                               
        {
            quest.Add(new Item());
            cellContainerQuest.transform.GetChild(i).GetComponent<CurrentQuest>().cell = i;     
        }

        cellCountequip = (byte)cellContainerEquip.transform.childCount;                               
        equip = new List<Item>();                                                  
        for(byte i = 0; i < cellCountequip; i++)                                  
        {
            equip.Add(new Item());
            cellContainerEquip.transform.GetChild(i).GetComponent<CurrentEquipment>().cell = i;     
        }

        MainInventory.SetActive(false);
    }
    void Update()
    {
        if(MainInventory.activeSelf)
        {
            Cursor.visible = true;
            Body.GetComponent<ControllerBody>().timeFactor = 0;
        }
        
        if (Input.GetKeyDown("i") && gameObject.GetComponent<ContaiterVisiable>().inContainer == false && gameObject.GetComponent<ShopVisiable>().inShop == false)                                     // Открытие/закрытие инвентаря
        {
            Alchemy.SetActive(false);
            if(MainInventory.activeSelf)
            {
                InventoryHide();
            }
            else
            {
                MainInventory.SetActive(true);
                Cursor.visible = true;
                Body.GetComponent<ControllerBody>().timeFactor = 0;
                inInv = true;
            } 
        }
    }

    public void InventoryHide()
    {
        MainInventory.SetActive(false);
        F.SetActive(false);
        Cursor.visible = false;
        Body.GetComponent<ControllerBody>().timeFactor = 1;
        inInv = false;
    }

    public void DisplayItems()                              //Обновление картинок предметов
    {
        for(int i = 0; i < cellCountequip; i++)               //берём каждую ячейку отдельно
        {
            Transform cell = cellContainerEquip.transform.GetChild(i);         //Ее картинку и количество
            Transform icon = cell.GetChild(0);
            Transform count = icon.GetChild(0);
            
            Text txt = count.GetComponent<Text>();
            Image img = icon.GetComponent<Image>();
            if(equip[i].countItem != 0 && equip[i].id != 0)                            //Если предметов в ячейке больше 0
            {
                img.enabled = true;                               //делаем для неё картику этого предмета(метод additem это определяет)
                img.sprite = equip[i].icon; 
                if(equip[i].TypeEquip == 0)
                {
                    if(equip[i].countItem > 1)                         //Если предметов больше 1, то показывает их колтчество
                    txt.text = equip[i].countItem.ToString();
                    else
                    txt.text = "";
                }
            }
            else                                         //Если 0, то удаляет картинку
            {
                img.enabled = false;
                img.sprite = null;
                txt.text = "";
                equip[i].id = 0;
            }
        }


        for(int i = 0; i < cellCountquest; i++)               //берём каждую ячейку отдельно
        {
            Transform cell = cellContainerQuest.transform.GetChild(i);         //Ее картинку и количество
            Transform icon = cell.GetChild(0);
            Transform count = icon.GetChild(0);
            
            Text txt = count.GetComponent<Text>();
            Image img = icon.GetComponent<Image>();
            if(quest[i].countItem != 0 && quest[i].id != 0)                            //Если предметов в ячейке больше 0
            {
                img.enabled = true;                               //делаем для неё картику этого предмета(метод additem это определяет)
                img.sprite = quest[i].icon; 
                if(quest[i].TypeEquip == 0)
                {
                    if(quest[i].countItem > 1)                         //Если предметов больше 1, то показывает их колтчество
                    txt.text = quest[i].countItem.ToString();
                    else
                    txt.text = "";
                }
            }
            else                                         //Если 0, то удаляет картинку
            {
                img.enabled = false;
                img.sprite = null;
                txt.text = "";
            }
        }

        
         for(int i = 0; i < cellCountitem; i++)               //берём каждую ячейку отдельно
        {
            Transform cell = cellContainerItem.transform.GetChild(i);         //Ее картинку и количество
            Transform icon = cell.GetChild(0);
            Transform count = icon.GetChild(0);
            
            Text txt = count.GetComponent<Text>();
            Image img = icon.GetComponent<Image>();
            if(item[i].countItem != 0 && item[i].id != 0)                            //Если предметов в ячейке больше 0
            {
                img.enabled = true;                               //делаем для неё картику этого предмета(метод additem это определяет)
                img.sprite = item[i].icon; 
                if(item[i].TypeEquip == 0)
                {
                    if(item[i].countItem > 1)                         //Если предметов больше 1, то показывает их колтчество
                    txt.text = item[i].countItem.ToString();
                    else
                    txt.text = "";
                }
            }
            else                                         //Если 0, то удаляет картинку
            {
                img.enabled = false;
                img.sprite = null;
                txt.text = "";
            }
        }
    }
    public void AddItem(int id_, byte count_, byte TypeInv)
    {
        switch (TypeInv)
        {
            case 1:
                cellCount = cellCountitem;
                AddItem2(id_, count_, item);
                break;
            case 2:
                cellCount = cellCountquest;
                AddItem2(id_, count_, quest);
                break;
            case 3:
                cellCount = cellCountequip;
                AddItem2(id_, count_, equip);
                break;
        }
    }

    public void AddItem2(int id_, byte count_, List<Item> item)
    {
        for(int i = 0; i < cellCount; i++)                          //берём каждую ячейку отдельно
        {
            if(item[i].id == id_ && item[i].isStackable == true && count_ != 0 && item[i].countItem < 255)       //Если предмет скатается и его ID найден в инвентаре, то их количество учеличиваетя на количество добавленных предметов(определяется самим предметом)
            {
                if(255 - item[i].countItem <= count_)
                {
                    item[i].countItem = 255;
                    int c = count_- (255 - item[i].countItem);
                    AddItem(id_, (byte)c, (byte)(item[i].inventoryList));       
                    break;      
                }
                item[i].countItem += count_;

                DisplayItems();
                break;
            }

            if(item[i].id == 0 && count_ != 0)                   //Если ячека с 0 ид, то добавляем в неё предмет
            {
                for(int j = 0; j < dataBase.transform.childCount; j++)                                                //заносим в него пустые объекты
                {
                    item[i].id = id_;
                    item[i].countItem = count_;
                    if(dataBase.transform.GetChild(j).GetComponent<Item>().id == item[i].id)
                    {
                        Item add = dataBase.transform.GetChild(j).GetComponent<Item>();
                        item[i].Cost = add.Cost;
                        item[i].Name = add.Name; 
                        item[i].isStackable = add.isStackable; item[i].isSell = add.isSell; item[i].isUsiable = add.isUsiable; 
                        item[i].infUsiable = add.infUsiable; item[i].dropping = add.dropping;
                        item[i].discription = add.discription; item[i].icon = add.icon; item[i].customEvent = add.customEvent; 
                        item[i].TypeEquip = add.TypeEquip; item[i].Type = add.Type;
                        item[i].inventoryList = add.inventoryList;
                        break;
                    }
                }

                DisplayItems();
                break;
            }
        }
    }

    void OnTriggerStay(Collider other)           //Способ закибывания предмета в инвентарь
    {
        if(other.GetComponent<Item>() && Input.GetKey("f"))
        {
            // if(other.GetComponent<Item>().id == 11) other.GetComponent<Item>().id=10;     // Если 1(7) монет, id 11, то добавить 7 монет с id 10  (старые золотые монеты)
            AddItem(other.GetComponent<Item>().id, other.GetComponent<Item>().countItem, other.GetComponent<Item>().inventoryList);
            Exit_F();
            Destroy(other.gameObject);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Item>()) 
            F.SetActive(true);
    }
    void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<Item>()) 
            F.SetActive(false);
    }
    void Exit_F()
    {
        F.SetActive(false);
    }

    public void LoadData(Save.PlayerSaveData save)
    {
        for(int i = 0; i < cellContainerItem.transform.childCount; i++)
            AddItem(save.InventoryItem[i].ID, save.InventoryItem[i].Count, 1);

        for(int i = 0; i < cellContainerQuest.transform.childCount; i++)
            AddItem(save.InventoryQuest[i].ID, save.InventoryQuest[i].Count, 2);

        for(int i = 0; i < cellContainerEquip.transform.childCount; i++)
            AddItem(save.InventoryEquip[i].ID, save.InventoryEquip[i].Count, 3);

        for (int i = 0; i < save.EquipQuest.Count; i++)
            cellContainerQuest.transform.GetChild(save.EquipQuest[i]).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Text>().text = "E";
        
        for (int i = 0; i < save.EquipEquip.Count; i++)
            cellContainerEquip.transform.GetChild(save.EquipEquip[i]).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Text>().text = "E";
    }
}

