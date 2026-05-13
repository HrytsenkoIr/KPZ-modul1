using System.Collections.Generic;
using System.Linq;

public enum DisplayType { Block, Inline }
public enum ClosingType { SelfClosing, Normal }

public class LightElementNode : LightNode
{
    public string TagName { get; set; }
    public DisplayType Display { get; set; }
    public ClosingType Closing { get; set; }
    public List<string> Classes { get; set; } = new();
    public List<LightNode> Children { get; set; } = new();

    public LightElementNode(string tagName, DisplayType display, ClosingType closing)
    {
        TagName = tagName;
        Display = display;
        Closing = closing;
    }

    public string InnerHTML() => string.Join("", Children.Select(c => c.OuterHTML()));

    public override string OuterHTML()
    {
        string classAttr = Classes.Count > 0 ? $" class=\"{string.Join(" ", Classes)}\"" : "";
        return Closing == ClosingType.SelfClosing
            ? $"<{TagName}{classAttr}/>"
            : $"<{TagName}{classAttr}>{InnerHTML()}</{TagName}>";
    }
}