/*****************************************************************************\
|* *|
\*****************************************************************************/
using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using static mineSweeper;
public class mineSweeper
{

    // |* Game Constants *|
    static readonly int ROWS = 5;
    static readonly int COLS = 10;
    static readonly int MINES = 2;
    static readonly string FLAG = "flag";
    static readonly string UNFLAG = "unflag";
    static readonly string SWEEP = "sweep";
    static readonly string FLAG_SYMBOL = "#";
    static readonly string MINE_SYMBOL = "*";
    //|* Game State *|
    bool isFirstSweep, isGameLost;
    Gridcell[,] cells;
    
    //|* Other Game Data Objects and Components *|
    public class Gridcell
    {
        public int mineCount = 0;
        public bool isFlagged = false;
        public bool isSwept = false;
        public bool isMine = false;

        public Gridcell() : this(0, false, false, false) { }

        public Gridcell(int mc, bool ifl, bool isp, bool imn)
        {
            mineCount = mc;
            isFlagged = ifl;
            isMine = imn;
            isSwept = isp;

        }
    }
   
    public static void Main(string[] arg)
    {
        mineSweeper ag = new mineSweeper();
        ag.Start();
    }
    /**************************************************************************\
    |* *|
    \**************************************************************************/
    public mineSweeper()
    {     
    }
    /**************************************************************************\
    |* *|
    \**************************************************************************/
    public void Start()
    {
        string input;
        Init(); // 1. Initialize Variables
        ShowGameStartScreen(); // 2. Show Game Start Screen
        do
        {
            ShowBoard(); // 3. Show Board / Scene / Map
            do
            {
                ShowInputOptions(); // 4. Show Input Options
                input = GetInput(); // 5. Get Input
            }
            while (!IsValidInput(input)); // 6. Validate Input
            ProcessInput(input); // 7. Process Input
            UpdateGameState(); // 8. Update Game State
        }
        while (!IsGameOver()); // 9. Check for Termination Conditions
        ShowGameOverScreen(); // 10. Show Game Results
    }
    /**************************************************************************\
    |* *|
    \**************************************************************************/
    public void Init()
    {
        isFirstSweep = true;
        isGameLost = false;
        cells = new Gridcell[15,20];

        for (int i = 0; i < cells.GetLength(0); i++)
        {
            for (int j = 0; j < cells.GetLength(1); j++)
            {
                cells[i, j] = new Gridcell();
            }
        }

        cells[0, 3].mineCount = 0;
    }
    /**************************************************************************\
    |* *|
    \**************************************************************************/
    public void ShowGameStartScreen()
    {
        Console.WriteLine("WELCOME TOOOOO!!!!");
        Console.WriteLine(@" 

                               _____  .___ _______  ___________                        
                              /     \ |   |\      \ \_   _____/                        
                             /  \ /  \|   |/   |   \ |    __)_                         
                            /    Y    \   /    |    \|        \                        
                            \____|__  /___\____|__  /_______  /                        
                                    \/            \/        \/                         
              ___________      _______________________________________________________ 
             /   _____/  \    /  \_   _____/\_   _____/\______   \_   _____/\______   \
             \_____  \\   \/\/   /|    __)_  |    __)_  |     ___/|    __)_  |       _/
             /        \\        / |        \ |        \ |    |    |        \ |    |   \
            /_______  / \__/\  / /_______  //_______  / |____|   /_______  / |____|_  /
                    \/       \/          \/         \/                   \/         \/ 
