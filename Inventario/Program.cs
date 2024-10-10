using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
namespace InventoryApp
{
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class InventoryItem
    {
        public string ItemName { get; set; }
        public int Quantity { get; set; }
    }

    class Program
    {
        static List<User> users = new List<User>();
        static List<InventoryItem> inventory = new List<InventoryItem>();
        private static User loggedInUser = null;

        const string UsersFilePath = "users.json";
        const string InventoryFilePath = "inventory.json";
        static void Main(string[] args)
        {
            LoadUsers();
            LoadInventory();
            users.Add(new User { UserName = "admin", Password = "admin" });
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n1 Registrar\n2. Login\n3. Añadir item en inventario\n4. Ver inventario\n5. Salir de cuenta\n6. Cerrar");
                Console.Write("Choose an option: ");
                int choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        Register();
                        break;
                    case 2:
                        Login();
                        break;
                    case 3:
                        AddInventoryItem();
                        break;
                    case 4:
                        ViewInventory();
                        break;
                    case 5:
                        Logout();
                        break;
                    case 6:
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("\nPorfavor pon un valor valido");
                        break;
                }
            }
        }

        static void LoadUsers()
        {
            if (File.Exists(UsersFilePath))
            {
             string json = File.ReadAllText(UsersFilePath);
             users = JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
            }
        }

        static void SaveUsers()
        {
            string json = JsonConvert.SerializeObject(users);
            File.WriteAllText(UsersFilePath, json);
        }
        static void LoadInventory()
        {
            if (File.Exists(InventoryFilePath))
            {
                string json = File.ReadAllText(InventoryFilePath);
                inventory = JsonConvert.DeserializeObject<List<InventoryItem>>(json) ?? new List<InventoryItem>();
            }
        }

        static void SaveInventory()
        {
            string json = JsonConvert.SerializeObject(inventory);
            inventory = JsonConvert.DeserializeObject<List<InventoryItem>>(json) ?? new List<InventoryItem>();
        }
        static void Register()
        {
            Console.Write("Introduce el usuario: ");
            string username = Console.ReadLine();
            
            Console.Write("Introduce la contraseña: ");
            string password = Console.ReadLine();
            
            users.Add(new User { UserName = username, Password = password });
            SaveUsers();
            Console.Write("¡Registro completo!");
        }

        static void Login()
        {
            Console.Write("Introduce el usuario: ");
            string username = Console.ReadLine();
            
            Console.Write("Introduce el password: ");
            string password = Console.ReadLine();

            foreach (var user in users)
            {
                if (user.UserName == username && user.Password == password)
                {
                    loggedInUser = user;
                    Console.WriteLine("Entraste satisfactoriamente");
                    return;
                }
            }
            Console.WriteLine("Invalid credentials");
        }

        static void AddInventoryItem()
        {
            if (loggedInUser == null)
            {
                Console.WriteLine("Ingrese el usuario para agregar item a inventario ");
                return;
            }
            Console.Write("Introduce el nombre del item: ");
            string itemName = Console.ReadLine();
            Console.Write("Ingresa la cantidad del item: ");
            int quantity = int.Parse(Console.ReadLine());
            inventory.Add(new InventoryItem { ItemName = itemName, Quantity = quantity });
            SaveInventory();
            Console.WriteLine("Se añadio el item satisfactoriamente");
        }

        static void ViewInventory()
        {
            LoadInventory();
            if (inventory.Count == 0)
            {
                Console.WriteLine("No hay inventario");
                return;
            }
            Console.WriteLine("Items de inventario:");
            foreach (var item in inventory)
            {
                Console.WriteLine($"Item: { item.ItemName } { item.Quantity }"
                    );
            }
        }

        static void Logout()
        {
            loggedInUser = null;
            Console.WriteLine("Saliste satisfactoriamente.");
        }
    }
}