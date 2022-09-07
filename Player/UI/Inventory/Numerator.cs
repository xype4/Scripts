using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Numerator : MonoBehaviour
{
    void Awake()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            if(!transform.GetChild(i).GetComponent<Number>())
            {
                Debug.Log("**DBG** - Items/ItemsInWorld - отстутсвие скрипта Number у предмета " + i);
                continue;
            }
            transform.GetChild(i).GetComponent<Number>().Num = i;
        }
    }
}
