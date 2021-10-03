using System.Collections.Generic;
using Taxi.UI.Models.Brands;

namespace Taxi.UI.Models.CarModels
{
	public class ModelCollectionViewModel
	{
		/// <summary>
		/// Выбранный Id марки
		/// </summary>
		public int Id { get; set; }

		public List<BrandViewModel> Brands { get; set; }

		public List<ModelViewModel> Models { get; set; }
	}
}
