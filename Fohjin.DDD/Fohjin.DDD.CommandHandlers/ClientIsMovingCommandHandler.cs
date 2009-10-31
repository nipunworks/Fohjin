using Fohjin.DDD.Commands;
using Fohjin.DDD.Contracts;
using Fohjin.DDD.Domain.Client;

namespace Fohjin.DDD.CommandHandlers
{
    public class ClientIsMovingCommandHandler : ICommandHandler<ClientIsMovingCommand>
    {
        private readonly IDomainRepository _repository;

        public ClientIsMovingCommandHandler(IDomainRepository repository)
        {
            _repository = repository;
        }

        public void Execute(ClientIsMovingCommand compensatingCommand)
        {
            var client = _repository.GetById<Client>(compensatingCommand.Id);

            client.ClientMoved(new Address(compensatingCommand.Street, compensatingCommand.StreetNumber, compensatingCommand.PostalCode, compensatingCommand.City));

            _repository.Save(client);
        }
    }
}