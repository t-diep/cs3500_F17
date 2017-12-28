///
/// Tony Diep (Version 1.1) 
///
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using System.Collections.Generic;
using SpreadsheetUtilities;

/// <summary>
/// Provides a collection of unit tests for testing the Spreadsheet class
/// </summary>
namespace SpreadsheetTester
{
    [TestClass]
    public class SpreadsheetTests
    {
        //Empty spreadsheets

        //Getting direct dependents
        
        /// <summary>
        /// Passing in a null argument to get direct dependents of spreadsheet;
        /// private object should throw an exception of its own, then we 
        /// throw the respective exception we expect
        /// </summary>
        [TestMethod]
        public void GetDirectDependentsWithNullCellName()
        {
            Spreadsheet emptyOne = new Spreadsheet();

            try
            {
                PrivateObject spreadSheetAccessor = new PrivateObject(emptyOne);
                spreadSheetAccessor.Invoke("GetDirectDependents", new String[1] { null });
            }
            catch(System.Reflection.TargetInvocationException exception)
            {
                Assert.IsInstanceOfType(exception.InnerException, typeof(InvalidNameException));
            }      
        }

        /// <summary>
        /// Passing in an invalid cell name to get direct dependents of spreadsheet;
        /// private object should throw an exception of its own, then we 
        /// throw the respective exception we expect
        /// </summary>
        [TestMethod]
        public void GetDirectDependentsWithInvalidCellName()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();

            try
            {
                PrivateObject spreadsheetAccessor = new PrivateObject(emptyOne);
                spreadsheetAccessor.Invoke("GetDirectDependents", new String[1] {"1A"});
            }
            catch(System.Reflection.TargetInvocationException exception)
            {
                Assert.IsInstanceOfType(exception.InnerException, typeof(InvalidNameException));
            }
        }

