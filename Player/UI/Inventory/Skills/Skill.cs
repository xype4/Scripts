using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Skill : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public byte group = 0;
    public byte id = 0;
    public string Name;
    [Multiline(5)]
    public string discription;
    public Sprite Icon_lock;
    public Sprite Icon;
    public byte activeCost;
    public byte stageCount;     //Максимальный уровень
    public byte stageActive;     // Действующий уровень
    [Space] [Space]
    public List<byte> cost;
    [Tooltip("Параметр для увеличения")]
    public List<float> stage;
    [Tooltip("Параметр для увеличения 2(если имеется)")]
    public List<float> stage2;
    public List<Sprite> IconActive;
    [Space] [Space]
    public GameObject InfoPanel;
    public PlayerStats Player_Stats;
    public SkillActivate skillActivate;

    [Space] [Space]
    public List<Skill> parent;   //Скилл, при прокачке которого станивтся доступен данный
    // public Skill parent2;
    public bool activate = true;
    public List<Skill> child;   //Скилл, при прокачке данного, который станет доступен

    void Awake()
    {
        activeCost = cost[0];
        gameObject.GetComponent<Image>().sprite = Icon;
    }
    void Start()
    {
        InfoPanel.SetActive(false);
        skillActivate = gameObject.transform.parent.parent.gameObject.GetComponent<SkillActivate>();

        SetActive();
    }

    public void SetActive()
    {
        for(int i = 0; i< parent.Count; i++)
        {
            if(parent[i].stageActive == 0)
            {
                activate = false;
                gameObject.GetComponent<Image>().sprite = Icon_lock;
                break;
            }
            else
                activate = true;
                gameObject.GetComponent<Image>().sprite = Icon;
        }
    }
    public void Skill1()
    {
        if(Player_Stats.Score >= activeCost && stageActive < stageCount && activate)
        {
            Player_Stats.Score-= activeCost;
            Player_Stats.AddEXP(0);
            Icon = IconActive[stageActive];
            
            gameObject.GetComponent<Image>().sprite = Icon;
            skillActivate.Activate(group, id, stage[stageActive], stage2[stageActive]);
            stageActive++;
            DisplaySkill();

            for(int i = 0; i< child.Count; i++)
            {
                child[i].SetActive();
            }

            if(stageCount != stageActive)
            activeCost = cost[stageActive];
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
       DisplaySkill();
    }
    void DisplaySkill()
    {
        InfoPanel.SetActive(true);

        Transform iconInfo = InfoPanel.transform.GetChild(0);
        Image icon_Info = iconInfo.GetComponent<Image>();
        icon_Info.sprite = gameObject.GetComponent<Image>().sprite;

        Transform nameInfo = InfoPanel.transform.GetChild(1);
        Text name_Info = nameInfo.GetComponent<Text>();
        name_Info.text = Name;

        Transform discInfo = InfoPanel.transform.GetChild(2);
        Text disc_Info = discInfo.GetComponent<Text>();
        if(activate == false)
            disc_Info.text = "";
        else
            disc_Info.text = discription;

        Transform activeInfo = InfoPanel.transform.GetChild(3);
        Text active_Info = activeInfo.GetComponent<Text>();
        active_Info.text = " ";

        Transform costInfo = InfoPanel.transform.GetChild(4);
        Text cost_Info = costInfo.GetComponent<Text>();
        if(stageCount == stageActive)
            cost_Info.text = "-";
        else
        {
            cost_Info.text = cost[stageActive].ToString();
        }
        if(activate == false)
            cost_Info.text = "-";


        if(activate)
            active_Info.text = stageActive.ToString() + " / " + stageCount.ToString();

        if(stageCount == stageActive)
        {
              active_Info.text = "MAX";
        }
        if(activate == false)
        {
            active_Info.text = "";
        }
        
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        InfoPanel.SetActive(false);
    }

    public void LoadData(Save.PlayerSkillsData save)
    {
        switch (group)
        {
            case 0:
                stageActive = save.SkillsInterationWarrior[id];
                break;
            case 1:
                stageActive = save.SkillsInterationMagic[id];
                break;
            case 2:
                stageActive = save.SkillsInterationOther[id];
                break;
            case 3:
                stageActive = save.SkillsInterationCraft[id];
                break;
        }
        if(stageActive!=0)
            SkillLoad();
    }

    public void SkillLoad()
    {
        Icon = IconActive[stageActive-1];
        activeCost = cost[stageActive-1];
        gameObject.GetComponent<Image>().sprite = Icon;
        DisplaySkill();
    }
}
