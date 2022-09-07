using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class Player_Attack : MonoBehaviour
{
    [Space] [Space]
    [Tooltip("Доп. Урон отравлением за атаку")]
    public float PoisonDamage;

    [Tooltip("количество ударов с отравлением")]
    public int PoisonCount;
    [Space]
    public Inventory inventory;
    public GameObject Sword;
    public ArrowStats Arrow;      //Стрела для лука
    public ArrowManager arrowManager;
    private Skill_Indicator Skills;

    [HideInInspector]
    public float StaminaHeavy;
    public Player_Sheild Hand2;
    [HideInInspector]
    public float StaminaCommon;
    public Player_Animation PlayerAnimation;
    //[HideInInspector]
    public int TypeAtcak = 0;
    private PlayerStats Player_Stats;
    public GameObject Body;
    public bool inAttack = false;

    public float damageFactor; // Множжитель типа атаки
    public float weaponDamageFactor;
    
    public GameObject PoisonCountDisplay;
    public Text PoisonCountText;

    void Start()
    {
        Skills = Body.GetComponent<Skill_Indicator>();
        Sword.GetComponent<BoxCollider>().enabled = false;
        Player_Stats = Body.GetComponent<PlayerStats>();
        Sword.SetActive (false);
        damageFactor = 1;
        weaponDamageFactor = 1;
        Arrow = new ArrowStats();
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && Player_Stats.Stamina_Count > Skills.StaminaHeavyAttack && Body.GetComponent<ControllerBody>().timeFactor != 0 && inAttack == false)              //Атака мечом             //(Тип атаки, Тип оружия)
        {
            int TypeAttackDigit = TypeAtcak;
            if(Sword.activeSelf == false)
            {
                TypeAttackDigit = 0;
            }

            if (Input.GetKey("left shift"))
            {
                Player_Stats.Stamina_Count -= Skills.StaminaHeavyAttack;
                damageFactor = 1.3f * Skills.HeavyAttackDamage;
                switch (TypeAttackDigit)
                {
                    case 0:
                        PlayerAnimation.Attack_type(0,0);   //Код кулака в ибало
                        break;
                    case 11:
                        PlayerAnimation.Attack_type(2,1);   //Код тяжелой атаки мечом
                        weaponDamageFactor = Skills.SwordDamage;
                        break;
                    case 12:
                        PlayerAnimation.Attack_type(2,1);   //Код тяжелой атаки топорой
                        weaponDamageFactor = Skills.AxeDamage;
                        break;
                    case 13:
                        PlayerAnimation.Attack_type(2,1);   //Код тяжелой атаки булавой
                        weaponDamageFactor = Skills.MaceDamage;
                        break;
                    case 21:
                        PlayerAnimation.Attack_type(2,2);   //1.5 тяжелая
                        break;
                    case 22:
                        PlayerAnimation.Attack_type(2,2);   //тяжклая секира
                        break;
                    case 31:
                        PlayerAnimation.Attack_type(0,3);   //Лук
                        Player_Stats.Stamina_Count += Skills.StaminaHeavyAttack;
                        break;
                }
                Hand2.Attack();      // Запустить анимацию щита
                
                
            } 
            else
            {
                Player_Stats.Stamina_Count -= Skills.StaminaCommonAttack;
                damageFactor = Skills.CommonAttackDamage;
                switch (TypeAttackDigit)
                {
                    case 0:
                        PlayerAnimation.Attack_type(0,0);   //Код кулака в ибало
                        break;
                    case 11:
                        PlayerAnimation.Attack_type(1,1);    //Код лёгкой атаки мечом 
                        weaponDamageFactor = Skills.SwordDamage;
                        break;
                    case 12:
                        PlayerAnimation.Attack_type(1,1);    //Код лёгкой атаки топор
                        weaponDamageFactor = Skills.AxeDamage;

                        break;
                    case 13:
                        PlayerAnimation.Attack_type(1,1);    //Код лёгкой атаки булава
                        weaponDamageFactor = Skills.MaceDamage;
                        break;
                    case 21:
                        PlayerAnimation.Attack_type(1,2);       // лёгкой 1.5 меч
                        break;
                    case 22:
                        PlayerAnimation.Attack_type(1,2);       // лёгкой секира
                        break;
                    case 31:
                        PlayerAnimation.Attack_type(0,3);   //Лук
                        Player_Stats.Stamina_Count += Skills.StaminaCommonAttack;
                        break;
                }
                Hand2.Attack();        // Запустить анимацию щита
            } 
        }
        if(Input.GetKeyDown("g") && inAttack == false)             //Экип. меча
        {
            PlayerAnimation.Equip_();
        }
    }
    public void Damage(GameObject entity, float Damage = 0,float PoisonOilDamage= 0, float magicDamage = 0, int bleendingTime = 0, int bleendingChance = 0, int bleendingDamage = 0, int poisonTime = 0, int poisonChance = 0, int poisonDamage = 0)
    {
        if(Sword.GetComponent<WeaponAttack>().Long == 1)
            Damage*= Skills.BowDamage;
        else
        {
            Damage*=damageFactor * Skills.Damage * weaponDamageFactor*((Player_Stats.Adrenaline_Count*Skills.AdrenalineDamage/200)+1);// * Random.Range(0.8f,1.2f) ;
        }

        if(Random.Range(0f,1f)< Skills.x2Damage)
            Damage*=2f;

        magicDamage *= Skills.SpellDamage;// * Random.Range(0.8f, 1.2f);
        int finalBleendingDamage =0; //Суммарный урон
        int finalBleendingTime = 0;
        int finalPoisonDamage =0;
        int finalPoisonTime = 0;

        if((Skills.bleendingChance + bleendingChance) > Random.Range(0,100))
        {
            finalBleendingDamage = (int)((float)bleendingDamage*Skills.bleendingDamage*Skills.bleendingTime);
            finalBleendingTime = (int)((float)bleendingTime*Skills.bleendingTime);
        }
        if(poisonChance > Random.Range(0,100))
        {
            finalPoisonDamage = poisonDamage;
            finalPoisonTime = poisonTime;
        }
        if(PoisonCount>0)
        {
            entity.GetComponent<EntityStats>().InputDamage(finalBleendingDamage, finalBleendingTime, finalPoisonDamage, finalPoisonTime, Damage, PoisonDamage, magicDamage, true);
            PoisonCount--;
            PoisonCountText.text = PoisonCount.ToString();
            if(PoisonCount<=0)
            PoisonCountDisplay.SetActive(false);
        }
        else
            entity.GetComponent<EntityStats>().InputDamage(finalBleendingDamage, finalBleendingTime, finalPoisonDamage, finalPoisonTime, Damage, 0, magicDamage, true);

    }

    public void SwordEquip(int weaponNumber)           // Активирует оружее, с неомером в иерархии Hand (c 0)         из CurrentItem(cобытие меча)
    {
        Sword.SetActive(false);
        Sword = gameObject.transform.GetChild(0).gameObject.transform.GetChild(weaponNumber).gameObject;

    }
    public void WeaponType(int Type)                     // 11- обычный меч. 12 - топор. 13- булава    21- 1.5 меч 22- двуручный топор 23- двуручная дубина     31-лук                из CurrentItem(cобытие меча)
    {
        TypeAtcak = Type;
        if(Type/20 >= 1)
        {
            Hand2.SheildUnEquip();
        }
    }

    public void ArrowEquip(int id_) // id стрелы
    {
        int count_ = 0;
        int Cell = 0;

        for(int i = 0; i < inventory.equip.Count; i++)                                                   //Находим такой же ид в инвентаре
        {
            if (inventory.equip[i].id == id_)
            {
                count_ = inventory.equip[i].countItem;                                         //берём количество
                Cell = i;
                break;
            }
        }

        for(int i = 0; i < arrowManager.arrows.Count; i++)
        {
            if (arrowManager.arrows[i].id == id_)
            {
                arrowManager.arrows[i].count = count_;
                arrowManager.arrows[i].InvCell = Cell;
                Arrow = arrowManager.arrows[i];
                break;
            }
        }
    }
    public void ArrowEquipUpdate(int id_)
    {
        for(int i = 0; i< arrowManager.arrows.Count; i++)
        {
            if (arrowManager.arrows[i].id == id_)
            {
                arrowManager.arrows[i].count = inventory.equip[arrowManager.arrows[i].InvCell].countItem;
                break;
            }
        }  
    }

    public void LoadData(Save.PlayerSaveData save)
    {
        Sword.SetActive(false);
        Sword = gameObject.transform.GetChild(save.Weapon).gameObject;

        for(int i = 0; i< arrowManager.arrows.Count; i++)
        {
            if (arrowManager.arrows[i].id == save.Arrow_.Id)
            {
                Arrow = arrowManager.arrows[i];
                Arrow.count = save.Arrow_.Count;
                Arrow.InvCell = save.Arrow_.InvCell;
                Debug.Log(Arrow.count);
                break;
            }
        }
        TypeAtcak = save.TypeAttack;
    }
}
