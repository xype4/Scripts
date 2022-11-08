using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityArrow : MonoBehaviour
{
    private bool fly = false;
    private float Damage;
    private float Speed = 5;
    private List<int> AngryGroup;
    private PlayerStats _playerStats;
    public int id;
    
    void Start()
    {
        Destroy(gameObject,60);
    }
    public void Initialization(float damage, PlayerStats playerStats, List<int> angryGroup)
    {
        _playerStats = playerStats;
        Speed = 3f;
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
                _playerStats.Damage(Damage);
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
