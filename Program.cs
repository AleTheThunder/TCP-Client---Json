using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;

namespace TCP_Client___Json
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TcpClient client = new TcpClient();
                client.Connect(IPAddress.Parse("127.0.0.1"), 8080);

                NetworkStream stream = client.GetStream();

                string password = "meinPasswort";
                byte[] passwordBytes = Encoding.ASCII.GetBytes(password);

                // Ausgabe des zu sendenden Passworts
                Console.WriteLine($"Sende Passwort: {password}");

                stream.Write(passwordBytes, 0, passwordBytes.Length);

                byte[] buffer = new byte[256];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                // Ausgabe der JSON-Rohdaten
                Console.WriteLine($"Server antwortet (Raw JSON): {response}");

                if (response.StartsWith("{")) // Überprüfe, ob die Antwort JSON-Daten enthält
                {
                    // Deserialisiere JSON-Daten
                    Person person = JsonConvert.DeserializeObject<Person>(response);
                    Console.WriteLine($"Server antwortet: Name = {person.Name}, Alter = {person.Alter}");
                }
                else
                {
                    Console.WriteLine($"Server antwortet: {response}");
                }

                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public class Person
        {
            public string Name { get; set; }
            public int Alter { get; set; }
        }
    }
}
