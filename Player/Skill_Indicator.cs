using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Indicator : MonoBehaviour
{
    public GameObject LeftHand;
    public Player_Attack RightHand;
    public GameObject HandRoot;
    public GameObject Player;
    public GameObject Camera;
    public GameObject PlayerHandCol;
    public GameObject PoisonCount;
    public Text PoisonCountText;
    [Space] [Space]
    public GameObject HealIcon; 
    public GameObject StaminaHealIcon; 
    [Space]
        
    [Tooltip("Множитель базовой защиты щитом при простом наличии (до 1)")]
    public float SheildDefence;     
    [Tooltip("Затраты на поддержания блока в щите (при 0.1 не тратится)")]
    public float SheildBlockStamina;            

    [Tooltip("Множитель задержки атаки (0-без перерыва)")] 
    public float StayTimeAttack;     
    [Tooltip("Затраты на тяжёлую атаку")] 
    public float StaminaHeavyAttack;           
    [Tooltip("Затраты на обычную атаку")] 
    public float StaminaCommonAttack;          
    [Tooltip("Множитель к любой атаке оружием (от 1)")]
    public float Damage;                    
    [Tooltip("Множитель к атаке мечом (от 1)")] 
    public float SwordDamage;              
    [Tooltip("Множитель к атаке топором (от 1)")] 
    public float AxeDamage;                
    [Tooltip("Множитель к атаке буловой (от 1)")] 
    public float MaceDamage;            

    [Tooltip("Множитель к атаке луком (от 1)")]
    public float BowDamage;    
    [Tooltip("Множитель к скорости полёта стрелы (от 1)")] 
    public float ArrowFly;
    [Tooltip("Разброс лука 20- обычное значение")]
    public int Dispersion; 

    [Tooltip("Множитель к легкой атаке (от 1)")]
    public float CommonAttackDamage;        
    [Tooltip("Множитель к тяжёлой атаке (от 1)")]
    public float HeavyAttackDamage;         
    [Tooltip("Множитель к опыту (до 1)")] 
    public float ExpFactor;    
    [Tooltip("Вкл/выкл нанесение ядов на оружее( 0- выкл, 1-вкл )")]
    public byte poison;                                    
    [Tooltip("Дополнительные атаки отравлением")]
    public byte poisonAttack;
    [Tooltip("Удвоение урона противнику шанс (до 1)")]
    public float x2Damage;

    [Tooltip("Адреналин(1 - вкл 0- выкл)")]//
    public int AdrenalineOff;

    [Tooltip("Множитель уменьшения адреналина")]//
    public float AdrenalineUnregen;

    [Tooltip("Множитель уронв адреналина")]//
    public float AdrenalineDamage;

    [Space] [Space]


    [Tooltip("Время кровотечения множитель (от 1)")] 
    public float bleendingTime;
    [Tooltip("Сумма шанс вызвать кровотечения (1-100)")] 
    public byte bleendingChance;
    [Tooltip("Множитель урон в секунду от кровотечения(от 1)")] 
    public float bleendingDamage;
    [Space] [Space]

    [Tooltip("Доступоно ячеек спеллов(2 поумолчанию)")]    //(до 5 от скиллов)
    public int AccessSkill; 
    [Tooltip("Уровень алхимии")] //(до 3 от скиллов)
    public int AlchLvl;   
    [Tooltip("Уровень кузнечного дела")] //(до 3 от скиллов)
    public int MetalLvl;  
    [Tooltip("Множитель пассивной регенерации стамины")]
    public float RegenStamina;          
    [Tooltip("Множитель пассивной регенерации здоровья")]
    public float RegenHeal;    
    [Tooltip("Множитель концентрации (реген стамины в ману), сколько стамины переходит в ману")]
    public float ConcetrationFactor;
    [Tooltip("Множетель регенерации маны")]
    public float FactorManaRegen; 
    [Tooltip("Затраты стамины на отпрыжку")] 
    public float StaminaRebound;         
    [Tooltip("Плюс Скорость бега")] //
    public float RunSpeed;           
    [Tooltip("Еда действует дольше в раз")]
    public float EatTime;                        
    [Tooltip("Зелье действует дольше в раз")]
    public float PotionTime;                     
    [Tooltip("Красноречие, процент скидки (0 - обычное значение)")]
    public float Oratory;             
    [Tooltip("Красноречие, процент обычной цены продаваемого предмета(100- обычное значение)")]
    public float CostFactor;        
    [Space] [Space]

    [Tooltip("Множитель брони (во сколько раз меньше урона входит)")] 
    public float ArmFactor;
    [Tooltip("Множитель лёгкой брони (во сколько раз меньше урона входит)")]   
    public float LArmFactor;
    [Tooltip("Множитель тяжёлой брони (во сколько раз меньше урона входит)")]  
    public float HArmFactor;
    [Tooltip("Множитель средней брони (во сколько раз меньше урона входит)")]  
    public float MArmFactor;

    [Tooltip("Бонус бонуса множитель лёгкой броне")]
    public float LStaminaFactor;
    [Tooltip("Бонус бонуса множитель в тяжёлой броне")]
    public float HStaminaFactor;
    [Tooltip("Бонус бонуса множитель в средней броне")]
    public float MStaminaFactor;

    [Tooltip("Умееньшение скорости бега в тяжёлой броне")]  //
    public float HeavyArmorSpeedMINUS;
    [Tooltip("Множитель регена HP в лёгкой броне (0.1 -уменьшение на 10% за одну шмотку)")]  //
    public float LightArmorHpMINUS;

    [Tooltip("Количество характеристик за уровень +5 за и тд")]
    public float LVL_UP; 
    [Tooltip("Проценет получить 2 травки из 1(10)")]
    public byte Herbalism; 

    [Space] [Space]

    
    [Tooltip("Множетель затраты маны (до 1)")] 
    public float ManaFactor; 
    [Tooltip("Множетель урона от отравления по игроку (до 1)")]
    public float PoisonFactor; 
    [Tooltip("Множетель кровотечения от отравления по игроку (до 1)")] 
    public float BleedingFactor; 

    [Tooltip("Множетель затраты маны на лечение (до 1)")]
    public float ManaHeal; 
    [Tooltip("Множетель затраты маны на атаку (до 1)")]
    public float ManaAttack; 

    [Tooltip("Множетель силы атакующих заклиний")]
    public float SpellDamage; 
    [Tooltip("Множетель силы защитных заклиний")]
    public float SpellHeal; 

    [Tooltip("Множетель затраты энергии здоровья")]
    public float FactorEnergyHeal; 


/*---------------------------------------------------------------------------------------------------*/

private float EatForceStamina, PotionForceStamina, EatForceHeal, PotionForceHeal, time5, time6, time7, time8;

/*---------------------------------------------------------------------------------------------------*/
/*                                      Стамина: Еда/Зелья                                           */ 
/*---------------------------------------------------------------------------------------------------*/
    public void EatStaminaEffectTime(float time)
    {
        StartCoroutine(EatStamina(time));
    }
    public void EatStaminaEffectForce(float force)
    {
        EatForceStamina = force;
    }
    IEnumerator EatStamina(float time)
    {
        yield return new WaitForSeconds(0.1f);
        Player.GetComponent<PlayerStats>().StEat = EatForceStamina;
        StaminaHealIcon.SetActive(true);
        yield return new WaitForSeconds(time*EatTime);
        StaminaHealIcon.SetActive(false);
        Player.GetComponent<PlayerStats>().StEat = 1;
    }


    public void PotionStaminaEffectTime(float time)
    {
        StartCoroutine(PotionStamina(time));
    }
    public void PotionStaminaEffectForce(float force)
    {
        PotionForceStamina = force;
    }
    IEnumerator PotionStamina(float time)
    {
        yield return new WaitForSeconds(0.1f);
        Player.GetComponent<PlayerStats>().StRegenPotion = PotionForceStamina;
        StaminaHealIcon.SetActive(true);
        yield return new WaitForSeconds(time*PotionTime);
        StaminaHealIcon.SetActive(false);
        Player.GetComponent<PlayerStats>().StRegenPotion = 1;
    }

/*---------------------------------------------------------------------------------------------------*/
/*                                      Здоровье: Еда/Зелья                                          */ 
/*---------------------------------------------------------------------------------------------------*/
    public void EatHealEffectTime(float time)
    {
        StartCoroutine(EatHeal(time));
    }
    public void EatHealEffectForce(int force)
    {
        EatForceHeal = force;
    }
    IEnumerator EatHeal(float time)
    {
        yield return new WaitForSeconds(0.1f);
        Player.GetComponent<PlayerStats>().regenEat = EatForceHeal;
        HealIcon.SetActive(true);
        yield return new WaitForSeconds(time*EatTime);
        HealIcon.SetActive(false);
        Player.GetComponent<PlayerStats>().regenEat = 1;
    }


    public void PotionHealEffectTime(float time)
    {
        StartCoroutine(PotionHeal(time));
    }
    public void PotionHealEffectForce(int force)
    {
        PotionForceHeal = force;
    }
    IEnumerator PotionHeal(float time)
    {
        yield return new WaitForSeconds(0.1f);
        Player.GetComponent<PlayerStats>().regenPotion = PotionForceHeal;
        HealIcon.SetActive(true);
        yield return new WaitForSeconds(time*PotionTime);
        HealIcon.SetActive(false);
        Player.GetComponent<PlayerStats>().regenPotion = 1;
    }

/*---------------------------------------------------------------------------------------------------*/
/*                                      Масло отравления на мечи                                     */ 
/*---------------------------------------------------------------------------------------------------*/

    public void CommonPoisonEffectCount(int time)
    {
        if(poison == 1)
        {
            RightHand.GetComponent<Player_Attack>().PoisonCount = time+poisonAttack;
            PoisonCount.SetActive(true);
            PoisonCountText.text = (time+poisonAttack).ToString();
        }
    }
    public void CommonPoisonEffectForce(float force)
    {
         if(poison == 1)
                RightHand.GetComponent<Player_Attack>().PoisonDamage = force;
    }


    // public void PotionHealEffectTime(float time)
    // {
    //     StartCoroutine(PotionHeal(time/10));
    // }
    // public void PotionHealEffectForce(int force)
    // {
    //     PotionForceHeal = force/10;
    // }
    // IEnumerator PotionHeal(float time)
    // {
    //     Hand.GetComponent<Player_HP>().regenPotion = PotionForceHeal;
    //     yield return new WaitForSeconds(time);
    //     Hand.GetComponent<Player_HP>().regenPotion = 1;
    // }
    public void LoadData(Save.PlayerSkillsData save)
    {

    }
}

