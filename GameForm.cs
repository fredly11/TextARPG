using MySql.Data.MySqlClient;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public class GameForm : Form
{
    private TextBox gameOutput;
    private TextBox inputBox;
    private GameManager gameManager;

    public GameForm()
    {
        // Game manager instance
        gameManager = GameManager.Instance;

        // Form properties
        this.Text = "Text-Based RPG";
        this.Size = new Size(800, 600);
        this.BackColor = Color.Black;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;

        // Input box setup
        inputBox = new TextBox
        {
            Dock = DockStyle.Fill,
            Font = new Font("Consolas", 14, FontStyle.Bold),
            BackColor = Color.Black,
            ForeColor = Color.Orange,
            BorderStyle = BorderStyle.None,
            Multiline = false,
            TabStop = true
        };
        // Panel around input box for border
        var inputPanel = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 26,
            BackColor = Color.Orange,
            Padding = new Padding(2)
        };

        inputPanel.Controls.Add(inputBox);
        this.Controls.Add(inputPanel);

        Debug.WriteLine("Creating Game Output");
        // The output area of the game for displaying game elements
        gameOutput = new TextBox
        {
            Multiline = true,
            ReadOnly = true,
            Dock = DockStyle.Fill,
            Font = new Font("Consolas", 14, FontStyle.Bold),
            BackColor = Color.Black,
            ForeColor = Color.Orange,
            BorderStyle = BorderStyle.None,
            TabStop = false
        };

        gameOutput.Enter += (s, e) => inputBox.Focus();
        this.Controls.Add(gameOutput);

        Debug.WriteLine("Setting game output in game manager");
        gameManager.SetGameOutput(gameOutput); 

        inputBox.KeyDown += InputBox_KeyDown;

        // Start the game on load
        this.Load += (s, e) =>
        {
            inputBox.Focus();
            gameManager.ProcessInput(""); 
        };
    }

// Key input handling
private void InputBox_KeyDown(object sender, KeyEventArgs e)
{
    if (e.KeyCode == Keys.Enter)
    {
        e.SuppressKeyPress = true;  // Prevents the beep sound
        gameManager.ProcessInput(inputBox.Text.Trim());
        inputBox.Clear();
    }
    else if (e.KeyCode == Keys.Up)
    {
        e.SuppressKeyPress = true;  // Prevents the beep sound
        // Instead of sending "up", you can directly trigger the scroll action in the game manager or screen
        if (gameManager.currentScreen is CharacterMenuScreen characterMenuScreen)
        {
            characterMenuScreen.ScrollUp();
        }
    }
    else if (e.KeyCode == Keys.Down)
    {
        e.SuppressKeyPress = true;  // Prevents the beep sound
        // Similarly for the down arrow key, directly trigger the scroll action
        if (gameManager.currentScreen is CharacterMenuScreen characterMenuScreen)
        {
            characterMenuScreen.ScrollDown();
        }
    }
}

    //Main function for running the program
    [DllImport("kernel32.dll")]
    static extern bool AllocConsole();

    [STAThread]
    public static void Main()
    {
        AllocConsole(); 

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new GameForm());
    }
}
