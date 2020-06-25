using HtmlAgilityPack;
using SuperMemoAssistant.Extensions;
using SuperMemoAssistant.Interop.SuperMemo.Content.Controls;
using SuperMemoAssistant.Interop.SuperMemo.Elements.Types;
using SuperMemoAssistant.Plugins.NuggetPalace.Models;
using SuperMemoAssistant.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SuperMemoAssistant.Plugins.NuggetPalace.UI
{

  public class ShorterTitle : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is IElement)
      {
        var element = value as IElement;
        string title = element.Title;
        return title.Length > 30
          ? title.Substring(0, 30) + "..."
          : title;
      }
      return "Error: value is not of type IElement";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }

  public class CheckboxForItemsOnly : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is IElement)
      {
        return (value as IElement).Type == Interop.SuperMemo.Elements.Models.ElementType.Item
          ? Visibility.Visible
          : Visibility.Hidden;
      }
      return "Error: value is not of type IElement";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }

  /// <summary>
  /// Interaction logic for NuggetPalaceWdw.xaml
  /// </summary>
  public partial class NuggetPalaceWdw : Window
  {
    public string Url { get; set; }
    public string NuggetTitle { get; set; }
    public string Author { get; set; }
    public string Text { get; set; }
    public string Source { get; set; }
    public ObservableCollection<IElement> RootElement { get; set; }
    public List<IElement> SelectedItems = new List<IElement>();


    public NuggetPalaceWdw(string content, int elementId)
    {

      if (string.IsNullOrEmpty(content))
        return;

      var root = Svc.SM.Registry.Element[elementId];
      if (root == null)
        return;

      RootElement = new ObservableCollection<IElement> { new ElementWrapper(root) };

      References references = ReferenceParser.GetReferences(content);
      if (references == null)
        return;

      Url = references.Link ?? string.Empty;
      NuggetTitle = references.Title ?? string.Empty;
      Author = references.Author ?? string.Empty;
      Source = references.Source ?? string.Empty;

      InitializeComponent();

      Closing += NuggetPalaceWdw_Closing;

      Svc<NuggetPalacePlugin>.Plugin.WdwOpen = true;
      DataContext = this;
    }

    private void NuggetPalaceWdw_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      Svc<NuggetPalacePlugin>.Plugin.WdwOpen = false;
    }

    private async void SendBtn_Click(object sender, RoutedEventArgs e)
    {
      List<Child> children = new List<Child>();
      if (SelectedItems != null && SelectedItems.Count > 0)
      {
        foreach (var item in SelectedItems)
        {

          Child child = new Child();
          child.Question = GetElementQuestion(item);
          child.Answer = GetElementAnswer(item);
          children.Add(child);

        }
      }
      await Svc<NuggetPalacePlugin>.Plugin.service.PostNuggetAsync(Url, NuggetTitle, Author, Source, Text, children);
      Close();
    }

    private string GetInnerText(string html)
    {
      if (string.IsNullOrEmpty(html))
        return string.Empty;

      var doc = new HtmlDocument();
      doc.LoadHtml(html);
      return doc.DocumentNode.InnerText ?? string.Empty;
    }

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }

    private string GetElementAnswer(IElement element)
    {
      if (element == null)
        return string.Empty;

      if (element.Type != Interop.SuperMemo.Elements.Models.ElementType.Item)
        return string.Empty;

      string answer = string.Empty;
      var components = element.ComponentGroup;
      if (components != null && components.Count > 0)
      {
        for (int i = 0; i < components.Count; i++)
        {
          var htmlComp = components[i].AsWeb();
          if (htmlComp == null)
            continue;
          if (htmlComp.DisplayAt == Interop.SuperMemo.Content.Models.AtFlags.NonQuestion)
          {
            answer = htmlComp.Text?.Value ?? string.Empty;
            break;
          }
        }
      }
      return GetInnerText(answer);
    }

    private string GetElementQuestion(IElement element)
    {
      if (element == null)
        return string.Empty;

      if (element.Type != Interop.SuperMemo.Elements.Models.ElementType.Item)
        return string.Empty;

      string question = string.Empty;

      var components = element.ComponentGroup;
      if (components != null && components.Count > 0)
      {
        for (int i = 0; i < components.Count; i++)
        {
          var htmlComp = components[i].AsWeb();
          if (htmlComp == null)
            continue;
          if (htmlComp.DisplayAt == Interop.SuperMemo.Content.Models.AtFlags.Browsing)
          {
            question = htmlComp.Text?.Value ?? string.Empty;
            break;
          }
        }
      }
      return GetInnerText(question);
    }

    private void TvElements_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {

      var element = TvElements.SelectedItem as IElement;
      if (element != null)
      {
        AnswerBox.Text = GetElementAnswer(element);
        QuestionBox.Text = GetElementQuestion(element);
      }
    }

    private void CheckBox_Checked(object sender, RoutedEventArgs e)
    {
      var checkbox = sender as CheckBox;
      var element = checkbox?.Tag as IElement;
      if (element != null)
      {
        SelectedItems.Add(element);
      }
    }

    private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
      var checkbox = sender as CheckBox;
      var element = checkbox?.Tag as IElement;
      if (element != null)
      {
        SelectedItems.Remove(element);
      }
    }
  }
}
