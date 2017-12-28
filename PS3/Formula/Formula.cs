// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// (Daniel Kopta) 
// Version 1.2 (9/10/17) 

// Change log:
//  (Version 1.2) Changed the definition of equality with regards
//                to numeric tokens

// (Tony Diep)
// (Version 1.3) (9/22/17) Implemented code in provided method stubs and made helper methods
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// 
/// </summary>
namespace SpreadsheetUtilities
{

    /// <summary>
    /// Allows us to add customary methods to the built-in Stack data structure in C#
    /// </summary>
    public static class FormulaExtensionStack
    {
        /// <summary>
        /// Extra customary method that checks whether a specific element is on top of the stack
        /// </summary>
        /// <param name="stack" the operator stack or the operand stack to look into></param>
        /// <param name="value" the operator to check for if it's on top of the stack></param>
        /// <returns>true if the value is on top of the stack and false otherwise</returns>
        public static bool IsOnTopOfStack(this Stack<String> stack, String firstOperator, String secondOperator)
        {
            if (stack.Count > 0 && ((stack.Peek().Equals(firstOperator) || stack.Peek().Equals(secondOperator))))
            {
                return true;
            }

            return false;
        }
    }

    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax; variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {
        //Holds all tokens from given formula
        private List<String> datListOfDemTokens;
        //Holds all variables after "normalizing" them (that is, uppercasing them)
        private List<String> normalizedVariables;

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            //Holds a list of all the individual tokens in the formula
            datListOfDemTokens = new List<string>(GetTokens(formula));
            //Holds all of the normalized variables 
            normalizedVariables = new List<string>();

            //Put formula into preliminary error check tests before proceeding
            TestFormulaThroughPremlinaryErrorChecks(formula, datListOfDemTokens, normalizedVariables, normalize, isValid);

            //Counters to keep track of number left and right parentheses
            int countLeftParentheses = 0;
            int countRightParentheses = 0;

            //Check each token in our formula for second round of errors
            foreach (String token in datListOfDemTokens)
            {
                if (IsValidToken(token, ref countLeftParentheses, ref countRightParentheses))
                {
                    continue;
                }
                else
                {
                    throw new FormulaFormatException("Illegal token in formula");
                }
            }

            //Verify if the total number of left parentheses doesn't equal the total number of right parentheses
            if (countLeftParentheses != countRightParentheses)
            {
                throw new FormulaFormatException("The number of left parentheses does not match the number of right parentheses in the formula...");
            }

            //Normalize our variables
            for (int index = 0; index < datListOfDemTokens.Count; index++)
            {
                String nonNormalizedToken = datListOfDemTokens[index];

                if (IsVariable(datListOfDemTokens[index]))
                {
                    datListOfDemTokens[index] = normalize(nonNormalizedToken);
                    normalizedVariables.Add(datListOfDemTokens[index]);
                }
            }
        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            //Helper methods used in this method may throw exceptions, so catch those here
            //and return a FormulaError object
            try
            {
                //Set up the stacks
                Stack<String> operatorStack = new Stack<String>();
                Stack<double> operandStack = new Stack<double>();

                //Variables to help us keep track of our
                double topValue = 0;
                double nextValue = 0;
                String oper = "";

                //Go through each token, placing them in their respective stacks and
                //performing their respective algorithms
                foreach (String token in datListOfDemTokens)
                {
                    object result = DesignateTokenToCorrectStack(operatorStack, operandStack, token, lookup);

                    if (result is FormulaError)
                    {
                        return result;
                    }
                }

                //Check if operator stack is empty
                if (operatorStack.Count < 1)
                {
                    return operandStack.Pop();
                }
                //Operator stack is empty, presumably at least two operands and at least one operator
                else
                {
                    topValue = operandStack.Pop();
                    nextValue = operandStack.Pop();
                    oper = operatorStack.Pop();

                    return ComputeResult(oper, ref nextValue, ref topValue);
                }
            }
            catch (ArgumentException)
            {
                return new FormulaError("Unable to evaluate formula");
            }
        }


        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            IEnumerable<String> copyOfNormalizedVariables = new List<String>(normalizedVariables);
            return copyOfNormalizedVariables;
        }

        /// <summary>
        /// Helps compare if two lists are the same; that is, they have the exact same elements 
        /// among each other
        /// </summary>
        /// <param name="firstList">first list</param>
        /// <param name="secondList">second list</param>
        /// <returns>true if all elements in first list also exist in the second one
        /// and false otherwise</returns>
        public bool ListsAreTheSame(List<String> firstList, List<String> secondList)
        {
            //Check all elements in first list to second list
            foreach (String token in firstList)
            {
                if (!secondList.Contains(token))
                {
                    return false;
                }
            }

            //Check all elements in second list to first list
            foreach (String token in secondList)
            {
                if (!firstList.Contains(token))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            String theFormula = "";

            foreach (String token in datListOfDemTokens)
            {
                theFormula += token;
            }

            return theFormula;
        }

        /// <summary>
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object obj)
        {
            //Object is null or a non-Formula type object, so it's automatically not equal
            if (ReferenceEquals(obj, null) || !(obj is Formula))
            {
                return false;
            }

            //"Cast" the current object as a Formula object
            Formula otherFormulaObject = obj as Formula;

            //Check every token from both Formula objects and compare each one of them to each other
            for (int index = 0; index < this.datListOfDemTokens.Count; index++)
            {
                String thisCurrentToken = this.datListOfDemTokens[index];
                String otherCurrentToken = otherFormulaObject.datListOfDemTokens[index];

                //Start comparing decimal and integer representations of the numbers
                if (!TwoFormulasHaveMatchingTokens(thisCurrentToken, otherCurrentToken))
                {
                    return false;
                }
            }

            //The two formulas are indeed equal
            return true;
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return true.  If one is
        /// null and one is not, this method should return false.
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            //Both formulas are null
            if (ReferenceEquals(f1, null) && ReferenceEquals(f2, null))
            {
                return true;
            }
            //One or the other but not both are null
            else if ((ReferenceEquals(f1, null) && !ReferenceEquals(f2, null)) || (!ReferenceEquals(f1, null) && ReferenceEquals(f2, null)))
            {
                return false;
            }

            //Check for further comparison
            return f1.Equals(f2);
        }

        /// <summary>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return false.  If one is
        /// null and one is not, this method should return true.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            if (!(f1 == f2))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            return this.ToString().Trim().GetHashCode();
        }

        //~HELPER METHODS~//

        /// <summary>
        /// Helps check if a current token in a formula is either the following:
        /// 
        /// ~A valid variable
        /// ~A floating point number
        /// ~A valid operator
        /// 
        /// </summary>
        /// <param name="token">the current string to check</param>
        /// <returns>true if the token meets any of the three criteria above and false otherwise</returns>
        private static bool IsValidToken(String token, ref int leftCounter, ref int rightCounter)
        {
            return IsVariable(token) || Double.TryParse(token, out double decimalVal) ||
                IsValidOperator(ref leftCounter, ref rightCounter, token);
        }

        /// <summary>
        /// Depending on which of the four arithmetic operator, this helper computes 
        /// the respective expression
        /// </summary>
        /// <param name="oper">the arithmetic operator</param>
        /// <param name="firstValue">value after popping the operand stack once</param>
        /// <param name="secondValue">value after popping the operand stack again</param>
        /// <returns>the result in an object form</returns>
        private static object ComputeResult(String oper, ref double firstValue, ref double secondValue)
        {
            //Compute respective expressions based on specific operator
            try
            {
                switch (oper)
                {
                    case "+":
                        return firstValue + secondValue;
                    case "-":
                        return firstValue - secondValue;
                    case "*":
                        return firstValue * secondValue;
                    case "/":
                        if (secondValue == 0)
                        {
                            throw new DivideByZeroException();
                        }

                        return firstValue / secondValue;
                }
            }
            catch (DivideByZeroException)
            {
                return new FormulaError("Error: Division by 0...");
            }

            return new ArgumentException("Error: Unable to evaluate formula");
        }

        /// <summary>
        /// Helps take each token in a given formula and pushes them into their respective stacks
        /// </summary>
        /// <param name="operators"></param>
        /// <param name="operands"></param>
        /// <param name="value"></param>
        private static object DesignateTokenToCorrectStack(Stack<String> operators, Stack<double> operands, String value, Func<string, double> lookup)
        {
            try
            {
                String currentOperator = "";
                double topValue = 0;

                switch (value)
                {
                    //Higher precedence operators
                    case "*":
                    case "/":
                    case "(":
                        operators.Push(value);
                        break;

                    //Lower precedence operators
                    case "+":
                    case "-":
                        ArithmeticOperatorAlgorithm(operators, operands);
                        operators.Push(value);
                        break;

                    case ")":
                        if (RightParenthesisAlgorithm(operators, operands))
                        {
                            break;
                        }
                        else
                        {
                            throw new DivideByZeroException("Divide by 0...");
                        }
                }

                //Check for operands; token is a floating point number
                if (Double.TryParse(value, out double decimalVal))
                {
                    if (FormulaExtensionStack.IsOnTopOfStack(operators, "*", "/"))
                    {
                        currentOperator = operators.Pop();
                        topValue = operands.Pop();

                        object result = ComputeResult(currentOperator, ref topValue, ref decimalVal);

                        if (result is FormulaError)
                        {
                            throw new ArgumentException("Division by zero");
                        }

                        operands.Push((double)result);
                    }
                    else
                    {
                        operands.Push(decimalVal);
                    }

                }
                //Token is a variable
                else if (IsVariable(value))
                {
                    double lookupVal = lookup(value);

                    if (ArithmeticOperatorAlgorithm(operators, operands))
                    {
                        operands.Push(lookupVal);
                    }
                    else
                    {
                        currentOperator = operators.Pop();
                        topValue = operands.Pop();
                        operands.Push((double)ComputeResult(currentOperator, ref topValue, ref lookupVal));
                    }
                }
            }
            catch (DivideByZeroException)
            {
                return new FormulaError("Divide by zero");
            }

            return 0;
        }

        /// <summary>
        /// Helps perform the formula evaluator algorithm depending on which of the four
        /// arithmetic operators is currently on top of the operator stack
        /// </summary>
        /// <param name="operatorStack">Stack containing operators</param>
        /// <returns>true if the algorithm is able to perform without exceptions 
        /// with respect to the stack and false otherwise</returns>
        private static bool ArithmeticOperatorAlgorithm(Stack<String> operatorStack, Stack<double> operandStack)
        {
            double topValue = 0;
            double nextValue = 0;
            String currOperator = "";

            try
            {
                //"+" or "-" is currently on top of the operator stack
                if (FormulaExtensionStack.IsOnTopOfStack(operatorStack, "+", "-"))
                {
                    topValue = operandStack.Pop();
                    nextValue = operandStack.Pop();
                    currOperator = operatorStack.Pop();

                    if (currOperator.Equals("+"))
                    {
                        operandStack.Push(nextValue + topValue);
                    }
                    else
                    {
                        operandStack.Push(nextValue - topValue);
                    }
                }
                //"*" or "/" is currently on top of the operator stack
                else if (FormulaExtensionStack.IsOnTopOfStack(operatorStack, "*", "/"))
                {
                    topValue = operandStack.Pop();
                    nextValue = operandStack.Pop();
                    currOperator = operatorStack.Pop();

                    if (currOperator.Equals("*"))
                    {
                        operandStack.Push(nextValue * topValue);
                    }
                    else
                    {
                        if (topValue == 0)
                        {
                            throw new DivideByZeroException("Cannot divide by 0");
                        }

                        operandStack.Push(nextValue / topValue);
                    }
                }
            }
            catch (InvalidOperationException)
            {
                //Keep track of the last operand after the exception occurred on the operator stack
                operandStack.Push(topValue);
                return false;
            }
            catch (DivideByZeroException)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Helps perform the formula evaluator algorithm in the case there exists either
        /// a multiply or divide operator on the operator stack
        /// </summary>
        /// <param name="operatorStack">contains the operators</param>
        /// <param name="operandStack">contains the operands</param>
        private static bool RightParenthesisAlgorithm(Stack<String> operatorStack, Stack<double> operandStack)
        {
            ArithmeticOperatorAlgorithm(operatorStack, operandStack);

            String leftParenthesis = operatorStack.Pop();

            while (!leftParenthesis.Equals("("))
            {
                leftParenthesis = operatorStack.Pop();
            }

            if (ArithmeticOperatorAlgorithm(operatorStack, operandStack))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Helps check for errors on formula before checking every token in entire formula
        /// </summary>
        /// <param name="formula">the formula in string form</param>
        /// <param name="normalize">our normalizer that uppercases variable names</param>
        /// <param name="isValid">our verifier to see if formula is correct</param>
        /// <returns></returns>
        private static void TestFormulaThroughPremlinaryErrorChecks(String formula, List<String> tokens, List<String> variables, Func<string, string> normalize, Func<string, bool> isValid)
        {
            String startToken = "";
            String endToken = "";
            String previousToken = "";
            String currentToken = "";

            //Retrieve start and last token from this formula
            if (tokens.Count > 0)
            {
                startToken = tokens[0];
                endToken = tokens[tokens.Count - 1];
            }

            //Check under the "one token rule"
            if (String.IsNullOrEmpty(formula) || ReferenceEquals(formula, null))
            {
                throw new FormulaFormatException("Formula is either an empty string or null");
            }
            //Check for if there are only two tokens
            if (tokens.Count == 2)
            {
                throw new FormulaFormatException("There are only two tokens in this formula");
            }
            //Check for correct normalization of a formula
            if (!isValid(formula))
            {
                throw new FormulaFormatException("Formula is not a valid formula");
            }
            //Check if first token is neither a left parenthesis, a variable, or a decimal
            if (!startToken.Equals("(") && !(IsVariable(startToken)) && !Double.TryParse(startToken, out double decimalVal))
            {
                throw new FormulaFormatException("First token of formula is neither a left parenthesis, a valid variable, or a decimal");
            }
            //Check if last token is neither a right parenthesis, a variable, or a decimal
            if (!IsValidDecimalOrVariableOrRightParenthesis(endToken))
            {
                throw new FormulaFormatException("First token of formula is neither a right parenthesis, a valid variable, or a decimal");
            }

            //Check for the same type of operator in a row
            for (int index = 1; index < tokens.Count; index++)
            {
                previousToken = tokens[index - 1];
                currentToken = tokens[index];

                if (BothTokensAreSameArithmeticOperators(previousToken, currentToken))
                {
                    throw new FormulaFormatException("Same operator in a row");
                }
            }

            //Check for operands that are next to a left parenthesis or after a right parenthesis
            for (int index = 1; index < tokens.Count; index++)
            {
                previousToken = tokens[index - 1];
                currentToken = tokens[index];

                if (IsValidOperandOrVariable(previousToken) && currentToken.Equals("("))
                {
                    throw new FormulaFormatException("Operand preceeds left parentheses");
                }
                else if (previousToken.Equals(")") && IsValidOperandOrVariable(currentToken))
                {
                    throw new FormulaFormatException("Operand follows after right parentheses");
                }
            }
        }

        /// <summary>
        /// Reports whether the current token is a valid variable or operand
        /// </summary>
        /// <param name="token"></param>
        /// <returns>true if token is valid variable or operand and false otherwise</returns>
        private static bool IsValidOperandOrVariable(String token)
        {
            return Double.TryParse(token, out double floatNum) || IsVariable(token);
        }

        /// <summary>
        /// Helps determine if two tokens contain same operator amongst each other
        /// </summary>
        /// <returns>true if both tokens contain same operator and false otherwise</returns>
        private static bool BothTokensAreSameArithmeticOperators(String previousToken, String currentToken)
        {
            String bothTokens = previousToken + currentToken;

            switch (bothTokens)
            {
                case "++":
                case "--":
                case "**":
                case "//":
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Helps check if a token is either a valid variable, decimal, or a right parenthesis
        /// </summary>
        /// <param name="token">the current token to check</param>
        /// <returns>true if it's either of the three tokens above and false otherwise</returns>
        private static bool IsValidDecimalOrVariableOrRightParenthesis(String token)
        {
            return token.Equals(")") || (IsVariable(token)) || Double.TryParse(token, out double decimalVal);
        }

        /// <summary>
        /// Helps determine if a given token is a valid operator; that is, the token 
        /// is either a "+", "-", "*", "/", "(", or ")"; also checks for equal number
        /// of parentheses 
        /// </summary>
        /// <param name="leftCounter">counts how many left parentheses</param>
        /// <param name="rightCounter">counts how many right parentheses</param>
        /// <param name="token">current token to check if valid</param>
        /// <returns>true if the token is a valid operator and false otherwise</returns>
        private static bool IsValidOperator(ref int leftCounter, ref int rightCounter, String token)
        {
            switch (token)
            {
                case "+":
                case "-":
                case "*":
                case "/":
                    return true;
                case "(":
                    leftCounter++;
                    return true;
                case ")":
                    rightCounter++;
                    if (rightCounter > leftCounter)
                    {
                        throw new FormulaFormatException("Number of right parentheses cannot exceed number of left parentheses!");
                    }
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Helps compare two tokens in either in decimal form (if parsable to a decimal)
        /// or in string form, or both from two formula objects 
        /// </summary>
        /// <param name="thisFormulaToken">the token coming from "this" Formula object</param>
        /// <param name="otherFormulaToken">the token coming from the "other" Formula object</param>
        /// <returns></returns>
        private static bool TwoFormulasHaveMatchingTokens(String thisFormulaToken, String otherFormulaToken)
        {
            if (Double.TryParse(thisFormulaToken, out double thisDecimal) && Double.TryParse(otherFormulaToken, out double otherDecimal))
            {
                if (thisDecimal != otherDecimal)
                {
                    return false;
                }

                return true;

            }
            return thisFormulaToken.Equals(otherFormulaToken);
        }

        /// <summary>
        /// Verifies if a token in a formula is a variable (assuming it follows the syntax rules
        /// of what qualifies to be a valid variable name)
        /// </summary>
        /// <param name="token">The current token in a formula</param>
        /// <returns>true if a token is a variable and false otherwise</returns>
        private static bool IsVariable(String token)
        {
            return Regex.IsMatch(token, @"[a-zA-Z_](?: [a-zA-Z_]|\d)*$", RegexOptions.Singleline) && token.Length > 1;
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }

        }
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }
}