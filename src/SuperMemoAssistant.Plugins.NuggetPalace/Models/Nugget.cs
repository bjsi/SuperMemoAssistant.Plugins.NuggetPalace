using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMemoAssistant.Plugins.NuggetPalace.Models
{

  public class Child
  {

    public string Question { get; set; }
    public string Answer { get; set; }

  }

  public class Nugget
  {

    public string Url { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Source { get; set; }
    public string Text { get; set; }
    public List<Child> Children { get; set; } = new List<Child>();

    public Nugget(string url, string title, string author, string source, string text, List<Child> children)
    {

      Url = url;
      Title = title;
      Author = author;
      Source = source;
      Text = text;
      Children = children;

    }
  }
}
