using Contact.Models;

namespace Contact;

public partial class ContactDetailPage : ContentPage
{
    public ContactDetailPage(ContactEntity contact)
    {
        InitializeComponent();

        FirstNameLabel.Text = contact.FirstName;
        LastNameLabel.Text = contact.LastName;
        EmailLabel.Text = contact.Email;
        PhoneLabel.Text = contact.Phone;
        AddressLabel.Text = contact.Address;
    }

    private async void OnCloseClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}