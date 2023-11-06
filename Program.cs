//Greet the user
using System.Buffers;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

//Initialize App Object
string defaultOwner = "John Doe";
decimal defaultBalance = 0.00m;
decimal defaultInt = 3.00m;
int checkingAccountCode = 100;
int savingsAccountCode = 200;

Account myAccount = new Account(defaultOwner, defaultBalance, defaultInt);
CheckingAccount myChecking = new CheckingAccount(defaultOwner, checkingAccountCode, defaultBalance, defaultInt);
SavingsAccount mySavings = new SavingsAccount(defaultOwner, savingsAccountCode, defaultInt);

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
  5.) Calculate Account Interest
  ");

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
      Console.WriteLine($"Hello, {myAccount.Owner}");
      myChecking.printBalance();
      mySavings.printBalance();

      Console.WriteLine();
      openMenu();
      return null;
    case 3:
      Console.WriteLine(@$"Withdraw or Deposit Money
      ");

      account.moveMoney(myChecking, mySavings);

      Console.WriteLine();
      openMenu();
      return null;
    case 4:
      Console.WriteLine("Transfer Funds");
      account.transferMoney(myChecking, mySavings);

      Console.WriteLine();
      openMenu();
      return null;
    case 5:
      Console.WriteLine(@"Calculate Account Interest
      ");
      account.getInterest(myChecking, mySavings);
      
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

  public decimal IntRate; 

  public Account(string owner, decimal openingBalance, decimal intRate)
  {
    Owner = owner;
    Balance = openingBalance;
    IntRate = intRate;
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
    decimal intRate = account.IntRate;

    if (decimal.TryParse(openingBalance, out _))
    {
      openingBalanceParsed = decimal.Parse(openingBalance);

      return new Account(owner, openingBalanceParsed, intRate);
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
    CheckingAccount newCheckingAccount = new CheckingAccount(account.Owner, 100, account.Balance, account.IntRate);
    return newCheckingAccount;
  }
  public static SavingsAccount initSavings(Account account) {
    SavingsAccount newSavingsAccount = new SavingsAccount(account.Owner, 200, account.IntRate);
    return newSavingsAccount;
  }

  //function to withdraw or deposit funds
  public void moveMoney(CheckingAccount checking, SavingsAccount savings)
  {
    //From which account?
    Console.WriteLine(@"Which account would you like to work with? (1-2)
    
    1.) Checking
    2.) Savings
    ");

    string? accountSelection = Console.ReadLine();

    Console.WriteLine(@"What would you like to do? (1-2)
    
    1.) Withdraw
    2.) Deposit
    ");

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

    public void transferMoney(CheckingAccount checking, SavingsAccount savings)
    {
      //From which account?
      Console.WriteLine(@"Which account are you transferring from? (1-2)
    
      1.) Checking
      2.) Savings
      ");

      string? accountSelection = Console.ReadLine();

      Console.WriteLine(@"How much would you like to transfer?
      ");

      string? reqAmount = Console.ReadLine();
      
      if(accountSelection == "1")
      {
        if(checking.withdrawChecking(checking, reqAmount)) 
        {
          savings.depositSavings(savings, reqAmount); //if the withdraw succeeds, then run the depositSavings function
        }
      } else if (accountSelection == "2")
      {
        if(savings.withdrawSavings(savings, reqAmount))
        {
          checking.depositChecking(checking, reqAmount);
        }
      } else if (accountSelection != "1" && accountSelection != "2") {
        Console.WriteLine("Invalid operation, enter 1 or 2 to choose an account");
        return;
      } else {
        return;
      }
    }

    public void getInterest(CheckingAccount checking, SavingsAccount savings)
    {
    //From which account?
    Console.WriteLine(@"Which account would you like your interest balance? (1-2)
    1.) Checking
    2.) Savings
    ");

    string? accountSelection = Console.ReadLine();

    if(accountSelection == "1")
      {
        checking.printIntBalance();
      } else if (accountSelection == "2")
      {
        savings.printIntBalance();
      } else if (accountSelection != "1" && accountSelection != "2") {
        Console.WriteLine("Invalid operation, enter 1 or 2 to choose an account");
        return;
      } else {
        return;
      }
    }
    
    public virtual void CalcInterest(decimal IntRate, int ID)
    {
      decimal intRateParsed = IntRate / 100;
      decimal intAmount = Balance * intRateParsed;

      Balance = Balance + intAmount;

      Console.WriteLine(@$"Your interest on your account (ID: {ID}) as of {DateTime.UtcNow.ToLongDateString()} is £{intAmount.ToString("F2")}
      Your total calculated balance with interest is £{Balance + intAmount}
      ");
    }
  }



class CheckingAccount : Account
{
  public int ID { get; private set; }

  public CheckingAccount(string owner, int id, decimal openingBalance, decimal intRate) : base(owner, openingBalance, intRate)
  {
    ID = id;
    IntRate = intRate + 0.0m;
  }

  public void printIntBalance()
  {
    CalcInterest(IntRate, ID);
  }

  public void printBalance()
  {
    Console.WriteLine($"Your checking balance (ID: {ID}) as of {DateTime.UtcNow.ToLongDateString()} is £{Balance.ToString("F2")}");
  }

  public CheckingAccount withdrawChecking(CheckingAccount checking)
    {
      Console.WriteLine(@"How much would you like to withdraw?
      ");
      string? withdrawAmt = Console.ReadLine();

      if (decimal.TryParse(withdrawAmt, out _))
      {
        decimal withdrawAmtParsed = decimal.Parse(withdrawAmt);

        if(withdrawAmtParsed > checking.Balance) { 
          Console.WriteLine("Insufficient funds");
          printBalance();
          return checking; 
        };

        checking.Balance = checking.Balance - withdrawAmtParsed;

        Console.WriteLine($"Withdrawal complete. Your checking balance is £{checking.Balance.ToString("F2")}");

        return checking;
      }
      else
      {
        Console.WriteLine("Invalid input. Please enter a valid number.");
        withdrawChecking(checking);

        return checking; //never reached, see Recursion above
      } 
    }

    public bool withdrawChecking(CheckingAccount checking, string? transferAmt) //Overload for Transfers
      {
        if (decimal.TryParse(transferAmt, out _))
        {
          decimal transferAmtParsed = decimal.Parse(transferAmt);

          if(transferAmtParsed > checking.Balance) { 
            Console.WriteLine("Insufficient funds");
            printBalance(); 
            return false; 
          };

          checking.Balance = checking.Balance - transferAmtParsed;

          Console.WriteLine($"Transfer complete. Your checking balance is £{checking.Balance.ToString("F2")}");

          return true;
        }
        else
        {
          Console.WriteLine(@"Operation failed. 
          Please check your request was entered correctly and
          the funds are available to send, then try again.");

          return false; //never reached, see Recursion above
        } 
      }
    public CheckingAccount depositChecking(CheckingAccount checking)
      {
        Console.WriteLine(@"How much would you like to deposit?
        ");
        string? depositAmt = Console.ReadLine();

        if (decimal.TryParse(depositAmt, out _))
        {
          decimal depositAmtParsed = decimal.Parse(depositAmt);
          checking.Balance = checking.Balance + depositAmtParsed;

          Console.WriteLine($"Thank you for your deposit. Your checking balance is £{checking.Balance.ToString("F2")}");

          return checking;
        }
        else
        {
          Console.WriteLine("Invalid input. Please enter a valid number.");
          depositChecking(checking);

          return checking; //never reached, see Recursion above
        } 
      }
    public CheckingAccount depositChecking(CheckingAccount checking, string? transferAmt)
      {
        if (decimal.TryParse(transferAmt, out _))
        {
          decimal transferAmtParsed = decimal.Parse(transferAmt);
          checking.Balance = checking.Balance + transferAmtParsed;

          Console.WriteLine($"Transfer complete. Your checking balance is £{checking.Balance.ToString("F2")}");

          return checking;
        }
        else
        {
          return checking; //never reached, see Recursion above
        } 
      }
}

class SavingsAccount : Account
{
  public int ID { get; private set; }

  public decimal SavingsBalance { get; set; }

  public SavingsAccount(string owner, int id, decimal intRate) : base(owner, 0, intRate)
  {
    SavingsBalance = Balance;
    ID = id;
    IntRate = intRate + 1.00m;
  }

  public void printBalance()
  {
    Console.WriteLine(@$"Your savings balance (ID: {ID}) as of {DateTime.UtcNow.ToLongDateString()} is £{SavingsBalance.ToString("F2")}
    ");
  }

  public void printIntBalance()
  {
    CalcInterest(IntRate, ID);
  }

  public override void CalcInterest(decimal IntRate, int ID) //Savings version
  {
    decimal intRateParsed = (IntRate + 1) / 100;
    decimal intAmount = SavingsBalance * intRateParsed;

    SavingsBalance = SavingsBalance + intAmount;

    Console.WriteLine(@$"Your interest on your account (ID: {ID}) as of {DateTime.UtcNow.ToLongDateString()} is £{intAmount.ToString("F2")}
    Your total calculated balance with interest is £{SavingsBalance + intAmount}");
  }

  public SavingsAccount withdrawSavings(SavingsAccount savings)
      {
        Console.WriteLine(@"How much would you like to withdraw?
        ");
        string? withdrawAmt = Console.ReadLine();

        if (decimal.TryParse(withdrawAmt, out _))
        {
          decimal withdrawAmtParsed = decimal.Parse(withdrawAmt);

          if(withdrawAmtParsed > savings.Balance) { 
            Console.WriteLine("Insufficient funds");
            printBalance(); 
            return savings; 
          };

          savings.SavingsBalance = savings.SavingsBalance - withdrawAmtParsed;

          Console.WriteLine($"Withdrawal complete. Your savings balance is £{savings.SavingsBalance.ToString("F2")}");

          return savings;
        }
        else
        {
          Console.WriteLine("Invalid input. Please enter a valid number.");
          withdrawSavings(savings);

          return savings; //never reached, see Recursion above
        } 
      }

      public bool withdrawSavings(SavingsAccount savings, string? transferAmt)
      {
        if (decimal.TryParse(transferAmt, out _))
        {
          decimal transferAmtParsed = decimal.Parse(transferAmt);

          if(transferAmtParsed > savings.Balance) { 
            Console.WriteLine("Insufficient funds");
            printBalance(); 
            return false; 
          };

          savings.SavingsBalance = savings.SavingsBalance - transferAmtParsed;

          Console.WriteLine($"Transfer complete. Your savings balance is £{savings.SavingsBalance.ToString("F2")}");

          return true;
        }
        else
        {
          Console.WriteLine(@"Operation failed. 
          Please check your request was entered correctly and
          the funds are available to send, then try again.");
          return false; //never reached, see Recursion above
        } 
      }

      public SavingsAccount depositSavings(SavingsAccount savings)
      {
        Console.WriteLine(@"How much would you like to deposit?
        ");
        string? depositAmt = Console.ReadLine();

        if (decimal.TryParse(depositAmt, out _))
        {
          decimal depositAmtParsed = decimal.Parse(depositAmt);
          savings.SavingsBalance = savings.SavingsBalance + depositAmtParsed;

          Console.WriteLine($"Thank you for your deposit. Your savings balance is £{savings.SavingsBalance.ToString("F2")}");
          return savings;
        }
        else
        {
          Console.WriteLine("Invalid input. Please enter a valid number.");
          depositSavings(savings);

          return savings; //never reached, see Recursion above
        } 
      }
      public SavingsAccount depositSavings(SavingsAccount savings, string? transferAmt)
      {
        if (decimal.TryParse(transferAmt, out _))
        {
          decimal transferAmtParsed = decimal.Parse(transferAmt);
          
          savings.SavingsBalance = savings.SavingsBalance + transferAmtParsed;

          Console.WriteLine($"Transfer complete. Your savings balance is £{savings.SavingsBalance.ToString("F2")}");
          return savings;
        }
        else
        {
          return savings; //never reached, see Recursion above
        } 
      }
}
