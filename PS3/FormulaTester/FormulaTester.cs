///
/// (Tony Diep)
/// Added some unit tests for the Formula class
///
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using System.Collections.Generic;

/// <summary>
/// Provides a collection of unit tests for the Formula class
/// </summary>
namespace FormulaTester
{
    [TestClass]
    public class FormulaTester
    {
        //~VALID EXPRESSION TESTS~//

        //CONSTRUCTOR METHOD TESTS
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TwoOpeartorsBetweenTwoOperandsWithNormalizerAndValidator()
        {
            Formula f = new Formula("5 + + 3", input => input, valid => false);
        }

        /// <summary>
        /// Checking its formula before evaluating
        /// </summary>
        [TestMethod]
        public void TestFormulaConstructorOneParameterCheckFormulaString()
        {
            Formula simpleFormula = new Formula("50 - 60");

            Assert.AreEqual("50-60", simpleFormula.ToString());
        }

        /// <summary>
        /// Checking its value after evaluating the formula
        /// </summary>
        [TestMethod]
        public void TestFormulaConstructorOneParameterCheckEvaluatorValue()
        {
            Formula simpleFormula = new Formula("50 - 60");

            Assert.AreEqual(-10.0, (double)simpleFormula.Evaluate(noVariables => 0));
        }

        /// <summary>
        /// Checking if the formula contains no variables
        /// </summary>
        [TestMethod]
        public void TestFormulaConstructorOneParameterCheckVariables()
        {
            Formula simpleFormula = new Formula("50 - 60");

            Assert.IsTrue(new HashSet<String>().SetEquals(simpleFormula.GetVariables()));
        }

        /// <summary>
        /// Checking the string before evaluating the formula; should be the formula string
        /// we passed in to the Formula constructor
        /// </summary>
        [TestMethod]
        public void TestFormulaConstructorThreeParametersCheckString()
        {
            Formula complicatedFormula = new Formula("(((((4 * 5 + A1)))))", A1 => A1, valid => true);

            Assert.AreEqual("(((((4*5+A1)))))", complicatedFormula.ToString());
        }

        /// <summary>
        /// Check the result matches our expected result; should evaluate to 21 when checking the string
        /// </summary>
        [TestMethod]
        public void TestFormulaConstructorThreeParametersCheckEvaluatorValue()
        {
            Formula complicatedFormula = new Formula("(((((4 * 5 + A1)))))", A1 => A1.ToUpper(), valid => true);

            Assert.AreEqual("(((((4*5+A1)))))", complicatedFormula.ToString());
            Assert.AreEqual(21, (double)complicatedFormula.Evaluate(A1 => 1));
        }

        /// <summary>
        /// Checks for all of the variables in this formula; should return the only one variable
        /// in the formula: A1
        /// </summary>
        [TestMethod]
        public void TestFormulaConstructorThreeParametersCheckVariables()
        {
            Formula complicatedFormula = new Formula("(((((4 * 5 + A1)))))", A1 => A1.ToUpper(), valid => true);

            List<String> expected = new List<String> { "A1" };
            List<String> actual = new List<String>(complicatedFormula.GetVariables());

            Assert.IsTrue(complicatedFormula.ListsAreTheSame(expected, actual));
        }

        ////EQUALS METHOD TESTS

        /// <summary>
        /// Integer and decimal representation should be the same if representing a whole number
        /// </summary>
        [TestMethod]
        public void CompareTwoFormulasOneBeingIntegerTheOtherDecimalWhole()
        {
            Formula onePlusOneInteger = new Formula("1 + 1");
            Formula onePlusOneDecimal = new Formula("1.0 + 1.0");

            object result = onePlusOneInteger.Evaluate(null);

            Assert.IsTrue(onePlusOneInteger.Equals(onePlusOneDecimal));
            Assert.AreEqual(2.0, (double)result);
        }

        /// <summary>
        /// By the .Equals() method specifications, should return false
        /// </summary>
        [TestMethod]
        public void CompareOneNullObjectAndOneNonNullFormula()
        {
            Formula nonNullFormula = new Formula("123 + 0.50");

            Assert.IsFalse(nonNullFormula.Equals(null));
        }

        /// <summary>
        /// The two formulas are different, so the Equals() method should return false
        /// </summary>
        [TestMethod]
        public void CompareTwoNonNullFormulasWithDifferentFormulas()
        {
            Formula allHardCodedNumbersFormula = new Formula("3 + 100 * 32");
            Formula allVariablesFormula = new Formula("A1 + B1 + C1");

            Assert.IsFalse(allHardCodedNumbersFormula.Equals(allVariablesFormula));
        }

