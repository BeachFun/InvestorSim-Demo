using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.Debug;

/// <summary>
/// Содержит шаблоны для начальных данных об игре
/// </summary>
internal class GameTemplates
{
    /// <summary>
    /// Возвращает список с корпорациями согласно шаблону 1.
    /// </summary>
    /// <returns>Список корпораций</returns>
    public static List<Corporation> GetTestCorporations()
    {
        // TODO: Доп привязка. изменить сводку

        Corporation corp1 = new Corporation()
        {
            Name = "Diamond Process",
            Description = "Корпорация, занимающаяся добычей, обработкой и продажей алмазов",
            Country = Country.Russia
        };
        corp1.IssueShares(ShareType.Ordinary, "DMP", 100, Controllers.CurrencyExchange.RUB, 100000000, new DateTime(2000, 1, 1), "Московская биржа");
        corp1.IssueCouponBonds("Diamond Process выпуск 1", "DMPB1", 1000, 800, Controllers.CurrencyExchange.RUB, 10000, 8000,
            (decimal)34.5, PaymentFrequency.Quarterly, new DateTime(2018, 4, 14), new DateTime(2020, 1, 14), "Московская биржа");

        Corporation corp2 = new Corporation()
        {
            Name = "Techno-Advance",
            Description = "Компания, разрабатывающая и производящая современные технологии и устройства",
            Country = Country.Russia
        };
        corp2.IssueShares(ShareType.Ordinary, "TCA", 70, Controllers.CurrencyExchange.RUB, 100000000, new DateTime(2000, 1, 1), "Московская биржа");
        corp2.IssueCouponBonds("Techno-Advance выпуск 7", "TCAB1", 1000, 940, Controllers.CurrencyExchange.USD, 10000, 5000,
            (decimal)34.5, PaymentFrequency.Quarterly, new DateTime(2019, 6, 10), new DateTime(2022, 6, 10), "Санкт-Петербургская биржа");

        Corporation corp3 = new Corporation()
        {
            Name = "Global Investment Group",
            Description = "Инвестиционная компания, специализирующаяся на работе с международными рынками",
            Country = Country.Russia
        };
        corp3.IssueShares(ShareType.Ordinary, "GID", 34, Controllers.CurrencyExchange.RUB, 100000000, new DateTime(2000, 1, 1), "Московская биржа");
        corp3.IssueCouponBonds("Global Investment выпуск 11", "GIDB1", 10000, 9700, Controllers.CurrencyExchange.RUB, 10000, 1000,
            (decimal)34.5, PaymentFrequency.Quarterly, new DateTime(2017, 10, 20), new DateTime(2021, 10, 20), "Московская биржа");


        List<Corporation> list = new List<Corporation>();
        list.Add(corp1);
        list.Add(corp2);
        list.Add(corp3);

        return list;
    }


