using System.Data;

namespace CitnDev.System.Data
{
    public interface ICommandToSql
    {
        string Interpret(IDbCommand command);
    }
}