        /// <summary>
        /// The .Equals() method specifications say that the two formulas that are communtatives
        /// of each other should return false, even though they both contain the same operand(s)
        /// and operator(s)
        /// </summary>
        [TestMethod]
        public void CompareTwoVariablesThatHaveTheVariablesButAreArrangedDifferently()
        {
            Formula numberThenVariable = new Formula("3 + A1");
            Formula variableThenNumber = new Formula("A1 + 3");

            Assert.IsFalse(numberThenVariable.Equals(variableThenNumber));
        }

        /// <summary>
        /// Even though the two formulas both contain the same number and variable, they 
        /// are different because one has a uppercase A and the other one with a lowercase a.
        /// Thus, we expect the two formulas to not be the same by the .Equals() method
        /// </summary>
        [TestMethod]
        public void CompareFormulasWithAnUpperCaseVariableAndTheOtherLowerCaseVariable()
        {
            Formula upperCaseVariable = new Formula("32 / A7");
            Formula lowerCaseVariable = new Formula("32 / a7", input => input.ToUpper(), validFormula => true);

            Assert.IsTrue(upperCaseVariable.Equals(lowerCaseVariable));
        }

        /// <summary>
        /// When passing in a "FUNC" that converts lowercase letters to uppercase letters in a variable,
        /// the two formulas should now be the same by the .Equals() method
        /// </summary>
        [TestMethod]
        public void ConvertingLowercaseVariableToUpperCaseVariable()
        {
            Formula lowerCaseVariable = new Formula("32 / A7", input => input.ToUpper(), input => true);
            Formula upperCaseVariable = new Formula("32 / A7");

            Assert.IsTrue(lowerCaseVariable.Equals(upperCaseVariable));
        }

        /// <summary>
        /// Even though the two formulas contain the same variable names, one is uppercased 
        /// and the other being lowercased.  By the .Equals() method, the result should be false
        /// </summary>
        [TestMethod]
        public void TwoLowerCaseVariablesVersusTwoUpperCaseVariables()
        {
            Formula lowerCaseVariables = new Formula("x2 + y2");
            Formula upperCaseVariables = new Formula("X2 + Y2");

            Assert.IsFalse(lowerCaseVariables.Equals(upperCaseVariables));
        }

        ////GET_HASH_CODE METHOD TESTS

        /// <summary>
        /// Compares two hash codes from the same formulas as each other
        /// </summary>
        [TestMethod]
        public void CompareHashCodesBetweenTwoOfTheExactSameFormulas()
        {
            Formula simpleSumFormula = new Formula("2 + 2");
            Formula theSameSimpleSumFormula = new Formula("2 + 2");

            Assert.IsTrue(simpleSumFormula.GetHashCode() == theSameSimpleSumFormula.GetHashCode());
        }

        /// <summary>
        /// Compare two formulas that are the same, but one of them has whitespaces
        /// and the other does not have whitespaces.  
        /// </summary>
        [TestMethod]
        public void CompareHashCodesBetweenTwoOfTheExactSameFormulasWithWhiteSpaces()
        {
            Formula simpleProductFormulaWhiteSpaces = new Formula("100 * 100");
            Formula simpleProductFormulaNoWhiteSpaces = new Formula("100*100");

            Assert.IsTrue(simpleProductFormulaWhiteSpaces.GetHashCode() == simpleProductFormulaNoWhiteSpaces.GetHashCode());
        }

        /// <summary>
        /// Compares the hash codes between a simple formula and a complicated formula; 
        /// since the complicated formula has more characters than the simple formula, the hash 
        /// codes should be different from each other
        /// </summary>
        [TestMethod]
        public void CompareHashCodesBetweenSimpleFormulaAndComplicatedFormula()
        {
            Formula simpleFormula = new Formula("300 / 2");
            Formula complicatedFormula = new Formula("(((((3000)))) * ((12 + 3.14) + (45 / 9.233491)) - 100 / 2)");

            Assert.IsFalse(simpleFormula.GetHashCode() == complicatedFormula.GetHashCode());
        }

        /// <summary>
        /// Compares the hash codes between two formulas with the same variables but have
        /// different "cases" (that is, one is upper case and the other is lower case)
        /// </summary>
        [TestMethod]
        public void CompareHashCodesBetweenLowerCaseAndUpperCaseVariablesInFormulas()
        {
            Formula lowerCaseVariables = new Formula("A1 + B1 - C1 / E4");
            Formula upperCaseVariables = new Formula("a1 + b1 - c1 / e4");

            Assert.IsTrue(lowerCaseVariables.GetHashCode() != upperCaseVariables.GetHashCode());
        }

        //"==" OPERTAOR OVERRIDE METHOD TESTS

