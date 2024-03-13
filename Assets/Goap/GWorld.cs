using System.Collections;
using UnityEngine;

public sealed class GWorld
{
    private  static readonly GWorld instance = new GWorld();
    public static GWorld Instance { get { return instance;} }

    public static WorldState World;

    static GWorld()
    {
        World = new WorldState();
    }

    private GWorld() { }




}