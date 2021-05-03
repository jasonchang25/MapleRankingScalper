using MapleRankingScalper.Models;
using MapleRankingScalper.Repository;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
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
            //var endingRank = 9400000;
            var endingRank = 33433650;
            var increment = 5;

            IEnumerable<int> rankTasks = Enumerable.Range(0, (endingRank - startingRank) / increment + 1)
                                                   .Select(i => startingRank + increment * i);

            var options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 20
            };

            var result = Parallel.ForEach(rankTasks, options, i =>
            {
                Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Getting Character Rank: {i}");
                _logWriter.LogWrite($"[{Thread.CurrentThread.ManagedThreadId}] Getting Character Rank: {i}");
                GetCharacterByRanking(i).Wait();
            });

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
                    foreach (var character in characters)
                    {
                        character.RankGetRequestId = ranking;
                        _characterRepository.AddCharacter(character);
                    }
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
