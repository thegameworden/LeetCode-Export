using LeetCode_Export;

public class Program
{
    static async Task Main()
    {

        Console.WriteLine("Starting Program....");


        LeetCode leetCode = new();

        User user = await leetCode.login();
       if (user== null)
        {
            Main();
            
        }
        Console.Write($"This program will be getting data for [{user.Username}]. If this is correct please press 'Enter':");
        Console.ReadLine();

        Console.Write("Plese paste the EXACT location that you would like your downloaded file to be placed: ");
        string downloadLocation = Console.ReadLine();


        Console.WriteLine("Generating general user info...");
        await leetCode.GetUser(user);



        Console.WriteLine("Getting the users submission info...");
        user.Questions = await leetCode.getAllQuestionsAnswered();
        Console.WriteLine("Done Getting the user's submission info!!");

        
        Console.WriteLine("Writing to file");
        Utilities.writeFiles(user, downloadLocation);
        Console.WriteLine("\n\n\nEnd of Program :)");
    }
}
