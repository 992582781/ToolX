using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic;
using System.Dynamic;
using System.Reflection;


namespace LinqX
{
    public class InitData
    {
        public List<House> Init()
        {
            List<House> list = new List<House>();
            list.Add(new House
            {
                Name = "张三",
                Month = "2016-01",
                Area = "江夏区",
                DfMoney = 240.9,
                SfMoney = 30,
                RqfMoney = 25
            });

            list.Add(new House
            {
                Name = "张三",
                Month = "2016-02",
                Area = "江夏区",
                DfMoney = 167,
                SfMoney = 24.5,
                RqfMoney = 17.9
            });

            list.Add(new House
            {
                Name = "小燕子",
                Month = "2016-01",
                Area = "江夏区",
                DfMoney = 340.9,
                SfMoney = 20,
                RqfMoney = 55
            });

            list.Add(new House
            {
                Name = "小燕子",
                Month = "2016-02",
                Area = "江夏区",
                DfMoney = 67,
                SfMoney = 64.5,
                RqfMoney = 77.9
            });

            list.Add(new House
            {
                Name = "李四",
                Month = "2016-01",
                Area = "洪山区",
                DfMoney = 56.7,
                SfMoney = 24.7,
                RqfMoney = 13.2
            });

            list.Add(new House
            {
                Name = "李四",
                Month = "2016-02",
                Area = "洪山区",
                DfMoney = 65.2,
                SfMoney = 18.9,
                RqfMoney = 14.9
            });

            list.Add(new House
            {
                Name = "尔康",
                Month = "2016-01",
                Area = "洪山区",
                DfMoney = 156.7,
                SfMoney = 124.7,
                RqfMoney = 33.2
            });

            list.Add(new House
            {
                Name = "尔康",
                Month = "2016-02",
                Area = "洪山区",
                DfMoney = 35.2,
                SfMoney = 28.9,
                RqfMoney = 44.9
            });
            return list;
        }

        public void operat()
        {
            var Dimensionstr = "Area,Month";
            List<string> DimensionList = Dimensionstr.Split(new char[] { ',' },
                StringSplitOptions.RemoveEmptyEntries).ToList();

            var list = Init();
            //List<string> DimensionList = new List<string>() { "Area", "Month" };
            //字符串传入排序
            var lst = list.OrderBy("Month ,RqfMoney ").ToList();
            foreach (var item in lst)
            {
                Console.WriteLine("name:{0}, Id:{1}", item.Month, item.RqfMoney);
            }

            Console.WriteLine("---------------------------");


            var dataGroup = list.GroupBy(string.Format("new ({0})", string.Join(",", DimensionList)),
                    "new(it as Vm)") as IEnumerable<IGrouping<dynamic, dynamic>>;

            foreach (var group in dataGroup)
            {
                Console.WriteLine(group.Key);
                var listVm = group.Select(e => e.Vm as House).ToList();

                foreach (var item in listVm)
                {
                    Console.WriteLine($"\t{item.Month},{item.RqfMoney}");
                }
                Console.WriteLine("**********************");
            }

            Console.WriteLine("------------------------------------------");


            foreach (var group in dataGroup)
            {
                Console.WriteLine(group.Key);
                foreach (var item in group)
                {
                    Console.WriteLine($"\t{item.Vm.Month},{item.Vm.RqfMoney}");
                }
                Console.WriteLine("**********************");
            }

            Console.WriteLine("------------------------------------------");


            try
            {
                var query =
                   list.Where("Name == @0 and Area == @1", "张三", "江夏区").
                   OrderBy("Month ,RqfMoney ").
                   Select(string.Format("new ({0})", string.Join(",", DimensionList)));
                //Select("New(Area, Month)");
                Console.WriteLine("query -->>>:" + query.ToString());

                foreach (dynamic item in query)
                {
                    Console.WriteLine($"\t{item.Area},{item.Month}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error -->>>:" + ex.ToString());
            }



            try
            {
                var query =
                   list.Where("Name == @0 and RqfMoney+1 == @1 and address.addr.Contains(@2) ", "张三", 26, "6").
                   OrderBy("Month ,RqfMoney ").
                   Select(string.Format("new ({0})", string.Join(",", DimensionList)));

                query =
                 list.Where("Name==\"张三\"" +
                 "and RqfMoney+1 == 26 " +
                 "and (Listfamily.Where(FamilyName == \"张4\").Count() > 0) ").
                 OrderBy("Month ,RqfMoney ").
                 Select(string.Format("new ({0})", string.Join(",", DimensionList)));

                query =
                  list.Where("Name ==@0 and RqfMoney+1 == @1  and address.addr.Contains(@2) and (Listfamily.Where(FamilyName == @3).Count() > 0) ",
                "张三", 26, "6", "张4").
                OrderBy("Month ,RqfMoney ").
                Select(string.Format("new ({0})", string.Join(",", DimensionList)));

                Console.WriteLine("query -->>>:" + query.ToString());

                foreach (dynamic item in query)
                {
                    Console.WriteLine($"\t{item.Area},{item.Month}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error -->>>:" + ex.ToString());
            }


            Console.Read();
        }
    }
}