    public static (List<Corporation>, List<Shares>, List<Bond>) GetCorporationsInfo1()
    {
        // TODO: свойства ActivityArea указаны в качестве затычек, они не имеют весса, переделать
        // TODO: FoundationDate установлены в качестве затычек, переделать

        List<Shares> sharesList = new();
        List<Bond> bondsList = new();


        Corporation corp1 = new Corporation()
        {
            Name = "StellarTech Solutions",
            Description = "Американская компания, специализирующаяся на разработке и производстве технологий связи и обмена данными.",
            Country = Country.USA,
            ActivityArea = "Технологии связи и обмена данными",
            FoundationDate = new DateTime(2005, 4, 15)
        };
        sharesList.Add(corp1.IssueShares(ShareType.Ordinary, "STS", 125, Controllers.CurrencyExchange.USD, 10000000, new DateTime(2005, 4, 15), "Нью-Йоркская фондовая биржа"));

        Corporation corp2 = new Corporation()
        {
            Name = "GreenGen Energy",
            Description = "Норвежская компания, занимающаяся разработкой и внедрением альтернативных источников энергии, включая солнечную и ветровую энергию.",
            Country = Country.Norway,
            ActivityArea = "Альтернативные источники энергии",
            FoundationDate = new DateTime(2010, 7, 7)
        };
        sharesList.Add(corp2.IssueShares(ShareType.Ordinary, "GGE", 73, Controllers.CurrencyExchange.EUR, 5000000, new DateTime(2010, 7, 7), "Лондонская фондовая биржа"));
        /* Выпуск облигаций corp2 */
        {
            bondsList.Add(corp2.IssueCouponBonds("GreenGen Energy выпуск 1", "GGEO1", 100, 96, Controllers.CurrencyExchange.EUR, 1500000, 150000, (decimal)2.5, PaymentFrequency.Quarterly,
                new DateTime(2011, 9, 26), new DateTime(2026, 9, 26), "Лондонская фондовая биржа"));
            bondsList.Add(corp2.IssueCouponBonds("GreenGen Energy выпуск 2", "GGEO2", 100, 92, Controllers.CurrencyExchange.EUR, 500000, 80000, (decimal)3, PaymentFrequency.Quarterly,
                new DateTime(2013, 3, 18), new DateTime(2028, 3, 18), "Лондонская фондовая биржа"));
            bondsList.Add(corp2.IssueCouponBonds("GreenGen Energy выпуск 3", "GGEO3", 80, 84, Controllers.CurrencyExchange.USD, 2500000, 400000, (decimal)2, PaymentFrequency.Monthly,
                new DateTime(2014, 7, 7), new DateTime(2024, 7, 7), "Лондонская фондовая биржа"));
            bondsList.Add(corp2.IssueCouponBonds("GreenGen Energy выпуск 4", "GGEO4", 75, 74, Controllers.CurrencyExchange.USD, 1000000, 150000, (decimal)4.5, PaymentFrequency.BiAnnually,
                new DateTime(2015, 8, 5), new DateTime(2025, 8, 5), "Лондонская фондовая биржа"));
            bondsList.Add(corp2.IssueDiscountBonds("GreenGen Energy выпуск 5", "GGEO5", 150, 130, Controllers.CurrencyExchange.EUR, 1000000, 15000,
                new DateTime(2016, 12, 21), new DateTime(2023, 12, 21), new List<string>() { "Лондонская фондовая биржа" }));
            bondsList.Add(corp2.IssueDiscountBonds("GreenGen Energy выпуск 6", "GGEO6", 50, 42, Controllers.CurrencyExchange.EUR, 1000000, 25000,
                new DateTime(2018, 2, 12), new DateTime(2021, 2, 12), new List<string>() { "Лондонская фондовая биржа" }));
        }

        Corporation corp3 = new Corporation()
        {
            Name = "AquaFresh Foods",
            Description = "Российская компания, производящая пищевые продукты, в том числе рыбные консервы, морепродукты и замороженные продукты.",
            Country = Country.Russia,
            ActivityArea = "Производство пищевых продуктов",
            FoundationDate = new DateTime(1998, 9, 22)
        };
        sharesList.Add(corp3.IssueShares(ShareType.Ordinary, "AFF", 2760, Controllers.CurrencyExchange.RUB, 20000000, new DateTime(1998, 9, 22), "Московская биржа"));

        Corporation corp4 = new Corporation()
        {
            Name = "LuxeFashion Group",
            Description = "Итальянская компания, занимающаяся производством модной одежды, обуви и аксессуаров для женщин.",
            Country = Country.Italy,
            ActivityArea = "Производство одежды, обуви и аксессуаров",
            FoundationDate = new DateTime(2012, 5, 3)
        };
        sharesList.Add(corp4.IssueShares(ShareType.Ordinary, "LFG", 230, Controllers.CurrencyExchange.EUR, 3000000, new DateTime(2012, 5, 3), "Лондонская фондовая биржа"));
        /* Выпуск облигаций corp4 */
        {
            bondsList.Add(corp4.IssueCouponBonds("LuxeFashion Group выпуск 1", "LFGO1", 50, 61, Controllers.CurrencyExchange.EUR, 1000000, 354000, (decimal)6, PaymentFrequency.Quarterly,
                new DateTime(2014, 3, 23), new DateTime(2024, 3, 23), "Лондонская фондовая биржа"));
            bondsList.Add(corp4.IssueCouponBonds("LuxeFashion Group выпуск 2", "LFGO2", 100, 99, Controllers.CurrencyExchange.EUR, 1000000, 12000, (decimal)4.5, PaymentFrequency.Quarterly,
                new DateTime(2015, 7, 16), new DateTime(2025, 7, 16), "Лондонская фондовая биржа"));
            bondsList.Add(corp4.IssueCouponBonds("LuxeFashion Group выпуск 3", "LFGO3", 150, 159, Controllers.CurrencyExchange.EUR, 1000000, 400000, (decimal)4, PaymentFrequency.Quarterly,
                new DateTime(2017, 2, 9), new DateTime(2022, 2, 9), "Лондонская фондовая биржа"));
            bondsList.Add(corp4.IssueCouponBonds("LuxeFashion Group выпуск 4", "LFGO4", 75, 77, Controllers.CurrencyExchange.EUR, 1000000, 259000, (decimal)5, PaymentFrequency.Quarterly,
                new DateTime(2018, 1, 15), new DateTime(2023, 1, 15), "Лондонская фондовая биржа"));
        }

        Corporation corp5 = new Corporation()
        {
            Name = "Sparkling Innovations",
            Description = "Российская компания, специализирующаяся на производстве газированных напитков, соков и других напитков.",
            Country = Country.Russia,
            ActivityArea = "Производство газированных напитков и соков",
            FoundationDate = new DateTime(2008, 11, 12)
        };
        sharesList.Add(corp5.IssueShares(ShareType.Ordinary, "SKI", 1895, Controllers.CurrencyExchange.RUB, 15000000, new DateTime(2008, 11, 12), "Московская биржа"));
        /* Выпуск облигаций corp5 */
        {
            bondsList.Add(corp5.IssueCouponBonds("Sparkling Innovations выпуск 1", "SKIO1", 1000, 974, Controllers.CurrencyExchange.RUB, 1000000, 229000, (decimal)35, PaymentFrequency.Quarterly,
                new DateTime(2011, 12, 05), new DateTime(2021, 12, 5), "Московская биржа"));
            bondsList.Add(corp5.IssueCouponBonds("Sparkling Innovations выпуск 2", "SKIO2", 1000, 940, Controllers.CurrencyExchange.RUB, 1000000, 289000, (decimal)49.5, PaymentFrequency.BiAnnually,
                new DateTime(2013, 06, 25), new DateTime(2023, 6, 25), "Московская биржа"));
            bondsList.Add(corp5.IssueCouponBonds("Sparkling Innovations выпуск 3", "SKIO3", 10000, 10800, Controllers.CurrencyExchange.RUB, 1000000, 159000, (decimal)349.5, PaymentFrequency.Quarterly,
                new DateTime(2015, 02, 18), new DateTime(2025, 2, 18), "Московская биржа"));
            bondsList.Add(corp5.IssueCouponBonds("Sparkling Innovations выпуск 4", "SKIO4", 1000, 1106, Controllers.CurrencyExchange.RUB, 1000000, 252000, (decimal)42.5, PaymentFrequency.Quarterly,
                new DateTime(2018, 01, 28), new DateTime(2028, 1, 28), "Московская биржа"));
        }

        Corporation corp6 = new Corporation()
        {
            Name = "MedicoGen Pharmaceuticals",
            Description = "Российская компания, занимающаяся разработкой и производством фармацевтических препаратов для лечения различных заболеваний.",
            Country = Country.Russia,
            ActivityArea = "Фармацевтическая промышленность",
            FoundationDate = new DateTime(2001, 7, 30)
        };
        sharesList.Add(corp6.IssueShares(ShareType.Ordinary, "MGP", 6190, Controllers.CurrencyExchange.RUB, 8000000, new DateTime(2001, 7, 30), "Московская биржа"));
        /* Выпуск облигаций corp6 */
        {
            bondsList.Add(corp6.IssueCouponBonds("Sparkling Innovations выпуск 1", "MGPO1", 1000, 1010, Controllers.CurrencyExchange.RUB, 1000000, 322000, (decimal)35, PaymentFrequency.Quarterly,
                new DateTime(2017, 05, 03), new DateTime(2020, 5, 3), "Московская биржа"));
            bondsList.Add(corp6.IssueDiscountBonds("Sparkling Innovations выпуск 2", "MGPO2", 1000, 802, Controllers.CurrencyExchange.RUB, 1000000, 190000,
                new DateTime(2019, 03, 07), new DateTime(2022, 3, 7), new List<string>() { "Московская биржа" }));
        }

        Corporation corp7 = new Corporation()
        {
            Name = "EliteTravel Co.",
            Description = "Российская компания, предоставляющая услуги в сфере туризма и путешествий, включая бронирование отелей, авиабилетов и организацию экскурсий.",
            Country = Country.Russia,
            ActivityArea = "Туризм и путешествия",
            FoundationDate = new DateTime(1995, 2, 18)
        };
        sharesList.Add(corp7.IssueShares(ShareType.Ordinary, "ETC", 3470, Controllers.CurrencyExchange.RUB, 25000000, new DateTime(1995, 2, 18), "Московская биржа"));
        /* Выпуск облигаций corp7 */
        {
            bondsList.Add(corp6.IssueCouponBonds("EliteTravel Co. выпуск 1", "ETCO1", 50000, 44070, Controllers.CurrencyExchange.RUB, 1000000, 507000, (decimal)350, PaymentFrequency.Monthly,
                new DateTime(2017, 12, 30), new DateTime(2027, 12, 30), "Московская биржа"));
            bondsList.Add(corp6.IssueCouponBonds("EliteTravel Co. выпуск 2", "ETCO2", 1000, 1010, Controllers.CurrencyExchange.RUB, 1000000, 322000, (decimal)35, PaymentFrequency.Quarterly,
                new DateTime(2019, 06, 22), new DateTime(2022, 06, 22), "Московская биржа"));
        }

        Corporation corp8 = new Corporation()
        {
            Name = "BuildBright Construction",
            Description = "Американская компания, специализирующаяся на строительстве жилых и коммерческих зданий, включая проектирование, строительство и управление проектами.",
            Country = Country.USA,
            ActivityArea = "Строительство жилых и коммерческих зданий",
            FoundationDate = new DateTime(2007, 10, 9)
        };
        sharesList.Add(corp8.IssueShares(ShareType.Ordinary, "BBC", 175, Controllers.CurrencyExchange.USD, 6000000, new DateTime(2007, 10, 9), "Нью-Йоркская фондовая биржа"));
        /* Выпуск облигаций corp8 */
        {
            bondsList.Add(corp8.IssueCouponBonds("BuildBright Construction выпуск 1", "BBCO1", 100, 117, Controllers.CurrencyExchange.USD, 1000000, 283000, (decimal)3.5, PaymentFrequency.Quarterly,
                new DateTime(2012, 04, 13), new DateTime(2032, 04, 13), "Нью-Йоркская фондовая биржа"));
            bondsList.Add(corp8.IssueDiscountBonds("BuildBright Construction выпуск 2", "BBCO2", 80, 65, Controllers.CurrencyExchange.USD, 1300000, 122000,
                new DateTime(2012, 11, 17), new DateTime(2022, 11, 17), new List<string>() { "Нью-Йоркская фондовая биржа" }));
        }

        Corporation corp9 = new Corporation()
        {
            Name = "SeaBreeze Shipping",
            Description = "Американская компания, занимающаяся морскими перевозками грузов и контейнеров, а также предоставляющая услуги складирования и таможенного оформления.",
            Country = Country.USA,
            ActivityArea = "Морские перевозки грузов и контейнеров",
            FoundationDate = new DateTime(1990, 12, 1)
        };
        sharesList.Add(corp9.IssueShares(ShareType.Ordinary, "SBS", 60, Controllers.CurrencyExchange.USD, 12000000, new DateTime(1990, 12, 1), "Нью-Йоркская фондовая биржа"));
        /* Выпуск облигаций corp9 */
        {
            bondsList.Add(corp9.IssueDiscountBonds("SeaBreeze Shipping выпуск 1", "SBSO1", 1000, 923, Controllers.CurrencyExchange.USD, 1000000, 302000,
                new DateTime(2018, 08, 02), new DateTime(2021, 8, 2), new List<string>() { "Нью-Йоркская фондовая биржа" }));
            bondsList.Add(corp9.IssueDiscountBonds("SeaBreeze Shipping выпуск 2", "SBSO2", 1000, 778, Controllers.CurrencyExchange.USD, 1000000, 197000,
                new DateTime(2019, 08, 08), new DateTime(2022, 8, 8), new List<string>() { "Нью-Йоркская фондовая биржа" }));
        }

        Corporation corp10 = new Corporation()
        {
            Name = "SkyVision Aerospace",
            Description = "Российская компания, занимающаяся разработкой и производством космических систем и технологий, включая спутники связи и навигации, а также оборудование для космических исследований.",
            Country = Country.Russia,
            ActivityArea = "Космическая промышленность",
            FoundationDate = new DateTime(2015, 8, 25)
        };
        sharesList.Add(corp10.IssueShares(ShareType.Ordinary, "SVA", 14000, Controllers.CurrencyExchange.RUB, 2000000, new DateTime(2015, 8, 25), "Московская биржа"));
        /* Выпуск облигаций corp10 */
        {
            bondsList.Add(corp10.IssueCouponBonds("SkyVision Aerospace выпуск 1", "SVAO1", 100000, 113000, Controllers.CurrencyExchange.RUB, 1000000, 453000, (decimal)3100, PaymentFrequency.Quarterly,
                new DateTime(2015, 09, 14), new DateTime(2025, 09, 14), "Московская биржа"));
            bondsList.Add(corp10.IssueCouponBonds("SkyVision Aerospace выпуск 2", "SVAO2", 1000, 1080, Controllers.CurrencyExchange.RUB, 100000000, 4780000, (decimal)27, PaymentFrequency.Quarterly,
                new DateTime(2016, 08, 20), new DateTime(2026, 08, 20), "Московская биржа"));
        }


        List<Corporation> corpList = new List<Corporation>() { corp1, corp2, corp3, corp4, corp5, corp6, corp7, corp8, corp9, corp10 };

        return (corpList, sharesList, bondsList);
    }

