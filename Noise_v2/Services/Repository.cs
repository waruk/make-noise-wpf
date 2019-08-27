using Noise_v2.Entities;
using System.Collections.Generic;

namespace Noise_v2.Services
{
    public class Repository : IRepository
    {
        private DataContext _dataContext;

        public Repository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void AddNoiseInterval(NoiseInterval noiseInterval)
        {
            _dataContext.NoiseIntervals.Add(noiseInterval);
        }

        public void DeleteNoiseInterval(NoiseInterval noiseInterval)
        {
            _dataContext.NoiseIntervals.RemoveAll(ni => ni.NoiseIntervalId == noiseInterval.NoiseIntervalId);
        }

        public IEnumerable<NoiseInterval> GetNoiseIntervals()
        {
            return _dataContext.NoiseIntervals;
        }

        public bool Save()
        {
            return (_dataContext.Save());
        }
    }
}
