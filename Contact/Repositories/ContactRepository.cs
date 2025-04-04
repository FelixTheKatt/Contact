using Contact.Models;
using Microsoft.EntityFrameworkCore;
using Contact.Data;

namespace Contact.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly AppDbContext _context;

        public ContactRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ContactEntity>> GetAllAsync()
        {
            return await _context.Contacts.ToListAsync();
        }

        public async Task<ContactEntity?> GetByIdAsync(int id)
        {
            return await _context.Contacts.FindAsync(id);
        }

        public async Task AddAsync(ContactEntity contact)
        {
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ContactEntity contact)
        {
            // On récupère l'entité existante suivie par le contexte (sinon EF va râler s'il en suit 2 avec le même Id)
            var existing = await _context.Contacts.FindAsync(contact.Id);

            if (existing is not null)
            {
                // Entity Framework ne permet pas de suivre deux entités avec le même Id dans le même DbContext.
                // On détache donc l'ancienne instance pour éviter le conflit de tracking.
                _context.Entry(existing).State = EntityState.Detached;
            }

            // On peut maintenant attacher la nouvelle instance et demander une mise à jour.
            _context.Contacts.Update(contact);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact is not null)
            {
                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();
            }
        }
    }
}
