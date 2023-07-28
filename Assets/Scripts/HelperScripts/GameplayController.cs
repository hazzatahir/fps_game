using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayController : MonoBehaviour
{
    public static GameplayController instance;

    [SerializeField] GameObject createPanel;

    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject[] npcPrefab;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject playerNamePanel, enemyNamePanel;
    [SerializeField] Text playerNameTxt1, playerNameTxt2, npcNameTxt1, npcNameTxt2;
    [SerializeField] GameObject playerHealthObj;
    [SerializeField] GameObject playerInfoPanel;
    int health = 10;


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

    public void ActivatePlayerPanel()
    {
        playerNamePanel.SetActive(true);
        enemyNamePanel.SetActive(false);

        instructions.transform.GetChild(0).gameObject.SetActive(false);
       
    }

    public void ActivateEnemyPanel()
    {
        GameObject player = GameObject.FindWithTag("Player");
       
        if (!player) return;

        GameObject[] NPC = GameObject.FindGameObjectsWithTag("NPC");

        if (NPC.Length < 5)
        {
            enemyNamePanel.SetActive(true);
            playerNamePanel.SetActive(false);
            npcNameTxt1.text = "";

            instructions.transform.GetChild(1).gameObject.SetActive(false);
        }

    }

    public void CreatePlayerBtn()
    {
        playerNameTxt2.gameObject.SetActive(true);
        playerHealthObj.SetActive(true);
        playerHealthObj.GetComponent<Text>().text = "HEALTH: " + health;

        if (playerNameTxt1.text.Length > 0)
        {
            playerNameTxt2.text = "NAME: " + playerNameTxt1.text;
        }
        else
        {
            playerNameTxt2.text = "NAME: PLAYER"; 
        }

        playerNamePanel.SetActive(false);

        instructions.transform.GetChild(1).gameObject.SetActive(true);

        GameObject player = GameObject.FindWithTag("Player");

        if (player) return;

        GameObject newPlayer = Instantiate(playerPrefab) as GameObject;

        newPlayer.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].localPosition + new Vector3(Random.Range(-2.5f , 2.5f) , 0 , Random.Range(-2.5f , 2.5f));

        newPlayer.transform.eulerAngles = new Vector3(0f, Random.Range(0, 359f), 0f);

    }

    public void CreateNPCBtn()
    {
        if (npcNameTxt1.text.Length <= 0) return;

        enemyNamePanel.SetActive(false);

        GameObject[] NPC = GameObject.FindGameObjectsWithTag("NPC");

        if (NPC.Length < 5)
        {
            GameObject npc = Instantiate(npcPrefab[Random.Range(0 , npcPrefab.Length)]) as GameObject;

            npc.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].localPosition + new Vector3(Random.Range(-2.5f, 2.5f), -1.5f, Random.Range(-2.5f, 2.5f));

            npc.GetComponent<NPCScript>().npcName = npcNameTxt1.text;

            npc.transform.eulerAngles = new Vector3(0f, Random.Range(0, 359f), 0f);

        }

        instructions.transform.GetChild(2).gameObject.SetActive(true);
        npcNameTxt1.text = "";
    }

    public void GivePlayerInfo()
    {
        if (playerInfoPanel.activeInHierarchy)
        {
            playerInfoPanel.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            playerInfoPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void DecreaseHealthPoint()
    {
        health--;
        playerHealthObj.GetComponent<Text>().text = "HEALTH: " + health;
    }


} //class
