        /// <summary>
        /// Should evaluate to false since 2 != 5
        /// </summary>
        [TestMethod]
        public void DoubleEqualsOperatorTwoFormulasWithDifferentOperands()
        {
            Formula justTheNumber2 = new Formula("2");
            Formula justTheNumber5 = new Formula("5");

            Assert.IsFalse(justTheNumber2 == justTheNumber5);
        }

        /// <summary>
        /// Should result in false since the two formulas have different operators even 
        /// though they have the exact same operands
        /// </summary>
        [TestMethod]
        public void DoubleEqualsOperatorForTwoFormulasWithDifferentOperatorsAndSameOperands()
        {
            Formula additionFormula = new Formula("100 - 97");
            Formula subtractionFormula = new Formula("100 + 97");

            Assert.IsFalse(additionFormula == subtractionFormula);
        }

        /// <summary>
        /// Should result in true since the two formulas are indeed equal
        /// </summary>
        [TestMethod]
        public void DoubleEqualsOperatorForTwoFormulasThatAreTheSameAsEachOther()
        {
            Formula aBitComplicatedFormula = new Formula("(2 + (9)) * (44.62 / 53 + 222.11)");
            Formula theSameFormulaAsAbove = new Formula("(2 + (9)) * (44.62 / 53 + 222.11)");

            Assert.IsTrue(aBitComplicatedFormula == theSameFormulaAsAbove);
        }

        /// <summary>
        /// Should result in false since the two formulas have different cases in the variable names
        /// </summary>
        [TestMethod]
        public void DoubleEqualsOperatorWithFormulasHavingDifferentCaseVariables()
        {
            Formula lowerCaseVariables = new Formula("(a0 + b0 - c0) * (d0 / e0)");
            Formula upperCaseVariables = new Formula("(A0 + B0 - C0) * (D0 / E0)");

            Assert.IsFalse(lowerCaseVariables == upperCaseVariables);
        }

        /// <summary>
        /// Should result in true since a whole integer number can also be represented
        /// in decimal form as long as the trailing numbers after the decimal point
        /// are all zeros
        /// </summary>
        [TestMethod]
        public void DoubleEqualsOperatorWithIntegerVersusDecimalPrecisionOfWholeNumber()
        {
            Formula integerRepresentation = new Formula("( (45 + 45) * (55 - 53) / 2 )");
            Formula decimalRepresentation = new Formula("( (45.0 + 45.0) * (55.0 - 53.0) / 2.0 )");

            Assert.IsTrue(integerRepresentation == decimalRepresentation);
        }

        /// <summary>
        /// Both the decimals should technically be the same even though they are not
        /// mathematically equal
        /// </summary>
        [TestMethod]
        public void DoubleEqualsOperatorWith15DecimalPlacesThatAreReallyCloseToEachOther()
        {
            Formula decimalEndIn1 = new Formula("1 + 3.0000000000000001");
            Formula decimalEndIn2 = new Formula("1 + 3.0000000000000002");

            Assert.IsTrue(decimalEndIn1 == decimalEndIn2);
        }

        /// <summary>
        /// Comparing both null formulas using "=="
        /// </summary>
        [TestMethod]
        public void DoubleEqualsOperatorBothNull()
        {
            Formula firstNull = null;
            Formula secondNull = null;

            Assert.IsTrue(firstNull == secondNull);
        }

        /// <summary>
        /// Should return false
        /// </summary>
        [TestMethod]
        public void firstOneNullSecondOneNonNull()
        {
            Formula nullFormula = null;
            Formula nonNullFormula = new Formula("1 + 1");

            Assert.IsFalse(nullFormula == nonNullFormula);
        }

        /// <summary>
        /// Should return false
        /// </summary>
        [TestMethod]
        public void FirstOneNotNullSecondOneNull()
        {
            Formula nonNullFormula = new Formula("8 * 8");
            Formula nullFormula = null;

            Assert.IsFalse(nonNullFormula == nullFormula);
        }

        //"!=" OPERATOR OVERRIDE METHOD TESTS

        /// <summary>
        /// Should be true since 2 != 5
        /// </summary>
        [TestMethod]
        public void NotEqualsOperatorTwoFormulasWithDifferentOperands()
        {
            Formula justTheNumber2 = new Formula("2");
            Formula justTheNumber5 = new Formula("5");

            Assert.IsTrue(justTheNumber2 != justTheNumber5);
        }

        /// <summary>
        /// Should be true since the two formulas have different operators 
        /// </summary>
        [TestMethod]
        public void NotEqualsOperatorForTwoFormulasWithDifferentOperatorsAndSameOperands()
        {
            Formula additionFormula = new Formula("100 - 97");
            Formula subtractionFormula = new Formula("100 + 97");

            Assert.IsTrue(additionFormula != subtractionFormula);
        }

