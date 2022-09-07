using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CurrentItem : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
{
    public byte cell;
    public Transform PlayerDrop;
    public GameObject handcol;
    public GameObject Hand;
    public GameObject Hand2;
    public GameObject InfoPanel;
    private GameObject InventoryPanel;
    public GameObject Player;
    public GameObject ShopAccess;
    public ContaiterVisiable CV;

    public Slider SliderShopAccess;
    public ButtonAccessrShop ButtonAccessShop;

    Inventory inventory;
    ShopVisiable shop;
    public AlchVisible alch;
    public ContaiterVisiable cont;
    private Skill_Indicator SkillIndicator;
    InventoryChange change;
    Text EquipText;
    void Start()
    {
        EquipText = transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Text>();
        inventory = handcol.GetComponent<Inventory>();
        InfoPanel.SetActive(false);
        SkillIndicator = Player.GetComponent<Skill_Indicator>();
        InventoryPanel = gameObject.transform.parent.gameObject;
        shop = handcol.GetComponent<ShopVisiable>();
        change = InventoryPanel.GetComponent<InventoryChange>();
        alch = handcol.GetComponent<AlchVisible>();
        cont = handcol.GetComponent<ContaiterVisiable>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)       //нажатие на ПКМ(удаление)
        {
            if (inventory.inInv && inventory.item[cell].dropping)
            {
                if (inventory.item[cell].countItem != 0)                                 // Cоздаёт выброшенный объект, если их больше 0
                {
                    Item item = new Item();
                    for(int i = 0; i<inventory.dataBase.transform.childCount; i++)
                    {
                        item = inventory.dataBase.transform.GetChild(i).GetComponent<Item>();
                        if (item && inventory.item[cell].id == item.id)
                        {
                            if(Input.GetKey("left shift"))                              //Если зажата shift то выкинуть все предметы
                                item.countItem = inventory.item[cell].countItem;
                            else
                                item.countItem = 1;                                       //Иначе только 1

                            GameObject droppedObj = Instantiate(item.gameObject);
                            droppedObj.transform.position = handcol.transform.position;
                            droppedObj.transform.SetParent(PlayerDrop);
                            droppedObj.transform.GetComponent<Rigidbody>().isKinematic = false;

                            if(Input.GetKey("left shift"))
                            inventory.item[cell] = new Item();
                            else
                            {
                                if (inventory.item[cell].countItem > 1)                       //уменьшает кольчество обьеутов при выбрасывании на 1, если их больше 1
                                {
                                    inventory.item[cell].countItem--;
                                }
                                else 
                                inventory.item[cell] = new Item();
                            }
                            inventory.DisplayItems  ();
                            return;
                        }
                    }
                }
            }
        }



        if(eventData.button == PointerEventData.InputButton.Left)                   //нажатие на ЛКМ(использование)
        {
            if(inventory.item[cell].isUsiable && inventory.inInv)
            {
                inventory.item[cell].customEvent.Invoke();                         //Запустить ивент

                if(inventory.item[cell].infUsiable == false)
                {

                    if(inventory.item[cell].countItem > 1)
                        inventory.item[cell].countItem--;

                    else 
                        inventory.item[cell] = new Item();

                    inventory.DisplayItems ();
                }
            }


            if (inventory.item[cell].isSell && shop.inShop)
            {
                if(Input.GetKey("left shift"))                              //Если зажата shift то продать все предметы
                {
                    ShopAccess.SetActive(true);//
                    SliderShopAccess.maxValue = inventory.item[cell].countItem;//
                    ButtonAccessShop.InventoryType = 1;
                    ButtonAccessShop.Cell = cell;   
                    ButtonAccessShop.shop = true;
                    //shop.Money += (int)(inventory.item[cell].Cost * (SkillIndicator.CostFactor/100)* inventory.item[cell].countItem);
                    //inventory.item[cell] = new Item();
                }
                else
                {
                    if(inventory.item[cell].countItem > 1)
                    {
                        shop.Money += (int)(inventory.item[cell].Cost * (SkillIndicator.CostFactor/100));
                        inventory.item[cell].countItem--;
                    }
                    else
                    {
                        shop.Money += (int)(inventory.item[cell].Cost * (SkillIndicator.CostFactor/100));
                        inventory.item[cell] = new Item();
                    }
                    inventory.DisplayItems();
                }   
            }


            if(inventory.item[cell].dropping && alch.inAlch)
            {
                if(inventory.item[cell].countItem > 1)
                {
                    alch.AddIngredient(inventory.item[cell]);
                    inventory.item[cell].countItem--;
                }
                else
                {
                    alch.AddIngredient(inventory.item[cell]);
                    inventory.item[cell] = new Item();
                }
                inventory.DisplayItems();
            
            
            }
            
            if(inventory.item[cell].dropping && cont.inContainer)
            {
                if(Input.GetKey("left shift"))                              //Если зажата shift то продать все предметы
                {
                    ShopAccess.SetActive(true);//
                    SliderShopAccess.maxValue = inventory.item[cell].countItem;//
                    ButtonAccessShop.InventoryType = 1;
                    ButtonAccessShop.Cell = cell;   
                    ButtonAccessShop.shop = false;
                    //shop.Money += (int)(inventory.item[cell].Cost * (SkillIndicator.CostFactor/100)* inventory.item[cell].countItem);
                    //inventory.item[cell] = new Item();
                }
                else
                {
                    if(inventory.item[cell].countItem > 1)
                    {
                        cont.AddItem(inventory.item[cell], 1);
                        inventory.item[cell].countItem--;
                    }
                    else
                    {
                        cont.AddItem(inventory.item[cell], 1);
                        inventory.item[cell] = new Item();
                    }
                }
                inventory.DisplayItems();
            }
        }
            if(eventData.button == PointerEventData.InputButton.Middle)
            {
                if(change.ItemChange.id == 0)
                {
                    change.ItemChange = inventory.item[cell];
                    change.CellNumber = cell;
                }
                else
                {
                    if(cell == change.CellNumber)
                    {
                        inventory.DisplayItems();
                        StartCoroutine(q1123());
                        return;
                    }
                    if(change.ItemChange.id == inventory.item[cell].id && change.ItemChange.isStackable == true)
                    {
                        inventory.item[cell].countItem+=change.ItemChange.countItem;
                        inventory.item[change.CellNumber] = new Item();
                    }
                    else
                    {
                        inventory.item[change.CellNumber] = inventory.item[cell];
                        inventory.item[cell] = change.ItemChange;
                    }

                    inventory.DisplayItems ();
                    StartCoroutine(q1123());
                }
            }
        }
    

    public void AccessShopButton()
    {
        shop.Money += (int)(inventory.item[ButtonAccessShop.Cell].Cost * (SkillIndicator.CostFactor/100)* ButtonAccessShop.Count);

        inventory.item[ButtonAccessShop.Cell].countItem -= ButtonAccessShop.Count;
        if(inventory.item[ButtonAccessShop.Cell].countItem<=0)
            inventory.item[ButtonAccessShop.Cell] = new Item();

        inventory.DisplayItems();
    }

    public void AccessContainerButton()
    {
        CV.AddItem(inventory.item[ButtonAccessShop.Cell], ButtonAccessShop.Count);
        inventory.item[ButtonAccessShop.Cell].countItem -= ButtonAccessShop.Count;
        if(inventory.item[ButtonAccessShop.Cell].countItem<=0)
            inventory.item[ButtonAccessShop.Cell] = new Item();
        inventory.DisplayItems();
    }

    IEnumerator q1123()
    {
        yield return new WaitForSeconds(0.1f);
        change.ItemChange = new Item();
        change.Equip = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(inventory.item[cell].id != 0)
        {
            InfoPanel.SetActive(true);
        
            Transform iconInfo = InfoPanel.transform.GetChild(0);
            Image icon_Info = iconInfo.GetComponent<Image>();
            icon_Info.sprite = inventory.item[cell].icon;

            Transform nameInfo = InfoPanel.transform.GetChild(1);
            Text name_Info = nameInfo.GetComponent<Text>();
            name_Info.text = inventory.item[cell].Name;

            Transform discInfo = InfoPanel.transform.GetChild(2);
            Text disc_Info = discInfo.GetComponent<Text>();
            disc_Info.text = inventory.item[cell].discription;

            Transform costInfo = InfoPanel.transform.GetChild(3);
            Text cost_Info = costInfo.GetChild(0).GetComponent<Text>();
            cost_Info.text = ((int)(inventory.item[cell].Cost * (SkillIndicator.CostFactor/100))).ToString();
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        InfoPanel.SetActive(false);
    }
}