    public static Dictionary<Corporation, (List<Shares>, List<Bond>)> GetCorporationsInfoPack1()
    {
        // TODO: свойства ActivityArea указаны в качестве затычек, они не имеют весса, переделать
        // TODO: FoundationDate установлены в качестве затычек, переделать
        // TODO: ActivityArea переделать


        var data = new Dictionary<Corporation, (List<Shares>, List<Bond>)>();
        List<Shares> sharesList = new List<Shares>();
        List<Bond> bondsList = new List<Bond>();


        Corporation corp1 = new Corporation()
        {
            Name = "StellarTech Solutions",
            Description = "Американская компания, специализирующаяся на разработке и производстве технологий связи и обмена данными.",
            Country = Country.USA,
            //ActivityArea = ActivityArea.TechnologiesAndCommunication,
            FoundationDate = new DateTime(2005, 4, 15)
        };
        sharesList.Add(corp1.IssueShares(ShareType.Ordinary, "STS", 125, Controllers.CurrencyExchange.USD, 10000000, new DateTime(2005, 4, 15), "Нью-Йоркская фондовая биржа"));
        data.Add(corp1, (sharesList.ToList(), null));
        sharesList.Clear();
        bondsList.Clear();

        Corporation corp2 = new Corporation()
        {
            Name = "GreenGen Energy",
            Description = "Норвежская компания, занимающаяся разработкой и внедрением альтернативных источников энергии, включая солнечную и ветровую энергию.",
            Country = Country.Norway,
            //ActivityArea = ActivityArea.TechnologiesAndCommunication,
            FoundationDate = new DateTime(2010, 7, 7)
        };
        sharesList.Add(corp2.IssueShares(ShareType.Ordinary, "GGE", 73, Controllers.CurrencyExchange.EUR, 5000000, new DateTime(2010, 7, 7), "Лондонская фондовая биржа"));
        /* Выпуск облигаций corp2 */ {
            bondsList.Add(corp2.IssueCouponBonds("GreenGen Energy выпуск 1", "GGEO1", 100, 96, Controllers.CurrencyExchange.EUR, 1500000, 150000, (decimal)2.5, PaymentFrequency.Quarterly,
                new DateTime(2011, 9, 26), new DateTime(2026, 9, 26), "Лондонская фондовая биржа"));
            bondsList.Add(corp2.IssueCouponBonds("GreenGen Energy выпуск 2", "GGEO2", 100, 92, Controllers.CurrencyExchange.EUR, 500000, 80000, (decimal)3, PaymentFrequency.Quarterly,
                new DateTime(2013, 3, 18), new DateTime(2028, 3, 18), "Лондонская фондовая биржа"));
            bondsList.Add(corp2.IssueCouponBonds("GreenGen Energy выпуск 3", "GGEO3", 80, 84, Controllers.CurrencyExchange.USD, 2500000, 400000, (decimal)2, PaymentFrequency.Monthly,
                new DateTime(2014, 7, 7), new DateTime(2024, 7, 7), "Лондонская фондовая биржа"));
            bondsList.Add(corp2.IssueCouponBonds("GreenGen Energy выпуск 4", "GGEO4", 75, 74, Controllers.CurrencyExchange.USD, 1000000, 150000, (decimal)4.5, PaymentFrequency.BiAnnually,
                new DateTime(2015, 8, 5), new DateTime(2025, 8, 5), "Лондонская фондовая биржа"));
            bondsList.Add(corp2.IssueDiscountBonds("GreenGen Energy выпуск 5", "GGEO5", 150, 130, Controllers.CurrencyExchange.EUR, 1000000, 15000,
                new DateTime(2016, 12, 21), new DateTime(2023, 12, 21), new List<string>() { "Лондонская фондовая биржа" }));
            bondsList.Add(corp2.IssueDiscountBonds("GreenGen Energy выпуск 6", "GGEO6", 50, 42, Controllers.CurrencyExchange.EUR, 1000000, 25000,
                new DateTime(2018, 2, 12), new DateTime(2021, 2, 12), new List<string>() { "Лондонская фондовая биржа" }));
        }
        data.Add(corp2, (sharesList.ToList(), bondsList.ToList()));
        sharesList.Clear();
        bondsList.Clear();

        Corporation corp3 = new Corporation()
        {
            Name = "AquaFresh Foods",
            Description = "Российская компания, производящая пищевые продукты, в том числе рыбные консервы, морепродукты и замороженные продукты.",
            Country = Country.Russia,
            //ActivityArea = ActivityArea.FoodProduction,
            FoundationDate = new DateTime(1998, 9, 22)
        };
        sharesList.Add(corp3.IssueShares(ShareType.Ordinary, "AFF", 2760, Controllers.CurrencyExchange.RUB, 20000000, new DateTime(1998, 9, 22), "Московская биржа"));
        data.Add(corp3, (sharesList.ToList(), null));
        sharesList.Clear();
        bondsList.Clear();

        Corporation corp4 = new Corporation()
        {
            Name = "LuxeFashion Group",
            Description = "Итальянская компания, занимающаяся производством модной одежды, обуви и аксессуаров для женщин.",
            Country = Country.Italy,
            //ActivityArea = ActivityArea.TechnologiesAndCommunication,
            FoundationDate = new DateTime(2012, 5, 3)
        };
        sharesList.Add(corp4.IssueShares(ShareType.Ordinary, "LFG", 230, Controllers.CurrencyExchange.EUR, 3000000, new DateTime(2012, 5, 3), "Лондонская фондовая биржа"));
        /* Выпуск облигаций corp4 */ {
            bondsList.Add(corp4.IssueCouponBonds("LuxeFashion Group выпуск 1", "LFGO1", 50, 61, Controllers.CurrencyExchange.EUR, 1000000, 354000, (decimal)6, PaymentFrequency.Quarterly,
                new DateTime(2014, 3, 23), new DateTime(2024, 3, 23), "Лондонская фондовая биржа"));
            bondsList.Add(corp4.IssueCouponBonds("LuxeFashion Group выпуск 2", "LFGO2", 100, 99, Controllers.CurrencyExchange.EUR, 1000000, 12000, (decimal)4.5, PaymentFrequency.Quarterly,
                new DateTime(2015, 7, 16), new DateTime(2025, 7, 16), "Лондонская фондовая биржа"));
            bondsList.Add(corp4.IssueCouponBonds("LuxeFashion Group выпуск 3", "LFGO3", 150, 159, Controllers.CurrencyExchange.EUR, 1000000, 400000, (decimal)4, PaymentFrequency.Quarterly,
                new DateTime(2017, 2, 9), new DateTime(2022, 2, 9), "Лондонская фондовая биржа"));
            bondsList.Add(corp4.IssueCouponBonds("LuxeFashion Group выпуск 4", "LFGO4", 75, 77, Controllers.CurrencyExchange.EUR, 1000000, 259000, (decimal)5, PaymentFrequency.Quarterly,
                new DateTime(2018, 1, 15), new DateTime(2023, 1, 15), "Лондонская фондовая биржа"));
        }
        data.Add(corp4, (sharesList.ToList(), bondsList.ToList()));
        sharesList.Clear();
        bondsList.Clear();

        Corporation corp5 = new Corporation()
        {
            Name = "Sparkling Innovations",
            Description = "Российская компания, специализирующаяся на производстве газированных напитков, соков и других напитков.",
            Country = Country.Russia,
            //ActivityArea = ActivityArea.TechnologiesAndCommunication,
            FoundationDate = new DateTime(2008, 11, 12)
        };
        sharesList.Add(corp5.IssueShares(ShareType.Ordinary, "SKI", 1895, Controllers.CurrencyExchange.RUB, 15000000, new DateTime(2008, 11, 12), "Московская биржа"));
        /* Выпуск облигаций corp5 */ {
            bondsList.Add(corp5.IssueCouponBonds("Sparkling Innovations выпуск 1", "SKIO1", 1000, 974, Controllers.CurrencyExchange.RUB, 1000000, 229000, (decimal)35, PaymentFrequency.Quarterly,
                new DateTime(2011, 12, 05), new DateTime(2021, 12, 5), "Московская биржа"));
            bondsList.Add(corp5.IssueCouponBonds("Sparkling Innovations выпуск 2", "SKIO2", 1000, 940, Controllers.CurrencyExchange.RUB, 1000000, 289000, (decimal)49.5, PaymentFrequency.BiAnnually,
                new DateTime(2013, 06, 25), new DateTime(2023, 6, 25), "Московская биржа"));
            bondsList.Add(corp5.IssueCouponBonds("Sparkling Innovations выпуск 3", "SKIO3", 10000, 10800, Controllers.CurrencyExchange.RUB, 1000000, 159000, (decimal)349.5, PaymentFrequency.Quarterly,
                new DateTime(2015, 02, 18), new DateTime(2025, 2, 18), "Московская биржа"));
            bondsList.Add(corp5.IssueCouponBonds("Sparkling Innovations выпуск 4", "SKIO4", 1000, 1106, Controllers.CurrencyExchange.RUB, 1000000, 252000, (decimal)42.5, PaymentFrequency.Quarterly,
                new DateTime(2018, 01, 28), new DateTime(2028, 1, 28), "Московская биржа"));
        }
        data.Add(corp5, (sharesList.ToList(), bondsList.ToList()));
        sharesList.Clear();
        bondsList.Clear();

        Corporation corp6 = new Corporation()
        {
            Name = "MedicoGen Pharmaceuticals",
            Description = "Российская компания, занимающаяся разработкой и производством фармацевтических препаратов для лечения различных заболеваний.",
            Country = Country.Russia,
            //ActivityArea = ActivityArea.TechnologiesAndCommunication,
            FoundationDate = new DateTime(2001, 7, 30)
        };
        sharesList.Add(corp6.IssueShares(ShareType.Ordinary, "MGP", 6190, Controllers.CurrencyExchange.RUB, 8000000, new DateTime(2001, 7, 30), "Московская биржа"));
        /* Выпуск облигаций corp6 */ {
            bondsList.Add(corp6.IssueCouponBonds("Sparkling Innovations выпуск 1", "MGPO1", 1000, 1010, Controllers.CurrencyExchange.RUB, 1000000, 322000, (decimal)35, PaymentFrequency.Quarterly,
                new DateTime(2017, 05, 03), new DateTime(2020, 5, 3), "Московская биржа"));
            bondsList.Add(corp6.IssueDiscountBonds("Sparkling Innovations выпуск 2", "MGPO2", 1000, 802, Controllers.CurrencyExchange.RUB, 1000000, 190000,
                new DateTime(2019, 03, 07), new DateTime(2022, 3, 7), new List<string>() { "Московская биржа" }));
        }
        data.Add(corp6, (sharesList.ToList(), bondsList.ToList()));
        sharesList.Clear();
        bondsList.Clear();

        Corporation corp7 = new Corporation()
        {
            Name = "EliteTravel Co.",
            Description = "Российская компания, предоставляющая услуги в сфере туризма и путешествий, включая бронирование отелей, авиабилетов и организацию экскурсий.",
            Country = Country.Russia,
            //ActivityArea = ActivityArea.TechnologiesAndCommunication,
            FoundationDate = new DateTime(1995, 2, 18)
        };
        sharesList.Add(corp7.IssueShares(ShareType.Ordinary, "ETC", 3470, Controllers.CurrencyExchange.RUB, 25000000, new DateTime(1995, 2, 18), "Московская биржа"));
        /* Выпуск облигаций corp7 */ {
            bondsList.Add(corp6.IssueCouponBonds("EliteTravel Co. выпуск 1", "ETCO1", 50000, 44070, Controllers.CurrencyExchange.RUB, 1000000, 507000, (decimal)350, PaymentFrequency.Monthly,
                new DateTime(2017, 12, 30), new DateTime(2027, 12, 30), "Московская биржа"));
            bondsList.Add(corp6.IssueCouponBonds("EliteTravel Co. выпуск 2", "ETCO2", 1000, 1010, Controllers.CurrencyExchange.RUB, 1000000, 322000, (decimal)35, PaymentFrequency.Quarterly,
                new DateTime(2019, 06, 22), new DateTime(2022, 06, 22), "Московская биржа"));
        }
        data.Add(corp7, (sharesList.ToList(), bondsList.ToList()));
        sharesList.Clear();
        bondsList.Clear();

        Corporation corp8 = new Corporation()
        {
            Name = "BuildBright Construction",
            Description = "Американская компания, специализирующаяся на строительстве жилых и коммерческих зданий, включая проектирование, строительство и управление проектами.",
            Country = Country.USA,
            //ActivityArea = ActivityArea.TechnologiesAndCommunication,
            FoundationDate = new DateTime(2007, 10, 9)
        };
        sharesList.Add(corp8.IssueShares(ShareType.Ordinary, "BBC", 175, Controllers.CurrencyExchange.USD, 6000000, new DateTime(2007, 10, 9), "Нью-Йоркская фондовая биржа"));
        /* Выпуск облигаций corp8 */ {
            bondsList.Add(corp8.IssueCouponBonds("BuildBright Construction выпуск 1", "BBCO1", 100, 117, Controllers.CurrencyExchange.USD, 1000000, 283000, (decimal)3.5, PaymentFrequency.Quarterly,
                new DateTime(2012, 04, 13), new DateTime(2032, 04, 13), "Нью-Йоркская фондовая биржа"));
            bondsList.Add(corp8.IssueDiscountBonds("BuildBright Construction выпуск 2", "BBCO2", 80, 65, Controllers.CurrencyExchange.USD, 1300000, 122000,
                new DateTime(2012, 11, 17), new DateTime(2022, 11, 17), new List<string>() { "Нью-Йоркская фондовая биржа" }));
        }
        data.Add(corp8, (sharesList.ToList(), bondsList.ToList()));
        sharesList.Clear();
        bondsList.Clear();

        Corporation corp9 = new Corporation()
        {
            Name = "SeaBreeze Shipping",
            Description = "Американская компания, занимающаяся морскими перевозками грузов и контейнеров, а также предоставляющая услуги складирования и таможенного оформления.",
            Country = Country.USA,
            //ActivityArea = ActivityArea.TechnologiesAndCommunication,
            FoundationDate = new DateTime(1990, 12, 1)
        };
        sharesList.Add(corp9.IssueShares(ShareType.Ordinary, "SBS", 60, Controllers.CurrencyExchange.USD, 12000000, new DateTime(1990, 12, 1), "Нью-Йоркская фондовая биржа"));
        /* Выпуск облигаций corp9 */ {
            bondsList.Add(corp9.IssueDiscountBonds("SeaBreeze Shipping выпуск 1", "SBSO1", 1000, 923, Controllers.CurrencyExchange.USD, 1000000, 302000,
                new DateTime(2018, 08, 02), new DateTime(2021, 8, 2), new List<string>() { "Нью-Йоркская фондовая биржа" }));
            bondsList.Add(corp9.IssueDiscountBonds("SeaBreeze Shipping выпуск 2", "SBSO2", 1000, 778, Controllers.CurrencyExchange.USD, 1000000, 197000,
                new DateTime(2019, 08, 08), new DateTime(2022, 8, 8), new List<string>() { "Нью-Йоркская фондовая биржа" }));
        }
        data.Add(corp9, (sharesList.ToList(), bondsList.ToList()));
        sharesList.Clear();
        bondsList.Clear();

        Corporation corp10 = new Corporation()
        {
            Name = "SkyVision Aerospace",
            Description = "Российская компания, занимающаяся разработкой и производством космических систем и технологий, включая спутники связи и навигации, а также оборудование для космических исследований.",
            Country = Country.Russia,
            //ActivityArea = ActivityArea.TechnologiesAndCommunication,
            FoundationDate = new DateTime(2015, 8, 25)
        };
        sharesList.Add(corp10.IssueShares(ShareType.Ordinary, "SVA", 14000, Controllers.CurrencyExchange.RUB, 2000000, new DateTime(2015, 8, 25), "Московская биржа"));
        /* Выпуск облигаций corp10 */ {
            bondsList.Add(corp10.IssueCouponBonds("SkyVision Aerospace выпуск 1", "SVAO1", 100000, 113000, Controllers.CurrencyExchange.RUB, 1000000, 453000, (decimal)3100, PaymentFrequency.Quarterly,
                new DateTime(2015, 09, 14), new DateTime(2025, 09, 14), "Московская биржа"));
            bondsList.Add(corp10.IssueCouponBonds("SkyVision Aerospace выпуск 2", "SVAO2", 1000, 1080, Controllers.CurrencyExchange.RUB, 100000000, 4780000, (decimal)27, PaymentFrequency.Quarterly,
                new DateTime(2016, 08, 20), new DateTime(2026, 08, 20), "Московская биржа"));
        }
        data.Add(corp10, (sharesList.ToList(), bondsList.ToList()));
        sharesList.Clear();
        bondsList.Clear();


        return data;
    }

