using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyPath;
using Newtonsoft.Json;
using DG.Tweening;

public class TestMap : MonoBehaviour
{
    public Transform character;

    private Grid<PathNode> map;
    private PathFind find;
    Vector2 startpoint;
    bool isMoving = false;
    float speed = 0.5f; // second/unit(cellSize)
    void Start()
    {
        // Init map
        map = new Grid<PathNode>(6, 10, 1, new Vector2(-2.8f, -5));
        for (int x = 0; x < map.GridArray.GetLength(0); x++)
            for (int y = 0; y < map.GridArray.GetLength(1); y++)
            {
                map.GridArray[x, y] = new PathNode(x,y);
            }
        string map_data = JsonConvert.SerializeObject(map);
        //Debug.Log(map_data);
        find = new PathFind(map);
        startpoint = map.GetWorldPosition(0, 0);
        character.position = startpoint;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            Vector2 touch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            map.GetXY(touch, out int x, out int y);
            //Debug.Log("Map(" + x + "," + y + ")");
            List<Vector3> points = find.Find(startpoint, touch);
            if (points == null || points.Count == 0)
            {
                isMoving = false;
                return;
            }                
            isMoving = true;
            //for(int i = 0; i < points.Count - 1; i++)
            //{
            //    map.DrawLine(points[i], points[i + 1], Color.blue, 0.5f);
            //}   
            startpoint = points[0];            
            points.RemoveAt(0);
            MoveTask(points);
        }
    }


    private void MoveTask(List<Vector3> points)
    {
        if (points.Count == 0)
        {
            isMoving = false;
            return;
        }
        for (int i = 0; i < points.Count - 1; i++)
        {
            map.DrawLine(points[i], points[i + 1], Color.blue, 0.5f);
        }
        character.position = startpoint;
        character.DOMove(points[0], speed).SetEase(Ease.Linear).OnUpdate(()=> {
            map.DrawLine(character.position, points[0], Color.blue, 0.05f);
        }).OnComplete(() =>
        {
            startpoint = points[0];
            points.RemoveAt(0);
            MoveTask(points);
        });
    }
   
}
