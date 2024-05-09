﻿using InterestingBlogWebApp.Application.Common.Interface.IRepositories;
using InterestingBlogWebApp.Domain.Entities;
using InterestingBlogWebApp.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterestingBlogWebApp.Infrastructures.Repositories
{
    public class BlogLogsheetRepository : RepositoryBase<BlogLogsheet>, IBlogLogsheetRepository
    {
        private readonly AppDbContext _appDbContext;

        public BlogLogsheetRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }
    }
}
