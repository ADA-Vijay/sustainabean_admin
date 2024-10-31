using Dapper;
using Npgsql;
using sustainbean_api.Models;
using System.Data;

namespace sustainbean_api.Repository
{

    public interface IBlogRepository
    {
        Task<IEnumerable<Blog>> GetAllBlogsAsync();
        Task<IEnumerable<B2CBlog>> GetAllB2CBlogsAsync(int pageNumber, int pageSize);
        Task<Blog?> GetBlogByIdAsync(int id);
        Task<Blog> AddBlogAsync(Blog blog);
        Task<Blog> UpdateBlogAsync(Blog blog);
        Task<bool> UpdateBlogStatusAsync(int blogId, bool isActive);
        Task<object> GetBlogs(); // For paginated results
        Task<Blog?> GetBlogBySlugAsync(string slug);
        Task<Blog?> GetBlogByCategoryAsync(string category);
        Task<Blog?> GetBlogByTagAsync(string tag);
    }
    public class BlogRepository : IBlogRepository
    {
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BlogRepository(string connectionString, IHttpContextAccessor httpContextAccessor)
        {
            _connectionString = connectionString;
            _httpContextAccessor = httpContextAccessor;
        }

        private IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);

        public async Task<IEnumerable<Blog>> GetAllBlogsAsync()
        {
            using (var connection = CreateConnection())
            {
                string query = @"SELECT b.*,c.category,tg.tag_name FROM public.tbl_blog b
                                Inner join public.tbl_category c on b.category_id=c.category_id 
                                Inner join public.tbl_tag tg on b.tag_id=tg.tag_id";
                return await connection.QueryAsync<Blog>(query);
            }
        }

        public async Task<IEnumerable<B2CBlog>> GetAllB2CBlogsAsync(int pageNumber, int pageSize)
        {
            using (var connection = CreateConnection())
            {
                int offset = (pageNumber - 1) * pageSize;

                // Base query with fixed sorting (created_by DESC) and pagination
                string query = @"
                    SELECT b.*, c.category, tg.tag_name 
                    FROM public.tbl_blog b
                    INNER JOIN public.tbl_category c ON b.category_id = c.category_id
                    INNER JOIN public.tbl_tag tg ON b.tag_id = tg.tag_id
                    ORDER BY b.created_on DESC
                    LIMIT @PageSize OFFSET @Offset";

                // Execute query with parameters for pagination
                return await connection.QueryAsync<B2CBlog>(query, new { PageSize = pageSize, Offset = offset });
            }
        }

