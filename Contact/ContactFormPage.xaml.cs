using Contact.Services;
using Contact.Models;
using Contact.Utils;
using System.Text.RegularExpressions;

namespace Contact;

public partial class ContactFormPage : ContentPage
{
    private readonly ContactService contactService;
    private readonly ContactEntity? editingContact;

    public ContactFormPage(ContactService contactService, ContactEntity? contact = null)
    {
        InitializeComponent();
        this.contactService = contactService;
        this.editingContact = contact;

        if (editingContact != null)
        {
            FirstNameEntry.Text = editingContact.FirstName;
            LastNameEntry.Text = editingContact.LastName;
            EmailEntry.Text = editingContact.Email;
            System.Diagnostics.Debug.WriteLine(editingContact.Phone);
            SetPhoneFields(editingContact.Phone);
            AddressEntry.Text = editingContact.Address;
        }

    }

    //refactor
    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (!ValidateAll())
            return;

        var contact = new ContactEntity
        {
            FirstName = FirstNameEntry.Text,
            LastName = LastNameEntry.Text,
            Email = EmailEntry.Text,
            Phone = GetFullPhoneNumber(),
            Address = AddressEntry.Text
        };

        if (editingContact is not null)
        {
            System.Diagnostics.Debug.WriteLine($"[DEBUG] contact.Id = {contact.Id}");
            contact.Id = editingContact.Id;
        }

        var errors = new List<string?>()
        {
            ContactValidator.ValidateFirstName(contact.FirstName),
            ContactValidator.ValidateLastName(contact.LastName),
            ContactValidator.ValidateEmail(contact.Email),
            ContactValidator.ValidatePhone(contact.Phone),
            ContactValidator.ValidateAddress(contact.Address)
            }.Where(e => e is not null).ToList();

        await contactService.SaveContactAsync(contact);
        await DisplayAlert("Succès", "Le contact a été enregistré.", "OK");
        await Navigation.PopAsync();

        // messaging center ? , global event ?
        //Messaging center super déprécier
        //https://learn.microsoft.com/fr-fr/previous-versions/xamarin/xamarin-forms/app-fundamentals/messaging-center
        // Vérifie si la page précédente dans la pile de navigation est bien la page de liste des contacts.
        // Si c'est le cas, on la récupère et on relance sa méthode LoadContacts() pour mettre à jour l'affichage.
        // Cela évite d'utiliser des systèmes plus lourds comme MessagingCenter ou des events globaux.
        // MAUI herite de xamarin
        // https://stackoverflow.com/questions/39510145/return-to-previous-page-with-refresh-data-xamarin-forms

        if (Navigation.NavigationStack.LastOrDefault() is ContactListPage listPage)
        {
            System.Diagnostics.Debug.WriteLine("[DEBUG] Refresh depuis ContactFormPage");
            listPage.LoadContacts();
        }
    }

    private void EmailEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        var message = ContactValidator.ValidateEmail(EmailEntry.Text);

        EmailError.Text = message;
        EmailError.IsVisible = !string.IsNullOrEmpty(message);
    }
    // TODO: Refactorer toutes les validations TextChanged pour utiliser ValidateField()
    // Permet d’éviter la duplication et les erreurs 
    private void FirstNameEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        var error = ContactValidator.ValidateFirstName(FirstNameEntry.Text);
        FirstNameError.Text = error;
        FirstNameError.IsVisible = error is not null;
    }

    private void LastNameEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        var error = ContactValidator.ValidateLastName(LastNameEntry.Text);
        LastNameError.Text = error;
        LastNameError.IsVisible = error is not null;
    }

    private void AddressEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        var error = ContactValidator.ValidateAddress(AddressEntry.Text);
        AddressError.Text = error;
        AddressError.IsVisible = error is not null;
    }

    private void CountryCodePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = sender as Picker;
        if (picker != null && picker.SelectedIndex >= 0)
        {
            string selectedCode = picker.Items[picker.SelectedIndex];
            // Optionnel : faire des truc avec le selected code
        }
    }

    private void PhoneEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        var error = ContactValidator.ValidatePhone(PhoneEntry.Text);
        PhoneError.Text = error;
        PhoneError.IsVisible = error is not null;
    }

    private string GetFullPhoneNumber()
    {
        var prefix = CountryCodePicker.SelectedItem?.ToString() ?? string.Empty;
        return prefix + PhoneEntry.Text;
    }

    // Le paramètre 'validator' est une fonction de validation passée en argument,
    // qui prend en entrée le texte (string?) d'un champ (Entry) et retourne soit
    // un message d'erreur (string) s'il y a une erreur, soit null si le champ est valide.
    private bool ValidateField(Entry entry, Label errorLabel, Func<string?, string?> validator)
    {
        var error = validator(entry.Text);
        errorLabel.Text = error;
        errorLabel.IsVisible = error is not null;
        return error is null;
    }

    // Utilise &= (ET logique cumulatif) pour évaluer TOUTES les validations,
    // même si certaines échouent. Contrairement à &&, le &= ne s’arrête pas au premier false.
    // Cela permet d’afficher toutes les erreurs en une seule fois.
    // sucre syntaxique c#
    private bool ValidateAll()
    {
        bool isValid = true;

        isValid &= ValidateField(FirstNameEntry, FirstNameError, ContactValidator.ValidateFirstName);
        isValid &= ValidateField(LastNameEntry, LastNameError, ContactValidator.ValidateLastName);
        isValid &= ValidateField(EmailEntry, EmailError, ContactValidator.ValidateEmail);
        isValid &= ValidateField(PhoneEntry, PhoneError, ContactValidator.ValidatePhone);
        isValid &= ValidateField(AddressEntry, AddressError, ContactValidator.ValidateAddress);

        return isValid;
    }

    //TODO phone helper
    private void SetPhoneFields(string fullPhone)
    {
        System.Diagnostics.Debug.WriteLine($"[DEBUG] fullPhone = {fullPhone}");
        if (!string.IsNullOrEmpty(fullPhone) && fullPhone.StartsWith("+"))
        {
            foreach (var item in CountryCodePicker.Items)
            {
                if (fullPhone.StartsWith(item))
                {
                    CountryCodePicker.SelectedItem = item;
                    PhoneEntry.Text = fullPhone.Substring(item.Length);
                    System.Diagnostics.Debug.WriteLine($"[DEBUG] countryCode = {item}");
                    System.Diagnostics.Debug.WriteLine($"[DEBUG] phoneNumber = {PhoneEntry.Text}");
                    break;
                }
            }
        }
        else
        {
            // fallback si aucun indicatif détecté
            PhoneEntry.Text = fullPhone;
        }
    }
}