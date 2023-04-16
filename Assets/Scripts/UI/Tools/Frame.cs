using System;
using System.Collections.Generic;

namespace ConnectIt.UI.Tools
{
    public readonly struct Frame<T> : IEquatable<Frame<T>>
    {
        public bool HasParent { get; }

        public T Element { get; }
        public T Parent { get; }

        public Frame(T element)
        {
            Element = element;
            Parent = default;

            HasParent = false;
        }

        public Frame(T element, T parent)
        {
            Element = element;
            Parent = parent;

            HasParent = true;
        }

        public bool Equals(Frame<T> other)
            => EqualityComparer<T>.Default.Equals(Element, other.Element);
    }
}
