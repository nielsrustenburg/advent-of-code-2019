using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AoC
{
    interface IQueue<T>
    {
        void Enqueue(T item);

        T Dequeue();

        bool Any();
    }

    class NaiveQueue<T> : IQueue<T>
    {
        List<T> queue;
        public int Count { get { return queue.Count; } }

        public NaiveQueue()
        {
            queue = new List<T>();
        }

        public void Enqueue(T item)
        {
            queue.Add(item);
        }

        public T Dequeue()
        {
            if (queue.Any())
            {
                T firstItem = queue.First();
                queue.RemoveAt(0);
                return firstItem;
            }
            throw new Exception("Attempting to dequeue an empty queue");
        }

        public bool Any()
        {
            return queue.Any();
        }
    }
}
