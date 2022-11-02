using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlchVisible : MonoBehaviour
{
    [HideInInspector]
    public List<Item> alch;
    [HideInInspector]
    public List<int> CraftId;
    [HideInInspector]
    public List<int> CraftCount;
    public GameObject AlchList;
    public Inventory inventory;
    [HideInInspector]
    public bool inAlch = false;
    public GameObject Body;
    public GameObject Inventory;
    public GameObject AlchMain;
    public GameObject ItemDataBase;
    public GameObject F;
    public Skill_Indicator Skills;
    [HideInInspector]

    private List<Craft> Crafts;

    private List<Craft> CraftsAchemy;
    private List<Craft> CraftsArmor;

    public int AlchLvl;
    public int CraftArmorLvl;
    
    private int CraftLvl;
    

    void Start()
    {
        for(int i = 0; i < AlchList.transform.childCount; i++)
        {
            AlchCurrent AC = AlchList.transform.GetChild(i).GetComponent<AlchCurrent>();
            alch.Add(new Item());
            AC.cell = i;  
            AC.Alch = GetComponent<AlchVisible>();
            AC.inventory = inventory;
        }
        CraftsAchemy = gameObject.GetComponent<CraftList>().alch;
        CraftsArmor = gameObject.GetComponent<CraftList>().arm;
    }
    void Update()
    {
        AlchLvl = Skills.AlchLvl;
        CraftArmorLvl = Skills.MetalLvl;
        if(Input.GetKeyDown("i") || Input.GetKeyDown(KeyCode.Escape))
        {
            inAlch = false;

            for(int j = 0; j < alch.Count; j++)
            {
                if(alch[j].id!=0)
                    inventory.AddItem(alch[j].id, alch[j].countItem, alch[j].inventoryList);
                alch[j] = new Item();
            }
            DisplayAll();
        }
    }

    void OpenClose()
    {
        Inventory.SetActive(true);
        Cursor.visible = true;
        Body.GetComponent<ControllerBody>().timeFactor = 0;
        inAlch = true;
        AlchMain.SetActive(true);
       
       AlchMain.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = CraftLvl.ToString();
    }

    public void AddIngredient(Item add)
    {
        for(int i = 0; i < alch.Count-1; i++)
        {
            if(alch[i].id == add.id)
            {
                alch[i].countItem++;
                break;
            }

            if(alch[i].id == 0)
            {
                alch[i].id = add.id;
                alch[i].countItem = 1;
                alch[i].Cost = add.Cost;
                alch[i].Name = add.Name; 
                alch[i].isStackable = add.isStackable; 
                alch[i].discription = add.discription; alch[i].icon = add.icon; 
                alch[i].inventoryList = add.inventoryList;
                break;
            }
            
            if(i == alch.Count-2)
            {
                inventory.AddItem(add.id, 1, add.inventoryList);
            }
        }
        DisplayAll();
    }

    public void Craft_()
    {   
        Item[] Items = new Item[8]; //Массив предметов в крафте

        for(int i = 0; i <  alch.Count-1; i++)
        {
            Items[i] = alch[i];
        }
        
        int Result = DataBase(Items);
        Item ResultItem = new Item();

        Debug.Log(Result);

        for(int j = 0; j < ItemDataBase.transform.childCount; j++)                                                //Ищем предмет по id
        {
            if(ItemDataBase.transform.GetChild(j).GetComponent<Item>().id == Result)
            {
                ResultItem = ItemDataBase.transform.GetChild(j).GetComponent<Item>();
                ResultItem.countItem = 1;
                break;
            }
        }

        if(ResultItem!=null)
        {
            for(int i = 0; i < alch.Count-1; i++)
            {
                alch[i] = new Item();
                
            }
            alch[alch.Count-1] = ResultItem;
        }
        
        DisplayAll();
    }

    public void DisplayAll()
    {
        for(int i = 0; i < AlchList.transform.childCount; i++)               //берём каждую ячейку отдельно
        {
            Transform cell = AlchList.transform.GetChild(i);         //Ее картинку и количество
            Transform icon = cell.GetChild(0);
            Transform count = icon.GetChild(0);
            
            Text txt = count.GetComponent<Text>();
            Image img = icon.GetComponent<Image>();
            if(alch[i].countItem != 0)                            //Если предметов в ячейке больше 0
            {
                img.enabled = true;                               //делаем для неё картику этого предмета(метод additem это определяет)
                img.sprite = alch[i].icon; 
                if(alch[i].countItem > 1)                         //Если предметов больше 1, то показывает их колтчество
                txt.text = alch[i].countItem.ToString();
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

    void OnTriggerStay(Collider other)           //Способ закибывания предмета в инвентарь
    {
        if(other.tag == "Alch" && Input.GetKey("f"))
        {
            Crafts = CraftsAchemy;
            CraftLvl = AlchLvl;
            F.SetActive(false);
            OpenClose();
        }

        if(other.tag == "CraftArmor" && Input.GetKey("f"))
        {
            Crafts = CraftsArmor;
            CraftLvl = CraftArmorLvl;
            F.SetActive(false);
            OpenClose();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Alch") F.SetActive(true);
        if(other.tag == "CraftArmor") F.SetActive(true);
    }
    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Alch") F.SetActive(false);
        if(other.tag == "CraftArmor") F.SetActive(false);
    }


    int DataBase(Item[] Items)
    {   
        CraftItem[] CraftItems = new CraftItem[8];

        for(int i = 0; i <8;i++)                        //Предметы из инвентаря преобразуются в структуру
        {
            if(Items[i].id!=null)
                CraftItems[i].ID = Items[i].id;
            else
                CraftItems[i].ID = 0;

            if(Items[i].id!=null)
                CraftItems[i].Count = Items[i].countItem;
            else
                CraftItems[i].Count = 0;
        }                                               //

        for(int i = 0; i < Crafts.Count; i++)
        {
            CraftItem[] CraftItemsVariant = new CraftItem[8];       ////Предметы для крафтов преобразуются в структуру

            for(int j = 0; j <8;j++)                                        
            {
                CraftItemsVariant[j].ID = Crafts[i].Items[j];
                CraftItemsVariant[j].Count = Crafts[i].ItemsCount[j];
            }                                                                   ///
            if(ArrayCompare(CraftItemsVariant,CraftItems) && Crafts[i].Lvl <= CraftLvl)
            {
                return Crafts[i].result;
            }
        }

        return 0;
    }

    private bool ArrayCompare(CraftItem[] first, CraftItem[] second)
    {
        if(first.Length!=second.Length)
            return false;

        for(int i = 0; i<first.Length;i++)
        {
            bool search = false;
            for(int j = 0; j<second.Length;j++)
            {
                if(first[i].ID == second[j].ID &&first[i].Count == second[j].Count)
                {
                    search = true;
                    break;
                }
            }
            if(search == false)
                return false;

            search = false;
            for(int j = 0; j<second.Length;j++)
            {
                if(first[j].ID == second[i].ID &&first[j].Count == second[i].Count)
                {
                    search = true;
                    break;
                }
            }
            if(search == false)
                return false;
        }

        return true;
    }

    struct CraftItem
    {
        public int ID;
        public int Count;
    }
}


