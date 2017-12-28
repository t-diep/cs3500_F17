using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormulaEvaluator;

/// <summary>
/// Contains a single class that provides a collection of unit tests 
/// for a plethora of different formula expressions
/// 
/// @author Tony Diep
/// </summary>
namespace FormulaEvaluatorTester
{
    /// <summary>
    /// The class containing the plethora of different formula expressions to test
    /// </summary>
    class EvaluatorTester
    {
        static void Main(string[] args)
        {
            //The look up variable delegate that works for all different variables and their respective values
            Evaluator.Lookup lookupDatVariableValue = LookUpVariable;

            ///~VALID EXPRESSIONS TESTS~///
            //TEST 1: Simple expression with no whitespaces and two single digit numbers: "1 + 1" 
            EvaluatorTester.TestValidExpressions(1, 2, Evaluator.Evaluate("1+1", lookupDatVariableValue));
            //TEST 2: Simple expression with no whitespaces and two single digit numbers: "10 + 100"
            EvaluatorTester.TestValidExpressions(2, 110, Evaluator.Evaluate("10 + 100", lookupDatVariableValue));
            //TEST 3: Simple expression with minus sign and two four digit numbers: "3000 - 3000"
            EvaluatorTester.TestValidExpressions(3, 0, Evaluator.Evaluate("3000 - 3000", lookupDatVariableValue));
            //TEST 4: Simple expression only containing multiply operator:  "100 * 4"
            EvaluatorTester.TestValidExpressions(4, 400, Evaluator.Evaluate("100 * 4", lookupDatVariableValue));
            //TEST 5: Simple expression with 2 of the same 6 digit number and a divide operator: "100000 / 100000"
            EvaluatorTester.TestValidExpressions(5, 1, Evaluator.Evaluate("100000 / 100000", lookupDatVariableValue));
            //TEST 6: Simple expression with all operators except parentheses: "6 / 3 + 10 - 20"
            EvaluatorTester.TestValidExpressions(6, -8, Evaluator.Evaluate("6 / 3 + 10 - 20", lookupDatVariableValue));
            //TEST 7: Simple expression with all operators including parentheses: "(3 * 2 / 7) + 8 - 10"
            EvaluatorTester.TestValidExpressions(7, -2, Evaluator.Evaluate("(3 * 2 / 7) + 8 - 10", lookupDatVariableValue));
            //TEST 8: Simple expression only containing an operand with LOTS of pairs of parentheses!: "((((((((((10101))))))))))"
            EvaluatorTester.TestValidExpressions(8, 10101, Evaluator.Evaluate("((((((((((10101))))))))))", lookupDatVariableValue));
            //TEST 9: Intermidate expression: "(2 * 4) + (7 / 7) - (1 * 1)"
            EvaluatorTester.TestValidExpressions(9, 8, Evaluator.Evaluate("(2 * 4) + (7 / 7) - (1 * 1)", lookupDatVariableValue));
            //TEST 10 Intermediate expression with LOTS of pairs of parentheses!: "(((((1 + 2 * 321 / 642)))))"
            EvaluatorTester.TestValidExpressions(10, 2, Evaluator.Evaluate("(((((0 + 1 * 642 / 321)))))", lookupDatVariableValue));
            //TEST 11: Simple addition expression while containing LOTS of parentheses!: "((((((((((10101 + 01010))))))))))"
            EvaluatorTester.TestValidExpressions(11, 11111, Evaluator.Evaluate("((((((((((10101 + 01010))))))))))", lookupDatVariableValue));
            //TEST 12: Simple expression with parentheses surrounding it: "(3 + 2)"
            EvaluatorTester.TestValidExpressions(12, 5, Evaluator.Evaluate("(3 + 2)", lookupDatVariableValue));
            //TEST 13: "2 - 2 + 5 - 5 + 3 - 3 + 9 - 9"
            EvaluatorTester.TestValidExpressions(13, 0, Evaluator.Evaluate("2 - 2 + 5 - 5 + 3 - 3 + 9 - 9", lookupDatVariableValue));
            //TEST 14: Simple expression with 2 sets of parentheses: "((4 - 8) / 2)"
            EvaluatorTester.TestValidExpressions(14, -2, Evaluator.Evaluate("((4 - 8) / 2)", lookupDatVariableValue));
            //TEST 15: "(100) / 2 / (5) / 10 + (0)" 
            EvaluatorTester.TestValidExpressions(15, 1, Evaluator.Evaluate("(100) / 2 / (5) / 10 + (0)", lookupDatVariableValue));
            //TEST 16: Simple expression with 2 sets of parentheses and lots of whitespace!: "(          (3))"
            EvaluatorTester.TestValidExpressions(16, 3, Evaluator.Evaluate("(          (3))", lookupDatVariableValue));
            //TEST 17: Complex expression: LOTS of parentheses and whitespace!: "( (8 + 2) + ( (32 * 2) ) - (((3 * 2 / 1))) * (((((6-6))))) )"
            EvaluatorTester.TestValidExpressions(17, 74, Evaluator.Evaluate("( (8 + 2) + ( (32 * 2) ) - (((3 * 2 / 1))) * (((((6-6))))) )", lookupDatVariableValue));
            //TEST 18: (102 / ABC123) + 21 * (100 - 99) 
            EvaluatorTester.TestValidExpressions(18, 38, Evaluator.Evaluate("(102 / ABC123) + 21 * (100 - 99)", lookupDatVariableValue));
            //TEST 19: Simple expression: Multiply operators, spaces, and only variable operands: "A1 * AB2 * ABC123" 
            EvaluatorTester.TestValidExpressions(19, 12, Evaluator.Evaluate("A1 * AB2 * ABC123", lookupDatVariableValue));
            //TEST 20: "((ABC123) + (AB2) - (A1))"
            EvaluatorTester.TestValidExpressions(20, 7, Evaluator.Evaluate("((ABC123) + (AB2) - (A1))", lookupDatVariableValue));
            //TEST 21: "AB2 / ABC123"
            EvaluatorTester.TestValidExpressions(21, 0, Evaluator.Evaluate("AB2 / ABC123", lookupDatVariableValue));
            //TEST 22: "((B1 + 100) * (123 - ABC123) / ((2*4) * AB2))"
            EvaluatorTester.TestValidExpressions(22, 804, Evaluator.Evaluate("((B1 + 100) * (123 - ABC123) / ((2*4) * AB2))", lookupDatVariableValue));
            //TEST 23: "5 (+ 10)"
            EvaluatorTester.TestValidExpressions(23, 15, Evaluator.Evaluate("5  (+ 10)", lookupDatVariableValue));
            //TEST 24: Long, complicated expression 
            EvaluatorTester.TestValidExpressions(24, 565, Evaluator.Evaluate("(100 * ( 50 + 100 - 100 / 5) - (3 + 2 * 2) + 2 ) / ( (((3 * 5))) + 27 / 9 + 5)", lookupDatVariableValue));
            //TEST 25: Containing just an operand: "ABC123"
            EvaluatorTester.TestValidExpressions(25, 6, Evaluator.Evaluate("ABC123", lookupDatVariableValue));
            //TEST 26: Variable has lowercase letters: "(abc987 + 78) / 2"
            EvaluatorTester.TestValidExpressions(26, 89, Evaluator.Evaluate("(abc987 + 78) / 2", lookupDatVariableValue));
            //TEST 27: Expression with inconsisent spacing: "( 5 *(43 + ABC123      )  +123)"
            EvaluatorTester.TestValidExpressions(27, 368, Evaluator.Evaluate("( 5 *(43 + ABC123      )  +123)", lookupDatVariableValue));
            //TEST 28: Long expression
            EvaluatorTester.TestValidExpressions(28, 24, Evaluator.Evaluate("5+3*7-8/(4+3)-2/2", lookupDatVariableValue));
            //TEST 29: Operand followed by a set of parentheses: "1()"
            EvaluatorTester.TestValidExpressions(29, 1, Evaluator.Evaluate("1()", lookupDatVariableValue));
            //TEST 30: Empty parentheses followed by an expression surrounded by parentheses
            EvaluatorTester.TestValidExpressions(30, 8, Evaluator.Evaluate("()(5+3)", lookupDatVariableValue));
            //TEST 31: 
            EvaluatorTester.TestValidExpressions(31, 0, Evaluator.Evaluate("B1-B1*B1/B1", lookupDatVariableValue));

            ///~INVALID EXPRESSIONS TESTS~///
            //TEST 1: Division by 0: "1 / 0"
            EvaluatorTester.TestInvalidExpressions(1, "1 / 0", lookupDatVariableValue);
            //TEST 2: Only a set of parentheses: "()"
            EvaluatorTester.TestInvalidExpressions(2, "()", lookupDatVariableValue);
            //TEST 3: ++5
            EvaluatorTester.TestInvalidExpressions(3, "++5", lookupDatVariableValue);
            //TEST 4: Leading operator followed by only one operand: "+ 1000"
            EvaluatorTester.TestInvalidExpressions(4, "+ 1000", lookupDatVariableValue);
            //TEST 5: Variable followed by two plus operators
            EvaluatorTester.TestInvalidExpressions(5, "ABC123++", lookupDatVariableValue);
            //TEST 6: Variable with no lookup variable value
            EvaluatorTester.TestInvalidExpressions(6, "ZX3", lookupDatVariableValue);
            //TEST 7: Empty String
            EvaluatorTester.TestInvalidExpressions(7, "", lookupDatVariableValue);
            //TEST 8: Null String
            EvaluatorTester.TestInvalidExpressions(8, null, lookupDatVariableValue);
            //TEST 9: Operators used as a unary operators
            EvaluatorTester.TestInvalidExpressions(9, "((+2) - (-4))", lookupDatVariableValue);
            //TEST 10: Only operators: "(+*/-)"
            EvaluatorTester.TestInvalidExpressions(10, "(+*/-)", lookupDatVariableValue);
            //TEST 11: Only operands: "3 700 2 ABC123 A1 AB2"
            EvaluatorTester.TestInvalidExpressions(11, "3 700 2 ABC123 A1 AB2", lookupDatVariableValue);
            //TEST 12: Illegal operator contained in expression
            EvaluatorTester.TestInvalidExpressions(12, "(7 + ?)", lookupDatVariableValue);
            //TEST 13: Illegal operator started at the beginning of the expression
            EvaluatorTester.TestInvalidExpressions(13, "^(4 * ABC123)", lookupDatVariableValue);
            //TEST 14: Minus and plus operators used as unary operators "-5 + -4"
            EvaluatorTester.TestInvalidExpressions(14, "-5 + -4", lookupDatVariableValue);
            //TEST 15: Expression with operands that precede an operator"(ABC123 3 +)" 
            EvaluatorTester.TestInvalidExpressions(15, "(ABC123 3 +)", lookupDatVariableValue);
            //TEST 16: "5----------3"
            EvaluatorTester.TestInvalidExpressions(16, "5----------3", lookupDatVariableValue);
            //TEST 17: Missing one left parenthesis 
            EvaluatorTester.TestInvalidExpressions(17, " 9 * (7) )", lookupDatVariableValue);
            //TEST 18: "3A + 5"
            EvaluatorTester.TestInvalidExpressions(18, "3A + 5", lookupDatVariableValue);
            //TEST 19: Invalid Variable Name: "(AA34A3Z) * 6 / 3"
            EvaluatorTester.TestInvalidExpressions(19, "(AA34A3Z) * 6 / 3", lookupDatVariableValue);
            //TEST 20: Illegal operator contained in variable name: "(A% / 7)"
            EvaluatorTester.TestInvalidExpressions(20, "(A% / 7)", lookupDatVariableValue);
            //TEST 21: An expression with implied multiplication: "5 (10)"
            EvaluatorTester.TestInvalidExpressions(21, "(10 +) 5 ", lookupDatVariableValue);
            //TEST 22: Another Invalid Variable Name: "A1X1X1X1X9 * 100"
            EvaluatorTester.TestInvalidExpressions(22, "A1X1X1X1X9", lookupDatVariableValue);
            //TEST 23: Division by zero without spaces
            EvaluatorTester.TestInvalidExpressions(23, "1/0", lookupDatVariableValue);
            //TEST 24: Many operators inside parentheses after valid expression
            EvaluatorTester.TestInvalidExpressions(24, "3 * (3 + */ ()", lookupDatVariableValue);
            //TEST 25: Alternating operator operand pattern
            EvaluatorTester.TestInvalidExpressions(25, "+1-2*3/4(5)6", lookupDatVariableValue);
            //TEST 26: Variable name starting with number 
            EvaluatorTester.TestInvalidExpressions(26, "1XXXX2", lookupDatVariableValue);
            //TEST 27: Space between two operands
            EvaluatorTester.TestInvalidExpressions(27, "3 3 * 6", lookupDatVariableValue);
            //TEST 28: Missing left parentheses: "5 + 6 * 4 / 1)
            EvaluatorTester.TestInvalidExpressions(28, "5 + 6 * 4 / 1)", lookupDatVariableValue);
            //TEST 29 "1 1 + 1"
            EvaluatorTester.TestInvalidExpressions(29, "1 1 + 1", lookupDatVariableValue);
            //TEST 30: Missing right hand parenthesis in expression
            EvaluatorTester.TestInvalidExpressions(30, "(5 + 6 * 4 / 1", lookupDatVariableValue);

            //Keeps the command line open while viewing output
            Console.ReadLine();
        }

