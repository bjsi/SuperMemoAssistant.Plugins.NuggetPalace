using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMemoAssistant.Plugins.NuggetPalace.Models
{
  public class Nugget
  {
    public string Url { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }

    public Nugget(string Url)
    {
      this.Url = Url;
      this.Title = Title;
      this.Text = Text;
    }
  }
}