        /// <summary>
        /// Should be false since the two formulas are actually equal to each other
        /// </summary>
        [TestMethod]
        public void NotEqualsOperatorForTwoFormulasThatAreTheSameAsEachOther()
        {
            Formula aBitComplicatedFormula = new Formula("(2 + (9)) * (44.62 / 53 + 222.11)");
            Formula theSameFormulaAsAbove = new Formula("(2 + (9)) * (44.62 / 53 + 222.11)");

            Assert.IsFalse(aBitComplicatedFormula != theSameFormulaAsAbove);
        }

        /// <summary>
        /// Should be true since the two formulas don't match in terms of capital/lowercase letters
        /// in the variable names
        /// </summary>
        [TestMethod]
        public void NotEqualsOperatorWithFormulasHavingDifferentCaseVariables()
        {
            Formula lowerCaseVariables = new Formula("(a0 + b0 - c0) * (d0 / e0)");
            Formula upperCaseVariables = new Formula("(A0 + B0 - C0) * (D0 / E0)");

            Assert.IsTrue(lowerCaseVariables != upperCaseVariables);
        }


        /// <summary>
        /// Should result in false since a whole integer number can also be represented
        /// in decimal form as long as the trailing numbers after the decimal point
        /// are all zeros
        /// </summary>
        [TestMethod]
        public void NotEqualsOperatorWithIntegerVersusDecimalPrecisionOfWholeNumber()
        {
            Formula integerRepresentation = new Formula("( (45 + 45) * (55 - 53) / 2 )");
            Formula decimalRepresentation = new Formula("( (45.0 + 45.0) * (55.0 - 53.0) / 2.0 )");

            Assert.IsFalse(integerRepresentation != decimalRepresentation);
        }

        //GET_VARIABLES 

        /// <summary>
        /// Should have an empty set of variables since the formula contains no variables
        /// </summary>
        [TestMethod]
        public void GetVariablesFromSimpleFormulaWithNoVariables()
        {
            Formula noVariables = new Formula("2 + 3 * 9 - 9999");

            Assert.IsTrue(new HashSet<String>().SetEquals(noVariables.GetVariables()));
        }

        /// <summary>
        /// Resulting hashset should return the one variable "XYZ2" in formula
        /// </summary>
        [TestMethod]
        public void GetVariablesFromSimpleFormulaWithOneVariable()
        {
            Formula oneVariable = new Formula("XYZ2 + 88");

            HashSet<String> actual = new HashSet<string>(oneVariable.GetVariables());

            Assert.IsTrue(new HashSet<String> { "XYZ2" }.SetEquals(actual));
        }

        [TestMethod]
        public void GetVariablesWithUnderscoreFromSimpleFormula()
        {
            Formula variablesWithUnderscores = new Formula("X_ + X_1 - ABCDE_");
            HashSet<String> expected = new HashSet<String>() {"X_", "X_1", "ABCDE_"};
            HashSet<String> actual = new HashSet<String>(variablesWithUnderscores.GetVariables());
            Assert.IsTrue(expected.SetEquals(actual));
        }

        /// <summary>
        /// Resulting hashset should return the one variable "ABC1" in formula
        /// </summary>
        [TestMethod]
        public void GetVariablesFromComplicatedFormulaWithOneVariable()
        {
            Formula complicatedFormulaOneVar = new Formula("((3.4 * 4.111111)) / (33.121) + (((3 - 2222)) + ABC1)");

            Assert.IsTrue(new HashSet<String> { "ABC1" }.SetEquals(complicatedFormulaOneVar.GetVariables()));
        }

        /// <summary>
        /// Should return the two variables contained in the simple formula: "TONY123" and "TONY321"
        /// </summary>
        [TestMethod]
        public void GetVariablesFromSimpleFormulaWithTwoVariables()
        {
            Formula twoVariables = new Formula("TONY123 * TONY321");

            Assert.IsTrue(new HashSet<String> { "TONY123", "TONY321" }.SetEquals(twoVariables.GetVariables()));
        }

        /// <summary>
        /// Should return the two variables in the complicated formula
        /// </summary>
        [TestMethod]
        public void GetVariablesFromComplicatedFormulaWithTwoVariables()
        {
            Formula twoVariables = new Formula("(AB2 * 77 + ( 7.5555 / 2.3333) - 5 + (7 / 333 / (C2)))");

            HashSet<String> expected = new HashSet<String> { "AB2", "C2" };
            HashSet<String> actual = new HashSet<String>(twoVariables.GetVariables());

            Assert.IsTrue(expected.SetEquals(actual));
        }

