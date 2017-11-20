using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Project_GrandeTravel.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Project_GrandeTravel.Services
{
    public static class DbInitializer
    {
        public static async void Initialize(GrandeTravelDbContext context,
                                            UserManager<MyUser> userManager,
                                            RoleManager<IdentityRole> roleManager)
        {
            context.Database.EnsureCreated();
            if (context.Roles.Any()) {
                return;
            }

            await roleManager.CreateAsync(new IdentityRole("Administrator"));
            await roleManager.CreateAsync(new IdentityRole("Provider"));
            await roleManager.CreateAsync(new IdentityRole("Customer"));

            string user = "admin";
            string password = "Aa#123456";
            await userManager.CreateAsync(new MyUser { UserName = user, Email = "admin@grandeTravel.com", EmailConfirmed = true }, password);
            await userManager.AddToRoleAsync(await userManager.FindByNameAsync(user), "Administrator");

            user = "prov1";
            await userManager.CreateAsync(new MyUser { UserName = user, Email = "info@prov1Travel.com", EmailConfirmed = true }, password);
            await userManager.AddToRoleAsync(await userManager.FindByNameAsync(user), "Provider");

            user = "prov2";
            await userManager.CreateAsync(new MyUser { UserName = user, Email = "info@prov2Holiday.com", EmailConfirmed = true }, password);
            await userManager.AddToRoleAsync(await userManager.FindByNameAsync(user), "Provider");

            context.TblProviderProfile.AddRange
            (
                new ProviderProfile {
                    //UserId = userManager.FindByNameAsync().
                    CompanyName = "Prov1 Travel",
                    Address = "11 Main St, Sydney, NSW 2000",
                    Phone = "0222235439",
                    Email = "info@prov1Travel.com",
                },
                new ProviderProfile {
                    UserId = userManager.FindByNameAsync("prov2").Id.ToString(),
                    CompanyName = "Prov2 Holiday",
                    Address = "9 Main Rd, Melbourne, VIC 3000",
                    Phone = "0328593831",
                    Email = "info@prov2Holiday.com"
                }
            );
            context.SaveChanges();

            context.TblCategory.AddRange
            (
                new Category
                {
                    Name = "New South Wales",
                    Description = "Beaches, mountains, caves and rivers; this state has it all. From outback New South Wales to the bustling, bright capital, from the north coast to south coast; there is nothing that will disappoint.Hop on a train, go for a drive; no matter how you choose to see it, Australia's most populous state is a long-standing favourite among travellers local and abroad.",
                    ImgPath = "admin\\New South Wales\\Australia_Sydney_photo_picture_Tacking-Point 16_9.jpg"
                },
                new Category
                {
                    Name = "Victoria",
                    Description = "It may be home to the nation's capital of 'cool', but there is more to beautiful Victoria than inner-city cafes and live music. Head to the High Country for wine tasting; get indulgent in Daylesford with a luxury boutique stay; take a rejuvenating dip in the Murray River; or tap into Victoria's history at the Goldfields. Where ever you head, Victoria is a boundless playground to explore.",
                    ImgPath = "admin\\Victoria\\Twelve-Apostles 16_9.jpg"
                },
                new Category
                {
                    Name = "Queensland",
                    Description = "Australia's sunshine state is a perfect holiday destination for any type of traveller of any age. From the Daintree Rainforest to the Great Barrier Reef, from the Gold Coast to the Whitsundays, Queensland will put a big smile on any face. Whether you enjoy swimming, diving, trekking, cruising or exploring natural wonders, this is the place to kick off your shoes, relax and create lifetime memories.",
                    ImgPath = "admin\\Queensland\\greatBerrierReef.jpg"
                },
                new Category
                {
                    Name = "South Australia",
                    Description = "South Australia is blessed with some of the country's greatest natural wonders; from the sunburnt, prehistoric surfaces of Flinders Ranges and the gaping abyss that is salt Lake Eyre, to the wide stretches of uncrowded beaches and the spectacular coastline of the Great Australian Bite. Sunny weather with blue open skies are typical; grab a glass of its top quality red wine and enjoy amazing views with its warm, friendly people.",
                    ImgPath = "admin\\South Australia\\lake-eyre-flying 16_9.jpg"
                },
                new Category
                {
                    Name = "Western Australia",
                    Description = "Looking for a place of inspiring beauty like nowhere else? Then Western Australia is the place to be. Camp under the stars at Cape Range National Park; take an outback tour to the magnificent Bungle Bungle Range; drive long dazzling coastlines; or just chill-out in city hubs for great food, bars and restaurants. These are what extraordinary holidays are made off.",
                    ImgPath = "admin\\Western Australia\\Lancelin-Pinnacles 16_9.jpg"
                },
                new Category
                {
                    Name = "Tasmania",
                    Description = "From the northernmost point to the southernmost point, Australia's most southern state is a treasure trove for travellers. Lush vineyards, mountain hideaways and stunning beaches; \"Tassie\" is a mix of country towns, hip city digs and natural wonder. The island is home to some of the world's most breath-taking national parks and World Heritage Sites, and is an ideal playground for travellers who love the outdoors.",
                    ImgPath = "admin\\Tasmania\\eyeem-74279425.jpg"
                },
                new Category
                {
                    Name = "Northern Territory",
                    Description = "This is the Australian cliche - harsh ochre-red deserts, strange rock formations, dusty plains with kangaroos gazing lazily in the hot sun, and lonely outback pubs. Further north to the coast, brilliant blue open skies seemingly meet the sea; and the rocky gorges of Kakadu and their deliciously inviting watering holes. Cliche it may be, but it's quintessential Australia.",
                    ImgPath = "admin\\Northern Territory\\Uluru_sky_16_9.jpg"
                },
                new Category
                {
                    Name = "Australian Capital Territory",
                    Description = "The action in Australia's political and national capital is centred around Parliament House, reflected spectacularly at night in Lake Burley Griffin. This city is not solely about politics; it is surrounded in national parks that you can hike, bike or ride; and some of the nation's best art exhibitions pass through its galleries. On weekends, poke around its markets and graze some of the area's best produce.",
                    ImgPath = "admin\\Australian Capital Territory\\act_pa_16_9.jpg"
                }
            );
            context.SaveChanges();

            var cats = context.TblCategory.ToDictionary(c => c.Name, c => c.CategoryId);

            context.TblPackage.AddRange
            (
                new Package
                {
                    Name = "Sydney Experience",
                    Location = "Sydney",
                    Description = "2 nights' accommodation in a Darling City View Room at The Darling\r\nOne Sea Life Sydney Aquarium general admission ticket.",
                    Price = 400,
                    IsActive = true,
                    CategoryId = cats["New South Wales"],
                    ImgPath = "prov1\\Sydney Experience\\sydney-opera house.jpg",
                    UserId = userManager.FindByNameAsync("prov1").Id.ToString()
                },
                new Package
                {
                    Name = "2 day Great Ocean Road & Phillip Island Tour",
                    Location = "Great Ocean Road",
                    Description = "Soak up the natural beauty of Australia’s coast as you spend two days visiting the highlights of the Great Ocean Road, Mornington Peninsula, Bellarine Peninsula and Phillip Island. Traveling to some of southern Australia’s most iconic destinations–including the Twelve Apostles, Loch Ard Gorge, the Otway Ranges, the stunning bay views of the Mornington Peninsula and the Penguin Parade on Phillip Island. You will discover Victoria's breathtaking coastline and make wildlife stops en route.",
                    Price = 289,
                    IsActive = true,
                    CategoryId = cats["Victoria"],
                    ImgPath = "prov1\\2 day Great Ocean Road & Phillip Island Tour\\great-2152332_1920.jpg",
                    UserId = userManager.FindByNameAsync("prov1").Id.ToString()
                },
                new Package
                {
                    Name = "Full Day Rottnest Island Ferry & Bike Hire",
                    Location = "Rottnest Island, Perth, WA",
                    Description = "Explore Rottnest Island in the most relaxing manner possible – by bike\r\nHire some optional snorkelling gear and find out why Rottnest Island is such a popular dive site\r\nTour the islands modern and historical features and learn about its fascinating past\r\n Indulge in plenty of free time, doing whatever you’d like",
                    Price = 119,
                    IsActive = true,
                    CategoryId = cats["Western Australia"],
                    ImgPath = "prov1\\Full Day Rottnest Island Ferry & Bike Hire\\quokka.jpg",
                    UserId = userManager.FindByNameAsync("prov1").Id.ToString()
                },
                new Package
                {
                    Name = "Whitehaven Beach, Hill Inlet and Lookout - Full Day",
                    Location = "Whitsunday Island, QLD",
                    Description = "Indulge in the ultimate experience of Whitehaven Beach with this full day cruise; from the northern end exploring spectacular Hill Inlet, to the southern, where you can swim, sunbathe and soak up the sun.",
                    Price = 200,
                    IsActive = true,
                    CategoryId = cats["Queensland"],
                    ImgPath = "prov1\\Whitehaven Beach, Hill Inlet and Lookout - Full Day\\Whitsunday-Islands.jpg",
                    UserId = userManager.FindByNameAsync("prov1").Id.ToString()
                },
                new Package
                {
                    Name = "Wudinna Day Tour - Lake Gairdner",
                    Location = "Lake Gairdner",
                    Description = "Day Drives to the spectacular Gawler Ranges from Wudinna, regional South Australia\r\nTouring from Wudinna, a great South Australian adventure into the outback… Don’t miss it!\r\n\r\n2 hour drive both ways to & from the stunning Lake Gairdner\r\n*Morning tea & lunch included | minimum of 2 people",
                    Price = 280,
                    IsActive = true,
                    CategoryId = cats["South Australia"],
                    ImgPath = "prov2\\Wudinna Day Tour - Lake Gairdner\\lake gairdner 16_9.jpg",
                    UserId = userManager.FindByNameAsync("prov2").Id.ToString()
                },
                new Package
                {
                    Name = "SPIRIT OF KAKADU TOUR",
                    Location = "Kakadu",
                    Description = "Experience the monsoonal rainforest along Jim Jim Creek on a magnificent bushwalk (2km return). With a bit of effort hiking over large rocks and boulders you will find it all worth while when you are enjoying a refreshing swim in the clear, cool waters under Jim Jim Fall’s towering cliffs. A short drive to Twin Falls where a short cruise and a walk along the base of the gorge to arrive at the sandy beach and the magnificent Twin Falls.\r\nNOTE: This tour is only suitable for people with a high level of fitness and who are surefooted, fit and agile. Not suitable for children under 8 or adults with a medical condition.\r\nTHE TOUR HAS A MODERN FLEET OF 4WDS AND MEALS AND REFRESHMENTS ARE PROVIDED THROUGHOUT THE DAY.",
                    Price = 219,
                    IsActive = true,
                    CategoryId = cats["Northern Territory"],
                    ImgPath = "prov2\\SPIRIT OF KAKADU TOUR\\jim-jim-falls_NT_16_9.jpg",
                    UserId = userManager.FindByNameAsync("prov2").Id.ToString()
                }
            );
            context.SaveChanges();
        }
    }
}
