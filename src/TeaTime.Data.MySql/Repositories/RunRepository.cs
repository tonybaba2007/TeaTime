﻿namespace TeaTime.Data.MySql.Repositories
{
    using System.Threading.Tasks;
    using Common.Abstractions.Data;
    using Common.Models.Data;

    public class RunRepository : BaseRepository, IRunRepository
    {
        public RunRepository(ConnectionFactory factory) : base(factory)
        {
        }

        public Task CreateAsync(Run run)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateAsync(Run run)
        {
            throw new System.NotImplementedException();
        }

        public Task<Run> GetAsync(long runId)
        {
            throw new System.NotImplementedException();
        }

        public Task CreateResultAsync(RunResult result)
        {
            throw new System.NotImplementedException();
        }
    }
}
