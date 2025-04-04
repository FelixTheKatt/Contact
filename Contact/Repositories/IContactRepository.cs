using Contact.Models;

namespace Contact.Repositories
{
    public interface IContactRepository
    {
        Task<List<ContactEntity>> GetAllAsync();
        Task<ContactEntity?> GetByIdAsync(int id);
        Task AddAsync(ContactEntity contact);
        Task UpdateAsync(ContactEntity contact);
        Task DeleteAsync(int id);
    }
}
