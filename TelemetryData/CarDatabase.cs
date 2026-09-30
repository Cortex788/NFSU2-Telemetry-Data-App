using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace TelemetryDataNFSU2
{
    public class CarDatabase
    {
        private readonly Dictionary<int, string>? _cars;

        public CarDatabase(string FilePath)
        {
            string json = File.ReadAllText(FilePath);

            _cars = JsonSerializer.Deserialize<Dictionary<int, string>>(json)
                ?? new Dictionary<int, string>();
        }

        public string GetCarName(int carId)
        {
            if (_cars!.TryGetValue(carId, out string? carName))
                return carName;

            return "Unknown";
        }

        public string GetCarId(int carId)
        {
            if (_cars!.TryGetValue(carId, out string? carName))
                return carId.ToString();

            return "Unknown";
        }
    }
}
