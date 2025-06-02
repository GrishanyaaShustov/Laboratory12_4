public class IdNumber
{
    public int Number { get; set; }

    // Конструктор
    public IdNumber(int number)
    {
        if (number < 0)
            throw new ArgumentException("Номер ID не может быть отрицательным.");
        Number = number;
    }

    // Конструктор копирования
    public IdNumber(IdNumber other)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other));
        Number = other.Number;
    }

    public override string ToString()
    {
        return $"ID: {Number}";
    }
    
    public override bool Equals(object obj)
    {
        if (obj is IdNumber other)
        {
            return Number == other.Number;
        }

        return false;
    }
}