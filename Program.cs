//Greet the user
using System.Buffers;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

//Initialize App Object
string defaultOwner = "John Doe";
decimal defaultBalance = 0.00m;
int checkingAccountCode = 100;
int savingsAccountCode = 200;

Account myAccount = new Account(defaultOwner, defaultBalance);
CheckingAccount myChecking = new CheckingAccount(defaultOwner, checkingAccountCode, defaultBalance);
SavingsAccount mySavings = new SavingsAccount(defaultOwner, savingsAccountCode);

Console.WriteLine("Welcome to MyAccount System 1.0");
Console.WriteLine("Press any key to continue");
Console.Read();
openMenu();



//This function opens the main menu and calls the handleMenuSelection function
void openMenu() 
{
  Console.WriteLine(@"What would you like to do today? (1-5)
  1.) New Account
  2.) Get Account and Balance Information
  3.) Withdraw or Deposit Money
  4.) Transfer Funds
  5.) Calculate Account Interest");

  string ?selected = Console.ReadLine();

  if (int.TryParse(selected, out _))
    {
      int menuItemSelected = int.Parse(selected);

      if(menuItemSelected < 1 || menuItemSelected > 5) {
        Console.WriteLine("Invalid input. Please enter a valid number.");
        openMenu();
      } else {
        Console.WriteLine();
        Console.WriteLine($"You selected menu option {menuItemSelected}");
        handleMenuSelection(menuItemSelected, myAccount);
      }
    }
  else
    {
    Console.WriteLine("Invalid input. Please enter a valid number.");
    openMenu(); 
  }
}

