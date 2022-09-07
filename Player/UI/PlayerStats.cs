using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    private float TimeConts;

    public GameObject BloodIcon; 
    public GameObject PoisonIcon; 


    public Color PoisonColor; 
    public Color BleedingColor; 
    public Color NormalColor;

    [Header("Здоровье")]
    public GameObject HP_bar;
    public Image HP_Color;
    private Slider HP_barS;
    public float HP_Count = 100f; 
    public float MaxHPCount = 100f;
    public float HPLightRegenFactor = 1f;
    public float ArmorHPBonus = 1f;

    [Header("Стамина")]

    public GameObject Stamina_Bar;
    private Slider Stamina_BarS;

    public float Stamina_Count = 100f;
    public float StaminaMax_Count = 100f;
    public float ArmorStaminaBonus = 1f;
    
    [Header("Опыт")]

    public short EXP;
    public byte Lvl;
    public byte Score;
    public GameObject EXPMenu;
    public Skill_Indicator SkillIndicator;
    public Text LvlT;
    public Text ScoreT;

    [Header("Мана")]

    public GameObject Mana_Bar;
    private Slider Mana_BarS;
    
    public float ManaCount = 100f;
    public float ManaMaxCount = 100f;
    private float ManaRegenFactor;

    [Header("Воздух")]

    public GameObject Air_bar;
    private Slider Air_barS;
    public float Air_Count = 100f;
    public short MaxAirCount = 100;


    [Header("Адреналин")]
    public GameObject Adrenaline_bar;
    private Slider AdrenalineS;
    public float Adrenaline_Count = 0; 
    public float MaxAdrenaline_Count = 100f;

    [Header("SkillIndicator")]
    [Space] 
    public float regenPotion = 1;
    public float regenEat = 1; 
    public float HPRegenSkill = 1;
    public float PoisonFactor = 1;
    [Space]
    public float StRegenPotion = 1; 
    public float StEat = 1; 
    public float RegenSkill = 1; 
    public float ArmorFactor = 1; 
    [Space]
    public float ManaRegenPotion = 1;
    public float ManaEat = 1;
    public float ManaRegenSkill = 1;


    private int TimeHeal = 0;
    private int TimeMana= 0;
    private int TimeStamina= 0;
    private int TimeAer= 0;

    private float ConcentrationMana = 1;
    private float ConcentrationStamina = 1;


    void Start()
    {
        HP_barS = HP_bar.GetComponent<Slider>();
        Stamina_BarS = Stamina_Bar.GetComponent<Slider>();
        Mana_BarS = Mana_Bar.GetComponent<Slider>();
        Air_barS = Air_bar.GetComponent<Slider>();
        AdrenalineS = Adrenaline_bar.GetComponent<Slider>();
        TimeConts = Time.fixedDeltaTime;
        AddEXP(0);
    }

    void FixedUpdate()
    {   
        HPRegenSkill = SkillIndicator.RegenHeal;
        RegenSkill = SkillIndicator.RegenStamina;
        ManaRegenSkill = SkillIndicator.FactorManaRegen;

        if(Input.GetKey("o"))
        {
            ConcentrationMana = 1f + SkillIndicator.ConcetrationFactor;
            ConcentrationStamina = 1f - SkillIndicator.ConcetrationFactor;
        }
        else
        {
            ConcentrationMana = 1;
            ConcentrationStamina = 1;
        }
        if(SkillIndicator.AdrenalineOff == 1)
        {
            Adrenaline_Count -= 1f*TimeConts * SkillIndicator.AdrenalineUnregen;
            Adrenaline_bar.SetActive(true);
        }
        else
        {
            Adrenaline_bar.SetActive(false);
        }
        HP_Count += 0.1f * regenPotion * regenEat * HPRegenSkill * TimeConts * HPLightRegenFactor;
        Stamina_Count += 5.4f * StRegenPotion * StEat * RegenSkill* ArmorFactor * TimeConts * ConcentrationStamina;
        ManaCount += 2.2f * ManaRegenPotion * ManaEat * ManaRegenSkill * TimeConts * ConcentrationMana;
        Air_Count += 4f * TimeConts;

        if(Air_Count< 2f)
        {
            HP_Count-= 0.2f;
        }

        if (HP_Count >= MaxHPCount) 
            HP_Count = MaxHPCount;
        HP_barS.value = HP_Count;
        HP_barS.maxValue = MaxHPCount;
        
        if(Stamina_Count < 0)
             Stamina_Count = 0;
        if (Stamina_Count >= MaxAdrenaline_Count) 
            Stamina_Count = MaxAdrenaline_Count;
        Stamina_BarS.maxValue = StaminaMax_Count;
        Stamina_BarS.value = Stamina_Count;

        if(Adrenaline_Count < 0)
             Adrenaline_Count = 0;
        if (Adrenaline_Count >= MaxAdrenaline_Count) 
            Adrenaline_Count = MaxAdrenaline_Count;
        AdrenalineS.value = Adrenaline_Count;

        if (ManaCount >= ManaMaxCount) 
            ManaCount = ManaMaxCount;
        Mana_BarS.value = ManaCount;
        Mana_BarS.maxValue = ManaMaxCount;



        if (Air_Count >= MaxAirCount) 
            Air_Count = MaxAirCount;
        Air_barS.value = Air_Count;
        Air_barS.maxValue = MaxAirCount;

        

        if(Air_Count == MaxAirCount)    TimeAer++;
        else 
        {
            TimeAer = 0;
            Air_bar.SetActive(true);
        }
    
        if (TimeAer > 100)  Air_bar.SetActive(false);
    }

   public void AddEXPFactor(int exp)
    {
        EXP += (short)(exp*SkillIndicator.ExpFactor);
        EXPCalculate();
    }

    public void AddEXP(int exp)
    {
        EXP += (short)(exp);
        EXPCalculate();
    }

    private void EXPCalculate()
    {
        while (EXP > 1000)
        {
            EXP-= 1000;
            Lvl++;
            Score++;
            byte lvl = Lvl;

            if(lvl>20)
                lvl = 20;
            
            MaxHPCount += SkillIndicator.LVL_UP*(1-(lvl/20));
            StaminaMax_Count += SkillIndicator.LVL_UP*(1-(lvl/20));
            ManaMaxCount += SkillIndicator.LVL_UP*(1-(lvl/20));
        }

        EXPMenu.GetComponent<Slider>().value = EXP;
        LvlT.text = Lvl.ToString();
        ScoreT.text = Score.ToString();
    }
    public void Damage(float damage)
    {
        float DamagePercent = (damage * gameObject.GetComponent<Player_Defence>().finishResistance)/MaxHPCount;
        HP_Count -= damage * gameObject.GetComponent<Player_Defence>().finishResistance;
        Adrenaline_Count+=DamagePercent*150;
    }

    public void RemoveScore(byte score)
    {
        Score -= score;
        ScoreT.text = Score.ToString();
    }

    public void LoadData(Save.PlayerSaveData save)
    {
        HP_Count = save.HP;
        EXP = (short)save.EXP;
        Lvl = (byte)save.Lvl;
        Score = (byte)save.Score;
        Stamina_Count = save.Stamina;
        AddEXP(0);
    }

    public IEnumerator BleedingPlayer(float time, float ALLdamage)
    {
        BloodIcon.SetActive(true);
        ALLdamage*=SkillIndicator.BleedingFactor;
        HP_Color.color = BleedingColor;
        float damagePerTick = ALLdamage/time/10f;
        for(int i = 0; i < (int)(time*10);i++)
        {
            HP_Count-=damagePerTick;
            yield return new WaitForSeconds(0.1f);
        }
        HP_Color.color = NormalColor;
        BloodIcon.SetActive(false);
    }

    public IEnumerator PoisonPlayer(float time, float ALLdamage)
    {
        PoisonIcon.SetActive(true);
        ALLdamage*=SkillIndicator.PoisonFactor;
        HP_Color.color = PoisonColor;
        float damagePerTick = ALLdamage/time/10f*PoisonFactor;
        for(int i = 0; i < (int)(time*10);i++)
        {
            HP_Count-=damagePerTick;
            yield return new WaitForSeconds(0.1f);
        }
        HP_Color.color = NormalColor;
        PoisonIcon.SetActive(false);
    }
}
