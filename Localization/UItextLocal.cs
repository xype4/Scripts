using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UItextLocal : MonoBehaviour
{
    public string EngLocal;
    public string RuLocal;
    public string Result;
    private int Local;
    public LangToggle Lang;

    void Start()
    {
        Local = Lang.LangToggle_;

        switch (Local)
        {
            case 0:
                Result = RuLocal;
                break;
            case 1:
                Result = EngLocal;
                break;
        }

        gameObject.GetComponent<Text>().text = Result;
    }
}
