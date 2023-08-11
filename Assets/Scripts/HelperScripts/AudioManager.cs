using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] AudioSource playerSource, npcGunSource, npcDeadSource, gameSource;
    [SerializeField] AudioClip gameWon, gameLose, npcRifleShot;
    [SerializeField] AudioClip[] npcDeaths;

    void Awake()
    {
        MakeInstance();
    }

    void MakeInstance()
    {
        if (instance == null) instance = this;
    }

    public void PlayPlayerGunshot()
    {
        playerSource.Play();
    }

    public void PlayNPCGunshot()
    {
        npcGunSource.clip = npcRifleShot;
        npcGunSource.Play();
    }

    public void PlayNPCDeadSound()
    {
        int randDeath = Random.Range(0, npcDeaths.Length);
        npcDeadSource.PlayOneShot(npcDeaths[randDeath]);
        Debug.Log("PLAYED");
    }

    public void PlayGameEndedLine(bool gameWin)
    {
        if (gameWin) gameSource.clip = gameWon;
        else gameSource.clip = gameLose;

        gameSource.Play();
    }

}