        /// <summary>
        /// Should return all 5 variables in the formula
        /// </summary>
        [TestMethod]
        public void GetVariablesFromSimpleFormulaWithMultipleVariables()
        {
            Formula manyVariables = new Formula("A1 - B2 - C3 - D4 - E5");

            Assert.IsTrue(new HashSet<String> { "A1", "B2", "C3", "D4", "E5" }.SetEquals(manyVariables.GetVariables()));
        }

        /// <summary>
        /// Should return all of the variables in the formula; should uppercase any lowercase
        /// letters contained in any of the variables
        /// </summary>
        [TestMethod]
        public void GetVariablesFromComplicatedFormulaWithMultipleVariablesNoNormalizer()
        {
            Formula complicatedFormula = new Formula("(66.6 * (TONY2 / TONY3) + (555555 / 2.323243) + ((65.123 - (( i3 * ((CS3500)) )) - aBcD123 - CS2420)))");

            List<String> expected = new List<String> { "TONY2", "TONY3", "i3", "CS3500", "aBcD123", "CS2420" };
            List<String> actual = new List<String>(complicatedFormula.GetVariables());

            Assert.IsTrue(complicatedFormula.ListsAreTheSame(expected, actual));
        }

        /// <summary>
        /// Should uppercase all of the variables then return them in a hashset
        /// </summary>
        [TestMethod]
        public void GetVariablesFromSimpleFormulaWithLowercaseVariables()
        {
            Formula simpleFormula = new Formula("a22 + bbb222 - abcdefgh69 / djflajdfk233", input => input.ToUpper(), valid => true);

            List<String> expected = new List<String> { "A22", "BBB222", "ABCDEFGH69", "DJFLAJDFK233" };

            Assert.IsTrue(simpleFormula.ListsAreTheSame(expected, new List<String>(simpleFormula.GetVariables())));
        }

        /// <summary>
        /// Should uppercase all letters in the variables and return them in a hashset
        /// </summary>
        [TestMethod]
        public void GetVariablesFromComplicatedFormulaWithUpperCaseVariables()
        {
            Formula complicatedFormula = new Formula("(66.6 * (tony2 / tony3) + (555555 / 2.323243) + ((65.123 - (( i3 * ((cs3500)) )) - abcd123 - cs2420)))");

            List<String> expected = new List<String> { "tony2", "tony3", "i3", "cs3500", "abcd123", "cs2420" };
            List<String> actual = new List<String>(complicatedFormula.GetVariables());

            Assert.IsTrue(complicatedFormula.ListsAreTheSame(expected, actual));
        }

        /// <summary>
        /// Should uppercase all of the funky cased variables and return them in a hashset
        /// </summary>
        [TestMethod]
        public void GetVariablesFromFormulaWithFunkyVariableCasesAndUpperCaseThem()
        {
            Formula funkyVariables = new Formula("fUnKy11 * (whOO2 + ohMAIgawD4) - HeLLOTHERE9 / 76", input => input.ToUpper(), valid => true);

            HashSet<String> actual = new HashSet<string>(funkyVariables.GetVariables());

            Assert.IsTrue(new HashSet<String> { "FUNKY11", "WHOO2", "OHMAIGAWD4", "HELLOTHERE9" }.SetEquals(actual));
        }

        /// <summary>
        /// Should lowercase all of the funky cased variables and return them in a hashset
        /// </summary>
        [TestMethod]
        public void GetVariablesFromFormulaWithFunkyVariablesCasesAndLowerCaseThem()
        {
            Formula funkyVariables = new Formula("fUnKy11 * (whOO2 + ohMAIgawD4) - HeLLOTHERE9 / 76", input => input.ToLower(), valid => true);

            HashSet<String> actual = new HashSet<string>(funkyVariables.GetVariables());

            Assert.IsTrue(new HashSet<String> { "funky11", "whoo2", "ohmaigawd4", "hellothere9" }.SetEquals(actual));
        }

        /// <summary>
        /// Should return all of the variables that appear once and multiple times 
        /// </summary>
        [TestMethod]
        public void GetVariablesFromFormulaWithDuplicateVariables()
        {
            Formula duplicateVars = new Formula("a1 + a1 - a1 * a1 + b2 + b2 + c3");
            List<String> expected = new List<String> { "a1", "a1", "a1", "a1", "b2", "b2", "c3" };
            List<String> actual = new List<String>(duplicateVars.GetVariables());

            Assert.IsTrue(duplicateVars.ListsAreTheSame(expected, actual));
        }

        ////EVALUATE METHOD TESTS

        /// <summary>
        /// Should evaluate to 4
        /// </summary>
        [TestMethod]
        public void EvaluateSimpleIntegerSumFormula()
        {
            Formula simpleSumFormula = new Formula("2 + 2");

            Assert.AreEqual(4, (double)simpleSumFormula.Evaluate(noVariables => 0.0));
        }

