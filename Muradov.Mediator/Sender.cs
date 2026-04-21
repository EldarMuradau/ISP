using System.Reflection;

namespace Muradov.Mediator
{
    public class Sender : ISender
    {
        private readonly Assembly? _assembly;
        private readonly Dictionary<Type, Type> _handlerImplementations = new();

        public Sender(Assembly? assembly = null)
        {
            _assembly = assembly ?? Assembly.GetCallingAssembly();
            RegisterHandlers(_assembly);
        }

        private void RegisterHandlers(Assembly assembly)
        {
            var handlerTypes = assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract)
                .SelectMany(type => type.GetInterfaces()
                    .Where(templateType => templateType.IsGenericType)
                    .Select(templateType => new
                    {
                        InterfaceType = templateType,
                        ImplementationType = type
                    }))
                .Where(typeCombination => typeCombination.InterfaceType.GetGenericTypeDefinition() == typeof(IRequestHandler<>) ||
                       typeCombination.InterfaceType.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));

            foreach (var handler in handlerTypes)
            {
                _handlerImplementations[handler.InterfaceType] = handler.ImplementationType;
            }
        }

        public void Send<TRequest>(TRequest request) where TRequest : IRequest
        {
            var requestType = request!.GetType();
            var handlerType = typeof(IRequestHandler<>).MakeGenericType(requestType);

            if (!_handlerImplementations.TryGetValue(handlerType, out var implementationType))
            {
                throw new InvalidOperationException($"No handler registered for {requestType.Name}.");
            }

            var handler = Activator.CreateInstance(implementationType);
            var method = implementationType.GetMethod("Handle");

            var task = (Task)method!.Invoke(handler, new object?[] { request, CancellationToken.None })!;
            task.GetAwaiter().GetResult();
        }

        public TResponse Send<TResponse>(IRequest<TResponse> request)
        {
            Type handlerType = typeof(IRequestHandler<,>)
                 .MakeGenericType(request.GetType(), typeof(TResponse));

            try
            {
                var handler = CreateHandler(handlerType);
                return handler.Handle((dynamic)request);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        dynamic CreateHandler(Type handlerType)
        {
            var implementationType = _handlerImplementations[handlerType];
            if (implementationType is not null)
            {
                return Activator.CreateInstance(implementationType)!;
            }

            throw new NotImplementedException("Handler not found.");
        }
    }
}
