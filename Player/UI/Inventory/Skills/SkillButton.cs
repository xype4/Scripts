using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButton : MonoBehaviour
{
    public GameObject WarriorPanel;
    public GameObject MagicPanel;
    public GameObject OtherPanel;
    public GameObject CraftPanel;
    void Start()
    {
        
    }

    public void Warrior()
    {
        WarriorPanel.SetActive(true);
        MagicPanel.SetActive(false);
        OtherPanel.SetActive(false);
        CraftPanel.SetActive(false);
    }
    public void Magic()
    {
        WarriorPanel.SetActive(false);
        MagicPanel.SetActive(true);
        OtherPanel.SetActive(false);
        CraftPanel.SetActive(false);
    }
    public void Other()
    {
        WarriorPanel.SetActive(false);
        MagicPanel.SetActive(false);
        OtherPanel.SetActive(true);
        CraftPanel.SetActive(false);
    }
    public void Craft()
    {
        WarriorPanel.SetActive(false);
        MagicPanel.SetActive(false);
        OtherPanel.SetActive(false);
        CraftPanel.SetActive(true);
    }
}
