//Initialize App with defaults, greet user, and open the main menu
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
  Console.WriteLine(@"What would you like to do today? (1-6)
  1.) New Account
  2.) Get Account and Balance Information
  3.) Withdraw or Deposit Money
  4.) Transfer Funds
  5.) Calculate Account Interest
  6.) Exit Application
  ");

  string ?selected = Console.ReadLine();

  if (int.TryParse(selected, out _))
    {
      int menuItemSelected = int.Parse(selected);

      if(menuItemSelected < 1 || menuItemSelected > 6) {
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
      return null;
    case 6:
      Console.WriteLine(@"Application will now exit. Goodbye!
      ");
      Environment.Exit(0);
      return null;
    default:
      Console.WriteLine();
      openMenu();
      return null;
  }
}




/* Classes go here */


//Create an Account base class
class Account {

  //Local variables
  public string Owner;
  public decimal Balance;
  public decimal IntRate; 

  //Constructor
  public Account(string owner, decimal openingBalance, decimal intRate)
  {
    Owner = owner;
    Balance = openingBalance;
    IntRate = intRate;
  }

  //Static method to create new Account with user input
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

  //functions to init Checking and Savings account instances
  public static CheckingAccount initChecking(Account account) {
    CheckingAccount newCheckingAccount = new CheckingAccount(account.Owner, 100, account.Balance, account.IntRate);
    return newCheckingAccount;
  }
  public static SavingsAccount initSavings(Account account) {
    SavingsAccount newSavingsAccount = new SavingsAccount(account.Owner, 200, account.IntRate);
    return newSavingsAccount;
  }

  //functions to withdraw or deposit funds
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

    //Function to transfer money between checking and savings
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

    //Function to calculate interest for a user specified account
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
    
    //Function to perform interest calculation and update balance (for checking only)
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



//Checking account class derived from Account.
class CheckingAccount : Account
{
  //Local variables
  public int ID { get; private set; }

  //Constructor. ID is created and passed during init. Currently hardcoded value.
  public CheckingAccount(string owner, int id, decimal openingBalance, decimal intRate) : base(owner, openingBalance, intRate)
  {
    ID = id;
    IntRate = intRate + 0.0m;
  }

  //Handles printing the interest balance. Called here to grant CalcInterest access to the ID.
  public void printIntBalance()
  {
    CalcInterest(IntRate, ID);
  }

  //Print balance and ID, no interest added.
  public void printBalance()
  {
    Console.WriteLine($"Your checking balance (ID: {ID}) as of {DateTime.UtcNow.ToLongDateString()} is £{Balance.ToString("F2")}");
  }

  //Withdraw funds from checking
  public void withdrawChecking(CheckingAccount checking)
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
        };

        checking.Balance = checking.Balance - withdrawAmtParsed;

        Console.WriteLine($"Withdrawal complete. Your checking balance is £{checking.Balance.ToString("F2")}");
      }
      else
      {
        Console.WriteLine("Invalid input. Please enter a valid number.");
        withdrawChecking(checking);
      } 
    }

    public bool withdrawChecking(CheckingAccount checking, string? transferAmt) 
    //Overload for Transfers. The bool return is used for error checking before running depositSavings.
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

          return false;
        } 
      }
    
    //Deposit into checking
    public void depositChecking(CheckingAccount checking)
      {
        Console.WriteLine(@"How much would you like to deposit?
        ");
        string? depositAmt = Console.ReadLine();

        if (decimal.TryParse(depositAmt, out _))
        {
          decimal depositAmtParsed = decimal.Parse(depositAmt);
          checking.Balance = checking.Balance + depositAmtParsed;

          Console.WriteLine($"Thank you for your deposit. Your checking balance is £{checking.Balance.ToString("F2")}");
        }
        else
        {
          Console.WriteLine("Invalid input. Please enter a valid number.");
          depositChecking(checking);
        } 
      }
    
    //Deposit into checking via transfer
    public void depositChecking(CheckingAccount checking, string? transferAmt)
      {
        if (decimal.TryParse(transferAmt, out _))
        {
          decimal transferAmtParsed = decimal.Parse(transferAmt);
          checking.Balance = checking.Balance + transferAmtParsed;

          Console.WriteLine($"Transfer complete. Your checking balance is £{checking.Balance.ToString("F2")}");
        }
      }
}


//Savings account class dericed from Account.
class SavingsAccount : Account
{
  //Local variables
  public int ID { get; private set; }
  public decimal SavingsBalance { get; set; }

  //Constructor. Note that the intRate for savings is adjusted here.
  public SavingsAccount(string owner, int id, decimal intRate) : base(owner, 0, intRate)
  {
    SavingsBalance = Balance;
    ID = id;
    IntRate = intRate + 1.00m; //Adjusted int rate
  }

  //Print account balance and ID
  public void printBalance()
  {
    Console.WriteLine(@$"Your savings balance (ID: {ID}) as of {DateTime.UtcNow.ToLongDateString()} is £{SavingsBalance.ToString("F2")}
    ");
  }

  //Print interest balance. Calls the overriden version for savings accounts.
  public void printIntBalance()
  {
    CalcInterest(IntRate, ID);
  }

  //Overriding the Account method to use the savings account
  public override void CalcInterest(decimal IntRate, int ID)  
  {
    decimal intRateParsed = IntRate / 100;
    decimal intAmount = SavingsBalance * intRateParsed;

    SavingsBalance = SavingsBalance + intAmount;

    Console.WriteLine(@$"Your interest on your account (ID: {ID}) as of {DateTime.UtcNow.ToLongDateString()} is £{intAmount.ToString("F2")}
    Your total calculated balance with interest is £{SavingsBalance + intAmount}");
  }

  //Function to withdraw from savings
  public void withdrawSavings(SavingsAccount savings)
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
      };

      savings.SavingsBalance = savings.SavingsBalance - withdrawAmtParsed;

      Console.WriteLine($"Withdrawal complete. Your savings balance is £{savings.SavingsBalance.ToString("F2")}");
    }
    else
    {
      Console.WriteLine("Invalid input. Please enter a valid number.");
      withdrawSavings(savings);
    } 
  }

  //Withdraw from savings for transfer to checking.
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

  //Deposit into savings
  public void depositSavings(SavingsAccount savings)
  {
    Console.WriteLine(@"How much would you like to deposit?
    ");
    string? depositAmt = Console.ReadLine();

    if (decimal.TryParse(depositAmt, out _))
    {
      decimal depositAmtParsed = decimal.Parse(depositAmt);
      savings.SavingsBalance = savings.SavingsBalance + depositAmtParsed;

      Console.WriteLine($"Thank you for your deposit. Your savings balance is £{savings.SavingsBalance.ToString("F2")}");
    }
    else
    {
      Console.WriteLine("Invalid input. Please enter a valid number.");
      depositSavings(savings);

    } 
  }

  //Deposit into savings via transfer
  public void depositSavings(SavingsAccount savings, string? transferAmt)
  {
    if (decimal.TryParse(transferAmt, out _))
    {
      decimal transferAmtParsed = decimal.Parse(transferAmt);
      
      savings.SavingsBalance = savings.SavingsBalance + transferAmtParsed;

      Console.WriteLine($"Transfer complete. Your savings balance is £{savings.SavingsBalance.ToString("F2")}");
    }
  }
}
