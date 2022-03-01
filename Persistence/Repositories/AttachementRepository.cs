using Test.Persistence.Entities;

namespace Test.Persistence.Repositories;

class AttachementRepository : IRepository<Attachment>
{
    private readonly Context _context;

    public AttachementRepository(Context context) =>
        _context = context;

    public Attachment? Get(Guid id) =>
        _context.Attachments.Find(id);

    public int CreateOrUpdate(Attachment attachment)
    {
        var entity = _context.Attachments.Find(attachment.Id);

        switch (entity)
        {
            case null:
                _context.Add(attachment);
                break;
            default:
                entity.FileName = attachment.FileName;
                break;
        }

        return _context.SaveChanges();
    }
}