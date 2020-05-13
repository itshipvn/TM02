using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MyPath;

public class MapEditor : Editor
{
    
}

[CreateAssetMenu(fileName = "map", menuName = "MapTools/MapDataObject", order = 1)]
public class MapScriptableObject : ScriptableObject
{
    [SerializeField]
    public Grid<PathNode> map;
}
