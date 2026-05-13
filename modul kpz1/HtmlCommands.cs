

public interface IHtmlCommand
{
    void Execute();
    void Undo();
}


public class AddChildCommand : IHtmlCommand
{
    private readonly LightElementNode _parent;
    private readonly LightNode _child;

    public AddChildCommand(LightElementNode parent, LightNode child)
    {
        _parent = parent;
        _child = child;
    }

    public void Execute() => _parent.Children.Add(_child);
    public void Undo() => _parent.Children.Remove(_child);
}


public class AddClassCommand : IHtmlCommand
{
    private readonly LightElementNode _element;
    private readonly string _className;

    public AddClassCommand(LightElementNode element, string className)
    {
        _element = element;
        _className = className;
    }

    public void Execute() => _element.Classes.Add(_className);
    public void Undo() => _element.Classes.Remove(_className);
}


public class SetTextCommand : IHtmlCommand
{
    private readonly LightTextNode _node;
    private readonly string _newText;
    private string _previousText;

    public SetTextCommand(LightTextNode node, string newText)
    {
        _node = node;
        _newText = newText;
    }

    public void Execute()
    {
        _previousText = _node.Text;
        _node.Text = _newText;
    }

    public void Undo() => _node.Text = _previousText;
}


public class HtmlEditor
{
    private readonly Stack<IHtmlCommand> _history = new();
    private readonly Stack<IHtmlCommand> _redoStack = new();

    public void Execute(IHtmlCommand command)
    {
        command.Execute();
        _history.Push(command);
        _redoStack.Clear();
    }

    public void Undo()
    {
        if (_history.Count == 0) return;
        var cmd = _history.Pop();
        cmd.Undo();
        _redoStack.Push(cmd);
    }

    public void Redo()
    {
        if (_redoStack.Count == 0) return;
        var cmd = _redoStack.Pop();
        cmd.Execute();
        _history.Push(cmd);
    }
}