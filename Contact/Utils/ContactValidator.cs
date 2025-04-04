using Contact.Models;
using System.Text.RegularExpressions;

namespace Contact.Utils
{
    public static class ContactValidator
    {

        /// Valide une adresse email.
        /// Retourne null si l'email est valide, sinon un message d'erreur.
        /// Utilise string? pour mieux exprimer l'absence d'erreur et s'intégrer aux conventions modernes C#.
        public static string? ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return "L'email est requis.";
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return "L'email n'est pas valide.";
            return null;
        }
        public static string? ValidateFirstName(string? firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                return "Le prénom est requis.";
            if (firstName.Length < 2)
                return "Le prénom doit contenir au moins 2 caractères.";
            return null;
        }

        public static string? ValidateLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
                return "Le nom est requis.";
            if (lastName.Length < 2)
                return "Le nom doit contenir au moins 2 caractères.";
            return null;
        }

        public static string? ValidateAddress(string? address)
        {
            if (string.IsNullOrWhiteSpace(address))
                return "L'adresse est requise.";
            if (address.Length < 5)
                return "L'adresse semble trop courte.";
            return null;
        }

        public static string? ValidatePhone(string? phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return "Le numéro de téléphone est requis.";

            var regex = new Regex(@"^\d{9}$");
            if (!regex.IsMatch(phone.Trim()))
                return "Le numéro de téléphone doit contenir exactement 9 chiffres (sans indicatif).";

            return null;
        }


    }
}
