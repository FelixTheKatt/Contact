using Contact.Services;

namespace Contact
{
    public partial class MainPage : ContentPage
    {
        private readonly ContactService contactService;

        public MainPage(ContactService contactService)
        {
            InitializeComponent();
            this.contactService = contactService;
        }

        private async void OnGoToFormClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ContactFormPage(contactService));
        }

        private async void OnGoToListClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ContactListPage(contactService));
        }
    }

}
