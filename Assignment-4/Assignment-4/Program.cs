using System;
using System.Collections.Generic;
using System.IO;
namespace Assignment_4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string RIDES_FILENAME = "rides.csv";
            List<Ride> rideList = new List<Ride>();
            bool redoProgram = true;

            if (File.Exists(RIDES_FILENAME))
            {
                LoadRideList(rideList, RIDES_FILENAME);
            }
            else
            {
                StartNewFile(rideList, RIDES_FILENAME);
            }

            Console.WriteLine("Welcome to Thrilladelphia Ride Tracker!\n");

            do
            {
                DisplayMainMenu();
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine().ToUpper();

                switch (choice)
                {
                    case "D":
                        DisplayAllRides(rideList);
                        break;
                    case "S":
                        SearchForRide(rideList);
                        break;
                    case "A":
                        AddRide(rideList);
                        break;
                    case "E":
                        EditRide(rideList);
                        break;
                    case "R":
                        RemoveRide(rideList);
                        break;
                    case "Q":
                        redoProgram = false;
                        break;
                    default:
                        Console.WriteLine("Unknown option. Please try again.");
                        break;
                }

            } while (redoProgram);

            SaveRideList(rideList, RIDES_FILENAME);
            Console.WriteLine("Goodbye!");
        }

        // ---------------------------- Menu ----------------------------

        static void DisplayMainMenu()
        {
            Console.WriteLine(
                "\nMain Menu\n" +
                "====================\n" +
                "[D] Display All Rides\n" +
                "[S] Search for a Ride\n" +
                "[A] Add a New Ride\n" +
                "[E] Edit a Ride\n" +
                "[R] Remove a Ride\n" +
                "[Q] Quit and Save\n"
            );
        }

        // ---------------------------- Input Helpers ----------------------------

        static int PromptInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int value))
                {
                    return value;
                }
                Console.WriteLine("Invalid number. Try again.");
            }
        }

        static double PromptDouble(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (double.TryParse(Console.ReadLine(), out double value))
                {
                    return value;
                }
                Console.WriteLine("Invalid number. Try again.");
            }
        }

        static bool AskYesOrNo()
        {
            while (true)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.Y) return true;
                if (key == ConsoleKey.N) return false;
            }
        }

        // ---------------------------- Ride Operations ----------------------------

        static void AddRide(List<Ride> list)
        {
            Console.Clear();
            Console.WriteLine("-- Add a New Ride --\n");

            Ride ride = new Ride();

            TryChangeRideDetail(ride, "all");

            Console.WriteLine("\nRide Details:");
            Console.WriteLine(ride.RideDetails());
            Console.Write("\nConfirm? (Y/N): ");

            if (AskYesOrNo())
            {
                list.Add(ride);
                Console.WriteLine("Ride added.");
            }
        }

        static void EditRide(List<Ride> list)
        {
            Console.Write("Enter ride name to edit: ");
            Ride ride = SearchRideRaw(list, Console.ReadLine());

            if (ride == null)
            {
                Console.WriteLine("Ride not found.");
                return;
            }

            TryChangeRideDetail(ride, "all");
            Console.WriteLine("Ride updated.");
        }

        static void RemoveRide(List<Ride> list)
        {
            Console.Write("Enter ride name to remove: ");
            Ride ride = SearchRideRaw(list, Console.ReadLine());

            if (ride == null)
            {
                Console.WriteLine("Ride not found.");
                return;
            }

            Console.WriteLine(ride.RideDetails());
            Console.Write("Confirm removal? (Y/N): ");

            if (AskYesOrNo())
            {
                list.Remove(ride);
                Console.WriteLine("Ride removed.");
            }
        }

        static void DisplayAllRides(List<Ride> list)
        {
            Console.WriteLine("\nName        Fright  Cost  Visitors");
            Console.WriteLine("==================================");

            foreach (Ride ride in list)
            {
                Console.WriteLine(ride.ToString());
            }
        }

        static void SearchForRide(List<Ride> list)
        {
            Console.Write("Enter ride name to search: ");
            Ride ride = SearchRideRaw(list, Console.ReadLine());

            if (ride != null)
            {
                Console.WriteLine(ride.RideDetails());
            }
            else
            {
                Console.WriteLine("Ride not found.");
            }
        }

        static Ride SearchRideRaw(List<Ride> list, string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;

            name = name.Trim().ToUpper();

            foreach (Ride ride in list)
            {
                if (ride.Name.ToUpper() == name)
                {
                    return ride;
                }
            }

            return null;
        }

        static void TryChangeRideDetail(Ride ride, string attribute)
        {
            try
            {
                if (attribute == "all")
                {
                    Console.Write("Enter ride name: ");
                    ride.Name = Console.ReadLine();

                    ride.FrightFactor = PromptInt("Enter fright factor (0–100): ");
                    ride.CostToEnter = PromptDouble("Enter cost to enter: ");
                    ride.VisitorsToday = PromptInt("Enter visitors today: ");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TryChangeRideDetail(ride, attribute);
            }
        }

        // ---------------------------- File Handling ----------------------------

        static void StartNewFile(List<Ride> list, string filename)
        {
            Console.WriteLine("No ride file found. Please add the first ride.");
            AddRide(list);
            SaveRideList(list, filename);
        }

        static void LoadRideList(List<Ride> list, string filename)
        {
            using StreamReader file = new StreamReader(filename);
            file.ReadLine();

            while (!file.EndOfStream)
            {
                string[] parts = file.ReadLine().Split(",");
                Ride ride = new Ride(
                    parts[0],
                    int.Parse(parts[1]),
                    double.Parse(parts[2]),
                    int.Parse(parts[3])
                );

                list.Add(ride);
            }
        }

        static void SaveRideList(List<Ride> list, string filename)
        {
            using StreamWriter file = new StreamWriter(filename);
            file.WriteLine("Name,FrightFactor,CostToEnter,VisitorsToday");

            foreach (Ride ride in list)
            {
                file.WriteLine(ride.ToCSVString());
            }
        }
    }
}
