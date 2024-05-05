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

        public BlogService() { }

        public void CreateBlog(BlogDTO blog)
        {
            var newBlog = new Blogs
            {
                Name = blog.Name,
                Description = blog.Description,
                ImageURI = blog.ImageURI,
                Upvotes = blog.Upvotes,
                Downvotes = blog.Downvotes,
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
                    Name = blog.Name,
                    Description = blog.Description,
                    ImageURI = blog.ImageURI,
                    Upvotes = blog.Upvotes,
                    Downvotes = blog.Downvotes,
                    ApplicationUserId = blog.ApplicationUserId
                };
                blogDTOs.Add(blogDTO);
            }
            return blogDTOs;
        }

        public BlogDTO GetById(int BlogId)
        {
            var blog = _unitOfWork.GenericRepositories<Blogs>().GetById(BlogId);
            if (blog != null)
            {
                var blogDTO = new BlogDTO
                {
                    Id = blog.Id,
                    Name = blog.Name,
                    Description = blog.Description,
                    ImageURI = blog.ImageURI,
                    Upvotes = blog.Upvotes,
                    Downvotes = blog.Downvotes,
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
                existingBlog.Name = blog.Name;
                existingBlog.Description = blog.Description;
                existingBlog.ImageURI = blog.ImageURI;
                existingBlog.Upvotes = blog.Upvotes;
                existingBlog.Downvotes = blog.Downvotes;
                existingBlog.ApplicationUserId = blog.ApplicationUserId;
                

                _unitOfWork.GenericRepositories<Blogs>().Update(existingBlog);
                _unitOfWork.Save();
            }
        }
    }
}
