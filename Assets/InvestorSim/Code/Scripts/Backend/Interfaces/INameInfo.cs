using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Задает свойства описания и наименования
/// </summary>
public interface INameInfo
{
    string Name { get; set; }
    string Description { get; set; }
}