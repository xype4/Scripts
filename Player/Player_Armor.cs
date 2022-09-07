using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Armor : MonoBehaviour
{

    public float Defence;

    [Tooltip("1 - без брони    2 - легкая   3-средняя   4-тяжёлая")]
    public int TypeDefence;

    [Tooltip("1 - шлем    2 - нагрудник   3-перчатки   4-ботинки")]
    public int TypeArmor;
    public int ID_Item;
}
