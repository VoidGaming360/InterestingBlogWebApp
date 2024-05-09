using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using InterestingBlogWebApp.Application.Common.Interface.IRepositories;
using InterestingBlogWebApp.Application.Common.Interface.IServices;
using InterestingBlogWebApp.Application.DTOs;
using InterestingBlogWebApp.Domain.Entities;
using InterestingBlogWebApp.Infrastructures.Repositories;
using Microsoft.AspNetCore.Identity;

public class BlogVoteService : IBlogVoteService
{
    private readonly IBlogVoteRepository _blogVoteRepository;
    private readonly IBlogRepository _blogRepository;
    private readonly UserManager<User> _userManager;
    private readonly ICommentService _commentService;



    public BlogVoteService(IBlogVoteRepository blogVoteRepository, IBlogRepository blogRepository, UserManager<User> userManager, ICommentService commentService)
    {
        _blogVoteRepository = blogVoteRepository;
        _blogRepository = blogRepository;
        _userManager = userManager;
        _commentService = commentService;
    }

    public async Task<List<BlogVoteDTO>> GetAll()
    {
        var blogVotes = await _blogVoteRepository.GetAll(null);

        return blogVotes.Select(vote => new BlogVoteDTO
        {
            Id = vote.Id,
            BlogId = vote.BlogId,
            IsUpVote = vote.IsUpVote,
            IsDownVote = vote.IsDownVote
        }).ToList();
    }

    public async Task<List<BlogVoteDTO>> GetBlogVotesByUserId(string userId)
    {
        var votes = await _blogVoteRepository.GetAll(null);

        var userBlogVote= votes.Where(vote => vote.UserId == userId).ToList();

        var blogVoteDTOs = new List<BlogVoteDTO>();

        foreach (var blogvote in userBlogVote)
        {
            var user = await _userManager.FindByIdAsync(blogvote.UserId);

            var blogVoteDTO = new BlogVoteDTO
            {
                Id = blogvote.Id,
                BlogId = blogvote.BlogId,
                UserId = blogvote.UserId,
                IsUpVote = blogvote.IsUpVote,
                IsDownVote = blogvote.IsDownVote

            };

            blogVoteDTOs.Add(blogVoteDTO);
        }

        return blogVoteDTOs.OrderByDescending(r => r.CreatedDate).ToList();
    }
    public async Task<BlogVoteDTO> GetVote(int blogId, string userId)
    {
        // Fetch the vote from the repository by blog ID and user ID
        var blogVote = await _blogVoteRepository.GetVote(blogId, userId);

        if (blogVote == null)
        {
            return null; // No vote found for the specified blog and user
        }

        // Convert the retrieved BlogVote to BlogVoteDTO
        return new BlogVoteDTO
        {
            Id = blogVote.Id,
            BlogId = blogVote.BlogId,
            UserId = blogVote.UserId,
            IsUpVote = blogVote.IsUpVote,
            IsDownVote = blogVote.IsDownVote
        };
    }


    public async Task<string> UpvoteBlog(UpvoteBlogDTO blogVoteDTO, List<string> errors)
    {
        try
        {
            var existingVote = await _blogVoteRepository.GetVote(blogVoteDTO.BlogId, blogVoteDTO.UserId);

            if (existingVote != null && existingVote.UserId == blogVoteDTO.UserId)
            {
                if (existingVote.IsUpVote == true)
                {
                    // If upvote exists, remove it
                    existingVote.IsUpVote = false;
                    existingVote.IsDownVote = false;
                    await _blogVoteRepository.SaveChangesAsync(); // Save changes
                    await UpdateBlogScoreAndCounts(blogVoteDTO.BlogId); // Recalculate score and counts

                    return "Upvote removed.";
                }

                // Change to upvote if user previously downvoted or had no reaction
                existingVote.IsUpVote = true;
                existingVote.IsDownVote = false;
            }
            else
            {
                // New upvote
                var newVote = new BlogVote
                {
                    BlogId = blogVoteDTO.BlogId,
                    UserId = blogVoteDTO.UserId,
                    CreatedDate = DateTime.UtcNow,
                    IsUpVote = true,
                    IsDownVote = false
                };

                await _blogVoteRepository.Add(newVote);
            }

            await _blogVoteRepository.SaveChangesAsync(); // Save changes
            await UpdateBlogScoreAndCounts(blogVoteDTO.BlogId); // Recalculate score and counts

            return "Upvoted successfully.";
        }
        catch (Exception ex)
        {
            errors.Add(ex.Message);
            throw;
        }
    }