//This function handles the menu selection and calls the specified menu item
Account? handleMenuSelection(int menuItemSelected, Account account)
{
  switch(menuItemSelected)
  {
    case 1:
      Console.WriteLine(@"New account
      ");
      myAccount = Account.OpenAccount(account);
      myChecking = Account.initChecking(myAccount);
      mySavings = Account.initSavings(myAccount);

      Console.WriteLine(@"
      ");
      openMenu();
      return null;
    case 2:
      Console.WriteLine(@"Get Account and Balance Information
      ");
      myChecking.printBalance();
      mySavings.printBalance();

      Console.WriteLine(@"
      ");
      openMenu();
      return null;
    case 3:
      Console.WriteLine(@$"Withdraw or Deposit Money (1-2)
      ");

      account.moveMoney(myChecking, mySavings);

      Console.WriteLine();
      openMenu();
      return null;
    case 4:
      Console.WriteLine("Transfer Funds");
      myAccount = account;

      Console.WriteLine();
      openMenu();
      return myAccount;
    case 5:
      Console.WriteLine("Calculate Account Interest");
      myAccount = account;

      Console.WriteLine();
      openMenu();
      return myAccount;
    default:
      Console.WriteLine();
      openMenu();
      return null;
  }
}




/* Classes go here */


//Create an Account base class
class Account {

  public string Owner;
  public decimal Balance;

  public Account(string owner, decimal openingBalance)
  {
    Owner = owner;
    Balance = openingBalance;
  }

  public static Account OpenAccount(Account account)
  { 
    Console.WriteLine("Hello! What is your name?");
    string owner = Console.ReadLine() ?? "John Doe";
    Console.WriteLine();

    Console.WriteLine("What is your opening balance?");
    var openingBalance = Console.ReadLine();
    Console.WriteLine();

    decimal openingBalanceParsed;

    if (decimal.TryParse(openingBalance, out _))
    {
      openingBalanceParsed = decimal.Parse(openingBalance);

      return new Account(owner, openingBalanceParsed);
    }
    else
    {
      Console.WriteLine("Invalid input. Please enter a valid number.");
      OpenAccount(account);

      return account; //never reached, see Recursion above
    }
  }

  //function to init Checking and Savings account instances
  public static CheckingAccount initChecking(Account account) {
    CheckingAccount newCheckingAccount = new CheckingAccount(account.Owner, 100, account.Balance);
    return newCheckingAccount;
  }
  public static SavingsAccount initSavings(Account account) {
    SavingsAccount newSavingsAccount = new SavingsAccount(account.Owner, 200);
    return newSavingsAccount;
  }

  //function to withdraw or deposit funds
  public void moveMoney(CheckingAccount checking, SavingsAccount savings)
  {
    //From which account?
    Console.WriteLine(@"Which account would you like to work with? (1-2)
    
    1.) Checking
    2.) Savings");

    string? accountSelection = Console.ReadLine();

    Console.WriteLine(@"What would you like to do? (1-2)
    
    1.) Withdraw
    2.) Deposit");

    string? operationSelection = Console.ReadLine();

    //Call account access - error check
    switch (accountSelection, operationSelection)
    {
      case ("1", "1"):
        Console.WriteLine("Withdraw from checking");
        checking.withdrawChecking(checking);
        break;
      case ("1", "2"):
        Console.WriteLine("Deposit into checking");
        checking.depositChecking(checking);
        break;
      case ("2", "1"):
        Console.WriteLine("Withdraw from savings");
        savings.withdrawSavings(savings);
        break;
      case ("2", "2"):
        Console.WriteLine("Deposit into savings");
        savings.depositSavings(savings);
        break;
      default:
        Console.WriteLine("Invalid option");
        moveMoney(checking, savings);
        break;
      }
    }
  }



class CheckingAccount : Account
{
  public int ID { get; private set; }

  public CheckingAccount(string owner, int id, decimal openingBalance) : base(owner, openingBalance)
  {
    ID = id;
  }

  public void printBalance()
  {
    Console.WriteLine($"Hello, {Owner}");
    Console.WriteLine($"Your checking balance as of {DateTime.UtcNow.ToLongDateString()} is £{Balance.ToString("F2")}");
  }
    public CheckingAccount withdrawChecking(CheckingAccount checking)
      {
        Console.WriteLine("How much would you like to withdraw?");
        string? withdrawAmt = Console.ReadLine();

        if (decimal.TryParse(withdrawAmt, out _))
        {
          decimal withdrawAmtParsed = decimal.Parse(withdrawAmt);
          checking.Balance = checking.Balance - withdrawAmtParsed;

          Console.WriteLine($"Withdrawal complete. Your balance is £{checking.Balance.ToString("F2")}");

          return checking;
        }
        else
        {
          Console.WriteLine("Invalid input. Please enter a valid number.");
          withdrawChecking(checking);

          return checking; //never reached, see Recursion above
        } 
      }
    public CheckingAccount depositChecking(CheckingAccount checking)
      {
        Console.WriteLine("How much would you like to deposit?");
        string? depositAmt = Console.ReadLine();

        if (decimal.TryParse(depositAmt, out _))
        {
          decimal depositAmtParsed = decimal.Parse(depositAmt);
          checking.Balance = checking.Balance + depositAmtParsed;

          Console.WriteLine($"Thank you for your deposit. Your balance is {checking.Balance.ToString("F2")}");

          return checking;
        }
        else
        {
          Console.WriteLine("Invalid input. Please enter a valid number.");
          depositChecking(checking);

          return checking; //never reached, see Recursion above
        } 
      }
}

class SavingsAccount : Account
{
  public int ID { get; private set; }

  public decimal SavingsBalance { get; set; }

  public SavingsAccount(string owner, int id) : base(owner, 0)
  {
    SavingsBalance = Balance;
    ID = id;
  }

  public void printBalance()
  {
    Console.WriteLine(@$"Your savings balance as of {DateTime.UtcNow.ToLongDateString()} is £{SavingsBalance.ToString("F2")}
    ");
  }

  public SavingsAccount withdrawSavings(SavingsAccount savings)
      {
        Console.WriteLine("How much would you like to withdraw?");
        string? withdrawAmt = Console.ReadLine();

        if (decimal.TryParse(withdrawAmt, out _))
        {
          decimal withdrawAmtParsed = decimal.Parse(withdrawAmt);
          savings.SavingsBalance = savings.SavingsBalance - withdrawAmtParsed;

          Console.WriteLine($"Withdrawal complete. Your balance is {savings.SavingsBalance.ToString("F2")}");

          return savings;
        }
        else
        {
          Console.WriteLine("Invalid input. Please enter a valid number.");
          withdrawSavings(savings);

          return savings; //never reached, see Recursion above
        } 
      }

      public SavingsAccount depositSavings(SavingsAccount savings)
      {
        Console.WriteLine("How much would you like to deposit?");
        string? depositAmt = Console.ReadLine();

        if (decimal.TryParse(depositAmt, out _))
        {
          decimal depositAmtParsed = decimal.Parse(depositAmt);
          savings.SavingsBalance = savings.SavingsBalance + depositAmtParsed;

          Console.WriteLine($"Thank you for your deposit. Your balance is {savings.SavingsBalance.ToString("F2")}");
          return savings;
        }
        else
        {
          Console.WriteLine("Invalid input. Please enter a valid number.");
          depositSavings(savings);

          return savings; //never reached, see Recursion above
        } 
      }
}
