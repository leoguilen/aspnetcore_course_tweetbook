using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetbook.Domain;
using TweetBook.Data;

namespace Tweetbook.Services
{
    public class PostService : IPostService
    {
        private readonly DataContext _context;

        public PostService(DataContext context)
        {
            _context = context;
        }
        public async Task<List<Post>> GetPostsAsync(PaginationFilter paginationFilter = null)
        {
            if(paginationFilter == null)
            {
                return await _context.Posts
                .Include(x => x.Tags)
                .ToListAsync();
            }

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;

            return await _context.Posts
                .Include(x => x.Tags)
                .Skip(skip)
                .Take(paginationFilter.PageSize)
                .ToListAsync();
        }

        public async Task<Post> GetPostByIdAsync(Guid postId)
        {
            return await _context.Posts
                .Include(x => x.Tags)
                .SingleOrDefaultAsync(x => x.Id == postId);
        }

        public async Task<bool> CreatePostAsync(Post post)
        {
            post.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());

            await AddNewTags(post);
            await _context.Posts.AddAsync(post);
            var created = await _context.SaveChangesAsync();
            return created > 0;
        }

        public async Task<bool> UpdatePostAsync(Post postToUpdate)
        {
            _context.Posts.Update(postToUpdate);
            var updated = await _context.SaveChangesAsync();
            return updated > 0;
        }

        public async Task<bool> DeletePostAsync(Guid postId)
        {
            var post = await GetPostByIdAsync(postId);

            if (post == null)
                return false;

            _context.Posts.Remove(post);
            var deleted = await _context.SaveChangesAsync();
            return deleted > 0;
        }

        public async Task<bool> UserOwnsPostAsync(Guid postId, string userId)
        {
            var post = await _context.Posts.AsNoTracking().SingleOrDefaultAsync(x => x.Id == postId);

            if (post == null) return false;
            if (post.UserId != userId) return false;
            
            return true;
        }

        public async Task<List<Tag>> GetAllTagsAsync()
        {
            return await _context.Tags.AsNoTracking().ToListAsync();
        }

        public async Task<bool> CreateTagAsync(Tag tag)
        {
            tag.Name = tag.Name.ToLower();
            var existingTag = await _context.Tags.AsNoTracking().SingleOrDefaultAsync(x => x.Name == tag.Name);
            if (existingTag != null)
                return true;

            await _context.Tags.AddAsync(tag);
            var created = await _context.SaveChangesAsync();
            return created > 0;
        }

        public async Task<Tag> GetTagByNameAsync(string tagName)
        {
            return await _context.Tags.AsNoTracking().SingleOrDefaultAsync(x => x.Name == tagName.ToLower());
        }

        public async Task<bool> DeleteTagAsync(string tagName)
        {
            var tag = await _context.Tags.AsNoTracking().SingleOrDefaultAsync(x => x.Name == tagName.ToLower());

            if (tag == null)
                return true;

            var postTags = await _context.PostTags.Where(x => x.TagName == tagName.ToLower()).ToListAsync();

            _context.PostTags.RemoveRange(postTags);
            _context.Tags.Remove(tag);
            return await _context.SaveChangesAsync() > postTags.Count;
        }

        private async Task AddNewTags(Post post)
        {
            foreach (var tag in post.Tags)
            {
                var existingTag =
                    await _context.Tags.SingleOrDefaultAsync(x =>
                        x.Name == tag.TagName);
                if (existingTag != null)
                    continue;

                await _context.Tags.AddAsync(new Tag
                { Name = tag.TagName, CreatedOn = DateTime.UtcNow, CreatorId = post.UserId });
            }
        }
    }
}
