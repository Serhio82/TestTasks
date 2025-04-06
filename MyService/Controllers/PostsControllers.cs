using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyService;
using MyService.Entities;
using System.Text.Json;

[ApiController]
[Route("posts")]
public class PostsController : ControllerBase
{
    private readonly ApplicationContext _context;

    public PostsController(ApplicationContext context)
    {
        _context = context;
    }

    [HttpGet("{postId}")]
    public async Task<ActionResult<Post>> GetPost(int postId)
    {
        var post = await _context.Posts.Include(p => p.Department).FirstOrDefaultAsync(p => p.Id == postId);
        if (post == null)
            return NotFound();

        SaveJsonToFile(new List<Post> { post }, "post.json");

        return Ok(post);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
    {
        var posts = await _context.Posts.Include(p => p.Department).ToListAsync();

        if (posts == null || posts.Count == 0)
            return NotFound();

        SaveJsonToFile(posts, "posts.json");

        return Ok(posts);
    }

    private void SaveJsonToFile<T>(List<T> data, string fileName)
    {
        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        System.IO.File.WriteAllText(fileName, json);
    }


    [HttpPost]
    public async Task<ActionResult<Post>> CreatePost(Post post)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPost), new { postId = post.Id }, post);
    }

    [HttpDelete("{postId}")]
    public async Task<IActionResult> DeletePost(int postId)
    {
        var post = await _context.Posts.FindAsync(postId);
        if (post == null)
            return NotFound();

        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}