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
    NPC, FRIEND, ENEMY
}

public class NPCScript : MonoBehaviour
{
    public string npcName;
    public Type npcType = Type.NPC;

    [Space]
    [SerializeField] Transform[] patrolPoints;
    [SerializeField] float patrolTime;
    float patrolTimer;
    NavMeshAgent navAgent;
    Animator anim;
    public State npcState = State.PATROL;
    public PlayerStatus npcStatus;
    public float npcHealth;
    public bool nearestEnemy;
    Transform player;
    float posDifference;
    public Vector3 enemyDest , playerDest;
    [HideInInspector] public bool changeNPCTarget;
    public LayerMask targetLayer;
    int enemyIndex;
    float shootTime;
    float shootTimer;
    bool setPlayerAsTarget;
    int destinationIndex;

    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        npcHealth = Random.Range(3 , 7);
        patrolPoints = new Transform[GameObject.Find("PatrolPoints").transform.childCount];
        anim = GetComponent<Animator>();
        patrolTime = Random.Range(6f, 12f);
        player = GameObject.FindWithTag("Player").transform;
        shootTime = Random.Range(0.5f, 1f);
    }

    void Start()
    {
        patrolTimer = patrolTime;
        
        navAgent.stoppingDistance = Random.Range(1.5f, 3f);
        //Debug.Log(navAgent.speed);
        if(tag == "Enemy")
        {
            navAgent.stoppingDistance = Random.Range(3f, 8f);
            //targetLayer = (int)8;
        }

        for (int i = 0; i < GameObject.Find("PatrolPoints").transform.childCount; i++)
        {
            patrolPoints[i] = GameObject.Find("PatrolPoints").transform.GetChild(i);
        }
    }


    void Update()
    {
        if (npcHealth <= 0)
        {
            //Invoke("PlayDeadAnimation", 0.5f);
            navAgent.isStopped = true;
            navAgent.velocity = Vector3.zero;
            transform.LookAt(player);
            Debug.Log("DEAD");
            return;
        }


        if (npcType == Type.NPC)
        {
            //Debug.Log(patrolTimer);

            if (patrolTimer <= patrolTime)
            {
                GoToNextDestination();
                patrolTimer += Time.deltaTime;
                
            }
            else
            {
                destinationIndex = Random.Range(0, patrolPoints.Length);
                navAgent.SetDestination(patrolPoints[destinationIndex].localPosition);
                patrolTimer = 0;
                
            }


            /*if (npcState == State.PATROL)
            {
                if (patrolTimer >= patrolTime)
                {
                    GoToNextDestination();
                }

            }
            else if (npcState == State.STAND)
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
                //Debug.Log("WORKED");
                patrolTime = Random.Range(3f, 10f);
            }
            */
        }

        if(npcType == Type.FRIEND)
        {
            if(GameplayController.instance.gameMode == GameMode.FREE)
            {
                anim.SetBool("Walk", false);
                Vector3 playerDest = player.position;
                navAgent.SetDestination(playerDest);
                navAgent.speed = 3.5f;
                navAgent.isStopped = false;

                
                
                if (navAgent.remainingDistance <= navAgent.stoppingDistance)
                {
                    anim.SetBool("Run", false);
                    Debug.Log("REMAININIG" + navAgent.remainingDistance);
                    Debug.Log("STOPPING" + navAgent.stoppingDistance);
                }
                else
                {
                    anim.SetBool("Run", true);
                    anim.SetBool("Shoot", false);
                    
                }
            }
            else
            {
                if (changeNPCTarget)
                {
                    Invoke("ChangeNPCTarget", 0.5f);
                    
                }

                Invoke("CheckDistance", 1f);

            }

        }

        if(npcType == Type.ENEMY)
        {
            
            if (changeNPCTarget)
            {
                Invoke("ChangeEnemyTarget", 0.5f);
            }

            Invoke("CheckDistance", 1f);

        }
    }

    
    void GoToNextDestination()
    {
        //patrolTimer = 0;

        navAgent.isStopped = false;

        anim.SetBool("Walk", true);

        //Vector3 destination = new Vector3(patrolPoints[Random.Range(0, patrolPoints.Length)].localPosition.x, transform.position.y, patrolPoints[Random.Range(0, patrolPoints.Length)].localPosition.z);

    }

    void ChangeNPCTarget()
    {
        navAgent.isStopped = false;
        changeNPCTarget = false;

        anim.SetBool("Walk", false);

        navAgent.stoppingDistance = Random.Range(6f, 10f);

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if(enemies.Length > 0)
        {
            enemyIndex = Random.Range(0, enemies.Length);
            enemyDest = enemies[enemyIndex].transform.position;

            navAgent.destination = enemyDest;
        }
        else
        {
            anim.SetBool("Shoot", false);
            anim.SetBool("Walk", false);
            anim.SetBool("Run", false);
            anim.Play("Idle");
        }
        //transform.LookAt(enemyDest);
        navAgent.speed = 4f;
        Debug.Log(navAgent.speed);
    }

    void ChangeEnemyTarget()
    {
        changeNPCTarget = false;

        anim.SetBool("Walk", false);

        navAgent.stoppingDistance = Random.Range(6f, 10f);

        GameObject[] friends = GameObject.FindGameObjectsWithTag("NPC");

        if (friends.Length > 0)
        {
            if(Random.Range(0 , 2) == 0)
            {
                setPlayerAsTarget = false;

                enemyIndex = Random.Range(0, friends.Length);
                enemyDest = friends[enemyIndex].transform.position;

                navAgent.destination = enemyDest;

            }
            else
            {
                navAgent.destination = player.position;
                setPlayerAsTarget = true;
            }
            
        }
        else
        {
            navAgent.destination = player.position;
            setPlayerAsTarget = true;
        }

        Debug.Log(navAgent.speed);
        navAgent.speed = 4f;
    }

    void CheckDistance()
    {
        navAgent.isStopped = false;
        
        if (tag == "Enemy")
        {
            if(setPlayerAsTarget)
            {
                navAgent.destination = player.position;
            }
            else
            {
                GameObject[] friends = GameObject.FindGameObjectsWithTag("NPC");

                if (friends.Length > 0)
                {
                    navAgent.destination = friends[enemyIndex].transform.position;
                }
                else
                {
                    navAgent.destination = player.position;
                }

            }

        }
        else
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            if (enemies.Length > 0)
            {
                navAgent.destination = enemies[enemyIndex].transform.position;
            }
            else
            {
                anim.SetBool("Shoot", false);
                anim.SetBool("Walk", false);
                anim.SetBool("Run", false);
                anim.Play("Idle");
                return;
            }

        }

        if (navAgent.remainingDistance <= navAgent.stoppingDistance)
        {
            anim.SetBool("Run", false);

            transform.LookAt(navAgent.destination);

            navAgent.isStopped = true;

            if (shootTimer < shootTime)
            {
                shootTimer += Time.deltaTime;
                anim.SetBool("Shoot", true);
            }
            else
            {
                Invoke("SetShootTimer", Random.Range(1f, 3f));
            }

        }
        else
        {
            navAgent.isStopped = false;
            anim.SetBool("Run", true);
            anim.SetBool("Shoot", false);
            //Debug.Log("RUN");
        }

    }

    void SetShootTimer()
    {
        shootTimer -= Time.deltaTime;
        anim.SetBool("Shoot", false);
    }

    public void TakeDamage()
    {
        if (npcHealth <= 0) return;

        npcHealth -= Time.deltaTime * 6f;
        
        Debug.Log(npcHealth);

        if(npcHealth <= 0)
        {
            GetComponent<NavMeshAgent>().enabled = false;
            this.enabled = false;
            navAgent.isStopped = true;
            navAgent.velocity = Vector3.zero;
            anim.SetTrigger("Dead");
            AudioManager.instance.PlayNPCDeadSound();
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<Rigidbody>().useGravity = false;
            tag = "Untagged";
            Invoke("DeActviate", 3.5f);
            
        }
    }

    void DeActviate()
    {
        gameObject.SetActive(false);
    }

   
    void Shooting(string target)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position + new Vector3(0f , 1.3f , 0f), transform.forward, out hit, 20f))
        {
            //Debug.Log(hit.collider.gameObject.name);

            if (hit.collider.tag == target && tag != "Enemy")
            {
                hit.collider.GetComponent<NPCScript>().TakeDamage();

                if(hit.collider.GetComponent<NPCScript>().npcHealth <= 0)
                {
                    Invoke("ResetChangeNPCTarget", 0.5f);

                    Invoke("CheckEnemies", 0.5f);
                }
                AudioManager.instance.PlayNPCGunshot();
            }

            if(hit.collider.tag == "NPC" && tag == "Enemy")
            {
                hit.collider.GetComponent<NPCScript>().TakeDamage();

                if (hit.collider.GetComponent<NPCScript>().npcHealth <= 0)
                {
                    Invoke("ResetChangeNPCTarget", 0.25f);

                }
                AudioManager.instance.PlayNPCGunshot();
            }

            if(hit.collider.tag == "Player" && tag == "Enemy")
            {
                hit.collider.GetComponent<PlayerScript>().TakeDamage();
                AudioManager.instance.PlayNPCGunshot();
            }
        }
    }

    void CheckEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length <= 0)
        {
            GameplayController.instance.gameMode = GameMode.END;
            GameplayController.instance.ActivateGameEndedPanel("YOU WON!");
            AudioManager.instance.PlayGameEndedLine(true);
        }
    }

    void ResetChangeNPCTarget()
    {
        changeNPCTarget = true;
    }

} //class

















