using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityArrow : MonoBehaviour
{
    public bool fly = false;
    public float Damage;
    public float Speed = 5;
    public List<int> AngryGroup;
    public PlayerStats PlayerHP;
    
    void Start()
    {
        Destroy(gameObject,60);
    }
    public void SetID(int id, float damage, PlayerStats PH, List<int> angryGroup)
    {
        gameObject.GetComponent<Item>().id = id;
        PlayerHP = PH;
        Speed = 2.5f;
        Damage = damage;
        AngryGroup = angryGroup;
        fly = true;
    }

    void FixedUpdate()
    {
        if(fly)
        {
            transform.Translate(Vector3.forward* Time.deltaTime* 25*Speed); 
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(fly)
        {
            if (col.tag == "PlayerBody")
            {
                PlayerHP.Damage(Damage);
                Destroy(gameObject,0);
                return;
            } 

            if(col.isTrigger == true)
            {
                return;
            }

            if(col.GetComponent<EntityStats>())
            {
                int targetGroup = col.GetComponent<EntityStats>().Group;
                for(int i = 0; i< AngryGroup.Count;i++)
                {
                    if(AngryGroup[i]==targetGroup)
                    {
                        col.GetComponent<EntityStats>().Health-=Damage;
                        Destroy(gameObject,0);
                    }
                }
                return;
            }
                
            fly = false;
        }
    } 
}
