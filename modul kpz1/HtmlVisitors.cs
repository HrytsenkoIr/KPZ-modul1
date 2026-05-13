

public interface IHtmlVisitor
{
    void Visit(LightElementNode element);
    void Visit(LightTextNode text);
}


public class NodeCountVisitor : IHtmlVisitor
{
    public int ElementCount { get; private set; }
    public int TextCount { get; private set; }
    public int Total => ElementCount + TextCount;

    public void Visit(LightElementNode element) => ElementCount++;
    public void Visit(LightTextNode text) => TextCount++;
}


public class TextSearchVisitor : IHtmlVisitor
{
    private readonly string _query;
    public List<string> Matches { get; } = new();

    public TextSearchVisitor(string query) { _query = query; }

    public void Visit(LightElementNode element) { }

    public void Visit(LightTextNode text)
    {
        if (text.Text.Contains(_query, System.StringComparison.OrdinalIgnoreCase))
            Matches.Add(text.Text);
    }
}


public class ClassCollectorVisitor : IHtmlVisitor
{
    public HashSet<string> AllClasses { get; } = new();

    public void Visit(LightElementNode element)
    {
        foreach (var cls in element.Classes)
            AllClasses.Add(cls);
    }

    public void Visit(LightTextNode text) { }
}


public static class LightNodeExtensions
{
    public static void Accept(this LightNode node, IHtmlVisitor visitor)
    {
        switch (node)
        {
            case LightElementNode elem:
                visitor.Visit(elem);
                foreach (var child in elem.Children)
                    child.Accept(visitor);
                break;
            case LightTextNode text:
                visitor.Visit(text);
                break;
        }
    }
}