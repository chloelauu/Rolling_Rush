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
}
