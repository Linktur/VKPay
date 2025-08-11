using Microsoft.EntityFrameworkCore;
using VkPayTest.Models;

namespace VkPayTest.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();

            // Check if VkItems already exist
            if (await context.VkItems.AnyAsync())
            {
                return; // DB has been seeded
            }

            var items = new VkItem[]
            {
                new VkItem
                {
                    Id = "veh_lelantoss",
                    Price = 99,
                    TitleEn = "A curiosity",
                    TitleRu = "Диковинка",
                    DescriptionEn = "The future",
                    DescriptionRu = "Будущее уже здесь – и оно быстрое",
                    PhotoUrl = "https://lyboe.ru/images/veh_lelantoss.jpg"
                },
                new VkItem
                {
                    Id = "veh_lyssa",
                    Price = 49,
                    TitleEn = "Real rage",
                    TitleRu = "Настоящая ярость",
                    DescriptionEn = "You have the power",
                    DescriptionRu = "У тебя в руках власть – дорога будет покорена",
                    PhotoUrl = "https://lyboe.ru/images/veh_lyssa.jpg"
                },
                new VkItem
                {
                    Id = "veh_prune",
                    Price = 69,
                    TitleEn = "Unlimited power",
                    TitleRu = "Бесконечная мощь",
                    DescriptionEn = "Every inch",
                    DescriptionRu = "Каждый дюйм – угроза. Каждая секунда – победа",
                    PhotoUrl = "https://lyboe.ru/images/veh_prune.jpg"
                },
                new VkItem
                {
                    Id = "veh_reddragon",
                    Price = 99,
                    TitleEn = "The Red Dragon",
                    TitleRu = "Красный Дракон",
                    DescriptionEn = "Fiery spirit",
                    DescriptionRu = "Огненный дух, непобедимый характер",
                    PhotoUrl = "https://lyboe.ru/images/veh_reddragon.jpg"
                },
                new VkItem
                {
                    Id = "char_tumsah",
                    Price = 49,
                    TitleEn = "Tumsah",
                    TitleRu = "Тумсах",
                    DescriptionEn = "Sahara",
                    DescriptionRu = "Субтак, что рассекает пустыню",
                    PhotoUrl = "https://lyboe.ru/images/char_tumsah.jpg"
                },
                new VkItem
                {
                    Id = "char_tralala",
                    Price = 59,
                    TitleEn = "Tralala",
                    TitleRu = "Тралала",
                    DescriptionEn = "A shark in the pool",
                    DescriptionRu = "Акула в бассейнах – хищник на колесах",
                    PhotoUrl = "https://lyboe.ru/images/char_tralala.jpg"
                },
                new VkItem
                {
                    Id = "ads_free",
                    Price = 150,
                    TitleEn = "Disabling ads",
                    TitleRu = "Отключить рекламу",
                    DescriptionEn = "Disable ads",
                    DescriptionRu = "Отключите всплывающую рекламу",
                    PhotoUrl = "https://lyboe.ru/images/ads_free.jpg"
                },
                new VkItem
                {
                    Id = "veh_europa",
                    Price = 69,
                    TitleEn = "Darling of the People",
                    TitleRu = "Любимец народа",
                    DescriptionEn = "The People",
                    DescriptionRu = "Народный чемпион – стиль, комфорт и скорость",
                    PhotoUrl = "https://lyboe.ru/images/veh_europa.jpg"
                },
                new VkItem
                {
                    Id = "veh_davide",
                    Price = 99,
                    TitleEn = "The Air",
                    TitleRu = "Поджигатель воздуха",
                    DescriptionEn = "Hot, sharp",
                    DescriptionRu = "Горячий, резкий, неудержимый характер",
                    PhotoUrl = "https://lyboe.ru/images/veh_davide.jpg"
                },
                new VkItem
                {
                    Id = "veh_cerberus",
                    Price = 199,
                    TitleEn = "Prototype",
                    TitleRu = "Прототип",
                    DescriptionEn = "The technology",
                    DescriptionRu = "Технологии будущего уже здесь",
                    PhotoUrl = "https://lyboe.ru/images/veh_cerberus.jpg"
                },
                new VkItem
                {
                    Id = "veh_aeolus",
                    Price = 299,
                    TitleEn = "The Green",
                    TitleRu = "Зеленая стрела",
                    DescriptionEn = "Eco-friendly",
                    DescriptionRu = "Экологичный, но безжалостный хищник",
                    PhotoUrl = "https://lyboe.ru/images/veh_aeolus.jpg"
                },
                new VkItem
                {
                    Id = "veh_fulmine",
                    Price = 299,
                    TitleEn = "Unbridled Lightning",
                    TitleRu = "Необузданная молния",
                    DescriptionEn = "Lightning",
                    DescriptionRu = "Молния на колесах – вас ждет только скорость",
                    PhotoUrl = "https://lyboe.ru/images/veh_fulmine.jpg"
                },
                new VkItem
                {
                    Id = "veh_ixchel",
                    Price = 149,
                    TitleEn = "Winner",
                    TitleRu = "Победитель",
                    DescriptionEn = "Faster thinking",
                    DescriptionRu = "Быстрее мысли, резче реакции",
                    PhotoUrl = "https://lyboe.ru/images/veh_ixchel.jpg"
                }
            };

            await context.VkItems.AddRangeAsync(items);
            await context.SaveChangesAsync();
        }
    }
}
