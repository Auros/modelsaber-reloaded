using System;
using ModelSaber.Common;
using ModelSaber.API.Models;
using ModelSaber.API.Interfaces;

namespace ModelSaber.API.Services
{
    public interface IAuditor
    {
        void Audit(ISource source, User user, string action);
    }

    public class Auditor : IAuditor
    {
        private readonly ModelSaberContext _modelSaberContext;

        public Auditor(ModelSaberContext modelSaberContext)
        {
            _modelSaberContext = modelSaberContext;
        }

        public async void Audit(ISource source, User user, string action)
        {
            await _modelSaberContext.Audits.AddAsync(new Audit
            {
                User = user,
                Action = action,
                Time = DateTime.UtcNow,
                Source = source.SourceName
            });
            await _modelSaberContext.SaveChangesAsync();
        }
    }
}