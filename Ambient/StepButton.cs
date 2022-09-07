using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StepButton : MonoBehaviour
{
    public bool Upper;
    public GameObject button;
    public UnityEvent Custevents;
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PlayerBody")
        {
            StartCoroutine(ButtonDown());
            Custevents.Invoke();

        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "PlayerBody" && Upper == true)
        {
            StartCoroutine(ButtonUp());

        }
    }

    IEnumerator ButtonDown()
    {
        for(int i = 0; i < 15; i++)
        {
            Vector3 vec = new Vector3 (button.transform.position.x, button.transform.position.y-0.002f, button.transform.position.z);
            button.transform.position = vec;
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator ButtonUp()
    {
        for(int i = 0; i < 15; i++)
        {
            Vector3 vec = new Vector3 (button.transform.position.x, button.transform.position.y+0.002f, button.transform.position.z);
            button.transform.position = vec;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
