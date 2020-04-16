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
