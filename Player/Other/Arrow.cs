using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public bool fly = false;
    public float Damage;
    public float FlyFactor;
    public float Speed;
    private Player_Attack ScriptAttack;
    public GameObject test;
    void Start()
    {
        Destroy(gameObject,60);
    }
    public void SetID(int id, Player_Attack pl_at, float damage, float flyFactor, float speed)
    {
        gameObject.GetComponent<Item>().id = id;
        ScriptAttack = pl_at;
        Damage = damage;
        FlyFactor = flyFactor;
        Speed = speed;
        fly = true;
    }

    void FixedUpdate()
    {
        if(fly)
        {
            transform.Translate(Vector3.forward* Time.deltaTime* 25*Speed); 
            transform.Rotate(0.15f/FlyFactor, 0, 0, Space.Self);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(fly)
        {
            if(col.isTrigger == true)
            {
                return;
            }

            if (col.GetComponent<EntityStats>())
            {
                ScriptAttack.Damage(col.gameObject, (byte)Damage);     
                Destroy(gameObject,0);
                return;
            } 
                
            if(col.tag == "Player"|| col.tag == "PlayerHandCol"|| col.tag == "PlayerBody" || col.tag == "PlayerBow")
            {
                return;
            }

            test = col.gameObject;
            fly = false;
        }
    } 
}