    public static List<Offer> GetOffers1_FinancialPyramids()
    {
        var investment1 = new Investment((decimal)0.05, PaymentFrequency.Monthly, Controllers.CurrencyExchange.RUB, new TimeSpan(365 * 2, 0, 0, 0));
        var offer1 = new Offer("Вложение в инвестиционную компанию",
            "Наша инвестиционная компания предлагает вам вложить средства с доходностью 5% в месяц.",
            investment1, Controllers.CurrencyExchange.RUB, 5, 5000, 20000, 200);

        var investment2 = new Investment((decimal)0.1, PaymentFrequency.Annually, Controllers.CurrencyExchange.RUB, new TimeSpan(365 * 5, 0, 0, 0));
        var offer2 = new Offer("Вложение в компанию",
            "Инвестируйте в нашу компанию и получайте доходность 10% в год.",
            investment2, Controllers.CurrencyExchange.RUB, 5, new List<decimal>() { 10000, 24000, 40000, 70000, 100000});

        var investment3 = new Investment((decimal)0.03, PaymentFrequency.Weekly, Controllers.CurrencyExchange.RUB, new TimeSpan(365, 0, 0, 0));
        var offer3 = new Offer("Специальное предложение",
            "У нас есть специальное предложение для вас: вложите средства и получайте доходность 3% в неделю.",
            investment3, Controllers.CurrencyExchange.RUB, 1, 1000, null, 100);

        var investment4 = new Investment((decimal)0.15, PaymentFrequency.Annually, Controllers.CurrencyExchange.RUB, new TimeSpan(365 * 3, 0, 0, 0));
        var offer4 = new Offer("Вложение в инвестиционную компанию",
            "Наша инвестиционная компания гарантирует доходность 15% в год при вложении средств на 3 года.",
            investment4, Controllers.CurrencyExchange.RUB, 5, 10000, 100000, 2000);

        var investment5 = new Investment((decimal)0.025, PaymentFrequency.Monthly, Controllers.CurrencyExchange.RUB, new TimeSpan(365 * 2, 0, 0, 0));
        var offer5 = new Offer("Вложение в компанию",
            "Вложите свои сбережения в нашу компанию и получайте ежемесячную доходность 2,5% на протяжении 2 лет.",
            investment5, Controllers.CurrencyExchange.RUB, 5, 3000, null, 500);

        var investment6 = new Investment((decimal)0.003, PaymentFrequency.Daily, Controllers.CurrencyExchange.RUB, new TimeSpan(30 * 9, 0, 0, 0));
        var offer6 = new Offer("Вложение в компанию",
            "Получайте ежедневную доходность 0,3% при инвестировании в нашу компанию на 9 месяцев.",
            investment6, Controllers.CurrencyExchange.RUB, 1, 500, 50000, 50);

        var investment7 = new Investment((decimal)0.3, PaymentFrequency.Once, Controllers.CurrencyExchange.RUB, new TimeSpan(365 * 4, 0, 0, 0));
        var offer7 = new Offer("Вложение в компанию",
            "Наша компания предлагает вложить средства на 4 года с доходностью 30%.",
            investment7, Controllers.CurrencyExchange.RUB, 7, new List<decimal>() { 10000, 30000, 50000, 100000});

        var investment8 = new Investment((decimal)0.04, PaymentFrequency.Monthly, Controllers.CurrencyExchange.RUB, new TimeSpan(365, 0, 0, 0));
        var offer8 = new Offer("Вложение в компанию",
            "Инвестируйте в нашу компанию и получайте ежемесячную доходность 4% на протяжении 1 года.",
            investment8, Controllers.CurrencyExchange.RUB, 3, 3000, null, 250);

        var investment9 = new Investment((decimal)0.18, PaymentFrequency.Annually, Controllers.CurrencyExchange.RUB, new TimeSpan(365 * 7, 0, 0, 0));
        var offer9 = new Offer("Вложение в компанию",
            "Мы предлагаем инвестировать средства на 7 лет с ежегодной доходностью 18%.",
            investment9, Controllers.CurrencyExchange.RUB, 9, new List<decimal>() { 30000, 100000, 250000, 400000, 650000, 800000, 1000000});

        var investment10 = new Investment((decimal)0.08, PaymentFrequency.Once, Controllers.CurrencyExchange.RUB, new TimeSpan(30 * 6, 0, 0, 0));
        var offer10 = new Offer("Вложение в компанию",
            "Наша компания предлагает вложить средства на 6 месяцев с доходностью 8%.",
            investment10, Controllers.CurrencyExchange.RUB, 5, 4000, 200000, 250);

        investment1.SetOffer(offer1.id);
        investment2.SetOffer(offer2.id);
        investment3.SetOffer(offer3.id);
        investment4.SetOffer(offer4.id);
        investment5.SetOffer(offer5.id);
        investment6.SetOffer(offer6.id);
        investment7.SetOffer(offer7.id);
        investment8.SetOffer(offer8.id);
        investment9.SetOffer(offer9.id);
        investment10.SetOffer(offer10.id);

        return new List<Offer>() { offer1, offer2, offer3, offer4, offer5, offer6, offer7, offer8, offer9, offer10 };
    }

