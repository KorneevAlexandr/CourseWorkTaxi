using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Taxi.BLL.Interfaces.Services;
using Taxi.BLL.ModelsDto;
using Taxi.UI.Data;

namespace Taxi.UI.Initializers
{
	public class PositionDefaultInitializer : IDefaultInitializer
	{
		private readonly string _adminDescription = "Администрирует приложение, полный доступ";
		private readonly string _driverDescription = "Водит автобомиль";
		private readonly string _dispatcherDescription = "Отвечает за вызовы такси";
		private readonly string _mechanicDescription = "Обслуживает автомобили";
		private readonly string _moderatorDescription = "Отвечает за данные в приложении";

		private readonly IPositionService _positionService;

		public PositionDefaultInitializer(IPositionService positionService)
		{
			_positionService = positionService;
		}

		public async Task InitializeAsync()
		{
			var positions = new List<PositionDto>
			{
				new PositionDto { Name = DefaultPositions.Администратор.ToString(), Description = _adminDescription },
				new PositionDto { Name = DefaultPositions.Водитель.ToString(), Description = _driverDescription },
				new PositionDto { Name = DefaultPositions.Диспетчер.ToString(), Description = _dispatcherDescription },
				new PositionDto { Name = DefaultPositions.Механик.ToString(), Description = _mechanicDescription },
				new PositionDto { Name = DefaultPositions.Модератор.ToString(), Description = _moderatorDescription },
			};

			var oldPositions = await _positionService.GetAllAsync();
			var oldPositionNames = oldPositions.Select(position => position.Name);
			foreach (var position in positions)
			{
				if (!oldPositionNames.Contains(position.Name))
				{
					await _positionService.CreateAsync(position); 
				}
			}
		}
	}
}
