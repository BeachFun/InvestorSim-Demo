using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Фильтр, хранящий несколько значений и представляющий их в виде набора строковых 
/// представлений каждого возможного значения фильтра. Аналог enum с атрибутом [Flags].
/// </summary>
/// <typeparam name="TFilter">Непосредственно наследник</typeparam>
public class MultivaluedFilter<TFilter> : IEqualityComparer<MultivaluedFilter<TFilter>>, IEquatable<MultivaluedFilter<TFilter>> where TFilter : MultivaluedFilter<TFilter>, new()
{
    /// <summary>
    /// Выбранные элементы
    /// </summary>
    private List<string> Selected { get; set; } = new List<string>();


    /// <summary>
    /// Аналог enum, типобезопасен.
    /// </summary>
    protected MultivaluedFilter() { }


    /// <summary>
    /// Объединяет наборы фильтров
    /// </summary>
    /// <param name="a">Первый набор фильтров</param>
    /// <param name="b">Второй набор фильтров</param>
    /// <returns>Объединенный набор фильтров</returns>
    public static TFilter operator |(MultivaluedFilter<TFilter> a, MultivaluedFilter<TFilter> b)
    {
        return new TFilter
        {
            Selected = (from x in a.Selected.Union(b.Selected)
                        orderby x
                        select x).ToList()
        };
    }

    /// <summary>
    /// Регистрирует возможное значение.
    /// Исключения:
    ///   T:System.ArgumentException:
    ///     Mask must be left power of 2 (i.e. only one bit must be equal to 1);mask
    /// </summary>
    /// <param name="mask">Маска</param>
    /// <param name="value">Значение</param>
    /// <returns></returns>
    protected static TFilter RegisterPossibleValue(ulong mask, string value)
    {
        IEnumerable<string> source = from x in value.Split(',')
                                     select x.Trim();
        TFilter val2 = new TFilter();
        val2.Selected.AddRange(source.OrderBy((string x) => x));
        return val2;
    }



    /// <summary>
    /// Преобразовать в строку.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return string.Join(",", Selected.ToArray());
    }

    public bool Equals(MultivaluedFilter<TFilter> x, MultivaluedFilter<TFilter> y)
    {
        if (x == null)
        {
            return false;
        }

        if (y == null)
        {
            return false;
        }

        if (y == x)
        {
            return true;
        }

        return x.Selected.SequenceEqual(y.Selected);
    }

    public bool Equals(MultivaluedFilter<TFilter> other)
    {
        return Equals(this, other);
    }

    public override bool Equals(object obj)
    {
        if (obj?.GetType() == GetType())
        {
            return Equals(this, (MultivaluedFilter<TFilter>)obj);
        }

        return false;
    }

    public int GetHashCode(MultivaluedFilter<TFilter> obj)
    {
        if (Selected == null)
        {
            return 0;
        }

        return Selected.GetHashCode();
    }

    public override int GetHashCode()
    {
        return GetHashCode(this);
    }
}


/* Удаленные элементы класса

    /// <summary>
    ///  Разобрать из json.
    /// </summary>
    /// <param name="val">Ответ сервера</param>
    /// <returns></returns>
    public static TFilter FromJsonString(string val)
    {
        IEnumerable<string> source = from x in val.Split(',')
                                     select x.Trim();
        TFilter val2 = new TFilter();
        val2.Selected.AddRange(source.OrderBy((string x) => x));
        return val2;
    }

    /// <summary>
    /// Регистрирует возможное значение.
    /// Исключения:
    ///   T:System.ArgumentException:
    ///     Mask must be left power of 2 (i.e. only one bit must be equal to 1);mask
    /// </summary>
    /// <param name="value">Значение</param>
    /// <returns></returns>
    protected static TFilter RegisterPossibleValue(string value)
    {
        return FromJsonString(value);
    }

    /// <summary>
    /// Разобрать из json.
    /// </summary>
    /// <param name="response">Ответ сервера</param>
    /// <returns>Объект перечисления типа TFilter Непосредственно наследник</returns>
    public static TFilter FromJson(VkResponse response)
    {
        return FromJsonString(response.ToString());
    }

    

 */