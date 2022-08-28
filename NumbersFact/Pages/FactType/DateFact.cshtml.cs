using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NumbersFact.Models;

namespace NumbersFact.Pages.FactType
{
   
    public class DateFactModel : PageModel
    {

        [BindProperty]
        public string Fact { get; set; }
        [BindProperty]
        public string Day { get; set; }
        [BindProperty]
        public string Month { get; set; }
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost()
        {

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://numbersapi.p.rapidapi.com/{Day}/{Month}/date?json=true"),
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
            }


            return Page();

        }

    }
}
