using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillActivate : MonoBehaviour
{
    public Skill_Indicator SkiillParametr;
    public void Activate(byte group, byte id, float parametr, float parametr2)
    {
        switch(group)
        {
            case 0:
                switch(id)
                {
                    case 0: 
                        AddDamage(parametr);
                        break;
                    case 1: 
                        MaceMaster(parametr);
                        break;
                    case 2: 
                        SwordMaster(parametr);
                        break;
                    case 3: 
                        AxeMaster(parametr);
                        break;
                    case 4: 
                        Poison(parametr);
                        break;
                    case 5: 
                        BloodStr(parametr);
                        break;
                    case 6: 
                        BloodTime(parametr);
                        break;
                    case 7: 
                        BloodChance(parametr);
                        break;
                    case 8: 
                        EXP(parametr);
                        break;
                    case 9: 
                        SwordSpeedMaster(parametr);
                        break;
                    case 10: 
                        SheildBlockkMaster(parametr, parametr2);
                        break;
                    case 11: 
                        LightAttackMaster(parametr, parametr2);
                        break;
                    case 12: 
                        HeavyAttackMaster(parametr, parametr2);
                        break;
                    case 13: 
                        BowMaster(parametr, parametr2);
                        break;
                    case 14: 
                        x2_Damage(parametr);
                        break;
                }
                break;
            case 1:
                switch(id)
                {
                    case 0:
                        AddRegenMana(parametr);
                        break;
                    case 1:
                        HealUP(parametr);
                        break;
                    case 2:
                        DamageUP(parametr);
                        break;
                    case 3:
                        Aura(parametr);
                        break;
                    case 4:
                        LowManaCost(parametr);
                        break;
                    case 5:
                        MagicUP(parametr);
                        break;
                    case 6:
                        LowManaCostAttack(parametr);
                        break;
                    case 7:
                        LowManaCostHeal(parametr);
                        break;
                    case 8:
                        MagicalZwlatin(parametr);
                        break;
                    case 9:
                        Range(parametr);
                        break;
                    case 10:
                        LowManaCostMagic(parametr);
                        break;
                    case 11:
                        HealStaminaLow(parametr);
                        break;
                    case 12:
                        Elixir(parametr);
                        break;
                }
                break;
            case 2:
                switch (id)
                {
                    case 0:
                        AddArmor(parametr);
                        break;
                    case 1:
                        LightAddArmor(parametr, parametr2);
                        break;
                    case 2:
                        MediumAddArmor(parametr, parametr2);
                        break;
                    case 3:
                        HeavyAddArmor(parametr, parametr2);
                        break;
                    case 4:
                        Red(parametr);
                        break;
                    case 5:
                        Famous(parametr);
                        break;
                    case 6:
                        StaminaReg(parametr);
                        break;
                    case 7:
                        LVLUP(parametr);
                        break;
                    case 8:
                        AlchemyLVL(parametr);
                        break;
                    case 9:
                        PotionTimeInAlch(parametr);
                        break;
                    case 10:
                        ReboundStamina(parametr);
                        break;
                    case 11:
                        SpellCount(parametr);
                        break;
                    case 12:
                        PoisonedBlood(parametr);
                        break;
                    case 13:
                        EatTimeInAlch(parametr);
                        break;
                    case 14:
                        Herb(parametr);
                        break;
                    case 15:
                        AdrenalineOn(parametr);
                        break;
                    case 16:
                        AdrenalineUp(parametr, parametr2);
                        break;
                }
                break;
        }
    }

    void AddDamage(float factor)                //Увеличение урона от всего на 10,10,15 и увелечение дальности стрельбы на столько же
    {
        SkiillParametr.Damage += factor;
        SkiillParametr.ArrowFly += factor;
    }

    void SwordMaster(float factor)                //Увеличение урона от меча
    {
        SkiillParametr.SwordDamage += factor;
    }

    void AxeMaster(float factor)                //Увеличение урона от топора
    {
        SkiillParametr.AxeDamage += factor;
    }

    void MaceMaster(float factor)                //Увеличение урона от булавы
    {
        SkiillParametr.MaceDamage += factor;
    }


    void BowMaster(float factor, float factor2)                //Уменьшение разброса лука
    {
        SkiillParametr.BowDamage += factor;
        SkiillParametr.Dispersion -= (int)factor2;
         
    }

    void HeavyAttackMaster(float factor, float factor2)                //Увелечение урона от тяжёлой атаки
    {
        SkiillParametr.HeavyAttackDamage += factor;
        SkiillParametr.StaminaHeavyAttack -= factor2;
         
    }

    void LightAttackMaster(float factor, float factor2)                //Увелечение урона от лёгкой атаки
    {
        SkiillParametr.CommonAttackDamage += factor;
        SkiillParametr.StaminaCommonAttack -= factor2;
         
    }

    void SheildBlockkMaster(float factor, float factor2)                //Увелечение блока и уменьшение затрат на блок щитом
    {
        SkiillParametr.SheildDefence -= factor;
        SkiillParametr.SheildBlockStamina -= factor2;
         
    }

    void Poison(float factor)                //яды оружее
    {
        SkiillParametr.poison = (byte)factor;
         
    }

    void BloodStr(float factor)                //Сила кровотеч
    {
        SkiillParametr.bleendingDamage += factor;
         
    }

    void BloodTime(float factor)                //Длит Кровотеч
    {
        SkiillParametr.bleendingTime += factor;
        SkiillParametr.bleendingDamage += factor;         
    }

    void BloodChance(float factor)                //Шанс кровотеч
    {
        SkiillParametr.bleendingChance += (byte)factor;
         
    }

    void EXP(float factor)                //Длит Кровотеч
    {
        SkiillParametr.ExpFactor += factor;
         
    }

    void x2_Damage(float factor)
    {
        SkiillParametr.x2Damage += factor;  
    }





    void AddRegenMana(float factor)
    {
        SkiillParametr.FactorManaRegen += factor;
         
    }

    void HealUP(float factor)
    {
        SkiillParametr.SpellHeal += factor;
         
    }

    void DamageUP(float factor)
    {
        SkiillParametr.SpellDamage += factor;
         
    }

    void MagicUP(float factor)
    {
        SkiillParametr.ConcetrationFactor += factor;
    }

    void Aura(float factor)
    {
        SkiillParametr.RegenStamina += factor;
        SkiillParametr.RegenHeal += factor;
         
    }

    void LowManaCost(float factor)
    {
        SkiillParametr.ManaFactor -= factor;
         
    }

    void LowManaCostAttack(float factor)
    {
        SkiillParametr.ManaAttack -= factor;
         
    }

    void LowManaCostHeal(float factor)
    {
        SkiillParametr.ManaHeal -= factor;
         
    }

    void LowManaCostMagic(float factor)
    {
        SkiillParametr.AccessSkill += (int)factor;
         
    }

    void MagicalZwlatin(float factor)
    {
        SkiillParametr.poisonAttack += (byte)factor;
         
    }

    void SwordSpeedMaster(float factor)
    {
        SkiillParametr.StayTimeAttack-=factor;
    }

    void Range(float factor)
    {
        SkiillParametr.ConcetrationFactor += factor;
        SkiillParametr.SpellDamage += factor;
    }

    void HealStaminaLow(float factor)
    {
        SkiillParametr.FactorEnergyHeal -= factor;
         
    }

    void Elixir(float factor)
    {
        SkiillParametr.PotionTime += factor;
         
    }




    void AddArmor(float factor)
    {
        SkiillParametr.ArmFactor += factor;
         
    }

    void LightAddArmor(float factor, float factor2)
    {
        SkiillParametr.LArmFactor += factor;
        SkiillParametr.LStaminaFactor += factor2;
         
    }

    void MediumAddArmor(float factor, float factor2)
    {
        SkiillParametr.MArmFactor += factor;
        SkiillParametr.MStaminaFactor += factor2;
    }

    void HeavyAddArmor(float factor, float factor2)
    {
        SkiillParametr.HArmFactor += factor;
        SkiillParametr.HStaminaFactor += factor2;
         
    }

    void Red(float factor)
    {
        SkiillParametr.CostFactor += factor;
         
    }

    void Famous(float factor)
    {
        SkiillParametr.Oratory += factor;
         
    }
    
    void StaminaReg(float factor)
    {
        SkiillParametr.RegenStamina += factor;
         
    }

    void LVLUP(float factor)
    {
        SkiillParametr.LVL_UP += factor;
         
    }
    
    void AlchemyLVL(float factor)
    {
        SkiillParametr.AlchLvl += (int)factor;
        SkiillParametr.MetalLvl += (int)factor;
    }

    void PoisonedBlood(float factor)
    {
        SkiillParametr.PoisonFactor -= factor;
    }
    
    void PotionTimeInAlch(float factor)
    {
        SkiillParametr.PotionTime += factor;
         
    }

    void ReboundStamina(float factor)
    {
        SkiillParametr.StaminaRebound -= factor;
         
    }

    void SpellCount(float factor)
    {
        SkiillParametr.BleedingFactor -= factor;
         
    }
    
    void EatTimeInAlch(float factor)
    {
        SkiillParametr.EatTime += factor;
         
    }
    
    void Herb(float factor)
    {
        SkiillParametr.Herbalism += (byte)factor;
         
    }

    void AdrenalineOn(float factor)
    {
        SkiillParametr.AdrenalineOff = (int)factor;
         
    }
    
    void AdrenalineUp(float factor, float factor2)
    {
        SkiillParametr.AdrenalineUnregen -= factor;
        SkiillParametr.AdrenalineDamage += factor2;
    }
}
