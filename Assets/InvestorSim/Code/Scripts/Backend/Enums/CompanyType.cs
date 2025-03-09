using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Тип компании по форме собственности
/// </summary>
public enum CompanyType
{
    /// <summary>
    /// Публичное акционерное общесто (ПАО)
    /// </summary>
    OpenedCorporation,
    /// <summary>
    /// Закрытое акционерное общество (ЗАО)
    /// </summary>
    ClosedCorporation,
    /// <summary>
    /// Общество с ограниченой отвественностью (ООО)
    /// </summary>
    LLC
}