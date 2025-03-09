using System;
using System.Globalization;
using System.Collections.Generic;

/// <summary>
/// Представляет из себя класс с наиболее часто применяемыми методами в коде C#
/// </summary>
internal static class SharpHelper
{
	/// <summary>
	/// Возвращает форматированную строку - число, в котором вставлены пробелы между разрядами
	/// </summary>
	/// <returns>Строка/число с пробелами</returns>
	internal static string AddNumSpaces(this decimal number)
	{
		if (-1 < number && number < 1)
			return number.ToString("N4", CultureInfo.InstalledUICulture); // TODO: временное решение, сделать расчет старта символов после запятой чтобы выводить 2 или 3 значимых числа
		else
			return number.ToString("N2", CultureInfo.InstalledUICulture);
	}

	/// <summary>
	/// Возвращает форматированную строку - число, в котором вставлены пробелы между разрядами
	/// </summary>
	/// <returns>Строка/число с пробелами</returns>
	internal static string AddNumSpaces(this double number)
	{
		return number.ToString("N2", CultureInfo.InstalledUICulture);
	}

	/// <summary>
	/// Возвращает форматированную строку - число, в котором вставлены пробелы между разрядами
	/// </summary>
	/// <returns>Строка/число с пробелами</returns>
	internal static string AddNumSpaces(this int number)
	{
		return number.ToString("N0", CultureInfo.InstalledUICulture);
	}

	/// <summary>
	/// Возвращает форматированную строку - число, в котором вставлены пробелы между разрядами
	/// </summary>
	/// <returns>Строка/число с пробелами</returns>
	internal static string AddNumSpaces(this long number)
	{
		return number.ToString("N0", CultureInfo.InstalledUICulture);
	}

	/// <summary>
	/// Переводит TimeSpan в удобный читаемый вид (4 месяца 3 недели)
	/// </summary>
	/// <param name="span">Временной промежуток</param>
	/// <returns>Строковое краткое представление TimeSpan</returns>
	public static string ToReadableShortTimeSpan(this TimeSpan span)
	{
		int years = (int)(span.Days / 365.25);
		int months = (int)((span.Days % 365.25) / 30.4375);
		int weeks = (int)((span.Days % 365.25) % 30.4375 / 7);
		int days = (int)((span.Days % 365.25) % 30.4375 % 7);

		string result = "";

		if (years > 4)
		{
			result += string.Format("{0} {1} ", years, GetDeclension(years, "год", "года", "лет"));
		}
		else if (years > 0)
		{
			result += string.Format("{0} {1} ", years, GetDeclension(years, "год", "года", "лет"));

			if (months > 0)
			{
				result += string.Format("{0} {1} ", months, GetDeclension(months, "месяц", "месяца", "месяцев"));
			}
		}
		else
		{
			if (months > 0)
			{
				result += string.Format("{0} {1} ", months, GetDeclension(months, "месяц", "месяца", "месяцев"));

				if (weeks > 0)
				{
					result += string.Format("{0} {1} ", weeks, GetDeclension(weeks, "неделя", "недели", "недель"));
				}
			}
			else
			{
				if (weeks > 0)
				{
					result += string.Format("{0} {1} ", weeks, GetDeclension(weeks, "неделя", "недели", "недель"));
				}
				if (days > 0)
				{
					result += string.Format("{0} {1} ", days, GetDeclension(days, "день", "дня", "дней"));
				}
			}
		}

		return result.Trim();
	}

	private static string GetDeclension(int number, string nominative, string genitiveSingular, string genitivePlural)
	{
		if (number % 100 >= 11 && number % 100 <= 19)
		{
			return genitivePlural;
		}
		else if (number % 10 == 1)
		{
			return nominative;
		}
		else if (number % 10 >= 2 && number % 10 <= 4)
		{
			return genitiveSingular;
		}
		else
		{
			return genitivePlural;
		}
	}

	/// <summary>
	/// Форматирует дату следующим образом [11.11.2001]
	/// </summary>
	public static string ToMyFormat(this DateTime date) => date.ToString("d"); // TODO: подумать над удалением


	/// <summary>
	/// Переводит PaymentFrequency в читаемый формат
	/// </summary>
	/// <returns>Строка формата: в день (PaymentFrequency.Daily)</returns>
	public static string ToMyFormat(this PaymentFrequency payment)
	{
		switch (payment)
		{
			case PaymentFrequency.Daily:
				return "в день";
			case PaymentFrequency.Weekly:
				return "в неделю";
			case PaymentFrequency.Monthly:
				return "в месяц";
			case PaymentFrequency.Quarterly:
				return "в 3 месяца";
			case PaymentFrequency.BiAnnually:
				return "в полгода";
			case PaymentFrequency.Annually:
				return "в год";

			default:
				return null;
		}
	}


	/// <summary>
	/// Возвращает случайный элемент из коллекции - списка
	/// </summary>
	public static T GetRandom<T>(this List<T> list) => list[new Random().Next(0, list.Count)];
	/// <summary>
	/// Возвращает случайный элемент из коллекции - массив
	/// </summary>
	public static T GetRandom<T>(params T[] list) => list[new Random().Next(0, list.Length)];


	/// <summary>
	/// Сокращает число в простой формат | 3280000 -> 3.28M
	/// </summary>
	public static string ShortenNumberString(this decimal number)
	{
		string suffix = "";

		if (number >= 1000 && number < 1000000)
		{
			number /= 1000;
			suffix = "K";
		}
		else if (number >= 1000000 && number < 1000000000)
		{
			number /= 1000000;
			suffix = "M";
		}
		else if (number >= 1000000000 && number < 1000000000000)
		{
			number /= 1000000000;
			suffix = "B";
		}
		else if (number >= 1000000000000)
		{
			number /= 1000000000000;
			suffix = "T";
		}

		if (Math.Round(number, 2) == (long)number)
		{
			return $"{(long)number}{suffix}".Replace(",", ".");
		}
		else
		{
			string formattedNumber = number.ToString("F2").Replace(",", ".");

			if (formattedNumber.EndsWith(".00"))
				formattedNumber = formattedNumber.Substring(0, formattedNumber.Length - 3);
			else if (formattedNumber.EndsWith("0"))
				formattedNumber = formattedNumber.Substring(0, formattedNumber.Length - 1);

			return $"{formattedNumber}{suffix}";
		}
	}
}