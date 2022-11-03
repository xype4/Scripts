using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;


public class RumAIHuman : MonoBehaviour
{
    [Space]
    [Header("Настройки целей")]
    [HideInInspector] public int friendly = 0; //0- стоит 1- битва с игроком 2 - битва с ии
    public GameObject player;
    public GameObject target; 
    public GameObject hadLookTarget;
    public GameObject bodyLookTarget;
    private Vector3 hadTargetStartPosition;
    private Vector3 bodyTargetStartPosition;
    private float x_OffsetTaget;
    private float y_OffsetTaget;
    public int stoppedAI; //0-не стоит 1 - стоит перед игроком
    private RigBuilder _rigBuilder;
    private float walkSpeed = 1f;
    private float runSpeed = 4f;
    [Tooltip("1 - Хищное животное 2 - Мирное животное 3 - Бандит 4 - Злой маг 5 - Житель 6 - Стражник")]
    public int group;
    public bool playerAngry;
    public List<int> angryGroup;


    [Space]
    [Header("Настройки патрулирования")]

    public List<GameObject> patrolPoints = new List<GameObject>();
    public GameObject patrolPointsContainer;
    [Tooltip("Важен ли порядок точек при патрулировании?")]
    public bool orderPatrol = false;

    private bool stay = false; //Ожидание на точке
    private bool walkToPoint = false; //Идёт ли к точке

    public float minStayTime = 1f;
    public float maxStayTime = 5f;
    private int numberOfPatrolPoint = 0;

    [Range(0f, 1f)]
    public float probabilityStayOnTarget = 0;
    [HideInInspector] public bool stoppedFrontPlayer; //Ожидание при встрече игрока
    [HideInInspector] public bool stoppedFrontPlayerFlag = false; //Ожидание при встрече игрока флаг для однократного срабатывания пофорота головы поумолчанию

    [Space]
    [Header("Настройки видимости")]

    [SerializeField] private float navDistanceToTarget;
    [SerializeField] private float straightDistanceToTarget;
    [SerializeField] private float lookDistance;
    [SerializeField] private float swordUnequipDistance;
    [SerializeField] private float attackDistance;
    [SerializeField] private float attackStayDistance;
    [SerializeField] private float attackDistanceFinal;
    [SerializeField] private float attackStayDistanceFinal;


    private UnityEngine.AI.NavMeshAgent _navMeshAgent;
    private Animator _animator;
    [HideInInspector] public Vector3 targetPosition;
    private Vector3 AIPosition;

    [Space]
    [Header("Настройки оружия")]
    public GameObject staySword;
    public GameObject attackSword;
    public GameObject stayShield;
    public GameObject attackShield;
    private PlayerStats playerStats;
    private EntityAttack weaponScript;
    private EntityStats statsScript;

    public float attackSpeed;
    public float damage;
    private EntityStats entityStatsScript;

    private bool weaponOn = false;
    private bool shieldOn = false;
    private bool isAttack = false; //атака или ожидание

    public bool animationStarted = false; // есть анимация оружия или щита
    private bool animationPatrol = false; // есть анимация деэкипировки для патруля
    private bool attackTarget = false; //цель свободная для атаки
    private bool waitAttack = false; //ожидание цели для атаки
    private bool starting = false; //ИИ инициализирован
    private bool runToTarget = false;//подбежит к цели при удалении в битве
    private bool speedFlag = false;//скорость меняется

    public int random = 0;
    public int randomCostant = 0;

    TaegetAttack TA;


    void Start()
    {
        //------------------------------------ИНИЦИАЛИЗАЦИЯ ПАТРУЛЯ--------------------------------
        x_OffsetTaget = Random.Range(-2f,2f);
        y_OffsetTaget = Random.Range(-2f,2f);

        if(patrolPointsContainer)
            for(int i = 0; i < patrolPointsContainer.transform.childCount;i++)
                patrolPoints.Add(patrolPointsContainer.transform.GetChild(i).gameObject);

        //------------------------------------ИНИЦИАЛИЗАЦИЯ КЛАССОВ ДЛЯ ИИ-------------------------
        _rigBuilder = gameObject.GetComponent<RigBuilder>();
        _rigBuilder.enabled  = false;
        _navMeshAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        _animator = gameObject.GetComponent<Animator>();
        hadTargetStartPosition = hadLookTarget.transform.localPosition;
        bodyTargetStartPosition = bodyLookTarget.transform.localPosition;


        //------------------------------------ИНИЦИАЛИЗАЦИЯ ЭКИПИРОВКИ-----------------------------
        if(staySword != null)
        staySword.SetActive (true);
        if(attackSword != null)
        attackSword.SetActive (false);

        if(stayShield != null)
        stayShield.SetActive (true);
        if(attackShield != null)
        attackShield.SetActive (false);

        randomCostant = Random.Range(1,100);
        StartCoroutine(Cyclic());
    }

