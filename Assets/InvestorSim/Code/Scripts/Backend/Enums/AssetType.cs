using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum AssetType
{
    /// <summary>
    /// Акции
    /// </summary>
    Shares,
    /// <summary>
    /// Облигации
    /// </summary>
    Bonds,
    /// <summary>
    /// Недвижемость
    /// </summary>
    RealEstate,
    /// <summary>
    /// Любой другой
    /// </summary>
    Default,
    /// <summary>
    /// Нет типа, нужен для рефлексии
    /// </summary>
    Null
}