        /// <summary>
        /// Should also evaluate to 4 (4 is the same thing as 4.0, so both assertions should 
        /// be true)
        /// </summary>
        [TestMethod]
        public void EvaluateDecimalSimpleFormula()
        {
            Formula simpleDecimalFormula = new Formula("2.0 + 2.0");

            Assert.AreEqual(4, (double)simpleDecimalFormula.Evaluate(noVariable => 0.0));
            Assert.AreEqual(4.0, (double)simpleDecimalFormula.Evaluate(noVariable => 0.0));
        }

        /// <summary>
        /// Should give 0
        /// </summary>
        [TestMethod]
        public void EvaluateSameVariableButWithDifferentOperators()
        {
            Formula variable = new Formula("A1-A1*A1/A1");
            Assert.AreEqual(0, (double)variable.Evaluate(input => 2));
        }

        /// <summary>
        /// 10 - 19 should give us -9 when evaluating the formula
        /// </summary>
        [TestMethod]
        public void EvalauteSimpleIntegerSubtractionFormula()
        {
            Formula simpleIntegerSubFormula = new Formula("10 - 19");

            Assert.AreEqual(-9.0, (double)simpleIntegerSubFormula.Evaluate(noVariable => 0.0));
        }

        /// <summary>
        /// Should evaluate to -9.0 or -9 
        /// </summary>
        [TestMethod]
        public void EvaluateSimpleDecimalSubtractionFormula()
        {
            Formula simpleSubDecimalFormla = new Formula("10.5 - 19.5");

            Assert.AreEqual(-9.0, (double)simpleSubDecimalFormla.Evaluate(noVariable => 0.0));
        }

        /// <summary>
        /// Should compute the product correctly
        /// </summary>
        [TestMethod]
        public void EvaluateSimpleIntegerProductFormula()
        {
            Formula simpleProductIntFormula = new Formula("9 * 7");

            Assert.AreEqual(63, (double)simpleProductIntFormula.Evaluate(noVariable => 0));
        }

        /// <summary>
        /// Should compute the product of two decmals correctly
        /// </summary>
        [TestMethod]
        public void EvaluateSimpleDecimalProductFormula()
        {
            Formula simpleProductDecimalFormula = new Formula("4.0 * 9.5");

            Assert.AreEqual(38.0, (double)simpleProductDecimalFormula.Evaluate(noVariable => 0));
        }

        /// <summary>
        /// Should compute the integer quotient correctly
        /// </summary>
        [TestMethod]
        public void EvaluateSimpleIntQuotientFormula()
        {
            Formula simpleQuotientIntFormula = new Formula("999 / 333");

            Assert.AreEqual(3, (double)simpleQuotientIntFormula.Evaluate(noVariable => 0));
        }

        /// <summary>
        /// Should compute the decimal quotient correctly
        /// </summary>
        [TestMethod]
        public void EvaluateSimpleDecimalQuotientFormula()
        {
            Formula simpleDecimalQuotientFormula = new Formula("4.221 / 4.221");

            Assert.AreEqual(1.0, (double)simpleDecimalQuotientFormula.Evaluate(noVariable => 0));
        }

        /// <summary>
        /// Should lookup the variable and evaluate its value used to compute the whole
        /// formula
        /// </summary>
        [TestMethod]
        public void EvaluateSimpleExpressionWithOneVariableContainingIntegerValue()
        {
            Formula formulaOneVariable = new Formula("100 * A1");

            Assert.AreEqual(1200, (double)formulaOneVariable.Evaluate(A1 => 12));
        }

        /// <summary>
        /// Should lookup the variable and evaluate the decimal value 
        /// </summary>
        [TestMethod]
        public void EvaluateSimpleExpressionWithOneVariableContainingDecimalValue()
        {
            Formula formulaOneVariable = new Formula("100 * B1");

            Assert.AreEqual(250, (double)formulaOneVariable.Evaluate(B1 => 2.5));
        }

        /// <summary>
        /// Given an unknown variable, should return a FormulaError object
        /// </summary>
        [TestMethod]
        public void FormulaErrorUnknownVariable()
        {
            Formula simpleFormula = new Formula("1 + X_1");
            Assert.IsInstanceOfType(simpleFormula.Evaluate(input => { throw new ArgumentException("Unknown variable"); }), typeof(FormulaError));
        }

        /// <summary>
        /// When dividing by zero, the Formula should return a FormulaError object
        /// </summary>
        [TestMethod]
        public void FormulaErrorDivisionByZero()
        {
            Formula uhohDivisionByZero = new Formula("5/0");
            Assert.IsInstanceOfType(uhohDivisionByZero.Evaluate(input => 0), typeof(FormulaError));
        }

