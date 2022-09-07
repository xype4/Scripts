using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door2 : MonoBehaviour
{
    public float Speed = 2;
    public GameObject F;
    private bool IsOpen = false;
    private bool IsProcess = false;
 
    IEnumerator Open()
    {
        IsProcess = true;
        if (IsOpen)
        {
            for (int i = 0; i < (int)87/Speed; i++)
            {
                transform.Rotate(0, 1*Speed, 0);
                yield return new WaitForSeconds(0.001f);
            }
            IsOpen = false;
        }
        else
        {
            for (int i = 0; i < (int)87/Speed; i++)
            {
                transform.Rotate(0, -1*Speed, 0);
                yield return new WaitForSeconds(0.001f);
            }
            IsOpen = true;
        }
        IsProcess = false;
    }
  
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerHandCol")
        F.SetActive(true);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "PlayerHandCol")
            if(Input.GetKeyDown("f") && IsProcess == false)
                StartCoroutine(Open());
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "PlayerHandCol")
            F.SetActive(false);
    }
}