    public static List<Offer> GetOffers1_Deptors()
    {
        Random rnd = new();

        var dept1 = new Dept(DeptorType.Familiar, DeptorRateType.Medium, 20000, rnd.NextDouble(), Controllers.CurrencyExchange.RUB, Managers.Game.Date.AddMonths(1));
        var offer1 = new Offer("В долг знакомому",
            "Можешь помочь мне с кредитом на 20 000 рублей? Я верну через месяц с небольшим процентом.",
            dept1, Controllers.CurrencyExchange.RUB, 5);

        var dept2 = new Dept(DeptorType.Familiar, DeptorRateType.Low, 25000, rnd.NextDouble(), Controllers.CurrencyExchange.RUB, Managers.Game.Date.AddMonths(1));
        var offer2 = new Offer("В долг знакомому",
            "Нужна помощь в получении займа на 25 000 рублей, обещаю вернуть через месяц с процентом.",
            dept2, Controllers.CurrencyExchange.RUB, 5);

        var dept3 = new Dept(DeptorType.Familiar, DeptorRateType.Medium, 3000, rnd.NextDouble(), Controllers.CurrencyExchange.RUB, Managers.Game.Date.AddDays(rnd.Next(2,5)));
        var offer3 = new Offer("В долг знакомому",
            "Не мог бы ты одолжить мне 3000 рублей на пару дней? Я верну с процентом.",
            dept3, Controllers.CurrencyExchange.RUB, 1);

        var dept4 = new Dept(DeptorType.Friend, DeptorRateType.High, 35000, null, Controllers.CurrencyExchange.RUB, Managers.Game.Date.AddMonths(1));
        var offer4 = new Offer("В долг другу",
            "Могу ли я попросить у тебя в долг 35 000 рублей? Я верну через месяц.",
            dept4, Controllers.CurrencyExchange.RUB, 5);

        var dept5 = new Dept(DeptorType.CloseFriend, DeptorRateType.Medium, 130000, rnd.NextDouble(), Controllers.CurrencyExchange.RUB, Managers.Game.Date.AddMonths(rnd.Next(1,6)));
        var offer5 = new Offer("В долг близкому другу",
            "Можешь помочь мне с покупкой машины? Мне нужно 130000 рублей. Верну когда смогу",
            dept5, Controllers.CurrencyExchange.RUB, 10);

        var dept6 = new Dept(DeptorType.Stranger, DeptorRateType.Low, 5000, null, Controllers.CurrencyExchange.RUB, Managers.Game.Date.AddDays(rnd.Next(2, 5)));
        var offer6 = new Offer("В долг незнакомцу",
            "Я друг твоего знакомого. Не мог бы ты одолжить мне 5000 рублей на пару дней? Я верну с процентом",
            dept6, Controllers.CurrencyExchange.RUB, 1);

        dept1.SetOffer(offer1.id);
        dept2.SetOffer(offer2.id);
        dept3.SetOffer(offer3.id);
        dept4.SetOffer(offer4.id);
        dept5.SetOffer(offer5.id);
        dept6.SetOffer(offer6.id);

        return new List<Offer>() { offer1, offer2, offer3, offer4, offer5, offer6 };
    }


