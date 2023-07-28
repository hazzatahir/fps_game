using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum State
{
    PATROL , STAND
}

public enum Type
{
    ENEMY , NPC , FRIEND
}

public class NPCScript : MonoBehaviour
{
    public string npcName;
    public Type npcType;

    [Space]
    [SerializeField] Transform[] patrolPoints;
    [SerializeField] float patrolTime;
    float patrolTimer;
    NavMeshAgent navAgent;
    Animator anim;
    public State npcState = State.PATROL;
    
    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        
        patrolPoints = new Transform[GameObject.Find("PatrolPoints").transform.childCount];
        anim = GetComponent<Animator>();
        patrolTime = Random.Range(3f, 10f);
        
    }

    void Start()
    {
        patrolTimer = patrolTime;

        for (int i = 0; i < GameObject.Find("PatrolPoints").transform.childCount; i++)
        {
            patrolPoints[i] = GameObject.Find("PatrolPoints").transform.GetChild(i);
        }
    }


    void Update()
    {
        if(npcState == State.PATROL)
        {
            if(patrolTimer >= patrolTime)
            {
                GoToNextDestination();
            }

        }
        else if(npcState == State.STAND)
        {
            patrolTimer += Time.deltaTime;
            navAgent.isStopped = true;
            anim.SetBool("Walk", false);

            if (patrolTimer >= patrolTime)
            {
                npcState = State.PATROL;
                return;
            }
        }

        if (Vector2.Distance(transform.position, navAgent.destination) <= 1f && npcState != State.STAND)
        {
            npcState = State.STAND;
            Debug.Log("WORKED");
            patrolTime = Random.Range(3f, 10f);
        }

    }

    void GoToNextDestination()
    {
        patrolTimer = 0;

        navAgent.isStopped = false;

        anim.SetBool("Walk", true);

        Vector3 destination = new Vector3(patrolPoints[Random.Range(0, patrolPoints.Length)].localPosition.x, transform.position.y, patrolPoints[Random.Range(0, patrolPoints.Length)].localPosition.z);

        navAgent.SetDestination(patrolPoints[Random.Range(0 ,patrolPoints.Length)].localPosition);

    }


} //class




















