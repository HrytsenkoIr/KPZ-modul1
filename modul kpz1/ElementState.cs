

public interface IElementState
{
    string Render(LightElementNode element);
    string StateName { get; }
}


public class NormalState : IElementState
{
    public string StateName => "Normal";

    public string Render(LightElementNode element) => element.OuterHTML();
}


public class HoverState : IElementState
{
    public string StateName => "Hover";

    public string Render(LightElementNode element)
    {
        string classAttr = element.Classes.Count > 0
            ? $" class=\"{string.Join(" ", element.Classes)}\""
            : "";
        string inner = element.InnerHTML();

        return element.Closing == ClosingType.SelfClosing
            ? $"<{element.TagName}{classAttr} style=\"cursor:pointer;\"/>"
            : $"<{element.TagName}{classAttr} style=\"cursor:pointer;\">{inner}</{element.TagName}>";
    }
}


public class DisabledState : IElementState
{
    public string StateName => "Disabled";

    public string Render(LightElementNode element)
    {
        string classAttr = element.Classes.Count > 0
            ? $" class=\"{string.Join(" ", element.Classes)}\""
            : "";
        string inner = element.InnerHTML();

        return $"<{element.TagName}{classAttr} disabled style=\"opacity:0.5;pointer-events:none;\">{inner}</{element.TagName}>";
    }
}


public class StatefulElement : LightElementNode
{
    private IElementState _state;

    public StatefulElement(string tagName, DisplayType display, ClosingType closing)
        : base(tagName, display, closing)
    {
        _state = new NormalState();
    }

    public void SetState(IElementState state)
    {
        _state = state;
        Console.WriteLine($"[{TagName}] State → {state.StateName}");
    }

    public string StateName => _state.StateName;

    public override string OuterHTML() => _state.Render(this);
}