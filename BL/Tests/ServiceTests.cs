using System;
using System.Data.Entity.Validation;
using System.Linq;
using BeerRecommender.Repositories;
using BL.Services;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using BeerRecommender.Utils;

namespace BeerRecommender.Tests
{
    [TestFixture]
    public class ServiceTests
    {
        ObjectFactory Factory = new ObjectFactory();

        private readonly Random rand = new Random();
    }
}