    public async Task<string> DownvoteBlog(DownvoteBlogDTO blogVoteDTO, List<string> errors)
    {
        try
        {
            var existingVote = await _blogVoteRepository.GetVote(blogVoteDTO.BlogId, blogVoteDTO.UserId);

            if (existingVote != null && existingVote.UserId == blogVoteDTO.UserId)
            {
                if (existingVote.IsDownVote == true)
                {
                    // If downvote exists, remove it
                    existingVote.IsDownVote = false;
                    existingVote.IsUpVote = false;
                    await _blogVoteRepository.SaveChangesAsync(); // Save changes
                    await UpdateBlogScoreAndCounts(blogVoteDTO.BlogId); // Recalculate score and counts

                    return "Downvote removed.";
                }

                // Change to downvote if user previously upvoted or had no reaction
                existingVote.IsDownVote = true;
                existingVote.IsUpVote = false;
            }
            else
            {
                // New downvote
                var newVote = new BlogVote
                {
                    BlogId = blogVoteDTO.BlogId,
                    UserId = blogVoteDTO.UserId,
                    CreatedDate = DateTime.UtcNow,
                    IsDownVote = true,
                    IsUpVote = false
                };

                await _blogVoteRepository.Add(newVote);
            }

            await _blogVoteRepository.SaveChangesAsync(); // Save changes
            await UpdateBlogScoreAndCounts(blogVoteDTO.BlogId); // Recalculate score and counts

            return "Downvoted successfully.";
        }
        catch (Exception ex)
        {
            errors.Add(ex.Message);
            throw;
        }
    }


    public async Task<BlogVoteDTO> GetVoteById(int voteId)
    {
        var blogVote = await _blogVoteRepository.GetById(voteId);

        return new BlogVoteDTO
        {
            Id = blogVote.Id,
            BlogId = blogVote.BlogId,
            UserId = blogVote.UserId,
            IsUpVote = blogVote.IsUpVote,
            IsDownVote = blogVote.IsDownVote
        };
    }

    public async Task<IEnumerable<BlogVoteDTO>> GetAllVotesForBlog(int blogId)
    {
        var votes = await _blogVoteRepository.GetAll(null);

        var blogVotes = votes.Where(vote => vote.BlogId == blogId).ToList();

        var blogVoteDTOs = new List<BlogVoteDTO>();

        foreach (var vote in blogVotes)
        {
            var blogVoteDTO = new BlogVoteDTO
            {
                Id = vote.Id,
                CreatedDate = vote.CreatedDate,
                BlogId = vote.BlogId,
                UserId = vote.UserId,
                IsUpVote = vote.IsUpVote,
                IsDownVote = vote.IsDownVote
            };

            blogVoteDTOs.Add(blogVoteDTO);
        }

        // Return the list of DTOs, optionally ordered by a specific field
        return blogVoteDTOs.OrderByDescending(c => c.CreatedDate).ToList();
    }

    public async Task<int> CalculateBlogPopularity(int blogId)
    {
        var blog = await _blogRepository.GetById(blogId);

        if (blog == null)
        {
            throw new KeyNotFoundException("Blog not found.");
        }

        const int upVoteWeight = 2;
        const int downVoteWeight = -1;
        const int commentWeight = 1;


        int upVotes = blog.UpVoteCount ?? 0;
        int downVotes = blog.DownVoteCount ?? 0;

        // Fetch all comments for the blog and count them
        var commentsList = await _commentService.GetCommentsByBlogId(blogId);
        int comments = commentsList?.Count ?? 0; // Get the count of comments

        int popularityScore = (upVoteWeight * upVotes) + (downVoteWeight * downVotes) + (commentWeight * comments);

        blog.Score = popularityScore; // Update the score in the Blog table

        await _blogRepository.Update(blog); // Ensure the blog repository has an update method
        await _blogRepository.SaveChangesAsync(); // Commit changes


        return popularityScore;
    }

    private async Task UpdateBlogScoreAndCounts(int blogId)
    {
        var votes = await _blogVoteRepository.GetAllVotesForBlog(blogId);

        var blog = await _blogRepository.GetById(blogId);

        if (blog == null)
        {
            throw new KeyNotFoundException("Blog not found.");
        }

        blog.UpVoteCount = votes.Count(v => v.IsUpVote == true);
        blog.DownVoteCount = votes.Count(v => v.IsDownVote == true);

        const int upVoteWeight = 2;
        const int downVoteWeight = -1;
        const int commentWeight = 1;

        int upVotes = blog.UpVoteCount ?? 0;
        int downVotes = blog.DownVoteCount ?? 0;

        // Fetch all comments for the blog and count them
        var commentsList = await _commentService.GetCommentsByBlogId(blogId);
        int comments = commentsList?.Count ?? 0; // Get the count of comments

        blog.Score = (upVoteWeight * upVotes) + (downVoteWeight * downVotes) + (commentWeight * comments);

        await _blogRepository.Update(blog);
    }
}
