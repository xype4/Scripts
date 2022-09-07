using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTextLocal : MonoBehaviour
{
    public string EngLocal;
    public string RuLocal;
    private string Result;
    [Multiline(5)]
    public string EngLocalD;
    [Multiline(5)]
    public string RuLocalD;
    [Multiline(5)]
    private string ResultD;
    private int Local;
    public LangToggle Lang;
    private Item item;

    void Start()
    {
        Lang = GameObject.FindGameObjectWithTag("Localization").GetComponent<LangToggle>();

        item = gameObject.GetComponent<Item>();

        Local = Lang.LangToggle_;

        switch (Local)
        {
            case 0:
                Result = RuLocal;
                ResultD = RuLocalD;
                break;
            case 1:
                Result = EngLocal;
                ResultD = EngLocalD;
                break;
        }

        item.Name = Result;
        item.discription = ResultD;
    }
}
