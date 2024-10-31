using Dapper;
using Npgsql;
using sustainbean_api.Models;
using System.Data;

namespace sustainbean_api.Repository
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<Category> AddCategoryAsync(Category category);
        Task<Category> UpdateCategoryAsync(Category category);
        Task<bool> UpdateCategoryStatusAsync(int categoryId, bool isActive);
        Task<object> GetCategories();
    }
    public class CategoryRepository : ICategoryRepository
    {
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CategoryRepository(string connectionString, IHttpContextAccessor httpContextAccessor)
        {
            _connectionString = connectionString;
            _httpContextAccessor = httpContextAccessor;
        }

        private IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            using (var connection = CreateConnection())
            {
                string query = "SELECT * FROM public.tbl_category";
                return await connection.QueryAsync<Category>(query);
            }
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            using (var connection = CreateConnection())
            {
                string query = "SELECT * FROM public.tbl_category WHERE category_id = @Id";
                return await connection.QueryFirstOrDefaultAsync<Category>(query, new { Id = id });
            }
        }

        public async Task<Category> AddCategoryAsync(Category category)
        {
            category.created_on = DateTime.UtcNow;
            category.created_by = "1"; // Replace with actual user ID if needed
            category.is_active = true; // Set default value

            using (var connection = CreateConnection())
            {
                string query = @"
                INSERT INTO public.tbl_category 
                (category, slug, parent_category, description, is_active, created_on, created_by) 
                VALUES 
                (@category, @slug, @parent_category, @description, @is_active, @created_on, @created_by)
                RETURNING category_id";

                category.category_id = await connection.ExecuteScalarAsync<int>(query, category);
            }

            return category; // Return the inserted category with the generated ID
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            category.updated_on = DateTime.UtcNow;
            category.updated_by = "1"; // Replace with actual user ID if needed

            using (var connection = CreateConnection())
            {
                string query = @"
                UPDATE public.tbl_category 
                SET 
                    category = @category, 
                    slug = @slug, 
                    parent_category = @parent_category, 
                    description = @description, 
                    updated_on = @updated_on, 
                    updated_by = @updated_by
                WHERE 
                    category_id = @category_id";

                await connection.ExecuteAsync(query, category);
            }

            return category; // Return the updated category object
        }

        public async Task<bool> UpdateCategoryStatusAsync(int categoryId, bool isActive)
        {
            using (var connection = CreateConnection())
            {
                string query = @"
                UPDATE public.tbl_category 
                SET 
                    is_active = @IsActive, 
                    updated_on = @UpdatedOn, 
                    updated_by = @UpdatedBy
                WHERE 
                    category_id = @CategoryId";

                var affectedRows = await connection.ExecuteAsync(query, new
                {
                    IsActive = isActive,
                    UpdatedOn = DateTime.UtcNow,
                    UpdatedBy = "1", // Replace with actual user ID if needed
                    CategoryId = categoryId
                });

                return affectedRows > 0;
            }
        }

        public async Task<object> GetCategories()
        {
            var totalRecords = 0;
            var filterRecords = 0;

            int pageSize = Convert.ToInt32(_httpContextAccessor.HttpContext.Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(_httpContextAccessor.HttpContext.Request.Form["start"].FirstOrDefault() ?? "0");
            var draw = Convert.ToInt32(_httpContextAccessor.HttpContext.Request.Form["draw"].FirstOrDefault());
            var searchValue = _httpContextAccessor.HttpContext.Request.Form["search[value]"].FirstOrDefault();
            var sortColumn = _httpContextAccessor.HttpContext.Request.Form["columns[" + _httpContextAccessor.HttpContext.Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = _httpContextAccessor.HttpContext.Request.Form["order[0][dir]"].FirstOrDefault();

            string sql = "SELECT c.*, pc.category AS parent_category_name FROM public.tbl_category c LEFT JOIN public.tbl_category pc ON CAST(c.parent_category AS INT) = pc.category_id WHERE 1=1";
            

            if (!string.IsNullOrEmpty(searchValue))
            {
                sql += " AND (c.category ILIKE @SearchValue OR pc.category ILIKE @SearchValue  OR description ILIKE @SearchValue)";
            }

            // Count total records
            string countSql = $"SELECT COUNT(*) FROM ({sql}) AS subquery;";

            using (var connection = CreateConnection())
            {
                connection.Open();

                // Get total record count
                totalRecords = await connection.ExecuteScalarAsync<int>(countSql, new { SearchValue = $"%{searchValue}%" });

                // Add sorting
                if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
                {
                    sql += $" ORDER BY {sortColumn} {sortColumnDirection} OFFSET {skip} LIMIT {pageSize};";
                }
                else
                {
                    sql += $" OFFSET {skip} LIMIT {pageSize};";
                }

                // Get filtered records
                var categoryList = await connection.QueryAsync<CategoryGrid>(sql, new { SearchValue = $"%{searchValue}%" });

                var returnObj = new
                {
                    draw = draw,
                    recordsTotal = totalRecords,
                    recordsFiltered = categoryList.Count(),
                    data = categoryList
                };

                return returnObj;
            }
        }



    }
}
