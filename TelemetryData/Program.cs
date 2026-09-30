using System.Diagnostics;
using TelemetryDataNFSU2;
using System.Timers;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Drawing;

namespace TelemetryData
{
    using static Nfsu2Adresses;
    
    public class Program
    {
        public static float speedMph;
        public static int carId;
        public static string ?carName;
        public static string ?playerName;
        public static int money;
        public static float positionX;
        public static float positionY;
        public static float positionZ;
        static void Main(string[] args)
        {

            Process? nfsu2 = Process.GetProcessesByName("SPEED2").FirstOrDefault();

            if (nfsu2 == null)
            {
                Console.WriteLine("NFSU2 is not running!");
                return;
            }

            using ProcessMemory memory = new ProcessMemory(nfsu2.Id);
            CarDatabase carDatabase = new CarDatabase($"Cars.json");

            Stopwatch sw = Stopwatch.StartNew();

            while (true)
            {
                if(sw .ElapsedMilliseconds >= 500)
                {
                    Console.Clear();
                    sw.Restart();

                    speedMph = memory.ReadFloat(SpeedMph);
                    carId = memory.ReadInt32(CarId);
                    carName = carDatabase.GetCarName(carId);
                    playerName = memory.ReadString(PlayerName, 7);
                    money = memory.ReadInt32(Money);
                    positionX = memory.ReadFloat(PositionX);
                    positionY = memory.ReadFloat(PositionY);
                    positionZ = memory.ReadFloat(PositionZ);

                    Console.WriteLine("===========NFSU2 Telemetry Data===========");
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"-------Player Name: {playerName}---------");
                    Console.ResetColor();
                    Console.WriteLine();


                    Console.Write($"    Money: ");
                    Console.ForegroundColor= ConsoleColor.Green;
                    Console.WriteLine($"${money}");
                    Console.ResetColor();
                    Console.WriteLine($"    Speed in MPH: {speedMph:F0} MPH");
                    Console.WriteLine($"    Speed in KMH: {speedMph * 1.609344f:F0} KMH");
                    Console.WriteLine($"    Car: {carName}");
                    Console.WriteLine($"    Car ID: {carId}");
                    Console.WriteLine($"    Position: X: {positionX:F0}; Y: {positionY:F0}; Z: {positionZ:F0}");


                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("==========================================");

                }
                
                
            }

        }
    }
}
