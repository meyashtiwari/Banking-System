using System;
using System.Data.SqlClient;

class Banking_System
{
    static private UInt32 Account_Number, TransferAccount;
    static private Double Total_Balance, TransferBalance;
    static private string Title, Name, LastLoginDetails, Password;
    static private SqlConnection connection;

    public Banking_System()
    {
        Console.SetWindowSize(72, 25);
    }

    private static string ReturnSqlCommand(int i)
    {
        string[] sqlCommand = new string[] {
                                            "select max(AccountNumber) from UserData",
                                            "select * from UserData where AccountNumber = " + Account_Number,
                                            "insert into UserData values(" + Account_Number + ",'" + Title + "','" + Name + "'," + Total_Balance + ",'" + Password + "'," + "SYSDATETIME())",
                                            "update UserData set Title = '" + Title + "', Name = '" + Name + "', TotalBalance = " + Total_Balance + ", LastLoginDetails = SYSDATETIME() where AccountNumber = " + Account_Number,
                                            "select Title, Name, TotalBalance from UserData where AccountNumber = " + TransferAccount,
                                            "update UserData set TotalBalance = " + TransferBalance + " where AccountNumber = " + TransferAccount
                                           };
        return sqlCommand[i];
    }
    private void SignUp()
    {
        Console.WriteLine("------------------------------------------------------------------------");
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
        SetDataToTheDatabase(2);
        Console.WriteLine("------------------------------------------------------------------------");
        Console.WriteLine("Thanks for banking with us | Your generated account number is " + Account_Number);
        Console.WriteLine("Please note down your account number and password");
        Console.WriteLine("------------------------------------------------------------------------");
    }
    private int Menu()
    {
        Console.WriteLine("------------------------------------------------------------------------");
        Console.WriteLine("1. Login for Existing Customers");
        Console.WriteLine("2. Open a new Account");
        Console.WriteLine("3. About Us");
        Console.WriteLine("4. Exit");
        Console.WriteLine("------------------------------------------------------------------------");
        Console.Write("Enter your choice : ");
        return (int.Parse(Console.ReadLine()));
    }
    private int LoggedInMenu()
    {
        Console.WriteLine("1. Deposit Money");
        Console.WriteLine("2. Withdraw Money");
        Console.WriteLine("3. Tranfer Money");
        Console.WriteLine("4. Show My Account Details");
        Console.WriteLine("5. Logout");
        Console.Write("Enter your choice : ");
        return (int.Parse(Console.ReadLine()));
    }
    private void ShowUserDetails()
    {
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("\nAccount Number            : {0,10}", Account_Number);
        Console.WriteLine("Account Holder's Name     : {0,17}", Title + ". " + Name);
        Console.WriteLine("Total Balance in account  : {0,10}", "Rs. " + Total_Balance);
        Console.WriteLine("Last Login Details        : {0,21}", LastLoginDetails);
        Console.ResetColor();
    }
    private void About()
    {
        Console.WriteLine("------------------------------------------------------------------------");
        Console.WriteLine("|{0}|",AlignText(22,"Both","Banking System v1.0"));
        Console.WriteLine("|{0}|",AlignText(22,"Both","Developed By Megha"));
        Console.WriteLine("|{0}|",AlignText(22,"Both","Computer Engineering[Vth Sem]"));
        Console.WriteLine("------------------------------------------------------------------------");
    }
    private static string AlignText(int SpacesToAdd, string alignment, string Msg)
    {
        if (alignment == "Left")
            Msg = (Msg.PadLeft(SpacesToAdd + Msg.Length));
        else if (alignment == "Right")
            Msg = (Msg.PadRight(SpacesToAdd + Msg.Length));
        else
        {
            Msg = Msg.PadLeft(SpacesToAdd + Msg.Length);
            Msg = Msg.PadRight((70 - Msg.Length) + Msg.Length);
        }
        return Msg;
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
    private static void EstablishConnectionWithDatabase()
    {
        string ConnectionString = "Server = localhost\\SQLEXPRESS; Database = Banking-System; Integrated Security = SSPI";
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
        string LocalSqlCommand = ReturnSqlCommand(choice);
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
        Double WithDrawalAmount = Double.Parse(Console.ReadLine());
        if (WithDrawalAmount <= Total_Balance)
        {
            Total_Balance -= WithDrawalAmount;
            Console.WriteLine("Amount withdrawal from your account was successfull!");
        }
        else
        {
            Console.WriteLine("You don't have sufficient balance in your account to complete this transaction");
        }
    }
    private static void TransferMoney()
    {
        Console.Write("Enter amount you want to transfer : ");
        Double TransferAmount = Double.Parse(Console.ReadLine());
        if (TransferAmount <= Total_Balance)
        {
            Console.Write("Enter Account Number to which you want to transfer the funds to : ");
            TransferAccount =  UInt32.Parse(Console.ReadLine());
            EstablishConnectionWithDatabase();
            SqlCommand command = new SqlCommand(ReturnSqlCommand(4), connection);
            SqlDataReader dataReader = command.ExecuteReader();
            
            if (dataReader.Read())
            {
                Console.WriteLine("The account number : " + TransferAccount + " belongs to " + dataReader.GetValue(0) + ". " + dataReader.GetValue(1));
                Console.Write("Do you want to proceed with this transaction[y/n] : ");
                char choice = Console.ReadLine()[0];
                if (choice == 'y' || choice == 'Y')
                {
                    TransferBalance = Double.Parse("" + dataReader.GetValue(2));
                    TransferBalance += TransferAmount;
                    Total_Balance -= TransferAmount;
                    SetDataToTheDatabase(5);
                    SetDataToTheDatabase(3);
                    Console.WriteLine("Amount has been successfully transfered from your account to " + dataReader.GetValue(0) + ". " + dataReader.GetValue(1) + "[" + TransferAccount + "]");
                }
                else
                {
                    Console.WriteLine("The transaction has been aborted");
                }
            }
            else
            {
                Console.WriteLine("Sorry but the account number : " + Account_Number + " does not exist in our database");
                Console.WriteLine("Please check the account number and try again!");
            }
            dataReader.Close();
            command.Dispose();
            connection.Close();
        }
        else
        {
            Console.WriteLine("You don't have sufficient balance in your account to complete this transaction");
        }
    }
    private static void Logout()
    {
        EstablishConnectionWithDatabase();
        SetDataToTheDatabase(3);
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
                    Console.Clear();
                    break;
                case 2: obj.SignUp();
                    Console.ReadKey();
                    Console.Clear();
                    break;
                case 3: obj.About();
                    Console.WriteLine("Press any key to go back to the main menu!");
                    Console.ReadKey();
                    Console.Clear();
                    break;
                case 4: Environment.Exit(0);
                    break;
                default: Console.WriteLine("Incorrect Option | Try Again");
                    Console.ReadKey();
                    Console.Clear();
                    break;
            }
        }
    }
}