    IEnumerator Cyclic ()
    {
        int i = 0;
        while(1<2)
        {
            _animator.SetInteger("Idle", Random.Range(1,3));
            target = TargetSearch();
            i++;
            if(i == 10)
            {
                i = 0;
                random = Random.Range(1,100);
            }
            yield return new WaitForSeconds(2);
        }

    }

    void Update()
    {
        if(Input.GetKeyDown("y")&& animationStarted == false)
            StartCoroutine(ShieldEquipUneguip(true));
        if(Input.GetKeyDown("u")&& animationStarted == false)
            StartCoroutine(SwordEquipUneguip(true));

        if(Input.GetKeyDown("h")&& animationStarted == false)
            StartCoroutine(AllEquip());
        if(Input.GetKeyDown("j")&& animationStarted == false)
            StartCoroutine(AllUnequip());


        
        SpeedThrougtAnimations();

        switch (friendly)
        {
            case 0:
                Patrol();   
            return;

            case 1:
                FriendlyOne();
                Attack();
            return;

            case 2:
                FriendlyTwo();
                Attack();
            return;

            default:
            return;
        }
    }
    




    /*----------------------------------------------------------------------------------------------------------------------------------------------------------------------
    ------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    
        ********                                                            МЕТОД ПАТРУЛЯ
                                                        
    ------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    ----------------------------------------------------------------------------------------------------------------------------------------------------------------------*/





    void Patrol()
    {
        //Поворот головы к игроку при приблежении
        if(stoppedFrontPlayer == true)  
        {
            stoppedFrontPlayerFlag = true;
            LookOnPlayer(hadLookTarget, bodyLookTarget, player, _rigBuilder, 1f);
            SetStayAnimations();
            return;
        }
        else
        {
            if(stoppedFrontPlayerFlag == true)
            {
                StartCoroutine(LookDisableSmoothIE());
                stoppedFrontPlayerFlag = false;
            }
        }


        //Движение к точке
        if(stay == true)
            return;

        SetWalkAnimations();
        
        if(orderPatrol == true && walkToPoint == false)
        {
            if(numberOfPatrolPoint>=patrolPoints.Count)
                numberOfPatrolPoint = 0;
            _navMeshAgent.SetDestination(PutrolTargetCalculate(patrolPoints[numberOfPatrolPoint].transform.position, x_OffsetTaget, y_OffsetTaget));
            walkToPoint = true;
        }

        if(orderPatrol == false && walkToPoint == false)
        {
            _navMeshAgent.SetDestination(PutrolTargetCalculate(patrolPoints[Random.Range(0,patrolPoints.Count)].transform.position, x_OffsetTaget ,y_OffsetTaget));
            walkToPoint = true;
        }
        
        //Достижение точки
        if(GetPathRemainingDistance(_navMeshAgent) <= 2 && walkToPoint == true)
        {
            if((Random.Range(0f,1f) <= probabilityStayOnTarget))
            {
                SetStayAnimations();
                StartCoroutine(StayOnTarget());
                return;
            }

            if(stay == false)
            {
                if(orderPatrol)
                    numberOfPatrolPoint++;
                walkToPoint= false;
            }
        }

    }





    /*----------------------------------------------------------------------------------------------------------------------------------------------------------------------
    ------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    
        ********    ********                                                  МЕТОД АТАКИ
                                                        
    ------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    ----------------------------------------------------------------------------------------------------------------------------------------------------------------------*/





