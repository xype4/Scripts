using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AlchCurrent : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
{
    public int cell;
    public Inventory inventory;
    public AlchVisible Alch;
    public GameObject InfoPanel;
    public Skill_Indicator SkillIndicator;
    
    void Start()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)       //нажатие на ПКМ(удаление)
        {
            if(Alch.alch[cell].id != 0)
            {
                inventory.AddItem(Alch.alch[cell].id, Alch.alch[cell].countItem, Alch.alch[cell].inventoryList);
                Alch.alch[cell] = new Item();
                Alch.DisplayAll();
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(Alch.alch[cell].id != 0)
        {
            InfoPanel.SetActive(true);
        
            Transform iconInfo = InfoPanel.transform.GetChild(0);
            Image icon_Info = iconInfo.GetComponent<Image>();
            icon_Info.sprite = Alch.alch[cell].icon;

            Transform nameInfo = InfoPanel.transform.GetChild(1);
            Text name_Info = nameInfo.GetComponent<Text>();
            name_Info.text = Alch.alch[cell].Name;

            Transform discInfo = InfoPanel.transform.GetChild(2);
            Text disc_Info = discInfo.GetComponent<Text>();
            disc_Info.text = Alch.alch[cell].discription;

            Transform costInfo = InfoPanel.transform.GetChild(3);
            Text cost_Info = costInfo.GetChild(0).GetComponent<Text>();
            cost_Info.text = ((int)(Alch.alch[cell].Cost * (SkillIndicator.CostFactor/100))).ToString();
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        InfoPanel.SetActive(false);
    }
}
