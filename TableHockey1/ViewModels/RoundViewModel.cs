using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TableHockey;
using TableHockeyData;

namespace TableHockey
{
    public class RoundViewModel
    {
        public string RoundDescription { get; set; }
        public int ContestId { get; set; }
        public int ContestRoundId { get; set; }
        public int RoundNumber { get; set; }
        public bool isFirstRound { get; set; }
        public bool isFinalRound { get; set; }
        public List<GameViewModel> m_vmGames { get; set; }
        public List<GameViewModel> m_vmFirstGame { get; set; }

        public RoundViewModel(TableHockeyContestRound i_round)
        {
            this.ContestId = i_round.ContestId;
            this.RoundNumber = i_round.RoundNumber;
            this.isFirstRound = i_round.isFirstRound;
            this.isFinalRound = i_round.isFinalRound;
            this.RoundDescription = Convert.ToString(i_round.RoundNumber);
            this.ContestRoundId = i_round.TableHockeyContestRoundId;
            m_vmGames = new List<GameViewModel>();
            m_vmFirstGame = new List<GameViewModel>();
            int i = 0;
            foreach (TableHockeyGame m_game in i_round.TableHockeyGame)
            {
                if((m_game.HomePlayerId > 0) && (m_game.AwayPlayerId > 0))
                {
                    GameViewModel m_gamevm = new GameViewModel(m_game);
                    m_vmGames.Add(m_gamevm);
                    if (i == 0)
                    {
                        GameViewModel m_gameVmSingle = new GameViewModel(m_game);
                        m_gameVmSingle.AwayPlayerScore = 0;
                        m_gameVmSingle.HomePlayerScore = 0;
                        m_gameVmSingle.ContestId = this.ContestId;
                        m_gameVmSingle.RoundNumber = this.RoundNumber;
                        m_vmFirstGame.Add(m_gameVmSingle);
                        i++;
                    }
                }        
            }
        }
    }

    public class EndGameOverviewViewModel
    {
        public string HomePlayerDescription { get; set; }
        public string AwayPlayerDescription { get; set; }
        public int HomePlayerGamesWon { get; set; }
        public int AwayPlayerGamesWon { get; set; }
        public int HomePlayerId { get; set; }
        public int AwayPlayerId { get; set; }
        public string PlayerDivider { get; set; }
        public string ScoreDivider { get; set; }

        public EndGameOverviewViewModel(TableHockeyGame i_game)
        {     
            this.HomePlayerDescription = i_game.TableHockeyPlayer.FirstName + " " + i_game.TableHockeyPlayer.LastName;
            this.AwayPlayerDescription = i_game.TableHockeyPlayer1.FirstName + " " + i_game.TableHockeyPlayer1.LastName;          
            this.HomePlayerId = i_game.TableHockeyPlayer.PlayerId;
            this.AwayPlayerId = i_game.TableHockeyPlayer1.PlayerId;    
            this.PlayerDivider = " - ";
            this.ScoreDivider = this.PlayerDivider;
        }
    }

    public class EndGameViewModel
    {
        public int GameId { get; set; }
        public int HomePlayerScore { get; set; }
        public int AwayPlayerScore { get; set; }
        public string HomePlayerScoreDescription { get; set; }
        public string AwayPlayerScoreDescription { get; set; }
        public int HomePlayerId { get; set; }
        public int AwayPlayerId { get; set; }
        public string PlayerDivider { get; set; }
        public string ScoreDivider { get; set; }

        public EndGameViewModel(TableHockeyGame i_game)
        {
            this.GameId = i_game.GameId;
            this.HomePlayerScore = (int)((i_game.HomePlayerScore != null) ? i_game.HomePlayerScore : -1);
            this.AwayPlayerScore = (int)((i_game.AwayPlayerScore != null) ? i_game.AwayPlayerScore : -1);
            this.HomePlayerScoreDescription = this.HomePlayerScore >= 0 ? Convert.ToString(this.HomePlayerScore) : "";
            this.AwayPlayerScoreDescription = this.AwayPlayerScore >= 0 ? Convert.ToString(this.AwayPlayerScore) : "";
            this.HomePlayerId = i_game.TableHockeyPlayer.PlayerId;
            this.AwayPlayerId = i_game.TableHockeyPlayer1.PlayerId;
            this.PlayerDivider = " - ";
            this.ScoreDivider = this.PlayerDivider;
        }
    }


    public class GameViewModel
    {
        public int GameId { get; set; }
        public string HomePlayerDescription { get; set; }
        public string AwayPlayerDescription { get; set; }
        public string IdlePlayerDescription { get; set; }
        public int HomePlayerScore { get; set; }
        public int AwayPlayerScore { get; set; }
        public string HomePlayerScoreDescription { get; set; }
        public string AwayPlayerScoreDescription { get; set; }
        public int HomePlayerId { get; set; }
        public int AwayPlayerId { get; set; }
        public int IdlePlayerId { get; set; }
        public string PlayerDivider { get; set; }
        public string ScoreDivider { get; set; }
        public bool hasSuddenDeath { get; set; }
        public int ContestId { get; set; }
        public int RoundNumber {get;set;}

        public GameViewModel(TableHockeyGame i_game)
        {
            this.GameId = i_game.GameId;
            this.HomePlayerScore = (int)((i_game.HomePlayerScore != null) ? i_game.HomePlayerScore : -1);
            this.AwayPlayerScore = (int)((i_game.AwayPlayerScore != null) ? i_game.AwayPlayerScore : -1);
            this.HomePlayerScoreDescription = this.HomePlayerScore >= 0 ? Convert.ToString(this.HomePlayerScore) : "";
            this.AwayPlayerScoreDescription = this.AwayPlayerScore >= 0 ? Convert.ToString(this.AwayPlayerScore) : "";
            this.HomePlayerDescription = i_game.TableHockeyPlayer.FirstName + " " + i_game.TableHockeyPlayer.LastName;
            this.AwayPlayerDescription = i_game.TableHockeyPlayer1.FirstName + " " + i_game.TableHockeyPlayer1.LastName;
            this.IdlePlayerDescription = i_game.TableHockeyPlayer2.FirstName + " " + i_game.TableHockeyPlayer2.LastName;
            this.HomePlayerId = i_game.TableHockeyPlayer.PlayerId;
            this.AwayPlayerId = i_game.TableHockeyPlayer1.PlayerId;
            this.IdlePlayerId = i_game.TableHockeyPlayer2.PlayerId;
            this.PlayerDivider = " - ";
            this.ScoreDivider = this.PlayerDivider;
            this.hasSuddenDeath = i_game.hasSuddenDeath;
        }
    }
}