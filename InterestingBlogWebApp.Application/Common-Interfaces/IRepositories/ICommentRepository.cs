using InterestingBlogWebApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterestingBlogWebApp.Application.Common.Interface.IRepositories
{
    public interface ICommentRepository : IRepositoryBase<Comment>
    {
        Task<int> GetAllCommentsCount();
        Task<int> GetCommentsCountForMonth(int month, int year);
    }
}
