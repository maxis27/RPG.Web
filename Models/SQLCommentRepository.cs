using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace RPG.Web.Models
{
    public class SQLCommentRepository : ICommentRepository
    {
        private readonly AppDbContext _context;

        public SQLCommentRepository(AppDbContext context)
        {
            _context = context;
        }

        public Comment Add(Comment comment)
        {
            _context.Comments.Add(comment);
            _context.SaveChanges();
            return comment;
        }

        public Comment Delete(int id)
        {
            Comment comment = _context.Comments.Find(id);
            if(comment != null)
            {
                _context.Comments.Remove(comment);
                _context.SaveChanges();
            }
            return comment;
        }

        public IEnumerable<Comment> GetComments(int changesId)
        {
            return _context.Comments.Where(x => x.ChangesId == changesId);
        }

        public Comment GetComment(int id)
        {
            return _context.Comments.Find(id);
        }

        public Comment Update(Comment commentChanges)
        {
            var comment = _context.Comments.Attach(commentChanges);
           comment.State = EntityState.Modified;
            _context.SaveChanges();
            return commentChanges;
        }
    }
}