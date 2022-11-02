using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class SimpleAI : MonoBehaviour
{
    [Space]
    [Header("Настройки целей")]
    [HideInInspector]
    public int friendly = 0; //0- стоит 1- битва с игроком 2 - битва с ии
    public GameObject player;
    public GameObject target; 
    public GameObject hadLookTarget;
    public GameObject bodyLookTarget;
    public GameObject stopTrigger;

    public int stoppedAI; //0-не стоит 1 - стоит перед игроком

    private RigBuilder lookRig;
    private float walkSpeed = 1f;
    private float runSpeed = 4f;
    [Tooltip("1 - Хищное животное 2 - Мирное животное 3 - Бандит 4 - Злой маг 5 - Житель 6 - Стражник")]
    public int group;
    public bool playerAngry;
    public List<int> angryGroup;


    [Space]
    [Header("Настройки патрулирования")]
    public List<GameObject> patrulPoints = new List<GameObject>();
    public GameObject patrulPointsContainer;
    public bool orderPatrul = false;
    private bool stay = false;
    private int numberOfPatrulPoint = 0;

    private float x_OffsetTaget;
    private float y_OffsetTaget;


    [Range(0f, 1f)]
    public float probabilityStayOnTarget = 0;
    private bool walkToPoint = false;

    [Space]
    [Header("Настройки видимости")]

    [SerializeField] private float navDistanceToTarget;
    [SerializeField] private float straightDistanceToTarget;
    [SerializeField] private float lookDistance;
    [SerializeField] private float bowDistance;
    [SerializeField] private float swordUnequipDistance;
    [SerializeField] private float attackDistance;
    [SerializeField] private float attackStayDistance;
    private float attackDistanceFinal;
    private float attackStayDistanceFinal;

    
    private NavMeshAgent pathAgent;
    private NavMeshPath calculatingPathAgent;
    private Animator animator;
    public Vector3 targetPosition;
    private Vector3 AIPosition;
    [Space]
    [Header("Настройки оружия")]
    public GameObject staySword;
    public GameObject attackSword;
    public GameObject stayShield;
    public GameObject attackShield;
    public GameObject stayBow;
    public GameObject attackBow;
    private PlayerStats playerStats;
    private EntityAttack weaponScript;
    private EntityStats statsScript;

    public float attackSpeed;
    public float damage;
    private bool weaponOn = false;
    private bool shieldOn = false;
    private bool bowOn = false;
    private EntityStats entityStatsScript;
    private bool isAttack = false;

    public bool animationStarted = false;
    private bool animationPatrol = false;
    private bool attackTarget = false;
    private bool waitAttack = false;
    private bool starting = false;
    private bool runToTarget = true;
    private bool speedFlag = false;
    private bool speedPause = false;
    private bool coerceBow = false;
    private Vector3 hadTargetStartPosition;
    private Vector3 bodyTargetStartPosition;

    public int random = 0;
    
    TaegetAttack TA;

    void Start()
    {
        calculatingPathAgent = new NavMeshPath();
        hadTargetStartPosition = hadLookTarget.transform.localPosition;
        bodyTargetStartPosition = bodyLookTarget.transform.localPosition;

        lookRig = gameObject.GetComponent<RigBuilder>();
        lookRig.enabled  = false;
        playerStats = player.GetComponent<PlayerStats>();
        x_OffsetTaget = Random.Range(-2f,2f);
        y_OffsetTaget = Random.Range(-2f,2f);

        
        
        statsScript = gameObject.GetComponent<EntityStats>();
        weaponScript = attackSword.GetComponent<EntityAttack>();

        lookDistance*=Random.Range(1f,1.2f);
        walkSpeed*=Random.Range(0.95f,1.05f);
        runSpeed*=Random.Range(0.95f,1.05f);
        weaponScript.damage = damage;
        statsScript.AngryGroup = angryGroup;
        statsScript.Group = group;
        weaponScript.StatsScript = statsScript;
        weaponScript.Player_Stats = playerStats;

        SetAttackSpeedBase(attackSpeed);
        if(patrulPointsContainer)
            for(int i = 0; i < patrulPointsContainer.transform.childCount;i++)
                patrulPoints.Add(patrulPointsContainer.transform.GetChild(i).gameObject);

        if(swordUnequipDistance >= lookDistance || swordUnequipDistance <= attackDistance)
        Debug.Log("***ОШИБКА*** SwordUnequipDistance больше LookDistance или меньше AttackDistance");
        if(swordUnequipDistance >=lookDistance)
        Debug.Log("***ОШИБКА*** SwordUnequipDistance может юыть больше LookDistance");

        entityStatsScript= gameObject.GetComponent<EntityStats>();
        pathAgent = gameObject.GetComponent<NavMeshAgent>();
        animator = gameObject.GetComponent<Animator>();
        
        if(staySword != null)
        staySword.SetActive (true);
        if(attackSword != null)
        attackSword.SetActive (false);

        if(stayShield != null)
        stayShield.SetActive (true);
        if(attackShield != null)
        attackShield.SetActive (false);

        if(stayBow != null)
        stayBow.SetActive (true);
        if(attackBow != null)
        attackBow.SetActive (false);

        StartCoroutine(IdleChange());
        StartCoroutine(StartaI());
        StartCoroutine(TargetSearchCycle());
    }

    IEnumerator StartaI ()
    {
        yield return new WaitForSeconds(Random.Range(0f,2f));
        target = null;
        starting = true;
    }

    IEnumerator TargetSearchCycle ()
    {
        while(1<2)
        {   
            if(target == null)
            {
                GameObject targ = TargetSearch();
                if(targ!=null)
                {
                    target = targ;
                }
            }   
            yield return new WaitForSeconds(2);
            random = Random.Range(1,100);
        }

    }

    IEnumerator IdleChange ()
    {
        while(1<2)
        {
            animator.SetInteger("Idle", Random.Range(1,5));
            yield return new WaitForSeconds(2);
        }

    }

    void Update()
    {
        if(starting == true)
        {
            if(friendly!=0)
                NonFriendly();
                
            if(friendly==0)
                Patrol();
        }
    }

    public void Patrol()
    {
        if(animationPatrol == true)
            return;
        animationStarted = false; isAttack = false;     //Обнуление атаки
        if(shieldOn == true || weaponOn == true || bowOn == true)
        {
            StartCoroutine(UnequipAllPatrol(true));
            return;
        }
        if(stay)
            return;

        if(stoppedAI == 1)
        {
            hadLookTarget.transform.position = Vector3.MoveTowards(hadLookTarget.transform.position, player.transform.GetChild(0).position, Time.deltaTime * 1f);
            bodyLookTarget.transform.position = Vector3.MoveTowards(bodyLookTarget.transform.position, player.transform.position+ new Vector3(0,0.3f,0), Time.deltaTime * 0.5f);

            animator.SetBool("ToWalk", false);
            animator.SetBool("ToIdle", true);
            pathAgent.speed = 0;
            lookRig.enabled  = true;
            return;
        }
        else
        {
            StartCoroutine(LookDisableSmooth());
        }

        animator.SetBool("ToIdle", false);
        animator.SetTrigger("IdleExit");

        if(animator.GetCurrentAnimatorStateInfo(1).IsName("Armature|Idle"))
            animator.SetBool("ToWalk",true);
        else
            animator.SetBool("ToWalk",false);
    
        StartCoroutine(SetSpeed(walkSpeed,0.2f));

        if(orderPatrul == true && walkToPoint == false)
        {
            if(numberOfPatrulPoint>=patrulPoints.Count)
                numberOfPatrulPoint = 0;
            SetPathDestination(PutrulTargetCalculate(patrulPoints[numberOfPatrulPoint].transform.position));
            walkToPoint = true;
        }

        if(orderPatrul == false && walkToPoint == false)
        {
            SetPathDestination(PutrulTargetCalculate(patrulPoints[Random.Range(0,patrulPoints.Count)].transform.position));
            walkToPoint = true;
        }
        if(pathAgent.remainingDistance <= 2 && walkToPoint == true)
        {
            if((Random.Range(0f,1f) <= probabilityStayOnTarget))
            {
                StartCoroutine(StayOnTarget());
                return;
            }
            if(stay == false)
            {
                if(orderPatrul)
                    numberOfPatrulPoint++;
                walkToPoint= false;
            }
        }
    }
    public Vector3 PutrulTargetCalculate(Vector3 startTarget)
    {
        return(new Vector3(startTarget.x+x_OffsetTaget, startTarget.y+y_OffsetTaget, startTarget.z));
    }
    IEnumerator StayOnTarget()
    {
        stay = true;
        StartCoroutine(SetSpeed(0f,0.35f));
        animator.SetBool("ToWalk", false);
        animator.SetBool("ToIdle", true);
        SetPathDestination(transform.position);
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("ToIdle", false);

        yield return new WaitForSeconds(3);

        if(orderPatrul)
            numberOfPatrulPoint++;
        walkToPoint = false;

        stay = false;
    }


    public void NonFriendly()
    {
        animationPatrol = false; walkToPoint = false; stay = false;      //Обнуление патрулирования
        if(target == null)
        {
            return;
        }

        if(friendly == 0)
            return;
        
        if(friendly == 1)
        {
            if(target == player)
            {
                if(playerAngry == false)
                {
                    TargetDiable();
                    return;
                }
                hadLookTarget.transform.position = Vector3.MoveTowards(hadLookTarget.transform.position, target.transform.GetChild(0).position, Time.deltaTime * 10);
                bodyLookTarget.transform.position = Vector3.MoveTowards(bodyLookTarget.transform.position, target.transform.position, Time.deltaTime * 10);
            }
                
            attackDistanceFinal = 2.2f;
            attackStayDistanceFinal = 2.4f;
            target = player.gameObject;
            targetPosition = player.transform.position;
        }

        if(friendly == 2)
        {
            attackDistanceFinal = attackDistance;
            attackStayDistanceFinal = attackStayDistance;
            if(target.GetComponent<TaegetAttack>())
                targetPosition = target.transform.position;


            hadLookTarget.transform.position = Vector3.MoveTowards(hadLookTarget.transform.position, target.transform.position+ new Vector3(0,1,0), Time.deltaTime * 10);
            bodyLookTarget.transform.position = Vector3.MoveTowards(bodyLookTarget.transform.position, target.transform.position, Time.deltaTime * 10);

            lookRig.enabled  = true;
        }



        if(target.GetComponent<EntityStats>())
            {
                if(target.GetComponent<EntityStats>().die == true)
                {
                    GameObject targ = TargetSearch();
                    if(targ!=null)
                    {
                        target = targ;
                    }
                    else
                    {
                        friendly=0;
                        ResetAnimations();
                        return;
                    }
                }
            }

        TA = target.GetComponent<TaegetAttack>();
        AIPosition = transform.position;
        navDistanceToTarget = pathAgent.remainingDistance;
        straightDistanceToTarget = Vector3.Distance (AIPosition , targetPosition);



        if(!(animator.GetCurrentAnimatorStateInfo(1).IsName("Armature|Run") || animator.GetCurrentAnimatorStateInfo(1).IsName("Armature|Walk")))
        {
            if(speedPause == false)
            pathAgent.speed = 0f;
        }

        if(animator.GetCurrentAnimatorStateInfo(1).IsName("Armature|Run"))
        {
            pathAgent.speed = runSpeed;
        }

        if(animator.GetCurrentAnimatorStateInfo(1).IsName("Armature|Walk"))
        {
            pathAgent.speed = walkSpeed;
        }

        if(animationStarted == true)
            return;

        bool targetWatch = false;
        RaycastHit lookArea;
        Vector3 targetVector = target.transform.position - transform.position;
        Ray ray = new Ray(transform.position+new Vector3(0,2,0) + Vector3.Normalize(targetVector)/2, target.transform.position - transform.position- new Vector3(0,2,0));
        Physics.Raycast(ray, out lookArea);

        if (lookArea.collider != null)
        {
            if (lookArea.collider.gameObject == target)
            {
                targetWatch = true;
            }   
        }

        if(straightDistanceToTarget > lookDistance)
        {
            TargetDiable();
            return;
        }

        if ((straightDistanceToTarget < bowDistance && navDistanceToTarget > swordUnequipDistance && targetWatch == true) || coerceBow == true)
        {
            Debug.Log("123");
            animator.SetTrigger("FootStay");
            return;
        }

        if(straightDistanceToTarget <= lookDistance && navDistanceToTarget > swordUnequipDistance && animationStarted == false)
        {
            waitAttack = false;
            if(attackTarget == true)
            {
                attackTarget = false;
                TA.AttackCount--;
            }

            animator.SetBool("ToWalk",false);
            animator.SetBool("ToIdle", false);
            animator.SetTrigger("FootRun");
            animator.SetTrigger("HandsRun");
            SetPathDestination(targetPosition);
        }

        if(navDistanceToTarget <= swordUnequipDistance && navDistanceToTarget > swordUnequipDistance && animationStarted == false )
        {
            animator.SetBool("ToIdle", false);
            if(stayBow!=null)
            {
                animator.SetTrigger("IdleExit");
                transform.LookAt(new Vector3(target.transform.position.x, transform.position.y ,target.transform.position.z));
                animator.SetInteger("AttackSelect", 0);
                animator.ResetTrigger("SwordsRun");
                animator.SetTrigger("FootStay");
                StartCoroutine(Fire());
                return;
            }
            else
            {   
                waitAttack = false;
                if(attackTarget == true)
                {
                    attackTarget = false;
                    TA.AttackCount--;
                }

                animator.SetBool("ToWalk",false);
                animator.SetTrigger("FootRun");
                animator.SetTrigger("HandsRun");
                SetPathDestination(targetPosition);
            }
        }

        if(navDistanceToTarget <= swordUnequipDistance && navDistanceToTarget >= attackStayDistanceFinal && animationStarted == false)
        {
            UnequipEquipFull();
            animator.ResetTrigger("Attack");

            if(random>80)
            {
                animator.SetInteger("AttackSelect", 0);
                animator.ResetTrigger("SwordsRun");
                animator.SetTrigger("ToAttack");
                animator.SetTrigger("ToIdleSword");
                return;
            }
            else
            {
                animator.ResetTrigger("ToAttack");
                animator.ResetTrigger("ToIdleSword");
            }
            RunToTargetAndEquip(false, true);
            runToTarget = true;
        }

        if(navDistanceToTarget < attackDistanceFinal*2.5f && navDistanceToTarget > attackStayDistanceFinal*1.7f && attackTarget == false && animationStarted == false)
        {
            lookRig.enabled  = true;

            if(TA.AttackCount>=3)
            {
                SetPathDestination(targetPosition);  
                animator.ResetTrigger("FootRun");
                animator.ResetTrigger("HandsRun");
                animator.ResetTrigger("SwordsRun");
                animator.SetTrigger("ToIdleSword");
                waitAttack = true;
            }
            else
            {
                TA.AttackCount++;
                animator.SetBool("ToWalk",false);
                animator.SetBool("ToIdle", false);
                animator.SetTrigger("SwordsRun");
                animator.SetTrigger("FootRun");
                animator.ResetTrigger("ToIdleSword");
                attackTarget = true;
                waitAttack = false;
            }
        }    
            SetPathDestination(targetPosition);

        if(navDistanceToTarget < attackDistanceFinal*2f-0.2f && navDistanceToTarget>attackDistanceFinal*2f-0.5f)
        {
            if(random < 1000)
            {
                Attack();
            }
        }

        if(navDistanceToTarget < attackStayDistanceFinal && navDistanceToTarget>attackDistanceFinal)
        {
            random = 10;
            if(runToTarget == true)
            {
                RunToTargetAndEquip(true, false);
            }
            else
            Attack();
        }    

        if(navDistanceToTarget < attackStayDistanceFinal)
        {
            animator.SetTrigger("ToAttack");
            random = 10;
            runToTarget = false;
            Attack();
        }     
    }

    private void SetPathDestination(Vector3 targetPosition)
    {
        pathAgent.SetDestination(targetPosition);
        pathAgent.CalculatePath(targetPosition, calculatingPathAgent);

        if(calculatingPathAgent.status != NavMeshPathStatus.PathComplete || pathAgent.remainingDistance <= 0.5)
        {
            coerceBow = true;
            animator.SetTrigger("ToIdleSword");
            pathAgent.SetDestination(gameObject.transform.position);

        }
        else
            coerceBow = false;
    }

    private void Attack()
    {
        if(weaponOn == false)
            StartCoroutine(weaponOnOff());

        animator.ResetTrigger("SwordsRun");
        animator.ResetTrigger("ToIdleSword");
        
        if(isAttack == false)
            StartCoroutine(AttackTime());
    }

    private void RunToTargetAndEquip(bool slow, bool equip)
    {
        if(waitAttack == false && equip == true)
        {
            UnequipEquipFull();
            if((shieldOn == true && attackShield!=null && stayShield!=null) || (shieldOn == false && attackShield==null) && weaponOn == true && animationStarted == false)
            {
                if(isAttack == false)
                {
                    animator.SetTrigger("SwordsRun");
                    SetPathDestination(targetPosition);
                    
                }
                    
                animator.SetTrigger("FootRun");
            }
        }
            
        else
            if(TA.AttackCount<3)     
                AttackChoise(slow);
    }


    private void AttackChoise(bool slow)
    {
        TA.AttackCount++;

        if(slow ==false)
        {
            animator.ResetTrigger("ToAttack");
            animator.SetTrigger("FootRun");
        }
        else
        {
            animator.ResetTrigger("ToAttack");
            animator.SetTrigger("FootWalk");
        }
            
        animator.ResetTrigger("ToIdleSword");
        attackTarget = true;
        waitAttack = false;
    }

    public void TargetDiable()
    {
        friendly=0;
        SetPathDestination(transform.position);
        target = null;
        ResetAnimations();
            
        StartCoroutine(LookDisableSmooth());
    }

    IEnumerator AttackTime()
    {
        isAttack = true;

        StartCoroutine(RotationToTagret());

        animator.SetInteger("AttackSelect", Random.Range(1,7));  //удар
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.1f);
        weaponScript.AttackStartCollider();
        yield return new WaitForSeconds(0.3f);
        animator.SetInteger("AttackSelect", 0);
        yield return new WaitForSeconds(0.2f);
        weaponScript.AttackFinishCollider();

        float attackTime = Random.Range(0.75f,1.25f)*attackSpeed; // ожидание между атаками и доворот в ожидании
        for(int i = 0; i < 5;i++)
        {
            StartCoroutine(RotationToTagret());
            yield return new WaitForSeconds(attackTime/5);
        }
        

        isAttack = false;
    }

    IEnumerator LookDisableSmooth()
    {
        while(bodyLookTarget.transform.localPosition != bodyTargetStartPosition)
        {
            hadLookTarget.transform.localPosition = Vector3.MoveTowards(hadLookTarget.transform.localPosition, hadTargetStartPosition, Time.deltaTime * 1f);
            bodyLookTarget.transform.localPosition = Vector3.MoveTowards(bodyLookTarget.transform.localPosition, bodyTargetStartPosition, Time.deltaTime * 0.5f);
            yield return null;
        }
        lookRig.enabled = false;
    }

    IEnumerator RotationToTagret()
    {
        if(target!=player && target!= null)
        {
            transform.LookAt(new Vector3(target.transform.position.x, transform.position.y ,target.transform.position.z));
        }

        Vector3 targetDirection;
        int step = 0;
        int limit = 480;
        while(25<Vector3.Angle(new Vector3(target.transform.position.x - transform.position.x, 0, target.transform.position.z - transform.position.z), transform.forward)) // доворот к игроку
        {
            animator.ResetTrigger("SwordRotationJumpExit");
            animator.SetTrigger("SwordRotationJump");
            if(animator.GetCurrentAnimatorStateInfo(1).IsName("Armature|SwordRotationJump"))
            {
                targetDirection = new Vector3(target.transform.position.x - transform.position.x, 0, target.transform.position.z - transform.position.z);
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, Time.deltaTime * 3, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDirection);
            }  
            yield return null;
            step++;
            if(step>limit)
                {
                    break;
                }
        }
        animator.ResetTrigger("SwordRotationJump");
        animator.SetTrigger("SwordRotationJumpExit");
    }

    

    IEnumerator BowUnEquip(bool animFalse)
    {
        animator.SetTrigger("BowOff");
        bowOn = true;
        animationStarted = true;
        yield return new WaitForSeconds(2f);
        stayBow.SetActive(false);
        attackBow.SetActive(true);
        if(animFalse)
        animationStarted = false;
    }

    IEnumerator Fire()
    {
        animationStarted = true;
        animator.SetTrigger("BowFire");
        yield return new WaitForSeconds(4f);
        animationStarted = false;
    }

    public void UnequipEquipFull()
    {
        if((weaponOn == false && attackSword!=null && staySword!=null) || (shieldOn == false && attackShield!=null && stayShield!=null))
        {

            animator.SetBool("ToWalk",false);
            animator.SetBool("ToIdle", false);
            isAttack = false;
            animator.ResetTrigger("Attack");
            animator.ResetTrigger("Attack");
            animator.SetTrigger("IdleExit");

            if(weaponOn == false && attackSword!=null && staySword!=null)
            {
                animator.SetTrigger("FootWalk");
                animator.ResetTrigger("HandsRun");

                StartCoroutine(weaponOnOff());
                return;
            }
            if(shieldOn == false && attackShield!=null && stayShield!=null)
            {
                animator.SetTrigger("FootWalk");
                StartCoroutine(shieldOnOff());
                return;
            }

            animator.ResetTrigger("HandsRun");
        }
    }

    IEnumerator BowEquip()
    {
        animator.SetTrigger("BowOn");
        bowOn = true;
        animationStarted = true;
        yield return StartCoroutine(UnequipAllPatrol(false));
        stayBow.SetActive(false);
        attackBow.SetActive(true);
        yield return new WaitForSeconds(2f);
        animationStarted = false;
    }
    
    IEnumerator UnequipAllPatrol (bool bowUnequip)
    {
        animationPatrol = true;

        if(bowOn == true && bowUnequip == true)
        {
            animator.SetTrigger("BowOff");
            bowOn = false;

            yield return new WaitForSeconds(2f);

            if(stayBow != null)
                stayBow.SetActive(false);
            if(attackBow != null)
                attackBow.SetActive(true);
        }

        if(shieldOn == true)
        {
            animator.SetTrigger("ShieldOFF");
            shieldOn = false;

            yield return new WaitForSeconds(0.5f);

            if(stayShield != null)
            stayShield.SetActive (true);
            if(attackShield != null)
            attackShield.SetActive (false);
            yield return new WaitForSeconds(0.5f);
        }

        if(weaponOn == true)
        {
            animator.SetTrigger("SwordRemove");
            weaponOn = false;

            yield return new WaitForSeconds(0.5f);

            if(staySword != null)
            staySword.SetActive (true);
            if(attackSword != null)
            attackSword.SetActive (false);
            weaponOn = false;
            yield return new WaitForSeconds(0.5f);
        }
        animator.SetTrigger("ToIdle");
        animationPatrol = false;
    }
    IEnumerator SetSpeed(float speed, float time)
    {
        if(speedFlag == true)
            yield break;
        speedFlag = true;
        time*=2;
        float SpeedNow = pathAgent.speed;
        float deltaSpeed = speed-SpeedNow;
        for(int i = 0; i < 5;i++)
        {
            pathAgent.speed +=deltaSpeed/5;
            yield return new WaitForSeconds(time/5);
        }
        speedFlag = false;

    }
    IEnumerator shieldOnOff ()
    {
        animationStarted = true;

        if(bowOn == true)
        {
            yield return StartCoroutine(BowUnEquip(false));
        }

        if(shieldOn == false)
        {
            animator.SetTrigger("ShieldON");   
            shieldOn = true;
        }
        else
        {
            animator.SetTrigger("ShieldOFF");
            shieldOn = false;

        }
        yield return new WaitForSeconds(0.5f);
        if(shieldOn == true)
        {
            if(stayShield != null)
            stayShield.SetActive (false);
            if(attackShield != null)
            attackShield.SetActive (true);
        }
        else
        {
            if(stayShield != null)
            stayShield.SetActive (true);
            if(attackShield != null)
            attackShield.SetActive (false);
        }
        yield return new WaitForSeconds(0.5f);
        animationStarted = false;
    }

    IEnumerator weaponOnOff ()
    {
        animationStarted = true;

        if(bowOn == true)
        {
            yield return StartCoroutine(BowUnEquip(false));
        }

        if(weaponOn == false)
        {
            animator.SetTrigger("SwordGet");   
            weaponOn = true;
        }
        else
        {
            animator.SetTrigger("SwordRemove");
            weaponOn = false;

        }
        
        yield return new WaitForSeconds(0.5f);

        if(weaponOn == true)
        {
            if(staySword != null)
            staySword.SetActive (false);
            if(attackSword != null)
            attackSword.SetActive (true);
            weaponOn = true;
        }
        else
        {
            if(staySword != null)
            staySword.SetActive (true);
            if(attackSword != null)
            attackSword.SetActive (false);
            weaponOn = false;
        }
        yield return new WaitForSeconds(0.4f);
        animationStarted = false;
    }

    private GameObject TargetSearch()
    {

        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, lookDistance);
            for(int i = 0; i < hitColliders.Length; i++)
            {
                if(hitColliders[i].GetComponent<PlayerStats>()&& playerAngry == true)
                {
                    friendly = 1;
                    return player;
                }
                    
                if(hitColliders[i].GetComponent<EntityStats>())
                {
                    if(hitColliders[i].GetComponent<EntityStats>().die == false)
                        if(hitColliders[i].GetComponent<SimpleAI>())
                        {
                            SimpleAI PotentialTarget = hitColliders[i].GetComponent<SimpleAI>();
                            for(int j = 0; j< angryGroup.Count;j++)
                            {
                                if(angryGroup[j] == PotentialTarget.group)
                                {
                                    friendly = 2;
                                    return PotentialTarget.gameObject;
                                }
                                    
                            }
                        }
                }
            }
        return null;
    }

    private void ResetAnimations()
    {
        foreach (var param in animator.parameters)
                    {
                        if (param.type == AnimatorControllerParameterType.Trigger)
                        {
                            animator.ResetTrigger(param.name);
                        }
                        if (param.type == AnimatorControllerParameterType.Bool)
                        {
                            animator.SetBool(param.name, false);
                        }

                    }
                    animator.Rebind();
    }

    public void die()
    {
        SetPathDestination(transform.position);
        lookRig.enabled = false;

        if(attackSword.activeSelf == true)
        {
            attackSword.GetComponent<BoxCollider>().enabled = true;
            attackSword.GetComponent<Rigidbody>().useGravity = true;
            attackSword.GetComponent<EntityAttack>().enabled = false;
        }
    }
    /*----------------ИНКАПСУЛЯЦИЯ---------------*/

    public void SetAttackSpeedBase(float x)
    {
        attackSpeed = Random.Range(0.75f,1.25f)*x;
    }
    public void SetAttackSpeed(float x)
    {
        if(x <=0)
            Debug.Log("***ОШИБКА*** Неверный индекс скорости атаки ИИ");
        else
            attackSpeed = x;
    }
}