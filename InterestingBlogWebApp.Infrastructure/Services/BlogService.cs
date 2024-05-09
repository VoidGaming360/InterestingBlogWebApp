using cloudscribe.Pagination.Models;
using InterestingBlogWebApp.Application.DTOs;
using InterestingBlogWebApp.Application.Interfaces;
using InterestingBlogWebApp.Domain.Entities;
using System.Collections;


namespace InterestingBlogWebApp.Infrastructure.Services
{
    public class BlogService : IBlog
    {
        private IUnitOfWork _unitOfWork;
        public BlogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void CreateBlog(BlogDTO blog)
        {
            var newBlog = new Blogs
            {
                Title = blog.Title,
                Description = blog.Description,
                ImageURI = blog.ImageURI,
                ApplicationUserId = blog.ApplicationUserId
                
                
            };

            _unitOfWork.GenericRepositories<Blogs>().Add(newBlog);
            _unitOfWork.Save();
        }

        public void DeleteBlog(int id)
        {
            var blog = _unitOfWork.GenericRepositories<Blogs>().GetById(id);
            if (blog != null)
            {
                _unitOfWork.GenericRepositories<Blogs>().Delete(blog);
                _unitOfWork.Save();
            }

        }

        public IEnumerable<BlogDTO> GetAll()
        {
            var blogs = _unitOfWork.GenericRepositories<Blogs>().GetAll();
            var blogDTOs = new List<BlogDTO>();
            foreach (var blog in blogs)
            {
                var blogDTO = new BlogDTO
                {
                    Id = blog.Id,
                    Title = blog.Title,
                    Description = blog.Description,
                    ImageURI = blog.ImageURI,
                    ApplicationUserId = blog.ApplicationUserId
                };
                blogDTOs.Add(blogDTO);
            }
            return blogDTOs;
        }

        public PagedResult<BlogDTO> GetAllForUser(string userId, int pageNumber, int pageSize)
        {
            var totalCount = _unitOfWork.GenericRepositories<Blogs>()
                .Count(a => a.ApplicationUserId == userId);

            var blogs = _unitOfWork.GenericRepositories<Blogs>()
                .Where(a => a.ApplicationUserId == userId)
                .OrderByDescending(a => a.Id) //change to DateTime (Created at)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();


            return new PagedResult<BlogDTO>
            {
                TotalItems = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

        }

        public BlogDTO GetById(int BlogId)
        {
            var blog = _unitOfWork.GenericRepositories<Blogs>().GetById(BlogId);
            if (blog != null)
            {
                var blogDTO = new BlogDTO
                {
                    Id = blog.Id,
                    Title = blog.Title,
                    Description = blog.Description,
                    ImageURI = blog.ImageURI,
                    ApplicationUserId = blog.ApplicationUserId
                    
                };
                return blogDTO;
            }
            return null;
        }

        public void UpdateBlog(BlogDTO blog)
        {
            var existingBlog = _unitOfWork.GenericRepositories<Blogs>().GetById(blog.Id);
            if (existingBlog != null)
            {
                existingBlog.Title = blog.Title;
                existingBlog.Description = blog.Description;
                existingBlog.ImageURI = blog.ImageURI;
                existingBlog.ApplicationUserId = blog.ApplicationUserId;
                

                _unitOfWork.GenericRepositories<Blogs>().Update(existingBlog);
                _unitOfWork.Save();
            }
        }
    }
}
