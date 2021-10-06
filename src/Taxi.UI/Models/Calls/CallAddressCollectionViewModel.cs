using System.Collections.Generic;

namespace Taxi.UI.Models.Calls
{
	public class CallAddressCollectionViewModel
	{
		public List<CallAddressViewModel> StartAddresses { get; set; }

		public List<CallAddressViewModel> EndAddresses { get; set; }
	}
}
