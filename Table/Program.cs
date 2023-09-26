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
        //Track gamestate
        static int gamestate = MENU;

        //Store gamestates
        const int MENU = 1;
        const int STATISTICS = 2;
        const int GAMEPLAY = 3;
        const int GAMEOVER = 4;

        //Store grid information
        const int ROW = 3;
        const int COL = 3;
        static int initgridLocX = 2;
        static int initgridLocY = 3;
        static int gridLocX = initgridLocX;
        static int gridLocY = initgridLocY;

        //Store the elements in each grid that will be displayed
        static string[,,,] displayedGrid = new string[ROW, COL, ROW, COL];

        //Store numbering (1-9) of each cell that can be selected
        static int[,] selectionCells = new int[ROW, COL];

        //Track selection of cell
        static bool isSelected = false;
        static int precellRow = 0;
        static int precellCol = 0;
        static int cellRow = 0;
        static int cellCol = 0;

        //Store status of a parent cell
        static string[,] statusParentCells = new string[ROW, COL];

        //Track player information
        const int NUM_PLAYERS = 2;
        static int player1 = 0;
        static int player2 = 1;
        static int currentPlayer = player1;
        static bool player1Turn = true;
        static bool player2Turn = false;
        static int[] wins = new int[] { 0, 0 };
        static int[] losses = new int[] { 0, 0 };
        static int draws = 0;
        static int games = 0;


        //Track the win and draw status
        static bool gameWon = false;
        const int WIN = 1;
        const int CHILD_DRAW = 2;
        const int PARENT_DRAW = 3;
        static int finalResult = -1;

        static StreamWriter outFile = null;
        static StreamReader inFile = null;

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
                //Update and Display different gamestates
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
            //Calculate total number of games played
            games = wins[player1] + losses[player1] + draws;
        }

        //Pre: None
        //Post: None
        //Desc: Handle all game related functionality
        private static void UpdateGame()
        {
            while (true)
            {
                //Change colour 
                Console.ForegroundColor = ConsoleColor.White;

                //Initialize game data before actually playing
                InitializeData();

                //Display game elements
                DrawGame();

                //Choose which parent cell to play in
                ChooseParentCell();

                //Back out if game is over
                if (gamestate == GAMEOVER) break;
            }
        }

        //Pre: None
        //Post: None
        //Desc: Handle input in the game over screen
        private static void UpdateEndgame()
        {
            //Reset game data 
            InitializeData();
            Console.ReadLine();

            //Change color
            Console.ForegroundColor = ConsoleColor.White;

            //Change gamestate
            gamestate = MENU;
        }

        //Pre: None
        //Post: None
        //Desc: Display the menu 
        private static void DrawMenu()
        {
            //Clear screen
            Console.Clear();

            //Display menu elements
            CentreText("TIKI TAKI TOE", ConsoleColor.Green, 1);               
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
            //Store table element information
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
            string[] p1Stats = new string[] { "1", Convert.ToString(games), Convert.ToString(wins[player1]), Convert.ToString(losses[player1]), Convert.ToString(draws) };
            string[] p2Stats = new string[] { "2", Convert.ToString(games), Convert.ToString(wins[player2]), Convert.ToString(losses[player2]), Convert.ToString(draws) };

            //Display title
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

            //Move to next row
            tabLocY += vertSpace;

            //Display player 1 stats
            for (int i = 0; i < playerDispNum; i++)
            {
                //Change color to player 1's color
                Console.ForegroundColor = ConsoleColor.Magenta;

                //Set cursor position to left side of table and display stats respectively from left to right
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

            //Change color
            Console.ForegroundColor = ConsoleColor.White;

            //Move to next row
            tabLocY += vertSpace;

            //Display player 2 stats
            for (int i = 0; i < playerDispNum; i++)
            {
                //Change color to player 2's color
                Console.ForegroundColor = ConsoleColor.Cyan;

                //Set cursor position to left side of table and display stats respectively from left to right
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

            //Change color
            Console.ForegroundColor = ConsoleColor.White;

            //Display table frame
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

            //Display message for continuing
            CentreText("Press ENTER to continue", ConsoleColor.Red, 16);
            Console.ReadLine();

            //Change gamestate
            gamestate = MENU;
        }

        //Pre: None
        //Post: None
        //Desc: Display all game elements
        private static void DrawGame()
        {
            //Change color
            Console.ForegroundColor = ConsoleColor.Green;

            //Display title of main board
            Console.SetCursorPosition(initgridLocX, 0);
            Console.WriteLine("MAIN BOARD");
            Console.SetCursorPosition(initgridLocX, 1);
            Console.WriteLine("==========");

            //Change color
            Console.ForegroundColor = ConsoleColor.White;

            //Initialize grid settings for mini grid
            int miniInitgridX = 30;
            int miniInitgridY = 3;
            int miniGridLocX = miniInitgridX;
            int miniGridLocY = miniInitgridY;

            //Set grid location modifiers 
            int signalRemainder = 2;
            int mainGridDimension = 3;
            int[] mainDist = new int[] { 3, 7 };
            int mainTotal = 11;
            int miniGridDimension = 2;
            int miniGridWidth = 6;
            int[] miniDist = new int[] { 1, 3 };
            int miniTotal = 5;


            //Display the value in each child cell
            for (int row1 = 0; row1 < ROW; row1++)
            {
                for (int col1 = 0; col1 < COL; col1++)
                {
                    for (int row2 = 0; row2 < ROW; row2++)
                    {
                        for (int col2 = 0; col2 < COL; col2++)
                        {
                            //Set cursor position to where x and y of active grid displaying location are
                            Console.SetCursorPosition(gridLocX, gridLocY);

                            //Display the value
                            Console.Write(displayedGrid[row1, col1, row2, col2]);

                            //Increment active grid x displaying location so next value can be displayed on the right
                            gridLocX++;
                        }

                        //Increment active grid y displaying location so next value can be displayed beneath
                        gridLocY++;

                        //Write at next parent cell's location when current child cell's last row has been displayed or continue writing next row of numbers in child cell
                        if (row2 % 3 == signalRemainder)
                        {
                            //Write at following parent cell's location 
                            gridLocX++;
                        }
                        else
                        {
                            //Continue writing next row of numbers in child cell
                            gridLocX -= mainGridDimension;
                        }
                    }

                    //Write at next parent cell's location (leftmost column on next row) when parent cell's last column has been displayed on right or continue writing to next parent cell on right
                    if (col1 % 3 == signalRemainder)
                    {
                        //Write at leftmost column on next row 
                        gridLocY++; ;  
                        gridLocX = initgridLocX;
                    }
                    else
                    {
                        //Continue writing to next parent cell on right
                        gridLocY -= mainGridDimension;
                    }
                }

                //Display the horizontal component of the main board
                for (int i = 0; i < mainTotal; i++)
                {
                    Console.SetCursorPosition(initgridLocX + i, initgridLocY + mainDist[0]);
                    Console.Write("═");
                    Console.SetCursorPosition(initgridLocX + i, initgridLocY + mainDist[1]);
                    Console.Write("═");
                }

                //Display the vertical component of the main board
                for (int j = 0; j < mainTotal; j++)
                {
                    Console.SetCursorPosition(initgridLocX + mainDist[0], initgridLocY + j);
                    Console.Write("║");
                    Console.SetCursorPosition(initgridLocX + mainDist[1], initgridLocY + j);
                    Console.Write("║");
                }
            }

            //Change color
            Console.ForegroundColor = ConsoleColor.Green;


            //Display label of status(mini) board
            Console.SetCursorPosition(miniInitgridX - 3, 0);
            Console.WriteLine("STATUS BOARD");
            Console.SetCursorPosition(miniInitgridX - 3, 1);
            Console.WriteLine("============");

            //Change color
            Console.ForegroundColor = ConsoleColor.White;

            //Set cursor position to where mini board initially begins drawing
            Console.SetCursorPosition(miniInitgridX, miniInitgridY);

            //Display the horizontal component in the mini board
            for (int i = 0; i < miniTotal; i++)
            {
                Console.SetCursorPosition(miniInitgridX + i, miniInitgridY + miniDist[0]);
                Console.Write("═");
                Console.SetCursorPosition(miniInitgridX + i, miniInitgridY + miniDist[1]);
                Console.Write("═");
            }

            //Display the vertical component in the mini board
            for (int j = 0; j < miniTotal; j++)
            {
                Console.SetCursorPosition(miniInitgridX + miniDist[0], miniInitgridY + j);
                Console.Write("║");
                Console.SetCursorPosition(miniInitgridX + miniDist[1], miniInitgridY + j);
                Console.Write("║");
            }

            //Display status of each parent cell using mini board
            for (int row = 0; row < ROW; row++)
            {
                for (int col = 0; col < COL; col++)
                {
                    //Set cursor position to where x and y of active mini grid displaying location are
                    Console.SetCursorPosition(miniGridLocX, miniGridLocY);
                    Console.Write(statusParentCells[row, col]);

                    //Write status at next spot on the right
                    miniGridLocX += miniGridDimension;
                }

                //Write status in next row and first column
                miniGridLocY += miniGridDimension;
                miniGridLocX -= miniGridWidth;
            }

            //Reset coords of active main grid displaying location
            gridLocX = initgridLocX;
            gridLocY = initgridLocY;

            //Colour the cell 
            ColouringCell();

            //Reposition the cursor to where prompts are shown
            RepositionCursor();
        }

        //Pre: None
        //Post: None
        //Desc: Display the game over screen
        private static void DrawEndgame()
        {
            int playerLabel = 7;
            int contMsg = 8;
            //Clear console
            Console.Clear();

            //Display final result
            CentreText("##############################################################", ConsoleColor.Green, 6);

            //Display who won
            if (finalResult == 0)
            {
                CentreText("PLAYER 1 HAS WON THE GAME!!", ConsoleColor.Magenta, playerLabel);
                CentreText("PRESS ENTER TO CONTINUE", ConsoleColor.Red, contMsg);
            }
            else if (finalResult == 1)
            {
                CentreText("PLAYER 2 HAS WON THE GAME!!\nPRESS ENTER TO CONTINUE", ConsoleColor.Cyan, playerLabel);
                CentreText("PRESS ENTER TO CONTINUE", ConsoleColor.Red, contMsg);
            }
            if (finalResult == 2)
            {
                CentreText("THE GAME HAS ENDED IN A DRAW!!", ConsoleColor.Yellow, playerLabel);
                CentreText("PRESS ENTER TO CONTINUE", ConsoleColor.Red, contMsg);
            }

            CentreText("##############################################################", ConsoleColor.Green, 9);
        }

        //Pre: None
        //Post: None
        //Desc: Initializing all game data before actual gameplay occurs
        private static void InitializeData()
        {
            //Reset all data 
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

            //Set multiplier for storing numbers
            int multi = 3;

            //Initialize numbers for row and length in each child cell (123456789)
            for (int row1 = 1; row1 <= ROW; row1++)
            {
                for (int col1 = 1; col1 <= COL; col1++)
                {
                    for (int row2 = 1; row2 <= ROW; row2++)
                    {
                        for (int col2 = 1; col2 <= COL; col2++)
                        {
                            displayedGrid[row1 - 1, col1 - 1, row2 - 1, col2 - 1] = Convert.ToString((row2 - 1) * multi + col2);
                        }
                    }
                }
            }

            //Initialize numbers for selection of cells  
            for (int row = 1; row <= ROW; row++)
            {
                for (int col = 1; col <= COL; col++)
                {
                    selectionCells[row - 1, col - 1] = (row - 1) * multi + col;
                }
            }

            //Initialize status of parent cells
            for (int row = 1; row <= ROW; row++)
            {
                for (int col = 1; col <= COL; col++)
                {
                    statusParentCells[row - 1, col - 1] = "";
                }
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
            //Change color
            Console.ForegroundColor = ConsoleColor.White;

            //Set multiplier for determining cursorposition
            int cursPosMulti = 4;


            //Change the colour of the previous cell that was moved from to white so it shows it is not the one currently being chosen
            if (!isSelected)
            {
                //Upon moving, the cell that was moved either up or down from, turns white
                if (precellRow > cellRow)
                {
                    for (int row1 = precellRow; row1 < precellRow + 1; row1++)
                    {
                        for (int col1 = precellCol; col1 < precellCol + 1; col1++)
                        {
                            for (int row2 = 0; row2 < 3; row2++)
                            {
                                for (int col2 = 0; col2 < 3; col2++)
                                {
                                    //Change colour upon moving up
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.SetCursorPosition(initgridLocX + (cursPosMulti * col1) + col2, initgridLocY + (cursPosMulti * row1) + row2);
                                    Console.Write(displayedGrid[row1, col1, row2, col2]);

                                }
                            }
                        }
                    }
                }
                else if (precellRow < cellRow)
                {
                    for (int row1 = precellRow; row1 < precellRow + 1; row1++)
                    {
                        for (int col1 = precellCol; col1 < precellCol + 1; col1++)
                        {
                            for (int row2 = 0; row2 < 3; row2++)
                            {
                                for (int col2 = 0; col2 < 3; col2++)
                                {
                                    //Change colour upon moving down
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.SetCursorPosition(initgridLocX + (cursPosMulti * col1) + col2, initgridLocY + (cursPosMulti * row1) + row2);
                                    Console.Write(displayedGrid[row1, col1, row2, col2]);

                                }
                            }
                        }
                    }
                }

                //Upon moving, the cell that was moved either right or left from, turns white
                if (precellCol < cellCol)
                {
                    for (int row1 = precellRow; row1 < precellRow + 1; row1++)
                    {
                        for (int col1 = precellCol; col1 < precellCol + 1; col1++)
                        {
                            for (int row2 = 0; row2 < 3; row2++)
                            {
                                for (int col2 = 0; col2 < 3; col2++)
                                {
                                    //Change colour upon moving right
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.SetCursorPosition(initgridLocX + (cursPosMulti * col1) + col2, initgridLocY + (cursPosMulti * row1) + row2);
                                    Console.Write(displayedGrid[row1, col1, row2, col2]);

                                }
                            }
                        }
                    }
                }
                else if (precellCol > cellCol)
                {
                    for (int row1 = precellRow; row1 < precellRow + 1; row1++)
                    {
                        for (int col1 = precellCol; col1 < precellCol + 1; col1++)
                        {
                            for (int row2 = 0; row2 < 3; row2++)
                            {
                                for (int col2 = 0; col2 < 3; col2++)
                                {
                                    //Change colour upon moving left
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.SetCursorPosition(initgridLocX + (cursPosMulti * col1) + col2, initgridLocY + (cursPosMulti * row1) + row2);
                                    Console.Write(displayedGrid[row1, col1, row2, col2]);

                                }
                            }
                        }
                    }
                }
            }

            //Change the colour of the current hovered/selected cell based on if it is chosen or not 
            for (int row1 = cellRow; row1 < cellRow + 1; row1++)
            {
                for (int col1 = cellCol; col1 < cellCol + 1; col1++)
                {
                    //each child cell
                    for (int row2 = 0; row2 < ROW; row2++)
                    {
                        for (int col2 = 0; col2 < COL; col2++)
                        {
                            //Modify the colour based on chosen or not
                            if (!isSelected)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                            }
                            else
                            {
                                //Change colour based on player 
                                if (player1Turn)
                                {
                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                }
                                else if (player2Turn)
                                {
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                }
                            }
                            Console.SetCursorPosition(initgridLocX + (cursPosMulti * col1) + col2, initgridLocY + (cursPosMulti * row1) + row2);
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

            //Change color
            Console.ForegroundColor = ConsoleColor.White;

            //Display prompt
            Console.WriteLine("Use WASD to choose slot\nPress ENTER to confirm");

            //User selects which parent cell is played in until confirmation (enter) is pressed
            do
            {
                key = Console.ReadKey(true).Key;
                
                //Navigate between cells using WASD keys
                switch (key)
                {
                    case ConsoleKey.W:
                        if (cellRow > 0)
                        {
                            //Track the previous cell row and column 
                            precellRow = cellRow;
                            cellRow--;
                            precellCol = cellCol;

                            //Update the colour
                            ColouringCell();
                        }
                        break;
                    case ConsoleKey.S:
                        if (cellRow < 2)
                        {
                            //Track the previous cell row and column 
                            precellRow = cellRow;
                            cellRow++;
                            precellCol = cellCol;

                            //Update the colour
                            ColouringCell();
                        }
                        break;
                    case ConsoleKey.D:
                        if (cellCol < 2)
                        {
                            //Track the previous cell row and column 
                            precellCol = cellCol;
                            cellCol++;
                            precellRow = cellRow;

                            //Update the colour
                            ColouringCell();
                        }
                        break;
                    case ConsoleKey.A:
                        if (cellCol > 0)
                        {
                            //Track the previous cell row and column 
                            precellCol = cellCol;
                            cellCol--;
                            precellRow = cellRow;

                            //Update the colour
                            ColouringCell();
                        }
                        break;
                }
            }
            while (key != ConsoleKey.Enter);

            //Upon conformation (pressing enter), continue next game functions 
            if (key == ConsoleKey.Enter )
            {
                //When the game is not over, update colour, check if the next parent cell is already claimed, and choose a child cell
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
            //Player claims a child slot
            for (int i = 0; i < selectionCells.GetLength(0); i++)
            {
                for (int j = 0; j < selectionCells.GetLength(1); j++)
                {
                    //Detect which child cell the choice was in
                    if (selectionCells[i, j] == choice)
                    {
                        //Change the child slot to X or O if it isn't already claimed
                        if (!displayedGrid[cellRow, cellCol, i, j].Equals("X") && !displayedGrid[cellRow, cellCol, i, j].Equals("O"))
                        {
                            //Change to respective values based on the player
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
                            //Display spot has been claimed and allow user to try again
                            ClearLines();
                            Console.WriteLine("This spot is occupied\nPress ENTER to try again");
                            Console.ReadLine();
                            ClearLines();
                            ChooseChildCell();
                        } 

                        //Continue the game process if the game is not over yet
                        if (gamestate != GAMEOVER)
                        {
                            //Check the status of both the parent and child cells
                            CheckStatus();

                            //Back out if game is over
                            if (gamestate == GAMEOVER) break;

                            //Set new parent cell column and row to what was previously selected in the child cell
                            cellRow = i;
                            cellCol = j;

                            //Draw the game again
                            DrawGame();

                            //Check if the new parent cell is already claimed
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

            //Back out if game is over
            if (gamestate == GAMEOVER) return;

            //Store input from user
            int input = 0;

            //Preparing for prompt
            ClearLines();
            RepositionCursor();

            //Provide prompt and make decisions based on player
            if (currentPlayer == player1)
            {
                //When it is player 1's turn, provide prompts and decisions for them
                while (player1Turn == true)
                {
                    //Provide prompt
                    Console.WriteLine("Player 1's turn (X)");
                    Console.Write("Enter choice: ");

                    //Receive user input and make decision based on it
                    if (Int32.TryParse(Console.ReadLine(), out input) && input >= 1 && input <= 9)
                    {
                        ClearLines();

                        //Change value of cell user chose
                        ProcessChoice(input);

                        //Back out if game is over
                        if (gamestate == GAMEOVER) break;

                        //Display what user chose
                        Console.WriteLine("Player 1 chose: " + input);
                        Console.WriteLine("Press ENTER to continue");
                        Console.ReadLine();
                        ClearLines();

                        //Change player turns
                        player1Turn = false;
                        currentPlayer = player2;
                        player2Turn = true;

                        //Recolor cells and choose child cell again
                        ColouringCell();
                        ChooseChildCell();
                    }
                    else
                    {
                        //Allow user to input again
                        Console.WriteLine("Invalid number!\nPress ENTER to try again");
                        Console.ReadLine();
                        ClearLines();
                    }
                }
            }
            else if (currentPlayer == player2)
            {
                //When it is player 2's turn, provide prompts and decisions for them
                while (player2Turn == true)
                {
                    //Provide prompt
                    Console.WriteLine("Player 2's turn (O)");
                    Console.Write("Enter choice: ");

                    //Receive user input and make decision based on it
                    if (Int32.TryParse(Console.ReadLine(), out input) && input >= 1 && input <= 9)
                    {
                        ClearLines();

                        //Change value of cell user chose
                        ProcessChoice(input);

                        //Back out if game is over
                        if (gamestate == GAMEOVER) break;

                        //Display what user chose
                        Console.WriteLine("Player 2 chose: " + input);
                        Console.WriteLine("Press ENTER to continue");
                        Console.ReadLine();
                        ClearLines();

                        //Change player turns
                        player2Turn = false;
                        currentPlayer = player1;
                        player1Turn = true;

                        //Recolor cells and choose child cell again
                        ColouringCell();
                        ChooseChildCell();
                    }
                    else
                    {
                        //Allow user to input again
                        Console.WriteLine("Invalid number!\nPress ENTER to try again");
                        Console.ReadLine();
                        ClearLines();
                    }
                }
            }

            //Back out if game is over
            if (gamestate != GAMEOVER)
                Console.ReadLine();
        }

        //Pre: None
        //Post: None
        //Desc: Check if the redirected parent cell is claimed
        private static void CheckClaimed()
        {
            //If new parent cell is already claimed, then allow next user to manually choose which parent cell they want to play in next
            if (statusParentCells[cellRow, cellCol].Equals("X") || statusParentCells[cellRow, cellCol].Equals("O") || statusParentCells[cellRow, cellCol].Equals("D"))
            {
                //Preparing for prompt
                RepositionCursor();
                ClearLines();

                //Display issue
                Console.WriteLine("This slot is occupied\nPress ENTER to try again");
                Console.ReadLine();
                ClearLines();

                isSelected = false;

                //Change player turns
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

                //User chooses which parent cell to play in
                ChooseParentCell();
            }
        }

        //Pre: None
        //Post: None
        //Desc: Detect if a win, draw, or none occurs
        private static void CheckStatus()
        {
            //Check for any win or draw in parent cells (between child cells)
            CheckChildStatus();

            //Check for any win or draw in whole game 
            CheckParentStatus();
        }

        //Pre: None
        //Post: None
        //Desc: Check for a win, draw, or nothing in an individual parent cell 
        private static void CheckChildStatus()
        { 
            //Check for a horizontal win
            for (int i = 0; i < ROW; i++)
            {
                //When all 3 cells in a row are equal, then the parent cell has a win
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
                //When all 3 cells in a column are equal, then the parent cell has a win
                if ((displayedGrid[cellRow, cellCol, 0, i].Equals(displayedGrid[cellRow, cellCol, 1, i]))
                    && (displayedGrid[cellRow, cellCol, 1, i].Equals(displayedGrid[cellRow, cellCol, 2, i])))
                {
                    DrawGame();
                    ChangeStatus(WIN);
                    Console.ReadLine();
                    ClearLines();
                }
            }

            //Check diagonal win top left to bottom right, when all 3 cells in a diagonal are equal, then the parent cell has a win
            if ((displayedGrid[cellRow, cellCol, 0, 0].Equals(displayedGrid[cellRow, cellCol, 1, 1]))
                && (displayedGrid[cellRow, cellCol, 1, 1].Equals(displayedGrid[cellRow, cellCol, 2, 2])))
            {
                DrawGame();
                ChangeStatus(WIN);
                Console.ReadLine();
                ClearLines();
            }

            //Check diagonal win top right to bottom left, when all 3 cells in a diagonal are equal, then the parent cell has a win
            if ((displayedGrid[cellRow, cellCol, 0, 2].Equals(displayedGrid[cellRow, cellCol, 1, 1]))
                && (displayedGrid[cellRow, cellCol, 1, 1].Equals(displayedGrid[cellRow, cellCol, 2, 0])))
            {
                DrawGame();
                ChangeStatus(WIN);
                Console.ReadLine();
                ClearLines();
            }

            //Check for any draws
            ChangeStatus(CHILD_DRAW);
        }

        //Pre: None
        //Post: None
        //Desc: Check for a win, draw, or nothing in the whole game
        private static void CheckParentStatus()
        {
            //Check for wins when game is not over
            if (gamestate != GAMEOVER)
            {
                //Check horizontal win
                for (int i = 0; i < ROW; i++)
                {
                    //When all 3 cells in a row are equal (or has a wild card), then the game has a win
                    if ((statusParentCells[i, 0].Equals(statusParentCells[i, 1]) || statusParentCells[i, 0].Equals("D") || statusParentCells[i, 1].Equals("D"))
                         && (statusParentCells[i, 1].Equals(statusParentCells[i, 2]) || statusParentCells[i, 1].Equals("D") || statusParentCells[i, 2].Equals("D")))
                    {
                        //Ensure all 3 cells are not empty
                        if (!statusParentCells[i, 0].Equals("")
                            && !statusParentCells[i, 1].Equals("")
                            && !statusParentCells[i, 2].Equals(""))
                        {
                            if (!(statusParentCells[i, 0].Equals("D") && statusParentCells[i, 1].Equals("D") && statusParentCells[i, 2].Equals("D")))
                            {
                                gameWon = true;
                                DrawGame();
                                ChangeStatus(WIN);

                                //Back out if game is over
                                if (gamestate == GAMEOVER) break;
                                Console.ReadLine();
                            }
                        }
                    }
                }
                //Check vertical win
                for (int i = 0; i < COL; i++)
                {
                    //When all 3 cells in a column are equal (or has a wild card), then the game has a win
                    if ((statusParentCells[0, i].Equals(statusParentCells[1, i]) || statusParentCells[0, i].Equals("D") || statusParentCells[1, i].Equals("D"))
                        && (statusParentCells[1, i].Equals(statusParentCells[2, i]) || statusParentCells[1, i].Equals("D") || statusParentCells[2, i].Equals("D")))
                    {
                        //Ensure all 3 cells are not empty
                        if (!statusParentCells[0, i].Equals("")
                            && !statusParentCells[1, i].Equals("")
                            && !statusParentCells[2, i].Equals(""))
                        {
                            if (!(statusParentCells[0, i].Equals("D") && statusParentCells[1, i].Equals("D") && statusParentCells[2, i].Equals("D")))
                            {
                                gameWon = true;
                                DrawGame();
                                ChangeStatus(WIN);

                                //Back out if game is over
                                if (gamestate == GAMEOVER) break;
                                Console.ReadLine();
                            }
                        }
                    }
                }

                //Check for wins when game is not over
                if (gamestate != GAMEOVER)
                {
                    //Check diagonal win top left to bottom right, when all 3 cells in a diagonal are equal (or has a wild card), then the game has a win
                    if ((statusParentCells[0, 0].Equals(statusParentCells[1, 1]) || statusParentCells[0, 0].Equals("D") || statusParentCells[1, 1].Equals("D"))
                    && (statusParentCells[1, 1].Equals(statusParentCells[2, 2]) || statusParentCells[1, 1].Equals("D") || statusParentCells[2, 2].Equals("D")))
                    {
                        //Ensure all 3 cells are not empty
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

                    //Check diagonal win top right to bottom left, when all 3 cells in a diagonal are equal (or has a wild card), then the game has a win
                    if ((statusParentCells[0, 2].Equals(statusParentCells[1, 1]) || statusParentCells[0, 2].Equals("D") || statusParentCells[1, 1].Equals("D"))
                        && (statusParentCells[1, 1].Equals(statusParentCells[2, 0]) || statusParentCells[1, 1].Equals("D") || statusParentCells[2, 0].Equals("D")))
                    {
                        //Ensure all 3 cells are not empty
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

                    //Check for any draws
                    ChangeStatus(PARENT_DRAW);
                }
            }
        }

        //Pre: None
        //Post: Return if the individual parent cell is full or not
        //Desc: Check if an individual parent cell is full
        private static bool IsFullChildGrid()
        {
            //Track when the child grid is full
            bool isFull = true;

            //Check if every element in child grid is full
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    //Detect for a number
                    int parseVal = 0;

                    //The grid is not full if there is still a number in the grid
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
            //Track when the parent grid is full
            bool isFull = true;

            //Check if every element in parent grid is full
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    //The grid is not full if there is still the default value in the status of the parent cell
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
            //Back out if game is over
            if (gamestate == GAMEOVER) return;

            //When a status is detected, display the result and update status
            if (status == WIN)
            {
                //When the whole game hasn't yet been won, display the result and update the parent cell status, otherwise when it has been won, display the result and proceed to next gamestate
                if (!gameWon)
                {
                    //Display and update data based on current player 
                    if (currentPlayer == player1)
                    {
                        if (!statusParentCells[cellRow, cellCol].Equals("X") && !statusParentCells[cellRow, cellCol].Equals("O") && !statusParentCells[cellRow, cellCol].Equals("D"))
                        {
                            //Change status to a win for player 1
                            statusParentCells[cellRow, cellCol] = "X";

                            ClearLines();
                            Console.WriteLine("PLAYER 1 HAS CLAIMED A SLOT\nPress ENTER to continue");
                        }

                    }
                    else if (currentPlayer == player2)
                    {
                        if (!statusParentCells[cellRow, cellCol].Equals("X") && !statusParentCells[cellRow, cellCol].Equals("O") && !statusParentCells[cellRow, cellCol].Equals("D"))
                        {
                            //Change status to a win for player 2
                            statusParentCells[cellRow, cellCol] = "O";
                            ClearLines();
                            Console.WriteLine("PLAYER 2 HAS CLAIMED A SLOT\nPress ENTER to continue");
                        }
                    }
                }
                else if (gameWon)
                {
                    //Display and update data based on current player, save information to file, and switch to gameover screen
                    if (currentPlayer == player1)
                    {
                        //Display information
                        ClearLines();
                        Console.WriteLine("PLAYER 1 HAS WON THE GAME!!!\nPress ENTER to continue");

                        //Change wins/losses count
                        wins[player1]++;
                        losses[player2]++;
                        Console.ReadLine();
                        Console.Clear();

                        //Save data to file
                        SaveData("tictactoe.txt");

                        //Change gamestate
                        gamestate = GAMEOVER;

                        //Indicate which player won
                        finalResult = 0;
                    }
                    else if (currentPlayer == player2)
                    {
                        //Display information
                        ClearLines();
                        Console.WriteLine("PLAYER 2 HAS WON THE GAME!!!\nPress ENTER to continue");

                        //Change wins/losses count
                        wins[player2]++;
                        losses[player1]++;
                        Console.ReadLine();
                        Console.Clear();

                        //Save data to file
                        SaveData("tictactoe.txt");

                        //Change gamestate
                        gamestate = GAMEOVER;

                        //Indicate which player won
                        finalResult = 1;
                    }
                }
            }
            else if (status == CHILD_DRAW)
            {
                if (IsFullChildGrid() && !statusParentCells[cellRow, cellCol].Equals("X") && !statusParentCells[cellRow, cellCol].Equals("O") && !statusParentCells[cellRow, cellCol].Equals("D"))
                {
                    //Change status to a draw
                    statusParentCells[cellRow, cellCol] = "D";

                    //Draw the game again
                    DrawGame();
                    ClearLines();

                    //Display information
                    Console.WriteLine("A DRAW HAS OCCURRED\nPress ENTER to continue");
                    Console.ReadLine();
                    ClearLines();
                }
            }
            else if (status == PARENT_DRAW)
            {
                if (IsFullParentGrid())
                {
                    //Draw the game again to show result
                    DrawGame();
                    ClearLines();

                    //Display information
                    Console.WriteLine("THE GAME HAS ENDED IN A DRAW\nPress ENTER to continue");
                    Console.ReadLine();
                    ClearLines();

                    //Change draw count
                    draws++;
                    Console.Clear();

                    //Save data to file
                    SaveData("tictactoe.txt");

                    //Change gamestate
                    gamestate = GAMEOVER;

                    //Indicate draw
                    finalResult = 2;
                }
            }
        }

        //Pre: None
        //Post: None
        //Desc: Centre text with desired colour and position
        private static void CentreText(string text, ConsoleColor colour, int yPos)
        {
            //Set Cursor Position to point where text will be centered despite it's length
            Console.SetCursorPosition(Console.WindowWidth / 2 - text.Length / 2, yPos);

            //Change color
            Console.ForegroundColor = colour;

            //Display text
            Console.Write(text);

            //Reset color
            Console.ResetColor();
        }

        //Pre: None
        //Post: None
        //Desc: Reposition the cursor to a specific point to provide prompts for user.
        private static void RepositionCursor()
        {
            int promptX = 0;
            int promptY = 15;

            //Set cursor position to specific point
            Console.SetCursorPosition(promptX, promptY);
        }

        //Pre: None
        //Post: None
        //Desc: Clear lines beneath a certain point
        private static void ClearLines()
        {
            //Store how many lines to clear in console
            int consoleLines = 10;

            //Set cursor position to certain position
            RepositionCursor();

            //Change colour
            Console.ForegroundColor = ConsoleColor.White;

            //Clear the lines
            for (int i = 0; i < consoleLines; i++)
            {
                Console.Write(new string(' ', Console.WindowWidth));
            }

            //Set cursor position to certain position
            RepositionCursor();
        }

        //Pre: None
        //Post: None
        //Desc: Save player data to text file
        private static void SaveData(string filePath)
        {
            try
            {
                //Instantiate variable
                outFile = File.CreateText(filePath);

                //Write stats to file
                outFile.WriteLine(wins[player1] + "," + losses[player1] + "," + draws);
                outFile.WriteLine(wins[player2] + "," + losses[player2] + "," + draws);
            }
            catch (Exception e)
            {
                //Display general error message
                Console.WriteLine("ERROR: " + e.Message);
            }
            finally
            {
                //Close the file
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
                //Create array for incoming info
                string[] data;

                //Instantiate variable
                inFile = File.OpenText(filePath);

                    //Read in info on each line and store each value into their respective variables
                    for (int i = 0; i < NUM_PLAYERS; i++)
                    {
                        //Read and split the line
                        data = inFile.ReadLine().Split(',');

                        //Store each value in the appropriate array
                        wins[i] = Convert.ToInt32(data[0]);
                        losses[i] = Convert.ToInt32(data[1]);
                        draws = Convert.ToInt32(data[2]);
                    }

            }
            catch (FileNotFoundException fnf)
            {
                //Display error message when file is not found
                Console.WriteLine("ERROR: File was not found.");
            }
            catch (FormatException fe)
            {
                //Display error message when format exception occurs
                Console.WriteLine("ERROR: File was not properly saved");
            }
            catch (Exception e)
            {
                //Display general error message
                Console.WriteLine("ERROR: " + e.Message);
            }
            finally
            {
                //Close the file
                if (inFile != null)
                {
                    inFile.Close();
                }
            }
        }
    }
}