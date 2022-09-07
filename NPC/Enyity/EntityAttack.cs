using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAttack : MonoBehaviour
{
    public BoxCollider AttackColleder;
    public PlayerStats Player_Stats;
    [HideInInspector]

    public EntityStats StatsScript;
    public float AttackTime = 0.3f;
    public float damage;
    private bool flag = false;

    void Start()
    {
        AttackColleder = gameObject.GetComponent<BoxCollider>();
    }


    public void StartAttack_()
    {
        StartCoroutine(StartAttack());
    }

    public void AttackStartCollider()
    {
        if(AttackColleder!=null)
            AttackColleder.enabled = true;
        else
        {
            AttackColleder = gameObject.GetComponent<BoxCollider>();
            AttackColleder.enabled = true;
        }
    }

    public void AttackFinishCollider()
    {
        if(AttackColleder!=null)
            AttackColleder.enabled = false;
        else
        {
            AttackColleder = gameObject.GetComponent<BoxCollider>();
            AttackColleder.enabled = false;
        }
    }
    
    IEnumerator StartAttack ()
    {
        yield return new WaitForSeconds(0.1f);
        AttackColleder.enabled = true;
        yield return new WaitForSeconds(AttackTime);
        AttackColleder.enabled = false;
    }

    void OnTriggerStay(Collider col)
    {
        if (col.GetComponent<PlayerStats>() && flag == false)
        {
            StartCoroutine(UnDamageTime());
            Player_Stats.Damage(damage * Random.Range(0.8f,1.2f));
        }

        if (col.GetComponent<EntityStats>() && flag == false)
        {
            int targetGroup = col.GetComponent<EntityStats>().Group;
            for(int i = 0; i < StatsScript.AngryGroup.Count;i++)
            {
                if(StatsScript.AngryGroup[i] == targetGroup)
                {
                    StartCoroutine(UnDamageTime());
                    col.GetComponent<EntityStats>().Health-=damage;
                    return;
                }
            }
        }
    }

    IEnumerator UnDamageTime()
    {
        flag = true;
        yield return new WaitForSeconds(3);
        flag = false;
    }
}
