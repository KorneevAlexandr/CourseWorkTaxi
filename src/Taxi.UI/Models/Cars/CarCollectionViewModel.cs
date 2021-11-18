using System.Collections.Generic;
using Taxi.UI.Models.Brands;

namespace Taxi.UI.Models.Cars
{
	public class CarCollectionViewModel
	{
		public int BrandId { get; set; }

		public int Mileage { get; set; }

		public int IssueYear { get; set; }

		public int Price { get; set; }

		public int CountPages { get; set; }

		public int CurrentPage { get; set; }

		public List<BrandViewModel> Brands { get; set; }

		public List<CarViewModel> Cars { get; set; }
	}
}
