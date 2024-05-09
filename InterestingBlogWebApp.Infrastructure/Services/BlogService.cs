using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using InterestingBlogWebApp.Application.Common_Interfaces;
using InterestingBlogWebApp.Application.DTOs;
using InterestingBlogWebApp.Application.Interfaces;
using InterestingBlogWebApp.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;


namespace InterestingBlogWebApp.Infrastructure.Services
{
    public class BlogService : IBlog
    {
        private Cloudinary _cloudinary;
        private Account _account;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBlogRepository _blogRepository;
        private readonly string _imageFolderPath;

        public BlogService(UserManager<ApplicationUser> userManager, IBlogRepository blogRepository, Cloudinary cloudinary, string imageFolderPath)
        {
            _userManager = userManager;
            _blogRepository = blogRepository;
            _cloudinary = cloudinary;
            _imageFolderPath = imageFolderPath;
        }

        public async Task<List<BlogDTO>> GetAll()
        {
            // Retrieve all blogs associated with the userId
            var blogs = await _blogRepository.GetAll(null); // Get all blogs


            var blogDTOs = new List<BlogDTO>();

            foreach (var blog in blogs)
            {
                var user = await _userManager.FindByIdAsync(blog.ApplicationUserId); // Use await to avoid blocking

                var blogDTO = new BlogDTO
                {
                    Id = blog.Id,
                    Title = blog.Title,
                    Description = blog.Description,
                    CreatedAt = blog.CreatedAt,
                    Image = blog.Image.ToString(),
                    ApplicationUserId = user.Id  // Ensure UserId is passed to the DTO
                };

                blogDTOs.Add(blogDTO);
            }

            return blogDTOs.OrderByDescending(r => r.CreatedAt).ToList();
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
                CreatedAt = blog.CreatedAt,
                Image = blog.Image.ToString(),
                ApplicationUserId = blog.ApplicationUserId
            };
        }

        public async Task<List<BlogDTO>> GetBlogsByUserId(string userId)
        {
            var blogs = await _blogRepository.GetAll(null);

            var userBlogs = blogs.Where(blog => blog.ApplicationUserId == userId).ToList();

            var blogDTOs = new List<BlogDTO>();

            foreach (var blog in userBlogs)
            {
                var user = await _userManager.FindByIdAsync(blog.ApplicationUserId);

                var blogDTO = new BlogDTO
                {
                    Id = blog.Id,
                    Description = blog.Description,
                    CreatedAt = blog.CreatedAt,
                    Image = blog.Image.ToString(),
                    Title = blog.Title,
                    ApplicationUserId = user.Id
                };

                blogDTOs.Add(blogDTO);
            }

            return blogDTOs.OrderByDescending(r => r.CreatedAt).ToList();
        }

        public async Task<string> AddBlog(AddBlogDTO blogDTO, List<string> errors)
        {
            if (blogDTO.Image == null)
            {

                return "There is no image";
            }
            var imageId = UploadImageToLocalFileSystem(blogDTO.Image, "Blogs/Images");
            if (imageId == Guid.Empty)
            {
                errors.Add("Image upload failed.");
                return null;
            }

            var user = await _userManager.FindByIdAsync(blogDTO.ApplicationUserId);
            if (user == null)
            {
                errors.Add("User not found.");
                return "Blog addition failed.";
            }

            var newBlog = new Blogs
            {
                Description = blogDTO.Description,
                CreatedAt = DateTime.UtcNow,
                Image = imageId,
                Title = blogDTO.Title,
                //ApplicationUserId = blogDTO.ApplicationUserId,
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
                existingBlog.Score = existingBlog.Score;
                existingBlog.UpVoteCount = existingBlog.UpVoteCount;
                existingBlog.DownVoteCount = existingBlog.DownVoteCount;

                // Initialize imagelogId with a default value
                Guid imagelogId = existingBlog.Image;

                if (updateBlogDTO.Image != null)
                {
                    var imageId = UploadImageToLocalFileSystem(updateBlogDTO.Image, "Blogs/Images");
                    existingBlog.Image = imageId;
                    imagelogId = imageId;
                }

                // Commit the changes to the database
                await _blogRepository.Update(existingBlog);
                await _blogRepository.SaveChangesAsync(); // Commit changes

                return "Blog updated successfully."; // Return success message
            }
            catch (Exception ex)
            {
                errors.Add($"An error occurred while updating the blog: {ex.Message}");
                return "Blog update failed."; // Return failure message with error details
            }
        }

        private Guid UploadImageToLocalFileSystem(IFormFile file, string folder)
        {
            // Generate a unique filename with dashes included
            var uniqueFileName = Guid.NewGuid().ToString();

            // Combine folder path and filename
            var filePath = Path.Combine(_imageFolderPath, folder, uniqueFileName);

            // Ensure the directory exists, if not, create it
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            // Save the file to the specified path
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            // Return the unique identifier (here, we can use the filename)
            return Guid.Parse(uniqueFileName);
        }


    }

}

    
