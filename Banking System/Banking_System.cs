using System;
using System.Data.SqlClient;

class Banking_System
{
    static private UInt32 Account_Number;
    static private Double Total_Balance;
    static private string Title, Name, LastLoginDetails, Password;
    static private SqlConnection connection;

    private int Menu()
    {
        Console.WriteLine("1. Login for Existing Customers");
        Console.WriteLine("2. Open a new Account");
        Console.WriteLine("3. About Us");
        Console.WriteLine("4. Exit");
        Console.Write("Enter your choice : ");
        return (int.Parse(Console.ReadLine()));
    }

    private static string ReturnSqlCommand(int i)
    {
        string[] sqlCommand = new string[] {"select max(AccountNumber) from UserData",
                                            "select * from UserData where AccountNumber = " + Account_Number,
                                            "insert into UserData values(" + Account_Number + ",'" + Title + "','" + Name + "'," + Total_Balance + ",'" + Password + "'," + "SYSDATETIME())",
                                            "update UserData set Title = '" + Title + "', Name = '" + Name + "', TotalBalance = " + Total_Balance + ", LastLoginDetails = SYSDATETIME() where AccountNumber = " + Account_Number
                                           };
        return sqlCommand[i];
    }

private void SignUp()
    {
        Console.Write("Enter your Full Name : ");
        Name = Console.ReadLine();
        Console.Write("Enter your gender[M/F] : ");
        string Gender = Console.ReadLine();
        Title = (Gender == "M" || Gender == "m") ? "Mr" : "Ms"; 
        Console.Write("Enter a password for your account[min 8 characters] : ");
        Password = Console.ReadLine();
        Console.Write("Enter the amount you want to deposit[Min Rs. 500] : ");
        Total_Balance = UInt32.Parse(Console.ReadLine());
        GenerateAccountNumber();
        SetDataToTheDatabase(1);
        Console.WriteLine("Thanks for banking with us | Your generated account number is " + Account_Number);
        Console.WriteLine("Please note down your account number and password");
    }   

    private static void GenerateAccountNumber()
    {
        EstablishConnectionWithDatabase();
        SqlCommand command = new SqlCommand(ReturnSqlCommand(0), connection);
        SqlDataReader dataReader = command.ExecuteReader();
        dataReader.Read();
        if (("" + dataReader.GetValue(0)) != "")
        {
            Account_Number = UInt32.Parse("" + dataReader.GetValue(0)) + 1;
        }
        else
        {
            Account_Number = 12081999;
        }
        dataReader.Close();
        command.Dispose();
        connection.Close();
    }

    private void About()
    {
        Console.WriteLine("-------------------------------------------------------------------");
        Console.WriteLine("{0,40}","Banking System v1.0");
        Console.WriteLine("{0,45}","Developed By Megha Attri");
        Console.WriteLine("{0,50}","Computer Engineering[Vth Sem]");
        Console.WriteLine("-------------------------------------------------------------------");
    }

    private int LoggedInMenu()
    {
        Console.WriteLine("1. Deposit Money");
        Console.WriteLine("2. Withdraw Money");
        Console.WriteLine("3. Tranfer Money");
        Console.WriteLine("4. Logout");
        Console.Write("Enter your choice : ");
        return (int.Parse(Console.ReadLine()));
    }

    private void ShowUserDetails()
    {
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("\nAccount Number            : {0,10}", Account_Number);
        Console.WriteLine("Account Holder's Name     : {0,17}", Title + ". " +Name);
        Console.WriteLine("Total Balance in account  : {0,10}", "Rs. " + Total_Balance);
        Console.WriteLine("Last Login Details        : {0,21}", LastLoginDetails);
        Console.ResetColor();
    }

    private static void EstablishConnectionWithDatabase()
    {
        string ConnectionString = "Server = localhost\\SQLEXPRESS; Database = Banking-System;Integrated Security = SSPI";
        connection = new SqlConnection(ConnectionString);

        try
        {
            connection.Open();
        }
        catch (SqlException ex)
        {
            Console.WriteLine("There is an error while establishing a connection with the SqlServer");
        }
    }

    private static bool GetDataFromTheDatabase()
    {
        bool AccountFound = false;
        SqlCommand command = new SqlCommand(ReturnSqlCommand(1), connection);
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
            Console.WriteLine("Sorry but the account number : " + Account_Number + " does not exist in our database");
            AccountFound = false;
        }
        dataReader.Close();
        command.Dispose();
        connection.Close();
        return AccountFound;
    }

    private static void SetDataToTheDatabase(int choice)
    {
        EstablishConnectionWithDatabase();
        string LocalSqlCommand = (choice == 1) ? ReturnSqlCommand(2) : ReturnSqlCommand(3);
        SqlCommand command = new SqlCommand(LocalSqlCommand, connection);
        command.ExecuteNonQuery();
        command.Dispose();
        connection.Close();
    }

    private void Login()
    {
        Console.Write("Enter your account number : ");
        Account_Number = UInt32.Parse(Console.ReadLine());
        EstablishConnectionWithDatabase();
        if (GetDataFromTheDatabase())
        {
            Console.Write("Enter your account password : ");
            string UserPassword = Console.ReadLine();
            if (UserPassword == Password)
            {
                bool LoggedInFlag = true;
                ShowUserDetails();
                while (LoggedInFlag)
                {
                    switch (LoggedInMenu())
                    {
                        case 1:
                            DepositMoney();
                            break;
                        case 2:
                            WithdrawMoney();
                            break;
                        case 3:
                            TransferMoney();
                            break;
                        case 4:
                            ShowUserDetails();
                            break;
                        case 5:
                            Logout();
                            LoggedInFlag = false;
                            break;
                        default:
                            Console.WriteLine("Incorrect Option");
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("The password you entered is incorrect");
            }
        }
    }

    private static void DepositMoney()
    {
        Console.Write("Enter amount you want to deposit : ");
        Total_Balance += UInt32.Parse(Console.ReadLine());
        Console.WriteLine("Amount deposited in your account successfully!");
    }

    private static void WithdrawMoney()
    {
        Console.Write("Enter amount you want to withdraw : ");
        Total_Balance -= UInt32.Parse(Console.ReadLine());
        Console.WriteLine("Amount withdrawal from your account was successfull!");
    }

    private static void TransferMoney()
    {

    }

    private static void Logout()
    {
        EstablishConnectionWithDatabase();
        SetDataToTheDatabase(2);
        Account_Number = 0;
        Total_Balance = 0;
        Title = Name = LastLoginDetails = Password = "";
    }

    static void Main(string[] args)
    {
        Banking_System obj = new Banking_System();
        while(true)
        {
            switch (obj.Menu())
            {
                case 1: obj.Login();
                    break;
                case 2: obj.SignUp();
                    break;
                case 3: obj.About();
                    break;
                case 4: Environment.Exit(0);
                    break;
                default: Console.WriteLine("Incorrect Option | Try Again");
                    break;
            }
        }
    }
}