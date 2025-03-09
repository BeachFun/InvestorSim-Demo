/// <summary>
/// Используется для имитации инфляции
/// </summary>
public enum MovingForce
{
    /// <summary>
    /// Положительная слабая
    /// </summary>
    PosWeak = 3, // last 1
    /// <summary>
    /// Положительная средняя
    /// </summary>
    PosModerate = 6, // last 2
    /// <summary>
    /// Положительная сильная
    /// </summary>
    PosStrong = 12, // last 4
    /// <summary>
    /// Отрицательная слабая
    /// </summary>
    NegWeak = -3, // last -1
    /// <summary>
    /// Отрицательная средняя
    /// </summary>
    NegModerate = -6, // last -2
    /// <summary>
    /// Отрицательная сильная
    /// </summary>
    NegStrong = -12 // last -4
}