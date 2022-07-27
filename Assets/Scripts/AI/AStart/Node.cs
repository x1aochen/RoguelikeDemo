using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool walkable;
    public Vector3 worldPos;

    //�ڵ�λ�������λ��
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;

    public Node parent;
    private int heapIndex;

    public Node(bool walkable, Vector3 worldPos, int gridX, int gridY)
    {
        this.walkable = walkable;
        this.worldPos = worldPos;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public int HeapIndex
    {
        get => heapIndex;
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node node)
    {
        //ǰ�ߴ��ں��߷���1�����ʾǰ�ߴ��۸���
        int compare = fCost.CompareTo(node.fCost);
        //������ȹ��򣺸������յ��������ж�
        if (compare == 0)
        {
            compare = hCost.CompareTo(node.hCost);
        }
        //��Ҫ���Ǵ��۸��ͣ������ȼ����ߵĽڵ�
        return -compare;
    }

}
