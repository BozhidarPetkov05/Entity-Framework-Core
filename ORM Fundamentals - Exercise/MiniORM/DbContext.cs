using System.Data;
using System.Reflection;

namespace MiniORM
{
    public abstract class DbContext
    {
        private readonly DatabaseConnection _connection;
        public DbContext(string connectionString)
        {
            _connection = new DatabaseConnection(connectionString);
        }
        internal static HashSet<Type> AllowedSqlTypes { get; } = new HashSet<Type>
        {
            typeof(string),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(decimal),
            typeof(bool),
            typeof(DateTime),
        };
        private IDictionary<Type, PropertyInfo> DiscoverDbSets()
        {
            GetType().GetProperties();
        }
    }
}
