using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyPath
{
    public class PathFind
    {
        #region Variables
        private Grid<PathNode> grid;
        private List<PathNode> openList;
        private List<PathNode> closedList;
        private bool find8direct;
        #endregion

        #region Contructor
        public PathFind(Grid<PathNode> map)
        {
            grid = map;
        }
        public Grid<PathNode> GetGrid()
        {
            return grid;
        }
        #endregion


        #region Public Method
        public List<Vector3> Find(Vector2 start, Vector2 end, bool find8direct = true)
        {
            grid.GetXY(start, out int startX, out int startY);
            grid.GetXY(end, out int endX, out int endY);
            this.find8direct = find8direct;
            List<PathNode> paths = Find(startX, startY, endX, endY);
            if(paths == null)
            {
                return null;
            }
            else
            {
                List<Vector3> vectorPath = new List<Vector3>();
                foreach(PathNode pathNode in paths)
                {
                    vectorPath.Add( grid.GetWorldPosition(pathNode.x, pathNode.y) * grid.CellSize + Vector3.one * grid.CellSize * 0.5f);
                }
                return vectorPath;
            }
        }
        public List<PathNode> Find(int startX, int startY, int endX, int endY)
        {
            PathNode startNode = grid.GetValue(startX, startY);
            PathNode endNode = grid.GetValue(endX, endY);
            openList = new List<PathNode>() { startNode};
            closedList = new List<PathNode>();
            for(int x =0;x<grid.Width; x++)
                for(int y=0;y < grid.Height; y++)
                {
                    PathNode pathNode = grid.GetValue(x, y);
                    pathNode.gCost = int.MaxValue;
                    pathNode.cameFromNode = null;
                }
            startNode.gCost = 0;
            startNode.hCost = CalculateDistance(startNode, endNode);
            startNode.CalculateFCost();

            while(openList.Count > 0)
            {
                PathNode cNode = GetLowerFCostNode(openList);
                // Reached final node
                if (cNode == endNode)
                    return CalculatePath(endNode);
                openList.Remove(cNode);
                closedList.Add(cNode);
                foreach(PathNode neighbourNode in GetNeighbourList(cNode))
                {
                    if (closedList.Contains(neighbourNode)) continue;
                    if (!neighbourNode.isWalkable)
                    {
                        closedList.Add(neighbourNode);
                        continue;
                    }
                    int tentativeGCost = cNode.gCost + CalculateDistance(cNode, neighbourNode);
                    if(tentativeGCost < neighbourNode.gCost)
                    {
                        neighbourNode.cameFromNode = cNode;
                        neighbourNode.gCost = tentativeGCost;
                        neighbourNode.hCost = CalculateDistance(neighbourNode, endNode);
                        neighbourNode.CalculateFCost();

                        if (!openList.Contains(neighbourNode))
                            openList.Add(neighbourNode);
                    }
                }
            }

            // out of open list
            return null;
        }
        #endregion

        #region Private Methods
        private List<PathNode> GetNeighbourList(PathNode cNode)
        {
            List<PathNode> neighbourList = new List<PathNode>();
            if(cNode.x -1 >= 0)
            {
                // left
                neighbourList.Add(GetNode(cNode.x - 1, cNode.y));
                if (find8direct)
                {
                    // left down
                    if (cNode.y - 1 >= 0)
                        neighbourList.Add(GetNode(cNode.x - 1, cNode.y - 1));
                    // left up
                    if (cNode.y + 1 < grid.Height)
                        neighbourList.Add(GetNode(cNode.x - 1, cNode.y + 1));
                }                
            }

            if(cNode.x +1 < grid.Width)
            {
                // right
                neighbourList.Add(GetNode(cNode.x + 1, cNode.y));
                if (find8direct)
                {
                    // right down
                    if (cNode.y - 1 >= 0)
                        neighbourList.Add(GetNode(cNode.x + 1, cNode.y - 1));
                    // left up
                    if (cNode.y + 1 < grid.Height)
                        neighbourList.Add(GetNode(cNode.x + 1, cNode.y + 1));
                }                
            }

            if (cNode.y - 1 >= 0)
            {
                // down
                neighbourList.Add(GetNode(cNode.x, cNode.y - 1));
            }

            if (cNode.y + 1 < grid.Height)
            {
                // up
                neighbourList.Add(GetNode(cNode.x, cNode.y + 1));
            }

            return neighbourList;
        }
        private List<PathNode> CalculatePath(PathNode endNode)
        {
            List<PathNode> path = new List<PathNode>();
            path.Add(endNode);
            PathNode cNode = endNode;
            while(cNode.cameFromNode != null)
            {
                path.Add(cNode.cameFromNode);
                cNode = cNode.cameFromNode;
            }
            path.Reverse();
            return path;
        }

        private const int MOVE_STARIGHT_COST = 10;
        private const int MOVE_DIAGNOAL_COST = 14;
        private int CalculateDistance(PathNode a, PathNode b)
        {
            int xDist = Mathf.Abs(a.x - b.x);
            int yDist = Mathf.Abs(a.y - b.y);
            int remaining = Mathf.Abs(xDist - yDist);
            return MOVE_DIAGNOAL_COST * Mathf.Min(xDist, yDist) + MOVE_STARIGHT_COST * remaining;
        }

        private PathNode GetLowerFCostNode(List<PathNode> pathNodeList)
        {
            PathNode lowerFCostNode = pathNodeList[0];
            for(int i=1;i< pathNodeList.Count; i++)
            {
                if(pathNodeList[i].fCost < lowerFCostNode.fCost)
                {
                    lowerFCostNode = pathNodeList[i];
                }
            }
            return lowerFCostNode;
        }

        private PathNode GetNode(int x, int y)
        {
            return grid.GetValue(x, y);
        }
        #endregion
    }

}

