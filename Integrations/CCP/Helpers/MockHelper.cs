using Integrations.CCP.DTO;

namespace Integrations.CCP.Helpers
{
    public static class MockHelper
    {
        public static List<CCPSoftware> GetAllSoftware()
        {
            return new List<CCPSoftware>
            {
                new CCPSoftware
                {
                     Id = 1,
                     Name = "Windows 10",
                     Description = "Microsofts Windows 10 operating system.",
                     Author = "Microsoft",
                     Price = 200
                },
                new CCPSoftware
                {
                     Id = 2,
                     Name = "Windows 11",
                     Description = "Microsofts latest operating system.",
                     Author = "Microsoft",
                     Price = 500
                },
                new CCPSoftware
                {
                     Id = 3,
                     Name = "Office Suite",
                     Description = "Microsofts Office 365 suite collection. Containing everything from Word to Power point",
                     Author = "Microsoft",
                     Price = 1000
                },
                new CCPSoftware
                {
                     Id = 4,
                     Name = "Visual Studio 2022 Pro",
                     Description = "Microsofts latest version of their integrated development environment (IDE).",
                     Author = "Microsoft",
                     Price = 300
                },
                new CCPSoftware
                {
                     Id = 5,
                     Name = "Outlook",
                     Description = "Microsoft Outlook is a personal information manager software system from Microsoft. Primarily popular as an email client for businesses.",
                     Author = "Microsoft",
                     Price = 250
                }
            };
        }

        public static List<CCPPurchasedSoftware> GetListOfPurchasedSoftware()
        {
            var existingSoftware = GetAllSoftware();

            return new List<CCPPurchasedSoftware>()
            {
                new CCPPurchasedSoftware
                {
                     Id = existingSoftware[0].Id,
                     Name = existingSoftware[0].Name,
                     Description = existingSoftware[0].Description,
                     ValidUntil = DateTime.Now.AddYears(1),
                     Keys = GetRandomAmountOfLicenses()
                },
                new CCPPurchasedSoftware
                {
                     Id = existingSoftware[3].Id,
                     Name = existingSoftware[3].Name,
                     Description = existingSoftware[3].Description,
                     ValidUntil = DateTime.Now.AddYears(2),
                     Keys = GetRandomAmountOfLicenses()
                },
                new CCPPurchasedSoftware
                {
                     Id = existingSoftware[4].Id,
                     Name = existingSoftware[4].Name,
                     Description = existingSoftware[4].Description,
                     ValidUntil = DateTime.Now.AddYears(1),
                     Keys = GetRandomAmountOfLicenses()
                }
            };
        }

        private static List<string> GetRandomAmountOfLicenses()
        {
            var list = new List<string>();
            var random = new Random();
            for (int i = 0; i < random.Next(1, 6); i++)
            {
                list.Add(Guid.NewGuid().ToString());
            }
            return list;
        }
    }
}
