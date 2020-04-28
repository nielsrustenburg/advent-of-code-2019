using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC.Utils
{
    public interface IPriorityQueue<T> where T : IComparable<T>
    {
        bool IsEmpty();

        void Enqueue(T element);

        T Dequeue();
    }

    public class HeapPriorityQueue<T> : IPriorityQueue<T> where T : IComparable<T>
    {
        MinHeap<T> heap;

        public HeapPriorityQueue()
        {
            heap = new MinHeap<T>();
        }

        public T Dequeue()
        {
            return heap.Delete();
        }

        public void Enqueue(T element)
        {
            heap.Insert(element);
        }

        public bool IsEmpty()
        {
            return !heap.Any();
        }
    }

    public class SortedNaivePriorityQueue<T> : IPriorityQueue<T> where T : IComparable<T>
    {
        List<T> queue;

        public SortedNaivePriorityQueue()
        {
            queue = new List<T>();
        }

        public T Dequeue()
        {
            var element = queue.First();
            queue.RemoveAt(0);
            return element;
        }

        public void Enqueue(T element)
        {
            var lastLower = queue.FindLastIndex(other => other.CompareTo(element) == -1);
            queue.Insert(lastLower + 1, element);
        }

        public bool IsEmpty()
        {
            return !queue.Any();
        }
    }

    public class NaivePriorityQueue<T> : IPriorityQueue<T> where T : IComparable<T>
    {
        List<T> unsortedQueue;

        public NaivePriorityQueue()
        {
            unsortedQueue = new List<T>();
        }

        public T Dequeue()
        {
            if (!IsEmpty())
            {
                var (minElement, index) = unsortedQueue.Select((ele, i) => (ele, i)).Aggregate((a, b) => a.ele.CompareTo(b.ele) == 1 ? b : a);
                unsortedQueue.RemoveAt(index);
                return minElement;
            }
            return default(T);
        }

        public void Enqueue(T element)
        {
            unsortedQueue.Add(element);
        }

        public bool IsEmpty()
        {
            return !unsortedQueue.Any();
        }
    }
}
