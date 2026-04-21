namespace Muradov.Mediator
{
    public interface IRequest { }

    public interface IRequest<out TResponse> { }
}
