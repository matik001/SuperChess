using Rudzoft.ChessLib;
using Rudzoft.ChessLib.Enums;
using Rudzoft.ChessLib.Factories;
using Rudzoft.ChessLib.Fen;
using Rudzoft.ChessLib.MoveGeneration;
using Rudzoft.ChessLib.Types;
using SuperChessBackend.DB.Repositories;

namespace SuperChessBackend.Services
{
    public interface IChessService
    {
        ChessData MakeMove(ChessData chessData, Move move);
    }
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public string ToSymbols()
        {
            return $"{(char)('a' + X)}{Y + 1}";
        }
    }
    //public enum PieceType
    //{
    //    None, Pawn, Knight, Bishop, Rook, Queen, King
    //}
    public class Move
    {
        public Position From { get; set; }
        public Position To { get; set; }
        public PieceTypes? Promotion { get; set; }
    }
    //public static class PiceTypeExtension
    //{
    //    public static string ToPieceTypeSymbol(this PieceType p)
    //    {
    //        return p switch
    //        {
    //            PieceType.Pawn => "P",
    //            PieceType.King => "K",
    //            PieceType.Queen => "Q",
    //            PieceType.Knight => "N",
    //            PieceType.Bishop => "B",
    //            PieceType.Rook => "R",
    //            PieceType.None => "",
    //            _ => throw new Exception("Invalid piece type")
    //        };
    //    }
    //}
    public class ChessService : IChessService
    {
        /// previous attempt to work with pgn (libraries in frontend and in backend have differences in this format)
        //public ChessData MakeMove(ChessData chessData, Move move)
        //{
        //    var game = Pgn.MapString(chessData.PositionFEN);
        //    var moveState = game.Move(new EngineMove(move.From.X, move.From.Y, move.To.X, move.To.Y, move.Promotion));
        //    if(moveState != MoveState.Ok)
        //    {
        //        throw new Exception("Invalid move");
        //    }
        //    var newPgn = Pgn.MapPieces(game.State);
        //    var newPgnSplitted = newPgn.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();

        //    var prevPgn = chessData.PositionFEN;
        //    var prevPgnSplitted = prevPgn.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();

        //    /// do not override history with bugged pgn
        //    newPgnSplitted = prevPgnSplitted.Concat(newPgnSplitted.Skip(prevPgnSplitted.Count)).ToList();
        //    newPgn = string.Join(" ", newPgnSplitted);

        //    if(move.Promotion != null) // fix last pgn move if executed promotion (library has bug)
        //    {

        //        var last = newPgnSplitted.Last();
        //        newPgnSplitted.RemoveAt(newPgnSplitted.Count - 1);
        //        var moveStr = $"{move.To.ToSymbols()}={move.Promotion.Value.ToPieceTypeSymbol()}";
        //        if(move.From.X != move.To.X) // capture
        //        {
        //            moveStr = $"{"a" + move.From.X}x{moveStr}";
        //        }
        //        newPgnSplitted.Add(moveStr);
        //        newPgn = string.Join(" ", newPgnSplitted);
        //    }
        //    return new ChessData()
        //    {
        //        PositionFEN = newPgn,
        //        Result = chessData.Result,
        //    };
        //}
        public ChessData MakeMove(ChessData chessData, Move move)
        {
            var game = GameFactory.Create(chessData.PositionFEN);
            var isWhitePlayerOnMove = game.CurrentPlayer().IsWhite;

            var moveType = MoveTypes.Normal;
            if(move.Promotion != null)
            {
                moveType = MoveTypes.Promotion;
            }
            var movingPiece = game.Pos.GetPiece(new Square(move.From.Y, move.From.X));
            var isKing = movingPiece == Piece.BlackKing || movingPiece == Piece.WhiteKing;
            var isCastling = isKing && Math.Abs(move.From.X - move.To.X) >= 2;
            if(isCastling)
                moveType = MoveTypes.Castling;

            var newMove = new Rudzoft.ChessLib.Types.Move(new Square(move.From.Y, move.From.X), new Square(move.To.Y, move.To.X), moveType, move.Promotion ?? PieceTypes.Queen);
            if(!game.Pos.IsLegal(newMove))
            {
                throw new Exception("Invalid move");
            }
            State newState = game.Pos.State;
            game.Pos.MakeMove(newMove, in newState);
            game.UpdateDrawTypes();
            var fen = game.GetFen();
            var fenStr = fen.Fen.ToString();
            ChessGameResult? result = null;
            
            if(game.Pos.IsMate)
            {
                result = isWhitePlayerOnMove ? ChessGameResult.WinWhite : ChessGameResult.WinBlack;
            }
            else if(game.GameEndType != GameEndTypes.None)
            {
                result = ChessGameResult.Draw;
            }

            return new ChessData()
            {
                PositionFEN = fenStr,
                Result = result,
            };
        }
    }
}