    void Attack()
    {
    
        //Обнуление патрулирования и первичное обнаружение цели



        animationPatrol = false; walkToPoint = false; stay = false;      
        if(target == null)
        {
            return;
        }



        //Финальное обнаружение целей и расстояний



        _navMeshAgent.SetDestination(targetPosition);
        TA = target.GetComponent<TaegetAttack>();
        AIPosition = transform.position;
        navDistanceToTarget = GetPathRemainingDistance(_navMeshAgent);
        straightDistanceToTarget = Vector3.Distance (AIPosition , targetPosition);

        if(target.GetComponent<EntityStats>())
        {
            TargetDieCheck();            
        }

        bool directVisible = DirectVisibleCheck(target, gameObject.transform.position);



        //Обработка по расстоянию до цели



        //Цель очень далека
        if(straightDistanceToTarget > lookDistance)
        {
            TargetDisable();
            return;
        }

        //Цель недоступна, но видна
        if(navDistanceToTarget== -1)
        {
            TargetNoAccess();
        }

        //Цель в пределах видимости и до доставания оружия и есть прямой зрительный контакт
        if(straightDistanceToTarget <= lookDistance && navDistanceToTarget > swordUnequipDistance && directVisible==true)
        {
            TargetFar();
        }

        //Цель достаточно близко, чтобы достать оружие, но не достаточно близко, чтобы бить
        if(navDistanceToTarget <= swordUnequipDistance && navDistanceToTarget >= attackStayDistanceFinal)
        {
            TargetMiddle();
        }

        //Цель ближе, можно перевести дух
        if(navDistanceToTarget < attackDistanceFinal*2.5f && navDistanceToTarget > attackStayDistanceFinal*1.7f && attackTarget == false)
        {
            TargetVeryMiddle();
        }

        //Цель далеко, но ударить можно на бегу
        if(navDistanceToTarget < attackDistanceFinal*2f-0.2f && navDistanceToTarget>attackDistanceFinal*2f-0.5f)
        {
            TargetNear();
        }

        //Цель близко, лучше ещё подбежать, но если убежит цель, догонять не буду
        if(navDistanceToTarget < attackStayDistanceFinal && navDistanceToTarget>attackDistanceFinal)
        {
            TargetVeryNear();
        }   

        //Ближе нельзя
        if(navDistanceToTarget < attackStayDistanceFinal)
        {
            TargetHere();
        }
    }



    /*----------------------------------------------------------------------------------------------------------------------------------------------------------------------
    ------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        ********    ********    ********                    МЕТОДЫ ПОВЕДЕНИЯ В ЗАВИСИМОСТИ ОТ РАССТОЯНИЯ
                                                        
    ------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    ----------------------------------------------------------------------------------------------------------------------------------------------------------------------*/



    private void TargetNoAccess()
    {
        _navMeshAgent.SetDestination(targetPosition);
        
    }

    private void TargetFar()
    {
        _navMeshAgent.SetDestination(targetPosition);

        SetRunFootAnimations();

        if(animationStarted == false)   //Анимация рук при отсутствии оных
        {
            if(randomCostant > 50 && weaponOn == true)
                StartCoroutine(SwordEquip());
            else
                SetRunHandsAnimations();
        }
    }

    private void TargetMiddle()
    {
        _navMeshAgent.SetDestination(targetPosition);

        SetRunFootAnimations();

        if(animationStarted == false)   //Анимация рук при отсутствии оных
        {
            if(weaponOn == true && shieldOn == true)
                SetRunHandsAnimations();
            else
                StartCoroutine(AllEquip());
        }
    }
    
    private void TargetVeryMiddle()
    {
        _navMeshAgent.SetDestination(targetPosition);

        SetRunFootAnimations();

        if(animationStarted == false)   //Анимация рук при отсутствии оных
        {
            SetRunHandsAnimations();
        }
    }

    private void TargetNear()
    {
        _navMeshAgent.SetDestination(targetPosition);

        runToTarget = true;
        SetRunFootAnimations();
        SetRunHandsAnimations();
    }

    private void TargetVeryNear()
    {
        _navMeshAgent.SetDestination(targetPosition);

        if(runToTarget == true)
        {
            SetRunFootAnimations();
            SetRunHandsAnimations();
            //RunToTargetAndEquip(true, false);
        }
    }

    private void TargetHere()
    {
        _navMeshAgent.SetDestination(targetPosition);

        SetAttackReadyFootAnimations();

        if(weaponOn == true)
            SetAttackReadyHandsAnimations();
        else
            SetStayHandsAnimations();

        StartCoroutine(RotationToTagret());

        runToTarget = false;
    }




