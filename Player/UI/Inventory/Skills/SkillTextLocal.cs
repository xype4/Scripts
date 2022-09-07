using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTextLocal : MonoBehaviour
{
    public string EngLocal;
    public string RuLocal;
    private string Result;
    [Multiline(9)]
    public string EngLocalD;
    [Multiline(9)]
    public string RuLocalD;
    [Multiline(9)]
    private string ResultD;
    private int Local;
    public LangToggle Lang;
    private Skill skill;

    void Start()
    {
        Lang = GameObject.FindGameObjectWithTag("Localization").GetComponent<LangToggle>();

        skill = gameObject.GetComponent<Skill>();

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

        skill.Name = Result;
        skill.discription = ResultD;
    }
}
