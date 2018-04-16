using System;
using System.Collections.Generic;
using System.Linq;

namespace UltimateTicTacToeMinimax.Bot
{
    public class SmartBot
    {
        private const bool Debug = true;

        private Random rand = new Random();

        /// <summary>
        /// Returns the next move to make. Edit this method to make your bot smarter.
        /// Currently does only random moves.
        /// </summary>
        /// <param name="state"></param>
        /// <returns>The column where the turn was made</returns>
        public Move GetMove(BotState state)
        {
            char player;
            if (state.Field.MyId == 0) player = 'X';
            else player = '0';

            return Minimax(state, player, 0);
        }

        private Move Minimax(BotState state, char player, int level)
        {
            // Have we reached a Terminal state? has the player won, tied or loss
            // return score: -10 - 10
            var gameState = state.UltimateBoard.GetGameStatus();

            if (gameState == UltimateBoard.GameStatus.OWon) { return new Move { Score = -10 }; }
            else if (gameState == UltimateBoard.GameStatus.XWon) { return new Move { Score = +10 }; }
            else if (gameState == UltimateBoard.GameStatus.Tie) { return new Move { Score = 0 }; }
            //Check the level (we dont want to go further then a certain level so we dont run out of memory)
            //Check level = 5 then return score
            if (level == 4) {
                var score =  state.UltimateBoard.GetScore();
                return new Move { Score = score };

            }


            var moves = state.UltimateBoard.AvailableMoves;
            foreach (var move in moves)
            {
                //Save state of board and macroboard
                var board = (char[,])state.UltimateBoard.Board.Clone();
                var macroboard = (char[,])state.UltimateBoard.Macroboard.Clone();

                //make the move
                state.UltimateBoard.MakeMove(move ,player);

                //Console.WriteLine(state.UltimateBoard);

                //if not level 5 then Score each move by calling Minimax with the oposite palyer
                if (player == UltimateBoard.PlayerX) { move.Score = Minimax(state, UltimateBoard.PlayerO, level + 1).Score; }
                else { move.Score = Minimax(state, UltimateBoard.PlayerX, level + 1).Score; }

                // Reset boards back to the save state
                state.UltimateBoard.Board = board;
                state.UltimateBoard.Macroboard = macroboard;
            }
            //retrun the best move
            if (player == UltimateBoard.PlayerX) { return moves.Max(); }
            else { return moves.Min(); }
        }
                
        static void Main(string[] args)
        {
            BotParser parser = new BotParser(new SmartBot());
            parser.Run();
        }
    }
}
