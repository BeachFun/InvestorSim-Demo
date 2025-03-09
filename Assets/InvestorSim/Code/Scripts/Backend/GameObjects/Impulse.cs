using System;

[Serializable]

public class Impulse : ICloneable
{
    // TODO: Дать сводку классу и подумать над добавлением методов перезапуска импульса или отката последнего вычисления

    public Impulse(decimal power, int durationInDays, int moment)
    {
        this.Power = power;
        this.Duration = durationInDays;
        this.Moment = moment;
    }


    /// <summary>
    /// Проверка на действие импульса
    /// </summary>
    public bool IsActive { get => this.Moment < this.Duration; }
    /// <summary>
    /// Сила импульса
    /// </summary>
    public decimal Power { get; private set; }
    /// <summary>
    /// Продолительность
    /// </summary>
    public int Duration { get; private set; }
    /// <summary>
    /// Текущий момент
    /// </summary>
    public int Moment { get; private set; }


    // Изменение в %
    public decimal CalcCurrentPower()
    {
        if (this.IsActive)
        {
            if(this.Moment < 0) this.Moment = 0;

            double x = (double)(this.Moment - 1) / this.Duration;

            //decimal impulseFactor = (decimal)(1 - Math.Sinh((double)this.Moment / duration)); // f(x) = 1 - sinh(x) | вычисление фактора импульса на основе прошедших дней и продолжительности новости
            //decimal impulseFactor = (decimal)(0 - Math.Log2((double)(this.Moment+1) / duration)); // f(x) = 0 - log2(x) вычисление фактора импульса на основе прошедших дней и продолжительности новости
            //decimal impulseFactor = (decimal)Math.Exp(-(double)this.Moment / duration); // f(x) = exp(-x) вычисление фактора импульса на основе прошедших дней и продолжительности новости
            decimal impulseFactor = (decimal)(Math.Exp(-2.5 * x));// f(x) = exp(-2.5x)

            decimal result = Power * impulseFactor; // вычисление импульса на основе фактора импульса и коэффициента
            this.Moment++;

            Messenger.Broadcast(NewsNotice.IMPULSE_CALCED);

            return Decimal.Round(result, 2);
        }
        else
        {
            return 0; // импульс равен нулю
        }
    }


    public object Clone()
    {
        return new Impulse(this.Power, this.Duration, this.Moment);
    }
}