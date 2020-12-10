using System;
using System.Collections.Generic;
using System.Text;

namespace Vending_Machine
{
    public class Program
    {
        public static int Money { get; set; }
        public static int Buy_amount { get; set; }
        public static int Buy_item_stock { get; set; }
        public static int Buy_item_value { get; set; }
        public static string Buy_item_name { get; set; }
        public static List<Item> Items = new List<Item>();
        public static StringBuilder history = new StringBuilder(1024);


        public enum Emenu
        {
            None,
            GetMenu,
            BuyItem,
            SetMoney,
            GetMoney,
            AdminLogin,
            History,
            Exit
        }

        static void Main()
        {

            Items.Add(new Item(value: 500, stock: 50, name: "Apple"));
            Items.Add(new Item(value: 200, stock: 80, name: "Banana"));
            Items.Add(new Item(value: 700, stock: 20, name: "Lemon"));
            Items.Add(new Item(value: 800, stock: 100, name: "Mikang"));
            Items.Add(new Item(value: 1000, stock: 10, name: "Steak"));

            Console.WriteLine("#################################");
            Console.WriteLine("###### Vending Machine 0.3 ######");
            Console.WriteLine("#################################\n");
            Item.GetItems();

            //종료전까지 반복 실행
            while (true)
            {
                //제어 메뉴 출력
                Console.WriteLine("1.Show Items  2.Order  3.Charge Money  4.Check Money  5.Admin Login  6.Hsitory 7.Exit \n");
                Console.Write(" \nExecute Order Number: ");
                InputPlusNumber(out int menu_num);
                Emenu menu_select = (Emenu)menu_num;

                switch (menu_select)
                {
                    case Emenu.GetMenu:
                        Item.GetItems();
                        break;

                    case Emenu.BuyItem:
                        BuyItem();
                        break;

                    case Emenu.SetMoney:
                        SetMoney();
                        break;

                    case Emenu.GetMoney:
                        Console.WriteLine($"NOW Money: {Money} \n");
                        break;

                    case Emenu.AdminLogin:
                        Admin.Login();
                        break;

                    case Emenu.History:
                        ShowHistory();
                        break;

                    case Emenu.Exit:
                        Console.WriteLine("###########################");
                        Console.WriteLine("########  BYE BYE  ########");
                        Console.WriteLine("###########################");
                        return;

                    default:
                        Console.WriteLine("It is wrong number\n");
                        break;
                }
            }
        }

        static void ShowHistory()
        {
            Console.WriteLine(history);
        }

        static void SetMoney()
        {
            Console.WriteLine($"\nMax: {int.MaxValue}\n");
            Console.WriteLine($"Current amount: {Money} \n");
            Console.WriteLine($"Rechargeable amount: {int.MaxValue - Money}");
            Console.Write("Charge amount: ");
            InputPlusNumber(out int money_charge);
            if (Money <= int.MaxValue - money_charge)
            {
                Money += money_charge;
            }
            else
            {
                Console.WriteLine("The maximum has been exceeded");
            }
            Console.WriteLine($"Current amount: {Money} \n");

        }

        static void BuyItem()
        {
            Console.Write("The product number to be purchased: ");
            InputPlusNumber(out int buy_num);
            buy_num--;

            if (buy_num > Items.Count)
            {
                Console.WriteLine("This product does not exist \n");
                return;
            }

            Console.Write("The number of products to purchase: ");
            InputPlusNumber(out int buy_counts);
            if (buy_counts == 0)
            {
                Console.WriteLine("The minimum number of purchases is 1. \n");
                return;
            }

            Buy_amount = Items[buy_num].Item_value * buy_counts;
            Buy_item_name = Items[buy_num].Item_name;
            Buy_item_stock = Items[buy_num].Item_stock;

            Console.WriteLine($"Total payment amount: {Buy_amount}\n");

            if (Money < Buy_amount && Buy_item_stock >= buy_counts)
            {
                Console.WriteLine("Not enough money.");
                Console.WriteLine($"Current money: {Money} \n");
            }
            else if (Money >= Buy_amount && Buy_item_stock >= buy_counts)
            {
                Money -= Buy_amount;
                string messages = $"Purchased product name: {Buy_item_name}  Number of purchases: {buy_counts}";
                Console.WriteLine("Payment finished\n");
                Console.WriteLine(messages);
                Console.WriteLine($"Current money: {Money} \n");
                Items[buy_num].Item_stock -= buy_counts;
                history.AppendLine(messages);
                history.AppendLine($"Amount: {Buy_amount}\n");

            }
            else if (Buy_item_stock < buy_counts)
            {
                Console.WriteLine("Out of stock\n");
            }
            Buy_amount = 0;
            Buy_item_stock = 0;
        }

        public static void InputPlusNumber(out int number)
        {
            bool valueSuccess = int.TryParse(Console.ReadLine(), out int input);

            if (valueSuccess && input > 0)
            {
                number = input;
            }
            else
            {
                Console.WriteLine("\nInvaild value\n");
                number = 0;
            }
        }
    }

    public class Item
    {
        public int Item_value { get; set; }
        public int Item_stock { get; set; }
        public string Item_name { get; set; }

        public Item(int value, int stock, string name)
        {
            this.Item_value = value;
            this.Item_stock = stock;
            this.Item_name = name;
        }

