using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public enum GameMode
{
    FREE, WAR , END
}

public class GameplayController : MonoBehaviour
{
    public static GameplayController instance;

    [SerializeField] GameObject createPanel;
    [SerializeField] GameObject createBtn;

    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject[] npcPrefab;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject playerNamePanel, enemyNamePanel;
    [SerializeField] Text playerNameTxt1, playerNameTxt2, npcNameTxt1, npcNameTxt2;
    [SerializeField] GameObject playerHealthObj;
    [SerializeField] Text playerTraitTxt;
    [SerializeField] GameObject playerInfoPanel;
    [SerializeField] GameObject npcInfoPanel;
    [SerializeField] Text npcName, npcHealth, npcTrait;
    [SerializeField] GameObject gameEndedPanel;
    public float health = 20;
    public bool checkAllies;
    GameObject curNPC;
    public GameMode gameMode = GameMode.FREE;

    GameObject instructions;

    void Awake()
    {
        MakeInstance();

        instructions = GameObject.Find("Instructions");
    }

    void Update()
    {
        CheckAllies();
    }

    void MakeInstance()
    {
        if (instance == null) instance = this;
    }

    void CheckAllies()
    {
        if (checkAllies)
        {
            GameObject allies = GameObject.Find("Allies");
            //Debug.Log("CHECKED");
            for (int j = 0; j < allies.transform.childCount; j++)
            {
                if (!allies.transform.GetChild(j).gameObject.activeInHierarchy)
                {
                    allies.transform.GetChild(j).gameObject.SetActive(true);
                    allies.transform.GetChild(j).GetComponent<Text>().text = "ALLY " + (j + 1) + ": " + curNPC.GetComponent<NPCScript>().npcName;
                    checkAllies = false;
                    break;
                }
            }
        }
    }


    public void CreateBtn()
    {
        //Debug.Log("PRESSED");
        createPanel.SetActive(!createPanel.activeInHierarchy);

    }

    public void ActivatePlayerPanel()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player) return;

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

        newPlayer.GetComponent<PlayerScript>().playerStatus.playerName = playerNameTxt2.text;
        newPlayer.GetComponent<PlayerScript>().playerStatus.playerHealth = health;
        newPlayer.GetComponent<PlayerScript>().playerStatus.playerTraits = (Random.Range(0, 3) == 0) ? Traits.KILLER : Traits.SOLDIER;

        playerTraitTxt.text = "TRAIT: " + newPlayer.GetComponent<PlayerScript>().playerStatus.playerTraits.ToString();

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

            //npc.transform.eulerAngles = new Vector3(0f, Random.Range(0, 359f), 0f);

            npc.GetComponent<NPCScript>().npcStatus.playerTraits = (Random.Range(0, 2) == 0) ? Traits.SOLDIER : Traits.KILLER;

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

    public void GiveNPCInfo()
    {
        if (GameplayController.instance.gameMode == GameMode.WAR) return;

        if (npcInfoPanel.activeInHierarchy)
        {
            npcInfoPanel.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            npcInfoPanel.SetActive(true);
            //playerTraitTxt.text = "TRAIT: " + 
            Time.timeScale = 0;

            GameObject[] friends = GameObject.FindGameObjectsWithTag("NPC");
            int noOfFriends = 0;

            foreach (GameObject friend in friends)
            {
                if(friend.GetComponent<NPCScript>().npcType == Type.FRIEND)
                {
                    noOfFriends++;
                }
            }

            if(noOfFriends >= 3)
            {
                npcInfoPanel.transform.GetChild(4).gameObject.SetActive(false);
                instructions.transform.GetChild(3).gameObject.SetActive(true);
            }
        }
    }

    public void SetNPCStatus(GameObject npc)
    {
        npcName.text = "NAME: " + npc.GetComponent<NPCScript>().npcName.ToString();
        npcHealth.text = "HEALTH: " + npc.GetComponent<NPCScript>().npcHealth;
        npcTrait.text = "TRAIT: " + npc.GetComponent<NPCScript>().npcStatus.playerTraits;
    }

    public void RecruitBtn()
    {
        npcInfoPanel.SetActive(false);
        Time.timeScale = 1;

        GameObject[] NPC = GameObject.FindGameObjectsWithTag("NPC");
        GameObject player = GameObject.FindWithTag("Player");

        for (int i = 0; i < NPC.Length; i++)
        {
            if (NPC[i].GetComponent<NPCScript>().nearestEnemy)
            {
               
                if(NPC[i].GetComponent<NPCScript>().npcStatus.playerTraits == player.GetComponent<PlayerScript>().playerStatus.playerTraits)
                {
                    Debug.Log("TRAITS MATCHED");
                    NPC[i].GetComponent<NPCScript>().nearestEnemy = false;
                    NPC[i].GetComponent<NPCScript>().npcType = Type.FRIEND;
                    NPC[i].transform.Find("AllyIcon").gameObject.SetActive(true);
                    //NPC[i].GetComponent<NPCScript>().targetLayer = 9;
                    curNPC = NPC[i].gameObject;
                    checkAllies = true;
                }
                else
                {
                    Debug.Log("TRAITS UNMATCHED");
                    DecreaseHealthPoint(2);
                }
            }
        }


    }

    public void DecreaseHealthPoint(float decrement)
    {
        health -= decrement;
        playerHealthObj.GetComponent<Text>().text = "HEALTH: " + (int)health;
    }

    public void StartBtn()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player && gameMode == GameMode.FREE)
        {
            createPanel.SetActive(false);
            createBtn.SetActive(false);
            gameMode = GameMode.WAR;
            int numOfEnemies = Random.Range(5, 9);

            for (int i = 0; i < numOfEnemies; i++)
            {
                GameObject enemy = Instantiate(npcPrefab[Random.Range(0, npcPrefab.Length)]) as GameObject;

                enemy.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].localPosition + new Vector3(Random.Range(-2.5f, 2.5f), -1.5f, Random.Range(-2.5f, 2.5f));

                enemy.GetComponent<NPCScript>().npcType = Type.ENEMY;

                enemy.GetComponent<NPCScript>().playerDest = player.transform.position;

                enemy.GetComponent<NavMeshAgent>().stoppingDistance = Random.Range(6f, 10f);

                enemy.gameObject.tag = "Enemy";

                enemy.GetComponent<NPCScript>().changeNPCTarget = true;

                enemy.layer = 9;
            }

            GameObject[] friends = GameObject.FindGameObjectsWithTag("NPC");

            foreach (GameObject friend in friends)
            {
                friend.GetComponent<NPCScript>().changeNPCTarget = true;

                if(friend.GetComponent<NPCScript>().npcType == Type.NPC)
                {
                    friend.gameObject.SetActive(false);
                }
            }
            
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void ActivateGameEndedPanel(string endingTxt)
    {
        gameEndedPanel.SetActive(true);
        gameEndedPanel.transform.GetChild(1).GetComponent<Text>().text = endingTxt;
        Time.timeScale = 0;
    }

} //class
















