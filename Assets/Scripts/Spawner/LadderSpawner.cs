using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LadderSpawner : MonoBehaviour
{
    public GameObject[] ladderSegmentPrefabs;
    public float segmentHeight = 3f;
    private float nextSpawnHeight;
    public Transform playerTransform;
    public int maxLaddersAbove = 25;
    public float spawnDelay = 0.05f;
    private bool isSpawning = false;
    private List<GameObject> spawnedLadders = new List<GameObject>();
    public int destroyThreshold = 20;

    void Start()
    {
        nextSpawnHeight = transform.position.y + segmentHeight;
    }

    void Update()
    {
        if (ShouldSpawnNextSegment() && !isSpawning)
        {
            StartCoroutine(SpawnNextSegmentCoroutine());
        }

        DestroyOldLadders();
    }

    private bool ShouldSpawnNextSegment()
    {
        int laddersAbovePlayer = Mathf.FloorToInt((nextSpawnHeight - playerTransform.position.y) / segmentHeight);
        return laddersAbovePlayer < maxLaddersAbove;
    }

    private IEnumerator SpawnNextSegmentCoroutine()
    {
        isSpawning = true;

        Vector3 spawnPosition = new Vector3(transform.position.x, nextSpawnHeight, transform.position.z);
        GameObject newSegment = Instantiate(SelectRandomLadderPrefab(), spawnPosition, Quaternion.identity);
        spawnedLadders.Add(newSegment);
        nextSpawnHeight += segmentHeight;

        Debug.Log("Spawned Ladder Segment at " + spawnPosition);

        yield return new WaitForSeconds(spawnDelay);

        isSpawning = false;
    }
    private GameObject SelectRandomLadderPrefab()
    {
        int randomIndex = Random.Range(0, ladderSegmentPrefabs.Length);
        return ladderSegmentPrefabs[randomIndex];
    }
    private void DestroyOldLadders()
    {
        float destroyBelowHeight = playerTransform.position.y - destroyThreshold * segmentHeight;
        for (int i = spawnedLadders.Count - 1; i >= 0; i--)
        {
            if (spawnedLadders[i].transform.position.y < destroyBelowHeight)
            {
                Destroy(spawnedLadders[i]);
                spawnedLadders.RemoveAt(i);
                Debug.Log("Destroyed Ladder Segment at " + destroyBelowHeight);
            }
        }
    }
}
