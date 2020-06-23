using SuperMemoAssistant.Extensions;
using SuperMemoAssistant.Services;
using System;
using System.Collections.Generic;
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
  /// <summary>
  /// Interaction logic for NuggetPalaceWdw.xaml
  /// </summary>
  public partial class NuggetPalaceWdw : Window
  {
    public string Url { get; set; }
    public string NuggetTitle { get; set; }
    public string Text { get; set; }

    public NuggetPalaceWdw(string url)
    {

      if (string.IsNullOrEmpty(url))
        return;

      this.Url = url;

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
      await Svc<NuggetPalacePlugin>.Plugin.service.PostNuggetAsync(Url, NuggetTitle, Text);
      Close();
    }

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }
  }
}
