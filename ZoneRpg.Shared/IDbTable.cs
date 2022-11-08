namespace ZoneRpg.Database
{
    public interface IDbTable
    {
        string GetTableName();
        string[] GetColumns();
        object[] GetValues();
    }

}