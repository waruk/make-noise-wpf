using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Noise_v2.Entities;
using Newtonsoft.Json.Serialization;

namespace Noise_v2.Services
{
    public class Repository : IRepository
    {
        List<NoiseInterval> _data = new List<NoiseInterval>();

        public void AddNoiseInterval(NoiseInterval noiseInterval)
        {
            _data.Add(noiseInterval);
        }

        public void DeleteNoiseInterval(NoiseInterval noiseInterval)
        {
            _data.RemoveAll(ni => ni.NoiseIntervalId == noiseInterval.NoiseIntervalId);
        }

        public IEnumerable<NoiseInterval> GetNoiseIntervals()
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }
    }
}
