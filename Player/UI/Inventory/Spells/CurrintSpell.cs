using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class CurrintSpell : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
{
    
    public byte cell;
    public SpellInventory SpellInvenv;
    public GameObject InfoPanel;
    public SpellChange change;
    bool Added = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            for(byte i = 0; i<SpellInvenv.accessCell; i++)
            {
                if(SpellInvenv.activeSpell[i].id ==0)
                {
                    SpellInvenv.activeSpell[i] = SpellInvenv.spellList[cell];
                    Added = true;
                    break;
                }
            } 
            if(Added)
                SpellInvenv.spellList[cell] = new Spell();
            Added = false;
        }
    
        if(eventData.button == PointerEventData.InputButton.Middle)
        {
            if(change.spellChange.id == 0)
            {
                change.spellChange = SpellInvenv.spellList[cell];
                change.CellNumber = cell;
            }
            else
            {
                SpellInvenv.spellList[change.CellNumber] = SpellInvenv.spellList[cell];
                SpellInvenv.spellList[cell] = change.spellChange;
                StartCoroutine(q1123());
            }
        }
        SpellInvenv.DisplaySpell();
    }
    IEnumerator q1123()
    {
        yield return new WaitForSeconds(0.1f);
        change.spellChange = new Spell();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(SpellInvenv.spellList[cell].id != 0)
        {
            InfoPanel.SetActive(true);
        
            Transform iconInfo = InfoPanel.transform.GetChild(0);
            Image icon_Info = iconInfo.GetComponent<Image>();
            icon_Info.sprite = SpellInvenv.spellList[cell].icon;

            Transform nameInfo = InfoPanel.transform.GetChild(1);
            Text name_Info = nameInfo.GetComponent<Text>();
            name_Info.text = SpellInvenv.spellList[cell].Name;

            Transform discInfo = InfoPanel.transform.GetChild(2);
            Text disc_Info = discInfo.GetComponent<Text>();
            disc_Info.text = SpellInvenv.spellList[cell].discription;

            Transform costInfo = InfoPanel.transform.GetChild(3);
            Text cost_Info = costInfo.GetChild(0).GetComponent<Text>();
            cost_Info.text = SpellInvenv.spellList[cell].Cost.ToString();
        }
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        InfoPanel.SetActive(false);
    }
}
