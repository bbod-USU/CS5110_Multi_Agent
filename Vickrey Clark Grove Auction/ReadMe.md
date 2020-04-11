# How To Run
1. Install dotnet3.0 on your machine.
1. Navigate inside the release folder and launch a command/terminal window.  Enter the following command
  ```dotnet '.\Vickery Clark Grove Auction.dll'```
  
# What I have learned
 
  I have a much better understanding of the VCG auction.  Figuring out a concise reusable algorithm to calculate the total price per bidder was very enlightning.

# Interesting Simulations

1. Given parameters in assignment.

```Please input the auctions parameters in the form <number of bidders> <number of advertising slots>: 
5 3

Enter number of clicks for each Ad placement 0 being the top: 
Ad Placement 0: 
500
Ad Placement 1: 
300
Ad Placement 2: 
100

Enter bids for each bidder: 
A0's bid amount: 
.5
A1's bid amount: 
.4
A2's bid amount: 
.3
A3's bid amount: 
.2
A4's bid amount: 
.1

A0 pays: $160.00
A1 pays: $80.00
A2 pays: $20.00
A3 pays: $0.00
A4 pays: $0.00
```

2. Interesting because 2 bidders have the same bid. 

```
Please input the auctions parameters in the form <number of bidders> <number of advertising slots>: 
3 1

Enter number of clicks for each Ad placement 0 being the top: 
Ad Placement 0: 
150

Enter bids for each bidder: 
A0's bid amount: 
200
A1's bid amount: 
1
A2's bid amount: 
200

A2 pays: $30,000.00
A0 pays: $0.00
A1 pays: $0.00
```

3. This one is interesting as the third bidder gets it for free. This could be beneficial if you were bidding as two seperate entities.
You could bid the same price multiple times and get free ad space.

```
Please input the auctions parameters in the form <number of bidders> <number of advertising slots>: 
3 3

Enter number of clicks for each Ad placement 0 being the top: 
Ad Placement 0: 
300
Ad Placement 1: 
200
Ad Placement 2: 
150

Enter bids for each bidder: 
A0's bid amount: 
.5
A1's bid amount: 
.75
A2's bid amount: 
.75

A2 pays: $100.00
A1 pays: $25.00
A0 pays: $0.00

```

4. More advertisement spaces than bidders

```
Please input the auctions parameters in the form <number of bidders> <number of advertising slots>: 
3 4

Enter number of clicks for each Ad placement 0 being the top: 
Ad Placement 0: 
200
Ad Placement 1: 
100
Ad Placement 2: 
50
Ad Placement 3: 
25

Enter bids for each bidder: 
A0's bid amount: 
25
A1's bid amount: 
.5
A2's bid amount: 
.3

A0 pays: $65.00
A1 pays: $15.00
A2 pays: $0.00
```

5. Bidders dont win in order.
```
Please input the auctions parameters in the form <number of bidders> <number of advertising slots>: 
4 4

Enter number of clicks for each Ad placement 0 being the top: 
Ad Placement 0: 
500
Ad Placement 1: 
400
Ad Placement 2: 
200
Ad Placement 3: 
75

Enter bids for each bidder: 
A0's bid amount: 
.75
A1's bid amount: 
.77
A2's bid amount: 
.56
A3's bid amount: 
.80

A3 pays: $297.00
A1 pays: $220.00
A0 pays: $70.00
A2 pays: $0.00
```

# Code
```using System;
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
            var (clicks, bids) = parameters;
            bids.OrderBy(x => x.Item2);
            bids.Reverse();
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
```
