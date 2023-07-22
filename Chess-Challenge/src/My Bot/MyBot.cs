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
    bool white;
    int set_depth = 4;
    double score = 0;

    public Move Think(Board board, Timer timer)
    {
        white = board.IsWhiteToMove;
        Move[] allMoves = board.GetLegalMoves();
        Random rng = new();
        bestmove = allMoves[rng.Next(allMoves.Length)];
        maxi(board, set_depth);
        moveToPlay = bestmove;
        //Console.WriteLine(score);
        //Console.WriteLine("--------");

        return moveToPlay;
    }
    double maxi(Board board, int depth)
    {
        if (depth == 0)
        {
            return Eval(board);
        }
        
        double max = double.NegativeInfinity;
        Move[] moves = board.GetLegalMoves();
        foreach (Move i in moves)
        {
            board.MakeMove(i);
            if (board.IsDraw() && ((Eval(board) > 0 && white) || (Eval(board) < 0 && !white)))
            {
                board.UndoMove(i);
                continue;
            }
                score = mini(board, depth - 1);
            if (score > max)
            {
                max = score;
                if (depth == set_depth)
                {
                    
                    bestmove = i;
                    //Console.WriteLine(bestmove.ToString());
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
        int pos_eval = 0;
        PieceList pawn = board.GetPieceList((PieceType)1, true);
        PieceList king = board.GetPieceList((PieceType)6, true);

        
        if (board.HasKingsideCastleRight(white) || board.HasQueensideCastleRight(white))
        {
            pos_eval += 200;
        }
        else if (!board.HasKingsideCastleRight(!white) || !board.HasQueensideCastleRight(!white))
        {
            pos_eval += 200;
        }

        

        /*
        else
        {
            if (board.HasKingsideCastleRight(white) || board.HasQueensideCastleRight(white))
            {
                pos_eval += 20;
            }
            else if (!board.HasKingsideCastleRight(!white) || !board.HasQueensideCastleRight(!white))
            {
                pos_eval += 20;
            }
        }*/

            
        /*
        foreach (Piece k in king)
        {
            Square curr_square = k.Square;
            if (curr_square.Rank <= 0 && white == true)
            {
                pos_eval += 150;
            }
            else if (curr_square.Rank >= 7 && white == false)
            {
                pos_eval += 150;
            }
        }*/

        foreach (Piece p in pawn)
        {
            Square curr_square = p.Square;

            if (curr_square.Rank > 2 && curr_square.File > 1 && curr_square.File < 5)
            {
                pos_eval += 50;
            }

        }

        int side = board.IsWhiteToMove ? 1 : -1;

        pos_eval *= side;

        int final_eval = EvalMaterials(board) + pos_eval;

        return final_eval;
    }

    


    public int EvalMaterials(Board board)
    {
        int white_eval = 0;
        int black_eval = 0;
        for (int i = 1; i < 7; i++)
        {
            white_eval += pieceValues[i] * (board.GetPieceList((PieceType)i, true).Count);
            black_eval += pieceValues[i] * (board.GetPieceList((PieceType)i, false).Count);
        }
        /*int white_val = pieceValues[1] * (board.GetPieceList((PieceType)1, true).Count)
            + pieceValues[2] * (board.GetPieceList((PieceType)2, true).Count)
            + pieceValues[3] * (board.GetPieceList((PieceType)3, true).Count)
            + pieceValues[4] * (board.GetPieceList((PieceType)4, true).Count)
            + pieceValues[5] * (board.GetPieceList((PieceType)5, true).Count)
            + pieceValues[6] * (board.GetPieceList((PieceType)6, true).Count);*/

        /*int black_val = pieceValues[1] * (board.GetPieceList((PieceType)1, false).Count)
            + pieceValues[2] * (board.GetPieceList((PieceType)2, false).Count)
            + pieceValues[3] * (board.GetPieceList((PieceType)3, false).Count)
            + pieceValues[4] * (board.GetPieceList((PieceType)4, false).Count)
            + pieceValues[5] * (board.GetPieceList((PieceType)5, false).Count)
            + pieceValues[6] * (board.GetPieceList((PieceType)6, false).Count);*/

        int diff = white_eval - black_eval;

        int side = board.IsWhiteToMove ? 1 : -1;

        return diff * side;
    }
}