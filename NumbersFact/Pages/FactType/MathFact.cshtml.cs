using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using NumbersFact.Models;
using System.Text;
using System.Text.Json;
using System.Web;
namespace NumbersFact.Pages.FactType
{
    public class MathFactModel : PageModel
    {
        [BindProperty]
        public string? Fact { get; set; }
        [BindProperty]
        public int Number { get; set; }
        [BindProperty]
        public string? FactTranslated { get; set; }

        private string _fact="";
        
        public void OnGet()
        {
         
        }

        public async Task<IActionResult> OnPostTranslate()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://google-translate1.p.rapidapi.com/language/translate/v2"),
                Headers =
                {
                    { "X-RapidAPI-Key", "d53d1552b3mshe7ab189d6e7c62bp148089jsn2aa8e3c4f838" },
                    { "X-RapidAPI-Host", "google-translate1.p.rapidapi.com" },
                },
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "q", $"{FactTranslated}" },
                    { "target", "pl" },
                    { "source", "en" },
                }),
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                
                await using var stream = await response.Content.ReadAsStreamAsync();
                var dto = await JsonSerializer.DeserializeAsync<TranslationDto>(stream);
                TranslationItem factItem = new TranslationItem() { translatedText = dto.data.translations[0].translatedText };
                Fact = factItem.translatedText;
            }

            return Page();
        }
        public async Task<IActionResult> OnPost()
        {

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://numbersapi.p.rapidapi.com/{Number}/math?json=true"),
                Headers =
                    {
                        { "X-RapidAPI-Key", "d53d1552b3mshe7ab189d6e7c62bp148089jsn2aa8e3c4f838" },
                        { "X-RapidAPI-Host", "numbersapi.p.rapidapi.com" },
                    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();

                await using var stream = await response.Content.ReadAsStreamAsync();
                var dto = await JsonSerializer.DeserializeAsync<FactDto>(stream);
                FactItem factItem = new FactItem() { Text = dto.text };
                Fact = factItem.Text;
                FactTranslated=Fact;
            }


            return Page();

        }
    }
}
