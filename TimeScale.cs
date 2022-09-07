using System.Collections;
// using System.Math;
using System.Collections.Generic;
using UnityEngine;

public class TimeScale : MonoBehaviour
{
    public byte hours = 12;
    public byte minutes = 0;
    public byte seconds = 0;
    public float Move = 0;
    public float MoveFunc = 0;

    void FixedUpdate()
    {
        Move += 0.03f;
        if(Move > 360f) Move -=360f;
        transform.rotation = Quaternion.Euler(Move,0f,0f);

        if((Move>0 && Move < 36)||(Move>144 && Move<180))
        {
            MoveFunc = Move;
            if(Move> 90)
                MoveFunc = 180-Move;
            MoveFunc*=2.5f;
            Debug.Log("MoveFunc");
            gameObject.GetComponent<Light>().intensity = Mathf.Sin(MoveFunc/57.2958f);
        }
        
        if(Move>180 && Move < 360)
            gameObject.GetComponent<Light>().intensity = 0;
    }
}
