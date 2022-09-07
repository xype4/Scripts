using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Item : MonoBehaviour
{
    public string Name;
    public int id;
    public byte countItem;
    public bool isStackable;
    public bool isSell;
    public bool isUsiable;
    public bool infUsiable;
    public bool dropping;
    public ushort Cost;
    [Tooltip("Листо инвентаря. 1- предметы 2- квестовые 3- экипировка")]
    public byte inventoryList; //1- предметы 2- квестовые 3 - экипировка

    [Multiline(5)]
    public string discription;
    public Sprite icon;
    public UnityEvent customEvent;
    [Tooltip("Если премет экипируется. 1-оружее   2-щиты    3-нагрудник    4 - стрела    5-шлем  6-перчатки  7-ботинки")]
    public byte TypeEquip; // 1-оружее   2-щиты    3-нагрудник    4 - стрела    5-шлем  6-перчатки  7-ботинки
    [Tooltip("Тип оружия. 1- обычный меч    2- 1.5 меч    3-лук")]
    public byte Type; // 1- обычный меч    2- 1.5 меч    3-лук
    public List<string> NBTs;

    void Start()                                                                                       //По порядку заполняем страницы инвентаря нулевыми предметами для листа             
    {
        
    }
}