    /// <summary>
    /// Возвщает список шаблонов новостей для финансовых отчетов. По аргументам фильтруются шаблоны
    /// </summary>
    /// <param name="isPositive">Положительна ли новость?</param>
    /// <param name="power">Сила новости</param>
    /// <param name="corpName"></param>
    /// <returns></returns>
    public static List<string> GetFinResult(bool isPositive, NewsPower power, string corpName) 
    {
        List<string> list = new();

        if (isPositive)
        {
            list.Add(string.Format("Компания {0} увеличила прибыль в прошлом квартале", corpName));
            list.Add(string.Format("Компания {0} заработала больше денег в этом году", corpName));
            list.Add(string.Format("Компания {0} заработала больше, чем ожидалось, в прошлом квартале", corpName));
            list.Add(string.Format("Чистые активы компании {0} выросли в прошлом году", corpName)); // <--
            list.Add(string.Format("Чистая прибыль компании {0} увеличилась в прошлом квартале", corpName));
            list.Add(string.Format("Компания {0} отчиталась о рекордных выручках в прошлом году", corpName)); // <--
            list.Add(string.Format("Компания {0} улучшила свои финансовые показатели в последнем квартале", corpName)); // ?
            list.Add(string.Format("Компания {0} увеличила объем продаж и прибыли в прошлом году", corpName)); // <--
            list.Add(string.Format("Чистые активы компании {0} достигли новых максимумов в последний квартал", corpName)); // ?
            list.Add(string.Format("Акции компании {0} увеличились после рекордной прибыли в прошлом году", corpName)); // <--
            list.Add(string.Format("Компания {0} отчиталась о рекордной чистой прибыли в последнем квартале", corpName)); // ?
            list.Add(string.Format("Компания {0} увеличила свою прибыль за год благодаря росту продаж", corpName)); // <--
            list.Add(string.Format("Чистые активы компании {0} улучшились в последнем квартале", corpName)); // ?
            //list.Add(string.Format("Акции компании {0} выросли после отчета о высоких финансовых показателях за год", corpName));
            //list.Add(string.Format("Компания {0} увеличила свою выручку и прибыль в прошлом году", corpName)); // <--
            list.Add(string.Format("Компания {0} улучшила свои финансовые показатели благодаря оптимизации своих производственных процессов", corpName));
        }
        else
        {
            list.Add(string.Format("Выручка компании {0} снизилась в этом году из-за нестабильности рынка", corpName)); // <--
            //list.Add(string.Format("Акции компании {0} упали после публикации финансового отчета", corpName));
            list.Add(string.Format("Компания {0} заработала меньше денег, чем ожидалось", corpName));
            list.Add(string.Format("Компания {0} снизила свои финансовые показатели по сравнению с прошлым годом", corpName)); // <--
            //list.Add(string.Format("Акции компании {0} снизились на фоне низких доходов в этом году", corpName)); 
            list.Add(string.Format("Компания {0} заработала меньше денег в прошлом квартале", corpName));
            list.Add(string.Format("Чистая прибыль компании {0} уменьшилась в последнем квартале", corpName)); // ?
            //list.Add(string.Format("Акции компании {0} снизились после неудовлетворительных финансовых результатов за год", corpName));
            //list.Add(string.Format("Акции компании {0} упали после неудовлетворительных финансовых результатов за прошлый квартал", corpName));
        }

        return list;
    }

