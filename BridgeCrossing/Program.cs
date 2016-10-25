using System;
using System.Collections.Generic;
using System.Linq;

namespace BridgeCrossing
{
    class Program
    {
        static List<int> left;
        static List<int> right;
        static List<List<int>> order;
        static Random r;

        static void Main(string[] args)
        {
            Console.Write("Enter number of iterations [defalut: 5000]: ");
            var iterStr = Console.ReadLine();
            int iter;

            if (!string.IsNullOrEmpty(iterStr))
            { 
                if (!Int32.TryParse(iterStr, out iter))
                {
                    Console.WriteLine("NaN");
                    return;
                }
            }
            else
            {
                iter = 5000;
            }

            var winningOrder = new List<List<int>>();
            var currentLowest = 100;
            int i;
            for (i = 0; i < iter; i++)
            {
                Reset();

                //execute 1 round
                while (Go1Trip()) { }

                var time = order.Sum(x => x.Max());
                if (time < currentLowest)
                {
                    currentLowest = time;
                    winningOrder = order;
                    Console.WriteLine($"{i} / CurrentLowest = {currentLowest}");
                }
            }

            Console.WriteLine($"Reached {i} iterations.");
            Console.WriteLine();

            Console.WriteLine("Shortest trip combination:");
            foreach (var trip in winningOrder)
            {
                trip.ForEach(t => Console.Write("{0} ", t));
                Console.WriteLine();
            }

            Console.ReadLine();
        }

        static void Reset()
        {
            left = new List<int> { 1, 2, 5, 10 };
            right = new List<int>();
            order = new List<List<int>>();
            r = new Random();
        }

        static bool Go1Trip()
        {
            //Select first 2 random people to the other side
            left = left.OrderBy(_ => Guid.NewGuid()).ToList();
            if (left.Count == 0) return false;
            var p1 = left.First();
            left.Remove(p1);
            right.Add(p1);
            if (left.Count == 0) return false;
            var p2 = left.First();
            left.Remove(p2);
            right.Add(p2);
            order.Add(new List<int> { p1, p2 });
            if (left.Count == 0) return false;

            //select 1 or 2 random people from go back
            right = right.OrderBy(_ => Guid.NewGuid()).ToList();
            p1 = right.First();
            right.Remove(p1);
            var o = new List<int>();
            o.Add(p1);
            left.Add(p1);
            if (r.Next(2) == 1)
            {
                p2 = right.First();
                right.Remove(p2);
                left.Add(p2);
                o.Add(p2);
            }
            order.Add(o);
            return true;
        }
    }
}
