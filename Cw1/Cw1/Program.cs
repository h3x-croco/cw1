using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Generic;

namespace Cw1
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
           
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(args[0]);

          

            if (args.Length == 0)
            {
                throw new ArgumentNullException("You have to pass the URL as first parameter");
            }

            bool result = Uri.TryCreate(args[0], UriKind.Absolute, out Uri uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttps || uriResult.Scheme == Uri.UriSchemeHttp);

            if (!result)
            {
                throw new ArgumentException("URL nie jest poprawny");
            }

            Console.WriteLine(uriResult);

           if (response.StatusCode == HttpStatusCode.OK)
           {
                var html = await response.Content.ReadAsStringAsync();
                var regex = new Regex("[a-zA-Z0-9]+@[a-z.]+");

                MatchCollection matches = regex.Matches(html);

                HashSet<string> uniqEmails = new HashSet<string>();

                if(matches == null)
                {
                    Console.WriteLine("Brak emaili do wyświetlenia");
                }
                else
                {
                    foreach (var i in matches)
                    {
                        uniqEmails.Add(i.ToString());
                    }
                    
                    foreach (var i in uniqEmails)
                    {
                        Console.WriteLine(i);
                    }
                }                
            }
           
            else
            {
                Console.WriteLine("Błąd w czasie pobierania strony");
            }
            httpClient.Dispose();
        }
    }
}
