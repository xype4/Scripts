using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellInventory : MonoBehaviour
{
    public List<Spell> spellList;
    public List<Spell> activeSpell;
    public int accessCell;
    public Text Access_cell;
    public GameObject SkillsMenu;
    private GameObject SpellListCell;
    private GameObject ActiveSpellCell;
    private GameObject SpellInfo;
    private GameObject SpellDataBase;
    public GameObject Camera;
    public Skill_Indicator Skills;
    byte FreeCell = 255;

    void Start()
    {
        SpellListCell = SkillsMenu.transform.GetChild(0).gameObject;
        ActiveSpellCell = SkillsMenu.transform.GetChild(1).gameObject;
        SpellInfo = SkillsMenu.transform.GetChild(2).gameObject;
        SpellDataBase = SkillsMenu.transform.GetChild(3).gameObject;

        SpellInfo.SetActive(false);

        for(byte i = 0; i < SpellListCell.transform.childCount; i++)                                           
        {
            spellList.Add(new Spell());
            SpellListCell.transform.GetChild(i).GetComponent<CurrintSpell>().cell = i;      
        }

        for(byte i = 0; i < ActiveSpellCell.transform.childCount; i++)                                           
        {
            activeSpell.Add(new Spell());
            ActiveSpellCell.transform.GetChild(i).GetComponent<CurrentQuickSpells>().cell = i;      
        }

        SpellListCell.GetComponent<SpellChange>().spellChange = new Spell();

        AddSpell(1); AddSpell(2); AddSpell(3); DisplaySpell();
    }

    void Update()
    {
        accessCell = Skills.AccessSkill;
        Access_cell.text = accessCell.ToString();

        byte button = 255;
        if(Input.GetKeyDown("1"))
            button = 0;
        if(Input.GetKeyDown("2"))
            button = 1;
        if(Input.GetKeyDown("3"))
            button = 2;
        if(Input.GetKeyDown("4"))
            button = 3;
        if(Input.GetKeyDown("5"))
            button = 4;
        if(Input.GetKeyDown("6"))
            button = 5;
        if(Input.GetKeyDown("7"))
            button = 6;
        if(Input.GetKeyDown("8"))
            button = 7;
        if(Input.GetKeyDown("9"))
            button = 8;

        if(button !=255 && button <  accessCell)
        {
            if(activeSpell[button].id!= 0)
                activeSpell[button].customEvent.Invoke(); 
            button = 255;
        }
    }

    public void DisplaySpell()
    {
        for(int i = 0; i < SpellListCell.transform.childCount; i++)              
        {
            Transform cell = SpellListCell.transform.GetChild(i);       
            Transform icon = cell.GetChild(0);
            
            Image img = icon.GetComponent<Image>();
            if(spellList[i].id != 0)                            
            {
                img.enabled = true;                             
                img.sprite = spellList[i].icon; 

            }
            else                                    
            {
                img.enabled = false;
                img.sprite = null;
            }
        }

        for(int i = 0; i < ActiveSpellCell.transform.childCount; i++)          
        {
            Transform cell = ActiveSpellCell.transform.GetChild(i);       
            Transform icon = cell.GetChild(0);
            Transform number = icon.GetChild(0);
            
            Text txt = number.GetComponent<Text>();
            Image img = icon.GetComponent<Image>();
            if(activeSpell[i].id != 0)                          
            {
                img.enabled = true;                              
                img.sprite = activeSpell[i].icon; 
            }
            else                                        
            {
                img.enabled = false;
                img.sprite = null;
            }
        }
    }
    public void AddSpell(int SpellID)
    {
        for(byte i = 0; i < SpellListCell.transform.childCount; i++)                                           
        {
            if(spellList[i].id == 0 && FreeCell == 255)
            {
                FreeCell = i;
            }
            if(spellList[i].id == SpellID)
            {
                return;
            }
        }

        for(byte i = 0; i < ActiveSpellCell.transform.childCount; i++)                                           
        {
            if(activeSpell[i].id == SpellID)
            {
                return;
            }
        }

        if(FreeCell == 255)
            return;

        for(byte i = 0; i < SpellDataBase.transform.childCount; i++)
        {
            if(SpellID == SpellDataBase.transform.GetChild(i).gameObject.GetComponent<Spell>().id)
            {
                Spell AddedSpell = SpellDataBase.transform.GetChild(i).gameObject.GetComponent<Spell>();
                spellList[FreeCell] = AddedSpell;
                break;
            }
        }
        FreeCell = 255;
        DisplaySpell();
    }
}
