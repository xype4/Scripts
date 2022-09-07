using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FoxSpawner : MonoBehaviour
{
    public SpawnerDataBase SpawnerDB;
    private Inventory inventory;
    private GameObject DropPanel;
    private GameObject F;
    private GameObject Player;
    private PlayerStats Player_Stats;
    public GameObject Fox;
    private List<GameObject> SpawnFox;
    private NavMeshAgent _agent;
    public NavMeshPath navMeshPath;
    public int distanceSpawn;
    public int MaxMobCount;
    public int MinMobCount;
    private bool mobExist = false;

    void Start()
    {
        navMeshPath = new NavMeshPath();
        SpawnFox = new List<GameObject>();
        _agent = gameObject.GetComponent<NavMeshAgent>();
        inventory = SpawnerDB.inventory;
        DropPanel = SpawnerDB.DropPanel;
        F = SpawnerDB.F;
        Player = SpawnerDB.Player;
        Player_Stats = SpawnerDB.Player_Stats;
    }

    void Update()
    {
        float distance = Vector3.Distance (gameObject.transform.position , Player.transform.position);
        if(distanceSpawn > distance && mobExist == false)
        {
            int count = Random.Range(MinMobCount,MaxMobCount+1);

            for(int i = 0; i < count; i++)
            {
                GameObject NewFox = Instantiate(Fox, AreaDeterminate(5,15), new Quaternion(0,0,0,0));
                AnimalAI ai = NewFox.GetComponent<AnimalAI>();
                ai.Player = Player;
                ai.EntityStatsScript = NewFox.GetComponent<EntityStats>();
                ai.EntityAttackScript = NewFox.transform.GetChild(1).gameObject.GetComponent<EntityAttack>();
                NewFox.transform.GetChild(1).gameObject.GetComponent<EntityAttack>().Player_Stats = Player_Stats;
                NewFox.GetComponent<DropMob>().DropPanel = DropPanel;
                NewFox.GetComponent<DropMob>().inventory = inventory;
                NewFox.GetComponent<DropMob>().F = F;
                SpawnFox.Add(NewFox);
            }
            mobExist = true;
        }
        else if(distanceSpawn < distance)
        {
            for(int i = 0; i < SpawnFox.Count; i++)
            {
                Destroy(SpawnFox[i]);
            }
            mobExist = false;
        }


    }

    Vector3 AreaDeterminate(int minRadius, int maxRadius)
    {
        Vector3 random_point = new Vector3(0,0,0);
        bool getCorrectPoint = false;
        while(!getCorrectPoint)
        {
            NavMeshHit navmeshHit;
            NavMesh.SamplePosition(Random.insideUnitSphere * maxRadius + gameObject.transform.position, out navmeshHit, maxRadius, NavMesh.AllAreas);
            random_point = navmeshHit.position;

            _agent.CalculatePath(random_point, navMeshPath);
            
            if(navMeshPath.status == NavMeshPathStatus.PathComplete)
                getCorrectPoint = true;
        }
        random_point += new Vector3 (0,1,0);
        return random_point;
    }
}
