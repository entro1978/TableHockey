using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.ObjectModel;

namespace TableHockey
{
    public class GenericTreeNode<T>
    {
        private readonly T _value;
        private readonly List<GenericTreeNode<T>> _children = new List<GenericTreeNode<T>>();
        private readonly int m_nLevel;

        public GenericTreeNode(T value, int i_nLevel)
        {
            _value = value;
            m_nLevel = i_nLevel;
        }

        public GenericTreeNode<T> this[int i]
        {
            get { return _children[i]; }
        }

        public GenericTreeNode<T> Parent { get; private set; }

        public T Value { get { return _value; } }

        public ReadOnlyCollection<GenericTreeNode<T>> Children
        {
            get { return _children.AsReadOnly(); }
        }

        public GenericTreeNode<T> AddChild(T value, int i_nLevel)
        {
            var node = new GenericTreeNode<T>(value, i_nLevel) { Parent = this };
            _children.Add(node);
            return node;
        }

        public GenericTreeNode<T>[] AddChildren(params T[] values)
        {
            return values.Select(AddChild).ToArray();
        }

        public bool RemoveChild(GenericTreeNode<T> node)
        {
            return _children.Remove(node);
        }

        public void Traverse(Action<T> action)
        {
            action(Value);
            foreach (var child in _children)
                child.Traverse(action);
        }

        public IEnumerable<T> Flatten()
        {
            return new[] { Value }.Union(_children.SelectMany(x => x.Flatten()));
        }
    }
}