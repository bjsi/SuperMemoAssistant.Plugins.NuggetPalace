using Forge.Forms.Annotations;
using Newtonsoft.Json;
using SuperMemoAssistant.Services.UI.Configuration;
using SuperMemoAssistant.Sys.ComponentModel;
using System.ComponentModel;

namespace SuperMemoAssistant.Plugins.NuggetPalace
{
  [Form(Mode = DefaultFields.None)]
  [Title("Dictionary Settings",
   IsVisible = "{Env DialogHostContext}")]
  [DialogAction("cancel",
          "Cancel",
          IsCancel = true)]
  [DialogAction("save",
          "Save",
          IsDefault = true,
          Validates = true)]
  public class NuggetPalaceCfg : CfgBase<NuggetPalaceCfg>, INotifyPropertyChangedEx
  {

    [Field(Name = "Nugget Palace API Token")]
    public string Token { get; set; } = @"dRtJPOdPj2ZWbAs0DxSiU1ttpfH1f4V9g69JW35Um9I4AGBN1VUTO5QnPy4g";

    [JsonIgnore]
    public bool IsChanged { get; set; }

    public override string ToString()
    {
      return "Nugget Palace";
    }

    public event PropertyChangedEventHandler PropertyChanged;
  }
}
