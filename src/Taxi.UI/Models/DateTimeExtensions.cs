using System;

namespace Taxi.UI.Models
{
	public static class DateTimeExtensions
	{
		private static readonly string _minDate = "2000-01-01";

		public static string MinDate { get => _minDate; }

		public static string MaxDate
		{
			get
			{
				var date = DateTime.Now;
				return date.GetHtmlInputFormat();
			}
		}

		public static string GetHtmlInputFormat(this DateTime date)
		{
			var year = date.Year.ToString();
			var month = GetValidDateItem(date.Month);
			var day = GetValidDateItem(date.Day);

			return string.Concat(year, "-", month, "-", day);
		}

		private static string GetValidDateItem(int item)
		{
			if (item < 10)
			{
				return string.Concat("0", item);
			}
			return item.ToString();
		}
	}
}
