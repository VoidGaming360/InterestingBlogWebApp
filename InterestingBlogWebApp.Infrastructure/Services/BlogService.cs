using InterestingBlogWebApp.Application.Common.Interface.IRepositories;
using InterestingBlogWebApp.Application.Common.Interface.IServices;
using InterestingBlogWebApp.Application.DTOs;
using InterestingBlogWebApp.Application.Helpers;
using InterestingBlogWebApp.Domain.Entities;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using static System.Net.Mime.MediaTypeNames;

namespace InterestingBlogWebApp.Infrastructure.Services
{
    public class BlogService : IBlogService
    {
        private Cloudinary _cloudinary;
        private CloudinarySettings _cloudinarySettings;
        private Account _account;
        private readonly IBlogRepository _blogRepository;
        private readonly IBlogRecordRepository _blogRecordRepository;
        private readonly UserManager<User> _userManager;

        public BlogService(IBlogRepository blogsRepository, UserManager<User> userManager, 
            IOptions<CloudinarySettings> cloudinarySettingsOptions, IBlogRecordRepository blogRecordRepository)
        {
            _cloudinarySettings = cloudinarySettingsOptions.Value;
            _account = new Account(
                _cloudinarySettings.CloudName,
                _cloudinarySettings.ApiKey,
                _cloudinarySettings.ApiSecret);
            _cloudinary = new Cloudinary(_account);
            _blogRepository = blogsRepository;
            _userManager = userManager;
            _blogRecordRepository = blogRecordRepository;
        }

        public async Task<List<BlogDTO>> GetAll()
        {
            // Retrieve all blogs associated with the userId
            var blogs = await _blogRepository.GetAll(null); // Get all blogs


            var blogDTOs = new List<BlogDTO>();

            foreach (var blog in blogs)
            {
                var user = await _userManager.FindByIdAsync(blog.UserId); // Use await to avoid blocking

                var blogDTO = new BlogDTO
                {
                    Id = blog.Id,
                    Description = blog.Description,
                    CreatedDate = blog.CreatedDate,
                    IsEdited = blog.IsEdited,
                    Category = blog.Category,
                    Image = blog.Image.ToString(),
                    Title = blog.Title,
                    Score = blog.Score ?? 0,
                    UpVoteCount = blog.UpVoteCount ?? 0,
                    DownVoteCount = blog.DownVoteCount ?? 0,
                    UserId = user.Id  // Ensure UserId is passed to the DTO
                };

                blogDTOs.Add(blogDTO);
            }

            return blogDTOs.OrderByDescending(r => r.CreatedDate).ToList(); 
        }

        public async Task<(List<BlogDTO> Blogs, int TotalPages, int TotalCount)> GetAllSorted(string sortBy, int pageNumber, int pageSize)
        {
            var blogs = await _blogRepository.GetAll(null);

            IEnumerable<Blog> sortedBlogs;

            switch (sortBy.ToLower())
            {
                case "popularity":
                    sortedBlogs = blogs.OrderByDescending(b => b.UpVoteCount);
                    break;
                case "recency":
                    sortedBlogs = blogs.OrderByDescending(b => b.CreatedDate);
                    break;
                case "random":
                    var random = new Random();
                    sortedBlogs = blogs.OrderBy(b => random.Next());
                    break;
                default:
                    sortedBlogs = blogs.OrderBy(_ => Guid.NewGuid());
                    break;
            }

            var totalCount = sortedBlogs.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var pagedBlogs = sortedBlogs
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            var blogDTOs = new List<BlogDTO>();

            foreach (var blog in pagedBlogs)
            {
                var user = await _userManager.FindByIdAsync(blog.UserId);

                var blogDTO = new BlogDTO
                {
                    Id = blog.Id,
                    Description = blog.Description,
                    CreatedDate = blog.CreatedDate,
                    IsEdited = blog.IsEdited,
                    Category = blog.Category,
                    Image = blog.Image.ToString(),
                    Title = blog.Title,
                    Score = blog.Score ?? 0,
                    UpVoteCount = blog.UpVoteCount ?? 0,
                    DownVoteCount = blog.DownVoteCount ?? 0,
                    UserId = user.Id
                };

                blogDTOs.Add(blogDTO);
            }

            return (blogDTOs, totalPages, totalCount);
        }

        public async Task<BlogDTO> GetBlogById(int blogId)
        {
            var blog = await _blogRepository.GetById(blogId);

            if (blog == null)
            {
                throw new KeyNotFoundException("Blog not found."); // Handle case where blog doesn't exist
            }

            return new BlogDTO
            {
                Id = blog.Id,
                Title = blog.Title,
                Description = blog.Description,
                CreatedDate = blog.CreatedDate,
                IsEdited = blog.IsEdited,
                Category = blog.Category,
                Image = blog.Image.ToString(),
                Score = blog.Score ?? 0,
                UpVoteCount = blog.UpVoteCount ?? 0,
                DownVoteCount = blog.DownVoteCount ?? 0,
                UserId = blog.UserId
            };
        }

