using eCommerce.ApplicationLayer.History;
using eCommerce.WebService.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace eCommerce.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        readonly IHistoryService historyService;

        public HistoryController(IHistoryService historyService)
        {
            this.historyService = historyService;
        }

        [HttpGet]
        public Response<HistoryDto> All()
        {
            Response<HistoryDto> response = new Response<HistoryDto>();
            try
            {
                response.Object = this.historyService.GetHistory();
            }
            catch (Exception ex)
            {
                //log error
                response.Errored = true;
                response.ErrorMessage = ex.Message;
            }
            return response;
        }
    }
}
