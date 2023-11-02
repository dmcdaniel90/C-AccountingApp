//Greet the user
Console.WriteLine("Welcome to MyAccount System 1.0");
Console.WriteLine("Press any key to continue");
Console.ReadKey();

Account checkingAccount = new Account("checking", 3000);

checkingAccount.addToBalance();






/* Classes go here */

//Create an Account base class
class Account {

  // protected int ID { get; set; }
  public decimal Balance { get; set; }
  public string Product { get; set; }

  public Account(string product, decimal balance)
  {
    Balance = balance;
    Product = product;
  }

  public string printBalance()
  {
    return $"Your balance as of {DateTime.UtcNow.ToLongDateString()} is {Balance}";
  }

  public void addToBalance()
  {
    
    Console.WriteLine("How much are you depositing?");
    string? userInput = Console.ReadLine();
    decimal deposit;

    if (decimal.TryParse(userInput, out _))
    {
      deposit = decimal.Parse(userInput);
    }
    else
    {
      Console.WriteLine("Invalid input. Please enter a valid number.");
      return;
    }

    Balance = Balance + deposit;

    printBalance();
  }
}

// ID = product.ToLower() == "checking" ? 100 : 200;