using Dapper;
using Npgsql;
using sustainbean_api.Models;
using System.Data;

namespace sustainbean_api.Repository
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllTagsAsync();
        Task<Tag?> GetTagByIdAsync(int id);
        Task<Tag> AddTagAsync(Tag tag);
        Task<Tag> UpdateTagAsync(Tag tag);
        Task<bool> UpdateTagStatusAsync(int tagId, bool isActive);
        Task<object> GetTags(); // Consider changing this to return Task<object> for async consistency
    }

    public class TagRepository : ITagRepository
    {
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TagRepository(string connectionString, IHttpContextAccessor httpContextAccessor)
        {
            _connectionString = connectionString;
            _httpContextAccessor = httpContextAccessor;
        }

        private IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);

        public async Task<IEnumerable<Tag>> GetAllTagsAsync()
        {
            using (var connection = CreateConnection())
            {
                string query = "SELECT * FROM public.tbl_tag";
                return await connection.QueryAsync<Tag>(query);
            }
        }

        public async Task<Tag?> GetTagByIdAsync(int id)
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    string query = "SELECT * FROM public.tbl_tag WHERE tag_id = @Id";
                    return await connection.QueryFirstOrDefaultAsync<Tag>(query, new { Id = id });
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return null;
            }
        }

        public async Task<Tag> AddTagAsync(Tag tag)
        {
            tag.created_on = DateTime.UtcNow;
            tag.created_by = "1"; // Replace with actual user ID if needed
            tag.is_active = true; // Set default value

            using (var connection = CreateConnection())
            {
                string query = @"
                INSERT INTO public.tbl_tag 
                (tag_name, slug, description, is_active, created_on, created_by) 
                VALUES 
                (@tag_name, @slug, @description, @is_active, @created_on, @created_by)
                RETURNING tag_id"; // Retrieve the tag_id after insertion

                // Execute the query and get the new tag_id
                tag.tag_id = await connection.ExecuteScalarAsync<int>(query, tag);
            }

            return tag; // Return the tag with the new ID
        }

        public async Task<Tag> UpdateTagAsync(Tag tag)
        {
            tag.updated_on = DateTime.UtcNow;
            tag.updated_by = "1"; // Replace with actual user ID if needed

            using (var connection = CreateConnection())
            {
                string query = @"
                UPDATE public.tbl_tag 
                SET 
                    tag_name = @tag_name, 
                    slug = @slug, 
                    description = @description, 
                    updated_on = @updated_on, 
                    updated_by = @updated_by
                WHERE 
                    tag_id = @tag_id";

                // Execute the update query
                await connection.ExecuteAsync(query, tag);
            }

            return tag; // Return the updated tag object
        }

        public async Task<bool> UpdateTagStatusAsync(int tagId, bool isActive)
        {
            using (var connection = CreateConnection())
            {
                string query = @"
                UPDATE public.tbl_tag 
                SET 
                    is_active = @IsActive, 
                    updated_on = @UpdatedOn, 
                    updated_by = @UpdatedBy
                WHERE 
                    tag_id = @TagId";

                var affectedRows = await connection.ExecuteAsync(query, new
                {
                    IsActive = isActive,
                    UpdatedOn = DateTime.UtcNow,
                    UpdatedBy = "1", // Replace with actual user ID if needed
                    TagId = tagId
                });

                return affectedRows > 0; // Return true if the update was successful
            }
        }

        public async Task<object> GetTags()
        {
            var totalRecords = 0;
            var filterRecords = 0;

            int pageSize = Convert.ToInt32(_httpContextAccessor.HttpContext.Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(_httpContextAccessor.HttpContext.Request.Form["start"].FirstOrDefault() ?? "0");
            var draw = Convert.ToInt32(_httpContextAccessor.HttpContext.Request.Form["draw"].FirstOrDefault());
            var searchValue = _httpContextAccessor.HttpContext.Request.Form["search[value]"].FirstOrDefault();
            var sortColumn = _httpContextAccessor.HttpContext.Request.Form["columns[" + _httpContextAccessor.HttpContext.Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = _httpContextAccessor.HttpContext.Request.Form["order[0][dir]"].FirstOrDefault();

            string sql = "SELECT * FROM public.tbl_tag WHERE 1=1"; // Base SQL query

            if (!string.IsNullOrEmpty(searchValue))
            {
                sql += " AND (tag_name ILIKE @SearchValue OR description ILIKE @SearchValue)";
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
                var tagList = await connection.QueryAsync<Tag>(sql, new { SearchValue = $"%{searchValue}%" });

                var returnObj = new
                {
                    draw,
                    recordsTotal = totalRecords,
                    recordsFiltered = tagList.Count(), // Update this to reflect filtered records
                    data = tagList
                };

                return returnObj;
            }
        }
    }
}
