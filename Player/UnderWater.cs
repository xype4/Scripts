using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderWater : MonoBehaviour
{
    public GameObject WaterScreen;
    public PlayerStats Air;

    void OnTriggerStay(Collider other)       
    {
        if(other.tag == "PlayerBody")
        {
            WaterScreen.SetActive(true);
            Air.Air_Count -= 0.16f;
        }
    }


    void OnTriggerExit(Collider other)
    {
        if(other.tag == "PlayerBody")
        {
            WaterScreen.SetActive(false);
        }
    }
}
