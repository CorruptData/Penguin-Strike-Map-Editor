using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Codes.Linus.IntVectors;


//Sav
[Serializable]
public class Map
{
    public List<Block> Blocks = new List<Block>();
    public List<Ramp> Ramps = new List<Ramp>();
    public List<Rampart> Ramparts = new List<Rampart>();
    public List<Turret> Turrets = new List<Turret>();
    public List<Fire> Fires = new List<Fire>();
    public List<Spawn> Spawns = new List<Spawn>();
    public int Width;
    public int Height;
    public string Name;
    public string Description;
    public int MinPlayers;
    public int MaxPlayers;
}

//Data for saving for each block
[Serializable]
public class Block
{
    public string material;
    public Vector3i coords;
}

[Serializable]
public class Ramp
{
    public string material;
    public string direction;
    public Vector3i coords;
}

[Serializable]
public class Rampart
{
    public string material;
    public string direction;
    public Vector3i coords;
}

[Serializable]
public class Turret
{
    public string size;
    public string direction;
    public string team;
    public Vector3i coords;
}

[Serializable]
public class Fire
{
    public string team;
    public Vector3i coords;
}

[Serializable]
public class Spawn
{
    public string team;
    public Vector3i coords;
}
