namespace Application.Core
{
    public interface IAutofacLocator
    {
        void Register();

        TInterface Get<TInterface>();

        TInterface Get<TInterface>(string type);
    }
}
