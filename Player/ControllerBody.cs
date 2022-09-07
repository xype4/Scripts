using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerBody : MonoBehaviour
{
    public float Run;
    public float Speed = 4f;
    public float SpeedRunBase = 8f;
    //[HideInInspector]
    public float SpeedRunHeavyFactor = 0f;
    public float HeavyRun = 2f;
    public float JumpSpeed = 6f;
    public float SpeedSwimRun = 6f;
    private float UpSwimSpeed = 30f;
    private float Gravity = 30f;
    public float StaminaRebound;
    private GameObject Stamina;
    private Skill_Indicator Skills;
    private bool run;
    private Vector3 moveDir = Vector3.zero;
    private CharacterController ControlleR;
    private GameObject Camera;
    public float StaminaCount;
    private bool reb1 = false;
    private bool reb1trigger = true;
    private bool reb2 = false;
    private bool reb2trigger = true;
    private bool reb3 = false;
    private bool reb3trigger = true;
    [HideInInspector]
    public float timeFactor = 1;
    private float angle;
    [HideInInspector]
    public bool swim = false;
    private float swimFactor;

    private float TimeConts;
    private float TimeConts2 = 0;
    private float TimeCs = 0;
    
    void Start()
    {
        Skills = gameObject.GetComponent<Skill_Indicator>();
        Camera = gameObject.transform.GetChild(0).gameObject;
        Stamina = Camera.transform.GetChild(1).gameObject;
        ControlleR = GetComponent<CharacterController>();
        StaminaCount = gameObject.GetComponent<PlayerStats>().Stamina_Count;
        Run = 0.2f;
        TimeConts = 1/Time.fixedDeltaTime;
    }

    void FixedUpdate()
    {
        float SpeedRun = SpeedRunBase + Skills.RunSpeed - SpeedRunHeavyFactor;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit)) 
            angle = Vector3.Angle(Vector3.up, hit.normal);

        StaminaCount = gameObject.GetComponent<PlayerStats>().Stamina_Count;

        run = false;

        if((ControlleR.isGrounded || swim) && (Input.GetKey("left shift") && StaminaCount > 5))
        {
            moveDir = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));
            if (moveDir.z < 0) moveDir.z *= 0.6f; 
            moveDir = transform.TransformDirection(moveDir);
            if(!swim)
            {
                moveDir *= SpeedRun;
                moveDir.y -= 4f;
            }
            else
                moveDir *= SpeedSwimRun;
            StaminaCount-=0.3f;
            run = true;
        }

        if((ControlleR.isGrounded || swim) && run == false && StaminaCount > 5)
        {
            moveDir = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));
            moveDir = transform.TransformDirection(moveDir);
            moveDir *= Speed;
        }

        if(StaminaCount <=5)
        {
            moveDir = new Vector3(0,0,0);
        }

        Rebounds();

        if(Input.GetKey("space"))
        {
            if(ControlleR.isGrounded && StaminaCount >= 10 && angle < 30)
            {
                moveDir.y +=JumpSpeed;
                if(!run)
                    moveDir.y += 0.7f;
                else
                    moveDir.y += 4f;
                StaminaCount-=10;
            }
            if(swim)
            {
                moveDir.y +=UpSwimSpeed*swimFactor;
                swimFactor+=0.04f;
                if(swimFactor > 1)
                    swimFactor = 1;
            }
        }

        run = false;

        if(!swim)
        {
            if(ControlleR.isGrounded)
                moveDir.y -= Gravity*0.06f;
            else
                moveDir.y -= Gravity*0.02f;
            swimFactor = 0.2f;
        }
        else
            moveDir.y -= Gravity*0.02f*6;
        ControlleR.Move (moveDir* 0.02f*timeFactor);

        transform.rotation = Quaternion.Euler(0f,Camera.GetComponent<ControllerHad>().MoveX,0f); 

        gameObject.GetComponent<PlayerStats>().Stamina_Count = StaminaCount;
        
    }

    public void Rebounds()
    {
        StaminaRebound = Skills.StaminaRebound;
        if(Input.GetKey("s") && ControlleR.isGrounded && StaminaCount > StaminaRebound && reb1trigger == true)
            {
                reb1trigger = false;
                if(reb1)
                {
                    moveDir = new Vector3(0,6,-6);
                    StaminaCount-=StaminaRebound;
                    moveDir = transform.TransformDirection(moveDir);
                }
                else
                    StartCoroutine(rebound(1));
            }
            if(!Input.GetKey("s"))
                reb1trigger = true;

            if(Input.GetKey("a") && ControlleR.isGrounded && StaminaCount > StaminaRebound && reb2trigger == true)
            {
                reb2trigger = false;
                if(reb2)
                {
                    moveDir = new Vector3(-7,6,0);
                    StaminaCount-=StaminaRebound;
                    moveDir = transform.TransformDirection(moveDir);
                }
                else
                    StartCoroutine(rebound(2));
            }
            if(!Input.GetKey("a"))
                reb2trigger = true;

            if(Input.GetKey("d") && ControlleR.isGrounded && StaminaCount > StaminaRebound && reb3trigger == true)
            {
                reb3trigger = false;
                if(reb3)
                {
                    moveDir = new Vector3(7,6,0);
                    StaminaCount-=StaminaRebound;
                    moveDir = transform.TransformDirection(moveDir);
                }
                else
                    StartCoroutine(rebound(3));
            }
            if(!Input.GetKey("d"))
                reb3trigger = true;
    }

    IEnumerator rebound(int type)
    {
        yield return new WaitForSeconds(0.05f);
        switch (type)
        {
            case 1:
                reb1 = true;
                break;
            case 2:
                reb2 = true;
                break;
            case 3:
                reb3 = true;
                break;
        }
        yield return new WaitForSeconds(0.3f);
        reb1 = reb2 = reb3 = false;
    }

    public void LoadData(Save.PlayerSaveData save)
    {
        ControlleR.enabled = false;

        gameObject.transform.position = new Vector3(save.Position.x, save.Position.y, save.Position.z);

        ControlleR.enabled = true;
    }
    
}
