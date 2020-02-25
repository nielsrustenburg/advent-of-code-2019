using System;
using System.Collections.Generic;
using System.Text;

namespace AoC
{
    interface ITreeNode<T> where T : ITreeNode<T>
    {
        T Parent();
        List<T> Children();
        int Depth();
    }

    interface ILabeled<T>
    {
        T Label();
    }

    abstract class TreaNode<T> : ITreeNode<T> where T : TreaNode<T>
    {
        T parent;
        List<T> children;
        readonly string id;
        int depth;

        public TreaNode(string id)
        {
            this.id = id;
            depth = 0;
            children = new List<T>();
        }

        public string GetId()
        {
            return id;
        }

        public void SetParent(T parent)
        {
            this.parent = parent;
        }

        public void AddChild(T child)
        {
            children.Add(child);
            child.SetParent((T) this);
            child.UpdateDepth(depth + 1);
        }

        public void UpdateDepth(int newDepth)
        {
            depth = newDepth;
            foreach(T child in children)
            {
                child.UpdateDepth(newDepth + 1);
            }
        }

        public int Depth()
        {
            return depth;
        }

        public T Parent()
        {
            return parent;
        }

        public List<T> Children()
        {
            return children;
        }
    }
}
