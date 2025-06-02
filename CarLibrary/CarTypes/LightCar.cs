namespace Car
{
    public class LightCar : Car
    {
        private static readonly Random random = new Random();
        
        private int seatCount;
        private int maxSpeed;

        public int SeatCount
        {
            get => seatCount;
            set
            {
                if (value < 2 || value > 7)
                    throw new ArgumentException("Количество мест должно быть от 2 до 7.");
                seatCount = value;
            }
        }

        public int MaxSpeed
        {
            get => maxSpeed;
            set
            {
                if (value < 100 || value > 300)
                    throw new ArgumentException("Максимальная скорость должна быть от 100 до 300 км/ч.");
                maxSpeed = value;
            }
        }

        public LightCar() { }

        public LightCar(string brand, int year, string color, decimal price, double clearance, int seatCount, int maxSpeed)
            : base(brand, year, color, price, clearance)
        {
            SeatCount = seatCount;
            MaxSpeed = maxSpeed;
        }
        
        public override void Init()
        {
            base.Init();
            Console.Write("Введите количество мест: ");
            while (!int.TryParse(Console.ReadLine(), out seatCount) || seatCount < 2 || seatCount > 7)
                Console.Write("Некорректный ввод. Введите количество мест снова: ");
            SeatCount = seatCount;

            Console.Write("Введите максимальную скорость (км/ч): ");
            while (!int.TryParse(Console.ReadLine(), out maxSpeed) || maxSpeed < 100 || maxSpeed > 300)
                Console.Write("Некорректный ввод. Введите максимальную скорость снова: ");
            MaxSpeed = maxSpeed;
        }
        
        public override string ToString()
        {
            return base.ToString() + $", Количество мест: {SeatCount}, Максимальная скорость: {MaxSpeed} км/ч";
        }

        public override void RandomInit()
        {
            base.RandomInit();
            SeatCount = random.Next(2, 8);
            MaxSpeed = random.Next(100, 301);
        }
        
        public void IRandomInit()
        {
            base.RandomInit();
            SeatCount = random.Next(2, 8);
            MaxSpeed = random.Next(100, 301);
        }
        
        public override object Clone()
        {
            LightCar clonedCar = (LightCar)base.Clone();
            clonedCar.seatCount = this.seatCount;
            clonedCar.maxSpeed = this.maxSpeed;

            return clonedCar;
        }

        public override LightCar ShallowCopy()
        {
            return (LightCar)this.MemberwiseClone();
        }

        public override void Show()
        {
            base.Show();
            Console.WriteLine($"Количество мест: {SeatCount}, Максимальная скорость: {MaxSpeed} км/ч");
        }
        
        public override bool Equals(object obj)
        {
            if (obj is LightCar other)
            {
                return base.Equals(other) && 
                       SeatCount == other.SeatCount && 
                       MaxSpeed == other.MaxSpeed;
            }
            return false;
        }
    }
}