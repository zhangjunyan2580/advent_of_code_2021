﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AdventOfCode;
//using Position = AdventOfCode.GenericPosition2D<int>;

namespace aoc
{
    public class Day12
    {
        // Today: 

        class Node
        {
            public string name;
            public List<Node> neighs;
            public bool canBeReentred;
            public Node(string s)
            {
                name = s;
                neighs = new List<Node>();
                canBeReentred = (s == s.ToUpper());
            }
        }

        static Dictionary<string, Node> ReadNodes(string file)
        {
            var nodes = new Dictionary<string, Node>();
            static Node AddNode(string s, Dictionary<string, Node> nodes)
            {
                Node n = nodes.ContainsKey(s) ? nodes[s] : new Node(s);
                nodes[s] = n;
                return n;
            }
            var lines = File.ReadAllLines(ReadInput.GetPath(Day, file));
            foreach (var s in lines)
            {
                var v = s.Split('-');
                Node n1 = AddNode(v[0], nodes);
                Node n2 = AddNode(v[1], nodes);
                n1.neighs.Add(n2);
                n2.neighs.Add(n1);
            }
            return nodes;
        }

        static void GoToNeighs(Node n1, Dictionary<string, Node> nodes, 
            Dictionary<string, int> visits, string myPath, ref int nOk)
        {
            if (n1.name == "end")
            {
                nOk += 1;
                //Console.WriteLine(myPath + "," + "end");
            }
            else
            {
                visits[n1.name] = visits.GetValueOrDefault(n1.name, 0) + 1;
                foreach (var n in n1.neighs)
                {
                    if (n.canBeReentred || !visits.ContainsKey(n.name))
                    {
                        var d = new Dictionary<string, int>(visits);
                        GoToNeighs(n, nodes, d, myPath + "," + n1.name, ref nOk);
                    }
                }
            }
        }

        public static Object PartA(string file)
        {
            var nodes = ReadNodes(file);
            var visits = new Dictionary<string, int>();
            var start = nodes["start"];
            //var paths = new List<List<Node>>();
            int n = 0;
            GoToNeighs(start, nodes, visits, "", ref n);
            //Console.WriteLine("A is {0}", a);
            return n;
        }

        static void GoToNeighsB(Node n1, Dictionary<string, Node> nodes,
            Dictionary<string, int> visits, string myPath, ref int nOk, bool canGoTwice)
        {
            if (n1.name == "end")
            {
                nOk += 1;
                //Console.WriteLine(myPath + "," + "end");
            }
            else
            {
                visits[n1.name] = visits.GetValueOrDefault(n1.name, 0) + 1;
                foreach (var n in n1.neighs)
                {
                    bool validPath = n.canBeReentred || !visits.ContainsKey(n.name);
                    if (validPath)
                    {
                        var d = new Dictionary<string, int>(visits);
                        GoToNeighsB(n, nodes, d, myPath + "," + n1.name, ref nOk, canGoTwice);
                    }
                    else if (!n.canBeReentred && canGoTwice && n.name != "start")
                    {
                        var d = new Dictionary<string, int>(visits);
                        GoToNeighsB(n, nodes, d, myPath + "," + n1.name, ref nOk, false);
                    }
                }
            }
        }

        public static Object PartB(string file)
        {
            var nodes = ReadNodes(file);
            var visits = new Dictionary<string, int>();
            var start = nodes["start"];
            //var paths = new List<List<Node>>();
            int n = 0;
            GoToNeighsB(start, nodes, visits, "", ref n, true);
            return n;
        }

        static void Main() => Aoc.Execute(Day, PartA, PartB);
        static string Day { get { return Aoc.Day(MethodBase.GetCurrentMethod()); } }
    }
}
