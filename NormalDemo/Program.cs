using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NormalDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            double total = 100;
            double min = 0.01;
            int num = 100;
            total -= min * num;
            double average = total / num;
            double variance = 1;
            Random u1 = new Random();
            Random u2 = new Random();
            double[] nums = new double[num];

            for (int i = 0; i < num; i++)
            {
                double? result = total;
                if (i < num - 1)
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
                nums[i] = min + (double)result;

                Console.WriteLine(string.Format("第{0}个红包金额：{1}", i + 1, (min + result)));
                Console.WriteLine("剩余金额：" + total);
            }
            Console.WriteLine("最大金额：" + nums.Max());
            Console.WriteLine("最小金额：" + nums.Min());
            Console.WriteLine("总额：" + nums.Sum());
            Console.WriteLine("初始方差：" + variance);
            Console.WriteLine("结果方差：" + Variance(nums));

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
                result = averageValue + Math.Sqrt(variance) * Math.Sqrt((-2) * Math.Log(Math.E) * Math.Log(u1)) * Math.Sin(2 * Math.PI * u2);
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
