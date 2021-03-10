using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Offline;
using Firebase.Database.Query;

namespace XamarinFirebase
{
    public class FirebaseHelper
    {
        private const string endpoint = "https://xamarinfirebase-63700-default-rtdb.firebaseio.com/";
        FirebaseClient firebase = new FirebaseClient(endpoint);

        public async Task<List<Person>> GetAllPersons()
        {
            return (await firebase
              .Child("nathan")
              .OnceAsync<Person>()).Select(item => new Person
              {
                  Name = item.Object.Name,
                  PersonId = item.Object.PersonId
              }).ToList();
        }

        public async Task AddPerson(int personId, string name)
        {
            await firebase
              .Child("nathan")
              .PostAsync(new Person() { PersonId = personId, Name = name });
        }

        public async Task<Person> GetPerson(int personId)
        {
            var allPersons = await GetAllPersons();
            await firebase
              .Child("nathan")
              .OnceAsync<Person>();
            return allPersons.Where(a => a.PersonId == personId).FirstOrDefault();
        }

        public async Task UpdatePerson(int personId, string name)
        {
            var toUpdatePerson = (await firebase
              .Child("nathan")
              .OnceAsync<Person>()).Where(a => a.Object.PersonId == personId).FirstOrDefault();

            await firebase
              .Child("nathan")
              .Child(toUpdatePerson.Key)
              .PutAsync(new Person() { PersonId = personId, Name = name });
        }

        public async Task DeletePerson(string personId)
        {
            var toDeletePerson = (await firebase
              .Child("nathan")
              .OnceAsync<Person>()).Where(a => a.Object.PersonId.ToString() == personId).FirstOrDefault();
            await firebase.Child("Persons").Child(toDeletePerson.Key).DeleteAsync();

        }

    }
}
