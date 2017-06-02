namespace ControllerAndWorkspacesExample.Services
{
    using System;

    public sealed class UniqueService : IUniqueService
    {
        private readonly Guid _id;

        public UniqueService()
        {
            _id = Guid.NewGuid();
        }

        public Guid Id { get { return _id; } }
    }
}