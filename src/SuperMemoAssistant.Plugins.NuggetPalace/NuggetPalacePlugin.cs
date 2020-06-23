#region License & Metadata

// The MIT License (MIT)
// 
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the 
// Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
// 
// 
// Created On:   6/23/2020 2:00:39 AM
// Modified By:  james

#endregion




namespace SuperMemoAssistant.Plugins.NuggetPalace
{
  using System;
  using System.Diagnostics.CodeAnalysis;
  using System.Windows;
  using System.Windows.Input;
  using SuperMemoAssistant.Extensions;
  using SuperMemoAssistant.Plugins.NuggetPalace.UI;
  using SuperMemoAssistant.Services;
  using SuperMemoAssistant.Services.IO.HotKeys;
  using SuperMemoAssistant.Services.IO.Keyboard;
  using SuperMemoAssistant.Services.Sentry;
  using SuperMemoAssistant.Services.UI.Configuration;
  using SuperMemoAssistant.Sys.IO.Devices;

  // ReSharper disable once UnusedMember.Global
  // ReSharper disable once ClassNeverInstantiated.Global
  [SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces")]
  public class NuggetPalacePlugin : SentrySMAPluginBase<NuggetPalacePlugin>
  {
    #region Constructors

    /// <inheritdoc />
    public NuggetPalacePlugin() : base("Enter your Sentry.io api key (strongly recommended)") { }


    #endregion


    #region Properties Impl - Public

    /// <inheritdoc />
    public override string Name => "NuggetPalace";

    /// <inheritdoc />
    public override bool HasSettings => true;
    public NuggetPalaceCfg Config;
    public NuggetPalaceService service = new NuggetPalaceService();
    public bool WdwOpen { get; set; }



    #endregion

    private void LoadConfig()
    {
      Config = Svc.Configuration.Load<NuggetPalaceCfg>() ?? new NuggetPalaceCfg();
    }


    #region Methods Impl

    /// <inheritdoc />
    protected override void PluginInit()
    {
      LoadConfig();

      Svc.HotKeyManager.RegisterGlobal(
        "OpenNuggetPalace",
        "Open a new nugget palace window",
        HotKeyScopes.SMBrowser,
        new HotKey(Key.N, KeyModifiers.CtrlAltShift),
        OpenNuggetPalace
      );

    }

    public void OpenNuggetPalace()
    {

      if (WdwOpen)
        return;

      string content = GetCurrentElementContent();
      string link = ParseLink(content);
      if (string.IsNullOrEmpty(link))
        return;

      OpenNuggetPalaceWdw(link);

    }

    private void OpenNuggetPalaceWdw(string url)
    {
      Application.Current.Dispatcher.Invoke(() =>
      {
        var wdw = new NuggetPalaceWdw(url);
        wdw.ShowAndActivate();
      });
    }

    private string ParseLink(string content)
    {
      if (string.IsNullOrEmpty(content))
        return null;
      string refString = ReferenceHelpers.ParseReferences(content);
      ElementReferences refs = ReferenceHelpers.CreateReferences(refString);
      return refs.Link;
    }

    private string GetCurrentElementContent()
    {
      var ctrlGroup = Svc.SM.UI.ElementWdw.ControlGroup;
      var htmlCtrl = ctrlGroup?.GetFirstHtmlControl()?.AsHtml();
      return htmlCtrl?.Text;
    }


     /// <inheritdoc />
    public override void ShowSettings()
    {
      ConfigurationWindow.ShowAndActivate(HotKeyManager.Instance, Config);
    }

    #endregion


    #region Methods

    #endregion
  }
}
