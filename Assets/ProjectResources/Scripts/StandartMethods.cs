using System;

public static class StandartMethods
{
    /// <summary>
    /// Проверка на содержания бита(степени двойки) в десятичном числе
    /// </summary>
    /// <param name="bit">Бит(степень двойки)</param>
    /// <param name="number">Десятичное число</param>
    /// <returns>Содержит ли число этот бит?</returns>
    static public bool CheckBitInNumber(int bit, int number)
    {
        bit = (int)Math.Pow(2, bit);

        for (int i = 31; i >= 0; i--)
        {
            if (Math.Pow(2, i) == bit)
            {
                if (number - Math.Pow(2, i) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (number - Math.Pow(2, i) > 0)
                {
                    number -= (int)Math.Pow(2, i);
                    continue;
                }
                else
                {
                    continue;
                }
            }
        }

        return false;
    }
}