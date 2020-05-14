using MyPath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    #region Inspector Variables
    public Transform root,start;
    public int width = 6, height = 10;
    public float cellSize = 1;
    public Transform[] box_posisions;
    #endregion

    #region Member Variables
    private Grid<PathNode> map;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        InitMap();
    }


    #endregion

    #region Public Methods
    public Grid<PathNode> PATH
    {
        get {
            if (map == null)
                InitMap();
            return map;
        }
    }
    #endregion

    #region Private
    private void InitMap()
    {
        map = new Grid<PathNode>(width, height, cellSize, root.position);
        for (int x = 0; x < map.GridArray.GetLength(0); x++)
            for (int y = 0; y < map.GridArray.GetLength(1); y++)
            {
                map.GridArray[x, y] = new PathNode(x, y);
            }
        for (int i = 0; i < box_posisions.Length; i++)
        {
            map.GetXY(box_posisions[i].position, out int x, out int y);
            map.GridArray[x, y].isWalkable = false;
        }
    }
    #endregion
}