        public async Task<List<BlogDTO>> GetBlogsByUserId(string userId)
        {
            var blogs = await _blogRepository.GetAll(null); 

            var userBlogs = blogs.Where(blog => blog.UserId == userId).ToList();

            var blogDTOs = new List<BlogDTO>();

            foreach (var blog in userBlogs)
            {
                var user = await _userManager.FindByIdAsync(blog.UserId); 

                var blogDTO = new BlogDTO
                {
                    Id = blog.Id,
                    Description = blog.Description,
                    CreatedDate = blog.CreatedDate,
                    IsEdited = blog.IsEdited,
                    Category = blog.Category,
                    Image = blog.Image.ToString(),
                    Title = blog.Title,
                    Score = blog.Score ?? 0,
                    UpVoteCount = blog.UpVoteCount ?? 0,
                    DownVoteCount = blog.DownVoteCount ?? 0,
                    UserId = user.Id  
                };

                blogDTOs.Add(blogDTO);
            }

            return blogDTOs.OrderByDescending(r => r.CreatedDate).ToList(); 
        }

        public async Task<string> AddBlog(AddBlogDTO blogDTO, List<string> errors)
        {
            if (blogDTO.Image == null)
            {
                
                return "There is no image";
            }
            var imageId = UploadImageToCloudinary(blogDTO.Image, "Blogs/Images");
            if (imageId == Guid.Empty)
            {
                errors.Add("Image upload failed.");
                return null; 
            }

            var user = await _userManager.FindByIdAsync(blogDTO.UserId);
            if (user == null)
            {
                errors.Add("User not found.");
                return "Blog addition failed.";
            }

            var newBlog = new Blog
            {
                Description = blogDTO.Description,
                IsEdited = false,
                Category = blogDTO.Category,
                CreatedDate = DateTime.UtcNow,
                Image = imageId,
                Title = blogDTO.Title,
                UserId = blogDTO.UserId,
                Score = 0,
                UpVoteCount = 0,
                DownVoteCount = 0
            };

            await _blogRepository.Add(newBlog); 
            await _blogRepository.SaveChangesAsync(); 

            return "Blog added successfully.";
        }



        public async Task<string> DeleteBlog(int blogId, List<string> errors)
        {
            var blog = await _blogRepository.GetById(blogId);

            if (blog == null)
            {
                errors.Add("Blog not found.");
                return "Blog deletion failed.";
            }

            await _blogRepository.Delete(blog);
            await _blogRepository.SaveChangesAsync(); // Ensure SaveChangesAsync() is called

            return "Blog deleted successfully.";
        }

        public async Task<string> UpdateBlog(UpdateBlogDTO updateBlogDTO, List<string> errors)
        {
            try
            {
                // Fetch the blog to be updated by its ID
                var existingBlog = await _blogRepository.GetById(updateBlogDTO.BlogId);

                if (existingBlog == null) // Check if the blog exists
                {
                    errors.Add("Blog not found.");
                    return "Blog update failed.";
                }


                // Update the existing blog with the provided data
                existingBlog.Title = updateBlogDTO.Title ?? existingBlog.Title; // If null, keep the current value
                existingBlog.Description = updateBlogDTO.Description ?? existingBlog.Description;
                existingBlog.Category = updateBlogDTO.Category ?? existingBlog.Category;
                existingBlog.Score = existingBlog.Score;
                existingBlog.UpVoteCount = existingBlog.UpVoteCount;
                existingBlog.DownVoteCount = existingBlog.DownVoteCount;
                existingBlog.IsEdited = true; // Mark as edited for tracking

                // Initialize imagelogId with a default value
                Guid imagelogId = existingBlog.Image;

                if (updateBlogDTO.Image != null)
                {
                    var imageId = UploadImageToCloudinary(updateBlogDTO.Image, "Blogs/Images");
                    existingBlog.Image = imageId;
                    imagelogId = imageId;
                }

                // Commit the changes to the database
                await _blogRepository.Update(existingBlog);
                await _blogRepository.SaveChangesAsync(); // Commit changes



                // Create a new Record entry with the updated details
                var blogLogsheetDTO = new BlogRecord
                {
                    Title = existingBlog.Title, // Use the updated title
                    Description = existingBlog.Description, // Use the updated description
                    Category = existingBlog.Category, // Use the updated category
                    BlogId = existingBlog.Id, // ID of the blog
                    UpdatedAt = DateTime.UtcNow, // Current time of update
                    UserId = existingBlog.UserId, // User who made the update
                    Image = imagelogId
                };

                // Add the Record to the repository
                await _blogRecordRepository.Add(blogLogsheetDTO);
                await _blogRecordRepository.SaveChangesAsync(); // Commit changes


                return "Blog updated successfully."; // Return success message
            }
            catch (Exception ex)
            {
                errors.Add($"An error occurred while updating the blog: {ex.Message}");
                return "Blog update failed."; // Return failure message with error details
            }
        }

        private Guid UploadImageToCloudinary(IFormFile file, string folder)
        {
            using (var stream = new MemoryStream())
            {
                
                file.CopyTo(stream);
                stream.Position = 0;
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = folder,
                    PublicId = Guid.NewGuid().ToString(),
                    Transformation = new Transformation().FetchFormat("auto")
                };

                var uploadResult = _cloudinary.Upload(uploadParams);
                if (uploadResult.Error != null)
                {
                    return Guid.Empty;
                }

                var publicIdParts = uploadResult.PublicId.Split('/');
                var guidPart = publicIdParts.Last();

                return Guid.Parse(guidPart);
            }
        }

        public async Task<string> GetUserNameById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user?.UserName;
        }
    }
}
