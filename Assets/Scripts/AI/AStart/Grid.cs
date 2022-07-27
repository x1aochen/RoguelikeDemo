using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AI
{
    public class Grid : MonoSingleton<Grid>
    {
        private Tilemap map;
        private Node[,] grid;

        //是否开启网格可视化
        public bool displayGridGizmos;
        //节点直径  可视化用
        private float nodeDiameter;
        //地图长宽
        private int gridSizeX, gridSizeY;
        //tilemap 原点
        private Vector3Int originPos;
        /// <summary>
        /// 面积大小
        /// </summary>
        public int MaxSize => gridSizeX * gridSizeY;
        private bool isWork = false;
        public bool IsWork => isWork;

        /// <summary>
        /// 生成网格地图
        /// </summary>
        /// <param name="index">房间号</param>
        /// <returns>返回地图长宽</returns>
        public Vector2Int GenerateGrid(int index)
        {
            isWork = true;
            if (grid == null)
            {
                grid = new Node[0, 0];
            }

            //找到所需房间的Ground tile
            map = CodeHelper.FindChild(transform.Find(Consts.Room + index), Consts.Ground).GetComponent<Tilemap>();
            return CreadGrid();
        }

        /// <summary>
        /// 初始化地图
        /// </summary>
        //TODO:映射方案是否可优化？
        // ？1.每次绘制地图后，给到四个角一个游戏对象，获取位置后，使用new Bounds
        // ? 2.Tilemap.CompressBounds
        private Vector2Int CreadGrid()
        {
            nodeDiameter = map.cellSize.x;
            //Tilemap的原点
            originPos = map.origin;
            //Tilemap大小
            gridSizeX = map.size.x;
            gridSizeY = map.size.y;

            grid = new Node[gridSizeX, gridSizeY];
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    //得到网格坐标
                    Vector3Int cellPos = new Vector3Int(originPos.x + x, originPos.y + y, 0);
                    //网格坐标转世界坐标
                    Vector3 pos = map.GetCellCenterWorld(cellPos);
                    //得到所对应的Tile
                    TileBase tile = map.GetTile(cellPos);
                    //Tile是否可行走
                    bool walkable = tile != null;
                    grid[x, y] = new Node(walkable, pos, x, y);
                }
            }
            isWork = false;
            return new Vector2Int(gridSizeX, gridSizeY);
        }


        /// <summary>
        /// 返回周围节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;

                    //判断是否越界
                    if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    {
                        neighbours.Add(grid[checkX, checkY]);
                    }
                }
            }

            return neighbours;
        }

        //世界坐标转节点坐标
        public Node NodeFromWorldPoint(Vector3 worldPosition)
        {
            //世界坐标转网格坐标，根据网格坐标得到对应数组下标            
            Vector3Int v3 = map.WorldToCell(worldPosition);
            int x = v3.x - originPos.x;
            int y = v3.y - originPos.y;
            return grid[x, y];

        }

        /// <summary>
        /// 用于生成敌人时判断是否为有效格子
        /// </summary>
        /// <param name="worldPos"></param>
        /// <returns></returns>
        public bool GridWalkable(int x,int y)
        {
            if (x < 0 || x > gridSizeX ||
                y < 0 || y > gridSizeY
                )
                return false;

            return grid[x,y].walkable;
        }

        public Vector3 WorldPosFromNode(int x, int y)
        {
            return grid[x, y].worldPos;
        }

        private void OnDrawGizmos()
        {
            if (grid != null && displayGridGizmos)
            {
                foreach (Node node in grid)
                {
                    Gizmos.color = node.walkable ? Color.white : Color.black;
                    Gizmos.DrawCube(node.worldPos, Vector3.one * (nodeDiameter - 0.1f));
                }
            }
        }
    }
}