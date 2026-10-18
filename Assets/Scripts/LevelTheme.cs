using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTheme : MonoBehaviour
{
    public float fogStart = 40f;
    public float fogEnd = 120f;

    void Start()
    {
        LevelSettings s = GetComponent<GameManager>().settings;

        // sky
        Camera.main.clearFlags = CameraClearFlags.SolidColor;
        Camera.main.backgroundColor = s.skyColor;

        // fog fades far-away track into the sky colour
        RenderSettings.fog = true;
        RenderSettings.fogColor = s.skyColor;
        RenderSettings.fogMode = FogMode.Linear;
        RenderSettings.fogStartDistance = fogStart;
        RenderSettings.fogEndDistance = fogEnd;

        // track and obstacles
        if (s.trackMaterial != null)
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Ground"))
                g.GetComponent<Renderer>().sharedMaterial = s.trackMaterial;

        if (s.obstacleMaterial != null)
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Obstacle"))
                g.GetComponent<Renderer>().sharedMaterial = s.obstacleMaterial;
    }
}