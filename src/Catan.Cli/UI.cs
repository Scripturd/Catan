namespace Catan.Cli;

public class UI
{
    public static int AskUserForInt(string question, int min = int.MinValue, int max = int.MaxValue)
    {
        Console.WriteLine(question);
        
        return AskUserForInt(min, max);
    }
    public static int AskUserForInt(int min = int.MinValue, int max = int.MaxValue)
    {
        var answer = Console.ReadLine();

        if (!int.TryParse(answer, out var parsedAnswer))
            return AskUserForInt("Your answer has to be an integer", min, max);

        if (parsedAnswer < min)
            return AskUserForInt($"Your answer can't be lower than {min}", min, max);

        if (parsedAnswer > max)
            return AskUserForInt($"Your answer can't be higher than {max}", min, max);

        return parsedAnswer;
    }
}