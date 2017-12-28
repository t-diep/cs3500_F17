using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

/// <summary>
/// A library that contains methods that evaluate mathematical expressions
/// 
/// @author Tony Diep
/// </summary>
namespace FormulaEvaluator
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
        public static bool IsOnTopOfStack(this Stack<String> stack, String value)
        {
            return stack.Count > 0 && stack.Peek().Equals(value);
        }
    }

    /// <summary>
    /// Represents our evaluator object; used to evaluate mathematical expressions
    /// with the help of a lookup delegate; valid mathematical expressions include
    /// addition, subtraction, multiplication, division, non-negative numbers, and
    /// parentheses 
    /// </summary>
    public static class Evaluator
    {
        /// <summary>
        /// Evaluates / computes a given mathematical function while also using a lookup function 
        /// to evaluate numerical values in the case of variables
        /// </summary>
        /// <param name="token" - the token to be passed under our evaluator function></param>
        /// <returns> a numeric value corresponding to our operand </returns>
        public delegate int Lookup(String token);

        /// <summary>
        /// Evaluates a given formula/expression that may compute a numerical value 
        /// </summary>
        /// <param name="exp" - the mathematical expression to be evaluated></param>
        /// <param name="variableEvaluator" - a lookup function that evaluates a variable's value></param>
        /// <returns>the numerical answer to the given formula/expression; throws an ArgumentException if cannot evaluate</returns>
        public static int Evaluate(String exp, Lookup variableEvaluator)
        {
            //Do preliminary check to see if it's a valid expression
            HasRightParenthesisOperandPattern(exp);

            //Holds result after computing a sub mathematical expression (which is used to be pushed back onto value stack)
            int currentEvaluatedValue = 0;

            //The numeric value from a digit token
            int parsedValue = 0;

            //Parenthesis counters; ensuring that each left parenthesis has a corresponding right parenthesis
            int numLeftParentheses = 0;
            int numRightParentheses = 0;

            //Breaks the whole mathematical expression into separate, individual cleanTokens 
            String[] demTokens = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            //Numbers, variables
            Stack<int> theOperands = new Stack<int>();
            //+, -, *, /, ()
            Stack<String> theOperators = new Stack<String>();

            //Variables for operands and operator
            String currentOperator = "";
            int topValue = 0;
            int bottomValue = 0;

            //Iterate each token
            foreach (String token in demTokens)
            {
                String cleanToken = token.Trim();

                //Skip whitespace token
                if(cleanToken.Equals(""))
                     { continue; }
              
                //Token is an integer
                if (int.TryParse(cleanToken, out parsedValue))
                {
                    //Token is either a "*" or "/"; go ahead and evaluate expression and push that result back onto operand stack
                    if (FormulaExtensionStack.IsOnTopOfStack(theOperators, "*") || FormulaExtensionStack.IsOnTopOfStack(theOperators, "/"))
                    {
                        //Operands stack is empty
                        if (theOperands.Count < 1)
                        {
                            throw new ArgumentException("Operand stack is empty...");
                        }

                        if(theOperators.Count > 0 && theOperators.Peek().Equals(")"))
                        {
                            throw new ArgumentException("Operand follows after right parenthesis");
                        }

                        //Pop two operands out of the value stack
                        int currentValue = theOperands.Pop();
                        currentOperator = theOperators.Pop();

                        //Compute product
                        if (currentOperator.Equals("*"))
                        {
                            currentEvaluatedValue = (currentValue * parsedValue);
                        }
                        //Compute quotient
                        else
                        {
                            try
                            {
                                currentEvaluatedValue = (currentValue / parsedValue);
                            }
                            catch(DivideByZeroException)
                            {
                                throw new ArgumentException("Cannot divide by 0!");
                            }                           
                        }

                        theOperands.Push(currentEvaluatedValue);
                    }
                    //There's no "*" or "/" operator after this current token, so push current cleanToken to operand stack
                    else
                    {
                        theOperands.Push(parsedValue);
                    }
                }
                //Token is a variable                  
                else if (cleanToken.Length > 1)
                {
                    //Variable must start with letter and end with a digit number
                    if(char.IsLetter(cleanToken[0]) && !char.IsDigit(cleanToken[cleanToken.Length - 1]))
                    {
                        throw new ArgumentException("Variable not in correct format (should start with letters followed by digits");
                    }
                    else
                    {
                        //Lookup variable value, if it exists
                        int variableValue = variableEvaluator(cleanToken);

                        if (FormulaExtensionStack.IsOnTopOfStack(theOperators, "*") || FormulaExtensionStack.IsOnTopOfStack(theOperators, "/"))
                        {
                            //Retrieve two operands on the value stack
                            topValue = theOperands.Pop();
                            currentOperator = theOperators.Pop();

                            //Compute product
                            if(currentOperator.Equals("*"))
                            {
                                currentEvaluatedValue = topValue * variableValue;
                            }
                            //Compute quotient; throw exception if dividing by zero
                            else
                            {
                                if(variableValue == 0)
                                {
                                    throw new ArgumentException("Cannot divide by zero!");
                                }
                                else
                                {
                                    currentEvaluatedValue = topValue / variableValue;
                                }                               
                            }

                            //Put result back onto value stack
                            theOperands.Push(currentEvaluatedValue);
                        }
                        else
                        {
                            theOperands.Push(variableValue);
                        }                      
                    }
                }
                //Token is one of the higher precedence operators ("*", "/", "(" )
                else if (cleanToken.Equals("*") || cleanToken.Equals("/") || cleanToken.Equals("("))
                {
                    theOperators.Push(cleanToken);

                    if (cleanToken.Equals("("))
                    {
                        numLeftParentheses++;
                    }

                }
                //Token is one of the lower precedence operators ("+", "-", ")" )
                else if (cleanToken.Equals("+") || cleanToken.Equals("-") || cleanToken.Equals(")"))
                {
                    //Check if the operator on top of the operator stack is either a "+" or "-"
                    if (FormulaExtensionStack.IsOnTopOfStack(theOperators, "+") || FormulaExtensionStack.IsOnTopOfStack(theOperators, "-"))
                    {
                        //Check if there's enough operands to evaluate before popping off the operand stack
                        if (theOperands.Count < 2)
                        {
                            throw new ArgumentException("There's not at least operands to use to evaluate expression");
                        }

                        //Take two operands and one operator to evaluate
                        topValue = theOperands.Pop();
                        bottomValue = theOperands.Pop();
                        currentOperator = theOperators.Pop();

                        //Compute addition
                        if (currentOperator.Equals("+"))
                        {
                            currentEvaluatedValue = bottomValue + topValue;
                        }
                        //Compute subtraction
                        else
                        {
                            currentEvaluatedValue = bottomValue - topValue;
                        }

                        //Put result back onto operand stack
                        theOperands.Push(currentEvaluatedValue);
                    }
                    //Token is a right parenthesis
                    if (cleanToken.Equals(")"))
                    {
                        String leftParenthesis = "";
                        numRightParentheses++;

                        //Left parenthesis is the only operator on operator stack, so take it out
                        if (theOperators.Count > 0)
                        {
                            leftParenthesis = theOperators.Pop();                           
                        }
                        //An non-right parenthesis operator is not on there or that the operator stack is empty, 
                        //so throw an exception
                        else if(!leftParenthesis.Equals("(") || theOperators.Count == 0)
                        {
                            throw new ArgumentException("A left parenthesis appeared when it should not appear at that time");
                        }

                        //Check if operator on top of operator stack is either a "*" or "/", if there's at least one operator
                        if (theOperators.Count > 0 && (theOperators.Peek().Equals("*") || theOperators.Peek().Equals("/")))
                        {
                            //Take out two operands and one operator to evaluate
                            topValue = theOperands.Pop();
                            bottomValue = theOperands.Pop();
                            currentOperator = theOperators.Pop();

                            //Compute product
                            if (currentOperator.Equals("*"))
                            {
                                currentEvaluatedValue = bottomValue * topValue;
                            }
                            //Compute quotient
                            else
                            {
                                //Check for possible division by zero
                                if(topValue == 0)
                                {
                                    throw new DivideByZeroException("Cannot divide by zero!");
                                }

                                currentEvaluatedValue = bottomValue / topValue;
                            }

                            //Put new result back onto operand stack
                            theOperands.Push(currentEvaluatedValue);
                            continue;
                        }
                    }
                    else
                    {
                        theOperators.Push(cleanToken);
                    }     
                }
                //Token is an illegal operator that is not used in this FormulaEvaluator, so throw an exception
                else
                {
                    throw new ArgumentException("Token contains illegal operator not used to evaluate expressions");
                }
            }//end foreach loop 

            //Check if There's still an operator left to use
            if (theOperators.Count > 0)
            {
                //When there isn't exactly two operands for one operator 
                if (theOperands.Count < 2 || theOperators.Count < 1)
                {
                    throw new ArgumentException("There's not at least 2 operands and 1 operator to compute");
                }

                //Take out two operands and one operator to evaluate
                currentOperator = theOperators.Pop();
                topValue = theOperands.Pop();
                bottomValue = theOperands.Pop();

                //Compute sum
                if (currentOperator.Equals("+"))
                {
                    currentEvaluatedValue = bottomValue + topValue;
                }
                //Compute difference
                else
                {
                    currentEvaluatedValue = bottomValue - topValue;
                }
            }
            //Operator stack is empty
            else
            {
                //No operator to use and only the single operand, so just simply add it to operand stack
                if(theOperands.Count == 1)
                {
                    //Only one value left on the operand stack so this should be the final result
                    currentEvaluatedValue = theOperands.Pop();
                }
                //No operands to use
                else
                {
                    throw new ArgumentException("There are no more operators to use for remaining operands left");
                }
            }

            //Check if expression's number of left parentheses doesn't match the number of right parentheses
            if (numLeftParentheses != numRightParentheses)
            {
                throw new ArgumentException("Number of left parentheses don't match number of right parentheses in expression");
            }

            //Return final answer
            return currentEvaluatedValue;

        } //end of Evaluate method

        /// <summary>
        /// Verifies whether the expression contains a right parenthesis followed by an operand
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        private static void HasRightParenthesisOperandPattern(String exp)
        {
            for(int index = 1; index < exp.Length; index++)
            {
                String prev = exp[index - 1].ToString();
                String curr = exp[index].ToString();
                bool isDigit = int.TryParse(curr, out int result);

                if(prev.Equals(")") && isDigit)
                {
                    throw new ArgumentException();
                }
            }
        }

    } //end of Evaluator class

}//end of namespace FormulaEvaluator
