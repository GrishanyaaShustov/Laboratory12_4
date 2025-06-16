using System;

namespace Car
{
    class Program
    {
        static void Main()
        {
            var list = new DoublyLinkedList<Car>();
            var hashTable = new MyHashTable<Car>(Car => Car.CarId.Number, (a, b) => (int)a == (int)b);
            Tree<Car> balancedTree = null;
            Tree<Car> searchTree = null;
            
            DoublyLinkedList<Car> clone = null;
            
            bool exitProgram = false;

            while (!exitProgram)
            {
                Console.WriteLine("\n1. Операции с двусвязным списком");
                Console.WriteLine("2. Операции с хеш-таблицей");
                Console.WriteLine("3. Операции с бинарным деревом");
                Console.WriteLine("4. Операции с реализованными интерфейсами");
                Console.WriteLine("0. Выход");
                Console.Write("Ваш выбор: ");
                string mainChoice = Console.ReadLine();

                switch (mainChoice)
                {
                    case "1":
                        bool backToMainFromList = false;
                        while (!backToMainFromList)
                        {
                            Console.WriteLine("\n--- Меню: Операции с двусвязным списком ---");
                            Console.WriteLine("1. Добавить элементы с номерами 1,3,5... (рандомные)");
                            Console.WriteLine("2. Вывести список");
                            Console.WriteLine("3. Удалить элементы от заданного имени до конца");
                            Console.WriteLine("4. Глубокое клонирование списка");
                            Console.WriteLine("5. Удалить список из памяти");
                            Console.WriteLine("0. Назад в главное меню");
                            Console.Write("Ваш выбор: ");

                            string listChoice = Console.ReadLine();
                            switch (listChoice)
                            {
                                case "1":
                                    Console.Write("Сколько нечётных элементов добавить?: ");
                                    if (int.TryParse(Console.ReadLine(), out int count))
                                    {
                                        Random rand = new Random();
                                        list.AddOddGenerated(count, () =>
                                        {
                                            Type[] derivedTypes =
                                                { typeof(LightCar), typeof(BigCar), typeof(DeliveryCar) };
                                            Type type = derivedTypes[rand.Next(derivedTypes.Length)];
                                            Car car = (Car)Activator.CreateInstance(type);
                                            car.RandomInit();
                                            return (Car)car;
                                        });
                                    }
                                    else
                                    {
                                        Console.WriteLine("Неверный ввод числа.");
                                    }

                                    break;

                                case "2":
                                {
                                    if (list.head == null)
                                    {
                                        Console.WriteLine("Список пуст.");
                                        break;
                                    }

                                    var current = list.head;
                                    int index = 1;
                                    while (current != null)
                                    {
                                        Console.Write($"{index}: ");
                                        if (current.Data != null)
                                        {
                                            Console.WriteLine(current.Data.ToString());
                                        }
                                        else
                                        {
                                            Console.WriteLine("null");
                                        }

                                        current = current.Next;
                                        index++;
                                    }
                                }
                                    break;

                                case "3":
                                    Console.Write("Введите имя бренда для удаления с него и до конца: ");
                                    string name = Console.ReadLine();

                                    list.DeleteFromKey(name,
                                        item => (item.GetType().GetProperty("Brand")?.GetValue(item) as IComparable) ?? "");

                                    Console.WriteLine($"Удаление элементов с брендом '{name}' и далее выполнено.");
                                    break;


                                case "4":
                                    var cloned = (DoublyLinkedList<Car>)list.Clone();
                                    Console.WriteLine("Список клонирован!");

                                    var currentClone = cloned.head;
                                    while (currentClone != null)
                                    {
                                        if (currentClone.Data != null)
                                        {
                                            // Пример изменения CarId у клона
                                            currentClone.Data.CarId = new IdNumber(5);
                                        }

                                        currentClone = currentClone.Next;
                                    }

                                    Console.WriteLine("Клонированный список с изменёнными CarId:");
                                    if (cloned.head == null)
                                    {
                                        Console.WriteLine("Список пуст.");
                                    }
                                    else
                                    {
                                        var current = cloned.head;
                                        int index = 1;
                                        while (current != null)
                                        {
                                            Console.Write($"{index}: ");
                                            if (current.Data != null)
                                                Console.WriteLine(current.Data.ToString());
                                            else
                                                Console.WriteLine("null");

                                            current = current.Next;
                                            index++;
                                        }
                                    }

                                    Console.WriteLine("\nОригинальный список после изменения CarId в клонированном списке:");
                                    if (list.head == null)
                                    {
                                        Console.WriteLine("Список пуст.");
                                    }
                                    else
                                    {
                                        var current = list.head;
                                        int index = 1;
                                        while (current != null)
                                        {
                                            Console.Write($"{index}: ");
                                            if (current.Data != null)
                                                Console.WriteLine(current.Data.ToString());
                                            else
                                                Console.WriteLine("null");

                                            current = current.Next;
                                            index++;
                                        }
                                    }
                                    break;
                                case "5":
                                    list.Clear();
                                    Console.WriteLine("Список очищен из памяти.");
                                    break;

                                case "0":
                                    backToMainFromList = true;
                                    break;

                                default:
                                    Console.WriteLine("Некорректный ввод!");
                                    break;
                            }
                        }

                        break;

                    case "2":

                        Random hashRand = new Random();
                        for (int i = 0; i < 20; i++)
                        {
                            Car newCar = hashRand.Next(3) switch
                            {
                                0 => new LightCar(),
                                1 => new BigCar(),
                                2 => new DeliveryCar(),
                                _ => null
                            };
                            newCar?.RandomInit();
                            if (newCar != null)
                                hashTable.Add(newCar);
                        }

                        bool backToMainFromHash = false;
                        while (!backToMainFromHash)
                        {
                            Console.WriteLine("\n--- Меню: Операции с хеш-таблицей ---");
                            Console.WriteLine("1. Добавить элемент в хеш-таблицу");
                            Console.WriteLine("2. Найти и (опц.) удалить элемент по ключу");
                            Console.WriteLine("3. Вывести хеш-таблицу");
                            Console.WriteLine("4. Очистка");
                            Console.WriteLine("0. Назад в главное меню");
                            Console.Write("Ваш выбор: ");

                            string hashChoice = Console.ReadLine();
                            switch (hashChoice)
                            {
                                case "1":
                                    Console.WriteLine("Выберите тип машины для добавления:");
                                    Console.WriteLine("1. Легковой автомобиль");
                                    Console.WriteLine("2. Грузовик");
                                    Console.WriteLine("3. Спортивный автомобиль");
                                    Console.Write("Ваш выбор: ");
                                    string carTypeChoice = Console.ReadLine();

                                    Car userCar = carTypeChoice switch
                                    {
                                        "1" => new LightCar(),
                                        "2" => new BigCar(),
                                        "3" => new DeliveryCar(),
                                        _ => null
                                    };

                                    if (userCar != null)
                                    {
                                        userCar.RandomInit();
                                        hashTable.Add(userCar);
                                        Console.WriteLine($"Машина с ID {userCar.CarId.Number} добавлена.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Неверный выбор типа автомобиля.");
                                    }

                                    break;

                                case "2":
                                    Console.Write("Введите ключ для поиска (число CarId): ");
                                    if (int.TryParse(Console.ReadLine(), out int searchKey))
                                    {
                                        var found = hashTable.Find(searchKey);
                                        if (found != null)
                                        {
                                            Console.WriteLine("Найденный элемент:");
                                            Console.WriteLine(found);

                                            Console.Write("Удалить этот элемент? (y/n): ");
                                            string deleteAnswer = Console.ReadLine()?.ToLower();
                                            if (deleteAnswer == "y")
                                            {
                                                if (hashTable.Remove(searchKey))
                                                    Console.WriteLine("Элемент удалён.");
                                                else
                                                    Console.WriteLine("Ошибка при удалении.");
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Элемент с таким ключом не найден.");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Неверный формат ключа.");
                                    }

                                    break;

                                case "3":
                                    Console.WriteLine("\nСодержимое хеш-таблицы:");
                                    for (int i = 0; i < hashTable.DefaultLength; i++)
                                    {
                                        Console.Write($"[{i}]: ");
                                        var node = hashTable.Table[i];
                                        while (node != null)
                                        {
                                            Console.Write($"{node.Data} -> ");
                                            node = node.Next;
                                        }

                                        Console.WriteLine("null");
                                    }

                                    break;

                                case "4":
                                    hashTable.Clear();
                                    Console.WriteLine("Хеш-таблица очищена.");
                                    break;

                                case "0":
                                    backToMainFromHash = true;
                                    break;

                                default:
                                    Console.WriteLine("Некорректный ввод!");
                                    break;
                            }
                        }

                        break;


                    case "3":

                        // Генерация объектов
                        List<Car> cars = new();
                        Random treeRand = new Random();
                        for (int i = 0; i < 15; i++)
                        {
                            Car car = treeRand.Next(3) switch
                            {
                                0 => new LightCar(),
                                1 => new BigCar(),
                                2 => new DeliveryCar(),
                                _ => new LightCar()
                            };
                            car.RandomInit();
                            cars.Add(car);
                        }

                        cars.Sort();

                        foreach (var car in cars)
                        {
                            Console.Write(car.Year + " ");
                        }

                        balancedTree = Tree<Car>.BuildBalancedTree(cars);

                        bool backToMainFromTree = false;
                        while (!backToMainFromTree)
                        {
                            Console.WriteLine("\n--- Меню: Операции с деревьями ---");
                            Console.WriteLine("1. Показать идеально сбалансированное дерево");
                            Console.WriteLine("2. Найти максимальный элемент (по году)");
                            Console.WriteLine("3. Преобразовать в дерево поиска");
                            Console.WriteLine("4. Показать дерево поиска");
                            Console.WriteLine("5. Удалить элемент по ключу (год)");
                            Console.WriteLine("6. Очистить дерево");
                            Console.WriteLine("0. Назад в главное меню");
                            Console.Write("Ваш выбор: ");

                            switch (Console.ReadLine())
                            {
                                case "1":
                                    if (balancedTree != null)
                                        balancedTree.ShowTree(Car =>
                                            $"{Car.Brand} (Id: {Car.CarId.Number}, Year: {Car.Year})");
                                    else
                                        Console.WriteLine("Дерево пусто.");
                                    break;

                                case "2":
                                    if (balancedTree != null)
                                    {
                                        var max = balancedTree.FindMaxElement(Car =>
                                            Car.Year); // Передаём делегат по возрасту
                                        Console.WriteLine($"Максимальный элемент: {max}");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Дерево пусто.");
                                    }

                                    break;

                                case "3":
                                    if (balancedTree != null)
                                    {
                                        searchTree = Tree<Car>.BuildBalancedSearchTree(balancedTree);
                                        Console.WriteLine("Преобразование в дерево поиска без повторов выполнено.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Идеально сбалансированное дерево отсутствует.");
                                    }

                                    break;

                                case "4":
                                    if (searchTree != null)
                                        searchTree.ShowTree(Car =>
                                            $"{Car.Brand} (Id: {Car.CarId.Number}, Year: {Car.Year})");
                                    else
                                        Console.WriteLine("Дерево поиска пусто.");
                                    break;

                                case "5":
                                    Console.Write("Введите год машины для удаления: ");
                                    if (int.TryParse(Console.ReadLine(), out int year))
                                    {
                                        if (balancedTree != null)
                                        {
                                            balancedTree = Tree<Car>.RemoveByKey(balancedTree, year, car => car.Year);
                                            Console.WriteLine("Удаление выполнено.");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Дерево поиска пусто.");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Неверный ввод.");
                                    }

                                    break;

                                case "6":
                                    balancedTree = null;
                                    searchTree = null;
                                    Console.WriteLine("Деревья очищены.");
                                    break;

                                case "0":
                                    backToMainFromTree = true;
                                    break;

                                default:
                                    Console.WriteLine("Некорректный ввод!");
                                    break;
                            }
                        }

                        break;
                    case "4":
                        {
                            MyCollection<Car> carsCollection = null;
                            LightCar lastManualCar = null;
                            bool exitSubMenu = false;

                            while (!exitSubMenu)
                            {
                                Console.WriteLine("\n--- Демонстрация работы с интерфейсами ---");
                                Console.WriteLine("1. Создать коллекцию и сгенерировать 5 автомобилей");
                                Console.WriteLine("2. Показать сгенерированные автомобили");
                                Console.WriteLine("3. Добавить автомобиль вручную");
                                Console.WriteLine("4. Проверить наличие вручную добавленного автомобиля (Contains)");
                                Console.WriteLine("5. Удалить вручную добавленный автомобиль");
                                Console.WriteLine("6. Проверить наличие после удаления");
                                Console.WriteLine("7. Копировать коллекцию в массив и показать");
                                Console.WriteLine("0. Вернуться в главное меню");
                                Console.Write("Выберите действие: ");
                                string subChoice = Console.ReadLine();

                                switch (subChoice)
                                {
                                    case "1":
                                        carsCollection = new MyCollection<Car>(
                                            length: 5,
                                            keySelector: car => car.CarId.Number,
                                            keyComparer: (a, b) => (int)a == (int)b
                                        );
                                        lastManualCar = null;
                                        Console.WriteLine("Коллекция создана и заполнена 5 случайными автомобилями.");
                                        break;

                                    case "2":
                                        if (carsCollection == null)
                                            Console.WriteLine("Коллекция пуста. Сначала создайте коллекцию (пункт 1).");
                                        else
                                        {
                                            Console.WriteLine("Сгенерированные элементы:");
                                            foreach (var car in carsCollection)
                                                car.Show();
                                        }
                                        break;

                                    case "3":
                                        if (carsCollection == null)
                                        {
                                            Console.WriteLine("Коллекция пуста. Сначала создайте коллекцию (пункт 1).");
                                        }
                                        else
                                        {
                                            LightCar manualCar = new LightCar();
                                            manualCar.RandomInit();
                                            carsCollection.Add(manualCar);
                                            lastManualCar = manualCar;
                                            Console.WriteLine("Автомобиль добавлен вручную:");
                                            manualCar.Show();
                                        }
                                        break;

                                    case "4":
                                        if (carsCollection == null || lastManualCar == null)
                                            Console.WriteLine("Сначала создайте коллекцию и добавьте автомобиль вручную (пункты 1 и 3).");
                                        else
                                            Console.WriteLine(carsCollection.Contains(lastManualCar) ? "Найден" : "Не найден");
                                        break;

                                    case "5":
                                        if (carsCollection == null || lastManualCar == null)
                                            Console.WriteLine("Сначала создайте коллекцию и добавьте автомобиль вручную (пункты 1 и 3).");
                                        else
                                        {
                                            bool removed = carsCollection.Remove(lastManualCar);
                                            Console.WriteLine(removed ? "Удалён" : "Не найден");
                                        }
                                        break;

                                    case "6":
                                        if (carsCollection == null || lastManualCar == null)
                                            Console.WriteLine("Сначала создайте коллекцию и добавьте автомобиль вручную (пункты 1 и 3).");
                                        else
                                            Console.WriteLine(carsCollection.Contains(lastManualCar) ? "Найден" : "Не найден");
                                        break;

                                    case "7":
                                        if (carsCollection == null)
                                            Console.WriteLine("Коллекция пуста. Сначала создайте коллекцию (пункт 1).");
                                        else
                                        {
                                            Car[] array = new Car[carsCollection.Count];
                                            carsCollection.CopyTo(array, 0);
                                            Console.WriteLine("Содержимое массива:");
                                            foreach (var car in array)
                                                car.Show();
                                        }
                                        break;

                                    case "0":
                                        exitSubMenu = true; // выйти из подменю, вернуться в главное меню
                                        break;

                                    default:
                                        Console.WriteLine("Неверный пункт. Попробуйте ещё раз.");
                                        break;
                                }
                            }

                            break;
                        }
                    case "0":
                        exitProgram = true;
                        break;

                    default:
                        Console.WriteLine("Некорректный ввод!");
                        break;
                }
            }

            Console.WriteLine("Завершение работы программы.");
        }
    }
}