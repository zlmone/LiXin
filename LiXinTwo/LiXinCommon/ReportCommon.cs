using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace LiXinCommon
{
	/// <summary>
	/// 报表中使用到的公共方法
	/// </summary>
	public class ReportCommon
	{

		/// <summary>
		/// 计算中位数
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="numberQueue">包含要计算的数据</param>
		/// <returns></returns>
		public static string CalculateMedian<T>(List<T> numberQueue)
		{
			if (numberQueue != null && numberQueue.Count > 0)
			{
				//排序

				numberQueue = numberQueue.Where(P => P.ToString() != "N/A" && P.ToString() != "99999").OrderBy(p => Convert.ToDouble(p)).ToList();
				if (numberQueue.Count > 0)
				{
					var sumcount = numberQueue.Count;
					var result = 0.00;
					//奇偶性
					var oddeven = sumcount % 2;
					//偶
					if (oddeven == 0)
					{
						var first = Convert.ToDouble(numberQueue[sumcount / 2 - 1]);
						var second = Convert.ToDouble(numberQueue[sumcount / 2]);
						result = Math.Round(Convert.ToDouble((first + second) / 2.00), 2, MidpointRounding.AwayFromZero);
					}
					else
					{
						result = Convert.ToDouble(numberQueue[sumcount / 2]);
					}
					return result.ToString();
				}
				else
				{
					return "N/A";
				}
			}
			else
			{
				return "0";
			}
		}


		/// <summary>
		/// 计算中位数返回Double
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="numberQueue">包含要计算的数据</param>
		/// <param name="digits">小数位数</param>
		/// <returns></returns>
		public static double CalculateMedianDouble<T>(List<T> numberQueue, int digits = 2)
		{
            if (numberQueue != null && numberQueue.Any())
			{
				//排序
				numberQueue = numberQueue.Where(p => Convert.ToDouble(p) >= 0).OrderBy(p => Convert.ToDouble(p)).ToList();

				var sumcount = numberQueue == null ? 0 : numberQueue.Count;
				if (sumcount > 0)
				{

					var result = 0.00;
					//奇偶性
					var oddeven = sumcount % 2;
					//偶
					if (oddeven == 0)
					{
						var first = Convert.ToDouble(numberQueue[sumcount / 2 - 1]);
						var second = Convert.ToDouble(numberQueue[sumcount / 2]);
						result = Math.Round(Convert.ToDouble((first + second) / 2.00), digits, MidpointRounding.AwayFromZero);
					}
					else
					{
						result = Convert.ToDouble(numberQueue[sumcount / 2]);
					}
					return result;

				}
				else
				{
					return 0.00;
				}

			}
			else
			{
				return 0.00;
			}
		}

      

	}
}