    /// <summary>
    /// Возвщает список шаблонов новостей для стратегических решений. По аргументам фильтруются шаблоны
    /// </summary>
    /// <param name="corpName">Наименование компанм</param>
    /// <returns></returns>
    public static List<string> GetStrategicBec(string corpName)
    {
        List<string> list = new();

        list.Add(string.Format("Компания {0} представила новую продукцию.", corpName));
        list.Add(string.Format("Компания {0} планирует выпустить новый продукт.", corpName));
        list.Add(string.Format("Компания {0} заключила сделку с {1}.", corpName, "новым партнером")); // TODO: доделать
        list.Add(string.Format("Компания {0} расширяет свой бизнес на {1}.", corpName, "новую сферу")); // TODO: доделать
        //list.Add(string.Format("Компания {0} покупает акции [название компании-цели].", corpName)); // TODO: доделать
        list.Add(string.Format("Компания {0} сокращает расходы, увольняя некоторую часть сотрудников.", corpName)); // TODO: доделать
        // list.Add(string.Format("Компания {0} планирует инвестировать [сумма] в [название проекта].", corpName)); // TODO: доделать
        // list.Add(string.Format("Компания {0} выпустила положительный отчет о прибылях.", corpName));
        list.Add(string.Format("Компания {0} открывает новое отделение в {1}.", corpName, "соседнем регионе"));

        return list;
    }