    /*----------------------------------------------------------------------------------------------------------------------------------------------------------------------
    ------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        ****                                                               МЕТОДЫ ПОДДЕРЖКИ
                                                        
    ------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    ----------------------------------------------------------------------------------------------------------------------------------------------------------------------*/




    IEnumerator LookDisableSmoothIE()
    {
        while(bodyLookTarget.transform.localPosition != bodyTargetStartPosition)
        {
            hadLookTarget.transform.localPosition = Vector3.MoveTowards(hadLookTarget.transform.localPosition, hadTargetStartPosition, Time.deltaTime * 1f);
            bodyLookTarget.transform.localPosition = Vector3.MoveTowards(bodyLookTarget.transform.localPosition, bodyTargetStartPosition, Time.deltaTime * 0.5f);
            yield return null;
        }
        _rigBuilder.enabled = false;
    }
    

    public static float GetPathRemainingDistance(UnityEngine.AI.NavMeshAgent navMeshAgent)
    {
        if (navMeshAgent.pathPending ||
            navMeshAgent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid ||
            navMeshAgent.path.corners.Length == 0)
            return -1f;

        float distance = 0.0f;
        for (int i = 0; i < navMeshAgent.path.corners.Length - 1; ++i)
        {
            distance += Vector3.Distance(navMeshAgent.path.corners[i], navMeshAgent.path.corners[i + 1]);
        }

        return distance;
    }

    public static void LookOnPlayer(GameObject hadLookTarget, GameObject bodyLookTarget, GameObject player, RigBuilder _rigBuilder, float speed)
    {
        hadLookTarget.transform.position = Vector3.MoveTowards(hadLookTarget.transform.position, player.transform.GetChild(0).position, Time.deltaTime * speed);
        bodyLookTarget.transform.position = Vector3.MoveTowards(bodyLookTarget.transform.position, player.transform.position+ new Vector3(0,0.3f,0), Time.deltaTime * speed);

        _rigBuilder.enabled  = true;
    }

    
    IEnumerator SwordEquip()
    {
        animationStarted = true;

        if(weaponOn == false)
            yield return StartCoroutine(SwordEquipUneguip(false));

        animationStarted = false;
    }

    IEnumerator AllEquip()
    {
        animationStarted = true;

        if(shieldOn == false)
            yield return StartCoroutine(ShieldEquipUneguip(false));

        if(weaponOn == false)
            yield return StartCoroutine(SwordEquipUneguip(false));
        
        yield return null;
        animationStarted = false;    
    }

    IEnumerator AllUnequip()
    {
        animationStarted = true;

        if(shieldOn == true)
            yield return StartCoroutine(ShieldEquipUneguip(false));

        if(weaponOn == true)
            yield return StartCoroutine(SwordEquipUneguip(false));
        
        yield return null;
        animationStarted = false;    
    }

    IEnumerator ShieldEquipUneguip (bool selfAnimationBlock) //параметр - использовать свою блокировку анимаций или нет (AllEquip, как пример)
    {
        if(selfAnimationBlock == true)
            animationStarted = true;

        yield return new WaitForSeconds(0.2f);

        if(shieldOn == false)
        {
            _animator.SetTrigger("ShieldEquip");   
            shieldOn = true;
        }
           

        else
        {
            _animator.SetTrigger("ShieldUnequip");
            shieldOn = false;
        }


        yield return new WaitForSeconds(0.5f);

        if(shieldOn == true)
        {
            if(stayShield != null)
                stayShield.SetActive (false);
            if(attackShield != null)
                attackShield.SetActive (true);
            shieldOn = true;
        }
        else
        {
            if(stayShield != null)
                stayShield.SetActive (true);
            if(attackShield != null)
                attackShield.SetActive (false);
            shieldOn = false;
        }
        yield return new WaitForSeconds(0.5f);

        if(selfAnimationBlock == true)
            animationStarted = false;
    }

