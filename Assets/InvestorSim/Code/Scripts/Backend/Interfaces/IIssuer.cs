using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Присваивает свойства эмитента
/// </summary>
public interface IIssuer : INameInfo
{
    // IBusiness

    /// <summary>
    /// Активы компании
    /// </summary>
    List<string> Assets { get; set; }

    /// <summary>
    /// Степень доверия к эмитенту
    /// </summary>
    public float CredibilityRate { get; set; }
}

