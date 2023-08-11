using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public PlayerStatus playerStatus;

    [SerializeField] float npcInteractDistance;

    public LayerMask targetLayer;

    void Start()
    {
        InvokeRepeating("InteractWithNPC", 2f, 0.5f);
    }

    
    void InteractWithNPC()
    {
        GameObject[] npc = GameObject.FindGameObjectsWithTag("NPC");

        if(npc.Length > 0)
        {
            for (int i = 0; i < npc.Length; i++)
            {
                if (Vector3.Distance(transform.position, npc[i].transform.position) <= npcInteractDistance)
                {
                   if(npc[i].GetComponent<NPCScript>().npcType == Type.NPC)
                    {
                        GetComponent<Inputs>().canInteract = true;
                        GameplayController.instance.SetNPCStatus(npc[i]);
                        npc[i].GetComponent<NPCScript>().nearestEnemy = true;
                        break;
                    } 
                   
                }
                else
                {
                    GetComponent<Inputs>().canInteract = false;
                    npc[i].GetComponent<NPCScript>().nearestEnemy = false;
                }
            }
        }

    }

    public void Shooting()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position , Camera.main.transform.forward , out hit, 12f, targetLayer))
        {
            //Debug.Log(hit.collider.gameObject.name);

            if(hit.collider.tag == "Enemy")
            {
                hit.collider.GetComponent<NPCScript>().TakeDamage();

                if(hit.collider.GetComponent<NPCScript>().npcHealth <= 0)
                {
                    hit.collider.transform.LookAt(transform);
                    Invoke("CheckEnemies", 0.5f);
                    
                }
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

    public void TakeDamage()
    {
        if (GameplayController.instance.health <= 0) return;

        GameplayController.instance.DecreaseHealthPoint(Time.deltaTime * 5f);

        Debug.Log(GameplayController.instance.health);

        if (GameplayController.instance.health <= 0)
        {
            GameplayController.instance.gameMode = GameMode.END;
            GameplayController.instance.ActivateGameEndedPanel("YOU LOSE!");
            AudioManager.instance.PlayGameEndedLine(false);
        }
    }


} //class

















