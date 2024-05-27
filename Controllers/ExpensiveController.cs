using DespesasControlApp.DTOs;
using DespesasControlApp.Models;
using DespesasControlApp.Models.Despesass;
using DespesasControlApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DespesasControlApp.Controllers
{
	public class DespesasController : Controller
	{
		private readonly ILogger<DespesasController> _logger;
		private readonly IDespesasService _DespesasService;
		int valorDoCheckbox = 0;

		public DespesasController(ILogger<DespesasController> logger,
			IDespesasService DespesasService)
		{
			_logger = logger;
			_DespesasService = DespesasService;
		}

		public async Task<IActionResult> ReceberValorCheck(int despesaId)
		{
			var despesa = await _DespesasService.GetById(despesaId);

			if (valorDoCheckbox == 0)
			{
				// Atualize o campo de status para 1
				despesa.Cor = 1;

				// Salve as mudanças no banco de dados
				await _DespesasService.Update(despesa);
			}
			else
			{
				despesa.Cor = 0;

				await _DespesasService.Update(despesa);
			}
			return null;
		}

		public async Task<IActionResult> FindWithFilter(ListDespesasDTO listDespesasDto)
		{
			try
			{
				listDespesasDto.Items = await _DespesasService.FindBy(listDespesasDto.StartDate, listDespesasDto.EndDate);
				listDespesasDto.Items = listDespesasDto.Items.Where((i) => i.Cor == 0).ToList();

				return View(listDespesasDto);
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("CustomError", ex.Message);
				return View(listDespesasDto);
			}
		}

		public async Task<IActionResult> Index()
		{
			var listDespesasDto = new ListDespesasDTO();

			listDespesasDto.Items = await _DespesasService.FindBy(listDespesasDto.StartDate, listDespesasDto.EndDate);

			return View(listDespesasDto);
		}

		public async Task<IActionResult> Republica()
		{
			var listDespesasDto = new ListDespesasDTO();

			listDespesasDto.Items = await _DespesasService.FindBy(listDespesasDto.StartDate, listDespesasDto.EndDate);

			return View(listDespesasDto);
		}

		[HttpPost]
		public async Task<IActionResult> Index(ListDespesasDTO listDespesasDto)
		{
			try
			{
				listDespesasDto.Items = await _DespesasService.FindBy(listDespesasDto.StartDate, listDespesasDto.EndDate);


				return View(listDespesasDto);
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("CustomError", ex.Message);
				return View(listDespesasDto);
			}
		}

		public async Task<IActionResult> Create()
		{
			var createDespesasDto = new CreateDespesasDTO();

			return View(createDespesasDto);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateDespesasDTO createDespesasDto)
		{
			try
			{
				await _DespesasService.Create(createDespesasDto);

				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("CustomError", ex.Message);
				return View(createDespesasDto);
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UpdateStatus(int despesaId)
		{
			try
			{
				// Obtenha a despesa do banco de dados usando o serviço ou contexto
				var despesa = await _DespesasService.GetById(despesaId);

				if (despesa != null)
				{
					// Atualize o campo de status para 1
					despesa.Cor = 0;

					// Salve as mudanças no banco de dados
					await _DespesasService.Update(despesa);
				}

				// Redirecione de volta para a página Index ou para onde for apropriado
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("CustomError", ex.Message);
				return RedirectToAction("Index");
			}
		}


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}


}