");
    }

    public void ShowBoard()
    {
        string board = "   ";

        // Mostrar encabezado de columnas con números
        for (int j = 0; j < COLS; j++)
            board += $" {j}";
        board += Environment.NewLine;

        board += "  +" + new string('-', COLS * 2 - 1) + "+" + Environment.NewLine;

        for (int i = 0; i < ROWS; i++)
        {
            board += i + " |";

            for (int j = 0; j < COLS; j++)
            {
                Gridcell cell = cells[i, j];

                if (cell.isFlagged)
                    board += $" {FLAG_SYMBOL}";
                else if (!cell.isSwept)
                    board += " .";
                else if (cell.isMine)
                    board += $" {MINE_SYMBOL}";
                else
                    board += $" {cell.mineCount}";
            }

            board += $" | {i}" + Environment.NewLine;
        }

        board += "  +" + new string('-', COLS * 2 - 1) + "+" + Environment.NewLine;

        // Mostrar pie de columnas con números
        board += "   ";
        for (int j = 0; j < COLS; j++)
            board += $" {j}";

        Console.WriteLine(board);
    }
    /**************************************************************************\
    |* *|
    \**************************************************************************/
    public void ShowInputOptions()
    {
        Console.WriteLine($"Enter [ {FLAG} | {UNFLAG} |  {SWEEP}] Between the rows [0-{ROWS}] and the cols [0-{COLS}]: ");
    }
   
    public string GetInput()
    {
        Console.Write("Your Move: ");
        string input = Console.ReadLine();
        input = input.ToLower().Trim(); 
        return input;
    }

    public bool IsValidInput(string input)
    {

        try
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.Clear();
                ShowBoard();
                Console.WriteLine($"Your move need to began with one of these trhee moves {FLAG} | {UNFLAG} | {SWEEP} and then the cordenates you want to move.\nPlease try it again :) ");
               // ShowBoard();
                return false;
            }
            

            string pattern = "\\s+"; // one or more white spaces
            string[] tokens = Regex.Split(input, pattern);



            if (tokens.Length != 3)
            {
                Console.Clear();
                ShowBoard();
                Console.WriteLine($"You have to put your move and the cordenates where you want to move. EXAMPLE {FLAG} 0 1. \nPlease try again :).");
               // ShowBoard();
                return false;
            }
            string move = tokens[0];
            string rowMove = tokens[1];
            string colMove = tokens[2];
           int  userRow = int.Parse(rowMove);
            int userCol = int.Parse (colMove);

           if (move != FLAG && move != UNFLAG && move != SWEEP)
            {
                Console.Clear();
                ShowBoard();
                Console.WriteLine($"\nYour move has to begin with one of these three moves: {FLAG} | {UNFLAG} | {SWEEP}. \nPlease Try Again. :) \n");
                //ShowBoard();
                return false;
            }
           else if (rowMove == null || colMove == null)
            {   Console.Clear();
                ShowBoard();
                Console.WriteLine($"You can't have an empty space. You have tu write your move like this: {FLAG} 0 1. \nPlease try again :).");
                return false; 
            }
           else if (userRow > ROWS || userCol > COLS )
           {
                Console.Clear();
                ShowBoard();
                Console.WriteLine("You cannot write a number greater than the number of rows or columns. The number must be [0-5], [0-9].\nPlease Try again :)))...");
               // ShowBoard();
                return false;

           }
            else
            {
                return true;
            }

        }
        catch (FormatException ex)
        {
            Console.Clear();
            ShowBoard();
            Console.WriteLine($"You have to write first one of these options {FLAG} | {UNFLAG} | {SWEEP} and then the cordenates. EXAMPLE {FLAG} 0 1. \nPlease try again :).");
            //ShowBoard();
            return false;
        }


      



    }
    
        public void ProcessInput(string input)
    {
        string[] tokens = Regex.Split(input, "\\s+");
        string action = tokens[0];
        int row = int.Parse(tokens[1]);
        int col = int.Parse(tokens[2]);

        if (action == FLAG)
        {
            cells[row, col].isFlagged = true;
        }
        else if (action == UNFLAG)
        {
            cells[row, col].isFlagged = false;
        }
        else if (action == SWEEP)
        {
            if (isFirstSweep)
            {
                PlaceMines(row, col);
                isFirstSweep = false;
            }

            if (cells[row, col].isMine)
            {
                isGameLost = true;
                SweepAllCells();
            }
            else
            {
                SweepCell(row, col);
            }
        }
        else
        {
            Console.WriteLine("Something went really wrong! This is never supposed to happen!");
        }
    }
    private void PlaceMines(int initialRow, int initialCol)
    {
        Random rand = new Random();
        int placed = 0;

        while (placed < MINES)
        {
            int r = rand.Next(0, ROWS);
            int c = rand.Next(0, COLS);

            if ((r == initialRow && c == initialCol) || cells[r, c].isMine)
                continue;

            cells[r, c].isMine = true;
            placed++;

            for (int i = -1; i <= 30; i++)
            {
                for (int j = -1; j <= 30; j++)
                {
                    int nr = r + i;
                    int nc = c + j;
                    if (nr >= 0 && nr < ROWS && nc >= 0 && nc < COLS && !(i == 0 && j == 0))
                    {
                        cells[nr, nc].mineCount++;
                    }
                }
            }
        }
    }

    private void SweepAllCells()
    {
        for (int i = 0; i < ROWS; i++)
            for (int j = 0; j < COLS; j++)
                cells[i, j].isSwept = true;
    }

    private void SweepCell(int row, int col)
    {
        if (row < 0 || row >= ROWS || col < 0 || col >= COLS)
            return;

        Gridcell cell = cells[row, col];
        if (cell.isSwept || cell.isFlagged)
            return;

        cell.isSwept = true;

        if (cell.mineCount == 0)
        {
            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                    if (!(i == 0 && j == 0))
                        SweepCell(row + i, col + j);
        }
    }
    /**************************************************************************\
    |* *|
    \**************************************************************************/
   public void UpdateGameState()
    {
        
    }
    /**************************************************************************\
    |* *|
    \**************************************************************************/
    public bool IsGameOver()
    {
        if (isGameLost)
            return true;

        for (int i = 0; i < ROWS; i++)
        {
            for (int j = 0; j < COLS; j++)
            {
                if (!cells[i, j].isMine && !cells[i, j].isSwept)
                    return false;
            }
        }
        return true;
    }
    /**************************************************************************\
    |* *|
    \**************************************************************************/
    public void ShowGameOverScreen()
    {
        ShowBoard();
        Console.WriteLine("Game Over!");
        if (isGameLost)
        {
            Console.WriteLine("You Lost! You exploded into meat chunks and blood mist!");
        }
        else
        {
            Console.WriteLine("Congrats! You Won! You managed to clear the minefield without getting blown to pieces!");
        }
    }
}