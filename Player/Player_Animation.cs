using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animation : MonoBehaviour
{
    public Player_Defence ScriptDefence;
    public Inventory inventory;

    private byte ArrowNumber = 0;

    public GameObject ArrowOnBow;
    private Animator Animation_;
    private Animator Animation_bow;

    public Skill_Indicator Skill;
    public Player_Attack ScriptAttack;
    private string attack;
    [HideInInspector]
    public float SwordDefence;
    public PlayerStats Player_Stats;
    [HideInInspector]
    public float StaminaBlock;
    [HideInInspector]
    public bool InFire = false;
    [HideInInspector]
    public bool bowShoot = false;
    [HideInInspector]
    public int Dispersion;
     [Space] [Space]
    public GameObject Arrow;
    public GameObject GoodArrow;
    public ArrowManager arrowManager;

    private Vector3 arrow_start_position;
    [Header("SkillIndicator")]
    [Space]
    public float AttackSpeedFactor = 1;

    
    void Start()
    {
        Animation_ = gameObject.GetComponent<Animator>();
    }
    public void Attack_type(int type, int WeaponType)
    {
        AttackSpeedFactor = Skill.StayTimeAttack;
        switch (WeaponType)
        {
            case 0:
                StartCoroutine(HandAttack());
                break;
            case 1:
                if (type == 1) StartCoroutine(CommonAttack());
                if (type == 2) StartCoroutine(HeavyAttack());
                break;
            case 2:
                if (type == 1) StartCoroutine(Common_15Attack());
                if (type == 2) StartCoroutine(Heavy_15Attack());
                break;
            case 3:                
                StartCoroutine(Fire());
                Animation_bow = ScriptAttack.Sword.transform.GetChild(ScriptAttack.Sword.transform.childCount-1).gameObject.GetComponent<Animator>();
                break;

        }
        
    }

    public void Equip_()
    {
        StartCoroutine(Equip());
    }

    public void Fire_()
    {
        StartCoroutine(Fire());
    }


     IEnumerator HandAttack()
    {
        ScriptAttack.inAttack = true; 
        ScriptAttack.Sword.GetComponent<BoxCollider>().enabled = true;
        int d = Random.Range(0,3);
        switch (d)
        {
            case 0:
                attack = "HandAttack1";
                break;
            case 1:
                attack = "HandAttack2";
                break;
            case 2:
                attack = "HandAttack3";
                break;
        }
        Animation_.SetBool(attack, true);
        yield return new WaitForSeconds(0.2f);
        Animation_.SetBool(attack, false);
        ScriptAttack.Sword.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(0.2f*AttackSpeedFactor);
        ScriptAttack.inAttack = false;
    }

    IEnumerator CommonAttack()
    {
        ScriptAttack.inAttack = true; 
        ScriptAttack.Sword.GetComponent<BoxCollider>().enabled = true;
        int d = Random.Range(0,3);
        switch (d)
        {
            case 0:
                attack = "SwordAttack1";
                break;
            case 1:
                attack = "SwordAttack2";
                break;
            case 2:
                attack = "SwordAttack3";
                break;
        }
        Animation_.SetBool(attack, true);
        yield return new WaitForSeconds(0.25f);
        Animation_.SetBool(attack, false);
        yield return new WaitForSeconds(0.5f);
        ScriptAttack.Sword.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(0.75f*AttackSpeedFactor);
        ScriptAttack.inAttack = false;
    }
    IEnumerator HeavyAttack()
    {
        ScriptAttack.inAttack = true;
        ScriptAttack.Sword.GetComponent<BoxCollider>().enabled = true;
        int d = Random.Range(0,2);
        switch (d)
        {
            case 0:
                attack = "SwordHeavyAttack1";
                break;
            case 1:
                attack = "SwordHeavyAttack2";
                break;
        }
        Animation_.SetBool(attack, true);
        yield return new WaitForSeconds(0.22f);
        Animation_.SetBool(attack, false);
        yield return new WaitForSeconds(1f);
        ScriptAttack.Sword.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(1.22f*AttackSpeedFactor);
        ScriptAttack.inAttack = false;
    }

    IEnumerator Common_15Attack()
    {
        ScriptAttack.inAttack = true;
        ScriptAttack.Sword.GetComponent<BoxCollider>().enabled = true;
        int d = Random.Range(0,2);
        switch (d)
        {
            case 0:
                attack = "Sword1.5Attack1";
                break;
            case 1:
                attack = "Sword1.5Attack2";
                break;
        }
        Animation_.SetBool(attack, true);
        yield return new WaitForSeconds(0.1f);
        Animation_.SetBool(attack, false);
        yield return new WaitForSeconds(1f);
        ScriptAttack.Sword.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(1.1f*AttackSpeedFactor);
        ScriptAttack.inAttack = false;
    }

    IEnumerator Heavy_15Attack()
    {
        ScriptAttack.inAttack = true;
        ScriptAttack.Sword.GetComponent<BoxCollider>().enabled = true;
        int d = Random.Range(0,2);
        switch (d)
        {
            case 0:
                attack = "Sword1.5HeavyAttack1";
                break;
            case 1:
                attack = "Sword1.5HeavyAttack2";
                break;
        }
        Animation_.SetBool(attack, true);
        yield return new WaitForSeconds(0.5f);
        Animation_.SetBool(attack, false);
        yield return new WaitForSeconds(1f);
        ScriptAttack.Sword.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(1.5f*AttackSpeedFactor);
        ScriptAttack.inAttack = false;
    }



    IEnumerator Equip()
    {
        ScriptAttack.inAttack = true;
        if (ScriptAttack.Sword.activeSelf)
        {
            Animation_.SetBool("UnEquip",true);
            yield return new WaitForSeconds(0.1f);
            Animation_.SetBool("UnEquip",false);
            yield return new WaitForSeconds(0.3f);
            ScriptAttack.Sword.SetActive (false);  
        }
        else
        {
            Animation_.SetBool("Equip",true);
            yield return new WaitForSeconds(0.1f);
            Animation_.SetBool("Equip",false);
            yield return new WaitForSeconds(0.3f);
            ScriptAttack.Sword.SetActive (true);
        }
        yield return new WaitForSeconds(1.1f);

        ScriptAttack.inAttack = false;
    }

    IEnumerator Fire()
    {
            ScriptAttack.inAttack = true; 
            Animation_.SetBool("BowFireStart",true);

            ScriptAttack.ArrowEquipUpdate(ScriptAttack.Arrow.id);

            if(ScriptAttack.Arrow.count > 0)
            {
                switch (ScriptAttack.Arrow.id)
                {
                    case 2002:  //id обычной стерлы
                        ArrowNumber = 0;
                        break;
                    case 2001:  //id хорошей стрелы
                        ArrowNumber = 1;
                        break;   
                } 
                ScriptAttack.Sword.transform.GetChild(ArrowNumber).transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = true;
                ScriptAttack.Sword.transform.GetChild(ArrowNumber).transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().enabled = true;
                ScriptAttack.Sword.transform.GetChild(ArrowNumber).transform.GetChild(2).gameObject.GetComponent<MeshRenderer>().enabled = true;
            }
            yield return new WaitForSeconds(0.5f);

            arrow_start_position = ScriptAttack.Sword.transform.GetChild(ArrowNumber).gameObject.transform.localPosition;

            Animation_bow.SetTrigger("Fire");
            Animation_bow.ResetTrigger("InIdle");
            for(int i = 0;i<15;i++)
            {
                ScriptAttack.Sword.transform.GetChild(ArrowNumber).gameObject.transform.Translate(0,0,-0.02f, Space.Self);
                yield return new WaitForSeconds(0.05f);
            }
            Animation_.SetBool("BowFireStart",false);
            yield return new WaitForSeconds(0.2f);

            while(Input.GetKey(KeyCode.Mouse0))
            {
                yield return new WaitForSeconds(0.05f);
                if(Input.GetKey(KeyCode.Mouse1))
                {
                    StartCoroutine(BowShoot(ScriptAttack.Arrow.id));
                    yield break;
                }

            }
            Animation_bow.ResetTrigger("Fire");
            StartCoroutine(BowDown(true));
    }

    IEnumerator BowShoot(int id)
    {
            GameObject Arr;        
            Animation_bow.ResetTrigger("Fire");
            Animation_bow.SetTrigger("InFire");

            if(id != 0 && inventory.equip[ScriptAttack.Arrow.InvCell].countItem != 0)
            {
                ScriptAttack.Arrow.count --;
                inventory.equip[ScriptAttack.Arrow.InvCell].countItem --;
                inventory.DisplayItems();
                Dispersion = (int)((float)Skill.Dispersion*ScriptAttack.Sword.GetComponent<WeaponAttack>().FlyTimeFacotr);
                Vector3 dispersion = new Vector3((float)Random.Range(-Dispersion,Dispersion)/10, (float)Random.Range(-Dispersion,Dispersion)/10, (float)Random.Range(-Dispersion,Dispersion)/10);

                GameObject CreateArrow = ScriptAttack.Sword.transform.GetChild(ArrowNumber).gameObject;
                    
                float damage = 0; float flyFactor = 0; float speedFactor = 0;

                for(int i = 0; i < arrowManager.arrows.Count; i++)
                {
                    if(arrowManager.arrows[i].id == id)
                    {
                        damage = arrowManager.arrows[i].damage;
                        flyFactor = arrowManager.arrows[i].flyRange;
                        speedFactor = arrowManager.arrows[i].speed * Skill.ArrowFly * ScriptAttack.Sword.GetComponent<WeaponAttack>().Damage;
                        break;
                    }
                }

                Arr = Instantiate (CreateArrow, ScriptAttack.Sword.transform.GetChild(0).position,transform.parent.gameObject.transform.rotation) as GameObject;
                Arr.transform.localScale = new Vector3(Arr.transform.localScale.x, Arr.transform.localScale.y, Arr.transform.localScale.z);
                Arr.transform.Rotate(dispersion, Space.Self);
                Arr.GetComponent<Arrow>().enabled = true;
                Arr.GetComponent<BoxCollider>().enabled = true;
                Arr.GetComponent<Arrow>().SetID(ScriptAttack.Arrow.id, ScriptAttack, damage, flyFactor, speedFactor);
                ScriptAttack.ArrowEquipUpdate(ScriptAttack.Arrow.id);
                    
                ArrrowInvise();
            }
        yield return new WaitForSeconds(0.05f);
        StartCoroutine(BowDown(false));
    }

    IEnumerator BowDown(bool arrowDisable)
    {
        yield return new WaitForSeconds(0.3f);
        Animation_bow.ResetTrigger("Fire");
        Animation_bow.SetTrigger("InIdle");
        Animation_.SetBool("BowFireFinish",true);
        yield return new WaitForSeconds(0.1f);
        Animation_.SetBool("BowFireFinish",false);

        foreach (var param in Animation_bow.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Trigger)
            {
                Animation_bow.ResetTrigger(param.name);
            }
        }
        Animation_bow.Rebind();

        if(arrowDisable)
            ArrrowInvise();

        ScriptAttack.Sword.transform.GetChild(ArrowNumber).gameObject.transform.localPosition = arrow_start_position;
        yield return new WaitForSeconds(1f);
        ScriptAttack.inAttack = false; 
    }

    private void ArrrowInvise()
    {
        ScriptAttack.Sword.transform.GetChild(ArrowNumber).transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = false;
        ScriptAttack.Sword.transform.GetChild(ArrowNumber).transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().enabled = false;
        ScriptAttack.Sword.transform.GetChild(ArrowNumber).transform.GetChild(2).gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
}
