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
            CarService carService = new CarService();
            Car car = new Car();
            Catalog catalog = new Catalog();
            int userInput = -1;
            int userChoice;

            while (userInput != 0)
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
                        car.ReceptionNextCar();
                        Console.Clear();
                        Console.WriteLine("Новый клиент:");
                        car.DisplayBreakageName();
                        Console.WriteLine("Цена за работу - " + car.ReturnRepairPrice());
                        Console.WriteLine();
                        Console.WriteLine("1. Выбрать деталь для замены");
                        Console.WriteLine("2. Отказатся ремонтировать");
                        userChoice = Convert.ToInt32(Console.ReadLine());

                        if(userChoice == 1)
                        {
                            carService.DisplayWarehouseDetail();
                            int DetailNumber = Convert.ToInt32(Console.ReadLine());
                            userInput = carService.TakeIncome(car.Repair(carService.Repair(DetailNumber)));
                        }
                        else if (userChoice == 2)
                        {
                            Console.Clear();
                            userInput = carService.PayFine();
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
                        catalog.Menu();
                        userChoice = Convert.ToInt32(Console.ReadLine());
                        Detail detail = catalog.SellingDetail(userChoice);

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
                    _warehouseDetail.TakeDetail(detail);
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
                return _warehouseDetail.GiveDetail(detailNumber);
            }

            public int TakeIncome(int countIncome)
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
                    return 0;
                }
                else
                    return -1;
            }

            public int PayFine()
            {
                _wallet -= 100;

                if (_wallet < 0)
                {
                    Console.Clear();
                    Console.WriteLine("Сожалеем. Ваш балланс опустился ниже нуля. Вы обанкротились.");
                    Console.ReadKey();
                    return 0;
                }
                else
                    return -1;
            }
        }

        class WarehouseDetail
        {
            private List<Detail> _details = new List<Detail>();

            public void TakeDetail(Detail detail)
            {
                _details.Add(detail);
            }

            public Detail GiveDetail(int detailNumber)
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
                    for(int i = 0; i < _details.Count; i++)
                    {
                        Console.WriteLine($"{i+1}. Наименование детали: {_details[i].Name} Цена: {_details[i].Price}");
                    }
                }
                else
                {
                    Console.WriteLine("Ваш склад пуст");
                }
            }
        }

        class Catalog
        {
            private Windshield _windshield = new Windshield();
            private Wheel _wheel = new Wheel();
            private Bamper _bamper = new Bamper();
            private Motor _motor = new Motor();
            private Hood _hood = new Hood();

            public void Menu()
            {
                Console.Clear();
                Console.WriteLine("Список деталей в каталоге, доступных для продажи");
                Console.WriteLine($"1. {_windshield.Name}   Цена: {_windshield.Price}");
                Console.WriteLine($"2. {_wheel.Name}   Цена: {_wheel.Price}");
                Console.WriteLine($"3. {_bamper.Name}   Цена: {_bamper.Price}");
                Console.WriteLine($"4. {_motor.Name}   Цена: {_motor.Price}");
                Console.WriteLine($"5. {_hood.Name}   Цена: {_hood.Price}");
                Console.WriteLine("0. Выйти");
            }

            public Detail SellingDetail(int detailID)
            {
                switch (detailID)
                {
                    case 1:
                        _windshield = new Windshield();
                        return _windshield;
                        break;
                    case 2:
                        _wheel = new Wheel();
                        return _wheel;
                        break;
                    case 3:
                        _bamper = new Bamper();
                        return _bamper;
                        break;
                    case 4:
                        _motor = new Motor();
                        return _motor;
                        break;
                    case 5:
                        _hood = new Hood();
                        return _hood;
                        break;
                    case 0:
                        return null;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Такой детали не существует");
                        Console.ReadKey();
                        return null;
                        break;
                }
            }
        }

        class Car
        {
            Random random = new Random();
            private Breakage _breakage;

            public void ReceptionNextCar()
            {
                int breakageID = random.Next(0, 5);

                switch (breakageID)
                {
                    case 1:
                        _breakage = new BreakageWindshield();
                        break;
                    case 2:
                        _breakage = new BreakageWheel();
                        break;
                    case 3:
                        _breakage = new BreakageBamper();
                        break;
                    case 4:
                        _breakage = new BreakageMotor();
                        break;
                    case 5:
                        _breakage = new BreakageHood();
                        break;
                }
            }

            public int Repair(Detail detail)
            {
                if (detail.Name == _breakage.NameRequiredDetail)
                    return detail.Price + _breakage.RepairPrice;
                else
                    return -(detail.Price + _breakage.RepairPrice)/2;
            }

            public void DisplayBreakageName()
            {
                Console.WriteLine("Поломка - " + _breakage.Name);
            }

            public int ReturnRepairPrice()
            {
                return _breakage.RepairPrice;
            }
        }

        class Breakage
        {
            public string Name { get; protected set; }
            public string NameRequiredDetail { get; protected set; }
            public int RepairPrice { get; protected set; }
        }

        class BreakageWindshield : Breakage
        {
            public BreakageWindshield()
            {
                Name = "Разбитое лобовое стекло";
                NameRequiredDetail = "Лобовое стекло";
                RepairPrice = 600;
            }
        }

        class BreakageWheel : Breakage
        {
            public BreakageWheel()
            {
                Name = "Пробитое колесо";
                NameRequiredDetail = "Колесо";
                RepairPrice = 100;
            }
        }

        class BreakageBamper : Breakage
        {
            public BreakageBamper()
            {
                Name = "Треснувший бампер";
                NameRequiredDetail = "Бампер";
                RepairPrice = 200;
            }
        }

        class BreakageMotor : Breakage
        {
            public BreakageMotor()
            {
                Name = "Стукнутый двигатель";
                NameRequiredDetail = "Мотор";
                RepairPrice = 2000;
            }
        }

        class BreakageHood : Breakage
        {
            public BreakageHood()
            {
                Name = "Погнутый капот";
                NameRequiredDetail = "Капот";
                RepairPrice = 500;
            }
        }

        class Detail
        {
            public int Price { get; protected set; }
            public string Name { get; protected set; }
        }

        class Windshield : Detail
        {
            public Windshield()
            {
                Name = "Лобовое стекло";
                Price = 1600;
            }
        }

        class Wheel : Detail
        {
            public Wheel()
            {
                Name = "Колесо";
                Price = 300;
            }
        }

        class Bamper : Detail
        {
            public Bamper()
            {
                Name = "Бампер";
                Price = 500;
            }
        }

        class Motor : Detail
        {
            public Motor()
            {
                Name = "Мотор";
                Price = 10000;
            }
        }

        class Hood : Detail
        {
            public Hood()
            {
                Name = "Капот";
                Price = 2000;
            }
        }
    }
}
