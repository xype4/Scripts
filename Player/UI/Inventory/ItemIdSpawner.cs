using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemIdSpawner : MonoBehaviour
{
    public GameObject ItemDataBase;
    void Start()
    {
        for(int i = 0; i< gameObject.transform.childCount; i++)
        {
            for(int j = 0; j < ItemDataBase.transform.childCount; j++)
            {
                if(ItemDataBase.transform.GetChild(j).GetComponent<Item>().id == gameObject.transform.GetChild(i).GetComponent<Item>().id)
                {
                gameObject.transform.GetChild(i).GetComponent<Item>().customEvent = ItemDataBase.transform.GetChild(j).GetComponent<Item>().customEvent;
                gameObject.transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;
                break;
                }
            } 
        }
    }
}