        /// <summary>
        /// Finds a corresponding variable and returns the variable's respective numeric value
        /// </summary>
        /// <param name="variable"></param>
        /// <returns>the numeric value; throws ArgumentException if unable to find value for particular variable</returns>
        public static int LookUpVariable(String variable)
        {
            switch (variable)
            {
                case "A1": return 1;
                case "AB2": return 2;
                case "ABC123": return 6;
                case "B1": return 10;
                case "abc987": return 100;
            }
            //Variable's value doesn't exist in lookup, so throw an exception 
            throw new ArgumentException("Cannot find variable's value");
        }

        /// <summary>
        /// A helper method to test all valid expressions used to validate the functionality of the FormulaEvaluator 
        /// </summary>
        /// <param name="testCaseNumber" the id number corresponding to a particular kind of expression used to test></param>
        /// <param name="expected" what value to expect when evaluating the expression></param>
        /// <param name="actual" what the program actually evaluated the expression to be ></param>
        public static void TestValidExpressions(int testCaseNumber, int expected, int actual)
        {
            if (expected == actual)
            {
                Console.WriteLine("Valid Expressions TEST " + testCaseNumber + " passed successfully!");
            }
            else
            {
                Console.WriteLine("Valid Expressions TEST " + testCaseNumber + " failed; expected " + expected + " but was " + actual);
            }
        }

        /// <summary>
        /// A helper method to test all invalid expressions for validating the functionality of the FormulaEvaluator
        /// </summary>
        /// <param name="testCaseNumber" the id number corresponding to a particular kind of expression used to test></param>
        /// <param name="expected" what value to expect when evaluating the expression></param>
        /// <param name="lookup" detects a parameterized variable's value></param>
        public static void TestInvalidExpressions(int testCaseNumber, String expected, Evaluator.Lookup lookup)
        {
            try
            {
                int tryExpectedValue = Evaluator.Evaluate(expected, lookup);
                Console.WriteLine("Invalid Expressions TEST " + testCaseNumber + " failed; expecting an exception");
            }
            catch (DivideByZeroException)
            {
                try
                {
                    throw new ArgumentException("Cannot divide by 0!");
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Invalid Expressions ArgumentException TEST " + testCaseNumber + " passed successfully!");
                }
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid Expressions ArgumentException TEST " + testCaseNumber + " passed successfully!");
            }
        }
    }
}