        public async Task<Blog?> GetBlogByIdAsync(int id)
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    string query = @"SELECT b.*,c.category,tg.tag_name FROM public.tbl_blog b
                                    Inner join public.tbl_category c on b.category_id=c.category_id 
                                    Inner join public.tbl_tag tg on b.tag_id=tg.tag_id WHERE blog_id = @Id";
                    return await connection.QueryFirstOrDefaultAsync<Blog>(query, new { Id = id });
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return null;
            }
        }

        public async Task<Blog?> GetBlogBySlugAsync(string slug)
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    string query = @"SELECT b.*,c.category,tg.tag_name FROM public.tbl_blog b
                                    Inner join public.tbl_category c on b.category_id=c.category_id 
                                    Inner join public.tbl_tag tg on b.tag_id=tg.tag_id WHERE b.slug = @Slug";
                    return await connection.QueryFirstOrDefaultAsync<Blog>(query, new { Slug = slug });
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return null;
            }
        }

        public async Task<Blog?> GetBlogByCategoryAsync(string category)
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    string query = @"SELECT b.*,c.category,tg.tag_name FROM public.tbl_blog b
                                    Inner join public.tbl_category c on b.category_id=c.category_id 
                                    Inner join public.tbl_tag tg on b.tag_id=tg.tag_id WHERE c.category = @Category";
                    return await connection.QueryFirstOrDefaultAsync<Blog>(query, new { Category = category });
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return null;
            }
        }
        public async Task<Blog?> GetBlogByTagAsync(string tag)
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    string query = @"SELECT b.*,c.category,tg.tag_name FROM public.tbl_blog b
                                    Inner join public.tbl_category c on b.category_id=c.category_id 
                                    Inner join public.tbl_tag tg on b.tag_id=tg.tag_id WHERE tg.tag_name = @Tag";
                    return await connection.QueryFirstOrDefaultAsync<Blog>(query, new { Tag = tag });
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return null;
            }
        }

        public async Task<Blog> AddBlogAsync(Blog blog)
        {
            blog.created_on = DateTime.UtcNow;
            blog.created_by = "1"; // Replace with actual user ID if needed
            blog.is_active = true; // Set default value

            using (var connection = CreateConnection())
            {
                string query = @"
               INSERT INTO public.tbl_blog 
                (category_id, tag_id,blog_title, slug, auther, img_url, seo_title, seo_key_word, description, html, is_active, created_on, created_by) 
                VALUES 
                (@category_id, @tag_id,@blog_title, @slug, @auther, @img_url, @seo_title, @seo_key_word, @description, @html, @is_active, @created_on, @created_by)
                RETURNING blog_id"; // Retrieve the blog_id after insertion

                // Execute the query and get the new blog_id
                blog.blog_id = await connection.ExecuteScalarAsync<int>(query, blog);
            }

            return blog; // Return the blog with the new ID
        }

        public async Task<Blog> UpdateBlogAsync(Blog blog)
        {
            blog.updated_on = DateTime.UtcNow;
            blog.updated_by = "1"; // Replace with actual user ID if needed

            using (var connection = CreateConnection())
            {
                string query = @"
                UPDATE public.tbl_blog 
                SET 
                    category_id = @category_id, 
                    tag_id = @tag_id, 
                    slug = @slug, 
                    blog_title=@blog_title,
                    auther = @auther, 
                    img_url = @img_url, 
                    seo_title = @seo_title, 
                    seo_key_word = @seo_key_word, 
                    description = @description, 
                    html = @html, 
                    updated_on = @updated_on, 
                    updated_by = @updated_by
                WHERE 
                    blog_id = @blog_id";

                // Execute the update query
                await connection.ExecuteAsync(query, blog);
            }

            return blog; // Return the updated blog object
        }

        public async Task<bool> UpdateBlogStatusAsync(int blogId, bool isActive)
        {
            using (var connection = CreateConnection())
            {
                string query = @"
                UPDATE public.tbl_blog 
                SET 
                    is_active = @IsActive, 
                    updated_on = @UpdatedOn, 
                    updated_by = @UpdatedBy
                WHERE 
                    blog_id = @BlogId";

                var affectedRows = await connection.ExecuteAsync(query, new
                {
                    IsActive = isActive,
                    UpdatedOn = DateTime.UtcNow,
                    UpdatedBy = "1", // Replace with actual user ID if needed
                    BlogId = blogId
                });

                return affectedRows > 0; // Return true if the update was successful
            }
        }

        public async Task<object> GetBlogs()
        {
            var totalRecords = 0;
            var filterRecords = 0;

            int pageSize = Convert.ToInt32(_httpContextAccessor.HttpContext.Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(_httpContextAccessor.HttpContext.Request.Form["start"].FirstOrDefault() ?? "0");
            var draw = Convert.ToInt32(_httpContextAccessor.HttpContext.Request.Form["draw"].FirstOrDefault());
            var searchValue = _httpContextAccessor.HttpContext.Request.Form["search[value]"].FirstOrDefault();
            var sortColumn = _httpContextAccessor.HttpContext.Request.Form["columns[" + _httpContextAccessor.HttpContext.Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = _httpContextAccessor.HttpContext.Request.Form["order[0][dir]"].FirstOrDefault();

            string sql = "SELECT * FROM public.tbl_blog WHERE 1=1"; // Base SQL query

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
                var blogList = await connection.QueryAsync<Blog>(sql, new { SearchValue = $"%{searchValue}%" });

                var returnObj = new
                {
                    draw = draw,
                    recordsTotal = totalRecords,
                    recordsFiltered = blogList.Count(), // Update this to reflect filtered records
                    data = blogList
                };

                return returnObj;
            }
        }
    }
}

