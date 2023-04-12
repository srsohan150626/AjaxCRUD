using AdvanceAjaxCRUD.Models;

namespace AdvanceAjaxCRUD.Services
{
    public interface IMenuService
    {
        List<Menu> GetMenus();
        Menu GetMenu(int id);
        void AddMenu(Menu menu);
        void UpdateMenu(Menu menu);
        void DeleteMenu(int id);
    }
}
