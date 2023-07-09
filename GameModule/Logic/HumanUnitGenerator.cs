using GameModule.Entities;
using GameModule.Entities.ValueObjects;
using Shared;

namespace GameModule.Logic
{
    internal static class HumanUnitGenerator
    {
        internal static HumanUnit Generate(Tribe tribe)
        {
            return new HumanUnit()
            {
                Name = GetRandomString(),
                Localization = new Localization
                {
                    X = tribe.Localization.X,
                    Y = tribe.Localization.Y
                },
                Tribe = tribe,
                FoodLevelPercentage = RandomCalculator.GetRandomInt(50, 100) 
            };
        }

        static string GetRandomString()
        {
            var names = new List<string>() 
            { 
                "Aka", "Kha", "Buk", "Bor", "Buku", "Brio", "Bro", "Ciech", "Dobro", "Dola", "Droga", "Goj", "Gost", "Mygi", "Jar", "Lub", "Mił", "Mir", "Mysł", "Rad", "Rat", "Siem", "Wit", "Włod", "Woj", "Hug", "Mug", "Los", "Żyr"
            };

            var random = new Random();
            int randomIndex = random.Next(names.Count);
            return names[randomIndex];
        }
    }
}