using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSLight13_9
{
    class Program
    {
        static void Main(string[] args)
        {
            DailyQueueOfCars dailyQueueOfCars = new DailyQueueOfCars();
            CarService carService = new CarService();
            Car car;
            DetailsStore detailsStore = new DetailsStore();
            int userInput;
            int userChoice;
            bool carServiceWorking = true;

            while (carServiceWorking)
            {
                Console.Clear();
                carService.DisplayWallet();
                Console.WriteLine("Это меню управления вашим автосервисом.");
                Console.WriteLine("1. Принять следующего клиента");
                Console.WriteLine("2. Открыть каталог деталей");
                Console.WriteLine("3. Осмотреть склад деталей");
                Console.WriteLine("0. Выйти");

                userInput = Convert.ToInt32(Console.ReadLine());

                switch (userInput)
                {
                    case 1:
                        car = dailyQueueOfCars.ReturnNextCar();

                        if (car == null)
                        {
                            Console.Clear();
                            Console.WriteLine($"На сегодня клиенты закончились. Поздравляем с окончанием рабочего дня! Хотите начать новый день?");
                            Console.WriteLine("1. Да.");
                            Console.WriteLine("2. Нет. Выйти.");
                            userInput = Convert.ToInt32(Console.ReadLine());

                            if(userInput == 1)
                            {
                                dailyQueueOfCars = new DailyQueueOfCars();
                                continue;
                            }
                            else if (userInput == 2)
                            {
                                carServiceWorking = false;
                                continue;
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("Неверная команда");
                                Console.ReadKey();
                                continue;
                            }
                        }

                        Console.Clear();
                        Console.WriteLine("Новый клиент");
                        car.DisplayListDetails();
                        Console.WriteLine();
                        Console.WriteLine("1. Выбрать деталь для замены");
                        Console.WriteLine("2. Отказатся ремонтировать");
                        userChoice = Convert.ToInt32(Console.ReadLine());

                        if (userChoice == 1)
                        {
                            carService.DisplayWarehouseDetail();
                            int DetailNumber = Convert.ToInt32(Console.ReadLine());
                            Detail detailForRepair = carService.Repair(DetailNumber);
                            int countIncome = car.Repair(detailForRepair);
                            carServiceWorking = carService.TakeIncome(countIncome);
                        }
                        else if (userChoice == 2)
                        {
                            Console.Clear();
                            carServiceWorking = carService.PayFine();
                            Console.WriteLine("Шраф 100 рублей");
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Неверная команда");
                            Console.ReadKey();
                        }
                        break;
                    case 2:
                        detailsStore.Menu();
                        userChoice = Convert.ToInt32(Console.ReadLine());
                        Detail detail = detailsStore.SellingDetail(userChoice);

                        if (detail != null)
                        {
                            carService.BuyDetail(detail);
                        }
                        break;
                    case 3:
                        carService.DisplayWarehouseDetail();
                        Console.ReadKey();
                        break;
                    case 0:
                        carServiceWorking = false;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Неверная команда");
                        Console.ReadKey();
                        break;
                }
            }
        }

        class CarService
        {
            private int _wallet;
            private WarehouseDetail _warehouseDetail = new WarehouseDetail();

            public CarService()
            {
                _wallet = 10000;
            }

            public void BuyDetail(Detail detail)
            {
                if ((_wallet - detail.Price) >= 0)
                {
                    _wallet -= detail.Price;
                    _warehouseDetail.Add(detail);
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("У вас не досточно средств для покупки этой детали");
                    Console.ReadKey();
                }
            }

            public void DisplayWarehouseDetail()
            {
                _warehouseDetail.DisplayAllDetail();
            }

            public void DisplayWallet()
            {
                Console.SetCursorPosition(60, 0);
                Console.WriteLine($"Балланс: {_wallet} рублей");
                Console.SetCursorPosition(0, 0);
            }

            public Detail Repair(int detailNumber)
            {
                return _warehouseDetail.GetDetail(detailNumber);
            }

            public bool TakeIncome(int countIncome)
            {
                Console.Clear();

                if (countIncome > 0)
                {
                    Console.WriteLine($"Ваш доход составил {countIncome} рублей.");
                }
                else
                {
                    Console.WriteLine($"Вы ошиблись и заменили исправную деталь. Вам пришлось возместить ущерб. {countIncome} рублей");
                }

                _wallet += countIncome;
                Console.ReadKey();

                if (_wallet < 0)
                {
                    Console.Clear();
                    Console.WriteLine("Сожалеем. Ваш балланс опустился ниже нуля. Вы обанкротились.");
                    Console.ReadKey();
                    return false;
                }
                else
                    return true;
            }

            public bool PayFine()
            {
                _wallet -= 100;

                if (_wallet < 0)
                {
                    Console.Clear();
                    Console.WriteLine("Сожалеем. Ваш балланс опустился ниже нуля. Вы обанкротились.");
                    Console.ReadKey();
                    return false;
                }
                else
                    return true;
            }
        }

        class WarehouseDetail
        {
            private List<Detail> _details = new List<Detail>();

            public void Add(Detail detail)
            {
                _details.Add(detail);
            }

            public Detail GetDetail(int detailNumber)
            {
                Detail detail = _details[detailNumber - 1];
                _details.RemoveAt(detailNumber - 1);
                return detail;
            }

            public void DisplayAllDetail()
            {
                Console.Clear();

                if (_details.Count > 0)
                {
                    for (int i = 0; i < _details.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. Наименование детали: {_details[i].Name} Цена: {_details[i].Price}");
                    }
                }
                else
                {
                    Console.WriteLine("Ваш склад пуст");
                }
            }
        }

        class DetailsStore
        {
            private List<Detail> _details = new List<Detail>();

            public DetailsStore()
            {
                _details.Add(new Windshield());
                _details.Add(new Wheel());
                _details.Add(new Bamper());
                _details.Add(new Motor());
                _details.Add(new Hood());
            }


            public void Menu()
            {
                Console.Clear();
                Console.WriteLine("Список деталей в каталоге, доступных для продажи");

                for (int i = 0; i < _details.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {_details[i].Name}   Цена: {_details[i].Price}");
                }

                Console.WriteLine("0. Выйти");
            }

            public Detail SellingDetail(int detailID)
            {
                if (detailID <= _details.Count && detailID > 0)
                {
                    return _details[detailID - 1];
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Такой детали не существует");
                    Console.ReadKey();
                    return null;
                }
            }
        }

        class DailyQueueOfCars
        {
            private Random _random = new Random();
            private List<Car> _cars = new List<Car>();

            public DailyQueueOfCars()
            {
                for (int i = 0; i < _random.Next(10, 15); i++)
                {
                    _cars.Add(new Car());
                }
            }

            public Car ReturnNextCar()
            {
                if (_cars.Count > 0)
                {
                    Car nextCar = _cars[0];
                    _cars.RemoveAt(0);

                    return nextCar;
                }
                else
                {
                    return null;
                }
            }
        }

        class Car
        {
            Random random = new Random();
            private List<Detail> _details = new List<Detail>();

            public Car()
            {
                _details.Add(new Windshield());
                _details.Add(new Wheel());
                _details.Add(new Bamper());
                _details.Add(new Motor());
                _details.Add(new Hood());

                _details[random.Next(0, 5)].Break();
            }

            public int Repair(Detail newDetail)
            {
                foreach (Detail detail in _details)
                {
                    if (newDetail.Name == detail.Name)
                    {
                        if (detail.Broken)
                        {
                            detail.Repair();

                            return newDetail.Price + detail.RepairPrice;
                        }
                        else
                            return -(newDetail.Price + detail.RepairPrice) / 2;
                    }
                }

                return 0;
            }

            public void DisplayListDetails()
            {
                Console.WriteLine("\nСписок деталей его автомобиля:\n");

                foreach (Detail detail in _details)
                {
                    if (!detail.Broken)
                        Console.WriteLine(detail.Name);
                    else
                        Console.WriteLine(detail.BreakageName);
                }
            }
        }

        class Detail
        {
            public int Price { get; protected set; }
            public int RepairPrice { get; protected set; }
            public string Name { get; protected set; }
            public string BreakageName { get; protected set; }
            public bool Broken { get; protected set; }

            public Detail()
            {
                Broken = false;
            }

            public void Break()
            {
                Broken = true;
            }

            public void Repair()
            {
                Broken = false;
            }
        }

        class Windshield : Detail
        {
            public Windshield()
            {
                Name = "Лобовое стекло";
                BreakageName = "Разбитое лобовое стекло";
                RepairPrice = 600;
                Price = 1600;
            }
        }

        class Wheel : Detail
        {
            public Wheel()
            {
                Name = "Колесо";
                Price = 300;
                BreakageName = "Пробитое колесо";
                RepairPrice = 100;
            }
        }

        class Bamper : Detail
        {
            public Bamper()
            {
                Name = "Бампер";
                Price = 500;
                BreakageName = "Треснувший бампер";
                RepairPrice = 200;
            }
        }

        class Motor : Detail
        {
            public Motor()
            {
                Name = "Мотор";
                Price = 10000;
                BreakageName = "Стукнутый двигатель";
                RepairPrice = 2000;
            }
        }

        class Hood : Detail
        {
            public Hood()
            {
                Name = "Капот";
                Price = 2000;
                BreakageName = "Погнутый капот";
                RepairPrice = 500;
            }
        }
    }
}
