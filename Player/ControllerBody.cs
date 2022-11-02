using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerBody : MonoBehaviour
{
    public float speedWalkSpeed = 4f;
    public float speedRunSpeed = 8f;
    public float speedRunSpeedWithSkills;
    [Space]
    [HideInInspector] public float SpeedRunHeavyFactor = 0f;
    [Space]
    public float jumpVelocity = 6f;
    private float jumpVelocityMoment = 0f;
    public float gravity = 300f;
    public float jumpStamina = 10f;
    [Space]
    public float speedSwimRun = 6f;
    public float speedSwim = 3f;
    public float upSwimSpeed = 30f;
    [Space]
    public float staminaRebound;
    private PlayerStats _playerStats;
    private Skill_Indicator Skills;

    private bool run = false;
    private bool walk = false;
    [HideInInspector] public bool swim = false;

    private Vector3 moveDir = Vector3.zero;
    private CharacterController ControlleR;
    public ControllerHad Camera;
    private Animator cameraAnimator;
    
    [HideInInspector] public float staminaCount;

    private bool reb1 = false;
    private bool reb1trigger = true;
    private bool reb2 = false;
    private bool reb2trigger = true;
    private bool reb3 = false;
    private bool reb3trigger = true;
    [HideInInspector]
    public float timeFactor = 1;
    private float angle;
    private float swimFactor;

    private float TimeConts;
    private float TimeConts2 = 0;
    private float TimeCs = 0;
    
    void Start()
    {
        Skills = gameObject.GetComponent<Skill_Indicator>();
        ControlleR = GetComponent<CharacterController>();
        _playerStats = gameObject.GetComponent<PlayerStats>();
        staminaCount = _playerStats.Stamina_Count;
        TimeConts = 1/Time.fixedDeltaTime;
        cameraAnimator = Camera.gameObject.GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        staminaCount = _playerStats.Stamina_Count;
        speedRunSpeedWithSkills = speedRunSpeed + Skills.RunSpeed - SpeedRunHeavyFactor;


        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit)) 
            angle = Vector3.Angle(Vector3.up, hit.normal);


        moveDir = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));


        run = false;
        walk = false;

        
        Run();
        Walk();
        Stay();
        Rebounds();
        Jump();
        Gravity();
        

        moveDir = transform.TransformDirection(moveDir);
        ControlleR.Move (moveDir* 0.02f*timeFactor);

        transform.rotation = Quaternion.Euler(0f,Camera.MoveX,0f); 

        _playerStats.Stamina_Count = staminaCount;
        
    }

    void Run()
    {
        if(run == false && staminaCount > 5 && Input.GetKey("left shift") && moveDir!=new Vector3(0f,0f,0f))
        {
            cameraAnimator.SetTrigger("ToRun");
            cameraAnimator.ResetTrigger("ToStay");
            cameraAnimator.ResetTrigger("ToWalk");
            if (moveDir.z < 0)
                moveDir.z *= 0.6f; 

            if(!swim)
            {
                moveDir *= speedRunSpeedWithSkills;
            }
            else
                moveDir *= speedSwimRun;
            staminaCount-=0.2f;
            run = true;
        }
    }

    void Walk()
    {
        if(run == false && staminaCount > 5 && moveDir!=new Vector3(0f,0f,0f))
        {
            cameraAnimator.ResetTrigger("ToRun");
            cameraAnimator.ResetTrigger("ToStay");
            cameraAnimator.SetTrigger("ToWalk");
            if (moveDir.z < 0)
                moveDir.z *= 0.6f; 

            if(!swim)
            {
                moveDir *= speedWalkSpeed;
            }
            else
                moveDir *= speedSwim;
            staminaCount-=0.03f;

            walk = true;
        }
    }

    void Stay()
    {
        if(run == false && walk == false)
        {
            cameraAnimator.ResetTrigger("ToRun");
            cameraAnimator.SetTrigger("ToStay");
            cameraAnimator.ResetTrigger("ToWalk");
        }
    }

    void Jump()
    {
        if(Input.GetKey("space"))
        {
            if(ControlleR.isGrounded && staminaCount >= 10 && angle < 30)
            {
                jumpVelocityMoment = jumpVelocity;

                staminaCount-=jumpStamina;
            }

            if(swim)
            {
                moveDir.y +=upSwimSpeed*swimFactor;
                swimFactor+=0.04f;
                if(swimFactor > 1)
                    swimFactor = 1;
            }
        }

        moveDir.y += jumpVelocityMoment;
        jumpVelocityMoment-=0.05f*gravity;
        if(jumpVelocityMoment<0)
            jumpVelocityMoment = 0;
        else
        Debug.Log("Jump");
    }


    void Gravity()
    {
        if(!swim)
        {
            moveDir.y -= gravity;  
            swimFactor = 0.2f;
        }
        else
            moveDir.y -= gravity*0.06f;
    }

    

    public void Rebounds()
    {
        staminaRebound = Skills.StaminaRebound;
        if(Input.GetKey("s") && ControlleR.isGrounded && staminaCount > staminaRebound && reb1trigger == true)
            {
                reb1trigger = false;
                if(reb1)
                {
                    moveDir = new Vector3(0,6,-6);
                    staminaCount-=staminaRebound;
                    moveDir = transform.TransformDirection(moveDir);
                }
                else
                    StartCoroutine(rebound(1));
            }
            if(!Input.GetKey("s"))
                reb1trigger = true;

            if(Input.GetKey("a") && ControlleR.isGrounded && staminaCount > staminaRebound && reb2trigger == true)
            {
                reb2trigger = false;
                if(reb2)
                {
                    moveDir = new Vector3(-7,6,0);
                    staminaCount-=staminaRebound;
                    moveDir = transform.TransformDirection(moveDir);
                }
                else
                    StartCoroutine(rebound(2));
            }
            if(!Input.GetKey("a"))
                reb2trigger = true;

            if(Input.GetKey("d") && ControlleR.isGrounded && staminaCount > staminaRebound && reb3trigger == true)
            {
                reb3trigger = false;
                if(reb3)
                {
                    moveDir = new Vector3(7,6,0);
                    staminaCount-=staminaRebound;
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
