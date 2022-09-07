using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    private GameObject playerAttack;

    public float MagicDamage = 0;
    [Tooltip("Урон (для лука множитель скорости")]
    public float Damage = 12;

    [Tooltip("ID оружия-предмета")]
    public int WeapID;  //ID оружия

    [Tooltip("Номер в иерархии")]
    public int WeapNumber;     // Новер в иерархии

    [Tooltip("1 - одноручное 2 - двурочное")]
    public int WeapType;     // 1 - одноручное 2 - двурочное

    [Tooltip("1 - дальнобойное")]
    public int Long; // 1 - дальнобойное

    [Tooltip("множитель точность полёта стрелы")]
    public float FlyTimeFacotr; // множитель точность полёта стрелы

    [Space] [Space]
    [Tooltip("время кровотечения")]
    public int bleendingTime;

    [Tooltip("шанс вызвать кровотечения(процент, прибавляется к проценту от скилла и считается)")]
    public int bleendingChance;

    [Tooltip("урон в секунду ")]
    public int bleendingDamage;

    [Space] [Space]

    [Tooltip("время отравления")]
    public int poisonTime;

    [Tooltip("шанс вызвать отравление")]
    public int poisonChance;

    [Tooltip("урон в секунду ")]
    public int poisonDamage;

    private Player_Attack ScriptAttack;
    private bool flag = false;


    void Start()
    {
        playerAttack = transform.parent.gameObject.transform.parent.gameObject;
        ScriptAttack = playerAttack.GetComponent<Player_Attack>();
    }

    private List<EntityStats> ES = new List<EntityStats>();

    void OnTriggerStay(Collider col)
    {
        if (col.GetComponent<EntityStats>())
        {
            for(int i = 0; i < ES.Count;i++)
            {
                if(ES[i] == col.GetComponent<EntityStats>())
                return;
            }

            ES.Add(col.GetComponent<EntityStats>());
            StartCoroutine(Spam_Protect());

            ScriptAttack.Damage(col.gameObject, Damage,0, MagicDamage, bleendingTime, bleendingChance, bleendingDamage, poisonTime, poisonChance, poisonDamage);

        }
    }
    IEnumerator Spam_Protect()
    {
        yield return new WaitForSeconds(2f);
        ES.RemoveAt(0);
    }
}
