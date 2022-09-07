using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    public GameObject F;
    [Tooltip("Имеющийся лут")]

    public List<Item> ExistItems = new List<Item>();
    public List<byte> ItemsCount = new List<byte>();
    private List<Item> ExistItem = new List<Item>();
    [Space][Tooltip("Генерерующийся лут")]
    public bool RandomLoot;
    public List<Item> Scrapitems = new List<Item>();
    private List<Item> Scrapitem = new List<Item>();
    public List<Item> items = new List<Item>();
    private List<Item> item = new List<Item>();
    public List<Item> Rareitems= new List<Item>();
    private List<Item> Rareitem= new List<Item>();
    public List<Item> VeryRareitems = new List<Item>();
    private List<Item> VeryRareitem = new List<Item>();

    public List<Item> ContainerInventory;
    public GameObject ContainerVis;

    int numInv = 0;
    void Start()
    {
        ContainerInventory = new List<Item>();

        ExistItem = ListItemCopy(ExistItems);
        for (int i = 0; i < ExistItem.Count; i++)
        {
            ExistItems[i].countItem = ItemsCount[i];
            ContainerInventory.Add(ExistItems[i]);
        }

        if(RandomLoot)
        {
            Scrapitem = ListItemCopy(Scrapitems);
            item = ListItemCopy(items);
            VeryRareitem = ListItemCopy(VeryRareitems);
            Rareitem = ListItemCopy(Rareitems);

            for (int i = 0; i < Scrapitem.Count; i++)
            {
                Scrapitem[i].countItem = (byte)Random.Range(0,4);          
                if (Scrapitem[i].countItem != 0)
                {
                    Scrapitem[i].countItem = (byte)Random.Range(0,10);
                    ContainerInventory.Add(Scrapitem[i]);
                }
                else  Scrapitem[i].countItem = 0;
            }
            for (int i = 0; i < item.Count; i++)
            {
                item[i].countItem =(byte) Random.Range(0,2);   
                if (item[i].countItem == 1)
                {
                    item[i].countItem = (byte)Random.Range(0,3);
                    ContainerInventory.Add(item[i]);
                }
                else  item[i].countItem = 0;
                
            }

            for (int i = 0; i < Rareitem.Count; i++)
            {
                Rareitem[i].countItem = (byte)Random.Range(0,3);
                
                if (Rareitem[i].countItem == 1)
                {
                    Rareitem[i].countItem = (byte)Random.Range(0,4);
                    ContainerInventory.Add(Rareitem[i]);
                }
                else  Rareitem[i].countItem = 0;
                
            }

            for (int i = 0; i < VeryRareitem.Count; i++)
            {
                VeryRareitem[i].countItem = (byte)Random.Range(0,3);
        
                if (VeryRareitem[i].countItem == 1)
                {
                    VeryRareitem[i].countItem = (byte)Random.Range(0,2);
                    ContainerInventory.Add(VeryRareitem[i]);
                }
                else  VeryRareitem[i].countItem = 0;
                
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "PlayerHandCol" && Input.GetKeyDown("f"))
        {
            F.SetActive(false);
            ContainerVis.GetComponent<ContaiterVisiable>().UpdateContainer(ContainerInventory);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PlayerHandCol")
        {
            F.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "PlayerHandCol")
        {
            F.SetActive(false);
        }
    }

    List<Item> ListItemCopy(List<Item> list)
    {
        List<Item> Items = new List<Item>();

        for(int i = 0; i < list.Count;i++)
        {
            Item add = new Item();
            add.id = list[i].id;
            add.countItem = 1;
            add.Cost = list[i].Cost;
            add.Name = list[i].Name; 
            add.isStackable = list[i].isStackable; 
            add.discription = list[i].discription; add.icon = list[i].icon; 
            add.inventoryList = list[i].inventoryList;
            Items.Add(add);
        }
        return Items;
    }
}
