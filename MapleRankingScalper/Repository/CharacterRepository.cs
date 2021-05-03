using MapleRankingScalper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapleRankingScalper.Repository
{
    interface ICharacterRepository
    {
        public void AddCharacter(Character character);
    }

    public class CharacterRepository : ICharacterRepository
    {
        public CharacterRepository()
        {
        }

        public void AddCharacter(Character character)
        {
            using var context = new mapleScalperContext();
            context.Characters.Add(character);
            context.SaveChanges();
        }
    }
}
