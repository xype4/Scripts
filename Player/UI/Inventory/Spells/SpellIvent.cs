using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellIvent : MonoBehaviour
{
    public GameObject fireBall;
    public Player_Animation PlayerAnimation;
    public Player_Attack ScriptAttack;
    public PlayerStats Player_Stats;
    
    public Animator Animation_;
    public Player_Sheild SheildScript;
    private bool CoolDown = true;
    [Space]
    public GameObject Heal_Part;
    public GameObject Stamina_Part;
    [Space]
    
    private byte hand; //0 - обе руки, 1 - левая, 2 - правая, 4 - всё свободно (не свободно)

    // если 0 или 1 или 4 - справа. если 2 - слева

    public float ManaCostFactor = 1;  //множитель затраты маны <1

    public float ManaCostAttack = 1;    //множитель затраты для атаки
    public float ManaCostHeal = 1;      //множитель затраты для защитф
    public float ManaCostMagic = 1;      //множитель затраты для магии

    public float DamageFactor = 1;  //мноитель для урона
    public float HealFactor = 1;    //множитель для лечения
    public float MagicFactor = 1;    //множитель для лечения магии

    public float RangeFactor = 1;   //множитель дальности полёта
    public float HealStaminaDamage = 1;  //множитель затраты энергии и хп(дел)
    public Skill_Indicator Skills;

    void Start()
    {
    }
    
    void Hand()
    {
        if (ScriptAttack.Sword.GetComponent<WeaponAttack>().WeapType == 2 && ScriptAttack.Sword.activeSelf)
            hand = 0;
        if (ScriptAttack.Sword.GetComponent<WeaponAttack>().WeapType == 1 && ScriptAttack.Sword.activeSelf && SheildScript.Sheild == null)
            hand = 2;
        if (ScriptAttack.Sword.GetComponent<WeaponAttack>().WeapType == 1 && ScriptAttack.Sword.activeSelf && SheildScript.Sheild != null)
            hand = 0;
        if ((ScriptAttack.Sword.GetComponent<WeaponAttack>().WeapType == 0 || !ScriptAttack.Sword.activeSelf) && SheildScript.Sheild != null)
            hand = 1;
        if ((ScriptAttack.Sword.GetComponent<WeaponAttack>().WeapType == 0 || !ScriptAttack.Sword.activeSelf) && SheildScript.Sheild == null)
            hand = 4;
    }


    ////////////////////////////////////////////////////////////////////////////////////////////

                //Огненный шар

    //////////////////////////////////////////////////////////////////////////////////////////////



    public void FireBall(string Info) //мана урон
    {
        string[] info = Info.Split(' ');
        int ManaCost = (int)(System.Convert.ToSingle(info[0])*Skills.ManaFactor*Skills.ManaAttack);    //затрата маны на использование
        int Damage = (int)(System.Convert.ToSingle(info[1]));      //урон

        if(CoolDown && ManaCost <= Player_Stats.ManaCount && Animation_.GetCurrentAnimatorStateInfo(0).IsName("Stay"))
        {
            Player_Stats.ManaCount-=ManaCost;
            Hand();
            StartCoroutine(FireBall_(Damage));
        }
    }

    IEnumerator FireBall_(int Damage)
    {
        CoolDown = false;

        if(hand == 0)
        {
            PlayerAnimation.Equip_();
            yield return new WaitForSeconds(1.4f);
        }

        float x = 0;
        if(hand == 1 || hand == 0 || hand == 4)
        {
            Animation_.SetTrigger("RightMagic");
            x = 0.5f;

        }
        if(hand == 2)
        {
            Animation_.SetTrigger("LeftMagic");
            x = -0.5f;
        }

        yield return new WaitForSeconds(0.4f);


        Vector3 spawn = new Vector3(transform.position.x,transform.position.y-0.8f,transform.position.z);
        GameObject FireBall = Instantiate (fireBall,spawn,transform.rotation) as GameObject;
        FireBall.transform.Translate(new Vector3(x, 0.6f, 1.2f) ,Space.Self);
        FireBall.GetComponent<FireBallPlayer>().Damage = Damage*Skills.SpellDamage;
        FireBall.GetComponent<FireBallPlayer>().ScaleDown/=RangeFactor; 
        yield return new WaitForSeconds(2);
        CoolDown = true;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////

                //Реген здоровья

    //////////////////////////////////////////////////////////////////////////////////////////////

    public void Heal(string Info)  //затрата маны; реген, хп/сек; время
    {
        string[] info = Info.Split(' ');
        int ManaCost = (int)(System.Convert.ToSingle(info[0])*Skills.ManaFactor*Skills.ManaHeal);    //затрата маны на использование
        int Damage = (int)(System.Convert.ToSingle(info[1]));      //урон
        int Time = (int)(System.Convert.ToSingle(info[2]));             //время действия

        if(CoolDown && ManaCost <= Player_Stats.ManaCount && Animation_.GetCurrentAnimatorStateInfo(0).IsName("Stay"))
        {
        Player_Stats.ManaCount-=ManaCost;
        Hand();
        StartCoroutine(Heal_(Damage, Time));
        }
    }
    IEnumerator Heal_(float Regen, float Time)
    {
        CoolDown = false;

        if(hand == 0)
        {
            PlayerAnimation.Equip_();
            yield return new WaitForSeconds(1.4f);
        }

        if(hand == 1 || hand == 0 || hand == 4)
        {
            Animation_.SetTrigger("RightMagic");
        }
        if(hand == 2)
        {
            Animation_.SetTrigger("LefttMagic");
        }

        yield return new WaitForSeconds(0.4f);

        GameObject Particle;
        Particle = Instantiate (Heal_Part,transform.position,transform.rotation) as GameObject;
        Particle.transform.SetParent(this.transform);
        Regen/=20/Skills.SpellHeal;
        Time *=20;
        for(int i=0; i < Time; i++)
        {
           Player_Stats.HP_Count += Regen; 
           yield return new WaitForSeconds(0.05f);
        }
        Destroy(Particle,0);
        CoolDown = true;
    }


    ////////////////////////////////////////////////////////////////////////////////////////////

                //Реген стамины

    //////////////////////////////////////////////////////////////////////////////////////////////



    public void HealStamina(string Info)  //затрата маны; реген, хп/сек; время
    {
        string[] info = Info.Split(' ');
        int ManaCost = (int)(System.Convert.ToSingle(info[0])*Skills.ManaFactor*Skills.ManaHeal);    //затрата маны на использование
        int Damage = (int)(System.Convert.ToSingle(info[1]));      //урон
        int Time = (int)(System.Convert.ToSingle(info[2]));             //время действия

        if(CoolDown && ManaCost <= Player_Stats.ManaCount && Animation_.GetCurrentAnimatorStateInfo(0).IsName("Stay"))
        {
        Player_Stats.ManaCount-=ManaCost;
        Hand();
        StartCoroutine(HealStamina_(Damage, Time));
        }
    }
    IEnumerator HealStamina_(float Regen, float Time)
    {
        CoolDown = false;

        if(hand == 0)
        {
            PlayerAnimation.Equip_();
            yield return new WaitForSeconds(1.4f);
        }

        if(hand == 1 || hand == 0 || hand == 4)
        {
            Animation_.SetTrigger("RightMagic");
        }
        if(hand == 2)
        {
            Animation_.SetTrigger("LefttMagic");
        }

        yield return new WaitForSeconds(0.4f);

        GameObject Particle;
        Particle = Instantiate (Stamina_Part,transform.position,transform.rotation) as GameObject;
        Particle.transform.SetParent(this.transform);
        Regen/=20/Skills.SpellHeal;
        Time *=20;
        for(int i=0; i < Time; i++)
        {
           Player_Stats.Stamina_Count += Regen; 
           yield return new WaitForSeconds(0.05f);
        }
        Destroy(Particle,0);
        CoolDown = true;
    }
}
