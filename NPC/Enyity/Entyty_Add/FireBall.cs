using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private GameObject Player;
    public float FireBallDamage;

    private bool inBody = true;
    private GameObject Partical;
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(Delete());
        Partical = gameObject.transform.GetChild(0).gameObject;
    }

    void FixedUpdate()
    {
        Partical.transform.localScale -= new Vector3(0.01f,0.01f,0.01f);
        transform.Translate(Vector3.forward* Time.deltaTime* 10); 
        if (Partical.transform.localScale.x <= 0)
            Destroy(gameObject,0);
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "PlayerBody")
        {
            Player.GetComponent<PlayerStats>().Damage(FireBallDamage);
        }

        if(!inBody)
        {
            if(col.GetComponent<BoxCollider>())
            {
                if(col.GetComponent<BoxCollider>().isTrigger == false)
                {
                    Destroy(gameObject,0);
                }
            }
            else
                Destroy(gameObject,0);
        }
    } 

    IEnumerator Delete()
    {
        yield return new WaitForSeconds(1);
        inBody = false;
    }
}

