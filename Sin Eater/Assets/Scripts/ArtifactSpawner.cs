using System;
using UnityEngine;


public class ArtifactSpawner : MonoBehaviour
{ 
    private bool[] usedSpawn;
    
    public GameObject[] artifactList;
    private int specificArtifact;
    public int x;
    private void Awake()
    {
        
    }

    private void SpawnArtifact()
    {

        specificArtifact = x;

        

        if (x == 0 && usedSpawn[0] == false)
        {
            Instantiate(artifactList[0]);

            usedSpawn[0] = true;

        } if(x == 1 && usedSpawn[1] == false)
        {
            Instantiate(artifactList[1]);
            usedSpawn[1] = true;
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


