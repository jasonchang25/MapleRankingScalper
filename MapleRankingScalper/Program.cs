using MapleRankingScalper.Models;
using MapleRankingScalper.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MapleRankingScalper
{
    class Program
    {
        static private CharacterRepository _characterRepository;
        static private LogWriter _logWriter;

        static void Main(string[] args)
        {
            Initialise();
            Console.WriteLine("Getting Character Information");
            GetCharacterInformation().Wait();
            Console.Write("Press any key to end");
            Console.ReadLine();
        }

        private static void Initialise()
        {
            _characterRepository = new CharacterRepository();            
            _logWriter = new LogWriter();
        }

        private static async Task GetCharacterInformation()
        {
            var startingRank = 9395000;
            var endingRank = 9400000;
            //var endingRank = 33433670;
            for (int i = startingRank; i <= endingRank; i = i + 5)
            {
                Console.WriteLine($"Getting Character Rank: {i}");
                _logWriter.LogWrite($"Getting Character Rank: {i}");
                GetCharacterByRanking(i);
            }
            Console.WriteLine($"Complete");
            _logWriter.LogWrite($"Complete");
        }

        private static async Task GetCharacterByRanking(int ranking)
        {
            var characters = new List<Character>();

            // Initialise client
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri("https://maplestory.nexon.net/");

            // API call parameters (currently hard coded)
            var type = "ranking";
            var id = "overall";
            var id2 = "legendary";
            var rebootIndex = 0;

            try
            {
                
                HttpResponseMessage response = await client.GetAsync($"api/{type}?id={id}&id2={id2}&rebootIndex={rebootIndex}&page_index={ranking}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    characters = JsonConvert.DeserializeObject<List<Character>>(jsonString);
                }

                if (characters.Count > 0)
                {
                    characters.ForEach(c => _characterRepository.AddCharacter(c));
                }
            }
            catch (Exception ex)
            {
                _logWriter.LogWrite(ex.Message);
            }            
        }

        /// <summary>
        /// Test method that adds a character to database
        /// </summary>
        private static void CharacterDatabaseTest()
        {            
            var newCharacter = new Character
            {
                pkCharacterId = 1,
                Rank = 1,
                CharacterName = "test",
                Level = 200,
                JobId = 1,
                JobName = "test job",
                WorldId = 1,
                WorldName = "Test World"
            };

            _characterRepository.AddCharacter(newCharacter);
        }
    }
}
