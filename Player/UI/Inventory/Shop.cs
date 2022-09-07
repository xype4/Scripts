using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject F;
    public List<Item> Scrapitems = new List<Item>();
    private List<Item> Scrapitem = new List<Item>();

    public List<Item> Shopitems = new List<Item>();
    private List<Item> Shopitem = new List<Item>();

    public List<Item> RareShopitems= new List<Item>();
    private List<Item> RareShopitem= new List<Item>();

    public List<Item> VeryRareShopitems = new List<Item>();
    private List<Item> VeryRareShopitem = new List<Item>();

    public List<Item> TraderInventory;
    public GameObject ShopVis;
    public Skill_Indicator SkillIndicator;
    private float cost;
    public float Reputation = 1;
    int numInv = 0;
    void Start()
    {
        TraderInventory = new List<Item>();
        Scrapitem = ListItemCopy(Scrapitems);
        Shopitem = ListItemCopy(Shopitems);
        VeryRareShopitem = ListItemCopy(VeryRareShopitems);
        RareShopitem = ListItemCopy(RareShopitems);

        for (int i = 0; i < Scrapitem.Count; i++)
        {
            Scrapitem[i].countItem = (byte)Random.Range(0,4);          
            if (Scrapitem[i].countItem != 0)
            {
                Scrapitem[i].countItem = (byte)Random.Range(0,10);
            }
            else  Scrapitem[i].countItem = 0;
        }
        for (int i = 0; i < Shopitem.Count; i++)
        {
            Shopitem[i].countItem =(byte) Random.Range(0,2);   
            if (Shopitem[i].countItem == 1)
            {
                Shopitem[i].countItem = (byte)Random.Range(0,3);
            }
            else  Shopitem[i].countItem = 0;
        }

        for (int i = 0; i < RareShopitem.Count; i++)
        {
            RareShopitem[i].countItem = (byte)Random.Range(0,3);
            
            if (RareShopitem[i].countItem == 1)
            {
                RareShopitem[i].countItem = (byte)Random.Range(0,4);
            }
            else  RareShopitem[i].countItem = 0;
        }

        for (int i = 0; i < VeryRareShopitem.Count; i++)
        {
            VeryRareShopitem[i].countItem = (byte)Random.Range(0,3);
    
            if (VeryRareShopitem[i].countItem == 1)
            {
                VeryRareShopitem[i].countItem = (byte)Random.Range(0,2);
            }
            else  VeryRareShopitem[i].countItem = 0;
        }

        CostItems();
    }

    public void CostItems()
    {
        cost = (1-SkillIndicator.Oratory/100) * Reputation * 3;
        
        for (int i = 0; i < Scrapitem.Count; i++)
        {
            Scrapitem[i].Cost = (ushort) (Scrapitem[i].Cost * cost);
            if (Scrapitem[i].countItem != 0) TraderInventory.Add(Scrapitem[i]);
        }
        for (int i = 0; i < Shopitem.Count; i++)
        {
            Shopitem[i].Cost = (ushort) (Shopitem[i].Cost * cost);
            if (Shopitem[i].countItem != 0) TraderInventory.Add(Shopitem[i]);
        }
        for (int i = 0; i < RareShopitem.Count; i++)
        {
            RareShopitem[i].Cost = (ushort) (RareShopitem[i].Cost * cost);
            if (RareShopitem[i].countItem != 0) TraderInventory.Add(RareShopitem[i]);
        }
        for (int i = 0; i < VeryRareShopitem.Count; i++)
        {
            VeryRareShopitem[i].Cost = (ushort) (VeryRareShopitem[i].Cost * cost);
            if (VeryRareShopitem[i].countItem != 0) TraderInventory.Add(VeryRareShopitem[i]);
        }
    }

    public void ReCost()
    {
        float OldCost = cost;
        cost = (1-SkillIndicator.Oratory/100) * Reputation * 3;
        for (int i = 0; i < TraderInventory.Count; i++)
        {
            TraderInventory[i].Cost = (ushort)(TraderInventory[i].Cost/OldCost*cost);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "PlayerHandCol" && Input.GetKeyDown("f"))
        {
            ReCost();
            F.SetActive(false);
            ShopVis.GetComponent<ShopVisiable>().updateTrigger(TraderInventory);
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
