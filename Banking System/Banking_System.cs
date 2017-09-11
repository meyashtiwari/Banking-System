using System;

class Banking_System
{
    ReadAndWriteDataBase User = new ReadAndWriteDataBase();
    public Banking_System()
    {
        Console.SetWindowSize(100, 25);
        Console.Title = "GTBPI Banking System[v1.2.0.8]";
    }
    private void SignUp()
    {
        Console.Clear();
        Center("**** GTBPI Banking System | Signup Page ****");
        DrawLine();
        Console.Write("{0}",AlignText(28,"Your Full Name          : ","L"));
        User.Name = Console.ReadLine();
        Console.Write("{0}",AlignText(28,"Your gender[M/F]        : ","L"));
        char Gender = Console.ReadLine()[0];
        User.Title = (Gender == 'M' || Gender == 'm') ? "Mr" : "Ms"; 
        Console.Write("{0}",AlignText(28,"Password[max 21 chars]  : ","L"));
        User.Password = Console.ReadLine();
        Console.Write("{0}",AlignText(28,"Enter amount to deposit : ","L"));
        User.Total_Balance = UInt32.Parse(Console.ReadLine());
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.ForegroundColor = ConsoleColor.White;
        DrawLine();
        Console.WriteLine("|{0}|",AlignText(13,"Thanks for banking with us | Your generated account number is " + User.GenerateAccountNumber()));
        User.WriteToDatabase(2);
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
        Console.WriteLine("|{0}|",AlignText(35, ""));
        Console.WriteLine("|{0}|",AlignText(35,"1. Login for Existing Customers"));
        Console.WriteLine("|{0}|",AlignText(35,"2. Open a new Account"));
        Console.WriteLine("|{0}|",AlignText(35,"3. About Us"));
        Console.WriteLine("|{0}|",AlignText(35,"4. Exit"));
        Console.WriteLine("|{0}|",AlignText(35, ""));
        DrawLine();
        Console.ResetColor();
        Console.Write("Enter your choice : ");
        return (int.Parse(Console.ReadLine()));
    }
    private int LoggedInMenu()
    {
        Console.Clear();
        Center("**** GTBPI Banking System | Welcome " + User.Title + ". " + User.Name + " ****");
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.ForegroundColor = ConsoleColor.White;
        DrawLine();
        Console.WriteLine("|{0}|",AlignText(35, ""));
        Console.WriteLine("|{0}|",AlignText(37,"1. Deposit Money"));
        Console.WriteLine("|{0}|",AlignText(37,"2. Withdraw Money"));
        Console.WriteLine("|{0}|",AlignText(37,"3. Tranfer Money"));
        Console.WriteLine("|{0}|",AlignText(37,"4. Show My Account Details"));
        Console.WriteLine("|{0}|",AlignText(37,"5. Logout"));
        Console.WriteLine("|{0}|",AlignText(25, ""));
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
        Console.WriteLine("|{0}|",AlignText(35, ""));
        Console.WriteLine("|{0}|",AlignText(25, "Account Number            :  " + User.Account_Number));
        Console.WriteLine("|{0}|",AlignText(25, "Account Holder's Name     :  " + User.Title + ". " + User.Name));
        Console.WriteLine("|{0}|",AlignText(25, "Total Balance in account  :  " + "Rs. " + User.Total_Balance));
        Console.WriteLine("|{0}|",AlignText(25, "Last Login Details        :  " + User.LastLoginDetails));
        Console.WriteLine("|{0}|",AlignText(35, ""));
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
        Console.WriteLine("|{0}|",AlignText(34, ""));
        Console.WriteLine("|{0}|",AlignText(34,"GTBPI Banking System v1.2.0.8"));
        Console.WriteLine("|{0}|",AlignText(35,"Developed By Yash Bhardwaj"));
        Console.WriteLine("|{0}|",AlignText(34,"Computer Engineering[Vth Sem]"));
        Console.WriteLine("|{0}|",AlignText(34, ""));
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
    private void Login()
    {
        Console.Clear();
        Center("**** GTBPI Banking System | Login Page ****");
        DrawLine();
        Console.Write("{0}",AlignText(27,"Enter your account number   :  ","L"));
        User.Account_Number = UInt32.Parse(Console.ReadLine());
        if (User.ReadFromDatabase())
        {
            Console.Write("{0}", AlignText(27,"Enter your account password :  ","L"));
            string UserPassword = Console.ReadLine();
            if (UserPassword == User.Password)
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
        else
        {
            Console.WriteLine("Sorry but the account number : " + User.Account_Number + " does not exist in our database");
            Console.WriteLine("Please check the account number and try again!");
            Console.ReadKey();
        }
    }
    private void DepositMoney()
    {
        Console.Write("Enter amount you want to deposit : ");
        User.Total_Balance += UInt32.Parse(Console.ReadLine());
        Console.WriteLine("Amount deposited in your account successfully!");
    }
    private void WithdrawMoney()
    {
        Console.Write("Enter amount you want to withdraw : ");
        Double WithDrawalAmount = Double.Parse(Console.ReadLine());
        if (WithDrawalAmount <= User.Total_Balance)
        {
            User.Total_Balance -= WithDrawalAmount;
            Console.WriteLine("Amount withdrawal from your account was successfull!");
        }
        else
        {
            Console.WriteLine("You don't have sufficient balance in your account to complete this transaction");
            Console.ReadKey();
        }
    }
    private void TransferMoney()
    {
        Console.Write("Enter amount you want to transfer : ");
        Double TransferAmount = Double.Parse(Console.ReadLine());
        if (TransferAmount <= User.Total_Balance)
        {
            ReadAndWriteDataBase Transfer = new ReadAndWriteDataBase();
            Console.Write("Enter Account Number to which you want to transfer the amount : ");
            Transfer.Account_Number =  UInt32.Parse(Console.ReadLine());
            if (Transfer.ReadFromDatabase())
            {
                Console.WriteLine("The account number : " + Transfer.Account_Number + " belongs to " + Transfer.Title + ". " + Transfer.Name);
                Console.Write("Do you want to proceed with this transaction[y/n] : ");
                char choice = Console.ReadLine()[0];
                if (choice == 'y' || choice == 'Y')
                {
                    Transfer.Total_Balance += TransferAmount;
                    User.Total_Balance -= TransferAmount;
                    Transfer.WriteToDatabase(4);
                    User.WriteToDatabase(3);
                    Console.WriteLine(TransferAmount + " has been successfully transfered to " + Transfer.Title + ". " + Transfer.Name + "[" + Transfer.Account_Number + "]");
                }
                else
                {
                    Console.WriteLine("The transaction has been aborted");
                }
            }
            else
            {
                Console.WriteLine("Sorry but the account number : " + Transfer.Account_Number + " does not exist in our database");
                Console.WriteLine("Please check the account number and try again!");
                Console.ReadKey();
            }
            Transfer.Close();
        }
        else
        {
            Console.WriteLine("You don't have sufficient balance in your account to complete this transaction");
            Console.ReadKey();
        }
    }
    private void Logout()
    {
        User.WriteToDatabase(3);
        User.Account_Number = 0;
        User.Total_Balance = 0;
        User.Title = User.Name = User.LastLoginDetails = User.Password = "";
        User.Close();
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