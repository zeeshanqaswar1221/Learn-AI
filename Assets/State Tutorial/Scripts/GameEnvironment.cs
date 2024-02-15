using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameEnvironment
{
    private static GameEnvironment instance;
    public List<GameObject> CheckPoints {  get; private set; } = new List<GameObject>();
    
    
    public static GameEnvironment Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameEnvironment();
                instance.CheckPoints.AddRange(GameObject.FindGameObjectsWithTag("Checkpoint"));
            }

            return instance;
        }
    }
}
