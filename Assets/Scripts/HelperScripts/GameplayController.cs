using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public static GameplayController instance;

    [SerializeField] GameObject createPanel;

    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject[] npcPrefab;
    [SerializeField] Transform[] spawnPoints;

    GameObject instructions;

    void Awake()
    {
        MakeInstance();

        instructions = GameObject.Find("Instructions");
    }

    
    void MakeInstance()
    {
        if (instance == null) instance = this;
    }

    public void CreateBtn()
    {
        //Debug.Log("PRESSED");
        createPanel.SetActive(!createPanel.activeInHierarchy);

    }

    public void CreatePlayerBtn()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player) return;

        GameObject newPlayer = Instantiate(playerPrefab) as GameObject;

        newPlayer.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].localPosition + new Vector3(Random.Range(-2.5f , 2.5f) , 0 , Random.Range(-2.5f , 2.5f));

        newPlayer.transform.eulerAngles = new Vector3(0f, Random.Range(0, 359f), 0f);

        instructions.transform.GetChild(0).gameObject.SetActive(false);
        instructions.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void CreateNPCBtn()
    {
        GameObject player = GameObject.FindWithTag("Player");
        GameObject[] NPC = GameObject.FindGameObjectsWithTag("NPC");

        if (player && NPC.Length < 5)
        {
            GameObject npc = Instantiate(npcPrefab[Random.Range(0 , npcPrefab.Length)]) as GameObject;

            npc.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].localPosition + new Vector3(Random.Range(-2.5f, 2.5f), -1.5f, Random.Range(-2.5f, 2.5f));

            npc.transform.eulerAngles = new Vector3(0f, Random.Range(0, 359f), 0f);

            instructions.transform.GetChild(1).gameObject.SetActive(false);
            instructions.transform.GetChild(2).gameObject.SetActive(true);
        }
    }


} //class
