        static public void GetItems()
        {
            Console.WriteLine();
            int index = 1;
            foreach (var i in Program.Items)
            {
                Console.WriteLine($" {index}.{i.Item_name}  Value: {i.Item_value}  Stock: {i.Item_stock}");
                index++;
            }
            Console.WriteLine($"\n Total number of products: {Program.Items.Count}\n");
        }

    }

    public static class Admin
    {
        private static int Pwd = 1234;
        private static bool Permission = false;

        private enum Menu
        {
            Err,
            Restock,
            AddItem,
            DeleteItem,
            EditItem,
            ChangePwd,
            Logout
        }

        public static void Login()
        {
            CheckPermission();
            if (Permission == true)
            {
                while (Permission == true)
                {
                    Console.WriteLine("1.Restock  2.Add new item  3.Delete item  4.Edit item info  5.Change password  6.Logout \n");
                    Console.Write("\nExecute Order Number: ");
                    Program.InputPlusNumber(out int AdminMenuInput);
                    Menu Emenu = (Menu)AdminMenuInput;

                    switch (Emenu)
                    {
                        case Menu.Restock:
                            Restock();
                            break;

                        case Menu.AddItem:
                            AddItem();
                            break;

                        case Menu.DeleteItem:
                            DeleteItem();
                            break;

                        case Menu.EditItem:
                            EditItem();
                            break;

                        case Menu.ChangePwd:
                            ChangePwd();
                            break;

                        case Menu.Logout:
                            Permission = false;
                            Console.WriteLine("\n\n###########################");
                            Console.WriteLine("##### Logout Success ######");
                            Console.WriteLine("###########################\n\n");
                            break;

                        case Menu.Err:
                            Console.WriteLine("Invaild order number!\n");
                            break;
                    }

                }
            }

        }


        public static void CheckPermission()
        {
            Console.Write("\nAdmin password: ");
            Program.InputPlusNumber(out int inputPwd);
            if (inputPwd == Pwd)
            {
                Console.WriteLine("\n\n###########################");
                Console.WriteLine("### Admin Login Success ###");
                Console.WriteLine("###########################\n\n");
                Permission = true;
            }
            else if (inputPwd != 0 && inputPwd != Pwd)
            {
                Console.WriteLine("\nIt's worng password!\n");
            }
        }

        public static void Restock()
        {
            Item.GetItems();

            Console.Write("Product number to fill stock: ");
            Program.InputPlusNumber(out int inputNum);

            if (inputNum == 0 || inputNum > Program.Items.Count)
            {
                Console.WriteLine("This product does not exist.\n");
                return;
            }

            inputNum--;

            Console.Write("Number of restock: ");
            Program.InputPlusNumber(out int inputCount);
            if (inputCount == 0)
            {
                Console.WriteLine("minimum is 1 \n");
                return;
            }

            Program.Items[inputNum].Item_stock += inputCount;
            Console.WriteLine("Complete stock filling\n");
            Console.WriteLine($"{Program.Items[inputNum].Item_name}'s stock: {Program.Items[inputNum].Item_stock}\n");

        }

        public static void AddItem()
        {
            string name;

            Item.GetItems();
            Console.Write("Product name to be added: ");
            name = Console.ReadLine();

            Console.Write("Product value to be added: ");
            Program.InputPlusNumber(out int value);
            if (value == 0)
            {
                Console.WriteLine("minimum is 1.\n");
                return;
            }

            Console.Write("Product stock to be added: ");
            Program.InputPlusNumber(out int stock);
            if (stock == 0)
            {
                Console.WriteLine("minimum is 1.\n");
                return;
            }

            Program.Items.Add(new Item(value: value, stock: stock, name: name));
        }

        public static void EditItem()
        {
            string name;

            Item.GetItems();

            Console.Write("Product number to be edited: ");
            Program.InputPlusNumber(out int InputNumber);
            InputNumber--;

            if (InputNumber == 0)
            {
                Console.WriteLine("This product does not exist.\n");
                return;
            }

            Console.Write("New item name: ");
            name = Console.ReadLine();

            Console.Write("New item value: ");
            Program.InputPlusNumber(out int value);
            if (value == 0)
            {
                Console.WriteLine("minimum is 1.\n");
                return;
            }

            Console.Write("New item stock: ");
            Program.InputPlusNumber(out int stock);
            if (stock == 0)
            {
                Console.WriteLine("minimum is 1.\n");
                return;
            }
            Program.Items[InputNumber].Item_name = name;
            Program.Items[InputNumber].Item_value = value;
            Program.Items[InputNumber].Item_stock = stock;
        }

        public static void DeleteItem()
        {
            Item.GetItems();
            Console.Write("Product number to be deleted: ");
            Program.InputPlusNumber(out int InputNumber);
            InputNumber--;

            if (InputNumber > Program.Items.Count)
            {
                Console.WriteLine("Wrong number\n");
                return;
            }

            Program.Items.RemoveAt(InputNumber);
        }

        public static void ChangePwd()
        {

            Console.WriteLine("You can use only number value for password\n");
            Console.Write("New Password: ");
            Program.InputPlusNumber(out int newPwd);
            if (newPwd == 0)
            {
                Console.WriteLine("Can't use value for password");
                return;
            }
            Pwd = newPwd;
            Console.WriteLine("Change Success!\n");
        }
    }
}
