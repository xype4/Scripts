using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EntityStats : MonoBehaviour
{
    public float HealthMax;
    public float Health;
    [Space]
    [Header("1 - полная защита")]
    public float HealthRegeneratoin;
    [Range(0f, 1f)]
    public float Resistance;
    [Range(0f, 1f)]
    public float MagicResistance;
    [Range(0f, 1f)]
    public float BleendingResistance;
    [Range(0f, 1f)]
    public float PoisonResistance;
    public Rigidbody[] rb_AR;
    public bool die = false;
    
    [HideInInspector]
    public int Group;
    [HideInInspector]
    public List<int> AngryGroup;


    [Space]
    [Header("Настройки дропа")]

    public Inventory inventory;
    public GameObject DropPanel;
    public GameObject F;
    public GameObject DropPrefab;

    [Tooltip("Обычные предметы")]

    public Item[] Drop = new Item[4];
    [Range(0,10)]     [Tooltip("Макс. количество прдметов, ведится на 2")]

    public int[] Range = new int[4];

    public Item[] RareDrop = new Item[3];
    [Range(0,10)]     [Tooltip("Макс. клличество прдметов, ведится на 4")]
    public int[] RareRange = new int[3];


    void Start()
    {
        Health = HealthMax;
        rb_AR = GetComponentsInChildren<Rigidbody>();
    }

    
    void Update()
    {
        if(Health < HealthMax)
        {
            Health += HealthRegeneratoin/50*Time.deltaTime;
        }
        if(Health <=0)
        {
            GetComponent<NavMeshAgent>().speed = 0;
            GetComponent<BoxCollider>().isTrigger = true;
            GetComponent<Animator>().enabled = false;
            if(GetComponent<SimpleAI>()) 
            {
                GetComponent<SimpleAI>().die();
                GetComponent<SimpleAI>().enabled = false;
            }
            if(GetComponent<AnimalAI>()) 
            {
                GetComponent<AnimalAI>().enabled = false;
            }

            GameObject drop = Instantiate(DropPrefab, gameObject.transform.position, Quaternion.identity);
            drop.GetComponent<DropMob>().DropPanel = DropPanel;
            drop.GetComponent<DropMob>().inventory = inventory;
            drop.GetComponent<DropMob>().F = F;
            drop.GetComponent<DropMob>().Drop = Drop;
            drop.GetComponent<DropMob>().Range = Range;
            drop.GetComponent<DropMob>().RareDrop = RareDrop;
            drop.GetComponent<DropMob>().RareRange = RareRange;
            
            die = true;
            for(int i = 0; i < rb_AR.Length; i ++)
            {
                rb_AR[i].isKinematic = false;
            }
            Destroy(gameObject,120);
            GetComponent<EntityStats>().enabled = false;
        }
    }

    public void InputDamage(int finalBleendingDamage,int finalBleendingTime,int finalPoisonDamage,int finalPoisonTime,float Damage,float PoisonOilDamage,float magicDamage, bool player)
    {
        Debug.Log(Damage+" "+PoisonOilDamage);
        //Debug.Log("Кровь урон" + finalBleendingDamage + " время" + finalBleendingTime);

        if(player && gameObject.GetComponent<SimpleAI>())
        {
            gameObject.GetComponent<SimpleAI>().target = gameObject.GetComponent<SimpleAI>().player;
            gameObject.GetComponent<SimpleAI>().friendly = 1;
        }
        
        Health -= PoisonOilDamage - PoisonOilDamage*PoisonResistance;
        Health -= Damage - Damage*Resistance;
        Health -= magicDamage - magicDamage * MagicResistance;
        if (finalBleendingTime > 0)
        StartCoroutine(Bleending(finalBleendingDamage, finalBleendingTime));
        if(finalPoisonTime > 0)
        StartCoroutine(Poison(finalPoisonDamage, finalPoisonTime));
    }

    IEnumerator Bleending(int damage, int time)
    {
        float damagePortion =(float) damage/(float) time/5f; //1, 10
        for(int i = 0; i < time*5; i++)
        {
            Health -= (damagePortion * (1-(BleendingResistance)));
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator Poison(int damage, int time)
    {
        float damagePortion =(float) damage/(float) time/5f;
        for(int i = 0; i < time*5; i++)
        {
            Health -= damagePortion * (1-(PoisonResistance));
            yield return new WaitForSeconds(0.2f);
        }
    }
}