    IEnumerator SwordEquipUneguip (bool selfAnimationBlock) //параметр - использовать свою блокировку анимаций или нет (AllEquip, как пример)
    {
        if(selfAnimationBlock == true)
            animationStarted = true;

        yield return new WaitForSeconds(0.2f);

        if(weaponOn == false)
        {
            _animator.SetTrigger("SwordEquip");
            weaponOn = true;
        }   
 
        else
        {
            _animator.SetTrigger("SwordUnequip");
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

        if(selfAnimationBlock == true)
            animationStarted = false;
    }



    /*----------------------------------------------------------------------------------------------------------------------------------------------------------------------
    ------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        ****    ****                                                   МЕТОДЫ АТАКИ
                                                        
    ------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    ----------------------------------------------------------------------------------------------------------------------------------------------------------------------*/




    public void TargetDisable()
    {
        friendly=0;
        _navMeshAgent.SetDestination(transform.position);
        target = null;
        ResetAnimations();
        StartCoroutine(LookDisableSmoothIE());
    }


    void FriendlyOne()
    {
        if(playerAngry == false)
        {
            TargetDisable();
            return;
        }

        LookOnPlayer(hadLookTarget, bodyLookTarget, player, _rigBuilder, 50f);
        attackDistanceFinal = 2.2f;
        attackStayDistanceFinal = 2.4f;
        target = player.gameObject;
        targetPosition = player.transform.position;
    }


    void FriendlyTwo()
    {
        attackDistanceFinal = attackDistance;
        attackStayDistanceFinal = attackStayDistance;

        if(target.GetComponent<TaegetAttack>())
            targetPosition = target.transform.position;

        LookOnOtherTarget(hadLookTarget, bodyLookTarget, target, _rigBuilder);
    }

    public static void LookOnOtherTarget(GameObject hadLookTarget, GameObject bodyLookTarget, GameObject target, RigBuilder _rigBuilder)
    {
        hadLookTarget.transform.position = Vector3.MoveTowards(hadLookTarget.transform.position, target.transform.position+ new Vector3(0,1,0), Time.deltaTime * 50);
        bodyLookTarget.transform.position = Vector3.MoveTowards(bodyLookTarget.transform.position, target.transform.position, Time.deltaTime * 50);

        _rigBuilder.enabled  = true;
    }

    public void TargetDieCheck()
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

    private static bool DirectVisibleCheck(GameObject targetVis, Vector3 self)
    {
        bool targetWatch = false;
        RaycastHit lookArea;
        Vector3 targetVector = targetVis.transform.position - self;
        Ray ray = new Ray(self+new Vector3(0,2,0) + Vector3.Normalize(targetVector)/2, targetVis.transform.position - self- new Vector3(0,2,0));
        Physics.Raycast(ray, out lookArea);

        if (lookArea.collider != null)
        {
            if (lookArea.collider.gameObject == targetVis)
            {
                return true;
            } 
            else
                return false;
        }

        else
            return false;
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
                    SimpleAI potentialTarget = hitColliders[i].GetComponent<SimpleAI>();
                    for(int j = 0; j< angryGroup.Count;j++)
                    {
                        if(angryGroup[j] == potentialTarget.group)
                        {
                            friendly = 2;
                            return potentialTarget.gameObject;
                        }
                    }
                }
            }
        }
        return null;
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
            _animator.ResetTrigger("SwordRotationJumpExit");
            _animator.SetTrigger("SwordRotationJump");
            if(_animator.GetCurrentAnimatorStateInfo(1).IsName("Armature|SwordRotationJump"))
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
        _animator.ResetTrigger("SwordRotationJump");
        _animator.SetTrigger("SwordRotationJumpExit");
    }



    /*----------------------------------------------------------------------------------------------------------------------------------------------------------------------
    ------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        ****    ****   ****                                          МЕТОДЫ ПАТРУЛИРОВАНИЯ
                                                        
    ------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    ----------------------------------------------------------------------------------------------------------------------------------------------------------------------*/







    public static Vector3 PutrolTargetCalculate(Vector3 startTarget, float xOffsetTaget, float yOffsetTaget)
    {
        return(new Vector3(startTarget.x+xOffsetTaget, startTarget.y+yOffsetTaget, startTarget.z));
    }

    

    IEnumerator StayOnTarget()
    {
        stay = true;
        _navMeshAgent.SetDestination(transform.position);

        yield return new WaitForSeconds(Random.Range(minStayTime,maxStayTime));

        if(orderPatrol)
            numberOfPatrolPoint++;
        walkToPoint = false;

        stay = false;
    }


    /*----------------------------------------------------------------------------------------------------------------------------------------------------------------------
    ------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        ****    ****   ****    ****                                          МЕТОДЫ АНИМАЦИЙ
                                                        
    ------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    ----------------------------------------------------------------------------------------------------------------------------------------------------------------------*/



    void SpeedThrougtAnimations()
    {
        if(!(_animator.GetCurrentAnimatorStateInfo(1).IsName("Armature|Run") || _animator.GetCurrentAnimatorStateInfo(1).IsName("Armature|Walk")))
        {
            _navMeshAgent.speed = 0f;
        }

        if(_animator.GetCurrentAnimatorStateInfo(1).IsName("Armature|Run"))
        {
            _navMeshAgent.speed = runSpeed;
        }

        if(_animator.GetCurrentAnimatorStateInfo(1).IsName("Armature|Walk"))
        {
            _navMeshAgent.speed = walkSpeed;
        }
    }

    private void ResetAnimations()
    {
        foreach (var param in _animator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Trigger)
            {
                _animator.ResetTrigger(param.name);
                }
                    if (param.type == AnimatorControllerParameterType.Bool)
                    {
                        _animator.SetBool(param.name, false);
                    }
                }
        _animator.Rebind();
    }

    //Анимации всего тела
    void SetStayAnimations()
    {
        _animator.SetTrigger("ToIdleFoots");
        _animator.ResetTrigger("ToRunFoots");
        _animator.ResetTrigger("ToWalkFoots");

        _animator.ResetTrigger("ToWalkHands");
        _animator.ResetTrigger("ToRunHands");
        _animator.SetTrigger("ToIdleHands");
    }

    void SetWalkAnimations()
    {
        _animator.ResetTrigger("ToIdleFoots");
        _animator.ResetTrigger("ToRunFoots");
        _animator.SetTrigger("ToWalkFoots");

        _animator.SetTrigger("ToWalkHands");
        _animator.ResetTrigger("ToRunHands");
        _animator.ResetTrigger("ToIdleHands");

        _animator.SetTrigger("ExitIdleHands");
    }

    void SetRunAnimations()
    {
        _animator.ResetTrigger("ToIdleFoots");
        _animator.SetTrigger("ToRunFoots");
        _animator.ResetTrigger("ToWalkFoots");

        _animator.ResetTrigger("ToWalkHands");
        _animator.SetTrigger("ToRunHands");
        _animator.ResetTrigger("ToIdleHands");
    }


    //Анимации рук
    void SetStayHandsAnimations()
    {
        _animator.ResetTrigger("ToWalkHands");
        _animator.ResetTrigger("ToRunHands");
        _animator.SetTrigger("ToIdleHands");
        _animator.ResetTrigger("ReadyAttackHands");
    }

    void SetWalkHandsAnimations()
    {
        _animator.SetTrigger("ToWalkHands");
        _animator.ResetTrigger("ToRunHands");
        _animator.ResetTrigger("ToIdleHands");
        _animator.ResetTrigger("ReadyAttackHands");

        _animator.SetTrigger("ExitIdleHands");
    }

    void SetRunHandsAnimations()
    {
        _animator.ResetTrigger("ToWalkHands");
        _animator.SetTrigger("ToRunHands");
        _animator.ResetTrigger("ToIdleHands");
        _animator.ResetTrigger("ReadyAttackHands");
    }

    void SetAttackReadyHandsAnimations()
    {
        _animator.ResetTrigger("ToWalkHands");
        _animator.ResetTrigger("ToRunHands");
        _animator.ResetTrigger("ToIdleHands");
        _animator.SetTrigger("ReadyAttackHands");
    }


    //Анимации ног
    void SetRunFootAnimations()
    {
        _animator.ResetTrigger("ToIdleFoots");
        _animator.ResetTrigger("ToWalkHands");
        _animator.SetTrigger("ToRunFoots");
        _animator.ResetTrigger("AttackFoot");
    }

    void SetIdleFootAnimations()
    {
        _animator.SetTrigger("ToIdleFoots");
        _animator.ResetTrigger("ToWalkHands");
        _animator.ResetTrigger("ToRunFoots");
        _animator.ResetTrigger("AttackFoot");
    }

    void SetAttackReadyFootAnimations()
    {
        _animator.ResetTrigger("ToIdleFoots");
        _animator.ResetTrigger("ToWalkHands");
        _animator.ResetTrigger("ToRunFoots");
        _animator.SetTrigger("AttackFoot");
    }
}