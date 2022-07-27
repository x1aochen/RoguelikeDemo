using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// �ѽṹ��������
/// ά��һ��С���ѣ������ڵ�Ϊ������͵�
/// </summary>
/// <typeparam name="T"></typeparam>
public class Heap<T> where T : IHeapItem<T>
{
    //�ѵײ��������
    private T[] items;
    //�Ѵ�С heapSize
    private int heapSize;
    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    public void Add(T item)
    {
        item.HeapIndex = heapSize;
        items[heapSize] = item;
        //ÿ����һ���ڵ�ͽ������ȼ�����
        SortUp(item);
        heapSize++;
    }
    /// <summary>
    /// �Ƴ���һ��
    /// </summary>
    public T RemoveFirst()
    {
        T firstItem = items[0];
        heapSize--;
        items[0] = items[heapSize];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    /// <summary>
    /// �жϵ�ǰ�ڵ��Ƿ����
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool Contains(T item)
    {
        //��ǰ�ڵ����洢�Ķ��±꣬�������ж�Ӧ��ֵ�Ƿ����Լ�
        return Equals(items[item.HeapIndex], item);
    }
    
    /// <summary>
    /// A�ǣ��ҵ����ʹ��۵Ľڵ����и������ȼ�
    /// </summary>
    /// <param name="item"></param>
    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    /// <summary>
    /// �Ѵ�С
    /// </summary>
    public int Count
    {
        get => heapSize;
    }

    //ɾ�������ڵ�ʱ���������򣬴�������Ѱ�Ҷ������������Ƚ�
    private void SortDown(T item)
    {
        while (true)
        {
            int leftIndex = item.HeapIndex * 2 + 1;
            int rightIndex = item.HeapIndex * 2 + 2;
            int tmpIndex = 0;

            //���Ӵ��������
            if (leftIndex < heapSize)
            {
                tmpIndex = leftIndex;
                //�Һ���Ҳ����
                if (rightIndex < heapSize)
                {
                    //�Ƚ�����������,����������ȼ�����
                    if (items[leftIndex].CompareTo(items[rightIndex]) < 0)
                    {
                        tmpIndex = rightIndex;
                    }
                }
                //������ ���ȼ����ߵĺ������Ƚϣ�˭��˭������
                if (item.CompareTo(items[tmpIndex]) < 0)
                {
                    Swap(item, items[tmpIndex]);
                }
                else
                {
                    return;
                }
            }
            //���û���κ��ӽڵ�
            else
                return;
        }
    }


    //���ӽڵ�ʱ���������򣬴��������� �������������Ƚ�
    private void SortUp(T item)
    {
        //���ڵ����� = �Լ������� - 1 �� 2
        int parentIndex = (item.HeapIndex - 1) / 2;

        while (true)
        {
            T parentItem = items[parentIndex];
            //item��ʵ�ֱȽϷ���
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            } else
            {
                break;  
            }
            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    private void Swap(T itemA,T itemB)
    {
        //����������λ�ý���
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;

        //T�����д洢���������ڶ��������Ľ���
        int tmp = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = tmp;
    }

}
//Ҫ�����洢T�����ڶ������������ṩ�ȽϷ���
public interface IHeapItem<T> : IComparable<T>
{
    public int HeapIndex
    {
        get;
        set;
    }
}


