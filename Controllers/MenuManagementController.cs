using AdvanceAjaxCRUD.Data;
using Microsoft.AspNetCore.Mvc;

namespace AdvanceAjaxCRUD.Controllers
{
    public class MenuManagementController : Controller
    {
        private readonly IDatabaseService _databaseService;

        public MenuManagementController(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

    }
}
