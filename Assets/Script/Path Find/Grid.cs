using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyPath
{
    public class Grid<T>
    {
        #region Variables
        private int width;
        private int height;
        private T[,] gridArr;
        private float cellSize;
        private Vector2 startPos;
        private bool isDebug = false;
        #endregion

        #region Contructor
        public Grid(int width, int height, float cellSize, Vector2 startPos)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.startPos = startPos;
            gridArr = new T[width, height];
            for (int x = 0; x < gridArr.GetLength(0); x++)
                for (int y = 0; y < gridArr.GetLength(1); y++)
                {
                    if (isDebug)
                    {
                        DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white);
                        DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white);
                    }
                    
                }
            if (isDebug)
            {
                DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white);
                DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white);
            }            
        }
        #endregion

        #region Public Methodss
        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }
        public void SetValue(int x, int y, T value)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
                gridArr[x, y] = value;
        }
        public void SetValue(Vector3 worldPoint, T value)
        {
            int x, y;
            GetXY(worldPoint, out x, out y);
            SetValue(x, y, value);
        }

        public T GetValue(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
                return gridArr[x, y];
            else
                return default;
        }

        public T GetValue(Vector3 worldPosition)
        {
            int x, y;
            GetXY(worldPosition, out x, out y);
            if (x >= 0 && y >= 0 && x < width && y < height)
                return gridArr[x, y];
            else
                return default;
        }

        public Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x+startPos.x, y+startPos.y) * cellSize;
        }
        public void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt((worldPosition.x-startPos.x) / cellSize);
            y = Mathf.FloorToInt((worldPosition.y-startPos.y) / cellSize);
        }

        public float CellSize
        {
            get { return cellSize; }
        }

        public T[,] GridArray
        {
            get { return gridArr; }
        }
        #endregion

        #region Private Methods

        #endregion


        public void DrawLine(Vector3 start, Vector3 end, Color color, float duration = -1)
        {
            GameObject myLine = new GameObject();
            myLine.transform.position = start;
            myLine.AddComponent<LineRenderer>();
            LineRenderer lr = myLine.GetComponent<LineRenderer>();
            //lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
            lr.SetColors(color, color);
            lr.SetWidth(0.05f, 0.05f);
            lr.SetPosition(0, start);
            lr.SetPosition(1, end);
            lr.sortingOrder = 10;
            if (duration != -1)
                GameObject.Destroy(myLine, duration);
        }
    }
}

