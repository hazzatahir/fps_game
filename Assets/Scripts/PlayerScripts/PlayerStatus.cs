using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Traits
{
    KILLER,
    SOLDIER
}

[System.Serializable]
public class PlayerStatus
{
    public string playerName;
    public float playerHealth;
    public Traits playerTraits;
}
