using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{

    public GameObject spawnPoint;
    public List<GameObject> itemPrefabs;

    public float speed = 1.0f;
    public float spawnWait = 5.0f; // time in seconds to wait before spawning new item
    private int spawnFrames;


    private void Start()
    {
        spawnFrames = 0;
    }
    public void SpawnItem()
    {
        int randomIndex = Random.Range(0, itemPrefabs.Count);
        GameObject item = Instantiate(itemPrefabs[randomIndex]);
        item.transform.position = spawnPoint.transform.position;
        ItemControl projControl = item.GetComponent<ItemControl>();
        projControl.SetSpeed(speed);
    }

    private void FixedUpdate()
    {
        if (spawnFrames / 50 > spawnWait)
        {
            spawnFrames = 0;
            SpawnItem();
        }
        spawnFrames++;

    }
}
