using ProjectNomad.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.ProjectNomadSharedTests
{
    public class RandomCalculatorTests
    {
        [Fact]
        public void GetRandomInt_should_generate_max_value()
        {
            var min_value = 1;
            var max_value = 10;

            List<int> results = new List<int>();

            for (int i = 0; i < 20; i++)
            {
                var testedValue = RandomCalculator.GetRandomInt(min_value, max_value);
                results.Add(testedValue);
            }

            Assert.True(results.Any(x => x == max_value));
        }

        [Fact]
        public void GetRandomInt_should_not_excide_max_value_and_excide_min_value()
        {
            var min_value = 1;
            var max_value = 10;

            List<int> results = new List<int>();

            for (int i = 0; i < 20; i++)
            {
                var testedValue = RandomCalculator.GetRandomInt(min_value, max_value);
                results.Add(testedValue);
            }

            Assert.False(results.Any(x => x > max_value));
            Assert.False(results.Any(x => x < min_value));

        }
    }
}
