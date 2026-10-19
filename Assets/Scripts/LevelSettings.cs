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

    [Header("Spawner")]
    public float levelLength = 500f;    // where the finish line goes
    public float firstRowZ = 40f;       // empty run-up before the first row
    public float rowSpacing = 30f;      // distance between rows (smaller = harder)
    public int minBlocksPerRow = 1;
    public int maxBlocksPerRow = 2;
    [Range(0f, 1f)] public float gateChance = 0f;
    [Range(0f, 1f)] public float sliderChance = 0f;
    [Range(0f, 1f)] public float sweeperChance = 0f;
    [Range(0f, 1f)] public float dropChance = 0f;
    public bool isFinalLevel = false;
}
