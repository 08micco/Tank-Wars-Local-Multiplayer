using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnPowerUp : MonoBehaviour
{
	public GameObject[] powerups;
	GameObject spawnPositions;
    public float powerUpCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        spawnPositions = GameObject.Find("SpawnPositions");
        InvokeRepeating("SpawnPowerUp", 5.0f, 5.0f);
    }


    void SpawnPowerUp(/*int spawnRate*/)
    {
        if(powerUpCount < 3)
        {
    	int powerupIndex = Random.Range(0, powerups.Length);
    	GameObject powerup = powerups[powerupIndex];
    	print(powerup.name);

    	int spawnPositionCount = spawnPositions.transform.childCount;
    	Transform spawnPosition = spawnPositions.transform.GetChild(Random.Range(0, spawnPositionCount));

    	Instantiate(powerup, spawnPosition.transform);
    	powerUpCount++;
        }
    }
}
