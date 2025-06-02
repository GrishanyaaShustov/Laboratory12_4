namespace Car
{
    public class DeliveryCar : Car
    {
        private static readonly Random random = new Random();
        
        private double loadCapacity;

        public double LoadCapacity
        {
            get => loadCapacity;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Грузоподъёмность должна быть положительной.");
                loadCapacity = value;
            }
        }

        public DeliveryCar() { }

        public DeliveryCar(string brand, int year, string color, decimal price, double clearance, double loadCapacity)
            : base(brand, year, color, price, clearance)
        {
            LoadCapacity = loadCapacity;
        }
        
        public override void Init()
        {
            base.Init();
            Console.Write("Введите грузоподъёмность (тонны): ");
            while (!double.TryParse(Console.ReadLine(), out loadCapacity) || loadCapacity <= 0)
                Console.Write("Некорректный ввод. Введите грузоподъёмность снова: ");
            LoadCapacity = loadCapacity;
        }
        
        public override string ToString()
        {
            return base.ToString() + $", Грузоподъёмность: {LoadCapacity} тонн";
        }

        public override void RandomInit()
        {
            base.RandomInit();
            LoadCapacity = Math.Round(random.NextDouble() * 20 + 1, 2); // от 1 до 20 тонн
        }
        
        public void IRandomInit()
        {
            base.RandomInit();
            LoadCapacity = random.NextDouble() * 20 + 1; // от 1 до 20 тонн
        }
        
        public override object Clone()
        {
            // Вызываем метод Clone базового класса
            DeliveryCar clonedCar = (DeliveryCar)base.Clone();
            
            clonedCar.loadCapacity = this.loadCapacity;

            return clonedCar;
        }

        public override DeliveryCar ShallowCopy()
        {
            return (DeliveryCar)this.MemberwiseClone();
        }

        public override void Show()
        {
            base.Show();
            Console.WriteLine($"Грузоподъёмность: {LoadCapacity} тонн");
        }
        
        public override bool Equals(object obj)
        {
            if (obj is DeliveryCar other)
            {
                return base.Equals(other) &&
                       LoadCapacity == other.LoadCapacity;
            }
            return false;
        }
    }
}