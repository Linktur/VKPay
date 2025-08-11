-- VK Pay Items Seed Data
-- Insert items from the provided table

INSERT INTO VkPaymentNotifications (Id, Type, NotificationType, AppId, UserId, ReceiverId, OrderId, Item, ItemTitle, ItemPhotoUrl, Price, Currency, Status, SubscriptionId, PaymentStatus, Sig, RawRequest, CreatedAt) VALUES
-- This is just a template table, actual items will be handled by the service

-- Create a separate items table if needed
CREATE TABLE IF NOT EXISTS VkItems (
    Id VARCHAR(50) PRIMARY KEY,
    Price DECIMAL(10,2) NOT NULL,
    TitleEn VARCHAR(255) NOT NULL,
    TitleRu VARCHAR(255) NOT NULL,
    DescriptionEn TEXT,
    DescriptionRu TEXT,
    IsActive BOOLEAN DEFAULT TRUE,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Insert VK items from the provided table
INSERT INTO VkItems (Id, Price, TitleEn, TitleRu, DescriptionEn, DescriptionRu) VALUES
('veh_lelantoss', 99, 'A curiosity', 'Диковинка', 'The future', 'Будущее уже здесь – и оно быстрое'),
('veh_lyssa', 49, 'Real rage', 'Настоящая ярость', 'You have the power', 'У тебя в руках власть – дорога будет покорена'),
('veh_prune', 69, 'Unlimited power', 'Бесконечная мощь', 'Every inch', 'Каждый дюйм – угроза. Каждая секунда – победа'),
('veh_reddragon', 99, 'The Red Dragon', 'Красный Дракон', 'Fiery spirit', 'Огненный дух, непобедимый характер'),
('char_tumsah', 49, 'Tumsah', 'Тумсах', 'Sahara', 'Субтак, что рассекает пустыню'),
('char_tralala', 59, 'Tralala', 'Тралала', 'A shark in the pool', 'Акула в бассейнах – хищник на колесах'),
('ads_free', 150, 'Disabling ads', 'Отключить рекламу', 'Disable ads', 'Отключите всплывающую рекламу'),
('veh_europa', 69, 'Darling of the People', 'Любимец народа', 'The People', 'Народный чемпион – стиль, комфорт и скорость'),
('veh_davide', 99, 'The Air', 'Поджигатель воздуха', 'Hot, sharp', 'Горячий, резкий, неудержимый характер'),
('veh_cerberus', 199, 'Prototype', 'Прототип', 'The technology', 'Технологии будущего уже здесь'),
('veh_aeolus', 299, 'The Green', 'Зеленая стрела', 'Eco-friendly', 'Экологичный, но безжалостный хищник'),
('veh_fulmine', 299, 'Unbridled Lightning', 'Необузданная молния', 'Lightning', 'Молния на колесах – вас ждет только скорость'),
('veh_ixchel', 149, 'Winner', 'Победитель', 'Faster thinking', 'Быстрее мысли, резче реакции');
