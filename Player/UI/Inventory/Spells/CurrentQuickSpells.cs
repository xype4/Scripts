using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CurrentQuickSpells : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
{
    public byte cell;
    public SpellInventory SpellInvenv;
    public GameObject InfoPanel;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            int id_ = SpellInvenv.activeSpell[cell].id;
            SpellInvenv.activeSpell[cell] = new Spell();
            SpellInvenv.AddSpell(id_);
        }
        SpellInvenv.DisplaySpell();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(SpellInvenv.activeSpell[cell].id != 0)
        {
            InfoPanel.SetActive(true);
        
            Transform iconInfo = InfoPanel.transform.GetChild(0);
            Image icon_Info = iconInfo.GetComponent<Image>();
            icon_Info.sprite = SpellInvenv.activeSpell[cell].icon;

            Transform nameInfo = InfoPanel.transform.GetChild(1);
            Text name_Info = nameInfo.GetComponent<Text>();
            name_Info.text = SpellInvenv.activeSpell[cell].Name;

            Transform discInfo = InfoPanel.transform.GetChild(2);
            Text disc_Info = discInfo.GetComponent<Text>();
            disc_Info.text = SpellInvenv.activeSpell[cell].discription;

            Transform costInfo = InfoPanel.transform.GetChild(3);
            Text cost_Info = costInfo.GetChild(0).GetComponent<Text>();
            cost_Info.text = SpellInvenv.activeSpell[cell].Cost.ToString();
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        InfoPanel.SetActive(false);
    }
}
