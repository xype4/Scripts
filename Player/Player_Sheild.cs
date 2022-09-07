using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Sheild : MonoBehaviour
{
    public GameObject Sheild;
    public GameObject LastSheild;
    private Skill_Indicator Skills;
    public float SheildDefence;
    public float StaminaSheildBlock;
    
    [HideInInspector]
    public Player_Defence PlayerDefence;
    public Player_Attack PlayerAttack;
    public GameObject InventoryPanel;
    public Inventory inventory;
    public PlayerStats Player_Stats;
    public ControllerBody BodyController;
    public Animator Animation_;
    public bool InAnim = false;

    public bool InBlock = false;
    void Start()
    {
        PlayerDefence  = BodyController.gameObject.GetComponent<Player_Defence>();
        Skills = Player_Stats.gameObject.GetComponent<Skill_Indicator>();
    }

    void Update()
    {
        if(InBlock)
            Player_Stats.Stamina_Count -= StaminaSheildBlock;


        if(Sheild!=null && Animation_.GetCurrentAnimatorStateInfo(1).IsName("Stay"))
        {
            Animation_.SetTrigger("LeftShiendStay");
        }
        if(Sheild==null && Animation_.GetCurrentAnimatorStateInfo(1).IsName("Armature|SheildStay"))
        {
            Animation_.SetTrigger("LeftStay");
        }
        if((Input.GetKey(KeyCode.Mouse1)&& BodyController.timeFactor != 0) && Sheild!=null && InAnim == false)
        {
            InAnim = true;
            StartCoroutine(Block());
        }

        if(Input.GetKeyDown("h") && InAnim == false)
        {
            if(Sheild!= null)
                StartCoroutine(Equip(1));
            if(LastSheild!=null)
                StartCoroutine(Equip(2));

        }
    }

    public void SheildEquip(int sheildNumber)           // Активирует щит, с неомером в иерархии           из CurrentItem(cобытие меча)
    {
        if(PlayerAttack.Sword.GetComponent<WeaponAttack>().WeapType != 2)
        {
            if(Sheild == gameObject.transform.GetChild(sheildNumber).gameObject)
            {
                Sheild.SetActive(false);
                Animation_.SetTrigger("LeftStay");   
                Sheild = null;
            }
            else
            {
                Sheild = gameObject.transform.GetChild(sheildNumber).gameObject;
                Sheild.SetActive(true);
                Animation_.SetTrigger("LeftShiendStay");

            }
        }
    }
    public void SheildUnEquip()
    {
        if(Sheild!=null)
            Sheild.SetActive(false);  
        LastSheild = null;
        Sheild=null;
        Animation_.SetTrigger("LeftStay");   

        for(int i = 0 ; i < inventory.equip.Count; i++)
        {
            if(inventory.equip[i].TypeEquip == 2)
            {
                InventoryPanel.transform.GetChild(i).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
            }
        }
    }
    public void SheildType(int Type)                     // 1- обычный щит    2- малый щит                     из CurrentItem(cобытие меча)
    {
        int TypeSheild = Type;
    }

    IEnumerator Block()
    {
        StaminaSheildBlock = Skills.SheildBlockStamina;
        Animation_.SetBool("BlockDown", false);
        PlayerAttack.inAttack = true; 
        Animation_.SetBool("BlockUp", true);
        yield return new WaitForSeconds(0.3f);

        PlayerDefence.ResistanceSheildItem = Sheild.GetComponent<SheildIDInHand>().ShieldDefence;
        InBlock = true;

        while(Input.GetKey(KeyCode.Mouse1))
        {
            Animation_.SetBool("BlockStay", true);
            yield return new WaitForSeconds(0.2f);
        }

        InBlock = false;
        Animation_.SetBool("BlockStay", false);
        Animation_.SetBool("BlockDown", true);
        PlayerDefence.ResistanceSheildItem = 1; 

        yield return new WaitForSeconds(0.2f);
        PlayerAttack.inAttack = false; 
        Animation_.SetBool("BlockUp", false);
        yield return new WaitForSeconds(0.3f);
        InAnim = false;
    }

    IEnumerator Equip(int act)
    {
        InAnim = true;
        PlayerAttack.inAttack = true;
        if (act == 1)
        {
            Animation_.SetTrigger("SheildDown");
            yield return new WaitForSeconds(0.4f);
            LastSheild = Sheild;
            Sheild.SetActive(false);  
            Sheild=null; 
        }
        else
        {
            Sheild = LastSheild;
            LastSheild = null;
            Animation_.SetTrigger("SheildUp");
            yield return new WaitForSeconds(0.2f);
            Sheild.SetActive(true); 
                 
            yield return new WaitForSeconds(0.3f);
        }
        PlayerAttack.inAttack = false;
        InAnim = false;
    }

    public void Attack()   //1-обычной 2 - тяжклая атаки
    {
        Animation_.SetTrigger("SheildAttack");
    }

    public void LoadData(Save.PlayerSaveData save)
    {
        if(save.LastSheild != 0)
        {
            LastSheild = gameObject.transform.GetChild(save.LastSheild-1).gameObject;
        }
        if(save.Sheild != 0)
        {
            LastSheild = gameObject.transform.GetChild(save.Sheild-1).gameObject;
        }
    }
}
