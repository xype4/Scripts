using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    private bool Fire = false;
    public bool AutoStart = false;
    public GameObject F;
    private GameObject Particle;
    private GameObject Light_;

    void Start()
    {
        Particle = gameObject.transform.GetChild(0).gameObject;
        Light_ = gameObject.transform.GetChild(1).gameObject;
        Particle.SetActive(false);
        Light_.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerHandCol" && F !=null)
        F.SetActive(true);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "PlayerHandCol" && F !=null)
            if(Input.GetKeyDown("f"))
                CampFireStart();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "PlayerHandCol" && F !=null)
            F.SetActive(false);
    }

    void CampFireStart()
    {
        if(Fire)
        {
            Fire = false;
            Particle.SetActive(false);
            Light_.SetActive(false);
        }
        else
        {
            Fire = true;
            Particle.SetActive(true);
            Light_.SetActive(true);
        }
    }
}
