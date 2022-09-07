using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CurrentQuest : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
{
    public byte cell;
    public Transform PlayerDrop;
    public GameObject handcol;
    public GameObject Hand;
    public GameObject Hand2;
    public GameObject InfoPanel;
    private GameObject InventoryPanel;
    public GameObject ShopAccess;
    public Slider SliderShopAccess;
    public ButtonAccessrShop ButtonAccessShop;
    public GameObject Player;
    public ContaiterVisiable CV;
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
            if (inventory.inInv && inventory.quest[cell].dropping)
            {
                if (inventory.quest[cell].countItem != 0)                                 // Cоздаёт выброшенный объект, если их больше 0
                {
                    Item quest = new Item();
                    for(int i = 0; i<inventory.dataBase.transform.childCount; i++)
                    {       
                        quest = inventory.dataBase.transform.GetChild(i).GetComponent<Item>();
                        if (quest && inventory.quest[cell].id == quest.id)
                        {
                            if(Input.GetKey("left shift"))                              //Если зажата shift то выкинуть все предметы
                                quest.countItem = inventory.quest[cell].countItem;
                            else
                                quest.countItem = 1;                                       //Иначе только 1

                            GameObject droppedObj = Instantiate(quest.gameObject);
                            droppedObj.transform.position = handcol.transform.position;
                            droppedObj.transform.SetParent(PlayerDrop);
                            droppedObj.transform.GetComponent<Rigidbody>().isKinematic = false;

                            if(Input.GetKey("left shift"))
                            inventory.item[cell] = new Item();
                            else
                            {
                                if (inventory.quest[cell].countItem > 1)                       //уменьшает кольчество обьеутов при выбрасывании на 1, если их больше 1
                                {
                                    inventory.quest[cell].countItem--;
                                }
                                else 
                                inventory.quest[cell] = new Item();
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
            if(inventory.quest[cell].isUsiable && inventory.inInv)
            {
                inventory.quest[cell].customEvent.Invoke();                         //Запустить ивент

                if(inventory.quest[cell].infUsiable == false)
                {
                    if(inventory.quest[cell].countItem > 1)
                        inventory.quest[cell].countItem--;   

                    else 
                        inventory.quest[cell] = new Item();

                    inventory.DisplayItems ();
                }
            }

            if (inventory.quest[cell].isSell && shop.inShop)
            {
                if(Input.GetKey("left shift"))                              //Если зажата shift то продать все предметы 
                {
                    ShopAccess.SetActive(true);//
                    SliderShopAccess.maxValue = inventory.quest[cell].countItem;//
                    ButtonAccessShop.InventoryType = 2;
                    ButtonAccessShop.Cell = cell; 
                    ButtonAccessShop.shop = true;  
                    //shop.Money += (int)(inventory.quest[cell].Cost * (SkillIndicator.CostFactor/100)* inventory.quest[cell].countItem);
                    //inventory.quest[cell] = new Item();
                }
                else
                {
                    if(inventory.quest[cell].countItem > 1)
                    {
                        shop.Money += (int)(inventory.quest[cell].Cost * (SkillIndicator.CostFactor/100));
                        inventory.quest[cell].countItem--;
                    }
                    else
                    {
                        shop.Money += (int)(inventory.quest[cell].Cost * (SkillIndicator.CostFactor/100));
                        inventory.quest[cell] = new Item();
                    }
                    inventory.DisplayItems();
                }
               
            }

            if(inventory.quest[cell].dropping && alch.inAlch)
            {
                if(inventory.quest[cell].countItem > 1)
                {
                    alch.AddIngredient(inventory.quest[cell]);
                    inventory.quest[cell].countItem--;
                }
                else
                {
                    alch.AddIngredient(inventory.quest[cell]);
                    inventory.quest[cell] = new Item();
                }
                inventory.DisplayItems();
            }

            if(inventory.quest[cell].dropping && cont.inContainer)
            {
                if(Input.GetKey("left shift"))                              //Если зажата shift то продать все предметы
                {
                    ShopAccess.SetActive(true);//
                    SliderShopAccess.maxValue = inventory.quest[cell].countItem;//
                    ButtonAccessShop.InventoryType = 1;
                    ButtonAccessShop.Cell = cell;   
                    ButtonAccessShop.shop = false;
                    //shop.Money += (int)(inventory.item[cell].Cost * (SkillIndicator.CostFactor/100)* inventory.item[cell].countItem);
                    //inventory.item[cell] = new Item();
                }
                else
                {
                    if(inventory.quest[cell].countItem > 1)
                    {
                        cont.AddItem(inventory.quest[cell],1);
                        inventory.quest[cell].countItem--;
                    }
                    else
                    {
                        cont.AddItem(inventory.quest[cell],1);
                        inventory.quest[cell] = new Item();
                    }
                    inventory.DisplayItems();
                }
            }
        }
        if(eventData.button == PointerEventData.InputButton.Middle)
        {
            if(change.ItemChange.id == 0)
            {
                change.ItemChange = inventory.quest[cell];
                change.CellNumber = cell;
            }
            else
            {
                if(cell == change.CellNumber)
                {
                    inventory.DisplayItems ();
                    StartCoroutine(q1123());
                    return;
                }
                if(change.ItemChange.id == inventory.item[cell].id && change.ItemChange.isStackable == true)
                {
                    inventory.quest[cell].countItem+=change.ItemChange.countItem;
                    inventory.quest[change.CellNumber] = new Item();
                }
                else
                {
                    inventory.quest[change.CellNumber] = inventory.quest[cell];
                    inventory.quest[cell] = change.ItemChange;
                }
                
                inventory.DisplayItems ();
                StartCoroutine(q1123());
            }
        }
    }

    public void AccessShopButton()
    {
        shop.Money += (int)(inventory.quest[ButtonAccessShop.Cell].Cost * (SkillIndicator.CostFactor/100)* ButtonAccessShop.Count);

        inventory.quest[ButtonAccessShop.Cell].countItem -= ButtonAccessShop.Count;
        if(inventory.quest[ButtonAccessShop.Cell].countItem<=0)
            inventory.quest[ButtonAccessShop.Cell] = new Item();

        inventory.DisplayItems();
    }

    public void AccessContainerButton()
    {
        CV.AddItem(inventory.quest[ButtonAccessShop.Cell], ButtonAccessShop.Count);
        inventory.quest[ButtonAccessShop.Cell].countItem -= ButtonAccessShop.Count;
        if(inventory.quest[ButtonAccessShop.Cell].countItem<=0)
            inventory.quest[ButtonAccessShop.Cell] = new Item();
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
        if(inventory.quest[cell].id != 0)
        {
            InfoPanel.SetActive(true);
        
            Transform iconInfo = InfoPanel.transform.GetChild(0);
            Image icon_Info = iconInfo.GetComponent<Image>();
            icon_Info.sprite = inventory.quest[cell].icon;

            Transform nameInfo = InfoPanel.transform.GetChild(1);
            Text name_Info = nameInfo.GetComponent<Text>();
            name_Info.text = inventory.quest[cell].Name;

            Transform discInfo = InfoPanel.transform.GetChild(2);
            Text disc_Info = discInfo.GetComponent<Text>();
            disc_Info.text = inventory.quest[cell].discription;

            Transform costInfo = InfoPanel.transform.GetChild(3);
            Text cost_Info = costInfo.GetChild(0).GetComponent<Text>();
            cost_Info.text = ((int)(inventory.quest[cell].Cost * (SkillIndicator.CostFactor/100))).ToString();
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        InfoPanel.SetActive(false);
    }

}
