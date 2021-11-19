using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Taxi.DAL.Data;
using Taxi.DAL.Domain;
using Taxi.DataInitialization.Interfaces;
using Taxi.DataInitialization.Options;

namespace Taxi.DataInitialization.Initializers
{
	internal class CallInitializer : IInitializer
	{
		private readonly string _belarussinCode = "+375";
		private readonly string[] _codes = new string[] { "29", "33", "44", "25" };

		private readonly string[] _streets = new string[] {
			"Авиационная", "Артиллерийская", "Бакунина", "Барыкина", "Белого", "Белорусская", "Богданова", "Борисенко", "БССР",
			"Бочкина", "Бровки", "Весенняя", "Виноградная", "Витебская", "Владимирова", "Войкова", "Володарского", "Восточная",
			"Гагарина", "Гайдара", "Гастелло", "Гоголя", "Гвоздичная", "Головацкого", "Горная", "Горького", "Давыдовская",
			"Дальняя", "Держинского", "Дорожная", "Дружбы", "Ефремова", "Жарковского", "Жемчужная", "Жукова", "Заводская",
			"Зайцева", "Западная", "Ильича", "Иногородняя", "Иринская", "Карповича", "Катунина", "Кирова", "Кожара",
			"Косарева", "Космическая", "Котовского", "Крестьянская", "Крылова", "Ленина", "Лазурная", "Лизюковых", "Луговая",
			"Ломоносова", "Мазурова", "Макаенка", "Маневича", "Мележа", "Минская", "Нагорная", "Народная", "Набережная",
			"Няжняя", "Никольская", "Объездная", "Огоренко", "Октября", "Олимпийская", "Осипова", "Оськина", "Песина",
			"Петровская", "Полесская", "Полякова", "Привокзальная", "Прозрачная", "Рабочая", "Речная", "Рогачевская", "Ридного",
			"Романовская", "Революционная", "Свердлова", "Советская", "Спартака", "СССР", "Сосновая", "Техническая", "Трудовая",
			"Троллейбусная", "Тельмана", "Тенистая", "Тимофеенко", "Толстого", "Украинская", "Урицкого", "Урожайная", "Уткина",
			"Фадеева", "Федосеенко", "Федюнинского", "Фрунзе", "Фурманова", "Хатаевича", "Химакова", "Химическая", "Чехова",
			"Чечерская", "Шамякина", "Шевченко", "Энгельса", "Юбилейная", "Южная", "Яговкина", "Якубова", "Ярославская" };

		private readonly TaxiContext _context;
		private readonly int _count;
		private readonly Random _random;

		private List<int> _carIds;
		private List<int> _dispatcherIds;

		public CallInitializer(TaxiContext context)
		{
			_context = context;
			_count = DataInitializeOptions.CountCalls;
			_random = new Random();
		}

		public bool Initialize { get => DataInitializeOptions.Initialize; }

		public async Task InitializeAsync()
		{
			if (!Initialize || _count <= 0)
			{
				return;
			}

			await GetIds();

			var calls = new List<Call>();
			for (int i = 0; i < _count; i++)
			{
				calls.Add(GenerateCall());
			}
			await _context.Calls.AddRangeAsync(calls);
			await _context.SaveChangesAsync();

			await Task.CompletedTask;
		}

		private async Task GetIds()
		{
			var dispatcherPosition = await _context.Positions.FirstOrDefaultAsync(position => position.Name.Equals(Positions.Диспетчер.ToString()));
			_dispatcherIds = await _context.Employees.Where(employee => employee.PositionId == dispatcherPosition.Id)
				.Select(dispatcher => dispatcher.Id).ToListAsync();

			_carIds = await _context.Cars.Select(car => car.Id).ToListAsync();
		}

		private Call GenerateCall()
		{
			var minYear = 2018;

			var carId = _carIds[_random.Next(0, _carIds.Count)];
			var dispatcherId = _dispatcherIds[_random.Next(0, _dispatcherIds.Count)];
			var phone = GeneratePhoneNumber();

			var startStreet = _streets[_random.Next(0, _streets.Length)];
			var endStreet = _streets[_random.Next(0, _streets.Length)];
			while (startStreet.Equals(endStreet))
			{
				endStreet = _streets[_random.Next(0, _streets.Length)];
			}
			var startHome = _random.Next(1, 301);
			var endHome = _random.Next(1, 301);
			var dateTime = new DateTime(
				_random.Next(minYear, DateTime.Now.Year + 1), _random.Next(1, 13), _random.Next(1, 29), 
				_random.Next(0, 24), _random.Next(0, 60), 0);
			var price = _random.Next(100, 1001);

			return new Call
			{
				DispatherId = dispatcherId,
				CallDateTime = dateTime,
				CarId = carId,
				Phone = phone,
				StartStreet = startStreet,
				EndStreet = endStreet,
				StartHomeNumber = startHome,
				EndHomeNumber = endHome,
				Price = price,
			};
		}

		private string GeneratePhoneNumber()
		{
			var code = string.Concat("(", _codes[_random.Next(0, _codes.Length)], ")");
			var number = _random.Next(1000000, 10000000);
			return string.Concat(_belarussinCode, code, number);
		}

		private enum Positions : byte
		{
			Диспетчер
		}
	}
}
