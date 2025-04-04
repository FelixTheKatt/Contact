using Contact.Services;
using Contact.Models;
using System.Collections.ObjectModel;

namespace Contact;

public partial class ContactListPage : ContentPage
{
    private readonly ContactService contactService;
    public ContactListPage(ContactService contactService)
    {
        InitializeComponent();
        this.contactService = contactService;
        LoadContacts();
    }

    public async void LoadContacts()
    {
        System.Diagnostics.Debug.WriteLine("[DEBUG] LoadContacts déclenché");
        var contacts = await contactService.GetAllContactsAsync();

        ContactsCollection.ItemsSource = null; // forcer le refresh
        ContactsCollection.ItemsSource = contacts;
    }

    private async void OnViewClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is ContactEntity contact)
        {
            await Navigation.PushModalAsync(new ContactDetailPage(contact));
        }
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is ContactEntity contact)
        {
            await Navigation.PushAsync(new ContactFormPage(contactService, contact)); // on prévoit le formulaire pré-rempli
        }
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is ContactEntity contact)
        {
            var confirm = await DisplayAlert("Confirmation", $"Supprimer {contact.FirstName} {contact.LastName} ?", "Oui", "Non");
            if (confirm)
            {
                await contactService.DeleteContactAsync(contact.Id);
                LoadContacts(); // rechargement
            }
        }
    }

}