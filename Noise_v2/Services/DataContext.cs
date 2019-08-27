using Noise_v2.Entities;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace Noise_v2.Services
{
    public class DataContext
    {
        private const string STORE_FILENAME = @"noise-intervals.json";
        private string dataFile;

        public List<NoiseInterval> NoiseIntervals = new List<NoiseInterval>();

        public DataContext()
        {
            dataFile = System.AppDomain.CurrentDomain.BaseDirectory + STORE_FILENAME;
        }

        public bool Save()
        {
            // save data in a text file in the app directory
            string json = JsonConvert.SerializeObject(NoiseIntervals.ToArray(), Formatting.Indented);

            File.WriteAllText(dataFile, json);
            return true;
        }

        public IEnumerable<NoiseInterval> LoadData()
        {
            List<NoiseInterval> noiseIntervals = new List<NoiseInterval>();
            if (File.Exists(dataFile))
            {
                string json = File.ReadAllText(dataFile);
                noiseIntervals = JsonConvert.DeserializeObject<List<NoiseInterval>>(json);
            }
            return noiseIntervals;
        }
    }
}
