//using AdvanceAjaxCRUD.Models;
//using Microsoft.Data.SqlClient;
//using System.Data;

//namespace AdvanceAjaxCRUD.Services
//{
//    public class MenuService
//    {
//       private readonly string _connectionString;

//    public MenuService(IConfiguration configuration)
//    {
//        _connectionString = configuration.GetConnectionString("DefaultConnection");
//    }

//        public async Task<List<Menu>> GetMenusAsync()
//        {
//            var menus = new List<Menu>();
//            var menuItems = await GetMenuItemsAsync();
//            var rootMenuItems = menuItems.Where(mi => mi.ParentId == null);

//            foreach (var rootMenuItem in rootMenuItems)
//            {
//                var menu = new Menu
//                {
//                    Id = rootMenuItem.Id,
//                    Name = rootMenuItem.Name,
//                    Url = rootMenuItem.Url
//                };

//                AddChildMenus(menuItems, rootMenuItem, menu);

//                menus.Add(menu);
//            }

//            return menus;
//        }
//        private async Task<List<Menu>> GetMenuItemsAsync()
//        {
//            var menuItems = new List<Menu>();

//            using (SqlConnection connection = new SqlConnection(_connectionString))
//            {
//                await connection.OpenAsync();
//                var command = new SqlCommand("SELECT * FROM Menus", connection);
//                var reader = await command.ExecuteReaderAsync();

//                while (await reader.ReadAsync())
//                {
//                    var menuItem = new Menu
//                    {
//                        Id = reader.GetInt32(0),
//                        Name = reader.GetString(1),
//                        Url = reader.GetString(2),
//                        ParentId = reader.IsDBNull(3) ? null : (int?)reader.GetInt32(3)
//                    };

//                    menuItems.Add(menuItem);
//                }
//            }

//            return menuItems;
//        }

//    public async Task AddMenuAsync(Menu menu)
//    {
//        using (SqlConnection connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            using (SqlCommand command = new SqlCommand("INSERT INTO Menus (Name, Url, DisplayOrder) VALUES (@Name, @Url, @DisplayOrder)", connection))
//            {
//                command.Parameters.AddWithValue("@Name", menu.Name);
//                command.Parameters.AddWithValue("@Url", menu.Url);
//                command.Parameters.AddWithValue("@DisplayOrder", menu.DisplayOrder);
//                await command.ExecuteNonQueryAsync();
//            }
//        }
//    }

//    public async Task UpdateMenuAsync(Menu menu)
//    {
//        using (SqlConnection connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            using (SqlCommand command = new SqlCommand("UPDATE Menus SET Name = @Name, Url = @Url, DisplayOrder = @DisplayOrder WHERE Id = @Id", connection))
//            {
//                command.Parameters.AddWithValue("@Name", menu.Name);
//                command.Parameters.AddWithValue("@Url", menu.Url);
//                command.Parameters.AddWithValue("@DisplayOrder", menu.DisplayOrder);
//                command.Parameters.AddWithValue("@Id", menu.Id);
//                await command.ExecuteNonQueryAsync();
//            }
//        }
//    }

//    public async Task DeleteMenuAsync(int id)
//    {
//        using (SqlConnection connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            using (SqlCommand command = new SqlCommand("DELETE FROM Menus WHERE Id = @Id", connection))
//            {
//                command.Parameters.AddWithValue("@Id", id);
//                await command.ExecuteNonQueryAsync();
//            }
//        }
//    }
//    }
//}
