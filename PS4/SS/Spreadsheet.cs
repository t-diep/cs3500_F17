///
/// Tony Diep (Version 1.1)
///
using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

/// <summary>
/// Contains blueprints for constructing the internals of a spreadsheet
/// </summary>
namespace SS
{
    /// <summary>
    /// An extension class of the abstract spreadsheet
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        //Holds string keys that map to cells in this spreadsheet
        private Dictionary<String, Cell> allDemCells;
        //Our dependency graph used for the spreadsheet
        private DependencyGraph dependencyGraph;

        /// <summary>
        /// Constructs a new spreadsheet
        /// </summary>
        public Spreadsheet()
        {
            //Construct the dictionary of cells and the dependency graph
            allDemCells = new Dictionary<string, Cell>();
            dependencyGraph = new DependencyGraph();
        }

        /// <summary>
        /// Gets the contents of a desired cell 
        /// </summary>
        /// <param name="name">corresponds to a particular cell</param>
        /// <returns>the contents of the cell</returns>
        public override object GetCellContents(string name)
        {
            //Verify for any invalid cell names
            if(ReferenceEquals(name, null) || !(IsValidName(name)))
            {
                throw new InvalidNameException();
            }

            //Try and retrieve the content inside the cell
            if(allDemCells.TryGetValue(name, out Cell datCell))
            {
                return datCell.Contents;
            }

            //No cell content found, so return an empty string
            return "";
        }

        /// <summary>
        /// Gets all of the non-empty cells of this spreadsheet
        /// </summary>
        /// <returns>all of the cells having content in them</returns>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            return allDemCells.Keys;
        }

        /// <summary>
        /// Sets a cell's current contents to different contents (since there could be 
        /// some dependents and dependees on this cell)
        /// </summary>
        /// <param name="name">name of this cell</param>
        /// <param name="number">the floating point number to replace the old contents</param>
        /// <returns>the new dependees after setting the new content to the cell</returns>
        public override ISet<string> SetCellContents(string name, double number)
        {
            //Verify for any invalid cell names
            if (ReferenceEquals(name, null) || !(IsValidName(name)))
            {
                throw new InvalidNameException();
            }

            Cell decimalCell = new Cell(number);

            //Cell name already exists in spreadsheet, so just add the content 
            if(allDemCells.ContainsKey(name))
            {
                allDemCells[name] = decimalCell;
            }
            //Add the cell name and its corresponding content to the spreadsheet
            else
            {
                allDemCells.Add(name, decimalCell);
            }

            dependencyGraph.ReplaceDependees(name, new HashSet<String>());

            HashSet<String> dependees = new HashSet<String>(GetCellsToRecalculate(name));

            return dependees;
        }

        /// <summary>
        /// Sets a cell's current contents to different contents (since there could be 
        /// some dependents and dependees on this cell)
        /// </summary>
        /// <param name="name">name of this cell</param>
        /// <param name="text">the text to be entered in the cell</param>
        /// <returns>the new dependees after setting the new content to the cell</returns>
        public override ISet<string> SetCellContents(string name, string text)
        {
            //Verify for any invalid cell names
            if (ReferenceEquals(name, null) || !(IsValidName(name)))
            {
                throw new InvalidNameException();
            }
            //Verify for any null text contents
            if(ReferenceEquals(text, null))
            {
                throw new ArgumentNullException();
            }

            Cell stringCell = new Cell(text);

            if(allDemCells.ContainsKey(name))
            {
                allDemCells[name] = stringCell;
            }
            else
            {
                allDemCells.Add(name, stringCell);
            }


            if(allDemCells[name].Contents.Equals(""))
            {
                allDemCells.Remove(name);
            }


            dependencyGraph.ReplaceDependees(name, new HashSet<String>());

            HashSet<String> dependees = new HashSet<String>(GetCellsToRecalculate(name));

            return dependees;
        }

        /// <summary>
        /// Sets the current content formula object in a cell to a different formula object
        /// </summary>
        /// <param name="name">name of this cell</param>
        /// <param name="formula">the new Formula used to replace the old contents</param>
        /// <returns>the new dependees after setting the new content to the cell</returns>
        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            //Verify for any invalid cell names
            if (ReferenceEquals(name, null) || !(IsValidName(name)))
            {
                throw new InvalidNameException();
            }
            //Verify for any invaild formulas
            if(ReferenceEquals(formula, null))
            {
                throw new ArgumentNullException();
            }

            IEnumerable<String> oldDependees = dependencyGraph.GetDependees(name);
            dependencyGraph.ReplaceDependees(name, formula.GetVariables());

            //Get all of the dependees and detect any circular dependencies during the process
            try
            {
                HashSet<String> allDependees = new HashSet<String>(GetCellsToRecalculate(name));
                Cell formulaCell = new Cell(formula);

                if(allDemCells.ContainsKey(name))
                {
                    allDemCells[name] = formulaCell;
                }
                else
                {
                    allDemCells.Add(name, formulaCell);
                }

                return allDependees;
            }
            catch(CircularException)
            {
                dependencyGraph.ReplaceDependees(name, oldDependees);
                throw new CircularException();
            }
        }

        /// <summary>
        /// Gets the dependents directly related to the name of the cell
        /// </summary>
        /// <param name="name">name corresponding to a particular cell</param>
        /// <returns>all of the dependents of that cell</returns>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            if(ReferenceEquals(name, null) || !(IsValidName(name)))
            {
                throw new InvalidNameException();
            }

            return dependencyGraph.GetDependents(name);
        }

        //~HELPER METHODS~//

        /// <summary>
        /// Helps report whether a given name is a valid name for a cell
        /// </summary>
        /// <param name="name">name of cell</param>
        /// <returns>true if the name is a valid cell name and false otherwise</returns>
        private static bool IsValidName(String name)
        {
            return Regex.IsMatch(name, @"^[a-zA-Z_](?: [a-zA-Z_]|\d)*$", RegexOptions.Singleline) && name.Length > 1;
        }

        /// <summary>
        /// Represents a single cell in a spreadsheet 
        /// </summary>
        private class Cell
        {
            //Tells us the contents of this cell
            public Object Contents
            {
                get;
                set;
            }

            /// <summary>
            /// Constructor that allows a string input
            /// </summary>
            /// <param name="name">a string text</param>
            public Cell(String name)
            {
                Contents = name;
            }

            /// <summary>
            /// Constructor that allows a floating point number
            /// </summary>
            /// <param name="name">a floating point number</param>
            public Cell(double name)
            {
                Contents = name;
            }

            /// <summary>
            /// Constructor that takes in a Formula object
            /// </summary>
            /// <param name="name">a Formula object</param>
            public Cell(Formula name)
            {
                Contents = name;
            }
        }
    }
}
