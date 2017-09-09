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
        Console.SetWindowSize(100, 25);
        Console.Title = "GTBPI Banking System";
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
        Console.Clear();
        Center("**** GTBPI Banking System | Signup Page ****");
        DrawLine();
        Console.Write("{0}",AlignText(28,"Your Full Name          : ","L"));
        Name = Console.ReadLine();
        Console.Write("{0}",AlignText(28,"Your gender[M/F]        : ","L"));
        char Gender = Console.ReadLine()[0];
        Title = (Gender == 'M' || Gender == 'm') ? "Mr" : "Ms"; 
        Console.Write("{0}",AlignText(28,"Password[max 21 chars]  : ","L"));
        Password = Console.ReadLine();
        Console.Write("{0}",AlignText(28,"Enter amount to deposit : ","L"));
        Total_Balance = UInt32.Parse(Console.ReadLine());
        GenerateAccountNumber();
        SetDataToTheDatabase(2);
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.ForegroundColor = ConsoleColor.White;
        DrawLine();
        Console.WriteLine("|{0}|",AlignText(13,"Thanks for banking with us | Your generated account number is " + Account_Number));
        DrawLine();
        Console.ResetColor();
        Center("Please note down your account number and password!");
    }
    private int Menu()
    {
        Console.Clear();
        Center("**** Welcome to GTBPI Banking System ****");
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.ForegroundColor = ConsoleColor.White;
        DrawLine();
        Console.WriteLine("|{0}|", AlignText(35, ""));
        Console.WriteLine("|{0}|",AlignText(35,"1. Login for Existing Customers"));
        Console.WriteLine("|{0}|",AlignText(35,"2. Open a new Account"));
        Console.WriteLine("|{0}|",AlignText(35,"3. About Us"));
        Console.WriteLine("|{0}|",AlignText(35,"4. Exit"));
        Console.WriteLine("|{0}|", AlignText(35, ""));
        DrawLine();
        Console.ResetColor();
        Console.Write("Enter your choice : ");
        return (int.Parse(Console.ReadLine()));
    }
    private int LoggedInMenu()
    {
        Console.Clear();
        Center("**** GTBPI Banking System | Welcome " + Title + ". " + Name + " ****");
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.ForegroundColor = ConsoleColor.White;
        DrawLine();
        Console.WriteLine("|{0}|", AlignText(35, ""));
        Console.WriteLine("|{0}|",AlignText(37,"1. Deposit Money"));
        Console.WriteLine("|{0}|",AlignText(37,"2. Withdraw Money"));
        Console.WriteLine("|{0}|",AlignText(37,"3. Tranfer Money"));
        Console.WriteLine("|{0}|",AlignText(37,"4. Show My Account Details"));
        Console.WriteLine("|{0}|",AlignText(37,"5. Logout"));
        Console.WriteLine("|{0}|", AlignText(25, ""));
        DrawLine();
        Console.ResetColor();
        Console.Write("Enter your choice : ");
        return (int.Parse(Console.ReadLine()));
    }
    private void ShowUserDetails()
    {
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.ForegroundColor = ConsoleColor.White;
        DrawLine();
        Console.WriteLine("|{0}|", AlignText(35, ""));
        Console.WriteLine("|{0}|",AlignText(25, "Account Number            :  " + Account_Number));
        Console.WriteLine("|{0}|",AlignText(25, "Account Holder's Name     :  " + Title + ". " + Name));
        Console.WriteLine("|{0}|",AlignText(25, "Total Balance in account  :  " + "Rs. " + Total_Balance));
        Console.WriteLine("|{0}|",AlignText(25, "Last Login Details        :  " + LastLoginDetails));
        Console.WriteLine("|{0}|", AlignText(35, ""));
        DrawLine();
        Console.ResetColor();
    }
    private void About()
    {
        Console.Clear();
        Center("**** GTBPI Banking System | About Us ****");
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.ForegroundColor = ConsoleColor.White;
        DrawLine();
        Console.WriteLine("|{0}|", AlignText(34, ""));
        Console.WriteLine("|{0}|",AlignText(34,"GTBPI Banking System v1.2.0.8"));
        Console.WriteLine("|{0}|",AlignText(35,"Developed By Yash Bhardwaj"));
        Console.WriteLine("|{0}|",AlignText(34,"Computer Engineering[Vth Sem]"));
        Console.WriteLine("|{0}|", AlignText(34, ""));
        DrawLine();
        Console.ResetColor();
    }
    private static string AlignText(int SpacesToAdd, string Msg, string Alignment = "R")
    {
        if (Alignment == "L")
        {
            Msg = Msg.PadLeft(SpacesToAdd + Msg.Length);
        }
        else
        {
            Msg = Msg.PadLeft(SpacesToAdd + Msg.Length);
            Msg = Msg.PadRight((98 - Msg.Length) + Msg.Length);
        }
        return Msg;
    }
    private static void DrawLine()
    {
        Console.WriteLine("+--------------------------------------------------------------------------------------------------+");
    }
    private static void Center(string message)
    {
        int spaces = 50 + (message.Length / 2);
        Console.WriteLine(message.PadLeft(spaces));
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
            Console.ReadKey();
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
            Console.ReadKey();
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
        Console.Clear();
        Center("**** GTBPI Banking System | Login Page ****");
        DrawLine();
        Console.Write("{0}",AlignText(27,"Enter your account number   :  ","L"));
        Account_Number = UInt32.Parse(Console.ReadLine());
        EstablishConnectionWithDatabase();
        if (GetDataFromTheDatabase())
        {
            Console.Write("{0}", AlignText(27,"Enter your account password :  ","L"));
            string UserPassword = Console.ReadLine();
            if (UserPassword == Password)
            {
                bool LoggedInFlag = true;
                while (LoggedInFlag)
                {
                    switch (LoggedInMenu())
                    {
                        case 1: DepositMoney();
                            break;
                        case 2: WithdrawMoney();
                            break;
                        case 3: TransferMoney();
                            break;
                        case 4: ShowUserDetails();
                            break;
                        case 5: Logout();
                            Center("Thanks for using our service!");
                            Center("You have been successfully logged out!");
                            Center("Press any key to go back to the main menu!");
                            LoggedInFlag = false;
                            break;
                        default: Center("Incorrect Option | Try Again!");
                            break;
                    }
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("The password you entered is incorrect");
                Console.ReadKey();
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
            Console.ReadKey();
        }
    }
    private static void TransferMoney()
    {
        Console.Write("Enter amount you want to transfer : ");
        Double TransferAmount = Double.Parse(Console.ReadLine());
        if (TransferAmount <= Total_Balance)
        {
            Console.Write("Enter Account Number to which you want to transfer the amount : ");
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
                    Console.WriteLine(TransferAmount + " has been successfully transfered to " + dataReader.GetValue(0) + ". " + dataReader.GetValue(1) + "[" + TransferAccount + "]");
                }
                else
                {
                    Console.WriteLine("The transaction has been aborted");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("Sorry but the account number : " + Account_Number + " does not exist in our database");
                Console.WriteLine("Please check the account number and try again!");
                Console.ReadKey();
            }
            dataReader.Close();
            command.Dispose();
            connection.Close();
        }
        else
        {
            Console.WriteLine("You don't have sufficient balance in your account to complete this transaction");
            Console.ReadKey();
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
                    Center("Press any key to go back to the main menu!");
                    Console.ReadKey();
                    Console.Clear();
                    break;
                case 4: Center("Thanks for using our service!");
                    Center("Press any key to close the console!");
                    Console.ReadKey();
                    Environment.Exit(0);
                    break;
                default: Center("Incorrect Option | Try Again!");
                    Console.ReadKey();
                    Console.Clear();
                    break;
            }
        }
    }
}