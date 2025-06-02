namespace Car
{
    public class BigCar : Car
    {
        private static readonly Random random = new Random();

        private bool allWheelDrive;
        private string offRoadType;

        public bool AllWheelDrive
        {
            get => allWheelDrive;
            set => allWheelDrive = value;
        }

        public string OffRoadType
        {
            get => offRoadType;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Тип бездорожья не может быть пустым.");
                offRoadType = value;
            }
        }

        public BigCar()
        {
        }

        public BigCar(string brand, int year, string color, decimal price, double clearance, bool allWheelDrive,
            string offRoadType)
            : base(brand, year, color, price, clearance)
        {
            AllWheelDrive = allWheelDrive;
            OffRoadType = offRoadType;
        }

        public override void Init()
        {
            base.Init();
            Console.Write("Есть ли полный привод? (true/false): ");
            while (!bool.TryParse(Console.ReadLine(), out allWheelDrive))
                Console.Write("Некорректный ввод. Введите true или false снова: ");
            AllWheelDrive = allWheelDrive;

            Console.Write("Введите тип бездорожья: ");
            OffRoadType = Console.ReadLine();
        }
        
        // Реализация интерфейса ICloneable (поверхностное копирование)
        public override object Clone()
        {
            // Вызываем метод Clone базового класса
            BigCar clonedCar = (BigCar)base.Clone();

            clonedCar.offRoadType = string.Copy(this.offRoadType);
            clonedCar.allWheelDrive = this.allWheelDrive;

            return clonedCar;
        }

        public override BigCar ShallowCopy()
        {
            return (BigCar)this.MemberwiseClone();
        }
        
        
        public override string ToString()
        {
            return base.ToString() + $", Полный привод: {(AllWheelDrive ? "Да" : "Нет")}, Тип бездорожья: {OffRoadType}";
        }

        public override void RandomInit()
        {
            base.RandomInit();
            AllWheelDrive = random.Next(2) == 1;
            OffRoadType = new[] { "Лес", "Песок", "Грязь", "Снег" }[random.Next(4)];
        }
        
        public void IRandomInit()
        {
            base.RandomInit();
            AllWheelDrive = random.Next(2) == 1;
            OffRoadType = new[] { "Лес", "Песок", "Грязь", "Снег" }[random.Next(4)];
        }

        public override void Show()
        {
            base.Show();
            Console.WriteLine($"Полный привод: {(AllWheelDrive ? "Да" : "Нет")}, Тип бездорожья: {OffRoadType}");
        }

        public override bool Equals(object obj)
        {
            if (obj is BigCar other)
            {
                return base.Equals(other) &&
                       AllWheelDrive == other.AllWheelDrive &&
                       OffRoadType == other.OffRoadType;
            }

            return false;
        }
    }
}