        //~INVALID EXPRESSIONS TESTS~//

        /// <summary>
        /// No opearator between two operands
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void SpaceBetweenTwoOperands()
        {
            Formula space = new Formula("A1 A1");
            List<String> expected = new List<String> { "A1", "A1" };
            List<String> actual = new List<String>(space.GetVariables());
        }

        /// <summary>
        /// Should throw an exception; 2X is not a valid variable name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FormulaTest()
        {
            Formula hmmm = new Formula("2X + 1", s => s + "" + s + "_", s => true);
            HashSet<String> variables = new HashSet<string> { "X1_" };
            Assert.IsTrue(variables.SetEquals(hmmm.GetVariables()));
        }

        /// <summary>
        /// Should throw an exception since there's no arithmetic operator between
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FormulaWithOperatorFollowedByParentheses()
        {
            Formula operatorThenParentheses = new Formula("5+7+(5)8");
        }

        /// <summary>
        /// Ampersand operator is an illegal operator and therefore the Formula should
        /// throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FormulaConstructorInvalidOperatorInFormula()
        {
            Formula invalidOpeartorInFormula = new Formula("6 + && * (A1 - 222)");
        }

        /// <summary>
        /// Blank formula; should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void BlankStringFormula()
        {
            Formula uhOhBlankFormula = new Formula("", input => input.ToUpper(), input => false);
        }

        /// <summary>
        /// Trying to divide zero... should throw an exception!
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void DivisionByZero()
        {
            Formula divideByZero = new Formula("5 / 0", input => input.ToUpper(), input => false);
        }

        /// <summary>
        /// Same as above test case, but this time it's a variable that holds the value of 0
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void DivisionByAVariableThatHasAValueOfZero()
        {
            Formula divideByVariableThatHasZero = new Formula("100000 / XYZ1", input => input.ToUpper(), input => false);
        }

        /// <summary>
        /// When evaluating a variable in formula, and it does not have a value, the Formula
        /// should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void VariableDoesNotExist()
        {
            Formula variableThatDoesNotExist = new Formula("1 + (23.1 - ZZZZZ1)", input => input.ToUpper(), input => false);
        }

        /// <summary>
        /// More left parentheses than right parentheses in given formula; should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void MoreLeftParenthesesThanRightParentheses()
        {
            Formula moreLeftParenThanRight = new Formula("((1 * 4 + 2)", input => input.ToUpper(), input => false);
        }

        /// <summary>
        /// Should throw an exception and display the message about unequal parentheses pairs
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void WayMoreLeftParenthesesThanRightParentheses()
        {
            Formula moreLeft = new Formula("((((((((((((222 + 444)");
        }

        /// <summary>
        /// More right parentheses than left parentheses in given formula; should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void MoreRightParenthesesThanLeftParentheses()
        {
            Formula moreRightParenThanLeft = new Formula("(1 * 4 + 2))))))", input => input.ToUpper(), input => false);
        }

        /// <summary>
        /// Variable name starts with a digit, then followed by a letter
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void InvalidVariableNameInFormula()
        {
            Formula badVariableName = new Formula("100 + 5a", input => input.ToUpper(), input => false);
        }

        /// <summary>
        /// Variable name starts with a letter-digit-letter-digit pattern
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void AnotherInvalidVariableNameInFormula()
        {
            Formula badVariableName = new Formula("B2 + B9C8", input => input.ToUpper(), input => false);
        }

        /// <summary>
        /// Five letters followed by 1 digit BUT is now followed by another letter and another digit
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void AlmostValidVariableNameInFormula()
        {
            Formula almostValidVariableName = new Formula("B3 + AAAAA2B9", input => input.ToUpper(), input => false);
        }

        /// <summary>
        /// In the middle of a variable name, there is a digit-letter-digit-letter pattern
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void IncorrectFormatInMiddleOfVariableName()
        {
            Formula variables = new Formula("C3 * ABCDE1X1X1X1X12345", input => input.ToUpper(), input => false);
        }

        /// <summary>
        /// In the middle of a variable, there contains an illegal operator 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void IllegalOperatorContainedInVariableName()
        {
            Formula illegalOperatorInVariable = new Formula("(3 + 9) * ABC$199", input => input.ToUpper(), input => false);
        }

        /// <summary>
        /// Variable only contains a letter and no digit followed by it; it is therefore an invalid
        /// variable name and should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void VariableNameOnlyContainingLetter()
        {
            Formula singleLetterVariableName = new Formula("100 - c");
        }

        /// <summary>
        /// In the middle of a formula, there contains an illegal operator
        /// Specifically, I add a percent sign that operates as a modulus operator, but
        /// the formula should throw an exception since it does not compute modulus
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void IllegalOperatorContainedInFormula()
        {
            Formula illegalOperatorFormula = new Formula("((4 * 7)) % 10000", input => input.ToUpper(), input => false);
        }

