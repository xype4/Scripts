using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cheats : MonoBehaviour
{
    public GameObject Body;
    public GameObject HandCol;
    public GameObject EnterText;

    public void Enter(string enter)
    {
        if(enter.StartsWith("AddEXP")) 
        {
            Body.GetComponent<PlayerStats>().AddEXP(short.Parse(enter.Substring(7)));
        }
        
        if(enter.StartsWith("AddScore")) 
        {
            Body.GetComponent<PlayerStats>().Score+= byte.Parse(enter.Substring(9));
            Body.GetComponent<PlayerStats>().AddEXP(0);
        }

        if(enter.StartsWith("AddMoney")) 
        {
            HandCol.GetComponent<ShopVisiable>().Money+= int.Parse(enter.Substring(9));
        }

        if(enter.StartsWith("AddItem")) //id count list
        {
            string[] ent = enter.Split(' ');
            HandCol.GetComponent<Inventory>().AddItem (int.Parse(ent[1]), byte.Parse(ent[2]), byte.Parse(ent[3]));
        }

        if(enter.StartsWith("AddSpell")) //id 
        {
            HandCol.GetComponent<SpellInventory>().AddSpell(int.Parse(enter.Substring(9)));
        }

        if(enter.StartsWith("Damage")) //id 
        {
            Debug.Log(Body.GetComponent<PlayerStats>().HP_Count);
            Body.GetComponent<PlayerStats>().Damage(int.Parse(enter.Substring(7)));
            Debug.Log(Body.GetComponent<PlayerStats>().HP_Count);
        }

        if(enter.StartsWith("Heal")) //id 
        {
            Body.GetComponent<PlayerStats>().HP_Count = Body.GetComponent<PlayerStats>().MaxHPCount;
            Debug.Log(Body.GetComponent<PlayerStats>().HP_Count);
        }

        if(enter.StartsWith("SetSpeedWalk")) //count 4-def
        {
            Body.GetComponent<ControllerBody>().Speed = int.Parse(enter.Substring(13));
        }
        
        if(enter.StartsWith("SetSpeedRun")) //count 9-def
        {
            Body.GetComponent<ControllerBody>().SpeedRunBase = int.Parse(enter.Substring(12));
        }

        EnterText.GetComponent<InputField>().text = "";
    }
    
}
