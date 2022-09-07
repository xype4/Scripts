using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Defence : MonoBehaviour
{
    public Player_Attack playerAttack;
    public Player_Animation PlayerAnimation;
    private GameObject ArmorContainer;
    private Skill_Indicator Skills;
    private ControllerBody Controller;
    public PlayerStats Player_Stats;
    [HideInInspector]
    public int Inform = 0;

    public GameObject DefaultArmorBody;
    public GameObject DefaultArmorHand;

    public GameObject ActiveArmorBody;
    public GameObject ActiveArmorHand;

    [HideInInspector]
    public float ResistanceArmor = 1;
    public float FinishResistanceArmor = 1;
    [HideInInspector]
    public int ArmID = 0;

    [HideInInspector]
    public float ResistanceSheild = 1;
    public float ResistanceSheildItem = 1;
    public float finishResistance;
    private GameObject PlayerBody;
    public Player_Sheild PlayerSheild;

    public float HPBonus = 0f;
    public float StaminaBonus = 0f;

    public int TypeHelment = 1;
    public int TypeBody = 1;
    public int TypeHand = 1;
    public int TypeGlow = 1;

    public int IDHelment = 0;
    public int IDBody = 0;
    public int IDHand = 0;
    public int IDGlow = 0;

    public float DefenceHelment = 1;
    public float DefenceBody = 1;
    public float DefenceHand = 1;
    public float DefenceGlow = 1;

    void Start()
    {
        ArmorContainer = PlayerAnimation.gameObject;
        Controller = gameObject.GetComponent<ControllerBody>();
        Skills = Player_Stats.gameObject.GetComponent<Skill_Indicator>();
        ResistanceSheild = 1;
        PlayerBody = gameObject;
        ActiveArmorHand = DefaultArmorHand;
        ActiveArmorBody = DefaultArmorBody;
    }

    void FixedUpdate()
    {
        if(PlayerSheild.Sheild!=null)
            ResistanceSheild = Skills.SheildDefence;
        else
            ResistanceSheild = 1;
        
        finishResistance = FinishResistanceArmor * ResistanceSheild * ResistanceSheildItem;
    }

    public void ArmorUnEquip(int id)
    {
        for(int i =1;i < ArmorContainer.transform.childCount;i++)
        {
            if(ArmorContainer.transform.GetChild(i).GetComponent<Player_Armor>().ID_Item == id)
            {
                Player_Armor armor = ArmorContainer.transform.GetChild(i).GetComponent<Player_Armor>();
                switch(armor.TypeArmor)
                {
                    case 1:
                        TypeHelment = 1;
                        DefenceHelment = 1f;
                        IDHelment = 0;
                        break;
                    case 2:
                        TypeBody= 1;
                        DefenceBody = 1f;
                        ActiveArmorBody.SetActive(false);
                        ActiveArmorBody = DefaultArmorBody;
                        ActiveArmorBody.SetActive(true);
                        IDBody = 0;
                        break;
                    case 3:
                        TypeHand= 1;
                        DefenceHand = 1f;
                        ActiveArmorHand.SetActive(false);
                        ActiveArmorHand = DefaultArmorHand;
                        ActiveArmorHand.SetActive(true);
                        IDHand = 0;
                        break;
                    case 4:
                        TypeGlow= 1;
                        DefenceGlow = 1f;
                        IDGlow = 0;
                        break;
                }
                
                FinishResistanceArmor = DefenceHelment* DefenceBody* DefenceHand*DefenceGlow;

                if(TypeHelment == TypeBody && TypeHand==TypeGlow &&TypeHand==TypeBody)
                {
                    switch(TypeBody)
                    {
                        case 1:
                            break;
                        case 2:
                            StaminaBonus = 1.3f;
                            break;
                        case 3:
                            StaminaBonus = 1.15f;
                            HPBonus = 1.15f;
                            break;
                        case 4:
                            HPBonus = 1.3f;
                            break;
                    }
                }
                else
                {
                    StaminaBonus = 1f;
                    HPBonus = 1f;
                }
                Player_Stats.ArmorHPBonus=HPBonus;
                Player_Stats.ArmorStaminaBonus=StaminaBonus;

                break;
            }
        }
        int countHeavy = 0;
        if(TypeHelment == 4) countHeavy++;
        if(TypeBody == 4) countHeavy++;
        if(TypeHand == 4) countHeavy++;
        if(TypeGlow == 4) countHeavy++;

        int countLight = 0;
        if(TypeHelment == 2) countLight++;
        if(TypeBody == 2) countLight++;
        if(TypeHand == 2) countLight++;
        if(TypeGlow == 2) countLight++;

        Controller.SpeedRunHeavyFactor = countHeavy*Skills.HeavyArmorSpeedMINUS;
        Player_Stats.HPLightRegenFactor = 1-(countLight*Skills.LightArmorHpMINUS);
    }
    public void ArmorEquipID (int id)  //ид надетой брони
    {
        for(int i =1;i < ArmorContainer.transform.childCount;i++)
        {
            if(ArmorContainer.transform.GetChild(i).GetComponent<Player_Armor>().ID_Item == id)
            {
                Player_Armor armor = ArmorContainer.transform.GetChild(i).GetComponent<Player_Armor>();
                switch(armor.TypeArmor)
                {
                    case 1:
                        TypeHelment = armor.TypeDefence;
                        DefenceHelment = armor.Defence;
                        IDHelment = id;
                        break;
                    case 2:
                        TypeBody= armor.TypeDefence;
                        DefenceBody = armor.Defence;
                        ActiveArmorBody.SetActive(false);
                        ActiveArmorBody = armor.gameObject;
                        ActiveArmorBody.SetActive(true);
                        IDBody = id;
                        break;
                    case 3:
                        TypeHand= armor.TypeDefence;
                        DefenceHand = armor.Defence;
                        ActiveArmorHand.SetActive(false);
                        ActiveArmorHand = armor.gameObject;
                        ActiveArmorHand.SetActive(true);
                        IDHand = id;
                        break;
                    case 4:
                        TypeGlow= armor.TypeDefence;
                        DefenceGlow = armor.Defence;
                        IDGlow = id;
                        break;
                }

                switch(armor.TypeDefence)
                {
                    case 1:
                        break;
                    case 2:
                        switch(armor.TypeArmor)
                        {
                            case 1:
                                DefenceHelment/=Skills.LArmFactor;
                                break;
                            case 2:
                                DefenceBody/=Skills.LArmFactor;
                                break;
                            case 3:
                                DefenceHand/=Skills.LArmFactor;
                                break;
                            case 4:
                                DefenceGlow/=Skills.LArmFactor;
                                break;
                        }
                        break;
                    case 3:
                        switch(armor.TypeArmor)
                        {
                            case 1:
                                DefenceHelment/=Skills.MArmFactor;
                                break;
                            case 2:
                                DefenceBody/=Skills.MArmFactor;
                                break;
                            case 3:
                                DefenceHand/=Skills.MArmFactor;
                                break;
                            case 4:
                                DefenceGlow/=Skills.MArmFactor;
                                break;
                        }
                        break;
                    case 4:
                        switch(armor.TypeArmor)
                        {
                            case 1:
                                DefenceHelment/=Skills.HArmFactor;
                                break;
                            case 2:
                                DefenceBody/=Skills.HArmFactor;
                                break;
                            case 3:
                                DefenceHand/=Skills.HArmFactor;
                                break;
                            case 4:
                                DefenceGlow/=Skills.HArmFactor;
                                break;
                        }
                        break;
                }

                FinishResistanceArmor = DefenceHelment * DefenceBody* DefenceHand * DefenceGlow / Skills.ArmFactor;


                int countHeavy = 0;
                if(TypeHelment == 4) countHeavy++;
                if(TypeBody == 4) countHeavy++;
                if(TypeHand == 4) countHeavy++;
                if(TypeGlow == 4) countHeavy++;

                int countLight = 0;
                if(TypeHelment == 2) countLight++;
                if(TypeBody == 2) countLight++;
                if(TypeHand == 2) countLight++;
                if(TypeGlow == 2) countLight++;

                Controller.SpeedRunHeavyFactor = countHeavy*Skills.HeavyArmorSpeedMINUS;
                Player_Stats.HPLightRegenFactor = 1-(countLight*Skills.LightArmorHpMINUS);

                if(TypeHelment == TypeBody && TypeHand==TypeGlow &&TypeHand==TypeBody)
                {
                    switch(TypeBody)
                    {
                        case 1:
                            break;
                        case 2:
                            StaminaBonus = 1.3f * Skills.LStaminaFactor;
                            break;
                        case 3:
                            StaminaBonus = 1.15f * Skills.MStaminaFactor;
                            HPBonus = 1.15f * Skills.MStaminaFactor;
                            break;
                        case 4:
                            HPBonus = 1.3f* Skills.HStaminaFactor;
                            break;
                    }
                }
                else
                {
                    StaminaBonus = 1f;
                    HPBonus = 1f;
                }
                Player_Stats.ArmorHPBonus=HPBonus;
                Player_Stats.ArmorStaminaBonus=StaminaBonus;

                break;
            }
        }
    }

    public void LoadData(Save.PlayerSaveData save)
    {
        ArmorEquipID(save.ArmorID);
    }
}

