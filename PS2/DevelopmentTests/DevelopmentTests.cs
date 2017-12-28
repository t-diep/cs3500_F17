using SpreadsheetUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

/// <summary>
/// The provided unit tests
/// </summary>
namespace PS2GradingTests
{
    /// <summary>
    ///  This is a test class for DependencyGraphTest
    /// 
    ///  These tests should help guide you on your implementation.  Warning: you can not "test" yourself
    ///  into correctness.  Tests only show incorrectness.  That being said, a large test suite will go a long
    ///  way toward ensuring correctness.
    /// 
    ///  You are strongly encouraged to write additional tests as you think about the required
    ///  functionality of yoru library.
    /// 
    ///</summary>
    [TestClass()]
    public class DependencyGraphTest
    {
        // ************************** TESTS ON EMPTY DGs ************************* //

        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void ZeroSize()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.AreEqual(0, t.Size);
        }

        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void HasNoDependees()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.IsFalse(t.HasDependees("a"));
        }

        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void HasNoDependents()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.IsFalse(t.HasDependents("a"));
        }

        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void EmptyDependees()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.IsFalse(t.GetDependees("a").GetEnumerator().MoveNext());
        }

        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void EmptyDependents()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.IsFalse(t.GetDependents("a").GetEnumerator().MoveNext());
        }

        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void EmptyIndexer()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.AreEqual(0, t["a"]);
        }

        /// <summary>
        ///Removing from an empty DG shouldn't fail
        ///</summary>
        [TestMethod()]
        public void RemoveFromEmpty()
        {
            DependencyGraph t = new DependencyGraph();
            t.RemoveDependency("a", "b");
            Assert.AreEqual(0, t.Size);
        }

        /// <summary>
        ///Adding to an empty DG shouldn't fail
        ///</summary>
        [TestMethod()]
        public void AddToEmpty()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
        }

        /// <summary>
        ///Adding to an empty DG shouldn't fail
        ///</summary>
        [TestMethod()]
        public void AddToEmptyAssertion()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            Assert.AreEqual(1, t.Size);
        }

        /// <summary>
        ///Replace on an empty DG shouldn't fail
        ///</summary>
        [TestMethod()]
        public void ReplaceEmptyDependents()
        {
            DependencyGraph t = new DependencyGraph();
            t.ReplaceDependents("a", new HashSet<string>());
            Assert.AreEqual(0, t.Size);
        }

        /// <summary>
        ///Replace on an empty DG shouldn't fail
        ///</summary>
        [TestMethod()]
        public void ReplaceEmptyDependees()
        {
            DependencyGraph t = new DependencyGraph();
            t.ReplaceDependees("a", new HashSet<string>());
            Assert.AreEqual(0, t.Size);
        }


        /**************************** SIMPLE NON-EMPTY TESTS ****************************/

        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void NonEmptySize()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            Assert.AreEqual(2, t.Size);
        }

        /// <summary>
        ///Slight variant
        ///</summary>
        [TestMethod()]
        public void AddDuplicate()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "b");
            Assert.AreEqual(1, t.Size);
        }

        /// <summary>
        ///Nonempty graph should contain something
        ///</summary>
        [TestMethod()]
        public void NonEmptyTest3()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("d", "c");
            Assert.IsFalse(t.HasDependees("a"));
            Assert.IsTrue(t.HasDependees("b"));
            Assert.IsTrue(t.HasDependents("a"));
            Assert.IsTrue(t.HasDependees("c"));
        }

        /// <summary>
        ///Nonempty graph should contain something
        ///</summary>
        [TestMethod()]
        public void ComplexGraphCount()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("d", "c");
            HashSet<String> aDents = new HashSet<String>(t.GetDependents("a"));
            HashSet<String> bDents = new HashSet<String>(t.GetDependents("b"));
            HashSet<String> cDents = new HashSet<String>(t.GetDependents("c"));
            HashSet<String> dDents = new HashSet<String>(t.GetDependents("d"));
            HashSet<String> eDents = new HashSet<String>(t.GetDependents("e"));
            HashSet<String> aDees = new HashSet<String>(t.GetDependees("a"));
            HashSet<String> bDees = new HashSet<String>(t.GetDependees("b"));
            HashSet<String> cDees = new HashSet<String>(t.GetDependees("c"));
            HashSet<String> dDees = new HashSet<String>(t.GetDependees("d"));
            HashSet<String> eDees = new HashSet<String>(t.GetDependees("e"));
            Assert.IsTrue(aDents.Count == 2 && aDents.Contains("b") && aDents.Contains("c"));
            Assert.IsTrue(bDents.Count == 0);
            Assert.IsTrue(cDents.Count == 0);
            Assert.IsTrue(dDents.Count == 1 && dDents.Contains("c"));
            Assert.IsTrue(eDents.Count == 0);
            Assert.IsTrue(aDees.Count == 0);
            Assert.IsTrue(bDees.Count == 1 && bDees.Contains("a"));
            Assert.IsTrue(cDees.Count == 2 && cDees.Contains("a") && cDees.Contains("d"));
            Assert.IsTrue(dDees.Count == 0);
            Assert.IsTrue(dDees.Count == 0);
        }

        /// <summary>
        ///Nonempty graph should contain something
        ///</summary>
        [TestMethod()]
        public void ComplexGraphIndexer()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("d", "c");
            Assert.AreEqual(0, t["a"]);
            Assert.AreEqual(1, t["b"]);
            Assert.AreEqual(2, t["c"]);
            Assert.AreEqual(0, t["d"]);
            Assert.AreEqual(0, t["e"]);
        }

        /// <summary>
        ///Removing from a DG 
        ///</summary>
        [TestMethod()]
        public void Remove()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("d", "c");
            t.RemoveDependency("a", "b");
            Assert.AreEqual(2, t.Size);
        }

        /// <summary>
        ///Replace on a DG
        ///</summary>
        [TestMethod()]
        public void ReplaceDependents()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("d", "c");
            t.ReplaceDependents("a", new HashSet<string>() { "x", "y", "z" });
            HashSet<String> aPends = new HashSet<string>(t.GetDependents("a"));
            Assert.IsTrue(aPends.SetEquals(new HashSet<string>() { "x", "y", "z" }));
        }

        /// <summary>
        ///Replace on a DG
        ///</summary>
        [TestMethod()]
        public void ReplaceDependees()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("d", "c");
            t.ReplaceDependees("c", new HashSet<string>() { "x", "y", "z" });
            HashSet<String> cDees = new HashSet<string>(t.GetDependees("c"));
            Assert.IsTrue(cDees.SetEquals(new HashSet<string>() { "x", "y", "z" }));
        }

        // ************************** STRESS TESTS ******************************** //
        /// <summary>
        ///Using lots of data
        ///</summary>
        [TestMethod()]
        public void StressTest1()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();

            // A bunch of strings to use
            const int SIZE = 100;
            string[] letters = new string[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                letters[i] = ("" + (char)('a' + i));
            }

            // The correct answers
            HashSet<string>[] dents = new HashSet<string>[SIZE];
            HashSet<string>[] dees = new HashSet<string>[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                dents[i] = new HashSet<string>();
                dees[i] = new HashSet<string>();
            }

            // Add a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j++)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }

            // Remove a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 2; j < SIZE; j += 2)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            // Make sure everything is right
            for (int i = 0; i < SIZE; i++)
            {
                Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(t.GetDependents(letters[i]))));
                Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(t.GetDependees(letters[i]))));
            }
        }



        // ********************************** ANOTHER STESS TEST ******************** //
        /// <summary>
        ///Using lots of data with replacement
        ///</summary>
        [TestMethod()]
        public void StressTest8()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();

            // A bunch of strings to use
            const int SIZE = 100;
            string[] letters = new string[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                letters[i] = ("" + (char)('a' + i));
            }

            // The correct answers
            HashSet<string>[] dents = new HashSet<string>[SIZE];
            HashSet<string>[] dees = new HashSet<string>[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                dents[i] = new HashSet<string>();
                dees[i] = new HashSet<string>();
            }

            // Add a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j++)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }

            // Remove a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 2; j < SIZE; j += 2)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            // Replace a bunch of dependents
            for (int i = 0; i < SIZE; i += 4)
            {
                HashSet<string> newDents = new HashSet<String>();
                for (int j = 0; j < SIZE; j += 7)
                {
                    newDents.Add(letters[j]);
                }
                t.ReplaceDependents(letters[i], newDents);

                foreach (string s in dents[i])
                {
                    dees[s[0] - 'a'].Remove(letters[i]);
                }

                foreach (string s in newDents)
                {
                    dees[s[0] - 'a'].Add(letters[i]);
                }

                dents[i] = newDents;
            }

            // Make sure everything is right
            for (int i = 0; i < SIZE; i++)
            {
                Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(t.GetDependents(letters[i]))));
                Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(t.GetDependees(letters[i]))));
            }
        }

        // ********************************** A THIRD STESS TEST ******************** //
        /// <summary>
        ///Using lots of data with replacement
        ///</summary>
        [TestMethod()]
        public void StressTest15()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();

            // A bunch of strings to use
            const int SIZE = 100;
            string[] letters = new string[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                letters[i] = ("" + (char)('a' + i));
            }

            // The correct answers
            HashSet<string>[] dents = new HashSet<string>[SIZE];
            HashSet<string>[] dees = new HashSet<string>[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                dents[i] = new HashSet<string>();
                dees[i] = new HashSet<string>();
            }

            // Add a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j++)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }

            // Remove a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 2; j < SIZE; j += 2)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            // Replace a bunch of dependees
            for (int i = 0; i < SIZE; i += 4)
            {
                HashSet<string> newDees = new HashSet<String>();
                for (int j = 0; j < SIZE; j += 7)
                {
                    newDees.Add(letters[j]);
                }
                t.ReplaceDependees(letters[i], newDees);

                foreach (string s in dees[i])
                {
                    dents[s[0] - 'a'].Remove(letters[i]);
                }

                foreach (string s in newDees)
                {
                    dents[s[0] - 'a'].Add(letters[i]);
                }

                dees[i] = newDees;
            }

            // Make sure everything is right
            for (int i = 0; i < SIZE; i++)
            {
                Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(t.GetDependents(letters[i]))));
                Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(t.GetDependees(letters[i]))));
            }
        }

        //~MY OWN UNIT TESTS~//

        /// <summary>
        /// Size of this dependency graph should be 3
        /// </summary>
        [TestMethod]
        public void VerifySizeOfCircularDependencyGraph()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            dependencyGraph.AddDependency("tony","tony's sister");
            dependencyGraph.AddDependency("tony's sister","tony's brother");
            dependencyGraph.AddDependency("tony's brother","tony");

            Assert.AreEqual(3, dependencyGraph.Size);
        }

        /// <summary>
        /// Each dependee should only have one dependent
        /// </summary>
        [TestMethod]
        public void GetDependentsOfEachTokenInCircularDependencyGraphOfSize3()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            dependencyGraph.AddDependency("tony", "tony's sister");
            dependencyGraph.AddDependency("tony's sister", "tony's brother");
            dependencyGraph.AddDependency("tony's brother", "tony");

            IEnumerable<String> tonysDependents = dependencyGraph.GetDependents("tony");
            IEnumerable<String> tonysSisterDependents = dependencyGraph.GetDependents("tony's sister");
            IEnumerable<String> tonysBrothersDependents = dependencyGraph.GetDependents("tony's brother");

            Assert.IsTrue(new HashSet<String>{"tony's sister"}.SetEquals(new HashSet<String>(tonysDependents)));
            Assert.IsTrue(new HashSet<String> { "tony's brother" }.SetEquals(new HashSet<String>(tonysSisterDependents)));
            Assert.IsTrue(new HashSet<String> { "tony" }.SetEquals(new HashSet<String>(tonysBrothersDependents)));
        }

        /// <summary>
        /// Each token should only have one dependee 
        /// </summary>
        [TestMethod]
        public void GetDependeesOfEachTokenInCircularDependencyGraphOfSize3()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            dependencyGraph.AddDependency("tony", "tony's sister");
            dependencyGraph.AddDependency("tony's sister", "tony's brother");
            dependencyGraph.AddDependency("tony's brother", "tony");

            Assert.AreEqual(1, dependencyGraph["tony"]);
            Assert.AreEqual(1, dependencyGraph["tony's sister"]);
            Assert.AreEqual(1, dependencyGraph["tony's brother"]);
        }

        /// <summary>
        /// Should have no dependees when there's no dependent anywhere in the graph
        /// </summary>
        [TestMethod]
        public void GetDependeesFromNonExistentTokenDependencyGraphOfSize3()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            dependencyGraph.AddDependency("tony", "tony's sister");
            dependencyGraph.AddDependency("tony's sister", "tony's brother");
            dependencyGraph.AddDependency("tony's brother", "tony");

            Assert.IsTrue(new HashSet<String>().SetEquals(new HashSet<String>(dependencyGraph.GetDependees("TONY"))));
        }

        /// <summary>
        /// Should have no dependents when there's no dependees anywhere in the graph 
        /// </summary>
        [TestMethod]
        public void GetDependentsFromNonExistentTokenDependencyGraphOfSize3()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            dependencyGraph.AddDependency("tony", "tony's sister");
            dependencyGraph.AddDependency("tony's sister", "tony's brother");
            dependencyGraph.AddDependency("tony's brother", "tony");

            Assert.IsTrue(new HashSet<String>().SetEquals(new HashSet<String>(dependencyGraph.GetDependees("TONY"))));
        }

        /// <summary>
        /// Size should remain unchanged 
        /// </summary>
        [TestMethod]
        public void RemoveNonExistentDependencyTokenDependencyGraphOfSize3()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            dependencyGraph.AddDependency("tony", "tony's sister");
            dependencyGraph.AddDependency("tony's sister", "tony's brother");
            dependencyGraph.AddDependency("tony's brother", "tony");
            dependencyGraph.RemoveDependency("notTony", "notTony");

            Assert.AreEqual(3, dependencyGraph.Size);
        }

        /// <summary>
        /// Should correctly replace the dependencies for "tony's brother"
        /// </summary>
        [TestMethod]
        public void ReplaceTheOneDependencyWithThreeDependenciesSize3()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            dependencyGraph.AddDependency("tony", "tony's sister");
            dependencyGraph.AddDependency("tony's sister", "tony's brother");
            dependencyGraph.AddDependency("tony's brother", "tony");
            dependencyGraph.ReplaceDependees("tony's brother", new HashSet<String> {"tonysGirlfriend", "tonysCousin", "tonysMom"});

            Assert.AreEqual(3, dependencyGraph["tony's brother"]);
        }

        /// <summary>
        /// A single dependent will now have multiple dependees
        /// </summary>
        [TestMethod]
        public void OneDependentAndMultipleDependees()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            dependencyGraph.AddDependency("tony", "tony");
            dependencyGraph.ReplaceDependees("tony", new HashSet<String> {"sleep", "food", "working brain"});

            Assert.AreEqual(3, dependencyGraph["tony"]);
        }
        
        /// <summary>
        /// Each dependent will have many dependees
        /// </summary>
        [TestMethod]
        public void StressTestEveryDependentHavingMultipleDependees()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            dependencyGraph.AddDependency("TonyAgain", "noFriendsYet");
            dependencyGraph.AddDependency("Andrew", "noFriendsYet");
            dependencyGraph.AddDependency("Anastasia", "noFriendsYet");
            dependencyGraph.AddDependency("Jack", "noFriendsYet");
            dependencyGraph.AddDependency("EdStephen", "noFriendsYet");
            dependencyGraph.AddDependency("Jared", "noFriendsYet");

            HashSet<String> friends = new HashSet<String>();

            for(int iterator = 1; iterator < 51; iterator++)
            {
                friends.Add("Friend" + iterator);
            }

            IEnumerable<String> dependents = new HashSet<String>{"TonyAgain", "Andrew", "Anastasia", "Jack", "EdStephen", "Jared"};

            foreach(String dependent in dependents)
            {
                dependencyGraph.ReplaceDependees(dependent, friends);
                Assert.AreEqual(50, dependencyGraph["TonyAgain"]);
            }           
        }

        /// <summary>
        /// Should contain lots of dependees after removing and then replacing 
        /// </summary>
        [TestMethod]
        public void RemoveAndReplaceTheOneDependentThatHasManyDependees()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            dependencyGraph.AddDependency("A1", "");

            HashSet<String> newDependees = new HashSet<String>();

            for(int iterator = 2; iterator < 52; iterator++)
            {
                newDependees.Add("A" + iterator);
            }

            dependencyGraph.ReplaceDependees("A1", newDependees);

            Assert.AreEqual(50, dependencyGraph["A1"]);
        }

        /// <summary>
        /// Should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveNullDependency()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            dependencyGraph.RemoveDependency(null, null);
        }

        /// <summary>
        /// Should throw an exception; cannot have null dependent
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveNonNullDependeeAndNullDependent()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            dependencyGraph.RemoveDependency("tony", null);
        }

        /// <summary>
        /// Should throw an exception; cannot have null dependent and null dependee
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddNullDependency()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            dependencyGraph.AddDependency(null, null);
        }

        /// <summary>
        /// Should throw an exception; cannot have null dependent
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddNonNullDependeeAndNullDepedent()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            dependencyGraph.AddDependency("tony", null);
        }


        //From Grading Tests

        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void PS2GradingTestSelfDependency()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "x");
            t.RemoveDependency("x", "x");
            //t.ReplaceDependents("x", new HashSet<string> { "x" });
            Assert.IsFalse(t.HasDependents("x"));
            Assert.IsFalse(t.HasDependees("x"));
            Assert.IsTrue(t.Size == 0);
        }
    }
}
