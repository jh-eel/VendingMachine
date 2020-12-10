using System;
using System.Collections.Generic;

namespace Vending_Machine
{
    class Program
    {
        public static uint Money { get; set; }
        public static uint Buy_amount { get; set; }
        public static uint Buy_item_stock { get; set; }
        public static uint Buy_item_value { get; set; }
        public static string Buy_item_name { get; set; }
        public enum Emenu
        {
            None,
            GetMenu,
            BuyItem,
            SetMoney,
            GetMoney,
            Exit
        }

        static void Main(string[] args)
        {


            // 아이템 선언
            Item apple = new Item(number: 1, value: 500, stock: 50, name: "Apple");
            Item banana = new Item(number: 2, value: 200, stock: 80, name: "Banana");
            Item lemon = new Item(number: 3, value: 700, stock: 20, name: "Lemon");
            Item mikang = new Item(number: 4, value: 800, stock: 100, name: "Mikang");
            Item steak = new Item(number: 5, value: 1000, stock: 10, name: "Steak");

            Console.WriteLine("#################################");
            Console.WriteLine("###### Vending Machine 0.2 ######");
            Console.WriteLine("#################################\n");
            Item.GetMenu();

            //종료전까지 반복 실행
            while (true)
            {
                //제어 메뉴 출력
                Console.WriteLine("1.메뉴 출력  2.주문하기  3.잔액 충전  4.잔액 확인  5.종료 \n");
                Console.Write("실행 명령 번호: \n");
                InputNumber(out uint menu_num);
                Emenu menu_select = (Emenu)menu_num;

                switch (menu_select)
                {
                    case Emenu.GetMenu:
                        Item.GetMenu();
                        break;

                    case Emenu.BuyItem:
                        BuyItem();
                        break;

                    case Emenu.SetMoney:
                        SetMoney();
                        break;

                    case Emenu.GetMoney:
                        Console.WriteLine($"현재 잔액: {Money} 원\n");
                        break;

                    case Emenu.Exit:
                        Console.WriteLine("###########################");
                        Console.WriteLine("######## 또 오세요 ########");
                        Console.WriteLine("###########################");
                        return;

                    default:
                        Console.WriteLine("존재하지 않는 명령 번호입니다.\n");
                        break;
                }

            }
        }

        static void SetMoney()
        {
            Console.WriteLine("\n최대 보유 가능 금액: 4294967295 원\n");
            Console.WriteLine($"현재 잔액: {Money} 원\n");
            Console.Write("충전 금액: ");
            InputNumber(out uint money_charge);
            if (Money <= uint.MaxValue - money_charge)
            {
                Money += money_charge;
            }
            else
            {
                Console.WriteLine("최대 보유 가능 금액을 초과하였습니다.");
            }
            Console.WriteLine($"현재 잔액: {Money} 원\n");

        }

        static void BuyItem()
        {
            Console.Write("구매할 상품 번호: ");
            InputNumber(out uint buy_num);
            if (buy_num == 0)
            {
                return;
            }

            if (buy_num > Item.item_instance.Count)
            {
                Console.WriteLine("존재하지 않는 상품 번호입니다. \n");
                return;
            }

            Console.Write("구매할 상품 개수: ");
            InputNumber(out uint buy_counts);
            if (buy_counts == 0)
            {
                return;
            }

            //생성된 인스턴스 조회
            foreach (var instance in Item.item_instance)
            {
                if (instance.Item_number == buy_num)
                {
                    Buy_amount = instance.Item_value * buy_counts;
                    Buy_item_name = instance.Item_name;
                    Buy_item_value = instance.Item_value;
                    Buy_item_stock = instance.Item_stock;
                    break;
                }
            }

            Console.WriteLine($"총 결제 금액: {Buy_amount}\n");

            if (Money < Buy_amount && Buy_item_stock >= buy_counts) // 잔액이 부족하고, 재고가 충분할 시
            {
                Console.WriteLine("잔액이 부족합니다.");
                Console.WriteLine($"현재 잔액: {Money} 원\n");
            }
            else if (Money >= Buy_amount && Buy_item_stock >= buy_counts) //잔액이 충분하고, 재고가 충분할 시
            {
                Money -= Buy_amount;
                Console.WriteLine("결제 완료\n");
                Console.WriteLine($"구매 상품 명: {Buy_item_name}  구매 수량: {buy_counts}");
                Console.WriteLine($"현재 잔액: {Money} 원\n");

                //재고 차감
                foreach (var instance in Item.item_instance)
                {
                    if (instance.Item_number == buy_num)
                    {
                        instance.Item_stock -= buy_counts;
                        break;
                    }
                }
            }
            else if (Buy_item_stock < buy_counts)
            {
                Console.WriteLine("재고가 부족합니다\n");
            }

            Buy_amount = 0;
            Buy_item_stock = 0;

        }

        static void InputNumber(out uint number)
        {
            bool valueSuccess = uint.TryParse(Console.ReadLine(), out uint input);

            if (valueSuccess)
            {
                number = input;
            }
            else
            {
                Console.WriteLine("\n잘못된 값입니다.\n");
                number = 0;
            }
        }



    }

    public class Item
    {
        static public List<Item> item_instance = new List<Item>();
        static public uint Item_count { get; set; }

        public uint Item_number { get; set; }
        public uint Item_value { get; set; }
        public uint Item_stock { get; set; }
        public string Item_name { get; set; }

        public Item(uint number, uint value, uint stock, string name)
        {
            Item.item_instance.Add(this);
            this.Item_number = number;
            this.Item_value = value;
            this.Item_stock = stock;
            this.Item_name = name;
            Item_count++;
        }

        static public void GetMenu()
        {
            foreach (var instance in Item.item_instance)
            {

                Console.WriteLine($"{instance.Item_number}.{instance.Item_name}  가격: {instance.Item_value}  재고: {instance.Item_stock}");

            }
            Console.WriteLine($"총 상품 가짓수: {Item.Item_count} \n\n");

        }

    }
}
