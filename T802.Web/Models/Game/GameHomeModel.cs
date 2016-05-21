namespace T802.Web.Models.Game
{
    public class GameHomeModel : BaseT802Model
    {
        public bool TakenInitialQuiz { get; set; }
        public bool PassedSRPBronzeQuiz { get; set; }
        public bool PassedSRPSilverQuiz { get; set; }
        public bool PassedSRPGoldQuiz { get; set; }

        public bool PassedOCPBronzeQuiz { get; set; }
        public bool PassedOCPSilverQuiz { get; set; }
        public bool PassedOCPGoldQuiz { get; set; }

        public bool PassedLSPBronzeQuiz { get; set; }
        public bool PassedLSPSilverQuiz { get; set; }
        public bool PassedLSPGoldQuiz { get; set; }

        public bool PassedISPBronzeQuiz { get; set; }
        public bool PassedISPSilverQuiz { get; set; }
        public bool PassedISPGoldQuiz { get; set; }

        public bool PassedDSPBronzeQuiz { get; set; }
        public bool PassedDSPSilverQuiz { get; set; }
        public bool PassedDSPGoldQuiz { get; set; }

        public bool TakenFinalQuiz { get; set; }
    }
}