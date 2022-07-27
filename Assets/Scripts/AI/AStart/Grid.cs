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

        //�Ƿ���������ӻ�
        public bool displayGridGizmos;
        //�ڵ�ֱ��  ���ӻ���
        private float nodeDiameter;
        //��ͼ����
        private int gridSizeX, gridSizeY;
        //tilemap ԭ��
        private Vector3Int originPos;
        /// <summary>
        /// �����С
        /// </summary>
        public int MaxSize => gridSizeX * gridSizeY;
        private bool isWork = false;
        public bool IsWork => isWork;

        /// <summary>
        /// ���������ͼ
        /// </summary>
        /// <param name="index">�����</param>
        /// <returns>���ص�ͼ����</returns>
        public Vector2Int GenerateGrid(int index)
        {
            isWork = true;
            if (grid == null)
            {
                grid = new Node[0, 0];
            }

            //�ҵ����跿���Ground tile
            map = CodeHelper.FindChild(transform.Find(Consts.Room + index), Consts.Ground).GetComponent<Tilemap>();
            return CreadGrid();
        }

        /// <summary>
        /// ��ʼ����ͼ
        /// </summary>
        //TODO:ӳ�䷽���Ƿ���Ż���
        // ��1.ÿ�λ��Ƶ�ͼ�󣬸����ĸ���һ����Ϸ���󣬻�ȡλ�ú�ʹ��new Bounds
        // ? 2.Tilemap.CompressBounds
        private Vector2Int CreadGrid()
        {
            nodeDiameter = map.cellSize.x;
            //Tilemap��ԭ��
            originPos = map.origin;
            //Tilemap��С
            gridSizeX = map.size.x;
            gridSizeY = map.size.y;

            grid = new Node[gridSizeX, gridSizeY];
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    //�õ���������
                    Vector3Int cellPos = new Vector3Int(originPos.x + x, originPos.y + y, 0);
                    //��������ת��������
                    Vector3 pos = map.GetCellCenterWorld(cellPos);
                    //�õ�����Ӧ��Tile
                    TileBase tile = map.GetTile(cellPos);
                    //Tile�Ƿ������
                    bool walkable = tile != null;
                    grid[x, y] = new Node(walkable, pos, x, y);
                }
            }
            isWork = false;
            return new Vector2Int(gridSizeX, gridSizeY);
        }


        /// <summary>
        /// ������Χ�ڵ�
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

                    //�ж��Ƿ�Խ��
                    if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    {
                        neighbours.Add(grid[checkX, checkY]);
                    }
                }
            }

            return neighbours;
        }

        //��������ת�ڵ�����
        public Node NodeFromWorldPoint(Vector3 worldPosition)
        {
            //��������ת�������꣬������������õ���Ӧ�����±�            
            Vector3Int v3 = map.WorldToCell(worldPosition);
            int x = v3.x - originPos.x;
            int y = v3.y - originPos.y;
            return grid[x, y];

        }

        /// <summary>
        /// �������ɵ���ʱ�ж��Ƿ�Ϊ��Ч����
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