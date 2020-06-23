using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using SuperMemoAssistant.Services;

namespace SuperMemoAssistant.Plugins.NuggetPalace
{
  public class NuggetPalaceService
  {

    private RestClient Client = new RestClient("https://goldennuggetpalace.com/api/nuggets");
    private NuggetPalaceCfg Config => Svc<NuggetPalacePlugin>.Plugin.Config;

    public async Task PostNuggetAsync(string url, string title, string text)
    {
      var request = new RestRequest().AddJsonBody(new { api_token = Config.Token, url, title, text });
      var x = await Client.ExecutePostAsync(request);
    }
  }
}
