//Author: Sophia Lin
//File Name: Program.cs
//Project Name: LinSPASS1
//Creation Date: September 15, 2023
//Modified Date: September 24, 2023
//Description: A game of ultimate tic tac toe that allows 2 players to play simultaneously
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LinSPASS1
{
    class Program
    {
        //Track gamestates
        static int gamestate = MENU;
        const int MENU = 1;
        const int STATISTICS = 2;
        const int GAMEPLAY = 3;
        const int GAMEOVER = 4;

        //Store information
        const int ROW = 3;
        const int COL = 3;
        static int initgridLocX = 2;
        static int initgridLocY = 3;
        static int gridLocX = initgridLocX;
        static int gridLocY = initgridLocY;
        static string[,,,] displayedGrid = new string[ROW, COL, ROW, COL];

        //Track selection of cell
        static int[,] selectionCells = new int[ROW, COL];
        static string[,] statusParentCells = new string[ROW, COL];
        static bool isSelected = false;
        static int precellRow = 0;
        static int precellCol = 0;
        static int cellRow = 0;
        static int cellCol = 0;

        //Track player information
        const int NUM_PLAYERS = 2;
        static int player1 = 0;
        static int player2 = 1;
        static int currentPlayer = player1;
        static bool player1Turn = true;
        static bool player2Turn = false;
        static int[] wins = new int[] { 0, 0 };
        static int[] losses = new int[] { 0, 0 };
        static int draw = 0;
        static int games = 0;


        //Track the win and draw status
        static bool gameWon = false;
        const int WIN = 1;
        const int CHILD_DRAW = 2;
        const int PARENT_DRAW = 3;
        static int finalResult = -1;

        static StreamWriter outFile;
        static StreamReader inFile;

        static void Main(string[] args)
        {
            //Read in both player's stats
            ReadFile("tictactoe.txt");

            //Change colour
            Console.ForegroundColor = ConsoleColor.White;

            //Input for menu option
            string input = "";

            //Run application unless user presses 3 to exit
            while (input != "3")
            {
                //Display and process different gamestates
                switch (gamestate)
                {
                    case MENU:
                        DrawMenu();
                        input = Console.ReadLine();
                        UpdateMenu(input);
                        break;
                    case STATISTICS:
                        UpdateStats();
                        DrawStats();
                        break;
                    case GAMEPLAY:
                        UpdateGame();
                        break;
                    case GAMEOVER:
                        DrawEndgame();
                        UpdateEndgame();
                        break;
                }
            }
        }

        //Pre: None
        //Post: None
        //Desc: Handle input in the menu 
        private static void UpdateMenu(string choice)
        {
            //Change game state based on input
            switch (choice)
            {
                case "1":
                    Console.Clear();
                    gamestate = GAMEPLAY;
                    break;
                case "2":
                    Console.Clear();
                    gamestate = STATISTICS;
                    break;
                case "3":
                    Environment.Exit(0);
                    break;
                default:
                    CentreText("Invalid Menu Option", ConsoleColor.Red, 8);
                    CentreText("Press ENTER to try again", ConsoleColor.Red, 9); 
                    Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
        }

        //Pre: None
        //Post: None
        //Desc: Update statistics
        private static void UpdateStats()
        {
            games = wins[player1] + losses[player1] + draw;
        }

        //Pre: None
        //Post: None
        //Desc: Handle all game related functionality
        private static void UpdateGame()
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.White;
                InitializeData();

                DrawGame();
                ChooseParentCell();

                //check gameover info
                if (gamestate == GAMEOVER) break;
            }
        }

        //Pre: None
        //Post: None
        //Desc: Handle input in the game over screen
        private static void UpdateEndgame()
        {
            InitializeData();
            Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            gamestate = MENU;
        }

        //Pre: None
        //Post: None
        //Desc: Display the menu 
        private static void DrawMenu()
        {
            Console.Clear();

            CentreText("TIC TAC TOE", ConsoleColor.Green, 1);
            CentreText("===========", ConsoleColor.Green, 2);
            CentreText("1. Play Game", ConsoleColor.White, 3);
            CentreText("2. Statistics", ConsoleColor.White, 4);
            CentreText("3. Exit Game", ConsoleColor.White, 5);
            CentreText("Type # of your choice: ", ConsoleColor.Gray, 7);
        }

        //Pre: None
        //Post: None
        //Desc: Display the statistics in a table
        private static void DrawStats()
        {
            int tableWidth = 45;
            int playerDispNum = 5;
            int horizSpace = 2;
            int vertSpace = 4;
            int centeredNumSpace = 3;

            int inittabLocX = Console.WindowWidth / 2 - tableWidth / 2;
            int inittabLocY = 5;
            int tabLocX = inittabLocX;
            int tabLocY = inittabLocY;

            int[] distCol = new int[] { 9, 18, 27, 36 };
            int distRow = 2;
            string[] labels = new string[] { "Player", "Games", "Wins", "Loss", "Draws" };
            string[] p1Stats = new string[] { "1", Convert.ToString(games), Convert.ToString(wins[player1]), Convert.ToString(losses[player1]), Convert.ToString(draw) };
            string[] p2Stats = new string[] { "2", Convert.ToString(games), Convert.ToString(wins[player2]), Convert.ToString(losses[player2]), Convert.ToString(draw) };

            CentreText("STATISTICS", ConsoleColor.Green, horizSpace);
            CentreText("======================", ConsoleColor.Green, horizSpace + 1);

            //Display labels in table
            for (int i = 0; i < playerDispNum; i++)
            {
                if (i == 0)
                {
                    Console.SetCursorPosition(inittabLocX + horizSpace, inittabLocY);
                    Console.WriteLine(labels[i]);
                }
                else
                {
                    Console.SetCursorPosition(inittabLocX + distCol[i - 1] + horizSpace, inittabLocY);
                    Console.WriteLine(labels[i]);
                }
            }

            //Move to next column
            tabLocY += vertSpace;

            //Display player 1 stats
            for (int i = 0; i < playerDispNum; i++)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                if (i == 0)
                {
                    Console.SetCursorPosition(inittabLocX + centeredNumSpace, tabLocY);
                    Console.WriteLine(p1Stats[i]);
                }
                else
                {
                    Console.SetCursorPosition(inittabLocX + distCol[i - 1] + centeredNumSpace, tabLocY);
                    Console.WriteLine(p1Stats[i]);
                }
            }

            Console.ForegroundColor = ConsoleColor.White;

            tabLocY += vertSpace;

            //Display player 2 stats
            for (int i = 0; i < playerDispNum; i++)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                if (i == 0)
                {
                    Console.SetCursorPosition(inittabLocX + centeredNumSpace, tabLocY);
                    Console.WriteLine(p2Stats[i]);
                }
                else
                {
                    Console.SetCursorPosition(inittabLocX + distCol[i - 1] + centeredNumSpace, tabLocY);
                    Console.WriteLine(p2Stats[i]);
                }
            }

            Console.ForegroundColor = ConsoleColor.White;

            //Display Table
            for (int i = 0; i < tableWidth; i++)
            {
                Console.SetCursorPosition(inittabLocX + i, inittabLocY + distRow);
                Console.Write("═");
            }

            for (int j = 0; j < 11; j++)
            {
                for (int k = 0; k < distCol.Length; k++)
                {
                    tabLocX = inittabLocX + distCol[k];
                    Console.SetCursorPosition(tabLocX, inittabLocY + j);
                    Console.Write("║");
                }
            }

            CentreText("Press ENTER to continue", ConsoleColor.Red, 16);
            Console.ReadLine();
            gamestate = MENU;
        }

        //Pre: None
        //Post: None
        //Desc: Display all game elements
        private static void DrawGame()
        {
            //Display label of main board
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(initgridLocX, 0);
            Console.WriteLine("MAIN BOARD");
            Console.SetCursorPosition(initgridLocX, 1);
            Console.WriteLine("==========");
            Console.ForegroundColor = ConsoleColor.White;

            //Initialize grid settings for mini grid
            int miniInitgridX = 30;
            int miniInitgridY = 3;
            int miniGridLocX = miniInitgridX;
            int miniGridLocY = miniInitgridY;

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
                            Console.Write(displayedGrid[row1, col1, row2, col2]);

                            gridLocX++;
                        }

                        gridLocY++;

                        //Add extra space for grid once child cell is finished displaying all numbers, otherwise continue writing next row of numbers in child cell
                        if (row2 % 3 == 2)
                        {
                            //to add extra line for ||
                            gridLocX++;
                        }
                        else
                        {
                            gridLocX -= 3;
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
                        gridLocY -= 3;
                    }

                }

                //reset gridLocX to initgridLocYX
                //gridLocX = initgridLocX;

                //Draw main grid
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

            //Display label of main board
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(miniInitgridX - 3, 0);
            Console.WriteLine("STATUS BOARD");
            Console.SetCursorPosition(miniInitgridX - 3, 1);
            Console.WriteLine("============");
            Console.ForegroundColor = ConsoleColor.White;

            //Draw status of each grid using mini grid
            Console.SetCursorPosition(miniInitgridX, miniInitgridY);

            for (int i = 0; i < 5; i++)
            {
                Console.SetCursorPosition(miniInitgridX + i, miniInitgridY + 1);
                Console.Write("═");
                Console.SetCursorPosition(miniInitgridX + i, miniInitgridY + 3);
                Console.Write("═");
            }

            for (int j = 0; j < 5; j++)
            {
                Console.SetCursorPosition(miniInitgridX + 1, miniInitgridY + j);
                Console.Write("║");
                Console.SetCursorPosition(miniInitgridX + 3, miniInitgridY + j);
                Console.Write("║");
            }

            //Display status in grid
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    Console.SetCursorPosition(miniGridLocX, miniGridLocY);
                    Console.Write(statusParentCells[row, col]);

                    miniGridLocX += 2;
                }

                miniGridLocY += 2;

                //Add extra space for grid once child cell is finished displaying all numbers, otherwise continue writing next row of numbers in child cell
                if (row % 3 == 2)
                {
                    miniGridLocX += 2;  //to add extra line for ||
                }
                else
                {
                    miniGridLocX -= 6;
                }
            }

            gridLocX = initgridLocX;
            gridLocY = initgridLocY;
            ColouringCell();
            RepositionCursor();
        }

        //Pre: None
        //Post: None
        //Desc: Display the game over screen
        private static void DrawEndgame()
        {
            Console.Clear();

            CentreText("##############################################################", ConsoleColor.Green, 6);
            if (finalResult == 0)
            {
                CentreText("PLAYER 1 HAS WON THE GAME!!", ConsoleColor.Magenta, 7);
                CentreText("PRESS ENTER TO CONTINUE", ConsoleColor.Red, 8);
            }
            else if (finalResult == 1)
            {
                CentreText("PLAYER 2 HAS WON THE GAME!!\nPRESS ENTER TO CONTINUE", ConsoleColor.Cyan, 7);
                CentreText("PRESS ENTER TO CONTINUE", ConsoleColor.Red, 8);
            }
            if (finalResult == 2)
            {
                CentreText("THE GAME HAS ENDED IN A DRAW!!", ConsoleColor.Yellow, 7);
                CentreText("PRESS ENTER TO CONTINUE", ConsoleColor.Red, 8);
            }
            CentreText("##############################################################", ConsoleColor.Green, 9);
        }

        //Pre: None
        //Post: None
        //Desc: Initializing all game data before actual gameplay occurs
        private static void InitializeData()
        {
            initgridLocX = 2;
            initgridLocY = 3;
            gridLocX = initgridLocX;
            gridLocY = initgridLocY;
            isSelected = false;
            precellRow = 0;
            precellCol = 0;
            cellRow = 0;
            cellCol = 0;
            currentPlayer = player1;
            player1Turn = true;
            player2Turn = false;
            gameWon = false;
            finalResult = -1;

            //Need to initialize numbers for row and length in each cell eg 123456789
            for (int row1 = 1; row1 <= ROW; row1++)
                for (int col1 = 1; col1 <= COL; col1++)
                    for (int row2 = 1; row2 <= ROW; row2++)
                        for (int col2 = 1; col2 <= COL; col2++)
                        {
                            displayedGrid[row1 - 1, col1 - 1, row2 - 1, col2 - 1] = Convert.ToString((row2 - 1) * 3 + col2);
                        }

            for (int row = 1; row <= ROW; row++)
                for (int col = 1; col <= COL; col++)
                {
                    selectionCells[row - 1, col - 1] = (row - 1) * 3 + col;
                }


            for (int row = 1; row <= ROW; row++)
                for (int col = 1; col <= COL; col++)
                {
                    statusParentCells[row - 1, col - 1] = "";
                }

            //debugging purposes
            //displayedGrid[0, 0, 0, 0] = "X";
            //displayedGrid[0, 0, 0, 1] = "X";
            //displayedGrid[0, 0, 0, 2] = "O";
            //displayedGrid[0, 0, 1, 0] = "O";
            //displayedGrid[0, 0, 1, 1] = "O";
            //displayedGrid[0, 0, 1, 2] = "X";
            //displayedGrid[0, 0, 2, 0] = "X";
            //displayedGrid[0, 0, 2, 1] = "O";


            //statusParentCells[0, 1] = "O";
            //statusParentCells[0, 2] = "X";
            //statusParentCells[1, 0] = "O";
            //statusParentCells[1, 1] = "X";
            //statusParentCells[1, 2] = "O";
            //statusParentCells[2, 0] = "O";
            //statusParentCells[2, 1] = "X";
            //statusParentCells[2, 2] = "O";

        }

        //Pre: None
        //Post: None
        //Desc: Changes colour of cell based on player moves
        private static void ColouringCell()
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
                                    Console.Write(displayedGrid[row1, col1, row2, col2]);

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
                                    Console.Write(displayedGrid[row1, col1, row2, col2]);

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
                                    Console.Write(displayedGrid[row1, col1, row2, col2]);

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
                                    Console.Write(displayedGrid[row1, col1, row2, col2]);

                                }
                            }
                        }
                    }
                }
            }
            else if (isSelected)
            {
                for (int row1 = 0; row1 < displayedGrid.GetLength(0); row1++)
                {
                    for (int col1 = 0; col1 < displayedGrid.GetLength(1); col1++)
                    {
                        //each child cell
                        for (int row2 = 0; row2 < 3; row2++)
                        {
                            for (int col2 = 0; col2 < 3; col2++)
                            {
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.SetCursorPosition(initgridLocX + (4 * col1) + col2, initgridLocY + (4 * row1) + row2);
                                Console.Write(displayedGrid[row1, col1, row2, col2]);

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
                            else
                            {
                                if (player1Turn)
                                {
                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                }
                                else if (player2Turn)
                                {
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                }
                            }
                            Console.SetCursorPosition(initgridLocX + (4 * col1) + col2, initgridLocY + (4 * row1) + row2);
                            Console.Write(displayedGrid[row1, col1, row2, col2]);
                        }
                    }
                }
            }
        }

        //Pre: None
        //Post: None
        //Desc: Chooses which parent cell will be played in 
        private static void ChooseParentCell()
        { 
            ConsoleKey key;

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Use WASD to choose slot\nPress ENTER to confirm");
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
                            ColouringCell();
                        }
                        break;
                    case ConsoleKey.S:
                        if (cellRow < 2)
                        {
                            precellRow = cellRow;
                            cellRow++;
                            precellCol = cellCol;
                            ColouringCell();
                        }
                        break;
                    case ConsoleKey.D:
                        if (cellCol < 2)
                        {
                            precellCol = cellCol;
                            cellCol++;
                            precellRow = cellRow;

                            ColouringCell();
                        }
                        break;
                    case ConsoleKey.A:
                        if (cellCol > 0)
                        {
                            precellCol = cellCol;
                            cellCol--;
                            precellRow = cellRow;
                            ColouringCell();
                        }
                        break;
                }
            }
            while (key != ConsoleKey.Enter);

            //Put after do while loop
            if (key == ConsoleKey.Enter )
            {
                if (gamestate != GAMEOVER)
                {
                    isSelected = true;
                    ColouringCell();
                    RepositionCursor();
                    CheckClaimed();
                    ChooseChildCell();
                }
            }
        }

        //Pre: None
        //Post: None
        //Desc: Handle input in the instructions screen
        private static void ProcessChoice(int choice)
        {
            //Claiming child cell slot
            for (int i = 0; i < selectionCells.GetLength(0); i++)
            {
                if (gamestate == GAMEOVER) break;
                for (int j = 0; j < selectionCells.GetLength(1); j++)
                {
                    if (selectionCells[i, j] == choice)
                    {
                        //Change the child slot to X or O if it isn't already claimed
                        if (!displayedGrid[cellRow, cellCol, i, j].Equals("X") && !displayedGrid[cellRow, cellCol, i, j].Equals("O"))
                        {
                            if (player1Turn)
                            {
                                displayedGrid[cellRow, cellCol, i, j] = "X";
                            }
                            else if (player2Turn)
                            {
                                displayedGrid[cellRow, cellCol, i, j] = "O";
                            }
                        }
                        else if (displayedGrid[cellRow, cellCol, i, j].Equals("X") || displayedGrid[cellRow, cellCol, i, j].Equals("O"))
                        {

                            ClearLines();
                            Console.WriteLine("This spot is occupied\nPress ENTER to try again");
                            Console.ReadLine();
                            ClearLines();

                            if (gamestate == GAMEOVER) break;

                            ChooseChildCell();
                        } 

                        if (gamestate != GAMEOVER)
                        {
                            CheckStatus();


                            if (gamestate == GAMEOVER) break;

                            cellRow = i;
                            cellCol = j;
                            DrawGame();
                            CheckClaimed();
                        }
                    }
                }
            }
        }

        //Pre: None
        //Post: None
        //Desc: Choose which child cell will be claimed by current player
        private static void ChooseChildCell()
        {
            if (gamestate == GAMEOVER) return;

            int input = 0;

            ClearLines();
            RepositionCursor();

            if (currentPlayer == player1)
            {
                while (player1Turn == true)
                {
                    Console.WriteLine("Player 1's turn (X)");
                    Console.Write("Enter choice: ");
                    if (Int32.TryParse(Console.ReadLine(), out input) && input >= 1 && input <= 9)
                    {
                        ClearLines();
                        ProcessChoice(input);
                        if (gamestate == GAMEOVER) break;

                        Console.WriteLine("Player 1 chose: " + input);
                        Console.WriteLine("Press ENTER to continue");
                        Console.ReadLine();
                        ClearLines();
                        player1Turn = false;
                        currentPlayer = player2;
                        player2Turn = true;
                        ColouringCell();
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
                    Console.WriteLine("Player 2's turn (O)");
                    Console.Write("Enter choice: ");
                    if (Int32.TryParse(Console.ReadLine(), out input) && input >= 1 && input <= 9)
                    {
                        ClearLines();
                        ProcessChoice(input);
                        if (gamestate == GAMEOVER) break;

                        Console.WriteLine("Player 2 chose: " + input);
                        Console.WriteLine("Press ENTER to continue");
                        Console.ReadLine();
                        ClearLines();
                        player2Turn = false;
                        currentPlayer = player1;
                        player1Turn = true;
                        ColouringCell();
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

            if (gamestate != GAMEOVER)
                Console.ReadLine();
        }

        //Pre: None
        //Post: None
        //Desc: Check if the redirected parent cell is claimed
        private static void CheckClaimed()
        {
            //When redirect to claimed spot, still register the x and o in current board, but allow player to select their own spot
            if (statusParentCells[cellRow, cellCol].Equals("X") || statusParentCells[cellRow, cellCol].Equals("O") || statusParentCells[cellRow, cellCol].Equals("D"))
            {
                RepositionCursor();
                ClearLines();
                Console.WriteLine("This slot is occupied\nPress ENTER to try again");
                Console.ReadLine();
                ClearLines();
                isSelected = false;
                if (player1Turn)
                {
                    player1Turn = false;
                    currentPlayer = player1;
                    player2Turn = true;
                }
                else if (player2Turn)
                {
                    player2Turn = false;
                    currentPlayer = player1;
                    player1Turn = true;
                }

                ChooseParentCell();
            }
        }

        //Pre: None
        //Post: None
        //Desc: Detect if a win, draw, or none occurs
        private static void CheckStatus()
        {
            CheckChildStatus();
            CheckParentStatus();
        }

        //Pre: None
        //Post: None
        //Desc: Check for a win, draw, or nothing in an individual parent cell 
        private static void CheckChildStatus()
        {
            //If child cell has win, change selection cells value to 0 so it disables parent cell from being chosen
            //Child Cell win 
            //Check horizontal win
            for (int i = 0; i < ROW; i++)
            {
                if ((displayedGrid[cellRow, cellCol, i, 0].Equals(displayedGrid[cellRow, cellCol, i, 1]))
                    && (displayedGrid[cellRow, cellCol, i, 1].Equals(displayedGrid[cellRow, cellCol, i, 2])))
                {
                    DrawGame();
                    ChangeStatus(WIN);
                    Console.ReadLine();
                    ClearLines();
                }
            }

            //Check vertical win
            for (int i = 0; i < COL; i++)
            {
                if ((displayedGrid[cellRow, cellCol, 0, i].Equals(displayedGrid[cellRow, cellCol, 1, i]))
                    && (displayedGrid[cellRow, cellCol, 1, i].Equals(displayedGrid[cellRow, cellCol, 2, i])))
                {
                    DrawGame();
                    ChangeStatus(WIN);
                    ClearLines();
                }
            }

            //Check diagonal win top left to bottom right
            if ((displayedGrid[cellRow, cellCol, 0, 0].Equals(displayedGrid[cellRow, cellCol, 1, 1]))
                && (displayedGrid[cellRow, cellCol, 1, 1].Equals(displayedGrid[cellRow, cellCol, 2, 2])))
            {
                DrawGame();
                ChangeStatus(WIN);
                Console.ReadLine();
                ClearLines();
            }

            //Check diagonal win top right to bottom left
            if ((displayedGrid[cellRow, cellCol, 0, 2].Equals(displayedGrid[cellRow, cellCol, 1, 1]))
                && (displayedGrid[cellRow, cellCol, 1, 1].Equals(displayedGrid[cellRow, cellCol, 2, 0])))
            {
                DrawGame();
                ChangeStatus(WIN);
                Console.ReadLine();
                ClearLines();
            }

            ChangeStatus(CHILD_DRAW);
        }

        //Pre: None
        //Post: None
        //Desc: Check for a win, draw, or nothing in the whole game
        private static void CheckParentStatus()
        {
            if (gamestate != GAMEOVER)
            {
                //Check horizontal win
                for (int i = 0; i < ROW; i++)
                {

                    if ((statusParentCells[i, 0].Equals(statusParentCells[i, 1]) || statusParentCells[i, 0].Equals("D") || statusParentCells[i, 1].Equals("D"))
                         && (statusParentCells[i, 1].Equals(statusParentCells[i, 2]) || statusParentCells[i, 1].Equals("D") || statusParentCells[i, 2].Equals("D")))
                    {
                        if (!statusParentCells[i, 0].Equals("")
                            && !statusParentCells[i, 1].Equals("")
                            && !statusParentCells[i, 2].Equals(""))
                        {
                            if (!(statusParentCells[i, 0].Equals("D") && statusParentCells[i, 1].Equals("D") && statusParentCells[i, 2].Equals("D")))
                            {
                                gameWon = true;
                                DrawGame();
                                ChangeStatus(WIN);
                                if (gamestate == GAMEOVER) break;
                                Console.ReadLine();
                            }
                        }
                    }
                }
                //Check vertical win
                for (int i = 0; i < COL; i++)
                {
                    if ((statusParentCells[0, i].Equals(statusParentCells[1, i]) || statusParentCells[0, i].Equals("D") || statusParentCells[1, i].Equals("D"))
                        && (statusParentCells[1, i].Equals(statusParentCells[2, i]) || statusParentCells[1, i].Equals("D") || statusParentCells[2, i].Equals("D")))
                    {
                        if (!statusParentCells[0, i].Equals("")
                            && !statusParentCells[1, i].Equals("")
                            && !statusParentCells[2, i].Equals(""))
                        {
                            if (!(statusParentCells[0, i].Equals("D") && statusParentCells[1, i].Equals("D") && statusParentCells[2, i].Equals("D")))
                            {
                                gameWon = true;
                                DrawGame();
                                ChangeStatus(WIN);
                                if (gamestate == GAMEOVER) break;
                                Console.ReadLine();
                            }
                        }
                    }


                }

                //Check diagonal win top left to bottom right
                if (gamestate != GAMEOVER)
                {
                    if ((statusParentCells[0, 0].Equals(statusParentCells[1, 1]) || statusParentCells[0, 0].Equals("D") || statusParentCells[1, 1].Equals("D"))
                    && (statusParentCells[1, 1].Equals(statusParentCells[2, 2]) || statusParentCells[1, 1].Equals("D") || statusParentCells[2, 2].Equals("D")))
                    {
                        if (!statusParentCells[0, 0].Equals("")
                            && !statusParentCells[1, 1].Equals("")
                            && !statusParentCells[2, 2].Equals(""))
                        {
                            if (!(statusParentCells[0, 0].Equals("D") && statusParentCells[1, 1].Equals("D") && statusParentCells[2, 2].Equals("D")))
                            {
                                gameWon = true;
                                DrawGame();
                                ChangeStatus(WIN);

                                Console.ReadLine();
                            }
                        }
                    }


                    //Check diagonal win top right to bottom left
                    if ((statusParentCells[0, 2].Equals(statusParentCells[1, 1]) || statusParentCells[0, 2].Equals("D") || statusParentCells[1, 1].Equals("D"))
                        && (statusParentCells[1, 1].Equals(statusParentCells[2, 0]) || statusParentCells[1, 1].Equals("D") || statusParentCells[2, 0].Equals("D")))
                    {
                        if (!statusParentCells[0, 2].Equals("")
                                && !statusParentCells[1, 1].Equals("")
                                && !statusParentCells[2, 0].Equals(""))
                        {
                            if (!(statusParentCells[0, 2].Equals("D") && statusParentCells[1, 1].Equals("D") && statusParentCells[2, 0].Equals("D")))
                            {
                                gameWon = true;
                                DrawGame();
                                ChangeStatus(WIN);
                                Console.ReadLine();
                            }
                        }
                    }



                    ChangeStatus(PARENT_DRAW);
                }
            }
        }

        //Pre: None
        //Post: Return if the individual parent cell is full or not
        //Desc: Check if an individual parent cell is full
        private static bool IsFullChildGrid()
        {
            bool isFull = true;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    int parseVal = 0;
                    if (int.TryParse(displayedGrid[cellRow, cellCol, i, j], out parseVal))
                    {
                        isFull = false;
                        break;
                    }
                }
            return isFull;

        }

        //Pre: None
        //Post: Return if the whole grid is full or not
        //Desc: Check if the whole grid is full
        private static bool IsFullParentGrid()
        {
            bool isFull = true;

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    
                    if (statusParentCells[i,j].Equals(""))
                    {
                        isFull = false;
                        break;
                    }
                }

            return isFull;

        }

        //Pre: None
        //Post: None DOLFGISDUFGP0AE9YU30-Y98-F09G8EA-0TY98G0SDFGOFG;'PO
        //Desc: Change the status of the parent cell to claimed by a player or a draw 
        private static void ChangeStatus(int status)
        {
            if (gamestate == GAMEOVER) return;
             if (status == WIN)
             {
                if (!gameWon)
                {
                    if (currentPlayer == player1)
                    {
                        if (!statusParentCells[cellRow, cellCol].Equals("X") && !statusParentCells[cellRow, cellCol].Equals("O") && !statusParentCells[cellRow, cellCol].Equals("D"))
                        {
                            statusParentCells[cellRow, cellCol] = "X";

                            ClearLines();
                            Console.WriteLine("PLAYER 1 HAS CLAIMED A SLOT\nPress ENTER to continue");
                        }

                    }
                    else if (currentPlayer == player2)
                    {
                        if (!statusParentCells[cellRow, cellCol].Equals("X") && !statusParentCells[cellRow, cellCol].Equals("O") && !statusParentCells[cellRow, cellCol].Equals("D"))
                        {
                            statusParentCells[cellRow, cellCol] = "O";
                            ClearLines();
                            Console.WriteLine("PLAYER 2 HAS CLAIMED A SLOT\nPress ENTER to continue");
                        }
                    }
                }
                else if (gameWon)
                {
                    if (currentPlayer == player1)
                    {
                        ClearLines();
                        Console.WriteLine("PLAYER 1 HAS WON THE GAME!!!\nPress ENTER to continue");
                        
                        wins[player1]++;
                        losses[player2]++;
                        Console.ReadLine();
                        Console.Clear();
                        SaveData("tictactoe.txt");
                        gamestate = GAMEOVER;
                        finalResult = 0;
                    }
                    else if (currentPlayer == player2)
                    {
                        ClearLines();
                        Console.WriteLine("PLAYER 2 HAS WON THE GAME!!!\nPress ENTER to continue");
                        wins[player2]++;
                        losses[player1]++;
                        Console.ReadLine();
                        Console.Clear();
                        SaveData("tictactoe.txt");
                        gamestate = GAMEOVER;
                        finalResult = 1;
                    }
                }
            }
            else if (status == CHILD_DRAW)
            {
                if (IsFullChildGrid() && !statusParentCells[cellRow, cellCol].Equals("X") && !statusParentCells[cellRow, cellCol].Equals("O") && !statusParentCells[cellRow, cellCol].Equals("D"))
                {
                    statusParentCells[cellRow, cellCol] = "D";

                    DrawGame();
                    ClearLines();
                    Console.WriteLine("A DRAW HAS OCCURRED\nPress ENTER to continue");
                    Console.ReadLine();
                    ClearLines();
                }
            }
            else if (status == PARENT_DRAW)
            {   
                if (IsFullParentGrid())
                {
                    DrawGame();
                    ClearLines();
                    Console.WriteLine("THE GAME HAS ENDED IN A DRAW\nPress ENTER to continue");
                    Console.ReadLine();
                    ClearLines();
                    draw++;
                    Console.Clear();
                    SaveData("tictactoe.txt");
                    gamestate = GAMEOVER;
                    finalResult = 2;
                }
            }
        }

        //Pre: None
        //Post: None
        //Desc: Centre text with desired colour and position
        private static void CentreText(string text, ConsoleColor colour, int yPos)
        {
            Console.SetCursorPosition(Console.WindowWidth / 2 - text.Length / 2, yPos);
            Console.ForegroundColor = colour;
            Console.Write(text);
            Console.ResetColor();
        }

        //Pre: None
        //Post: None
        //Desc: Reposition the cursor to a specific point 
        private static void RepositionCursor()
        {
            Console.SetCursorPosition(0, 15);
        }

        //Pre: None
        //Post: None
        //Desc: Clear lines beneath a certain point
        private static void ClearLines()
        {
            int consoleLines = 10;

            RepositionCursor();

            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < consoleLines; i++)
            {
                Console.Write(new string(' ', Console.WindowWidth));
            }

            RepositionCursor();
        }

        //Pre: None
        //Post: None
        //Desc: Save player data to text file
        private static void SaveData(string filePath)
        {
            try
            {
                outFile = File.CreateText(filePath);

                outFile.WriteLine(wins[player1] + "," + losses[player1] + "," + draw);
                outFile.WriteLine(wins[player2] + "," + losses[player2] + "," + draw);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: " + e.Message);
            }
            finally
            {
                if (outFile != null)
                {
                    outFile.Close();
                }
            }
        }
        
        //Pre: None
        //Post: None
        //Desc: Read data in to game 
        private static void ReadFile(string filePath)
        {
            try
            {
                string[] data;

                inFile = File.OpenText(filePath);

                    for (int i = 0; i < NUM_PLAYERS; i++)
                    {

                        data = inFile.ReadLine().Split(',');

                        wins[i] = Convert.ToInt32(data[0]);
                        losses[i] = Convert.ToInt32(data[1]);
                        draw = Convert.ToInt32(data[2]);
                    }

            }
            catch (FileNotFoundException fnf)
            {
                Console.WriteLine("ERROR: " + fnf.Message);
            }
            catch (FormatException fe)
            {
                Console.WriteLine("ERROR: " + "File was not properly saved");
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: " + e.Message);
            }
            finally
            {
                if (inFile != null)
                {
                    inFile.Close();
                }
            }
        }
    }
}