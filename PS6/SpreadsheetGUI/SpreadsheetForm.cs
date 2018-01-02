
///
///@author Tony Diep and Sona Torosyan
///
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;

/// <summary>
/// Represents the GUI of the spreadsheet 
/// </summary>
namespace SpreadsheetGUI
{
    /// <summary>
    /// The Windows application of our spreadsheet
    /// </summary>
    public partial class SpreadsheetForm : Form
    {
        //Constants for max column and row numbers
        private static int MAX_COL = 26;
        private static int MAX_ROW = 99;

        //for watermark textboxes
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);
        
        //Stacks to keep track of our redoing and undoing of changes
        private Stack<KeyValuePair<String, String>> undo;
        private Stack<KeyValuePair<String, String>> redo;

        //Represents the spreadsheet model
        private SS.Spreadsheet spreadsheet;
        //Provides the default name of our spreadsheet file
        private string fileName = "Untitled Spreadsheet";
       

        /// <summary>
        /// Instantiates our Spreadsheet form 
        /// </summary>
        public SpreadsheetForm()
        {
            InitializeComponent();

            //Set up our spreadsheet model and action events
            spreadsheet = new SS.Spreadsheet(IsValid, input => input.ToUpper(), "ps6"); 
            spreadsheetPanel1.SelectionChanged += DisplaySelection;
            this.AcceptButton = evaluateButton;

            //Set up our undo/redo stacks
            undo = new Stack<KeyValuePair<string, string>>();
            redo = new Stack<KeyValuePair<string, string>>();

            //for watermark contents textbox
            SendMessage(contentsTextBox.Handle, 0x1501, 1, "Please type here.");
        }

        /// <summary>
        /// Helps determine if a cell name is valid
        /// </summary>
        /// <param name="cellName">cell name</param>
        /// <returns>true if cell name is valid and false otherwise</returns>
        private bool IsValid(string cellName)
        {
            if (cellName == null)
                return false;
            if (cellName.Length < 2 || cellName.Length > 3)
                return false;

            if (!Char.IsLetter(cellName[0]))
                return false;

            if (!Char.IsDigit(cellName[1]))
                return false;
            else
            {
                if (cellName[1] == '0')
                    return false;
            }
            if (cellName.Length == 3 && !Char.IsDigit(cellName[2]))
                return false;

            return true;

        }
        /// <summary>
        /// Before panel appears for the first time. Used to set default selected cell.
        /// </summary>
        /// <param name="sender">the object represent the event that happened</param>
        /// <param name="e">the event</param>
        private void SpreadsheetPanel1_Load(object sender, EventArgs e)
        {
            DisplaySelection(spreadsheetPanel1);   
        }

        /// <summary>
        /// Helps update the spreadsheet panel 
        /// </summary>
        /// <param name="spreadsheetPanel">the spreadsheet panel</param>
        private void DisplaySelection(SS.SpreadsheetPanel spreadsheetPanel)
        {
            spreadsheetPanel.GetSelection(out int col, out int row);
            spreadsheetPanel.GetValue(col, row, out string value);

            String cellName = GetCellName(row, col);
            cellNameTextBox.Text = cellName;

            valueTextBox.Text = value;

            String contents = "";

            if (spreadsheet.GetCellContents(cellName) is Formula)
            {
                contents = "=";
            }
               
            contentsTextBox.Text = contents + spreadsheet.GetCellContents(cellName).ToString();
        }

       
        /// <summary>
        /// Helps get the cell name based on given row and column
        /// </summary>
        /// <param name="row">row</param>
        /// <param name="col">col</param>
        /// <returns>cell name</returns>
        private string GetCellName(int row, int col)
        {
            char colLetter = Convert.ToChar(65 + col);
            int current_row = row;
            return (colLetter + "" + ++current_row);
        }

