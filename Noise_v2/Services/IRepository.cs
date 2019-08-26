using Noise_v2.Entities;
using System.Collections.Generic;

namespace Noise_v2.Services
{
    public interface IRepository
    {
        IEnumerable<NoiseInterval> GetNoiseIntervals();
        void AddNoiseInterval(NoiseInterval noiseInterval);
        void DeleteNoiseInterval(NoiseInterval noiseInterval);
        bool Save();
    }
}
