using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Taxi.UI.Models.CarModels
{
	public class FuelViewModel
	{
		public int Id { get; set; }

		public string Name { get; set; }
	}

	public class FuelsData
	{
		private static readonly List<string> _fuels = new List<string>
		{
			"Пропан-бутан",
			"Бензин АИ-80",
			"Бензин АИ-92",
			"Бензин АИ-95",
			"Бензин АИ-95+",
			"Бензин АИ-98",
			"Бензин АИ-100",
			"Дизель",
			"Евродизель",
			"Биодизель",
			"Водород",
			"Электро"
		};

		public static List<FuelViewModel> Fuels 
		{
			get
			{
				var fuels = new List<FuelViewModel>();
				for (int i = 1; i <= _fuels.Count; i++)
				{
					fuels.Add(new FuelViewModel { Id = i, Name = _fuels[i - 1], });
				}
				return fuels;
			}
		}

		public static string GetFuelName(int id)
		{
			id = id == 0 ? 1 : id;
			return _fuels[id - 1];
		}

		public static int GetFuelId(string name)
		{
			var index = _fuels.IndexOf(name);
			return index == -1 ? 1 : index + 1;
		}
	}
}
