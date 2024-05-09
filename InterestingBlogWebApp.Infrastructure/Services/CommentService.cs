using InterestingBlogWebApp.Application.Common.Interface.IRepositories;
using InterestingBlogWebApp.Application.Common.Interface.IServices;
using InterestingBlogWebApp.Application.DTOs;
using InterestingBlogWebApp.Domain.Entities;
using InterestingBlogWebApp.Infrastructures.Repositories;
using Microsoft.AspNetCore.Identity;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly UserManager<User> _userManager;
    private readonly ICommentRecordRepository _commentLogsheetRepository;


    public CommentService(ICommentRepository commentRepository, UserManager<User> userManager, ICommentRecordRepository commentLogsheetRepository)
    {
        _commentRepository = commentRepository;
        _userManager = userManager;
        _commentLogsheetRepository = commentLogsheetRepository;
    }

    public async Task<string> AddComment(AddCommentDTO addCommentDTO, List<string> errors)
    {
        // Create a new Comment entity
        var newComment = new Comment
        {
            Description = addCommentDTO.Description,
            CreatedDate = DateTime.UtcNow,
            BlogId = addCommentDTO.BlogId,
            UserId = addCommentDTO.UserId,
            IsEdited = false
        };

        await _commentRepository.Add(newComment); // Add the new comment
        await _commentRepository.SaveChangesAsync(); // Commit changes

        return "Comment added successfully.";
    }
    public async Task<List<CommentDTO>> GetAll()
    {
        // Retrieve all comments
        var comments = await _commentRepository.GetAll(null);

        var commentDTOs = new List<CommentDTO>();

        foreach (var comment in comments)
        {
            // Fetch user information to add to the DTO
            var user = await _userManager.FindByIdAsync(comment.UserId);

            var commentDTO = new CommentDTO
            {
                Id = comment.Id,
                Description = comment.Description,
                CreatedDate = comment.CreatedDate,
                IsEdited = comment.IsEdited,
                BlogId = comment.BlogId,
                UserId = user?.Id, // Ensure UserId is included, check for null
            };

            commentDTOs.Add(commentDTO);
        }

        // Return the list of CommentDTOs ordered by CreatedDate
        return commentDTOs.OrderByDescending(c => c.CreatedDate).ToList();
    }
    public async Task<List<CommentDTO>> GetCommentsByBlogId(int blogId)
    {
        // Retrieve all comments
        var comments = await _commentRepository.GetAll(null);

        // Filter comments by blog ID
        var blogComments = comments.Where(comment => comment.BlogId == blogId).ToList();

        // Create a list of CommentDTOs to map the filtered comments
        var commentDTOs = new List<CommentDTO>();

        foreach (var comment in blogComments)
        {
            // Map the filtered comments to DTOs
            var commentDTO = new CommentDTO
            {
                Id = comment.Id,
                Description = comment.Description,
                CreatedDate = comment.CreatedDate,
                IsEdited = comment.IsEdited,
                BlogId = comment.BlogId,
                UserId = comment.UserId
            };

            commentDTOs.Add(commentDTO);
        }

        // Return the list of DTOs, optionally ordered by a specific field
        return commentDTOs.OrderByDescending(c => c.CreatedDate).ToList();
    }


    public async Task<List<CommentDTO>> GetCommentsByUserId(string userId)
    {
        // Retrieve all comments
        var comments = await _commentRepository.GetAll(null);

        // Filter comments by user ID
        var userComments = comments.Where(comment => comment.UserId == userId).ToList();

        // Create a list of CommentDTOs to map the filtered comments
        var commentDTOs = new List<CommentDTO>();

        foreach (var comment in userComments)
        {
            // Map the filtered comments to DTOs
            var commentDTO = new CommentDTO
            {
                Id = comment.Id,
                Description = comment.Description,
                CreatedDate = comment.CreatedDate,
                IsEdited = comment.IsEdited,
                BlogId = comment.BlogId,
                UserId = comment.UserId
            };

            commentDTOs.Add(commentDTO);
        }

        // Return the list of DTOs, optionally ordered by a specific field
        return commentDTOs.OrderByDescending(c => c.CreatedDate).ToList();
    }


    public async Task<CommentDTO> GetCommentById(int commentId)
    {
        var comment = await _commentRepository.GetById(commentId);

        if (comment == null)
        {
            throw new KeyNotFoundException("Comment not found.");
        }

        return new CommentDTO
        {
            Id = comment.Id,
            Description = comment.Description,
            CreatedDate = comment.CreatedDate,
            IsEdited = comment.IsEdited,
            BlogId = comment.BlogId,
            UserId = comment.UserId
        };
    }

    public async Task<string> UpdateComment(UpdateCommentDTO updateCommentDTO, List<string> errors)
    {
        var comment = await _commentRepository.GetById(updateCommentDTO.Id);

        if (comment == null)
        {
            errors.Add("Comment not found.");
            return "Update failed.";
        }

        comment.Description = updateCommentDTO.Description ?? comment.Description;
        comment.IsEdited = true; // Mark comment as edited

        await _commentRepository.Update(comment); // Update the comment
        await _commentRepository.SaveChangesAsync(); // Commit changes

        // Create a new comment logsheet entry
        var commentLogsheet = new CommentReaction
        {
            Description = comment.Description,
            UpdatedAt = DateTime.UtcNow,
            CommentId = comment.Id,
            BlogId = comment.BlogId,
            UserId = comment.UserId
        };

        // Add the logsheet entry to the repository
        await _commentLogsheetRepository.Add(commentLogsheet);
        await _commentLogsheetRepository.SaveChangesAsync(); // Commit changes



        return "Comment updated successfully.";
    }

    public async Task<string> DeleteComment(int commentId, List<string> errors)
    {
        var comment = await _commentRepository.GetById(commentId);

        if (comment == null)
        {
            errors.Add("Comment not found.");
            return "Delete failed.";
        }

        await _commentRepository.Delete(comment); // Delete the comment
        await _commentRepository.SaveChangesAsync(); // Commit changes

        return "Comment deleted successfully.";
    }
}
