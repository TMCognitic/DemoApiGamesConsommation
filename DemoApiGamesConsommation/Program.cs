using System.Net.Http.Headers;
using System.Text.Json;


namespace DemoApiGamesConsommation
{
    class Program
    {
        static void Main(string[] args)
        {
            //GetContacts();
            //GetOneContact(1);

            //Contact contact = new Contact() { LastName = "Stark", FirstName = "Tony"};
            //AddContact(contact);
            //GetContacts();

            //dans ma base de données Tony a obtenu l'id 5 (à changer dans votre cas)
            //Contact? tony = GetOneContact(5);

            //if (tony is not null)
            //{
            //    tony.LastName = "Alzeihmer";
            //    tony.FirstName = "Aloïs";

            //    UpdateContact(tony);
            //    GetContacts();
            //}

            //DeleteContact(5);
            //GetContacts();

            //Attention que le fait de récupéré du json dépend du retour de la méthode au niveau de l'api
        }

        private static void GetContacts()
        {
            Console.WriteLine("Récupération des contacts");
            //Instanciation d'un client http
            using (HttpClient client = new HttpClient())
            {
                //Définit l'adresse de base à partie de laquelle faire les appels
                client.BaseAddress = new Uri("https://localhost:7022/api/");

                //Lance un appel à l'url suivante : https://localhost:7022/api/contact
                HttpResponseMessage httpResponseMessage = client.GetAsync("Contact").Result;
                //On s'assure que l'appel s'est bien passé et que le serveur à répondu
                httpResponseMessage.EnsureSuccessStatusCode();
                //On récupère le json
                string json = httpResponseMessage.Content.ReadAsStringAsync().Result;

                //On transforme le json en objets
                IEnumerable<Contact>? contacts = JsonSerializer.Deserialize<Contact[]>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                if(contacts is not null)
                {
                    foreach (Contact item in contacts)
                    {
                        //item.Id:D4 : on affiche l'id en 4 chiffres
                        Console.WriteLine($"{item.Id:D4} : {item.LastName} {item.FirstName}");
                    }
                }
            }
        }
        private static Contact? GetOneContact(int id)
        {
            Console.WriteLine("Récupération d'un contact sur base de son Id");
            //Instanciation d'un client http
            using (HttpClient client = new HttpClient())
            {
                //Définit l'adresse de base à partie de laquelle faire les appels
                client.BaseAddress = new Uri("https://localhost:7022/api/");

                //Lance un appel à l'url suivante : https://localhost:7022/api/contact/1
                HttpResponseMessage httpResponseMessage = client.GetAsync($"Contact/{id}").Result;
                //On s'assure que l'appel s'est bien passé et que le serveur à répondu
                httpResponseMessage.EnsureSuccessStatusCode();
                //On récupère le json
                string json = httpResponseMessage.Content.ReadAsStringAsync().Result;

                //On transforme le json en objet
                Contact? contact = JsonSerializer.Deserialize<Contact>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                if (contact is not null)
                {
                    Console.WriteLine($"{contact.Id:D4} : {contact.LastName} {contact.FirstName}");
                }

                return contact;
            }
        }
        private static void AddContact(Contact contact)
        {
            Console.WriteLine("Ajout d'un contact");
            //Instanciation d'un client http
            using (HttpClient client = new HttpClient())
            {
                //Définit l'adresse de base à partie de laquelle faire les appels
                client.BaseAddress = new Uri("https://localhost:7022/api/");

                //On transforme le contact en Json en ne reprennant que les propriétés nécessaire
                string json = JsonSerializer.Serialize(new { contact.LastName, contact.FirstName });

                //On prépare le contenu de l'enveloppe 
                HttpContent httpContent = new StringContent(json);
                //On spécifie qu'il s'agit d'un contenu au format json
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                //Lance un appel en POST à l'url suivante : https://localhost:7022/api/contact en spécifiant le contenu de l'enveloppe
                HttpResponseMessage httpResponseMessage = client.PostAsync("Contact", httpContent).Result;
                //On s'assure que l'appel s'est bien passé
                httpResponseMessage.EnsureSuccessStatusCode();                
            }
        }

        private static void UpdateContact(Contact contact)
        {
            Console.WriteLine("Mise à jour d'un contacts");
            //Instanciation d'un client http
            using (HttpClient client = new HttpClient())
            {
                //Définit l'adresse de base à partie de laquelle faire les appels
                client.BaseAddress = new Uri("https://localhost:7022/api/");

                //On transforme le contact en Json en ne reprennant que les propriétés nécessaire
                string json = JsonSerializer.Serialize(contact);

                //On prépare le contenu de l'enveloppe 
                HttpContent httpContent = new StringContent(json);
                //On spécifie qu'il s'agit d'un contenu au format json
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                //Lance un appel en PUT à l'url suivante : https://localhost:7022/api/contact/5 en spécifiant le contenu de l'enveloppe
                HttpResponseMessage httpResponseMessage = client.PutAsync($"Contact/{contact.Id}", httpContent).Result;
                //On s'assure que l'appel s'est bien passé
                httpResponseMessage.EnsureSuccessStatusCode();
            }
        }

        private static void DeleteContact(int id)
        {
            Console.WriteLine("Suppression d'un contacts");
            //Instanciation d'un client http
            using (HttpClient client = new HttpClient())
            {
                //Définit l'adresse de base à partie de laquelle faire les appels
                client.BaseAddress = new Uri("https://localhost:7022/api/");

                //Lance un appel en DELETE à l'url suivante : https://localhost:7022/api/contact/5 en spécifiant le contenu de l'enveloppe
                HttpResponseMessage httpResponseMessage = client.DeleteAsync($"Contact/{id}").Result;
                //On s'assure que l'appel s'est bien passé
                httpResponseMessage.EnsureSuccessStatusCode();
            }
        }
    }
}


