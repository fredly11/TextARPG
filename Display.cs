using System;
using System.Windows.Forms;
using System.Diagnostics;

// Class for handlingthe display on the form
public class Display
{// Function for updating the screen with mew display data from the screen objects and adding the border, input history, and system message
    public static void UpdateUI(TextBox gameOutput, string[] displayLines, string lastUserInput, string systemMessage)
    {
        gameOutput.Clear();

        // Add top border
        gameOutput.AppendText(new string('=', 77) + Environment.NewLine);

        // Info display lines (lines 2-21), the space available for information from screen objects
        int maxInfoLines = 20; // Max lines for display data
        int lineCount = 0;

        // Add each display line
        foreach (var line in displayLines)
        {
            if (lineCount >= maxInfoLines) break;

            string formattedLine = FormatLine(line);
            gameOutput.AppendText(formattedLine + Environment.NewLine);
            lineCount++;
        }

        // Add bottom border line
        gameOutput.AppendText(new string('=', 77) + Environment.NewLine);

        // Add last user input (line 23)
        gameOutput.AppendText(FormatLine("Last input: " + lastUserInput) + Environment.NewLine);

        // Add system message (line 24)
        gameOutput.AppendText(FormatLine("> " + systemMessage) + Environment.NewLine);
    }

    // Function for formatting lines with wrapping and borders.
    private static string FormatLine(string line)
    {
        if (line == null) line = "";

        // Wrap text if it's too long
        var wrappedLines = WrapText(line, 73);

        // Combine wrapped lines into a single formatted string
        return string.Join(Environment.NewLine, wrappedLines.Select(wrap => "| " + wrap.PadRight(73) + " |"));
    }

    private static List<string> WrapText(string text, int maxWidth)
    {
        var wrappedLines = new List<string>();
        var words = text.Split(' ');

        string currentLine = "";
        foreach (var word in words)
        {
            if ((currentLine + word).Length > maxWidth)
            {
                // If the line exceeds maxWidth, add the current line to the list and start a new line
                wrappedLines.Add(currentLine.TrimEnd());
                currentLine = word + " ";  // Start a new line with the current word
            }
            else
            {
                // Otherwise, add the word to the current line
                currentLine += word + " ";
            }
        }

        // Add the last line if any text remains
        if (!string.IsNullOrWhiteSpace(currentLine))
        {
            wrappedLines.Add(currentLine.TrimEnd());
        }

        return wrappedLines;
    }

}


