namespace COIS2020.MolayoOgunfowora0772346.Assignment3;

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis; // For NotNull attributes
using System.Xml.Linq;


public sealed class Node<T>
{
    public T Item { get; set; }

    // "internal" = only things within `A3-DataStructures` have access (AKA can access within LinkedList, but not from
    // within Program.cs)
    public Node<T>? Next { get; internal set; }
    public Node<T>? Prev { get; internal set; }


    public Node(T item)
    {
        Item = item;
    }
}


public class LinkedList<T> : IEnumerable<T>
{
    public Node<T>? Head { get; protected set; }
    public Node<T>? Tail { get; protected set; }


    public LinkedList()
    {
        Head = null;
        Tail = null;
    }


    // IEnumerable is done for you:

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator(); // Call the <T> version

    public IEnumerator<T> GetEnumerator()
    {
        Node<T>? curr = Head;
        while (curr != null)
        {
            yield return curr.Item;
            curr = curr.Next;
        }
    }


    // This getter is done for you:

    /// <summary>
    /// Determines whether or not this list is empty or not.
    /// </summary>
    [MemberNotNullWhen(false, nameof(Head))] // (these "attributes" tell the `?` thingies that Head and Tail are not
    [MemberNotNullWhen(false, nameof(Tail))] // null whenever this getter returns `false`, which stops the `!` warnings)
    public bool IsEmpty
    {
        get
        {
            bool h = Head == null;
            bool t = Tail == null;
            if (h ^ t) // Can't hurt to do a sanity check while we're here.
                throw new Exception("Head and Tail should either both be null or both non-null.");
            return h;
        }
    }


    // --------------------------------------------------------------
    // Put your code down here:
    // --------------------------------------------------------------

    /*Code to get count of elements in list*/
    public int getCount()
    {
        Node<T>? current = Head;
        int count = 0;
        while (current != null)
        {
            count++;
            current = current.Next;
        }
        return count;
    }
    /* AddFront */
    public void AddFront(T item)
    {
        Node<T> newNode = new Node<T>(item);
        AddFront(newNode);
    }

    public void AddFront(Node<T> newNode)
    {
        if (Head == null)
        {
            Head = newNode;
        }
        else
        {
            Head.Prev = newNode;
            newNode.Next = Head;
            Head = newNode;
        }
    }
    /* AddBack */
    public void AddBack(T item)
    {
        Node<T> newNode = new Node<T>(item);
        AddBack(newNode);
    }

    public void AddBack(Node<T> newNode)
    {
        if (Head == null)
        {
            Head = newNode;
            Tail = newNode;
        }
        else
        {
            Tail.Next = newNode;
            newNode.Prev = Tail;
            Tail = newNode;
        }
    }

    /* InsertAfter */
    public void InsertAfter(Node<T> Node, T item)
    {
        Node<T> newNode = new Node<T>(item);
        InsertAfter(Node, newNode);
    }

    public void InsertAfter(Node<T> Node, Node<T> newNode)
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("List is empty");
        }

        if (Node == null)
        {
            throw new InvalidOperationException("Node cannot be null");
        }

        else if (Node == Tail)
        {
            AddBack(newNode);
        }
        else
        {
            //Add newNode
            newNode.Prev = Node;
            newNode.Next = Node.Next;
            Node.Next = newNode;
            newNode.Next.Prev = newNode;
        }
        

    }

    /* InsertBefore */
    public void InsertBefore(Node<T> Node, T item)
    {
        Node<T> newNode = new Node<T>(item);
        InsertBefore(Node, newNode);
    }
    public void InsertBefore(Node<T> Node, Node<T> newNode)
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("List is empty");
        }
        if (Node == null)
        {
            throw new InvalidOperationException("Node cannot be null");
        }
        else if (Node == Head)
        {
            AddFront(newNode);
        }
        else
        {
            //Add newNode
            newNode.Prev = Node.Prev;
            newNode.Next = Node;
            newNode.Prev.Next = newNode;
            Node.Prev = newNode;
        }
    }

    /* Remove */
    public void Remove(Node<T> node)
    {
        if (node == null)
        {
            throw new InvalidOperationException("Node cannot be null");
        }

        if (node == Head)
        {
            Head = Head.Next;
            if (Head != null)
            {
                Head.Prev = null;
            }
            else
            {
                Tail = null; // If Head is null, Tail should also be null
            }
        }
        else if (node == Tail)
        {
            Tail = Tail.Prev;
            if (Tail != null)
            {
                Tail.Next = null;
            }
            else
            {
                Head = null; // If Tail is null, Head should also be null
            }
        }
        else
        {
            node.Prev.Next = node.Next;
            node.Next.Prev = node.Prev;

        }
    }

    public void Remove(T item)
    {
        Node<T> current = Head;
        EqualityComparer<T> comparer = EqualityComparer<T>.Default;
        while (current != null)
        {
            if (comparer.Equals(current.Item, item))
            {
                Remove(current);
                return;
            }
            current = current.Next;
        }
    }

    /* Splitting the list */
    public LinkedList<T> SplitAfter(Node<T> Node)
    {
        if (Node == null)
        {
            throw new InvalidOperationException("Node cannot be null");
        }
        LinkedList<T> newList = new LinkedList<T>();
        newList.Head = Node.Next;
        newList.Tail = Tail;
        Tail = Node;
        Node.Next = null;
        return newList;
    }

    /* Merging two lists */
    public void AppendAll(LinkedList<T> otherlist)
    {
        if (otherlist.IsEmpty)
        {
            return; //Since there are no nodes to append
        }

        if (IsEmpty)
        {
            Head = otherlist.Head;
            Tail = otherlist.Tail;
        }
        else
        {
            Tail.Next = otherlist.Head;
            otherlist.Head.Prev = Tail;
            Tail = otherlist.Tail; //update Tail
        }

        otherlist.Head = otherlist.Tail = null;
    }

    /* Find element within list */
    public Node<T>? Find(T item)
    {
        Node<T> current = Head;
        EqualityComparer<T> comparer = EqualityComparer<T>.Default;
        while (current != null)
        {
            if (comparer.Equals(current.Item, item))
            {
                return current;
            }
            current = current.Next;
        }
        return null;
    }
}
