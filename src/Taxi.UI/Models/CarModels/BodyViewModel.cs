using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Taxi.UI.Models.CarModels
{
	public class BodyViewModel
	{
		public int Id { get; set; }

		public string Name { get; set; }
	}

	public class BodysData
	{
		private static readonly List<string> _bodys = new List<string>
		{
			"Седан",
			"Универсал",
			"Купе",
			"Минивен",
			"Внедорожник 3 дв.",
			"Внедорожник 5 дв.",
			"Кабриолет",
			"Легковой фургон",
			"Лимузин",
			"Лифтбек",
			"Микроавтобус",
			"Пикап",
			"Хэтчбек 3 дв.",
			"Хэтчбек 5 дв.",
		};

		public static List<BodyViewModel> Bodys
		{
			get
			{
				var bodys = new List<BodyViewModel>();
				for (int i = 1; i <= _bodys.Count; i++)
				{
					bodys.Add(new BodyViewModel { Id = i, Name = _bodys[i - 1] } );
				}
				return bodys;
			}
		}

		public static string GetBodyName(int id)
		{
			id = id == 0 ? 1 : id;
			return _bodys[id - 1];
		}

		public static int GetBodyId(string name)
		{
			var index = _bodys.IndexOf(name);
			return index == -1 ? 1 : index + 1;
		}
	}
}
