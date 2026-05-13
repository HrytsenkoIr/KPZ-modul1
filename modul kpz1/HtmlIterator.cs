
using System.Collections;
using System.Collections.Generic;


public class DepthFirstIterator : IEnumerator<LightNode>
{
    private readonly Stack<LightNode> _stack = new();
    private LightNode _current;

    public DepthFirstIterator(LightNode root)
    {
        _stack.Push(root);
    }

    public LightNode Current => _current;
    object IEnumerator.Current => _current;

    public bool MoveNext()
    {
        if (_stack.Count == 0) return false;

        _current = _stack.Pop();

        if (_current is LightElementNode elem)
        {

            for (int i = elem.Children.Count - 1; i >= 0; i--)
                _stack.Push(elem.Children[i]);
        }

        return true;
    }

    public void Reset() => throw new System.NotSupportedException();
    public void Dispose() { }
}


public class BreadthFirstIterator : IEnumerator<LightNode>
{
    private readonly Queue<LightNode> _queue = new();
    private LightNode _current;

    public BreadthFirstIterator(LightNode root)
    {
        _queue.Enqueue(root);
    }

    public LightNode Current => _current;
    object IEnumerator.Current => _current;

    public bool MoveNext()
    {
        if (_queue.Count == 0) return false;

        _current = _queue.Dequeue();

        if (_current is LightElementNode elem)
            foreach (var child in elem.Children)
                _queue.Enqueue(child);

        return true;
    }

    public void Reset() => throw new System.NotSupportedException();
    public void Dispose() { }
}


public class HtmlTreeIterable : IEnumerable<LightNode>
{
    private readonly LightNode _root;
    private readonly bool _depthFirst;

    public HtmlTreeIterable(LightNode root, bool depthFirst = true)
    {
        _root = root;
        _depthFirst = depthFirst;
    }

    public IEnumerator<LightNode> GetEnumerator() =>
        _depthFirst
            ? new DepthFirstIterator(_root)
            : new BreadthFirstIterator(_root);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}