﻿
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

                //flippedArray[j, i] = array[i, j]; // best diagonal
                flippedArray[i, j] = array[j, i]; // best diagonal
                //flippedArray[j, i] = array[i, rows-1-j]; // best diagonal and vertical

                //flippedArray[i, j] = array[rows - 1 - i, j];// top-down

                //flippedArray[i, j] = array[i, columns - 1 - j];
                //flippedArray[rows - 1 - i, columns - 1 - j] = array[i, j];
                //flippedArray[i,j] = array[i, columns - 1 - j];
            }
        }

        return flippedArray;
    }

    public void DebugBoard(ICanvas canvas, Wall w)
    {
        //// debug only
        canvas.FontColor = Colors.Blue;
        canvas.FontSize = 12;
        canvas.DrawString($"({w.Position.X},{w.Position.Y})", w.Element.Location.X, w.Element.Location.Y, w.Dimension.Width, w.Dimension.Height, HorizontalAlignment.Center, VerticalAlignment.Top);
        canvas.DrawString($"({w.MatrixPosition.X},{w.MatrixPosition.Y})", w.Element.Location.X, w.Element.Location.Y, w.Dimension.Width, w.Dimension.Height, HorizontalAlignment.Center, VerticalAlignment.Bottom);
    }
}

