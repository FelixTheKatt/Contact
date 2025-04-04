using Contact.Repositories;
using Contact.Models;

namespace Contact.Services
{
    public class ContactService
    {
        private readonly IContactRepository icontactRepositories;

        public ContactService(IContactRepository icontactRepositories)
        {
            this.icontactRepositories = icontactRepositories;
        }

        public Task<List<ContactEntity>> GetAllContactsAsync() => icontactRepositories.GetAllAsync();

        public Task AddContactAsync(ContactEntity contact)
            =>icontactRepositories.AddAsync(contact);

        public Task<ContactEntity?> GetByIdAsync(int id)
            => icontactRepositories.GetByIdAsync(id);

        // changer si rest API => separation avec les headers (post , put ect...)
        public Task SaveContactAsync(ContactEntity contact)
        {
            if (contact.Id != 0)
                return icontactRepositories.UpdateAsync(contact);
            return icontactRepositories.AddAsync(contact);
        }

        public Task DeleteContactAsync(int id)
            => icontactRepositories.DeleteAsync(id);
    }
}
