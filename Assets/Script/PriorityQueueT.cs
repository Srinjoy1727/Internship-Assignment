using System;
using System.Collections.Generic;

public class PriorityQueue<T> where T : IHeapItem<T>
{
    private List<T> items = new List<T>();

    public int Count => items.Count;

    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }

    public void Enqueue(T item)
    {
        item.HeapIndex = items.Count;
        items.Add(item);
        SortUp(item);
    }

    public T Dequeue()
    {
        if (items.Count == 0)
        {
            throw new InvalidOperationException("Cannot dequeue from an empty priority queue.");
        }

        T firstItem = items[0];
        int lastIndex = items.Count - 1;
        items[0] = items[lastIndex];
        items[0].HeapIndex = 0;
        items.RemoveAt(lastIndex);
        if (items.Count > 0)
        {
            SortDown(items[0]);
        }
        return firstItem;
    }


    private void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;
        while (true)
        {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) < 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }

            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    private void SortDown(T item)
    {
        while (true)
        {
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            if (childIndexLeft < items.Count)
            {
                swapIndex = childIndexLeft;

                if (childIndexRight < items.Count && items[childIndexRight].CompareTo(items[childIndexLeft]) < 0)
                {
                    swapIndex = childIndexRight;
                }

                if (item.CompareTo(items[swapIndex]) > 0)
                {
                    Swap(item, items[swapIndex]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }

    private void Swap(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex { get; set; }
}

