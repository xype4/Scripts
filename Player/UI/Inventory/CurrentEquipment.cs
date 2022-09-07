using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CurrentEquipment : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
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
    public Slider SliderShopAccess;
    public ButtonAccessrShop ButtonAccessShop;
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
        EquipText = transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>();
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
            if (inventory.inInv && inventory.equip[cell].dropping)
            {
                if (inventory.equip[cell].countItem != 0)                                 // Cоздаёт выброшенный объект, если их больше 0
                {
                    Item equip = new Item();
                    for(int i = 0; i<inventory.dataBase.transform.childCount; i++)
                    {
                        equip = inventory.dataBase.transform.GetChild(i).GetComponent<Item>();
                        if (equip && inventory.equip[cell].id == equip.id)
                        {
                            if(Input.GetKey("left shift"))                              //Если зажата shift то выкинуть все предметы
                                equip.countItem = inventory.equip[cell].countItem;
                            else
                                equip.countItem = 1;                                       //Иначе только 1

                            GameObject droppedObj = Instantiate(equip.gameObject);
                            droppedObj.transform.position = handcol.transform.position;
                            droppedObj.transform.SetParent(PlayerDrop);
                            droppedObj.transform.GetComponent<Rigidbody>().isKinematic = false;
                            unEquip(inventory.equip[cell].id);

                            if(Input.GetKey("left shift"))
                                inventory.equip[cell] = new Item();
                            else
                            {
                                if (inventory.equip[cell].countItem > 1)                       //уменьшает кольчество обьеутов при выбрасывании на 1, если их больше 1
                                    inventory.equip[cell].countItem--;
                                else 
                                    inventory.equip[cell] = new Item();
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
            if(inventory.equip[cell].isUsiable && inventory.inInv)
            {
                if (InventoryPanel.transform.GetChild(cell).transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text == "E")
                {
                    unEquip(inventory.equip[cell].id);
                    return;
                }

                else
                {
                    inventory.equip[cell].customEvent.Invoke();                         //Запустить ивент

                    if(inventory.equip[cell].TypeEquip != 0 && !(inventory.equip[cell].TypeEquip == 2 && Hand.GetComponent<Player_Attack>().Sword.GetComponent<WeaponAttack>().WeapType ==2))
                    {
                        for(int i = 0 ; i < inventory.equip.Count; i++)
                        {
                            if(inventory.equip[i].TypeEquip == inventory.equip[cell].TypeEquip)
                            {
                                InventoryPanel.transform.GetChild(i).transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "";
                            }
                        }
                        EquipText.text = "E";
                    }
                }

                inventory.DisplayItems ();
                return;
            }


            if (inventory.equip[cell].isSell && shop.inShop)
            {
                unEquip(inventory.equip[cell].id);
                if(Input.GetKey("left shift"))                             //Если зажата shift то продать все предметы
                {
                    ShopAccess.SetActive(true);//
                    SliderShopAccess.maxValue = inventory.equip[cell].countItem;//
                    ButtonAccessShop.InventoryType = 3;
                    ButtonAccessShop.Cell = cell;   
                    ButtonAccessShop.shop = true;
                    //shop.Money += (int)(inventory.equip[cell].Cost * (SkillIndicator.CostFactor/100)* inventory.equip[cell].countItem);
                    //inventory.equip[cell] = new Item();
                }
                else
                {
                    if(inventory.equip[cell].countItem > 1)
                    {
                        shop.Money += (int)(inventory.equip[cell].Cost * (SkillIndicator.CostFactor/100));
                        inventory.equip[cell].countItem--;
                    }
                    else
                    {
                        shop.Money += (int)(inventory.equip[cell].Cost * (SkillIndicator.CostFactor/100));
                        inventory.equip[cell] = new Item();
                    }
                    inventory.DisplayItems();
                }
            }


            if(inventory.equip[cell].dropping && alch.inAlch)
            {
                unEquip(inventory.equip[cell].id);
                if(inventory.equip[cell].countItem > 1)
                {
                    alch.AddIngredient(inventory.equip[cell]);
                    inventory.equip[cell].countItem--;
                }
                else
                {
                    alch.AddIngredient(inventory.equip[cell]);
                    inventory.equip[cell] = new Item();
                }
                inventory.DisplayItems();
            }

            if(inventory.equip[cell].dropping && cont.inContainer)
            {
                if(Input.GetKey("left shift"))                              //Если зажата shift то продать все предметы
                {
                    ShopAccess.SetActive(true);//
                    SliderShopAccess.maxValue = inventory.equip[cell].countItem;//
                    ButtonAccessShop.InventoryType = 1;
                    ButtonAccessShop.Cell = cell;   
                    ButtonAccessShop.shop = false;
                    //shop.Money += (int)(inventory.item[cell].Cost * (SkillIndicator.CostFactor/100)* inventory.item[cell].countItem);
                    //inventory.item[cell] = new Item();
                }
                else
                {
                    unEquip(inventory.equip[cell].id);
                    if(inventory.equip[cell].countItem > 1)
                    {
                        cont.AddItem(inventory.equip[cell],1);
                        inventory.equip[cell].countItem--;
                    }
                    else
                    {
                        cont.AddItem(inventory.equip[cell],1);
                        inventory.equip[cell] = new Item();
                    }
                    inventory.DisplayItems();
                }              
            }
        }


        if(eventData.button == PointerEventData.InputButton.Middle)
        {
            if(change.ItemChange.id == 0)
            {
                change.ItemChange = inventory.equip[cell];
                change.CellNumber = cell;
                if(EquipText.text == "E")
                    change.Equip = true;
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
                    inventory.equip[cell].countItem+=change.ItemChange.countItem;
                    inventory.equip[change.CellNumber] = new Item();
                }
                else
                {
                    inventory.equip[change.CellNumber] = inventory.equip[cell];
                    if(EquipText.text == "E")
                        InventoryPanel.transform.GetChild(change.CellNumber).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Text>().text = "E";
                    else
                        InventoryPanel.transform.GetChild(change.CellNumber).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
                    inventory.equip[cell] = change.ItemChange;
                    if(change.Equip == true)
                        EquipText.text = "E";
                    else
                        EquipText.text = " ";
                }

                inventory.DisplayItems ();
                StartCoroutine(q1123());
            }
        }
    }

     public void AccessShopButton()
    {
        shop.Money += (int)(inventory.equip[ButtonAccessShop.Cell].Cost * (SkillIndicator.CostFactor/100)* ButtonAccessShop.Count);

        inventory.equip[ButtonAccessShop.Cell].countItem -= ButtonAccessShop.Count;
        if(inventory.equip[ButtonAccessShop.Cell].countItem<=0)
            inventory.equip[ButtonAccessShop.Cell] = new Item();

        inventory.DisplayItems();
    }

    public void AccessContainerButton()
    {
        CV.AddItem(inventory.equip[ButtonAccessShop.Cell], ButtonAccessShop.Count);
        inventory.equip[ButtonAccessShop.Cell].countItem -= ButtonAccessShop.Count;
        if(inventory.equip[ButtonAccessShop.Cell].countItem<=0)
            inventory.equip[ButtonAccessShop.Cell] = new Item();
        inventory.DisplayItems();
    }
    
    IEnumerator q1123()
    {
        yield return new WaitForSeconds(0.1f);
        change.ItemChange = new Item();
        change.Equip = false;
    }
    void unEquip(int id)                            //Если в руках предмет с id то он удаляется из рук
    {
        if(id == Hand.GetComponent<Player_Attack>().Sword.GetComponent<WeaponAttack>().WeapID && InventoryPanel.transform.GetChild(cell).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Text>().text == "E")
        {
            Hand.GetComponent<Player_Attack>().Sword.SetActive(false);
            Hand.GetComponent<Player_Attack>().Sword = Hand.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
            Hand.GetComponent<Player_Attack>().TypeAtcak = 0;
        }

        Player_Sheild plSh = Hand2.GetComponent<Player_Sheild>();
        if(plSh.Sheild != null && InventoryPanel.transform.GetChild(cell).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Text>().text == "E")
            if(id == plSh.Sheild.GetComponent<SheildIDInHand>().SheildID)
            {
                plSh.Sheild.SetActive(false);
                plSh.Sheild = null;
            }
        if(plSh.LastSheild != null && InventoryPanel.transform.GetChild(cell).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Text>().text == "E")
            if(id == plSh.LastSheild.GetComponent<SheildIDInHand>().SheildID)
            {
                plSh.LastSheild.SetActive(false);
                plSh.LastSheild = null;
            }
        Player_Defence pd = Player.GetComponent<Player_Defence>();
        if((id == pd.IDHelment || id == pd.IDBody || id == pd.IDGlow || id == pd.IDHand) && InventoryPanel.transform.GetChild(cell).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Text>().text == "E")
        {
            pd.ArmorUnEquip(id);
        }
        
        if(inventory.equip[cell].TypeEquip != 0)
        {
            InventoryPanel.transform.GetChild(cell).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
        }
    }

    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(inventory.equip[cell].id != 0)
        {
            InfoPanel.SetActive(true);
        
            Transform iconInfo = InfoPanel.transform.GetChild(0);
            Image icon_Info = iconInfo.GetComponent<Image>();
            icon_Info.sprite = inventory.equip[cell].icon;

            Transform nameInfo = InfoPanel.transform.GetChild(1);
            Text name_Info = nameInfo.GetComponent<Text>();
            if(inventory.equip[cell].countItem>=2)
                name_Info.text = inventory.equip[cell].Name+"("+ inventory.equip[cell].countItem+")";
            else
                name_Info.text = inventory.equip[cell].Name;


            Transform discInfo = InfoPanel.transform.GetChild(2);
            Text disc_Info = discInfo.GetComponent<Text>();
            disc_Info.text = inventory.equip[cell].discription;

            Transform costInfo = InfoPanel.transform.GetChild(3);
            Text cost_Info = costInfo.GetChild(0).GetComponent<Text>();
            cost_Info.text = ((int)(inventory.equip[cell].Cost * (SkillIndicator.CostFactor/100))).ToString();
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        InfoPanel.SetActive(false);
    }

}
