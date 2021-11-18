using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Taxi.UI.TagHelpers
{
	public class PaginationTagHelper : TagHelper
	{
		private readonly int _countBlocks = 7;
		private readonly int _maxCountBlocks = 9;
		private readonly int _indent = 3;

		public int CurrentPage { get; set; }

		public int CountPages { get; set; }

		public string AspController { get; set; }

		public string AspAction { get; set; }

		public string Name { get; set; }

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			output.TagName = "div";
			string htmlContent = "";

			// Страниц не более 9
			if (CountPages <= _maxCountBlocks)
			{
				htmlContent = GenerateSimpleBlocks(1, CountPages);
			}

			// Страниц более 9 и выбрана начальная страница
			if (CountPages > _maxCountBlocks && CurrentPage < _indent + 2)
			{
				htmlContent = GenerateSimpleBlocks(1, _countBlocks);
				htmlContent += "<label>...</label>";
				htmlContent += GenerateSimpleBlocks(CountPages, CountPages);
			}

			// Страниц более 9 и выбрана не начальная страница, но и не в конце
			if (CountPages > _maxCountBlocks && CurrentPage > _indent + 1 && CurrentPage < CountPages - _indent)
			{
				htmlContent = GenerateSimpleBlocks(1, 1);
				htmlContent += "<label>...</label>";
				htmlContent += GenerateSimpleBlocks(CurrentPage - _indent, CurrentPage + _indent);
				htmlContent += "<label>...</label>";
				htmlContent += GenerateSimpleBlocks(CountPages, CountPages);
			}

			// Страниц более 9 и выбрана одна из последних
			if (CountPages > _maxCountBlocks && CurrentPage > CountPages - _indent - 1)
			{
				htmlContent = GenerateSimpleBlocks(1, 1);
				htmlContent += "<label>...</label>";
				htmlContent += GenerateSimpleBlocks(CountPages - _countBlocks + 1, CountPages);
			}

			output.Content.SetHtmlContent(htmlContent);
		}

		private string GenerateSimpleBlocks(int startNumber, int endNumber)
		{
			string result = "";
			for (int i = startNumber; i <= endNumber; i++)
			{
				if (i == CurrentPage)
				{
					result += GetCurrentInputString(i);
				}
				else
				{
					result += GetInputString(i);
				}
			}
			return result;
		}

		private string GetInputString(int value)
		{
			return $"<input type='submit' name='{Name}' asp-controller='{AspController}' asp-action='{AspAction}' value='{value}' />";
		}

		private string GetCurrentInputString(int value)
		{
			return $"<input type='submit' style='color: red' name='{Name}' asp-controller='{AspController}' asp-action='{AspAction}' value='{value}' />";
		}
	}
}
