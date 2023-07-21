using ChessChallenge.API;
using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;
using System.Globalization;

public class MyBot : IChessBot
{
    int[] pieceValues = { 0, 100, 300, 300, 500, 900, 10000 };

    public Move Think(Board board, Timer timer)
    {
        bool white = board.IsWhiteToMove;

        Move[] moves = board.GetLegalMoves();
        board.MakeMove(moves[0]);
        int eval = CalculateDifference(board);
        Move BestMove = moves[0];
        board.UndoMove(moves[0]);

        

        foreach (Move i in moves)
        {
            if (MoveIsCheckmate(board, i))
            {
                return i;
            }
            board.MakeMove(i);



            if (white)
            {
                int new_eval = CalculateDifference(board);
                if(new_eval >= eval)
                {
                    eval = new_eval;
                    BestMove = i;
                    Console.WriteLine(BestMove.ToString());
                }
            }
            else
            {
                int new_eval = CalculateDifference(board);
                if ( new_eval <= eval)
                {
                    eval = new_eval;
                    BestMove = i;
                    Console.WriteLine(BestMove.ToString());
                }
            }

            




            board.UndoMove(i);
        }
        Console.WriteLine("------------------------------------");
        return BestMove;
    }
    bool MoveIsCheckmate(Board board, Move move)
    {
        board.MakeMove(move);
        bool isMate = board.IsInCheckmate();
        board.UndoMove(move);
        return isMate;
    }

    public int CalculateDifference(Board board)
    {
        int white_val = (board.GetPieceList((PieceType)1, true).Count)
            + pieceValues[1] * (board.GetPieceList((PieceType)2, true).Count)
            + pieceValues[2] * (board.GetPieceList((PieceType)3, true).Count)
            + pieceValues[3] * (board.GetPieceList((PieceType)4, true).Count)
            + pieceValues[4] * (board.GetPieceList((PieceType)5, true).Count)
            + pieceValues[5] * (board.GetPieceList((PieceType)6, true).Count);

        int black_val = pieceValues[1] * (board.GetPieceList((PieceType)1, false).Count)
            + pieceValues[1] * (board.GetPieceList((PieceType)2, false).Count)
            + pieceValues[1] * (board.GetPieceList((PieceType)3, false).Count)
            + pieceValues[1] * (board.GetPieceList((PieceType)4, false).Count)
            + pieceValues[1] * (board.GetPieceList((PieceType)5, false).Count)
            + pieceValues[1] * (board.GetPieceList((PieceType)6, false).Count);

        int diff = white_val - black_val;

        return diff;
    }
}