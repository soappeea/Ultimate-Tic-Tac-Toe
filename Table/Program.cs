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
        const int LENGTH = 3;
        static int initgridLocX = 0;
        static int initgridLocY = 0;
        static int gridLocX = 0;
        static int gridLocY = 0;
        static int[,,,] GRID = new int[ROW, LENGTH, ROW, LENGTH];

        //Selection for cell
        static int[,] SELECTION_CELLS = new int[ROW, LENGTH];
        static bool isSelected = false;
        static int prevCellRow = 0;
        static int prevCellCol = 0;
        //static int[,,,]PREVIOUS_GRID = new int[ROW, LENGTH, ROW, LENGTH];

        static void Main(string[] args)
        {
            //Need to initialize numbers for row and length in each cell eg 123456789
            for (int row1 = 1; row1 <= 3; row1++)
                for (int col1 = 1; col1 <= 3; col1++)
                    for (int row2 = 1; row2 <= 3; row2++)
                        for (int col2 = 1; col2 <= 3; col2++)
                        {
                            GRID[row1 - 1, col1 - 1, row2 - 1, col2 - 1] = (row2 - 1) * 3 + col2;
                        }

            for (int row = 1; row <= 3; row++)
                for (int col = 1; col <= 3; col++)
                {
                    SELECTION_CELLS[row - 1, col - 1] = (row - 1) * 3 + col;
                }

            DrawGame();

            
            ChooseCell();
            
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
                            Console.Write(GRID[row1, col1, row2, col2]);
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

            //
            //ColouringCell(0, 0);

            RepositionCursor();
        }
        //private static void ChooseCell()
        //{
        //    //Use number to keep track of selected index
        //    int cellRow = 0;
        //    int cellCol = 0;
        //    int prevCellRow = cellRow;
        //    int prevCellCol = cellCol;


        //    RepositionCursor();
        //    ColourActiveCell(cellRow, cellCol);

        //    while (!isSelected)
        //    {
        //        prevCellRow = cellRow;
        //        prevCellCol = cellCol;
        //        ConsoleKeyInfo keyPressed = Console.ReadKey();
        //        switch (keyPressed.Key)
        //        {
        //            case ConsoleKey.S:
        //                if (cellRow < 2)
        //                {
        //                    //prevCellRow = cellRow;
        //                    cellRow++;
        //                    RepositionCursor();
        //                }
        //                break;
        //            case ConsoleKey.W:
        //                if (cellRow > 0)
        //                {
        //                    //prevCellRow = cellRow;
        //                    cellRow--;
        //                    RepositionCursor();
        //                }
        //                break;
        //            case ConsoleKey.A:
        //                if (cellCol > 0)
        //                {
        //                    //prevCellCol = cellCol;
        //                    cellCol--;
        //                    RepositionCursor();
        //                }
        //                break;
        //            case ConsoleKey.D:
        //                if (cellCol < 2)
        //                {
        //                    //prevCellCol = cellCol;
        //                    cellCol++;
        //                    RepositionCursor();
        //                }
        //                break;
        //            case ConsoleKey.Enter:
        //                isSelected = true;
        //                RepositionCursor();
        //                Console.WriteLine("ENTER HAS BEEN PRESSED");
        //                break;
        //            default:
        //                RepositionCursor();
        //                break;
        //        }
        //        Console.WriteLine(cellRow + "," + cellCol);
        //        ColourActiveCell(cellRow, cellCol);
        //        ColourPrevCell(prevCellRow, prevCellCol);
        //        Console.ReadKey(true);
        //    }
        //    //Figure out why colour isn't changing for selected cell
        //    //ColourActiveCell(cellRow, cellCol);


        //}

        //Call in ChooseCell
        private static void ColouringCell(ConsoleColor background, int cellRow, int cellCol, int precellRow, int precellCol)
        {
            //up
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
                                Console.Write(GRID[row1, col1, row2, col2]);

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
                                Console.Write(GRID[row1, col1, row2, col2]);

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
                                Console.Write(GRID[row1, col1, row2, col2]);

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
                                Console.Write(GRID[row1, col1, row2, col2]);

                            }
                        }
                    }
                }
            }



            for (int row1 = cellRow; row1 < cellRow + 1; row1++)
            {
                for (int col1 = cellCol; col1 < cellCol + 1; col1++)
                {
                    //each child cell
                    for (int row2 = 0; row2 < 3; row2++)
                    {
                        for (int col2 = 0; col2 < 3; col2++)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.SetCursorPosition(initgridLocX + (4 * col1) + col2, initgridLocY + (4 * row1) + row2);
                            Console.Write(GRID[row1, col1, row2, col2]);

                        }
                    }
                }
            }
        }
        private static void ChooseCell()
        {
            int cellRow = 0;
            int cellCol = 0;
            int precellRow = 0;
            int precellCol = 0;
            ConsoleKey key;

            do
            {
                key = Console.ReadKey(true).Key;


                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        if (cellRow > 0)
                        {
                            precellRow = cellRow;
                            cellRow--;
                            precellCol = cellCol;
                            ColouringCell(Console.BackgroundColor, cellRow, cellCol, precellRow, precellCol);
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (cellRow < 2)
                        {
                            precellRow = cellRow;
                            cellRow++;
                            precellCol = cellCol;
                            ColouringCell(Console.BackgroundColor, cellRow, cellCol, precellRow, precellCol);
                        }
                        break;


                    case ConsoleKey.RightArrow:
                        if (cellCol < 2)
                        {
                            precellCol = cellCol;
                            cellCol++;
                            precellRow = cellRow;

                            ColouringCell(Console.BackgroundColor, cellRow, cellCol, precellRow, precellCol);
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (cellCol > 0)
                        {
                            precellCol = cellCol;
                            cellCol--;
                            precellRow = cellRow;
                            ColouringCell(Console.BackgroundColor, cellRow, cellCol, precellRow, precellCol);
                        }
                        break;


                }
            }
            while (key != ConsoleKey.X);

        }

        private static void RepositionCursor()
        {
            Console.SetCursorPosition(0, 13);
        }

        //Call in ChooseCell
        //private static void ColourActiveCell(int cellRow, int cellCol)
        //{
        //    if (!isSelected)
        //    {
        //        for (int row1 = cellRow; row1 < cellRow + 1; row1++)
        //        {
        //            for (int col1 = cellCol; col1 < cellCol + 1; col1++)
        //            {
        //                //each child cell
        //                for (int row2 = 0; row2 < 3; row2++)
        //                {
        //                    for (int col2 = 0; col2 < 3; col2++)
        //                    {
        //                        Console.ForegroundColor = ConsoleColor.Red;
        //                        Console.SetCursorPosition(initgridLocX + (4 * col1) + col2, initgridLocY + (4 * row1) + row2);
        //                        Console.Write(GRID[row1, col1, row2, col2]);
        //                        RepositionCursor();

        //                    }
        //                }
        //            }
        //        }
        //    }
        //    else if (isSelected)
        //    {
        //        for (int row1 = cellRow; row1 < cellRow + 1; row1++)
        //        {
        //            for (int col1 = cellCol; col1 < cellCol + 1; col1++)
        //            {
        //                //each child cell
        //                for (int row2 = 0; row2 < 3; row2++)
        //                {
        //                    for (int col2 = 0; col2 < 3; col2++)
        //                    {
        //                        Console.ForegroundColor = ConsoleColor.Yellow;
        //                        Console.SetCursorPosition(initgridLocX + (4 * col1) + col2, initgridLocY + (4 * row1) + row2);
        //                        Console.Write(GRID[row1, col1, row2, col2]);
        //                        RepositionCursor();

        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        //private static void ColourPrevCell(int prevCellRow, int prevCellCol)
        //{
        //    for (int row1 = prevCellRow; row1 < prevCellRow + 1; row1++)
        //    {
        //        for (int col1 = prevCellCol; col1 < prevCellCol + 1; col1++)
        //        {
        //            //each child cell
        //            for (int row2 = 0; row2 < 3; row2++)
        //            {
        //                for (int col2 = 0; col2 < 3; col2++)
        //                {
        //                    Console.ResetColor();
        //                    Console.SetCursorPosition(initgridLocX + (4 * col1) + col2, initgridLocY + (4 * row1) + row2);
        //                    Console.Write(GRID[row1, col1, row2, col2]);
        //                    RepositionCursor();

        //                }
        //            }
        //        }
        //    }
        //}



    }
}

