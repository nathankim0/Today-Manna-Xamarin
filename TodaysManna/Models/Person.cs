using System;
namespace XamarinFirebase
{
    public class Person
    {
        public int PersonId { get; set; }
        public string Name { get; set; }
    }
}



/*
 * Firebase database crud sample

using System;
using Xamarin.Forms;

namespace XamarinFirebase
{
    public partial class MainPage : ContentPage
    {
        private readonly FirebaseHelper firebaseHelper = new FirebaseHelper();

        public MainPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            var allPersons = await firebaseHelper.GetAllPersons();
            lstPersons.ItemsSource = allPersons;
        }

        private async void BtnAdd_Clicked(object sender, EventArgs e)
        {
            try
            {
                await firebaseHelper.AddPerson(Convert.ToInt32(txtId.Text), txtName.Text);
                txtId.Text = "";// string.Empty;
                txtName.Text = "";//string.Empty;
                await DisplayAlert("Success", "Person Added Successfully", "OK");
                var allPersons = await firebaseHelper.GetAllPersons();
                lstPersons.ItemsSource = allPersons;
            }
            catch (Exception exception)
            {
                await DisplayAlert("Fail", exception.Message, "OK");

            }

        }

        private async void BtnRetrive_Clicked(object sender, EventArgs e)
        {
            try
            {
                var person = await firebaseHelper.GetPerson(Convert.ToInt32(txtId.Text));
                if (person != null)
                {
                    txtId.Text = person.PersonId.ToString();
                    txtName.Text = person.Name;
                    await DisplayAlert("Success", "Person Retrive Successfully", "OK");
                }
                else
                {
                    await DisplayAlert("Fail", "No Person Available", "OK");
                }
            }
            catch (Exception exception)
            {
                await DisplayAlert("Fail", exception.Message, "OK");

            }
        }

        private async void BtnUpdate_Clicked(object sender, EventArgs e)
        {
            try
            {
                await firebaseHelper.UpdatePerson(Convert.ToInt32(txtId.Text), txtName.Text);
                txtId.Text = string.Empty;
                txtName.Text = string.Empty;
                await DisplayAlert("Success", "Person Updated Successfully", "OK");
                var allPersons = await firebaseHelper.GetAllPersons();
                lstPersons.ItemsSource = allPersons;
            }
            catch (Exception exception)
            {
                await DisplayAlert("Fail", exception.Message, "OK");
            }
        }

        private async void BtnDelete_Clicked(object sender, EventArgs e)
        {
            try
            {
                var text = txtId.Text;

                await firebaseHelper.DeletePerson(text); // Convert.ToInt32(txtId.Text));
                await DisplayAlert("Success", "Person Deleted Successfully", "OK");
                var allPersons = await firebaseHelper.GetAllPersons();
                lstPersons.ItemsSource = allPersons;
            }
            catch (Exception exception)
            {
                await DisplayAlert("Fail", exception.Message, "OK");
            }
        }
    }
}



 */