    /// <summary>
    /// Возвращает список советов по игре
    /// </summary>
    /// <returns>Список List<string></returns>
    public static List<string> GetGameTips()
    {
        List<string> investmentTips = new List<string>();

        investmentTips.Add("Инвестируйте только те деньги, которые вы можете позволить себе потерять.");
        investmentTips.Add("Изучайте финансовые отчеты компаний, прежде чем принимать решение об инвестировании.");
        investmentTips.Add("Диверсифицируйте свой портфель, чтобы снизить риски.");
        investmentTips.Add("Не паникуйте, если рынок падает - это может быть временное явление.");
        investmentTips.Add("Следите за новостями и событиями, которые могут повлиять на цены акций.");
        investmentTips.Add("Изучайте индексы рынка, чтобы понимать общее состояние рынка.");
        investmentTips.Add("Инвестируйте в компании, которые имеют стабильный доход и растут со временем.");
        investmentTips.Add("Не покупайте акции, просто потому что они дешевые - это может быть знаком снижения цен.");
        investmentTips.Add("Избегайте эмоциональных решений при принятии решений об инвестировании.");
        investmentTips.Add("Будьте готовы к риску, если хотите получить большую прибыль.");
        investmentTips.Add("Пользуйтесь различными инструментами, чтобы анализировать рынок и компании.");
        investmentTips.Add("Изучайте историю компаний, прежде чем инвестировать в них.");
        investmentTips.Add("Не пренебрегайте малоизвестными компаниями - они могут иметь большой потенциал.");
        investmentTips.Add("Избегайте инвестирования только в одну отрасль, чтобы снизить риски.");
        investmentTips.Add("Инвестируйте в компании, которые выплачивают дивиденды.");
        investmentTips.Add("Не паникуйте, если ваши инвестиции временно падают в цене - это может быть обычной коррекцией.");
        investmentTips.Add("Не забывайте о налогах при принятии решений об инвестировании.");
        investmentTips.Add("Изучайте конкурентов компаний, чтобы понимать их преимущества и недостатки.");
        investmentTips.Add("Изучайте рынок и компании, прежде чем принимать решения об инвестировании в их акции.");
        investmentTips.Add("Инвестируйте только в те компании, которые вы понимаете.");

        return investmentTips;
    }
}