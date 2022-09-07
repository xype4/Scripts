using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalAI : MonoBehaviour
{
    public GameObject Player;
    public GameObject Target;

    public float lookDistance;
    private float biteDistance = 2.4f;
    private float distance;
    private NavMeshAgent _agent;
    private Animator _animator;
    private Vector3 targPos;
    private Vector3 AIPosition;
    public GameObject MeshTrigger;
    public EntityAttack EntityAttackScript;
    public EntityStats EntityStatsScript;

    public float AngryFactor = 1;

    private bool biteFlag = true;
    private bool walkFlag = true;
    private bool toPlayer = false;
    private bool toTaget = false;
    private bool toTrigger = false;
    private int triggerPassword;
    private int triggerCount = 0;
    

    NavMeshPath navMeshPath;
    GameObject HeavyTrigger;
    Vector3 random_point;
    Vector3 final_point;
    Vector3 reserv_point;

    [Tooltip("1 - Хищное животное 2 - Мирное животное 3 - Бандит 4 - Злой маг 5 - Житель 6 - Стражник")]
    public int Group;
    public List<int> FriendlyGroup;

    void Start()
    {
         _agent = gameObject.GetComponent<NavMeshAgent>();
         _animator = gameObject.GetComponent<Animator>();
         targPos = Player.transform.position;
         AIPosition = transform.position;
         navMeshPath = new NavMeshPath();
    }

    void Update()
    {
        if(EntityStatsScript.die == true)
        {
            _agent.SetDestination(transform.position);
            gameObject.GetComponent<AnimalAI>().enabled = false;
        }
        targPos = Player.transform.position;
        AIPosition = transform.position;
        distance = Vector3.Distance (AIPosition , targPos);

        if(distance > lookDistance)
        {
            if(walkFlag == true)
            {
                walkFlag = false;
                _animator.ResetTrigger("Run");
                _animator.ResetTrigger("Idle");
                _animator.SetTrigger("Walk");
                _agent.SetDestination(AIPosition);
                StartCoroutine(walk());
            }
            return;
        }
        else
        {
            if(walkFlag == false)
            {
                _agent.speed = 6;
                walkFlag = true;
                StopCoroutine(walk());
            }
        }

        if(distance > biteDistance)
        {
            _animator.ResetTrigger("Idle");
            _animator.SetTrigger("Run");
        }

        if(toTaget == true && _agent.remainingDistance < 1f && toTrigger == false)
        {
            toPlayer = false; toTaget = false;
        }

        if (distance < lookDistance && distance > biteDistance && toPlayer == false && toTaget == false && toTrigger == false)

        SetTarget(false);

        if(distance < lookDistance && distance > biteDistance && toTaget == false && toTrigger == false)
        {
            _agent.SetDestination(targPos);
            return;
        }

        if(distance < biteDistance && toPlayer == true)
        {
            _animator.ResetTrigger("Run");
            _animator.SetTrigger("Idle");
            _agent.SetDestination(AIPosition);
            if(biteFlag == true)
            StartCoroutine(bite());
            return;
        }
    }

    IEnumerator walk ()
    {
        _agent.SetDestination(TargetDeterminate(5, 25,0,360));
        _agent.speed = 1;
        while(_agent.remainingDistance > 0.1f)
        {
            yield return new WaitForSeconds(0.1f);
        }
        _animator.SetTrigger("Idle");
        yield return new WaitForSeconds(Random.Range(5f,10f));
        walkFlag = true;
        _agent.speed = 6;
    }

    public void SetTarget(bool toTarg)
        {
            _animator.ResetTrigger("Idle");
            _animator.SetTrigger("Run");

            if(Random.Range(0,100) > (int)(70f*AngryFactor) && toTarg == false)
            {
                Vector3 look = new Vector3 (targPos.x - AIPosition.x, 0, targPos.z - AIPosition.z);
                if(Vector3.Angle(look, transform.forward) > 40)
                {
                   ShortToPlayer(false);
                }
                else
                {
                    final_point = targPos;
                    toPlayer = true;
                }
            }
            else
            {
                final_point = TargetDeterminate(5, 15, 25, 40);
                toTaget = true;
            }
            _agent.SetDestination(final_point);
            
            return;
        }
    void ShortToPlayer(bool toTrigger_)
    {
        toTrigger = true;
        triggerCount++;
        
        if(toTrigger_ == true)
            {
            final_point = ShortToPlayer_ShortPoint(HeavyTrigger.transform.position);
            }
        else
            {
            final_point = ShortToPlayer_ShortPoint(targPos);
            }
        Destroy(HeavyTrigger);
        HeavyTrigger = Instantiate(MeshTrigger, final_point, new Quaternion(0,0,0,0), transform);
        GameObject NewTrigger = Instantiate(MeshTrigger, final_point, new Quaternion(0,0,0,0));
        triggerPassword = Random.Range(0,1000);
        NewTrigger.GetComponent<TriggerMeshAI>().password = triggerPassword;
        final_point = NewTrigger.transform.position;
    }

    Vector3 ShortToPlayer_ShortPoint(Vector3 targ)
    {
        Vector3[] SpawnTigger = new Vector3[15];
        SpawnTigger[0] = TargetDeterminate(2, 5, 40, 45);
        Vector3 new_point;
        new_point = SpawnTigger[0];
        for(int i = 1; i < 15; i++)
        {
            SpawnTigger[i] = TargetDeterminate(2, 5, 40, 45);
            if(Vector3.Distance(SpawnTigger[i], targ) < Vector3.Distance(SpawnTigger[i-1], targ))
            {
                new_point = SpawnTigger[i];
            }
        }
        return new_point;
    }

    void OnTriggerStay(Collider col)
    {
        if(col.gameObject.GetComponent<TriggerMeshAI>())
        {
            if(col.gameObject.GetComponent<TriggerMeshAI>().password == triggerPassword)
            {
                Destroy(col.gameObject);

                Vector3 look = new Vector3 (targPos.x - AIPosition.x, 0, targPos.z - AIPosition.z);
                if(Vector3.Angle(look, transform.forward) > 40 && triggerCount < 8)
                {
                    ShortToPlayer(true);
                    _agent.SetDestination(final_point);
                }
                else
                {
                    final_point = targPos;
                    toPlayer = true;
                    toTrigger = false;
                    triggerCount = 0;
                }
            }
        }
    }

    IEnumerator bite ()
    {
        //transform.LookAt(playerPos);
        biteFlag = false;
        if(Random.Range(0,2) == 0)
        _animator.SetTrigger("Bite1");
        else
        _animator.SetTrigger("Bite2");

        EntityAttackScript.StartAttack_();

        yield return new WaitForSeconds(Random.Range(0.7f,1.2f));

        if(Random.Range(0,100) > (int)(20f/AngryFactor))
        {
            toPlayer = false; toTaget = false;
            SetTarget(true);
        }
        yield return new WaitForSeconds(Random.Range(0.5f,1f));
        biteFlag = true;
    }

    Vector3 TargetDeterminate(int minRadius, int maxRadius, int minA, int maxA)
    {
        bool getCorrectPoint = false;
        int flag = 0; int flag2 = 0;
        while(!getCorrectPoint)
        {
            if(flag2 >= 100)
            {
                random_point = new Vector3(0,0,0);
                break;
            }
            flag2++;
            NavMeshHit navmeshHit;
            NavMesh.SamplePosition(Random.insideUnitSphere * maxRadius + AIPosition, out navmeshHit, maxRadius, NavMesh.AllAreas);
            random_point = navmeshHit.position;

            _agent.CalculatePath(random_point, navMeshPath);

                        
            Vector3 look = new Vector3 (random_point.x - AIPosition.x, 0, random_point.z - AIPosition.z);

            if(navMeshPath.status == NavMeshPathStatus.PathComplete)
            {
                flag++;
                if((Vector3.Angle(look, transform.forward) < maxA && Vector3.Angle(look, transform.forward) > minA && Vector3.Distance(AIPosition, random_point) > minRadius) || flag >= 15)
                    getCorrectPoint = true;
            }
        }
        return random_point;
    }
}