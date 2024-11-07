using System;
using UnityEngine;


public class ArtifactSpawner : MonoBehaviour
{
    private Transform spawnLocation;


    private bool usedSpawn;
    


    public GameObject[] artifactList = new GameObject[5];
    private int specificArtifact;
    public int x;

    private void SpawnArtifact()
    {

        specificArtifact = x;
        if (x == 0 && usedSpawn == false)
        {
            Instantiate(artifactList[0], spawnLocation.position, Quaternion.identity);
            
            usedSpawn = true;

        } if(x == 1 && usedSpawn == false)
        {
            Instantiate(artifactList[1], spawnLocation.position, Quaternion.identity);
            usedSpawn = true;
        }
    }
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        


    }
   
}


