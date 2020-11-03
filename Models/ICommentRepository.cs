using System.Collections.Generic;

namespace RPG.Web.Models
{
    public interface ICommentRepository
    {
        Comment GetComment(int id);
        IEnumerable<Comment> GetComments(int changesId);
        Comment Add(Comment comment);
        Comment Update(Comment commentChanges);
        Comment Delete(int id);
    }
}