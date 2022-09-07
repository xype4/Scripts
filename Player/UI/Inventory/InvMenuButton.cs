using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvMenuButton : MonoBehaviour
{
    public GameObject InInventory;
    public GameObject InSkills;
    public GameObject InSpells;

    public GameObject InInventoryButton;
    public GameObject InSkillsButton;
    public GameObject InSpellsButton;

    public Color Active;
    public Color Deactive;


    public void In_Inventory()
    {
        InInventory.SetActive(true);
        InInventoryButton.GetComponent<Image>().color = Active;
        InSkills.SetActive(false);
        InSkillsButton.GetComponent<Image>().color = Deactive;
        InSpells.SetActive(false);
        InSpellsButton.GetComponent<Image>().color = Deactive;
    }

    public void In_Skills()
    {
        InInventory.SetActive(false);
        InInventoryButton.GetComponent<Image>().color = Deactive;
        InSkills.SetActive(true);
        InSkillsButton.GetComponent<Image>().color = Active;
        InSpells.SetActive(false);
        InSpellsButton.GetComponent<Image>().color = Deactive;
    }

    public void In_Spells()
    {
        InInventory.SetActive(false);
        InInventoryButton.GetComponent<Image>().color = Deactive;
        InSkills.SetActive(false);
        InSkillsButton.GetComponent<Image>().color = Deactive;
        InSpells.SetActive(true);
        InSpellsButton.GetComponent<Image>().color = Active;
    }
}