        /// <summary>
        /// Formula uses right parentheses first then the left parentheses after
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ImproperSyntaxMisplacedParenthesesFormula()
        {
            Formula misplacedParentheses = new Formula(")))))4 + 5 * 6 / 7(((((", input => input.ToUpper(), input => false);
        }

        /// <summary>
        /// Expression doesn't have at least two operands and one operator in order to be valid 
        /// formula; rather it only contains one operator and one operand
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ImproperSyntaxOneOperandAndOneOperator()
        {
            Formula invalidFormula = new Formula("5 + ", input => input.ToUpper(), input => false);
        }

        /// <summary>
        /// Formula has no operands and only one operator, so it should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void NoOperandsAndOnlyOneOperator()
        {
            Formula noOperandsButOneOperator = new Formula("+", input => input.ToUpper(), input => false);
        }

        /// <summary>
        /// Formula only contains a single operand and no operators; the Formula should throw an 
        /// exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void OnlyOneOperandAndNoOperators()
        {
            Formula onlyOneOperand = new Formula("55555", input => input.ToUpper(), input => false);
        }

        /// <summary>
        /// Formula only contains two operands and no operators; the Formula should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TwoOperandsAndNoOperators()
        {
            Formula onlyTwoOperands = new Formula("6666 GT1", input => input.ToUpper(), input => false);
        }

        /// <summary>
        /// Formula only contains multiple operands and no operators; the Formula should throw an 
        /// exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void MultipleOperandsAndNoOperators()
        {
            Formula onlyMultipleOperands = new Formula("5 a1 7 B3 3.14159 XXX1", input => input.ToUpper(), input => false);
        }

        /// <summary>
        /// Formula contains a programming expression that is not a valid formula to evaluate
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void IncrementFormula()
        {
            Formula iPlusPlus = new Formula("i1++", input => input.ToUpper(), input => false);
        }

        /// <summary>
        /// Formula contains a programming expression is not a valid formula to evaluate
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void DecrementFormula()
        {
            Formula iMinusMinus = new Formula("i1--", input => input.ToUpper(), input => false);
        }

        /// <summary>
        /// Formula is using too many operators in between two operands
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TooManyOperatorsBetweenTwoOperands()
        {
            Formula manyOperatorsBetweenOperands = new Formula("4(+-*/)9", input => input.ToUpper(), input => false);
        }

        /// <summary>
        /// Formula only contains a line of strings and no valid variable names, operators, or operands
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ASentenceFormula()
        {
            Formula justASentence = new Formula("all by myself...", input => input.ToUpper(), input => false);
        }

        /// <summary>
        /// Formula contains one of my favorite keyboard/text emoticons 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void EmoticonFormula()
        {
            Formula myFavoriteEmoticon = new Formula("-_________________-", input => input.ToUpper(), input => false);
        }

        /// <summary>
        /// Formula is not written correctly because it's just a bunch of unordered operators and operands
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void RandomlyAssortedOperatorsAndOperands()
        {
            Formula jumbledOperandsAndOperators = new Formula(")4 9+/-34/*43(100+12.12123()(", input => input.ToUpper(), input => false);
        }

        /// <summary>
        /// Expression has a plus sign followed by a number in parentheses, which is technically 
        /// invalid according to PS1 specifications
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void OperatorFollowedByANumberSurroundedByParentheses()
        {
            Formula formula = new Formula("5 * 3 + (123.981)", input => input.ToUpper(), input => false);
        }

        /// <summary>
        /// Plus operator used as an unary operator; should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void PlusOperatorUsedToDenotePositiveNumber()
        {
            Formula plusOperatorFormula = new Formula("+2.120 * (+333)", input => input.ToUpper(), input => false);
        }

        /// <summary>
        /// Minus operator used as an unary operator; should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void MinusOperatorUsedToDenoteNegativeNumber()
        {
            Formula minusOperatorFormula = new Formula("(420 + 3) + (-43)", input => input.ToUpper(), input => false);
        }

        /// <summary>
        /// Two minus operators used to indicate a negated negative number, which cancels to be 
        /// a positive number; should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TwoMinusOperatorsUsedToDenoteNegativeNegativeNumber()
        {
            Formula twoMinusOperators = new Formula("(--4) + 2", input => input.ToUpper(), input => false);
        }


        //From the grading tests

        /// <summary>
        /// Should validate before throw the exception
        /// </summary>
        [TestMethod()]
        public void TestValidatorX1()
        {
            Formula f = new Formula("2+x", s => s, s => (s == "x"));
        }
    }
}
