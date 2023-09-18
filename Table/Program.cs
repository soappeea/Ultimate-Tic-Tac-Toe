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
        private static void ChooseCell()
        {
            //Use number to keep track of selected index
            int cellRow = 0;
            int cellCol = 0;
            ConsoleKeyInfo keyPressed = Console.ReadKey();
            RepositionCursor();
            ColouringCell(cellRow, cellCol);

            
            while (!isSelected)
            {
                switch (keyPressed.Key)
                {
                    case ConsoleKey.S:
                        if (cellRow < 2)
                        {
                            cellRow++;
                            RepositionCursor();
                            Console.Write("Pressed S");
                        }
                        break;
                    case ConsoleKey.W:
                        if (cellRow > 0)
                        {
                            cellRow--;
                            RepositionCursor();
                            Console.Write("Pressed W");
                        }
                        break;
                    case ConsoleKey.A:
                        if (cellCol > 0)
                        {
                            cellCol--;
                            RepositionCursor();
                            Console.Write("Pressed A");
                        }
                        break;
                    case ConsoleKey.D:
                        if (cellCol < 2)
                        {
                            cellCol++;
                            RepositionCursor();
                            Console.Write("Pressed D");
                        }
                        break;
                    case ConsoleKey.Enter:
                        isSelected = true;
                        break;
                    default:
                        RepositionCursor();
                        break;
                }
                Console.WriteLine(cellRow + "," + cellCol);
                ColouringCell(cellRow, cellCol);
                Console.ReadKey(true);
            }

        }

        private static void RepositionCursor()
        {
            Console.SetCursorPosition(0, 13);
        }

        //Call in ChooseCell
        private static void ColouringCell(int cellRow, int cellCol)
        {
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

    }
}

