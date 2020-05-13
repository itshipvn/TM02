using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyPath
{
    public class PathNode
    {
        #region Variables
        public bool isWalkable;
        public int x;
        public int y;
        public int gCost;
        public int hCost;
        public int fCost;
        public PathNode cameFromNode;
        #endregion

        #region Contructor
        public PathNode(int x, int y, bool isWalkable = true)
        {
            this.x = x;
            this.y = y;
            this.isWalkable = isWalkable;
        }
        #endregion

        #region Public Methods
        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }
        public override string ToString()
        {
            return x + "," + y;
        }
        #endregion
    }
}

