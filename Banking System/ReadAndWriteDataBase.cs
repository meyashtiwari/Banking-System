using System;
using System.Data.SqlClient;

class ReadAndWriteDataBase
{
    public UInt32 Account_Number;
    public Double Total_Balance;
    public string Title, Name, LastLoginDetails, Password;
    private SqlConnection Connection;

    public ReadAndWriteDataBase(string Server = "localhost\\SQLEXPRESS", string DatabaseName = "Banking-System")
    {
        string ConnectionString = "Server = " + Server + "; Database = " + DatabaseName + "; Integrated Security = SSPI";
        Connection = new SqlConnection(ConnectionString);
        try
        {
            Connection.Open();
        }
        catch (SqlException)
        {
            Console.WriteLine("There is an error while establishing a connection with the SqlServer");
            Console.ReadKey();
        }
    }
    private string GetQuery(int i)
    {
        string[] Query = new string[] {
            "select max(AccountNumber) from UserData",
            "select * from UserData where AccountNumber = " + Account_Number,
            "insert into UserData values(" + Account_Number + ",'" + Title + "','" + Name + "'," + Total_Balance + ",'" + Password + "'," + "SYSDATETIME())",
            "update UserData set Title = '" + Title + "', Name = '" + Name + "', TotalBalance = " + Total_Balance + ", LastLoginDetails = SYSDATETIME() where AccountNumber = " + Account_Number,
            "update UserData set TotalBalance = " + Total_Balance + " where AccountNumber = " + Account_Number,
            "select * from Passbook where AccountNumber = " + Account_Number
        };
        return Query[i];
    }
    public UInt32 GenerateAccountNumber()
    {
        SqlCommand Command = new SqlCommand(GetQuery(0), Connection);
        SqlDataReader DataReader = Command.ExecuteReader();
        DataReader.Read();
        if (("" + DataReader.GetValue(0)) != "")
            Account_Number = UInt32.Parse("" + DataReader.GetValue(0)) + 1;
        else
            Account_Number = 12081999;
        DataReader.Close();
        Command.Dispose();
        return Account_Number;
    }
    public void WriteToDatabase(int choice)
    {
        SqlCommand command = new SqlCommand(GetQuery(choice), Connection);
        command.ExecuteNonQuery();
        command.Dispose();
    }
    public SqlDataReader ReadPassbook()
    {
        SqlCommand command = new SqlCommand(GetQuery(5), Connection);
        SqlDataReader dataReader = command.ExecuteReader();
        return dataReader;
    }
    public void UpdatePassbook(Double Amount,string Description)
    {
        string Query = "insert into Passbook values(" + Account_Number + "," + Amount + ", SYSDATETIME(),'" + Description + "')";
        SqlCommand command = new SqlCommand(Query, Connection);
        command.ExecuteNonQuery();
        command.Dispose();
    }
    public bool ReadFromDatabase()
    {
        bool AccountFound = false;
        SqlCommand command = new SqlCommand(GetQuery(1), Connection);
        SqlDataReader dataReader = command.ExecuteReader();
        if (dataReader.Read())
        {
            Title = ("" + dataReader.GetValue(1));
            Name = ("" + dataReader.GetValue(2));
            Total_Balance = Double.Parse("" + dataReader.GetValue(3));
            Password = ("" + dataReader.GetValue(4));
            LastLoginDetails = ("" + dataReader.GetValue(5));
            AccountFound = true;
        }
        else
        {
            AccountFound = false;
        }
        dataReader.Close();
        command.Dispose();
        return AccountFound;
    }
    public void CloseConnection()
    {
        Connection.Close();
        Connection.Dispose();
    }
}