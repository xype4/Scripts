using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spell : MonoBehaviour
{
    public string Name;
    [Multiline(5)]
    public string discription;
    public int id;
    public ushort Cost;
    public Sprite icon;
    public UnityEvent customEvent;
}
