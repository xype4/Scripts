using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallPlayer : MonoBehaviour
{
    public bool fly = true;
    public float Damage;
    public float FlyFactor;
    private GameObject Partical;
    public float ScaleDown = 0.015f;
    public float ScaleUP = 0.2f;
    bool BlowTrigger = false;
    void Start()
    {
        Partical = gameObject.transform.GetChild(0).gameObject;
    }

    void FixedUpdate()
    {
        if(!BlowTrigger)
        {
            transform.Translate(Vector3.forward* Time.deltaTime* 20); 
            Partical.transform.localScale -= new Vector3(ScaleDown,ScaleDown,ScaleDown);

        }
        else
        {
            Partical.transform.localScale += new Vector3(ScaleUP,ScaleUP,ScaleUP);
            transform.Translate(Vector3.forward* Time.deltaTime* 30); 
        }
        if (Partical.transform.localScale.x <= 0)
            Destroy(gameObject,0);
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player"|| col.tag == "PlayerHandCol"|| col.tag == "PlayerBody")
        {
            return;
        }
        if(col.tag == "NPC")
        {
            DestroyFireBall();
            return;
        }

        if (col.GetComponent<EntityStats>())
        {
            col.GetComponent<EntityStats>().InputDamage(0,0, 0, 0, 0, 0, Damage, true);  
            DestroyFireBall();
            return;
        } 
        DestroyFireBall();
    }

    private void DestroyFireBall()
    {
        BlowTrigger = true;
        Destroy(gameObject,0.5f);
    }
} 