       /// <summary>
       /// Helps save the current spreadsheet file
       /// </summary>
       /// <param name="sender">event that happened</param>
       /// <param name="e">event</param>
        private void SaveSpreadsheetMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Spreadsheet Files (*.sprd)|*.sprd|All Files (*.*)|*.*",
                DefaultExt = ".sprd",
                Title = "Saving spreadsheet",
                AddExtension = true,
                FileName = fileName,
                OverwritePrompt = false
            };

            DialogResult result = saveFileDialog.ShowDialog();
            
            
            switch (result)
            {                       
                case DialogResult.OK:
                   
                    if (saveFileDialog.FileName != "")
                    {
                        spreadsheet.Save(saveFileDialog.FileName);
                    }
                    
                    break;

                case DialogResult.Cancel:
                    saveFileDialog.Dispose();
                    break;
            }
        }

        /// <summary>
        /// Helps handle the event when the newToolStripMenuItem is clicked
        /// </summary>
        /// <param name="sender">object representing button was clicked</param>
        /// <param name="e">event triggered</param>
        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Tell the application context to run the form on the same
            // thread as the other forms.
            SpreadsheetApplication.GetAppContext().RunForm(new SpreadsheetForm());

        }

        /// <summary>
        /// Helps handle the event when the user wants to close the spreadsheet program
        /// </summary>
        /// <param name="sender">object representing the event that happened</param>
        /// <param name="e">event </param>
        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {    
            //Check if the spreadsheet has been modified
            if (spreadsheet.Changed)
            {
                DialogResult res = MessageBox.Show("Would you like to save your changes?", "Spreadsheet", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                if (res == DialogResult.Yes)
                {
                    SaveSpreadsheetMenuItem_Click(sender, e);
                }
                else if(res == DialogResult.No)
                {
                    Close();
                }
            }
            else
            {
                Close();
            }
        }

        /// <summary>
        /// Helps handle the event when the user wants to open an existing spreadsheet file
        /// </summary>
        /// <param name="sender">object that represents the event</param>
        /// <param name="e">the event</param>
        private void OpenMenuItem_Click(object sender, EventArgs e)
        {
            //Set up configurations of the open dialog and open it
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Spreadsheet Files (*.sprd)|*.sprd|All Files (*.*)|*.*",
                Title = "Opening spreadsheet",
                RestoreDirectory = true
            };

            switch (openFileDialog.ShowDialog())
            {
                case DialogResult.OK:
                    if(spreadsheet.Changed)
                    {
                        DialogResult res = MessageBox.Show("Would you like to save your changes?", "Spreadsheet", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                        if (res == DialogResult.Yes)
                        {
                            SaveSpreadsheetMenuItem_Click(sender, e);
                        }

                        if(res == DialogResult.Cancel)
                        {
                            return;
                        }
                       
                    }

                    fileName = openFileDialog.SafeFileName;
                    string version = spreadsheet.GetSavedVersion(openFileDialog.FileName);
                    //reading contents of spreadsheet from the file
                    spreadsheet = new SS.Spreadsheet(openFileDialog.FileName, IsValid, input => input.ToUpper(), version);

                    //reset the panel based on the new spreadsheet
                    spreadsheetPanel1.Clear();
                    SpreadsheetPanel1_Load(sender, e);
                    
                    UpdatePanel(spreadsheet.GetNamesOfAllNonemptyCells());
                    
                    break;

                case DialogResult.Cancel:
                    break;
            }

        }

        /// <summary>
        /// Sets the values of cells on panel.
        /// </summary>
        /// <param name="cells">the collection of cells whose view on panel will be changed</param>
        private void UpdatePanel(IEnumerable<string> cells)
        {
            foreach (string cell in cells)
            {
                int colLetter = cell[0] - 65;
                int row = int.Parse(cell.Substring(1));
                spreadsheetPanel1.SetValue(colLetter, --row, spreadsheet.GetCellValue(cell).ToString());
            }
        }

        /// <summary>
        /// Helps display the directions of how to modify contents of a cell 
        /// </summary>
        /// <param name="sender">object that represents event triggered</param>
        /// <param name="e">event</param>
        private void AddModifyContentsOfCellToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("On the top, there is a rectangular text box where you " +
                "can enter contents of a cell.  Simply click on it, type whatever " +
                "contents you would like, and hit the ENTER key to confirm.  Whoo, you're done!", "Modifying Contents");
        }

        /// <summary>
        /// Helps display the directions on how to open an existing spreadsheet 
        /// </summary>
        /// <param name="sender">object that represents event triggered</param>
        /// <param name="e">event</param>
        private void OpeningAnExistingSpreadsheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("On the top left corner, there is a File tab.  Hover over it " +
                "and you should see a menu that says Open.  It will then prompt you " +
                "to the file explorer to choose an existing spreadsheet to open.", "Opening Existing Spreadsheet");
        }

        /// <summary>
        /// Helps display the instructions on how to save an existing spreadsheet 
        /// </summary>
        /// <param name="sender">object that represents the event triggered</param>
        /// <param name="e">event</param>
        private void SavingYourSpreadsheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Under the File tab, click on the Save tab to prompt the "
                + " save file explorer to choose your destination and file name for the "
                + " spreadsheet.", "Saving a Spreadsheet");
        }

        /// <summary>
        /// Helps handle the event where the user wants to evaluate current cell content
        /// </summary>
        /// <param name="sender">object that represents the event triggered</param>
        /// <param name="e">event</param>
        private void EvaluateButton_Click(object sender, EventArgs e)
        {      
            spreadsheetPanel1.Focus();
            String contents = contentsTextBox.Text;

            spreadsheetPanel1.GetSelection(out int col, out int row);
            string cellName = GetCellName(row, col);

            undo.Push(new KeyValuePair<string, string>(cellName, spreadsheet.GetCellContents(cellName).ToString()));
            try
            {
                ISet<string> updatingCells = spreadsheet.SetContentsOfCell(cellName, contents);

                //set the contents textbox view based on the cell contents
                if (Double.TryParse(contents, out double decVal))
                {
                    valueTextBox.Text = contents;

                }
                else if (contents.Length > 0 && contents[0] == '=')
                {
                    
                    Object value = spreadsheet.GetCellValue(cellName);
                    
                    if (value is FormulaError error)
                    {
                        MessageBox.Show(error.Reason, "Error");

                    }
                    else
                    {
                        valueTextBox.Text = value.ToString();
                    }

                }
                else
                {
                    valueTextBox.Text = contents;
                }

            //update any changes to other cells on panel
            UpdatePanel(updatingCells);    
            //enable undo option    
            undoToolStripMenuItem.Enabled = true;

            }
            catch (FormulaFormatException exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }

            catch (SS.CircularException)
            {
                MessageBox.Show("Cannot have circular dependency.", "Error");
            }          
        }

        /// <summary>
        /// Helps handle the event where the user wants to trigger the Excel Background
        /// </summary>
        /// <param name="sender">object representing the event triggered</param>
        /// <param name="e">event</param>
        private void ExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {           
            if (excelToolStripMenuItem.Checked)
            {
                //set the colors to default then change to Excel edition based on default

                DefaultToolStripMenuItem_Click(sender, e);
                defaultToolStripMenuItem.CheckState = CheckState.Unchecked;
                this.BackColor = Color.Green;
                menuStrip2.BackColor = Color.LightGreen;
                excelToolStripMenuItem.CheckState = CheckState.Checked;
            }
            else
            {
                //switch back to default
                DefaultToolStripMenuItem_Click(sender, e);
                defaultToolStripMenuItem.CheckState = CheckState.Checked;
            }
        }

        /// <summary>
        /// Helps handle the event when the user wants to trigger the Sona Edition background
        /// </summary>
        /// <param name="sender">object representing the event triggered</param>
        /// <param name="e">event</param>
        private void SonaEditionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sonaEditionToolStripMenuItem.Checked)
            {
                //set the colors to default then change to Sona edition based on default

                DefaultToolStripMenuItem_Click(sender, e);
                defaultToolStripMenuItem.CheckState = CheckState.Unchecked;
                this.BackColor = Color.CornflowerBlue;
                cellNameLabel.BackColor = Color.DarkBlue;
                cellNameLabel.ForeColor = Color.White;
                valueLabel.BackColor = Color.DarkBlue;
                valueLabel.ForeColor = Color.White;
                contentsTextBox.ForeColor = Color.MediumBlue;

                valueTextBox.BackColor = Color.Azure;
                cellNameTextBox.BackColor = Color.Azure;
                evaluateButton.BackColor = Color.Azure;
                menuStrip2.BackColor = Color.AliceBlue;
                sonaEditionToolStripMenuItem.CheckState = CheckState.Checked;
            }
            else
            {
                //set back to default colors
                DefaultToolStripMenuItem_Click(sender, e);
                defaultToolStripMenuItem.CheckState = CheckState.Checked;
            }
            
        }

        /// <summary>
        /// Helps handle the event when the user wants to trigger the Tony Edition background
        /// </summary>
        /// <param name="sender">object representing the event triggered</param>
        /// <param name="e">event</param>
        private void TonyEditionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tonyEditionToolStripMenuItem.Checked)
            {
                //set the colors to default then change to Tony edition based on default
                DefaultToolStripMenuItem_Click(sender, e);
                defaultToolStripMenuItem.CheckState = CheckState.Unchecked;
                this.BackColor = Color.Yellow;
                menuStrip2.BackColor = Color.Gold;
                tonyEditionToolStripMenuItem.CheckState = CheckState.Checked;
            }

            else //go back to default
            {
                DefaultToolStripMenuItem_Click(sender, e);
                defaultToolStripMenuItem.CheckState = CheckState.Checked;
            }
        }

        /// <summary>
        /// Helps handle the event when the user wants to use the arrow keys
        /// to navigate cells and the spacebar to switch focus to cell content
        /// text box
        /// </summary>
        /// <param name="sender">object representing the event triggered</param>
        /// <param name="e">event</param>
        private void SpreadsheetPanel1_KeyDown(object sender, KeyEventArgs e)
        {          
            spreadsheetPanel1.GetSelection(out int col, out int row);
            
            switch (e.KeyCode) {
                case Keys.Up:
                    if (row != 0)
                    {
                        spreadsheetPanel1.SetSelection(col, --row);                      
                    }
                    break;
                case Keys.Down:
                    if (row != MAX_ROW)
                    {
                        spreadsheetPanel1.SetSelection(col, ++row);
                    }
                    break;
                case Keys.Left:
                    if (col != 0)
                    {
                        spreadsheetPanel1.SetSelection(--col, row);
                    }
                    break;
                case Keys.Right:
                    if (col != MAX_COL)
                    {
                        spreadsheetPanel1.SetSelection(++col, row);
                    }
                    break;
                    //spacebar triggers the focus to the textbox of contents
                case Keys.Space: 
                    contentsTextBox.Focus();
                    break;

               }
            DisplaySelection(spreadsheetPanel1);
        }

        /// <summary>
        /// Helps handle the arrow key recognition
        /// </summary>
        /// <param name="sender">object representing the event triggered</param>
        /// <param name="e">event</param>
        private void SpreadsheetPanel1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {    
            //accept the arrow key functionalities
           switch (e.KeyCode)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                    e.IsInputKey = true;
                    break;
            }
        }

        /// <summary>
        /// Helps handle the event when the user wants to undo a change
        /// </summary>
        /// <param name="sender">object representing the event triggered</param>
        /// <param name="e">event</param>
        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //there needs to be at least one modification
            if(spreadsheet.Changed)
            {
                redoToolStripMenuItem.Enabled = true;
                KeyValuePair<String, String> current = undo.Pop();
                redo.Push(new KeyValuePair<string, string>(current.Key, spreadsheet.GetCellContents(current.Key).ToString()));

                ISet<string> updatingCells = spreadsheet.SetContentsOfCell(current.Key, current.Value);
                UpdatePanel(updatingCells);
                DisplaySelection(spreadsheetPanel1);
            }          
            //if after doing the undo there are no more changes disable the undo 
            if(undo.Count == 0)
            {
                undoToolStripMenuItem.Enabled = false;
            }
        }

        /// <summary>
        /// Helps handle the event when the user wants to redo a change
        /// </summary>
        /// <param name="sender">object representing the event triggered</param>
        /// <param name="e">event</param>
        private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //There neeeds to be at least one change present 
            if(redo.Count != 0)
            {              
                KeyValuePair<String, String> current = redo.Pop();
                undo.Push(new KeyValuePair<string, string>(current.Key, spreadsheet.GetCellContents(current.Key).ToString()));
                undoToolStripMenuItem.Enabled = true;

                ISet<string> updatingCells = spreadsheet.SetContentsOfCell(current.Key, current.Value);
                UpdatePanel(updatingCells);
                DisplaySelection(spreadsheetPanel1);
            }

            //Disable redo feature if no changes exist already
            if(redo.Count == 0)
            {
                redoToolStripMenuItem.Enabled = false;
            }
        }

        /// <summary>
        /// Helps handle the event when the user wants to trigger the default background
        /// </summary>
        /// <param name="sender">object representing the event triggered</param>
        /// <param name="e">event</param>
        private void DefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            defaultToolStripMenuItem.CheckState = CheckState.Checked;
            tonyEditionToolStripMenuItem.CheckState = CheckState.Unchecked;
            sonaEditionToolStripMenuItem.CheckState = CheckState.Unchecked;
            excelToolStripMenuItem.CheckState = CheckState.Unchecked;

            //set back to default colors on all components
            menuStrip2.BackColor = default(Color);
            valueLabel.BackColor = default(Color);
            valueLabel.ForeColor = default(Color);

            cellNameLabel.BackColor = default(Color);
            cellNameLabel.ForeColor = default(Color);

            contentsTextBox.BackColor = default(Color);
            contentsTextBox.ForeColor = default(Color);

            cellNameTextBox.BackColor = default(Color);
            cellNameTextBox.ForeColor = default(Color);

            valueTextBox.BackColor = default(Color);
            valueTextBox.ForeColor = default(Color);
            evaluateButton.ForeColor = default(Color);
            evaluateButton.BackColor = default(Color);

            BackColor = DefaultBackColor;
            ForeColor = DefaultForeColor;
        }

        /// <summary>
        /// Helps display the instructions of how to redo/undo changes
        /// </summary>
        /// <param name="sender">object representing the event triggered</param>
        /// <param name="e">event</param>
        private void UndoRedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Under the Edit tab, there contains Undo and Redo menu items "
                + "If you want to revert to the previous change, hit Undo.  If you want" +
                " to go back to the subsequent change, hit Redo. Note that you must have "
                 + " at least one change present in order to use them.", "Undo/Redo Changes");
        }

        /// <summary>
        /// Helps display the instructions on how to use keyboard shortcuts
        /// </summary>
        /// <param name="sender">object representing the event triggered</param>
        /// <param name="e">event</param>
        private void KeyboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You can use the arrow keys to navigate from one cell to "
                + " another cell. Give it a try!", "Arrow Keys to Navigate Cells");
        }

        /// <summary>
        /// Helps display the instructions on how to change spreadsheet's background
        /// </summary>
        /// <param name="sender">object representing the event triggered</param>
        /// <param name="e">event</param>
        private void ChangeBackgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("If you want to give your spreadsheet some color, you can do "
                + "so by selecting several color options.  We have Excel, Sona Edition, "
                + " and Tony Edition.  We won't spoil too much, so give those a shot.", "Spreadsheet Background");
        }

        /// <summary>
        /// Helps display the description of the watermark feature on the text box
        /// </summary>
        /// <param name="sender">object representing the event triggered</param>
        /// <param name="e">event</param>
        private void WatermarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Changing contents is easier to locate with the watermark "
                + "feature!  On the top, you will see a text box that has a default text "
                + "telling you to enter in cell contents there.  When you click on it, " +
                "the default text disappears! Pretty cool.", "Watermark Feature");
        }

        /// <summary>
        /// Helps display the instructions on how to close the spreadsheet program
        /// </summary>
        /// <param name="sender">object representing the event triggered</param>
        /// <param name="e">event</param>
        private void ClosingThisProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Closing the program is easy to do.  You can either close " + 
                "via the File Tab and hit the Close menu item, or you can hit the " + 
                "X Button located on the top right hand corner.  NOTE: If you have "
                + "multiple spreadsheet windows open, Close will only close the current "
                + "spreadsheet you are working on. See ya!", "Closing This Program");
        }

        /// <summary>
        /// Helps display the instructions on how to change the focus to the text box
        /// </summary>
        /// <param name="sender">object representing the event triggered</param>
        /// <param name="e">event</param>
        private void ChangingFocusToTextBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Do you not like clicking on the text box to change cell contents? "
                + "Have no fear, the space keyboard shortcut is here!  Simply hit the "
                + "spacebar key to shift from selecting a cell to the text box!"
                , "Spacebar Keyboard Shortcut To Text Box");
        }
    }
}
