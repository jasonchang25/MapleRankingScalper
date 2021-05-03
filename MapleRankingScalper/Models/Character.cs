using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapleRankingScalper.Models
{
    public partial class Character
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int pkCharacterId { get; set; }
        public int RankGetRequestId { get; set; }
        public int Rank { get; set; }
        public string WorldName { get; set; }
        public string JobName { get; set; }
        public string CharacterName { get; set; }
        public int JobId { get; set; }
        public int Level { get; set; }
        public int WorldId { get; set; }
    }
}
