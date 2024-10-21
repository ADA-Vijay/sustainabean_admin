using Dapper;
using Npgsql;
using sustainbean_api.Models;
using System.Data;

namespace sustainbean_api
{
    public interface IFeatureRepository
    {
        Task<IEnumerable<Feature>> GetAllFeaturesAsync();
        Task<Feature?> GetFeatureByIdAsync(int id);
        Task<Feature> AddFeatureAsync(Feature feature);
        Task<Feature> UpdateFeatureAsync(Feature feature);
        Task<bool> UpdateFeatureStatusAsync(int featureId, bool isActive);
        Task<object> GetFeatures();
    }

    public class FeatureRepository : IFeatureRepository
    {
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FeatureRepository(string connectionString, IHttpContextAccessor httpContextAccessor)
        {
            _connectionString = connectionString;
            _httpContextAccessor = httpContextAccessor;
        }

        private IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);

        public async Task<IEnumerable<Feature>> GetAllFeaturesAsync()
        {
            using (var connection = CreateConnection())
            {
                string query = "SELECT * FROM public.tbl_feature";
                return await connection.QueryAsync<Feature>(query);
            }
        }

        public async Task<Feature?> GetFeatureByIdAsync(int id)
        {
            using (var connection = CreateConnection())
            {
                string query = "SELECT * FROM public.tbl_feature WHERE feature_id = @Id";
                return await connection.QueryFirstOrDefaultAsync<Feature>(query, new { Id = id });
            }
        }

        public async Task<Feature> AddFeatureAsync(Feature feature)
        {
            feature.created_on = DateTime.UtcNow;
            feature.created_by = "1"; // Replace with actual user ID
            feature.is_active = true; // Default value

            using (var connection = CreateConnection())
            {
                string query = @"
                INSERT INTO public.tbl_feature 
                (title, alt_text, caption, description, is_active, created_on, created_by) 
                VALUES 
                (@title, @alt_text, @caption, @description, @is_active, @created_on, @created_by)
                RETURNING feature_id";

                feature.feature_id = await connection.ExecuteScalarAsync<int>(query, feature);
            }

            return feature;
        }

        public async Task<Feature> UpdateFeatureAsync(Feature feature)
        {
            feature.updated_on = DateTime.UtcNow;
            feature.updated_by = "1"; // Replace with actual user ID

            using (var connection = CreateConnection())
            {
                string query = @"
                UPDATE public.tbl_feature 
                SET 
                    title = @title, 
                    alt_text = @alt_text, 
                    caption = @caption, 
                    description = @description, 
                    updated_on = @updated_on, 
                    updated_by = @updated_by
                WHERE 
                    feature_id = @feature_id";

                await connection.ExecuteAsync(query, feature);
            }

            return feature;
        }

        public async Task<bool> UpdateFeatureStatusAsync(int featureId, bool isActive)
        {
            using (var connection = CreateConnection())
            {
                string query = @"
                UPDATE public.tbl_feature 
                SET 
                    is_active = @IsActive, 
                    updated_on = @UpdatedOn, 
                    updated_by = @UpdatedBy
                WHERE 
                    feature_id = @FeatureId";

                var affectedRows = await connection.ExecuteAsync(query, new
                {
                    IsActive = isActive,
                    UpdatedOn = DateTime.UtcNow,
                    UpdatedBy = "1", // Replace with actual user ID
                    FeatureId = featureId
                });

                return affectedRows > 0;
            }
        }

        public async Task<object> GetFeatures()
        {
            var totalRecords = 0;
            var filterRecords = 0;

            int pageSize = Convert.ToInt32(_httpContextAccessor.HttpContext.Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(_httpContextAccessor.HttpContext.Request.Form["start"].FirstOrDefault() ?? "0");
            var draw = Convert.ToInt32(_httpContextAccessor.HttpContext.Request.Form["draw"].FirstOrDefault());
            var searchValue = _httpContextAccessor.HttpContext.Request.Form["search[value]"].FirstOrDefault();
            var sortColumn = _httpContextAccessor.HttpContext.Request.Form["columns[" + _httpContextAccessor.HttpContext.Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = _httpContextAccessor.HttpContext.Request.Form["order[0][dir]"].FirstOrDefault();

            string sql = "SELECT * FROM public.tbl_feature WHERE 1=1"; // Base SQL query

            if (!string.IsNullOrEmpty(searchValue))
            {
                sql += " AND (title ILIKE @SearchValue OR description ILIKE @SearchValue)";
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
                var featureList = await connection.QueryAsync<Feature>(sql, new { SearchValue = $"%{searchValue}%" });

                var returnObj = new
                {
                    draw = draw,
                    recordsTotal = totalRecords,
                    recordsFiltered = featureList.Count(),
                    data = featureList
                };

                return returnObj;
            }
        }
    }
}
