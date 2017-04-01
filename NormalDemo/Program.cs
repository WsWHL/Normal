using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NormalDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //初始化要发起的红包基础数据
            double total = 100;
            int num = 50;
            double min = 0.01;
            string temp;
            bool flag = false;
            Console.WriteLine(string.Format("是否需要自定义红包金额和数量（默认{0}元/{1}人）Y/N：", total, num));
            temp = Console.ReadLine();
            if (temp.Trim().ToLower().Equals("y") || temp.Trim().ToLower().Equals("yes"))
            {
                Console.WriteLine("请输入你要发起的红包金额：");
                do
                {
                    temp = Console.ReadLine();
                    flag = double.TryParse(temp, out total);
                    if (!flag)
                    {
                        Console.WriteLine("金额必须为整数或小数，请重新输入：");
                    }
                } while (!flag);
                Console.WriteLine("请输入你要发起的红包个数：");
                do
                {
                    temp = Console.ReadLine();
                    flag = int.TryParse(temp, out num);
                    if (!flag)
                    {
                        Console.WriteLine("红包个数必须为整数，请重新输入：");
                    }
                } while (!flag);
            }

            total -= min * num;
            if (total < 0)
            {
                Console.WriteLine("抱歉，你的金额不足！");
                return;
            }

            //产生正态分布的随机红包金额，并计算相关的金额和数量保证数据的准确性
            double average = total / num;
            double variance = 1;
            Random u1 = new Random();
            Random u2 = new Random();
            double[] nums = new double[num];

            for (int i = 0; i < num; i++)
            {
                double? result = total;
                if (i < num - 1 && total > 0)
                {
                    do
                    {
                        result = Round((double)Normal(u1.NextDouble(), u2.NextDouble(), average, variance), 2);
                    } while (result == null || result < 0);
                    if (total > result)
                    {
                        total = (double)Round((total - (double)result), 2);
                    }
                    else
                    {
                        result = total;
                        total = 0;
                    }
                }
                else if (i == num - 1)
                {
                    total = 0;
                }
                nums[i] = Math.Round(min + (double)result, 2);  //浮点运算问题，这里需要四舍五入数据才正确

                Console.WriteLine(string.Format("第{0}个红包金额：{1}", i + 1, (min + result)));
                Console.WriteLine("剩余金额：" + ((i != num - 1 && total == 0) ? min * (num - i - 1) : total + (min * (num - i - 1))));
            }
            Console.WriteLine("最大金额：" + nums.Max());
            Console.WriteLine("最小金额：" + nums.Min());
            Console.WriteLine("总额：" + Round(nums.Sum(), 2));
            Console.WriteLine("初始方差：" + variance);
            Console.WriteLine("结果方差：" + Variance(nums));
            Console.WriteLine("按任意键退出！");

            Console.ReadKey();
        }

        /// <summary>
        /// 产生符合正态分布的随机数
        /// </summary>
        /// <param name="u1">正态分布第一个随机数</param>
        /// <param name="u2">正态分布第二个随机数</param>
        /// <param name="averageValue">正态期望(平均值)</param>
        /// <param name="variance">正态标准差(Math.Sqrt(方差))</param>
        /// <returns></returns>
        public static double? Normal(double u1, double u2, double averageValue, double variance)
        {
            double? result = null;
            try
            {
                result = averageValue + Math.Sqrt(variance) * Math.Sqrt((-2) * Math.Log(u1)) * Math.Sin(2 * Math.PI * u2);
            }
            catch (Exception)
            {
                result = null;
            }

            return result;
        }

        /// <summary>
        /// 求一组数据的方差
        /// </summary>
        /// <param name="list">要求的数组</param>
        /// <returns></returns>
        public static double Variance(double[] nums)
        {
            double average = nums.Sum() / nums.Length;
            double sum = 0;
            double variance = 0;
            foreach (double num in nums)
            {
                sum += Math.Pow((num - average), 2);
            }
            variance = sum / nums.Length;

            return variance;
        }

        /// <summary>
        /// 截取小数指定小数位，且不四舍五入
        /// </summary>
        /// <param name="originNum">要截取的小数</param>
        /// <param name="lastNum">截取小数后位数</param>
        /// <returns></returns>
        public static double? Round(double originNum, int lastNum)
        {
            double? result = null;
            int index = originNum.ToString().IndexOf('.');
            if (index != -1)
            {
                string temp = originNum.ToString();
                result = Convert.ToDouble(temp.Substring(0, index + 1) + temp.Substring(index + 1, Math.Min(temp.Length - index - 1, lastNum)));
            }
            if (result == 0)
            {
                result = null;
            }
            else if (index == -1)
            {
                result = originNum;
            }

            return result;
        }
    }
}
