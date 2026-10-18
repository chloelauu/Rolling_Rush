using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSettings", menuName = "Rolling Rush/Level Settings")]

public class LevelSettings : ScriptableObject
{
    public float forwardForce = 2000f;
    public float sidewaysForce = 500f;
    public float restartDelay = 1f;
    public string levelName = "Level 1";
    public string newHazard = "";

    [Header("Look")]
    public Material trackMaterial;
    public Material obstacleMaterial;
    public Color skyColor = new Color32(0x0F, 0x3B, 0x6E, 255);
}
