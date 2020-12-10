using System;
using System.Collections.Generic;

namespace Vending_Machine
{
    class Program
    {
        static void Main(string[] args)
        {
            int money = 0;
            int menu_num;
            int buy_num;
            int buy_counts;
            int buy_amount = 0;
            int buy_item_stock = 0;
            int buy_item_value;
            string buy_item_name = "";

            // 아이템 선언
            Item apple = new Item(number: 1, value: 500, stock: 50, name: "Apple");
            Item banana = new Item(number: 2, value: 200, stock: 80, name: "Banana");
            Item lemon = new Item(number: 3, value: 700, stock: 20, name: "Lemon");
            Item mikang = new Item(number: 4, value: 800, stock: 100, name: "Mikang");

            Console.WriteLine("#################################");
            Console.WriteLine("###### Vending Machine 0.1 ######");
            Console.WriteLine("#################################\n");
            Item.GetMenu();

            //종료전까지 반복 실행
            while (true)
            {
                //제어 메뉴 출력
                Console.WriteLine("1.메뉴 출력  2.주문하기  3.잔액 충전  4.잔액 확인  5.종료 \n");
                Console.Write("실행 명령 번호: ");
                menu_num = int.Parse(Console.ReadLine());
                Console.WriteLine("");

                if (menu_num == 1)
                {
                    Item.GetMenu();
                }
                else if (menu_num == 2)
                {
                    BuyItem();
                }
                else if (menu_num == 3)
                {
                    SetMoney();
                }
                else if (menu_num == 4)
                {
                    Console.WriteLine($"현재 잔액: {money} 원\n");
                }
                else if (menu_num == 5)
                {
                    Console.WriteLine("###########################");
                    Console.WriteLine("######## 또 오세요 ########");
                    Console.WriteLine("###########################");
                    break;
                }
                else
                {
                    Console.WriteLine("올바르지 않은 값입니다.");
                }

            }

            void SetMoney()
            {
                Console.Write("충전 금액: ");
                money += int.Parse(Console.ReadLine());
                Console.WriteLine($"현재 잔액: {money} 원\n");
            }

            void BuyItem()
            {
                Console.Write("구매할 상품 번호: ");
                buy_num = int.Parse(Console.ReadLine());
                Console.Write("구매할 상품 개수: ");
                buy_counts = int.Parse(Console.ReadLine());

                //생성된 인스턴스 조회
                foreach (var instance in Item.item_instance)
                {
                    if (instance.item_number == buy_num)
                    {
                        buy_amount = instance.item_value * buy_counts;
                        buy_item_name = instance.item_name;
                        buy_item_value = instance.item_value;
                        buy_item_stock = instance.item_stock;
                        break;
                    }
                }

                Console.WriteLine($"총 결제 금액: {buy_amount}\n");

                if (money < buy_amount && buy_item_stock >= buy_counts) // 잔액이 부족하고, 재고가 충분할 시
                {
                    Console.WriteLine("잔액이 부족합니다.");
                    Console.WriteLine($"현재 잔액: {money} 원\n");
                }
                else if (money >= buy_amount && buy_item_stock >= buy_counts) //잔액이 충분하고, 재고가 충분할 시
                {
                    money -= buy_amount;
                    Console.WriteLine("결제 완료\n");
                    Console.WriteLine($"구매 상품 명: {buy_item_name}  구매 수량: {buy_counts}");
                    Console.WriteLine($"현재 잔액: {money} 원\n");

                    //재고 차감
                    foreach (var instance in Item.item_instance)
                    {
                        if (instance.item_number == buy_num)
                        {
                            instance.item_stock -= buy_counts;
                            break;
                        }
                    }
                }
                else if (buy_item_stock < buy_counts)
                {
                    Console.WriteLine("재고가 부족하거나 잘못된 상품 번호입니다.\n");
                }

                buy_amount = 0;
                buy_item_stock = 0;

            }

        }

    }

    public class Item
    {
        static public List<Item> item_instance = new List<Item>();
        static public int item_count { get; set; }

        public int item_number { get; set; }
        public int item_value { get; set; }
        public int item_stock { get; set; }
        public string item_name { get; set; }

        public Item(int number, int value, int stock, string name)
        {
            Item.item_instance.Add(this);
            this.item_number = number;
            this.item_value = value;
            this.item_stock = stock;
            this.item_name = name;
            item_count++;
        }

        static public void GetMenu()
        {
            foreach (var instance in Item.item_instance)
            {

                Console.WriteLine($"{instance.item_number}.{instance.item_name}  가격: {instance.item_value}  재고: {instance.item_stock}");

            }
            Console.WriteLine($"총 상품 가짓수: {Item.item_count} \n\n");

        }

    }
}
