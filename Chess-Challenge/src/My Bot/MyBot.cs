using ChessChallenge.API;
using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;
using System.Globalization;
using System.Data;



public class MyBot : IChessBot
{
    int[] pieceValues = { 0, 100, 233, 279, 428, 873, 0 };
    Move moveToPlay = Move.NullMove;
    Move curr_move = Move.NullMove;
    Move bestmove = Move.NullMove;
    int set_depth = 4;
    double score = 0;

    public Move Think(Board board, Timer timer)
    {
        Move[] allMoves = board.GetLegalMoves();
        Random rng = new();
        //moveToPlay = allMoves[rng.Next(allMoves.Length)];
        maxi(board, set_depth);
        Console.WriteLine(score);

        return moveToPlay;
    }
    bool MoveIsCheckmate(Board board, Move move)
    {
        board.MakeMove(move);
        bool isMate = board.IsInCheckmate();
        board.UndoMove(move);
        return isMate;
    }


    double maxi (Board board, int depth)
    {
        if (depth == 0 )
        {
            moveToPlay = bestmove;
            
            return Eval(board);
            
        }
        double max = double.NegativeInfinity;
        Move[] moves = board.GetLegalMoves();
        foreach (Move i in moves)
        {
           
            board.MakeMove(i);
            score = mini(board, depth - 1);
            if (score > max)
            {
                max = score;
                if (depth == set_depth)
                {
                    bestmove = i;
                }
                
            }
            

            board.UndoMove(i);
        }
        return max;
    }

    double mini(Board board, int depth)
    {
        if (depth == 0)
        {
            return Eval(board);
        }
        double min = double.PositiveInfinity;
        Move[] moves = board.GetLegalMoves();
        foreach (Move i in moves)
        {
            board.MakeMove(i);
            score = maxi(board, depth - 1);
            if (score < min)
            {
                min = score;
            }
            
            board.UndoMove(i);
        }
        return min;
    }

    public int Eval(Board board)
    {

        int white_val = pieceValues[1] * (board.GetPieceList((PieceType)1, true).Count)
            + pieceValues[2] * (board.GetPieceList((PieceType)2, true).Count)
            + pieceValues[3] * (board.GetPieceList((PieceType)3, true).Count)
            + pieceValues[4] * (board.GetPieceList((PieceType)4, true).Count)
            + pieceValues[5] * (board.GetPieceList((PieceType)5, true).Count)
            + pieceValues[6] * (board.GetPieceList((PieceType)6, true).Count);

        int black_val = pieceValues[1] * (board.GetPieceList((PieceType)1, false).Count)
            + pieceValues[2] * (board.GetPieceList((PieceType)2, false).Count)
            + pieceValues[3] * (board.GetPieceList((PieceType)3, false).Count)
            + pieceValues[4] * (board.GetPieceList((PieceType)4, false).Count)
            + pieceValues[5] * (board.GetPieceList((PieceType)5, false).Count)
            + pieceValues[6] * (board.GetPieceList((PieceType)6, false).Count);

        int diff = white_val - black_val;

        int side = board.IsWhiteToMove ? 1 : -1;

        return diff * side;
    }
}