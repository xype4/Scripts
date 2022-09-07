using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camps : MonoBehaviour
{
    public List<GameObject> CampsList = new List<GameObject>();
    public byte SpawnChance;
    private int d;

    void Start()
    {
        for(int i = 0; i < CampsList.Count; i++)
        {
            d = Random.Range(0,100);
            if(d < SpawnChance)
                CampsList[i].SetActive (true);
            else
                CampsList[i].SetActive (false);
        }
    }

}
