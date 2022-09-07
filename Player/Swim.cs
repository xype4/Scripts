using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swim : MonoBehaviour
{
    public ControllerBody Swim_;

    void OnTriggerStay(Collider other)       
    {
        if(other.tag == "PlayerBody")
        {
            Swim_.swim = true;
        }
    }


    void OnTriggerExit(Collider other)
    {
        if(other.tag == "PlayerBody")
        {
            Swim_.swim = false;
        }
    }
}
