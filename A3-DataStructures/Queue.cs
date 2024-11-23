namespace COIS2020.MolayoOgunfowora0772346.Assignment3;

using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Queue<T> : IEnumerable<T>
{
    public const int DefaultCapacity = 8;

    private T?[] buffer;
    private int start;
    private int end;

    public Queue() : this(DefaultCapacity)
    { }

    public Queue(int capacity)
    {
        // Your code here...
        buffer = new T[capacity];
        start = end = -1;
    }

    // Your code here...
    public bool IsEmpty
    {
        get { return start == -1 && end == -1; }
    }

    public int Count
    {
        get
        {
            if(IsEmpty)
            {
                return 0;
            }
            if (end >= start)
            {
                return end - start + 1;
            }
            else if (start > end)
            {
                return buffer.Length - start + end + 1;
            }
            else
            {//if its full
                return buffer.Length;
            }
        }
    }
    public int Capacity
    {
        get { return buffer.Length; }
    }
    protected void Grow()
    {
        T?[] newBuffer = new T[Capacity * 2];
        //loop to add copy each element in old array to newArray
        for (int i = 0; i < Count; i++)
        {
            newBuffer[i] = buffer[(start + i) % Capacity]; //add each element based on offset from start index in former array
        }
        buffer = newBuffer;
        //since our queue has been rearranged to start from 0 again
        start = 0;
        end = Count - 1;

    }

    public void Enqueue(T item)
    {
        //determine if queue is empty
        if (start == -1 && end == -1)
        {
            start = end = 0;
            buffer[end] = item; //add item to array
        }
        //determine if queue is full
        else if (((end + 1) % Capacity) == start)
        {
            Grow();
            end = (end + 1) % Capacity; //increment end
            buffer[end] = item; //add item to array
        }
        //Add item
        else
        {
            end = (end + 1) % Capacity; //increment end
            buffer[end] = item; //add item to array
        }
    }

    public T Dequeue()
    {
        //determine if queue is empty
        if (IsEmpty)
        {
            throw new InvalidOperationException("Queue is empty");
        }

        T element = buffer[start]; //capture element
        buffer[start] = default(T);
        //determine dequeue when there is only one element
        if (start == end)
        {
            start = end = -1; //initialize to default(empty array)
        }

        //determine when there are more than one elements
        else
        {
            start = (start + 1) % Capacity; //increment start
        }
        return element;
    }

    public T Peek()
    {
        //determine if queue is empty
        if (IsEmpty)
        {
            throw new InvalidOperationException("Queue is empty");
        }

        T element = buffer[start];
        return element;
    }

    // IEnumerable<T> implementation
    public IEnumerator<T> GetEnumerator()
    {
        if (IsEmpty)
        {
            yield break;
        }
        for (int i = 0; i < Count; i++)
        {
            yield return buffer[(start + i) % Capacity];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
