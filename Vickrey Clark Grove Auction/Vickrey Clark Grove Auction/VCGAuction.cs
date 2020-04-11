using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Vickrey_Clark_Grove_Auction
{
    internal static class VCGAuction
    {
        public static void Main(string[] args)
        {
            var parameters = GetAuctionParameters();
            CalculateAgentCost(parameters);
        }

        private static List< Tuple<int, double>> GetBids(int biddersCount)
        {
            Console.WriteLine("\nEnter bids for each bidder: ");
            var bids = new List< Tuple<int, double>>();
            for (var i = 0; i < biddersCount; i++)
            {
                var success = false;
                while(!success)
                {
                    Console.WriteLine($"A{i}'s bid amount: ");
                    var input = Console.ReadLine()?.Split().Select(x => double.TryParse(x, out var i) ? i : -1).ToList();
                    if (input == null || input.Count != 1)
                        Console.WriteLine("Enter correct number of integers!");
                    else if (Math.Abs(input[0] - (-1)) < 0)
                        Console.WriteLine("Enter valid input!");
                    else
                    {
                        success = true;
                        bids.Add(new Tuple<int, double>(bids.Count, input[0]));
                    }
                }
            }

            return bids;
        }

        private static Tuple<int, int> GetBidderCountAdvertisingCount()
        {
            var success = false;
            var biddersAndSlots = new List<int>();
            while (!success)
            {
                Console.WriteLine(
                    $"Please input the auctions parameters in the form <number of bidders> <number of advertising slots>: ");
                biddersAndSlots = Console.ReadLine()?.Split().Select(x => Int32.TryParse(x, out var i) ? i : -1).ToList();
                if (biddersAndSlots == null || biddersAndSlots.Count != 2)
                    Console.WriteLine("Enter correct number of integers!");
                else if (biddersAndSlots.Contains(-1))
                    Console.WriteLine("Enter valid input!");
                else
                    success = true;
            }

            var bidderCount = biddersAndSlots[0];
            var advertisingCount = biddersAndSlots[1];
            var bidderAndAdvertisingCount = new Tuple<int, int>(bidderCount, advertisingCount);
            return bidderAndAdvertisingCount;
        }

        private static Tuple<List<int>, List<Tuple<int, double>>> GetAuctionParameters()
        {
            var (bidCount, adCount) = GetBidderCountAdvertisingCount();
            var slotClickCount = GetAdClickCount(adCount);
            var bids = GetBids(bidCount);
            var info = new Tuple<List<int>, List<Tuple<int, double>>>(slotClickCount, bids);
            return info;
        }

        private static List<int> GetAdClickCount(int slotCount)
        {
            Console.WriteLine("\nEnter number of clicks for each Ad placement 0 being the top: ");
            var clickCounts = new List<int>();
            for (var i = 0; i < slotCount; i++)
            {
                var success = false;
                while(!success)
                {
                    Console.WriteLine($"Ad Placement {i}: ");
                    var input = Console.ReadLine()?.Split().Select(x => Int32.TryParse(x, out var i) ? i : -1).ToList();
                    if (input == null || input.Count != 1)
                        Console.WriteLine("Enter correct number of integers!");
                    else if (input[0] == -1)
                        Console.WriteLine("Enter valid input!");
                    else
                    {
                        success = true;
                        clickCounts.Add(input[0]);
                    }
                }
            }

            return clickCounts;
        }

        private static void CalculateAgentCost(Tuple<List<int>, List<Tuple<int, double>>> parameters)
        {
            Console.WriteLine();
            var ci = new CultureInfo("en-us");
            var bids = parameters.Item2;
            bids.OrderBy(x => x.Item2);
            bids.Reverse();
            var clicks = parameters.Item1;
            for (var i = 0; i < bids.Count; i++)
            {
                var total = 0.0;
                for (var j = i; j < clicks.Count && j + 1 < bids.Count; j++) 
                {
                    total += bids[j+1].Item2 * (clicks[j] - (j + 1 == clicks.Count? 0 : clicks[j + 1]));
                }
                Console.WriteLine($"A{bids[i].Item1} pays: {total.ToString("C2",ci)}");
            }
        }
    }
}