        /// <summary>
        /// Should give us an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetContentsFromEmptySpreadsheetNullCellName()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            emptyOne.GetCellContents(null);
        }

        /// <summary>
        /// Russian characters as cell name; should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetContentsFromEmptySpreadsheetForeignCellName()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            emptyOne.GetCellContents("о_мой_Бог11");
        }

        /// <summary>
        /// Should give us an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetContentsFromEmptySpreadsheetInvalidVariableCellName()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            emptyOne.GetCellContents("1X1");
        }

        /// <summary>
        /// Should give us an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetContentsFromEmptySpreadsheetBlankStringCellName()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            emptyOne.GetCellContents("");
        }

        /// <summary>
        /// Cell name is only one character long; should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetContentsFromEmptySpreadsheetSingleLetterCellName()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            emptyOne.GetCellContents("s");
        }

        /// <summary>
        /// Cell name is only one character long; should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetContentsFromEmptySpreadsheetSingleNumberCellName()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            emptyOne.GetCellContents("4");
        }

        /// <summary>
        /// Should give us an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetContentsFromEmptySpreadsheetPhraseAsCellName()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            emptyOne.GetCellContents("tony is awesome");
        }

        /// <summary>
        /// Should give us an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetContentsFromEmptySpreadsheetEmoticonAsCellName()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            emptyOne.GetCellContents("-__________________-");
        }

        /// <summary>
        /// Should give us an empty string
        /// </summary>
        [TestMethod]
        public void GetContentsFromEmptySpreadsheetValidCellName()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            Assert.AreEqual("", (String)emptyOne.GetCellContents("X1"));
        }

        //Setting cell content to data of type "double"

        /// <summary>
        /// Cell name is null; should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetDecimalToEmptySpreadsheetWithNullCellName()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            emptyOne.SetCellContents(null, 943.32);
        }

        /// <summary>
        /// Cell name is an emoticon; should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetDecimalToEmptySpreadsheetWithEmoticonCellName()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            emptyOne.SetCellContents("O______O", 1.2);
        }

        /// <summary>
        /// Chinese characters as cell name; should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetDecimalToEmptySpreadsheetWithForeignCharactersCellName()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            emptyOne.SetCellContents("强调", 1.9000);
        }

        /// <summary>
        /// Variable is only character long; should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetDecimalToEmptySpreadsheetWithOneLetterAsVariableName()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            emptyOne.SetCellContents("E", 8.4);
        }

        /// <summary>
        /// Variable is only one letter long; should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetDecimalToEmptySpreadsheetWithOneDigitAsVariableName()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            emptyOne.SetCellContents("5", 3.2);
        }

        /// <summary>
        /// Cell name only starts with letters; should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetDecimalToEmptySpreadsheetCellWithAllLettersAsCellName()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            emptyOne.SetCellContents("XXX", 0.0);
        }

        /// <summary>
        /// Cell name only starts with numbers; should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetDecimalToEmptySpreadsheetCellWithAllNumbersAsCellName()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            emptyOne.SetCellContents("1010101010", 12.33);
        }

        /// <summary>
        /// Cell name is in a number letter alternating pattern
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetDecimalToEmptySpreadsheetCellWithAlternatingNumberLetterCellName()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            emptyOne.SetCellContents("1A2B3C4D", 3.2111111);
        }

        /// <summary>
        /// Should have 3.14159 set on cell A1 correctly
        /// </summary>
        [TestMethod]
        public void SetDecimalToEmptySpreadsheetCellZeroDecimal()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            emptyOne.SetCellContents("A1", 3.14159);
            Assert.AreEqual(3.14159, (double)emptyOne.GetCellContents("A1"), 1e-9);
        }

        /// <summary>
        /// Since each cell contains a double number having the variable name itself, should
        /// return all of the cell names 
        /// </summary>
        [TestMethod]
        public void SetManyCellsWithEachOfThemHavingDoubleContents100()
        {
            AbstractSpreadsheet doubles = new Spreadsheet();
            HashSet<String> allZCells = new HashSet<String>();

            for(int index = 1; index < 100; index++)
            {
                allZCells.Add("Z" + index);
                Assert.IsTrue(allZCells.SetEquals(doubles.SetCellContents("Z" + index, index)));
                allZCells.Clear();
            }
        }

        //Setting cell content to a data of type String

        /// <summary>
        /// Cell name is null; should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetTextStringToEmptySpreadsheetWithNullCellName()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            emptyOne.SetCellContents(null, "hi");
        }

        /// <summary>
        /// Cell name is an emoticon; should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetTextStringToEmptySpreadsheetWithEmoticonCellName()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            emptyOne.SetCellContents("O______O", "hii");
        }

        /// <summary>
        /// Greek characters as cell name; should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetTextStringToEmptySpreadsheetWithForeignCharactersCellName()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            emptyOne.SetCellContents("αβ", "hiii");
        }

        /// <summary>
        /// Variable is only character long; should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetTextStringToEmptySpreadsheetWithOneLetterAsVariableName()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            emptyOne.SetCellContents("N", "hihi");
        }

        /// <summary>
        /// Variable is only one letter long; should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetTextStringToEmptySpreadsheetWithOneDigitAsVariableName()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            emptyOne.SetCellContents("1", "hhii");
        }

        /// <summary>
        /// Cell name only starts with letters; should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetTextStringToEmptySpreadsheetCellWithAllLettersAsCellName()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            emptyOne.SetCellContents("XXX", "hiiii");
        }

        /// <summary>
        /// Cell name only starts with numbers; should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetTextStringToEmptySpreadsheetCellWithAllNumbersAsCellName()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            emptyOne.SetCellContents("1010101010", "hai");
        }

        /// <summary>
        /// Cell name is in a number letter alternating pattern
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetTextStringToEmptySpreadsheetCellWithAlternatingNumberLetterCellName()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            emptyOne.SetCellContents("1A2B3C4D", "hehe");
        }

        /// <summary>
        /// Should have 3.14159 set on cell A1 correctly
        /// </summary>
        [TestMethod]
        public void SetTextStringToEmptySpreadsheetWordCellName()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            emptyOne.SetCellContents("A1", "ohmaigawd");
            Assert.AreEqual("ohmaigawd", (String) emptyOne.GetCellContents("A1"));
        }

        /// <summary>
        /// Just for the funsies; one of my favorite keyboard emoticons
        /// </summary>
        [TestMethod]
        public void SetTextStringToEmptySpreadsheetEmoticonCellName()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            emptyOne.SetCellContents("B2", "^_____^");
            Assert.AreEqual("^_____^", (String) emptyOne.GetCellContents("B2"));
        }

        /// <summary>
        /// Since each cell contains a string that is the cell name itself, should only
        /// return the name of the cell name for each one
        /// </summary>
        [TestMethod]
        public void SetManyCellsWithEachOfThemHavingStringContentsOfThemselves100()
        {
            AbstractSpreadsheet strings = new Spreadsheet();
            ISet<String> allZCells = new HashSet<String>();

            for (int index = 1; index <= 100; index++)
            {
                allZCells.Add("Z" + index);
                Assert.IsTrue(allZCells.SetEquals(strings.SetCellContents("Z" + index, "Z" + index)));
                allZCells.Clear();
            }
        }

        //Setting cell content to be a data of type Formula

        /// <summary>
        /// Cell name is null; should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetFormulaObjectToEmptySpreadsheetWithNullCellName()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            emptyOne.SetCellContents(null, new Formula("1 + 1"));
        }

        /// <summary>
        /// Cell name is an emoticon; should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetFormulaObjectToEmptySpreadsheetWithEmoticonCellName()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            emptyOne.SetCellContents("O______O", new Formula("1 + 3"));
        }

        /// <summary>
        /// Arabic characters as cell name; should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetFormulaObjectToEmptySpreadsheetWithForeignCharactersCellName()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            emptyOne.SetCellContents("وجه الفتاة", new Formula("1 + 1"));
        }

        /// <summary>
        /// Variable is only character long; should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetFormulaObjectToEmptySpreadsheetWithOneLetterAsVariableName()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            emptyOne.SetCellContents("N", new Formula("1 + 3"));
        }

        /// <summary>
        /// Variable is only one letter long; should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetFormulaObjectToEmptySpreadsheetWithOneDigitAsVariableName()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            emptyOne.SetCellContents("1", new Formula("1 + 3"));
        }

        /// <summary>
        /// Cell name only starts with letters; should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetFormulaObjectToEmptySpreadsheetCellWithAllLettersAsCellName()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            emptyOne.SetCellContents("XXX", new Formula("1 + 3"));
        }

        /// <summary>
        /// Cell name only starts with numbers; should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetFormulaObjectToEmptySpreadsheetCellWithAllNumbersAsCellName()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            emptyOne.SetCellContents("1010101010", new Formula("1 + 3"));
        }

        /// <summary>
        /// Cell name is in a number letter alternating pattern
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetFormulaObjectToEmptySpreadsheetCellWithAlternatingNumberLetterCellName()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            emptyOne.SetCellContents("1A2B3C4D", new Formula("1 + 3"));
        }

        /// <summary>
        /// Should throw an exception since we pass in a null argument
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetFormulaObjectToEmptySpreadsheetCellWithNullString()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            String nullString = null;
            emptyOne.SetCellContents("V3", nullString);
        }

        /// <summary>
        /// Should throw an exception since we pass in a null argument
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetFormulaObjectToEmptySpreadsheetCellWithNullFormula()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            Formula nullFormula = null;
            emptyOne.SetCellContents("A1", nullFormula);
        }

        /// <summary>
        /// Should have the correct Formula object; expressions with same operators and operands
        /// and with the same ordering should match regardless of whitespace
        /// </summary>
        [TestMethod]
        public void SetFormulaObjectToEmptySpreadsheetSimpleAddition()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            emptyOne.SetCellContents("A1", new Formula("1 + 1"));
            Assert.IsTrue(new Formula("1 + 1").Equals((Formula) emptyOne.GetCellContents("A1")));
            Assert.IsTrue(new Formula("1+1").Equals((Formula) emptyOne.GetCellContents("A1")));
            Assert.AreNotEqual(new Formula("1 + 2"), (Formula) emptyOne.GetCellContents("A1"));
        }

        /// <summary>
        /// Should have the correct Formula object; expressions with same operators and operands
        /// and with the same ordering should match regardless of whitespace
        /// </summary>
        [TestMethod]
        public void SetFormulaObjectToEmptySpreadsheetSimpleSubtraction()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            emptyOne.SetCellContents("A2", new Formula("2 - 3"));
            Assert.IsTrue(new Formula("2-3").Equals((Formula) emptyOne.GetCellContents("A2")));
            Assert.IsTrue(new Formula("2 - 3").Equals((Formula)emptyOne.GetCellContents("A2")));
            Assert.AreNotEqual(new Formula("2 + 3"), (Formula)emptyOne.GetCellContents("A2"));
        }

        /// <summary>
        /// Should have the correct formula object; expressions with same operators and operands
        /// and with the same ordering should match regardless of whitespace
        /// </summary>
        [TestMethod]
        public void SetFormulaObjectToEmptySpreadsheetComplicatedFormula()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();
            emptyOne.SetCellContents("A3", new Formula("(2 + (9)) * (44.62 / 53 + 222.11)"));
            Assert.IsTrue(new Formula("(2 + (9)) * (44.62 / 53 + 222.11)").Equals((Formula) emptyOne.GetCellContents("A3")));
            Assert.IsTrue(new Formula("(2+(9))*(44.62/53+222.11)").Equals((Formula) emptyOne.GetCellContents("A3")));
            Assert.AreNotEqual(new Formula("(2 + (9)) * (44.62 / 53 + 222.12)"), (Formula) emptyOne.GetCellContents("A3"));
        }
 
        /// <summary>
        /// Getting all of the non-empty cells 
        /// </summary>
        [TestMethod]
        public void GetNamesOfNonEmptyCellsFromEmptySpreadsheet()
        {
            AbstractSpreadsheet emptyOne = new Spreadsheet();

            HashSet<String> expected = new HashSet<String>();
            HashSet<String> actual = new HashSet<String>(emptyOne.GetNamesOfAllNonemptyCells());
            Assert.IsTrue(expected.SetEquals(actual));
        }

        /// <summary>
        /// Since each cell contains a formula having the variable name itself, should
        /// return all of the cell names 
        /// </summary>
        [TestMethod]
        public void SetManyCellsWithEachOfThemHavingFormulaContentsOfThemselves100()
        {
            AbstractSpreadsheet strings = new Spreadsheet();
            ISet<String> allZCells = new HashSet<String>();

            for (int index = 1; index <= 100; index++)
            {
                allZCells.Add("Z" + index);
                Assert.IsTrue(allZCells.SetEquals(strings.SetCellContents("Z" + index, new Formula("Z" + (index + 1)))));
            }
        }

        //NON EMPTY SPREADSHEET TESTS

        //Circular Dependency Checks

        /// <summary>
        /// Should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void CircularDependencyTwoCellNames()
        {
            AbstractSpreadsheet twoCellNames = new Spreadsheet();
            twoCellNames.SetCellContents("B1", new Formula("B2"));
            twoCellNames.SetCellContents("B2", new Formula("B1"));
        }

        /// <summary>
        /// Circular dependency; should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void CircularDependencyThreeVariablesPart1()
        {
            AbstractSpreadsheet threeVariables = new Spreadsheet();
            threeVariables.SetCellContents("X1", new Formula("X2 * X3"));
            threeVariables.SetCellContents("X2", 9.4);
            threeVariables.SetCellContents("X3", 2.000000);
            threeVariables.SetCellContents("X2", new Formula("X3 * X1"));
        }

        /// <summary>
        /// Checking for the contents of X2 making sure they are the correct contents
        /// before throwing exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void CircularDependencyThreeVariablesPart2()
        {
            AbstractSpreadsheet threeVariables = new Spreadsheet();

            try
            {
                threeVariables.SetCellContents("X1", new Formula("X2 * X3"));
                threeVariables.SetCellContents("X2", 9.4);
                threeVariables.SetCellContents("X3", 2.000000);
                threeVariables.SetCellContents("X2", new Formula("X3 * X1"));
            }
            catch(CircularException)
            {
                Assert.AreEqual(9.4, threeVariables.GetCellContents("X2"));
                throw new CircularException();
            }
        }

        /// <summary>
        /// Circular dependency; should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void CircularDependencyMultipleVariables()
        {
            AbstractSpreadsheet multipleCellNames = new Spreadsheet();
            multipleCellNames.SetCellContents("C1", new Formula("C2 + C3"));
            multipleCellNames.SetCellContents("C3", new Formula("C4 - C5"));
            multipleCellNames.SetCellContents("C5", new Formula("C6 * C7"));
            multipleCellNames.SetCellContents("C7", new Formula("C1 / C1"));
        }

        /// <summary>
        /// Should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void GiantCircularDependency()
        {
            AbstractSpreadsheet giantDependency = new Spreadsheet();

            for(int index = 1; index < 200; index++)
            {
                giantDependency.SetCellContents("X" + index, new Formula("X" + (index + 1)));

                if(index == 199)
                {
                    giantDependency.SetCellContents("X" + index, new Formula("X1 * X2"));
                }
            }
        }

        /// <summary>
        /// Should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void SelfDependency()
        {
            AbstractSpreadsheet oneVariable = new Spreadsheet();
            oneVariable.SetCellContents("A1", new Formula("A1"));
        }

        //Getting non-empty cells

        /// <summary>
        /// Even though we create a new cell, we inserted a blank string which is 
        /// technically an empty content; there should be no name of cells that are
        /// non-empty
        /// </summary>
        [TestMethod]
        public void GetNamesOfNonEmptyCellsSetNewCellNameToBlank()
        {
            AbstractSpreadsheet oneCellName = new Spreadsheet();
            oneCellName.SetCellContents("A1", "");

            HashSet<String> expected = new HashSet<String>();
            HashSet<String> actual = new HashSet<string>(oneCellName.GetNamesOfAllNonemptyCells());

            Assert.IsTrue(expected.SetEquals(actual));
        }
      
        /// <summary>
        /// Should get the XXX cell containing the double value
        /// </summary>
        [TestMethod]
        public void GetNamesOfNonEmptyCellsForDoubles()
        {
            AbstractSpreadsheet doubles = new Spreadsheet();
            doubles.SetCellContents("X1", 2.71);

            HashSet<String> expected = new HashSet<String> {"X1"};
            HashSet<String> actual = new HashSet<String>(doubles.GetNamesOfAllNonemptyCells());

            Assert.IsTrue(expected.SetEquals(actual));
        }

        /// <summary>
        /// Should get the N6 cell since it has a non-empty string
        /// </summary>
        [TestMethod]
        public void GetNamesOfNonEmptyCellsForStringTexts()
        {
            AbstractSpreadsheet strings = new Spreadsheet();
            strings.SetCellContents("N6", "gooooooooooooooooooooooooogle");

            HashSet<String> expected = new HashSet<String> {"N6"};
            HashSet<String> actual = new HashSet<String>(strings.GetNamesOfAllNonemptyCells());
        }

        /// <summary>
        /// Should get the Formula object from the cell name G123
        /// </summary>
        [TestMethod]
        public void GetNameOfNonEmptyCellsForFormulas()
        {
            AbstractSpreadsheet formulas = new Spreadsheet();
            formulas.SetCellContents("G123", new Formula("100"));
        }

        //Verifying the return values of setting cell contents
        
        /// <summary>
        /// Should return a hashset containing the non-empty cell "A1"
        /// </summary>
        [TestMethod]
        public void SetCellContentsOneCell()
        {
            AbstractSpreadsheet oneNonEmptyCell = new Spreadsheet();
            HashSet<String> expected = new HashSet<String> {"A1"};
            Assert.IsTrue(oneNonEmptyCell.SetCellContents("A1", 2.222).SetEquals(expected));
        }

        /// <summary>
        /// Should return a hashset containing non empty cell "A2" 
        /// </summary>
        [TestMethod]
        public void SetCellContentsTwoCells()
        {
            AbstractSpreadsheet twoNonEmptyCells = new Spreadsheet();
            HashSet<String> actual = new HashSet<String> {"A2"};
            twoNonEmptyCells.SetCellContents("A1", 3.21111);
            Assert.IsTrue(twoNonEmptyCells.SetCellContents("A2", "whoa dude").SetEquals(actual));
        }

        /// <summary>
        /// Should return a hashset containing non empty cell "C1" 
        /// </summary>
        [TestMethod]
        public void SetCellContentsThreeCells()
        {
            AbstractSpreadsheet threeNonEmptyCells = new Spreadsheet();
            HashSet<String> actual = new HashSet<String> {"C1"};
            threeNonEmptyCells.SetCellContents("A1", 5.67);
            threeNonEmptyCells.SetCellContents("B1", "5.67");
            Assert.IsTrue(threeNonEmptyCells.SetCellContents("C1", new Formula("5.67")).SetEquals(actual));
        }

        /// <summary>
        /// Should return a hashset containing Z5, Z4, Z3, and Z1
        /// </summary>
        [TestMethod]
        public void SetCellContentsMultipleCells()
        {
            AbstractSpreadsheet multipleCells = new Spreadsheet();
            multipleCells.SetCellContents("Z1", new Formula("Z2 + Z3"));
            multipleCells.SetCellContents("Z2", new Formula("yo"));
            multipleCells.SetCellContents("Z3", new Formula("Z2 + Z4"));
            multipleCells.SetCellContents("Z4", new Formula("Z2 + Z5"));
            HashSet<String> actual = new HashSet<String> {"Z5", "Z4", "Z3", "Z1"};
            Assert.IsTrue(multipleCells.SetCellContents("Z5", "yo").SetEquals(actual));
        }

        //Changing Cell Contents From Non-Empty Cell

        /// <summary>
        /// Should return 9.0 instead of 3.0
        /// </summary>
        [TestMethod]
        public void SetNonEmptyCellOldContentToNewContentDouble()
        {
            AbstractSpreadsheet oneCell = new Spreadsheet();
            oneCell.SetCellContents("Y1", 3.0);
            oneCell.SetCellContents("Y1", 9.0);
            Assert.AreEqual(9.0, (double) oneCell.GetCellContents("Y1"));
        }

        /// <summary>
        /// Should return "TONY" instead of "tony"
        /// </summary>
        [TestMethod]
        public void SetNonEmptyCellOldContentToNewContentString()
        {
            AbstractSpreadsheet oneCell = new Spreadsheet();
            oneCell.SetCellContents("Y1", "tony");
            oneCell.SetCellContents("Y1", "TONY");
            Assert.AreEqual("TONY", (String) oneCell.GetCellContents("Y1"));
        }

        /// <summary>
        /// Should return the Formula object with the ""1 + 1 + 1 + 1 + 1 + 1" in it
        /// </summary>
        [TestMethod]
        public void SetNonEmptyCellOldContentToNewContentFormula()
        {
            AbstractSpreadsheet oneCell = new Spreadsheet();
            oneCell.SetCellContents("C2", new Formula("1 + 1"));
            oneCell.SetCellContents("C2", new Formula("1 + 1 + 1 + 1 + 1 + 1"));
            Assert.AreEqual(new Formula("1 + 1 + 1 + 1 + 1 + 1"), (Formula) oneCell.GetCellContents("C2"));
        }
    }
}
