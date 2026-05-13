using System;

class Program
{
    static void Main(string[] args)
    {
        
        Console.WriteLine("Template Method");
        var div = new LoggedDivNode();
        div.Create();
        div.Classes.Add("container");

        var root = new LightElementNode("body", DisplayType.Block, ClosingType.Normal);
        div.Insert(root);
        div.Children.Add(new LightTextNode("Hello"));
        Console.WriteLine(div.OuterHTML());
        div.Remove(root);

        
        Console.WriteLine("\nIterator (Depth-First)");
        var ul = new LightElementNode("ul", DisplayType.Block, ClosingType.Normal);
        var li1 = new LightElementNode("li", DisplayType.Block, ClosingType.Normal);
        li1.Children.Add(new LightTextNode("Item 1"));
        var li2 = new LightElementNode("li", DisplayType.Block, ClosingType.Normal);
        li2.Children.Add(new LightTextNode("Item 2"));
        ul.Children.Add(li1);
        ul.Children.Add(li2);

        foreach (var node in new HtmlTreeIterable(ul, depthFirst: true))
            Console.WriteLine(node.OuterHTML());

        Console.WriteLine("\nIterator (Breadth-First)");
        foreach (var node in new HtmlTreeIterable(ul, depthFirst: false))
            Console.WriteLine(node.OuterHTML());

      
        Console.WriteLine("\nCommand (Undo/Redo)");
        var editor = new HtmlEditor();
        var p = new LightElementNode("p", DisplayType.Block, ClosingType.Normal);
        var text = new LightTextNode("Hello");

        editor.Execute(new AddChildCommand(p, text));
        editor.Execute(new AddClassCommand(p, "highlight"));
        Console.WriteLine("After commands: " + p.OuterHTML());

        editor.Undo();
        Console.WriteLine("After undo class: " + p.OuterHTML());

        editor.Undo();
        Console.WriteLine("After undo child: " + p.OuterHTML());

        editor.Redo();
        Console.WriteLine("After redo: " + p.OuterHTML());

       
        Console.WriteLine("\nState");
        var btn = new StatefulElement("button", DisplayType.Inline, ClosingType.Normal);
        btn.Children.Add(new LightTextNode("Click me"));

        Console.WriteLine(btn.OuterHTML());
        btn.SetState(new HoverState());
        Console.WriteLine(btn.OuterHTML());
        btn.SetState(new DisabledState());
        Console.WriteLine(btn.OuterHTML());
        
        Console.WriteLine("\nVisitor");
        var counter = new NodeCountVisitor();
        ul.Accept(counter);
        Console.WriteLine($"Elements: {counter.ElementCount}, Texts: {counter.TextCount}");

        var search = new TextSearchVisitor("Item");
        ul.Accept(search);
        Console.WriteLine("Found: " + string.Join(", ", search.Matches));

        var classes = new ClassCollectorVisitor();
        ul.Accept(classes);
        Console.WriteLine("Classes: " + string.Join(", ", classes.AllClasses));
    }
}