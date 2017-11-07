﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeerRecommender.Repositories;
using HtmlAgilityPack;

namespace BeerRecommender
{
    class Program
    {
        static void Main(string[] args) {
            using (var context = new AppDbContext())
            {
                PrintBreweriesFromDb(context);
                PrintBeersFromDb(context);
            }
            Console.ReadLine();
        }

        private static void PrintBreweriesFromDb(AppDbContext context)
        {
            foreach (var brewery in context.Breweries.OrderBy(b => b.Name).Distinct())
            {
                Console.WriteLine(brewery);
            }
        }

        private static void PrintBeersFromDb(AppDbContext context)
        {
            foreach (var beer in context.Beers.OrderBy(b => b.Name).Distinct())
            {
                Console.WriteLine(beer);
            }
        }
    }
}
