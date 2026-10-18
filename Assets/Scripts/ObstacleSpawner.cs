using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public GameObject sliderPrefab;
    public float[] lanes = { -6f, -3f, 0f, 3f, 6f };  // x positions across the track
    public float obstacleY = 1f;                       // height to place blocks at

    // Awake runs before LevelTheme's Start, so the new blocks get the level's colours
    void Awake()
    {
        LevelSettings s = GetComponent<GameManager>().settings;

        for (float z = s.firstRowZ; z < s.levelLength; z += s.rowSpacing)
            SpawnRow(z, s);

        // move the finish line to the end of the level
        EndTrigger end = FindObjectOfType<EndTrigger>();
        if (end != null)
        {
            Vector3 p = end.transform.position;
            p.z = s.levelLength;
            end.transform.position = p;
        }
    }

    void SpawnRow(float z, LevelSettings s)
    {
        float roll = Random.value;

        // slider row: one block in the middle that slides side to side
        if (roll < s.sliderChance)
        {
            Instantiate(sliderPrefab, new Vector3(0f, obstacleY, z), Quaternion.identity);
            return;
        }

        // gate row: every lane blocked except one
        int count;
        if (roll < s.sliderChance + s.gateChance)
            count = lanes.Length - 1;
        else
            count = Random.Range(s.minBlocksPerRow, s.maxBlocksPerRow + 1);

        count = Mathf.Min(count, lanes.Length - 1);   // always leave at least one gap

        // shuffle the lane order, then fill the first `count` lanes
        int[] order = new int[lanes.Length];
        for (int i = 0; i < order.Length; i++) order[i] = i;
        for (int i = order.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (order[i], order[j]) = (order[j], order[i]);
        }

        for (int i = 0; i < count; i++)
            Instantiate(obstaclePrefab, new Vector3(lanes[order[i]], obstacleY, z), Quaternion.identity);
    }
}