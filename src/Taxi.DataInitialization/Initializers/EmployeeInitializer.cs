using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taxi.DAL.Data;
using Taxi.DAL.Domain;
using Taxi.DataInitialization.Interfaces;
using Taxi.DataInitialization.Options;

namespace Taxi.DataInitialization.Initializers
{
	internal class EmployeeInitializer : IInitializer
	{
		private readonly string[] _manNames = new string[] { 
			"Александр", "Иван", "Сергей", "Дмитрий", "Олег", "Денис", "Никита", "Вадим",
			"Владимир", "Игнат", "Виталий", "Виктор", "Даниил", "Игорь", "Егор", "Алексей",
			"Владислав", "Андрей", "Илья", "Максим", "Кирилл", "Тимофей", "Константин", "Валентин" };
		private readonly string[] _womenNames = new string[] {
			"Ирина", "Анастасия", "Анна","Валентина", "Марина", "Карина", "Елизавета",
			"Екатерина", "Вероника", "Виктория", "Дана", "Ольга", "Оксана", "Ксения", 
			"Ульяна", "Надежда", "Любовь", "Лариса", "Анжела", "Светлана", "Наталья" };
		private readonly string[] _manSurnames = new string[] {
			"Петров", "Иванов", "Корнеев", "Дралов", "Кругляков", "Иваненко", "Саложков", "Бульков",
			"Смирнов", "Сидоров", "Николаев", "Шумилов", "Новичков", "Гулевич", "Касьян", "Отчик", 
			"Новиков", "Гуменков", "Пупкин", "Титюк", "Гуменников", "Дашкевич", "Митрахович"};
		private readonly string[] _womenSurnames = new string[] {
			"Иванова", "Ходанович", "Кузьмина", "Тимановская", "Никалаева", "Булькова", "Сидорова",
			"Пантюшечкина", "Соловьева", "Воробьева", "Сорокина", "Смирнова", "Корнеева", "Касьян",
			"Селезнева", "Головнева", "Болотникова", "Степанова", "Ефимова", "Жукова", "Решеткина" };

		private readonly TaxiContext _context;
		private readonly int _count;
		private readonly Random _random;

		private List<int> _positionIds;

		public EmployeeInitializer(TaxiContext context)
		{
			_context = context;
			_count = DataInitializeOptions.CountEmployees;
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

			var employees = new List<Employee>();
			for (int i = 0; i < _count; i++)
			{
				employees.Add(GenerateEmployee());
			}
			await _context.Employees.AddRangeAsync(employees);
			await _context.SaveChangesAsync();

			await Task.CompletedTask;
		}

		private async Task GetIds()
		{
			_positionIds = await _context.Positions.Select(position => position.Id).ToListAsync();
		}

		private Employee GenerateEmployee()
		{
			int minYear = 2010;

			string name;
			string surname;
			if (_random.Next(0, 2) == 0)
			{
				name = _manNames[_random.Next(0, _manNames.Length)];
				surname = _manSurnames[_random.Next(0, _womenNames.Length)];
			}
			else
			{
				name = _womenNames[_random.Next(0, _womenNames.Length)];
				surname = _womenSurnames[_random.Next(0, _womenSurnames.Length)];
			}

			var dateTimeStart = new DateTime(
				_random.Next(minYear, DateTime.Now.Year), _random.Next(1, 13), _random.Next(1, 29));
			var positionId = _positionIds[_random.Next(0, _positionIds.Count)];

			return new Employee
			{
				Name = name,
				Surname = surname,
				DateStartOfWork = dateTimeStart,
				PositionId = positionId,
			};
		}
	}
}
