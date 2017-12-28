// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// Tony Diep, Version 1.2  (updated 9-24-17) 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpreadsheetUtilities
{ 
    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        (The set of things that depend on s)    
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    ///        (The set of things that s depends on) 
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {
        //Counts how many nodes are in this graph
        private int graphSize;

        //Holds all of the dependees
        private Dictionary<string, HashSet<string>> demDependees;

        //Holds all of the dependents
        private Dictionary<string, HashSet<string>> demDependents;

        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            //Create dependees map, dependents map, and a size counter
            demDependees = new Dictionary<string, HashSet<string>>();
            demDependents = new Dictionary<string, HashSet<string>>();
            graphSize = 0;
        }

        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get
            {
                return graphSize;
            }
        }

        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// <paramref name="dependent"/>Used as an indexer to look into the dependent dictionary</param>
        /// </summary>
        public int this[string dependent]
        {
            get
            {
                if (demDependents.ContainsKey(dependent))
                {
                    return demDependents[dependent].Count;
                }

                return 0;
            }
        }

        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string dependent)
        {
            return demDependees.ContainsKey(dependent);
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string dependee)
        {
            return demDependents.ContainsKey(dependee);
        }

        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string dependee)
        {
            if (demDependees.ContainsKey(dependee))
            {
                return new HashSet<string>(demDependees[dependee]);
            }

            return new HashSet<string>();
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string dependee)
        {
            if (demDependents.ContainsKey(dependee))
            {
                return new HashSet<string>(demDependents[dependee]);
            }

            return new HashSet<string>();
        }


        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   t depends on s
        ///
        /// </summary>
        /// <param name="dependee"> s must be evaluated first. T depends on S; T is the dependent</param>
        /// <param name="dependent"> t cannot be evaluated until s is; S is the dependee</param>/// 
        public void AddDependency(string dependee, string dependent)
        {
            //Check into the dependency dictionary to see if the dependent already exists
            if(demDependents.ContainsKey(dependent))
            {
                if(demDependents[dependent].Add(dependee))
                {
                    graphSize++;
                }
            }
            //The dependency-dependee pair doesn't already exist, so add it to dependent dictionary
            else
            {
                demDependents[dependent] = new HashSet<string>();
                demDependents[dependent].Add(dependee);
                graphSize++;
            }
            //Check into the dependee dictionary to see if a dependent already exists
            if (demDependees.ContainsKey(dependee))
            {
                demDependees[dependee].Add(dependent);
            }
            //Add a dependent to the dependee
            else
            {
                demDependees[dependee] = new HashSet<String>();
                demDependees[dependee].Add(dependent);
            }
        }

        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="dependee">The key</param>
        /// <param name="dependent">The collection of values</param>
        public void RemoveDependency(string dependee, string dependent)
        {
            //CASE 1: The "s" which is the dependent
            if (demDependees.ContainsKey(dependee))
            {
                if (demDependees[dependee].Remove(dependent))
                {
                    graphSize--;
                }
            }

            //CASE 2: The "t" which is the dependee
            if (demDependents.ContainsKey(dependent))
            {
                demDependents[dependent].Remove(dependee);
            }

        }

        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string key, IEnumerable<string> newDependents)
        {
            //Retrieve all of the old dependents
            IEnumerable<string> demOldDependents = GetDependents(key);

            //Take out all of the old dependencies in this graph
            foreach (string oldToken in demOldDependents)
            {
                RemoveDependency(key, oldToken);
            }
            //Insert all of the new dependencies 
            foreach (string newToken in newDependents)
            {
                AddDependency(key, newToken);
            }
        }

        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string key, IEnumerable<string> newDependees)
        {
            //Retrieve all of the old dependees
            IEnumerable<string> demOldDependees = GetDependees(key);

            //Take out all old dependees
            foreach (string oldToken in demOldDependees)
            {
                RemoveDependency(oldToken, key);
            }
            //Insert new old dependees
            foreach (string newToken in newDependees)
            {
                AddDependency(newToken, key);
            }
        }
    }
}