﻿using System;
using System.Reflection.Emit;

namespace PacManApp.Models;

public class Board
{
    public int[,] Matrix=new int[20,20];
    public Image image = new Image { };
    public Board()
	{
        image.Source = "board.bmp";
        Init();
	}

    private void Init(int Level=1)
    {
        // Initialise Game Board Matrix
        switch (Level)
        {
            case 1:
                Matrix = new int[,] {
                        { 10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10 },
                        { 10,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,10 },
                        { 10,01,10,10,01,10,10,10,10,01,10,10,10,10,10,10,01,10,01,10 },
                        { 10,01,10,10,01,01,01,01,01,01,10,10,01,01,01,01,01,10,01,10 },
                        { 10,01,01,01,01,10,10,10,10,01,10,10,01,10,01,10,01,01,01,10 },
                        { 10,01,10,10,01,10,01,01,01,01,10,10,01,10,01,10,01,10,01,10 },
                        { 10,01,10,10,01,10,10,10,10,01,10,10,01,10,10,10,01,10,01,10 },
                        { 10,01,10,10,01,01,01,01,01,01,01,01,01,01,01,01,01,10,01,10 },
                        { 10,01,10,10,10,10,01,10,00,99,00,10,01,10,10,01,10,01,01,10 },
                        { 10,01,01,01,01,01,01,10,99,00,99,10,01,10,10,01,01,10,01,10 },
                        { 10,01,10,01,10,10,01,10,00,99,00,10,01,01,01,01,10,01,01,10 },
                        { 10,01,10,10,01,10,01,10,10,10,10,10,01,10,10,10,10,10,01,10 },
                        { 10,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,10 },
                        { 10,01,10,10,10,10,01,10,01,10,10,10,10,01,10,01,10,10,01,10 },
                        { 10,01,10,01,01,01,01,10,01,01,01,10,10,01,10,01,01,10,01,10 },
                        { 10,01,10,10,01,10,01,10,01,10,01,10,10,01,01,01,10,10,01,10 },
                        { 10,01,01,01,01,10,01,10,01,10,01,10,10,01,10,01,01,01,01,10 },
                        { 10,01,10,10,01,10,01,10,01,10,01,10,10,01,10,10,01,10,01,10 },
                        { 10,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,10 },
                        { 10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10 },
                        
                       };
                break;
        }


    }

    public static T[,] FlipArray<T>(T[,] array)
    {
        int rows = array.GetLength(0);
        int columns = array.GetLength(1);

        T[,] flippedArray = new T[rows, columns];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                flippedArray[i, j] = array[i, columns - 1 - j];
            }
        }

        return flippedArray;
    }

}

