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
    [HideInInspector] public int stoppedAI; //0-не стоит 1 - стоит перед игроком
    [Tooltip("Вероятность ожидания противника в бою, ИИ сам не подходит близко")]
    [Range(0, 100)]
    public int waitTargetChance = 20;
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
    
    [Range(0f, 600f)]
    public float minStayTime = 1f;
    [Range(0f, 600f)]
    public float maxStayTime = 5f;
    private int numberOfPatrolPoint = 0;
    [Tooltip("Шанс того, что ИИ остановится на точке")]
    [Range(0f, 1f)]
    public float probabilityStayOnTarget = 0;
    [HideInInspector] public bool stoppedFrontPlayer; //Ожидание при встрече игрока
    [HideInInspector] public bool stoppedFrontPlayerFlag = false; //Ожидание при встрече игрока флаг для однократного срабатывания пофорота головы поумолчанию
    [HideInInspector] public bool rotationPatrolFlag = false; //Ожидание при встрече повороте по направлению

    [Space]
    [Header("Настройки видимости")]

    [SerializeField]private float navDistanceToTarget;
    private float straightDistanceToTarget;
    [SerializeField] private float lookDistance;
    [SerializeField] private float swordUnequipDistance;
    [SerializeField] private float bowEquipDistance;
    [SerializeField] private float attackDistance;
    [SerializeField] private float attackStayDistance;
    private float attackDistanceFinal;
    private float attackStayDistanceFinal;


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
    public GameObject stayBow;
    public GameObject attackBow;
    public GameObject flyArrow;
    public GameObject arrowOnBow;
    public GameObject arrowInHand;
    [Range(0f, 10f)]
    public float bowAttackMinSpeed = 1f;
    [Range(0f, 10f)]
    public float bowAttackMaxSpeed = 5f;
    private bool haveSword;
    private bool haveSheild;
    private bool haveBow;
    private Animator _bowAnimator;
    private PlayerStats _playerStats;
    private EntityAttack _entityAttack;
    private EntityStats _entityStats;

    public float attackSpeed;
    public float damage;
    public float bowDamage;
    private EntityStats entityStatsScript;

    private bool weaponOn = false;
    private bool shieldOn = false;
    private bool bowOn = false;
    private bool isAttack = false; //атака или ожидание

    public bool animationStarted = false; // есть анимация оружия или щита
    private bool animationPatrol = false; // есть анимация деэкипировки для патруля
    private bool attackTarget = false; //цель свободная для атаки
    private bool waitAttack = false; //ожидание цели для атаки
    private bool starting = false; //ИИ инициализирован
    private bool runToTarget = false;//подбежит к цели при удалении в битве
    private bool speedFlag = false;//скорость меняется
    public bool directVisible;

    private IEnumerator targetNoAccessCoroutine;
    public bool targetNoAccessFlag = false;//скорость меняется
    public bool targetNoAccessIEFlag = false;//скорость меняется

    private int random = 0;
    private int randomCostant = 0;

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

        _entityStats = gameObject.GetComponent<EntityStats>();
        _entityAttack = attackSword.GetComponent<EntityAttack>();
        _playerStats = player.GetComponent<PlayerStats>();
        _navMeshAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        _animator = gameObject.GetComponent<Animator>();
        _bowAnimator = attackBow.GetComponent<Animator>();
        
        _rigBuilder = gameObject.GetComponent<RigBuilder>();
        _rigBuilder.enabled  = false;
            hadTargetStartPosition = hadLookTarget.transform.localPosition;
            bodyTargetStartPosition = bodyLookTarget.transform.localPosition;

        _entityAttack.damage = damage;
        _entityStats.AngryGroup = angryGroup;
        _entityStats.Group = group;
        _entityAttack.StatsScript = _entityStats;
        _entityAttack.Player_Stats = _playerStats;

      

        //------------------------------------ИНИЦИАЛИЗАЦИЯ РАНДОМАЙЗЕРОВ-----------------------------
        randomCostant = Random.Range(1,100);
        StartCoroutine(Cyclic());


        //------------------------------------ИНИЦИАЛИЗАЦИЯ ЭКИПИРОВКИ-----------------------------

        if(staySword != null && attackSword != null)
        {
            staySword.SetActive (true);
            attackSword.SetActive (false);
            haveSword = true;
        }
        else
            haveSword = false;

        if(stayBow != null && attackBow != null)
        {
            stayBow.SetActive (true);
            attackBow.SetActive (false);
            haveBow = true;
        }
        else
            haveBow = false;
        
        if(stayShield != null && attackShield != null)
        {
            stayShield.SetActive (true);
            attackShield.SetActive (false);
            haveSheild = true;
        }
        else
            haveSheild = false;

    }

    IEnumerator Cyclic ()
    {
        int i = 0;
        while(1<2)
        {
            _animator.SetInteger("Idle", Random.Range(1,3));
            target = TargetSearch();
            i++;
            if(i == 3)
            {
                i = 0;
                random = Random.Range(1,100);
            }
            yield return new WaitForSeconds(2);
        }

    }

    void Update()
    {
        // Debug.DrawLine(gameObject.transform.position, gameObject.transform.position+DirectionNavMeshPath(_navMeshAgent, gameObject), Color.red, 2.5f, false);
        if(Input.GetKeyDown("u"))
            {
                if(animationStarted == false)
                    StartCoroutine(AllUnequip(true));
            }
        if(Input.GetKeyDown("y"))
            {
                if(animationStarted == false)
                    StartCoroutine(AllEquip(true));
            }
        if(Input.GetKeyDown("j"))
            {
                if(animationStarted == false)
                    StartCoroutine(BowUnequip(true));
            }
        if(Input.GetKeyDown("h"))
            {
                if(animationStarted == false)
                    StartCoroutine(BowEquip(true));
            }



        
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
        if(animationStarted == false)
            StartCoroutine(AllUnequip(true));

        //Поворот головы к игроку при приблежении
        if(stoppedFrontPlayer == true)  
        {
            stoppedFrontPlayerFlag = true;
            LookOnPlayer(hadLookTarget, bodyLookTarget, player, _rigBuilder, 1f, bodyTargetStartPosition);
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

        if(patrolPoints.Count == 0)
        {
            SetStayAnimations();
            return;
        }

        //Движение к точке
        if(stay == true|| rotationPatrolFlag == true)
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

        if(Mathf.Abs(AnglePathLine())>25)
        {
            StartCoroutine(PatrolRotateToPath());
            return;
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

        directVisible = DirectVisibleCheck(target, gameObject.transform.position);
        


        //Обработка по расстоянию до цели

        //Цель очень далека
        if(straightDistanceToTarget > lookDistance)
        {
            TargetDisable();
            return;
        }

        //Цель недоступна
        if(navDistanceToTarget== -1)
        {
            TargetNoAccess();
        }
        else
        {
            if(targetNoAccessCoroutine != null)
            {
                StopCoroutine(targetNoAccessCoroutine);
                targetNoAccessCoroutine = null;
            }

            targetNoAccessFlag = false;
            targetNoAccessIEFlag = false;
        }

        //Цель в пределах видимости и до доставания оружия и есть прямой зрительный контакт
        if(straightDistanceToTarget <= lookDistance && navDistanceToTarget > swordUnequipDistance && directVisible==true)
        {
            TargetFar();
        }

        //Цель достаточно близко, чтобы достать оружие, но не достаточно близко, чтобы бить
        if(navDistanceToTarget <= swordUnequipDistance && navDistanceToTarget >= bowEquipDistance)
        {
            TargetMiddle();
        }

        //Цель ближе, можно перевести дух
        if(navDistanceToTarget < bowEquipDistance && navDistanceToTarget > attackStayDistanceFinal*1.7f && attackTarget == false)
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
        if(navDistanceToTarget < attackStayDistanceFinal && navDistanceToTarget > 0)
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
        SetStayAnimations();
        if(targetNoAccessFlag == false)
        {
            if(targetNoAccessIEFlag == false)
            {
                targetNoAccessCoroutine = TargetNoAccessWait();
                StartCoroutine(targetNoAccessCoroutine);
            }
        }
        else
        {
            _navMeshAgent.SetDestination(targetPosition);
            if(directVisible==true && animationStarted == false)
            {
                if(bowOn == false)
                    StartCoroutine(BowEquip(true));
                else
                {
                    StartCoroutine(BowShoot());
                }
            }
        }
    }
    IEnumerator TargetNoAccessWait()
    {
        targetNoAccessIEFlag = true;
        yield return new WaitForSeconds(1.5f);
        targetNoAccessFlag = true;
    }

    private void TargetFar()
    {
        /////////////////////////////////////////////////////////////////////////////////////ДОБАВИТЬ ВЕРОЯТНОСТЬ ПОДНЯТИЯ ЛУКА
        _navMeshAgent.SetDestination(targetPosition);
        if(bowOn == true && directVisible==true && animationStarted == false)
        {
            SetIdleFootAnimations();
            StartCoroutine(BowShoot());
            return;
        }

        SetRunFootAnimations();

        if(animationStarted == false)   //Анимация рук при отсутствии оных
        {
            if(randomCostant > 50 && weaponOn == true)
                StartCoroutine(SwordEquip(true));
            else
                SetRunHandsAnimations();
        }
    }

    private void TargetMiddle()
    {
        _navMeshAgent.SetDestination(targetPosition);

        if(bowOn == true && directVisible==true && animationStarted == false)
        {
            StartCoroutine(BowShoot());
            return;
        }

        SetRunFootAnimations();

        if(animationStarted == false)   //Анимация рук при отсутствии оных
        {
            if(weaponOn == true && shieldOn == true)
                SetRunHandsAnimations();
            else
                StartCoroutine(AllEquip(true));
        }
    }
    
    private void TargetVeryMiddle()
    {
        _navMeshAgent.SetDestination(targetPosition);


        if(animationStarted == false)   //Анимация рук при отсутствии оных
        {
            StartCoroutine(SwordEquip(true));
            SetRunHandsAnimations();

            if(random < waitTargetChance)
            {
                SetStayAnimations();
                return;
            }
        }

        SetRunFootAnimations();

    }

    private void TargetNear()
    {
        _navMeshAgent.SetDestination(targetPosition);

        runToTarget = true;
        SetRunFootAnimations();
        SetRunHandsAnimations();

        if(animationStarted == false)
            HitAndEquip(false);
    }

    private void TargetVeryNear()
    {
        _navMeshAgent.SetDestination(targetPosition);

        if(runToTarget == true)
        {
            SetRunFootAnimations();
            SetRunHandsAnimations();
            HitAndEquip(false);
        }
        else
            if(animationStarted == false)
                HitAndEquip(true);
    }

    private void TargetHere()
    {
        _navMeshAgent.SetDestination(targetPosition);

        SetAttackReadyFootAnimations();

        if(weaponOn == true)
            SetAttackReadyHandsAnimations();
        else
            SetStayHandsAnimations();

        if(animationStarted == false)
            HitAndEquip(true);
        runToTarget = false;
    }




    /*----------------------------------------------------------------------------------------------------------------------------------------------------------------------
    ------------------------------------------------------------------------------------------------------------------------------------------------------------------------

                                                                           МЕТОДЫ ПОДДЕРЖКИ
                                                        
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
    

    private static float GetPathRemainingDistance(UnityEngine.AI.NavMeshAgent navMeshAgent)
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

        if(distance == 0)
            return -1f;

        return distance;
    }

    public static void LookOnPlayer(GameObject hadLookTarget, GameObject bodyLookTarget, GameObject player, RigBuilder _rigBuilder, float speed, Vector3 bodyTargetStartPosition)
    {
        hadLookTarget.transform.position = Vector3.MoveTowards(hadLookTarget.transform.position, player.transform.GetChild(0).position, Time.deltaTime * speed);
        while(bodyLookTarget.transform.localPosition != bodyTargetStartPosition)
        {
            bodyLookTarget.transform.localPosition = Vector3.MoveTowards(bodyLookTarget.transform.localPosition, bodyTargetStartPosition, Time.deltaTime * 20f);
        }
        //bodyLookTarget.transform.position = Vector3.MoveTowards(bodyLookTarget.transform.position, player.transform.position+ new Vector3(0,0.3f,0), Time.deltaTime * speed);

        _rigBuilder.enabled  = true;
    }

    public void LookOnPlayerWithBow(float speed)
    {
        hadLookTarget.transform.position = Vector3.MoveTowards(hadLookTarget.transform.position, player.transform.GetChild(0).position, Time.deltaTime * speed);
        bodyLookTarget.transform.position = Vector3.MoveTowards(bodyLookTarget.transform.position, player.transform.position+ new Vector3(0,0.3f,0), Time.deltaTime * speed);

        if(10<Vector3.Angle(new Vector3(player.transform.position.x - transform.position.x, 0, player.transform.position.z - transform.position.z), transform.forward))
        {
           
            StartCoroutine(RotationToTagret(5,1));
           
        }

        _rigBuilder.enabled  = true;
    }

    public static void LookOnOtherTarget(GameObject hadLookTarget, GameObject bodyLookTarget, GameObject target, RigBuilder _rigBuilder, Vector3 bodyTargetStartPosition)
    {
        hadLookTarget.transform.position = Vector3.MoveTowards(hadLookTarget.transform.position, target.transform.position+ new Vector3(0,1,0), Time.deltaTime * 50);
        while(bodyLookTarget.transform.localPosition != bodyTargetStartPosition)
        {
            bodyLookTarget.transform.localPosition = Vector3.MoveTowards(bodyLookTarget.transform.localPosition, bodyTargetStartPosition, Time.deltaTime * 20f);
        }
        //bodyLookTarget.transform.position = Vector3.MoveTowards(bodyLookTarget.transform.position, target.transform.position, Time.deltaTime * 50);

        _rigBuilder.enabled  = true;
    }

    private static Vector3 DirectionNavMeshPath(UnityEngine.AI.NavMeshAgent navMeshAgent, GameObject self)
    {
        if (navMeshAgent.pathPending ||
            navMeshAgent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid ||
            navMeshAgent.path.corners.Length < 2)
            return new Vector3(0f,0f,0f);
        
        return new Vector3(navMeshAgent.path.corners[1].x - self.transform.position.x, 0, navMeshAgent.path.corners[1].z - self.transform.position.z);
    }


    public float AnglePathLine() //угол между ИИ и путём
    {
        Vector3 vector = DirectionNavMeshPath(_navMeshAgent, gameObject);
        Vector3 self = transform.forward;
        Vector3 upSelf = transform.up;
        return Mathf.Atan2(Vector3.Dot(upSelf, Vector3.Cross(vector, self)), Vector3.Dot(vector, self)) * Mathf.Rad2Deg;
    }


    public void Die()
    {
        _navMeshAgent.SetDestination(transform.position);
        _rigBuilder.enabled = false;

        if(attackSword.activeSelf == true)
        {
            attackSword.GetComponent<BoxCollider>().enabled = true;
            attackSword.GetComponent<Rigidbody>().useGravity = true;
            attackSword.GetComponent<EntityAttack>().enabled = false;
        }
    }


    /*----------------------------------------------------------------------------------------------------------------------------------------------------------------------
    ------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        ****                                                           МЕТОДЫ ЭКИПИРОВКИ
                                                        
    ------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    ----------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

    IEnumerator BowEquip(bool selfAnimationBlock)
    {
        if(selfAnimationBlock == true)
            animationStarted = true;

        if(bowOn == false)
        {
            yield return StartCoroutine(AllUnequip(false));
            yield return StartCoroutine(BowEquipUnequip(false));
        }

        if(selfAnimationBlock == true)
            animationStarted = false;
    }

    IEnumerator BowUnequip(bool selfAnimationBlock)
    {
        if(selfAnimationBlock == true)
            animationStarted = true;

        if(bowOn == true)
        {
            yield return StartCoroutine(BowEquipUnequip(false));
        }

        if(selfAnimationBlock == true)
            animationStarted = false;
    }

    IEnumerator SwordUnequip(bool selfAnimationBlock)
    {
        if(selfAnimationBlock == true)
            animationStarted = true;

        if(weaponOn == true)
            yield return StartCoroutine(SwordEquipUneguip(false));

        if(selfAnimationBlock == true)
            animationStarted = false;
    }


    IEnumerator SwordEquip(bool selfAnimationBlock)
    {
        if(selfAnimationBlock == true)
            animationStarted = true;

        if(weaponOn == false)
            yield return StartCoroutine(SwordEquipUneguip(false));

        if(selfAnimationBlock == true)
            animationStarted = false;
    }

    IEnumerator ShieldUnequip(bool selfAnimationBlock)
    {
        if(selfAnimationBlock == true)
            animationStarted = true;
        
        if(shieldOn == true)
            yield return StartCoroutine(ShieldEquipUneguip(false));

        if(selfAnimationBlock == true)
            animationStarted = false;
    }


    IEnumerator ShieldEquip(bool selfAnimationBlock)
    {
        if(selfAnimationBlock == true)
            animationStarted = true;

        if(shieldOn == false)
            yield return StartCoroutine(ShieldEquipUneguip(false));

        if(selfAnimationBlock == true)
            animationStarted = false;
    }

    IEnumerator AllEquip(bool selfAnimationBlock)
    {
        if(selfAnimationBlock == true)
            animationStarted = true;

        if(shieldOn == false)
            yield return StartCoroutine(ShieldEquipUneguip(false));

        if(weaponOn == false)
            yield return StartCoroutine(SwordEquipUneguip(false));
        
        yield return null;

        if(selfAnimationBlock == true)
            animationStarted = false;    
    }

    IEnumerator AllUnequip(bool selfAnimationBlock)
    {
        if(selfAnimationBlock == true)
            animationStarted = true;

        if(shieldOn == true)
        {
            yield return StartCoroutine(ShieldEquipUneguip(false));
        }

        if(weaponOn == true)
        {
            yield return StartCoroutine(SwordEquipUneguip(false));
        }
        
        yield return null;

        if(selfAnimationBlock == true)
            animationStarted = false;    
    }

    IEnumerator BowEquipUnequip(bool selfAnimationBlock)
    {
        if(selfAnimationBlock == true)
            animationStarted = true;

        if(haveBow == false)
            yield break;

        yield return new WaitForSeconds(0.2f);

        if(bowOn == false)
        {
            _animator.SetTrigger("BowEquip");   
            bowOn = true;
        }
           

        else
        {
            //Debug.Log("Деикип "+navDistanceToTarget);
            _animator.SetTrigger("BowUnequip");
            bowOn = false;
        }


        yield return new WaitForSeconds(0.1f);

        if(bowOn == true)
        {
            if(stayBow != null)
                stayBow.SetActive (false);
            if(attackBow != null)
                attackBow.SetActive (true);
            bowOn = true;
            yield return new WaitForSeconds(0.4f);
            _bowAnimator.SetTrigger("Charging");
        }
        else
        {
            if(stayBow != null)
                stayBow.SetActive (true);
            if(attackBow != null)
                attackBow.SetActive (false);
            bowOn = false;
             yield return new WaitForSeconds(0.4f);
            _bowAnimator.SetTrigger("Fire");
        }

        
         yield return new WaitForSeconds(0.2f);


        if(selfAnimationBlock == true)
            animationStarted = false;
    }

    IEnumerator ShieldEquipUneguip (bool selfAnimationBlock) //параметр - использовать свою блокировку анимаций или нет (AllEquip, как пример)
    {
        if(selfAnimationBlock == true)
            animationStarted = true;

        if(haveSheild == false)
            yield break;

        if(bowOn == true)
            yield return StartCoroutine(BowEquipUnequip(false));

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

        if(haveSword == false)
            yield break;

        if(bowOn == true)
            yield return StartCoroutine(BowEquipUnequip(false));

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

    
    IEnumerator BowShoot()
    {
        animationStarted = true;
        if(!_bowAnimator.GetCurrentAnimatorStateInfo(0).IsName("Armature|Run"))
        {
            _bowAnimator.SetTrigger("Charging");
            yield return null;
        }
        _bowAnimator.ResetTrigger("Charging");
        _bowAnimator.SetTrigger("Fire");
        _animator.SetTrigger("BowFire");

        arrowOnBow.SetActive(false);
        GameObject arrow = Instantiate(flyArrow, arrowOnBow.transform.position, arrowOnBow.transform.rotation);

        arrow.GetComponent<EntityArrow>().Initialization(bowDamage, _playerStats, angryGroup);

        arrow.transform.LookAt(target.gameObject.transform);

        yield return new WaitForSeconds(0.7f);
        arrowInHand.SetActive(true);
        yield return new WaitForSeconds(0.8f);

        _bowAnimator.SetTrigger("Charging");
        arrowOnBow.SetActive(true);
        arrowInHand.SetActive(false);

        yield return new WaitForSeconds(4f+Random.Range(1f,5f));
        animationStarted = false;
    }

    private void HitAndEquip(bool totalAttack)//Удар вместе с ногами или только руками
    {
        if(weaponOn == false)
        {
            StartCoroutine(SwordEquip(true));
            return;
        }

        if(isAttack == false)
            StartCoroutine(Hit(totalAttack));
    }

    IEnumerator Hit(bool totalAttack)
    {
        isAttack = true;

        yield return StartCoroutine(RotationToTagret(25,2));

        if(totalAttack == true)
            SetHitAnimations();
        else
            SetHitHandsAnimations();

        yield return new WaitForSeconds(0.1f);
        _entityAttack.AttackStartCollider();
        yield return new WaitForSeconds(0.3f);
        
        _animator.SetInteger("AttackSelect", 0);

        yield return new WaitForSeconds(0.2f);
        _entityAttack.AttackFinishCollider();

        float attackTime = Random.Range(0.75f,1.25f)*attackSpeed; // ожидание между атаками и доворот в ожидании
        for(int i = 0; i < 5;i++)
        {
            yield return StartCoroutine(RotationToTagret(25,2));
            yield return new WaitForSeconds(attackTime/5);
        }

        isAttack = false;
    }


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

        if(bowOn == false)
            LookOnPlayer(hadLookTarget, bodyLookTarget, player, _rigBuilder, 50f, bodyTargetStartPosition);
        else
            LookOnPlayerWithBow(50f);

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

        if(bowOn == false)
            LookOnOtherTarget(hadLookTarget, bodyLookTarget, target, _rigBuilder, bodyTargetStartPosition);
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
        int layerMask = 1 << 0;

        for(int i = 0; i < 3;i++)
        {
            
            //ray.direction = targetVis.transform.position - self - new Vector3(0,i,0);
            Debug.DrawLine(self+new Vector3(0,2,0) + Vector3.Normalize(targetVector)/2, targetVis.transform.position - new Vector3(0,i-0.7f,0), Color.red, 2.5f, false);
            Physics.Linecast(self+new Vector3(0,2,0) + Vector3.Normalize(targetVector)/2, targetVis.transform.position - new Vector3(0,i-0.7f,0), out lookArea, layerMask);

            if (lookArea.collider != null)
            {
                if (lookArea.collider.gameObject == targetVis)
                {
                    return true;
                } 
            }
        }
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
                if(hitColliders[i].GetComponent<RumAIHuman>())
                {
                    RumAIHuman potentialTarget = hitColliders[i].GetComponent<RumAIHuman>();
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

    IEnumerator RotationToTagret(int angle, int returnType)
    {
        if(target!=player && target!= null)
        {
            transform.LookAt(new Vector3(target.transform.position.x, transform.position.y ,target.transform.position.z));
        }   

        Vector3 targetDirection;
        int step = 0;
        int limit = 480;
        while(angle<Vector3.Angle(new Vector3(target.transform.position.x - transform.position.x, 0, target.transform.position.z - transform.position.z), transform.forward)) // доворот к игроку
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
            if(step>limit || target == null)
                {
                    break;
                }
        }

        _animator.SetInteger("JumpReturn",returnType);
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

    IEnumerator PatrolRotateToPath()
    {
        if(rotationPatrolFlag == true)
            yield break;

        rotationPatrolFlag = true;
        SetStayAnimations();
        float ang = AnglePathLine();

        Vector3 targetDirection;
        int step = 0;
        int limit = 480;

        _animator.ResetTrigger("ExitRotationFoots");

        if(ang<0)
        {
            SetLeftTurn();

            while(-8 > AnglePathLine()) // доворот по пути
            {
                targetDirection = DirectionNavMeshPath(_navMeshAgent, gameObject);
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, Time.deltaTime*0.7f, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDirection);  
                yield return null;

                step++;
                if(step>limit)
                {
                    break;
                }
            }
        }

        else
        {
            SetRightTurn();

            while(8 < AnglePathLine()) // доворот по пути
            {
                targetDirection = DirectionNavMeshPath(_navMeshAgent, gameObject);
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, Time.deltaTime*0.7f, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDirection);  
                yield return null;

                step++;
                if(step>limit)
                {
                    break;
                }
            }
        }
        yield return null;
        
        _animator.SetTrigger("ExitRotationFoots");
        rotationPatrolFlag = false;
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
    void SetLeftTurn()
    {
        _animator.SetTrigger("ToRRotationFoots");
        _animator.ResetTrigger("ToIdleFoots");
        _animator.ResetTrigger("ToRunFoots");
        _animator.ResetTrigger("ToWalkFoots");

        _animator.ResetTrigger("ToWalkHands");
        _animator.ResetTrigger("ToRunHands");
        _animator.SetTrigger("ToIdleHands");
    }
    void SetRightTurn()
    {
        _animator.SetTrigger("ToLRotationFoots");
        _animator.ResetTrigger("ToIdleFoots");
        _animator.ResetTrigger("ToRunFoots");
        _animator.ResetTrigger("ToWalkFoots");

        _animator.ResetTrigger("ToWalkHands");
        _animator.ResetTrigger("ToRunHands");
        _animator.SetTrigger("ToIdleHands");
    }


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

    void SetHitAnimations()
    {
        _animator.SetInteger("AttackSelect", Random.Range(1,7));
        _animator.SetTrigger("ReadyAttackFoot");
        _animator.SetTrigger("AttackHands");
    }


    //Анимации рук

    void SetHitHandsAnimations()
    {
        _animator.SetInteger("AttackSelect", Random.Range(1,7));
        _animator.SetTrigger("AttackHands");
    }

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