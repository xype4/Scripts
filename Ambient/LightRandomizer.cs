using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRandomizer : MonoBehaviour
{
    public int Chance;
    public GameObject player;
    public bool distanceDisable;
    public float distance;
    public float dist;
    private Light light_;
    private Vector3 self;

    void Start()
    {
        light_ = gameObject.GetComponent<Light>();
        self = gameObject.transform.position;
        if(Chance>Random.Range(1,100))
            light_.enabled = true;
    }
    void Update() 
    {
        if(distanceDisable && player!=null)
        {
            dist = (self - player.transform.position).magnitude;
            if(distance > dist)
            {
                light_.enabled = true;
            }
            else
            {
                light_.enabled = false;
            }
        }
        
    }
}
