using GameModule.Entities;
using GameModule.Entities.ValueObjects;
using ProjectNomad.Shared;

namespace GameModule.Logic.Services
{
    internal static class HumanUnitGenerator
    {
        internal static List<Human> GenerateForNewTribe(Tribe tribe, int humansToGenerate)
        {
            List<Human> result = new List<Human>();

            for (int i = 0; i < humansToGenerate; i++)
            {
                var human = new Human()
                {
                    Name = GetRandomString(result.Select(x => x.Name)),//COŚ SIĘ TU ZPITLOLIŁO PO ZMIANIE, SPR
                    Localization = new Localization
                    {
                        X = tribe.Localization.X,
                        Y = tribe.Localization.Y
                    },
                    Tribe = tribe,
                    FoodLevelPercentage = RandomCalculator.GetRandomInt(50, 100),
                    ThermalLevelPercentage = 50
                };

                result.Add(human);
            }

            return result;
        }

        internal static Human Generate(Tribe tribe, IEnumerable<string> existingNames)
        {
            return new Human()
            {
                Name = GetRandomString(existingNames),
                Localization = new Localization
                {
                    X = tribe.Localization.X,
                    Y = tribe.Localization.Y
                },
                Tribe = tribe,
                FoodLevelPercentage = RandomCalculator.GetRandomInt(50, 100),
                ThermalLevelPercentage = 50
            };
        }

        static string GetRandomString(IEnumerable<string> existingNames)
        {
            var names = new List<string>()
            {
                "Aka", "Kha", "Buk", "Bor", "Buku", "Brio", "Bro", "Ciech", "Dobro", "Dola", "Droga", "Goj", "Gost", "Mygi", "Jar", "Lub", "Mił", "Mir", "Mysł", "Rad", "Rat", "Siem", "Wit", "Włod", "Woj", "Hug", "Mug", "Los", "Żyr"
            };

            var random = new Random();
            string name;

            do
            {
                int randomIndex = random.Next(names.Count);
                name = names[randomIndex];
            } while (existingNames.Contains(name));

            return name;
        }
    }
}