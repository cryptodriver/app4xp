using SQLite;

namespace Application.Infrastructure.SQLiter
{
    public interface ISQLiter
    {
        SQLiteConnection DBC();
        SQLiteConfig Config { get; }
    }
}
