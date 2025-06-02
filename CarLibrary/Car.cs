namespace Car
{
    public class Car : IInit, IComparable<Car>, ICloneable
    {
        private static readonly Random random = new Random();

        private string brand;
        private int year;
        private string color;
        private decimal price;
        private double clearance;

        public string Brand
        {
            get => brand;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Бренд не может быть пустым.");
                brand = value;
            }
        }

        public int Year
        {
            get => year;
            set
            {
                if (value < 1900 || value > DateTime.Now.Year)
                    throw new ArgumentException("Год выпуска должен быть между 1900 и текущим годом.");
                year = value;
            }
        }

        public string Color
        {
            get => color;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Цвет не может быть пустым.");
                color = value;
            }
        }

        public decimal Price
        {
            get => price;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Стоимость должна быть положительной.");
                price = value;
            }
        }

        public double Clearance
        {
            get => clearance;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Дорожный просвет не может быть отрицательным.");
                clearance = value;
            }
        }
        
        public IdNumber CarId { get; set; }

        public Car() { }

        public Car(string brand, int year, string color, decimal price, double clearance)
        {
            Brand = brand;
            Year = year;
            Color = color;
            Price = price;
            Clearance = clearance;
        }

        public virtual void Show()
        {
            Console.WriteLine($"Бренд: {Brand}, {CarId.ToString()}, Год выпуска: {Year}, Цвет: {Color}, " +
                              $"Стоимость: {Price:C}, Дорожный просвет: {Clearance} мм, ID: {CarId}");
        }

        public virtual void Init()
        {
            Console.Write("Введите бренд: ");
            Brand = Console.ReadLine();
            Console.Write("Введите год выпуска: ");
            while (!int.TryParse(Console.ReadLine(), out year) || year < 1900 || year > DateTime.Now.Year)
                Console.Write("Некорректный ввод. Введите год снова: ");
            Year = year;

            Console.Write("Введите цвет: ");
            Color = Console.ReadLine();

            Console.Write("Введите стоимость: ");
            while (!decimal.TryParse(Console.ReadLine(), out price) || price <= 0)
                Console.Write("Некорректный ввод. Введите стоимость снова: ");
            Price = price;

            Console.Write("Введите дорожный просвет (мм): ");
            while (!double.TryParse(Console.ReadLine(), out clearance) || clearance < 0)
                Console.Write("Некорректный ввод. Введите дорожный просвет снова: ");
            Clearance = clearance;
        }
        
        public virtual object Clone()
        {
            Car clonedCar = (Car)this.MemberwiseClone();
            
            clonedCar.Brand = string.Copy(this.Brand);
            clonedCar.Color = string.Copy(this.Color);
            
            if (this.CarId != null)
            {
                clonedCar.CarId = new IdNumber(this.CarId.Number);
            }
    
            // Возвращаем клонрн объект.
            return clonedCar;
        }


        public virtual Car ShallowCopy()
        {
            return (Car)this.MemberwiseClone();
        }
        
        public override string ToString()
        {
            return $"Марка: {Brand}, {CarId.ToString()}, Год выпуска: {Year}, Цвет: {Color}, Цена: {Price:C}, Дорожный просвет: {Clearance} см";
        }
        
        public int CompareTo(Car other)
        {
            if (other == null) return 1;
            return Year.CompareTo(other.Year);
        }

        public virtual void RandomInit()
        {
            Brand = new[] { "Toyota", "BMW", "Mercedes", "Ford", "Audi" }[random.Next(5)];
            Year = random.Next(1990, DateTime.Now.Year + 1);
            Color = new[] { "Red", "Blue", "Black", "White", "Silver" }[random.Next(5)];
            Price = (decimal)(random.NextDouble() * 100000 + 10000);
            Clearance = random.Next(100, 300);
            CarId = new IdNumber(random.Next(1, 10000));
        }
        
        public void IRandomInit()
        {
            Brand = new[] { "Toyota", "BMW", "Mercedes", "Ford", "Audi" }[random.Next(5)];
            Year = random.Next(1990, DateTime.Now.Year + 1);
            Color = new[] { "Red", "Blue", "Black", "White", "Silver" }[random.Next(5)];
            Price = (decimal)(random.NextDouble() * 100000 + 10000);
            Clearance = random.Next(100, 300);
        }
        
        public static BigCar GetMostExpensiveBigCar(Car[] cars)
        {
            BigCar mostExpensive = null;

            foreach (var car in cars)
            {
                if (car.GetType() == typeof(BigCar))
                {
                    BigCar bigCar = car as BigCar;
                    if (mostExpensive == null || (bigCar != null && bigCar.Price > mostExpensive.Price))
                    {
                        mostExpensive = bigCar;
                    }
                }
            }
            return mostExpensive;
        }

        public static double GetAverageSpeedOfLightCars(Car[] cars)
        {
            int totalSpeed = 0;
            int count = 0;

            foreach (var car in cars)
            {
                if (car is LightCar)
                {
                    LightCar lightCar = car as LightCar;
                    if (lightCar != null && lightCar.GetType() == typeof(LightCar))
                    {
                        totalSpeed += lightCar.MaxSpeed;
                        count++;
                    }
                }
            }

            return count > 0 ? (double)totalSpeed / count : 0;
    }

        public static DeliveryCar[] GetTrucksWithLoadCapacityAbove(Car[] cars, double loadThreshold)
        {
            int count = 0;
            foreach (var car in cars)
            {
                if (car is DeliveryCar)
                {
                    DeliveryCar deliveryCar = car as DeliveryCar;
                    if (deliveryCar != null && deliveryCar.GetType() == typeof(DeliveryCar) && deliveryCar.LoadCapacity > loadThreshold)
                    {
                        count++;
                    }
                }
            }

            DeliveryCar[] result = new DeliveryCar[count];
            int index = 0;
            foreach (var car in cars)
            {
                if (car is DeliveryCar)
                {
                    DeliveryCar deliveryCar = car as DeliveryCar; 
                    if (deliveryCar != null && deliveryCar.GetType() == typeof(DeliveryCar) && deliveryCar.LoadCapacity > loadThreshold)
                    {
                        result[index++] = deliveryCar;
                    }
                }
            }

            return result;
        }
        
        public override bool Equals(object obj)
        {
            if (obj is Car other)
            {
                return Brand == other.Brand &&
                       Year == other.Year &&
                       Color == other.Color &&
                       Price == other.Price &&
                       Clearance == other.Clearance;
            }
            return false;
        }
    }
}
