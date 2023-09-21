using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Table
{
    class Program
    {
        //Grid info
        const int CELLS = 9;
        const int ROW = 3;
        const int COL = 3;
        static int initgridLocX = 0;
        static int initgridLocY = 0;
        static int gridLocX = 0;
        static int gridLocY = 0;
        static string[,,,] DISPLAYED_GRID = new string[ROW, COL, ROW, COL];

        //Selection for cell
        static int[,] SELECTION_CELLS = new int[ROW, COL];
        static bool isSelected = false;
        static int precellRow = 0;
        static int precellCol = 0;
        static int cellRow = 0;
        static int cellCol = 0;

        static int player1 = 1;
        static int player2 = 2;
        static int currentPlayer = player1;
        static bool player1Turn = true;
        static bool player2Turn = false;


        static void Main(string[] args)
        {
            //Need to initialize numbers for row and length in each cell eg 123456789
            for (int row1 = 1; row1 <= 3; row1++)
                for (int col1 = 1; col1 <= 3; col1++)
                    for (int row2 = 1; row2 <= 3; row2++)
                        for (int col2 = 1; col2 <= 3; col2++)
                        {
                            DISPLAYED_GRID[row1 - 1, col1 - 1, row2 - 1, col2 - 1] = Convert.ToString((row2 - 1) * 3 + col2);
                        }

            for (int row = 1; row <= 3; row++)
                for (int col = 1; col <= 3; col++)
                {
                    SELECTION_CELLS[row - 1, col - 1] = (row - 1) * 3 + col;
                }

            DrawGame();
            ChooseParentCell();

        }

        private static void DrawGame()
        {
            //Display the numbers in each cell
            //each parent cell
            for (int row1 = 0; row1 < 3; row1++)
            {
                for (int col1 = 0; col1 < 3; col1++)
                {
                    //each child cell
                    for (int row2 = 0; row2 < 3; row2++)
                    {
                        for (int col2 = 0; col2 < 3; col2++)
                        {
                            Console.SetCursorPosition(gridLocX, gridLocY);
                            Console.Write(DISPLAYED_GRID[row1, col1, row2, col2]);
                            gridLocX++;
                        }

                        gridLocY++;

                        //Add extra space for grid once child cell is finished displaying all numbers, otherwise continue writing next row of numbers in child cell
                        if (row2 % 3 == 2)
                        {
                            gridLocX++;  //to add extra line for ||

                            // gridLocY--;
                        }
                        else
                        {
                            gridLocX = gridLocX - 3;
                        }
                    }

                    //Adjust the location to left of grid to continue writing next parent row, otherwise write to next parent column
                    if (col1 % 3 == 2)
                    {
                        //Add extra space for grid once last parent column in row is finished displaying
                        gridLocY = gridLocY + 1;  //to add extra line for ===

                        gridLocX = initgridLocX;
                    }
                    else
                    {
                        gridLocY = gridLocY - 3;
                        //gridLocX = gridLocX + 3;
                    }

                }

                //reset gridLocX to initgridLocYX
                //gridLocX = initgridLocX;

                //Draw Grid
                for (int i = 0; i < 11; i++)
                {
                    Console.SetCursorPosition(initgridLocX + i, initgridLocY + 3);
                    Console.Write("═");
                    Console.SetCursorPosition(initgridLocX + i, initgridLocY + 7);
                    Console.Write("═");
                }

                for (int j = 0; j < 11; j++)
                {
                    Console.SetCursorPosition(initgridLocX + 3, initgridLocY + j);
                    Console.Write("║");
                    Console.SetCursorPosition(initgridLocX + 7, initgridLocY + j);
                    Console.Write("║");
                }
            }


            gridLocX = initgridLocX;
            gridLocY = initgridLocY;
            //RepositionCursor();
            ColouringCell(Console.BackgroundColor);
            RepositionCursor();
        }

        //Call in ChooseCell
        private static void ColouringCell(ConsoleColor background)
        {
            Console.ForegroundColor = ConsoleColor.White;
            //up
            if (!isSelected)
            {
                if (precellRow > cellRow)
                {
                    for (int row1 = precellRow; row1 < precellRow + 1; row1++)
                    {
                        for (int col1 = precellCol; col1 < precellCol + 1; col1++)
                        {
                            //each child cell
                            for (int row2 = 0; row2 < 3; row2++)
                            {
                                for (int col2 = 0; col2 < 3; col2++)
                                {
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.SetCursorPosition(initgridLocX + (4 * col1) + col2, initgridLocY + (4 * row1) + row2);
                                    Console.Write(DISPLAYED_GRID[row1, col1, row2, col2]);

                                }
                            }
                        }
                    }
                }
                //down
                else if (precellRow < cellRow)
                {
                    for (int row1 = precellRow; row1 < precellRow + 1; row1++)
                    {
                        for (int col1 = precellCol; col1 < precellCol + 1; col1++)
                        {
                            //each child cell
                            for (int row2 = 0; row2 < 3; row2++)
                            {
                                for (int col2 = 0; col2 < 3; col2++)
                                {
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.SetCursorPosition(initgridLocX + (4 * col1) + col2, initgridLocY + (4 * row1) + row2);
                                    Console.Write(DISPLAYED_GRID[row1, col1, row2, col2]);

                                }
                            }
                        }
                    }
                }

                //right
                if (precellCol < cellCol)
                {
                    for (int row1 = precellRow; row1 < precellRow + 1; row1++)
                    {
                        for (int col1 = precellCol; col1 < precellCol + 1; col1++)
                        {
                            //each child cell
                            for (int row2 = 0; row2 < 3; row2++)
                            {
                                for (int col2 = 0; col2 < 3; col2++)
                                {
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.SetCursorPosition(initgridLocX + (4 * col1) + col2, initgridLocY + (4 * row1) + row2);
                                    Console.Write(DISPLAYED_GRID[row1, col1, row2, col2]);

                                }
                            }
                        }
                    }
                }
                //left
                else if (precellCol > cellCol)
                {
                    for (int row1 = precellRow; row1 < precellRow + 1; row1++)
                    {
                        for (int col1 = precellCol; col1 < precellCol + 1; col1++)
                        {
                            //each child cell
                            for (int row2 = 0; row2 < 3; row2++)
                            {
                                for (int col2 = 0; col2 < 3; col2++)
                                {
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.SetCursorPosition(initgridLocX + (4 * col1) + col2, initgridLocY + (4 * row1) + row2);
                                    Console.Write(DISPLAYED_GRID[row1, col1, row2, col2]);

                                }
                            }
                        }
                    }
                }
            }
            else if (isSelected)
            {
                for (int row1 = 0; row1 < DISPLAYED_GRID.GetLength(0); row1++)
                {
                    for (int col1 = 0; col1 < DISPLAYED_GRID.GetLength(1); col1++)
                    {
                        //each child cell
                        for (int row2 = 0; row2 < 3; row2++)
                        {
                            for (int col2 = 0; col2 < 3; col2++)
                            {
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.SetCursorPosition(initgridLocX + (4 * col1) + col2, initgridLocY + (4 * row1) + row2);
                                Console.Write(DISPLAYED_GRID[row1, col1, row2, col2]);

                                for (int i = 0; i < 11; i++)
                                {
                                    Console.SetCursorPosition(initgridLocX + i, initgridLocY + 3);
                                    Console.Write("═");
                                    Console.SetCursorPosition(initgridLocX + i, initgridLocY + 7);
                                    Console.Write("═");
                                }

                                for (int j = 0; j < 11; j++)
                                {
                                    Console.SetCursorPosition(initgridLocX + 3, initgridLocY + j);
                                    Console.Write("║");
                                    Console.SetCursorPosition(initgridLocX + 7, initgridLocY + j);
                                    Console.Write("║");
                                }
                            }
                        }
                    }
                }
            }

            //changing to red or magenta
            for (int row1 = cellRow; row1 < cellRow + 1; row1++)
            {
                for (int col1 = cellCol; col1 < cellCol + 1; col1++)
                {
                    //each child cell
                    for (int row2 = 0; row2 < 3; row2++)
                    {
                        for (int col2 = 0; col2 < 3; col2++)
                        {
                            if (!isSelected)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                            }
                            else //if (isSelected)
                            {
                                //PROBLEM IS AFTER SELECTING A NUMBER FOR CHILD CELL, IT ALL TURNS MAGENTA.
                                Console.ForegroundColor = ConsoleColor.Magenta;
                            }
                            Console.SetCursorPosition(initgridLocX + (4 * col1) + col2, initgridLocY + (4 * row1) + row2);
                            Console.Write(DISPLAYED_GRID[row1, col1, row2, col2]);

                        }
                    }
                }
            }
        }

        private static void ChooseParentCell()
        {
            ConsoleKey key;

            do
            {
                key = Console.ReadKey(true).Key;


                switch (key)
                {
                    case ConsoleKey.W:
                        if (cellRow > 0)
                        {
                            precellRow = cellRow;
                            cellRow--;
                            precellCol = cellCol;
                            ColouringCell(Console.BackgroundColor);
                        }
                        break;
                    case ConsoleKey.S:
                        if (cellRow < 2)
                        {
                            precellRow = cellRow;
                            cellRow++;
                            precellCol = cellCol;
                            ColouringCell(Console.BackgroundColor);
                        }
                        break;
                    case ConsoleKey.D:
                        if (cellCol < 2)
                        {
                            precellCol = cellCol;
                            cellCol++;
                            precellRow = cellRow;

                            ColouringCell(Console.BackgroundColor);
                        }
                        break;
                    case ConsoleKey.A:
                        if (cellCol > 0)
                        {
                            precellCol = cellCol;
                            cellCol--;
                            precellRow = cellRow;
                            ColouringCell(Console.BackgroundColor);
                        }
                        break;
                }
            }
            while (key != ConsoleKey.Enter);

            //Put after do while loop
            if (key == ConsoleKey.Enter)
            {
                isSelected = true;
                ColouringCell(Console.BackgroundColor);
                RepositionCursor();
                Console.WriteLine("Enter pressed");
                ChooseChildCell();
            }
        }

        private static void RepositionCursor()
        {
            Console.SetCursorPosition(0, 13);
        }

        //Call in ChooseCell
        private static void ChooseChildCell()
        {
            int input = 0;

            RepositionCursor();

            if (currentPlayer == player1)
            {
                while (player1Turn == true)
                {
                    Console.WriteLine("Player 1's turn");
                    Console.WriteLine("What is your choice?");
                    if (Int32.TryParse(Console.ReadLine(), out input))
                    {
                        ClearLines();
                        ProcessChoice(input);
                        Console.WriteLine("Player 1 chose: " + input);
                        Console.WriteLine("Press ENTER to continue");
                        Console.ReadLine();
                        ClearLines();
                        player1Turn = false;
                        currentPlayer = player2;
                        player2Turn = true;
                        ChooseChildCell();
                    }
                    else
                    {
                        Console.WriteLine("Invalid number!\nPress ENTER to try again");
                        Console.ReadLine();
                        ClearLines();
                    }
                }
            }
            else if (currentPlayer == player2)
            {
                while (player2Turn == true)
                {
                    Console.WriteLine("Player 2's turn");
                    Console.WriteLine("What is your choice?");
                    if (Int32.TryParse(Console.ReadLine(), out input))
                    {
                        ClearLines();
                        ProcessChoice(input);
                        Console.WriteLine("Player 2 chose: " + input);
                        Console.WriteLine("Press ENTER to continue");
                        Console.ReadLine();
                        ClearLines();
                        player2Turn = false;
                        currentPlayer = player1;
                        player1Turn = true;
                        ChooseChildCell();
                    }
                    else
                    {
                        Console.WriteLine("Invalid number!\nPress ENTER to try again");
                        Console.ReadLine();
                        ClearLines();
                    }
                }
            }

            Console.ReadLine();
        }

        private static void ProcessChoice(int choice)
        {

            for (int i = 0; i < SELECTION_CELLS.GetLength(0); i++)
            {
                for (int j = 0; j < SELECTION_CELLS.GetLength(1); j++)
                {
                    if (SELECTION_CELLS[i, j] == choice)
                    {
                        if (!DISPLAYED_GRID[cellRow, cellCol, i, j].Equals("X") && !DISPLAYED_GRID[cellRow, cellCol, i, j].Equals("O"))
                        {
                            if (player1Turn)
                            {
                                DISPLAYED_GRID[cellRow, cellCol, i, j] = "X";
                            }
                            else if (player2Turn)
                            {
                                DISPLAYED_GRID[cellRow, cellCol, i, j] = "O";
                            }
                        }
                        else if (DISPLAYED_GRID[cellRow, cellCol, i, j].Equals("X") || DISPLAYED_GRID[cellRow, cellCol, i, j].Equals("O"))
                        {
                            RepositionCursor();
                            Console.WriteLine("This spot is occupied\nPress ENTER to try again");
                            Console.ReadLine();
                            ClearLines();
                            ChooseChildCell();
                        }

                        CheckStatus();
                        cellRow = i;
                        cellCol = j;
                        DrawGame();

                    }
                }
            }
        }

        private static void ClearLines()
        {
            int consoleLines = 8;

            int currentLineCursor = Console.CursorTop;
            RepositionCursor();

            for (int i = 0; i < consoleLines; i++)
            {
                Console.Write(new string(' ', Console.WindowWidth));
            }

            RepositionCursor();
        }

        private static void CheckStatus()
        {
            //If child cell has win, change selection cells value to 0 so it disables parent cell from being chosen

            //for (int childCellRow = 0; childCellRow < DISPLAYED_GRID.GetLength(2); childCellRow++)
            //{
            //    for (int childCellCol = 0; childCellCol < DISPLAYED_GRID.GetLength(3); childCellCol++)
            //    {

            //        if (DISPLAYED_GRID[cellRow, cellCol, childCellRow, childCellCol].Equals("X"))
            //        {

            //        }
            //        else if (DISPLAYED_GRID[cellRow, cellCol, childCellRow, childCellCol].Equals("O"))
            //        {

            //        }
            //    }
            //}
            //Child Cell win 
            //Check horizontal win
            for (int i = 0; i < COL; i++)
            {
                if ((DISPLAYED_GRID[cellRow, cellCol, i, 0].Equals(DISPLAYED_GRID[cellRow, cellCol, i, 1]))
                    && (DISPLAYED_GRID[cellRow, cellCol, i, 1].Equals(DISPLAYED_GRID[cellRow, cellCol, i, 2])))
                {
                    DrawGame();
                    ClearLines();
                    Console.WriteLine("WIN DUHHH");
                    Console.ReadLine();
                }
            }

            //Check vertical win
            for (int i = 0; i < ROW; i++)
            {
                if ((DISPLAYED_GRID[cellRow, cellCol, 0, i].Equals(DISPLAYED_GRID[cellRow, cellCol, 1, i]))
                    && (DISPLAYED_GRID[cellRow, cellCol, 1, i].Equals(DISPLAYED_GRID[cellRow, cellCol, 2, i])))
                {
                    DrawGame();
                    ClearLines();
                    Console.WriteLine("WIN DUHHH");
                    Console.ReadLine();
                }
            }

            //Check diagonal win top left to bottom right
            if ((DISPLAYED_GRID[cellRow, cellCol, 0, 0].Equals(DISPLAYED_GRID[cellRow, cellCol, 1, 1]))
                && (DISPLAYED_GRID[cellRow, cellCol, 1, 1].Equals(DISPLAYED_GRID[cellRow, cellCol, 2, 2])))
            {
                DrawGame();
                ClearLines();
                Console.WriteLine("WIN DUHHH");
                Console.ReadLine();
            }

            //Check diagonal win top right to bottom left
            if ((DISPLAYED_GRID[cellRow, cellCol, 0, 2].Equals(DISPLAYED_GRID[cellRow, cellCol, 1, 1]))
                && (DISPLAYED_GRID[cellRow, cellCol, 1, 1].Equals(DISPLAYED_GRID[cellRow, cellCol, 2, 0])))
            {
                DrawGame();
                ClearLines();
                Console.WriteLine("WIN DUHHH");
                Console.ReadLine();
            }
        }


    }
}