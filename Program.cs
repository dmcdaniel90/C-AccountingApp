//Greet the user
using System.Buffers;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;

//Initialize App Object
Account myAccount = new Account("John Doe", 0.00m);

Console.WriteLine("Welcome to MyAccount System 1.0");
Console.WriteLine("Press any key to continue");
Console.Read();
openMenu();



//This function opens the main menu and calls the handleMenuSelection function
void openMenu() 
{
  Console.WriteLine(@"What would you like to do today? (1-5)
  1.) Open an Account
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
void handleMenuSelection(int menuItemSelected, Account account)
{
  switch(menuItemSelected)
  {
    case 1:
      Console.WriteLine("Open an account");
      Account.OpenAccount(account);
      openMenu();
      break;
    case 2:
      Console.WriteLine("Get Account and Balance Information");
      account.printBalance();
      break;
    case 3:
      Console.WriteLine("Withdraw or Deposit Money");
      break;
    case 4:
      Console.WriteLine("Transfer Funds");
      break;
    case 5:
      Console.WriteLine("Calculate Account Interest");
      break;
  }
}















/* Classes go here */


//Create an Account base class
class Account {

  public string Owner { get; set; }
  public decimal Balance { get; set; }

  public Account(string owner, decimal openingBalance)
  {
    Owner = owner;
    Balance = openingBalance;
  }

  public static Account OpenAccount(Account account)
  { 
    Console.WriteLine("Hello! What is your name?");
    string owner = Console.ReadLine() ?? "John Doe";

    Console.WriteLine("What is your opening balance?");
    var openingBalance = Console.ReadLine();
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

  public void printBalance()
  {
    Console.WriteLine($"Your balance as of {DateTime.UtcNow.ToLongDateString()} is {Balance}");
  }

  // public void addToBalance()
  // {
    
  //   Console.WriteLine("How much are you depositing?");
  //   string? userInput = Console.ReadLine();
  //   decimal deposit;

  //   if (decimal.TryParse(userInput, out _))
  //   {
  //     deposit = decimal.Parse(userInput);
  //   }
  //   else
  //   {
  //     Console.WriteLine("Invalid input. Please enter a valid number.");
  //     return;
  //   }

  //   Balance = Balance + deposit;

  //   Console.WriteLine("Thank you for your deposit.");
  //   printBalance();
  // }
}

class CheckingAccount : Account
{
  public int ID { get; private set; }

  public CheckingAccount(string owner, int id, decimal openingBalance) : base(owner, openingBalance)
  {
    ID = id;
  }
}

