

public abstract class LifecycleElementNode : LightElementNode
{
    public LifecycleElementNode(string tagName, DisplayType display, ClosingType closing)
        : base(tagName, display, closing) { }
    
    public void Create()
    {
        OnCreated();
    }

    public void Insert(LightElementNode parent)
    {
        parent.Children.Add(this);
        OnInserted();
    }

    public void Remove(LightElementNode parent)
    {
        parent.Children.Remove(this);
        OnRemoved();
    }

    public new string OuterHTML()
    {
        var html = base.OuterHTML();
        OnStylesApplied();
        if (Classes.Count > 0) OnClassListApplied();
        if (Children.OfType<LightTextNode>().Any()) OnTextRendered();
        return html;
    }
    
    protected virtual void OnCreated() { }
    protected virtual void OnInserted() { }
    protected virtual void OnRemoved() { }
    protected virtual void OnStylesApplied() { }
    protected virtual void OnClassListApplied() { }
    protected virtual void OnTextRendered() { }
}


public class LoggedDivNode : LifecycleElementNode
{
    public LoggedDivNode() : base("div", DisplayType.Block, ClosingType.Normal) { }

    protected override void OnCreated() =>
        Console.WriteLine($"[{TagName}] OnCreated");

    protected override void OnInserted() =>
        Console.WriteLine($"[{TagName}] OnInserted");

    protected override void OnRemoved() =>
        Console.WriteLine($"[{TagName}] OnRemoved");

    protected override void OnStylesApplied() =>
        Console.WriteLine($"[{TagName}] OnStylesApplied");

    protected override void OnClassListApplied() =>
        Console.WriteLine($"[{TagName}] OnClassListApplied: {string.Join(", ", Classes)}");

    protected override void OnTextRendered() =>
        Console.WriteLine($"[{TagName}] OnTextRendered");
}