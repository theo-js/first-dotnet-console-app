using Spectre.Console;

class Program
{
    private enum GameType
    {
        IsPalindrome,
        GuessTheNumber,
    }

    static void Main(string[] args)
    {
        var gameTypeOptions = new List<(GameType GameType, string Label)>
        {
            (GameType.IsPalindrome, "Est-ce un palindrôme ?"),
            (GameType.GuessTheNumber, "Deviner le nombre")
        };

        // TODO Investigate whether it's possible to display only the option labels w/ Spectre
        var selectedGameTypeOption = AnsiConsole.Prompt(
            new SelectionPrompt<(GameType GameType, string Label)>()
            .Title("Veuillez choisir un jeu:")
            .AddChoices(gameTypeOptions)
        );

        switch (selectedGameTypeOption.GameType)
        {
            case GameType.IsPalindrome:
                new IsPalindromeGame().Play();
                break;
            case GameType.GuessTheNumber:
                new GuessTheNumberGame().Play();
                break;
            default: return;
        }
    }
}

class IsPalindromeGame
{
    public void Play()
    {
        // Get prompt
        Console.WriteLine("Insérez une expression et je vous dirai s'il s'agit d'un palindrôme");
        string promptResult = Console.ReadLine() ?? "";

        // Handle empty prompt
        if (promptResult == "")
        {
            Play();
            return;
        }

        // Compute result
        string reversedPromptResult = new string(promptResult.Reverse().ToArray());

        if (promptResult == reversedPromptResult)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Il s'agit d'un palindrôme");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Ce n'est pas un palindrôme");
        }
        Console.ForegroundColor = ConsoleColor.Black;

        // Suggest to play again
        bool shouldPlayAgain = AnsiConsole.Prompt(
             new SelectionPrompt<string>()
             .Title("Voulez-vous rejouer ?")
             .AddChoices(["Oui", "Non"])
        )
            == "Oui";

        if (shouldPlayAgain) Play();
        return;

    }
}

class GuessTheNumberGame
{
    public float GetPrompt()
    {
        Console.WriteLine("Choisissez un nombre de 0 à 100");
        return Int32.Parse(Console.ReadLine() ?? "");

    }



    public void Play()
    {
        // Generate random number
        Random random = new Random();
        int numberToGuess = random.Next(0, 101);

        // Get and validate prompt
        float promptResult = -1;

        // Compare w/ number
        while (promptResult != numberToGuess)
        {
            try
            {
                promptResult = GetPrompt();
            }

            // Validate prompt
            catch (FormatException)
            {
                continue;
            }

            if (promptResult < 0 || promptResult > 100) continue;

            // Show hint
            Console.ForegroundColor = ConsoleColor.Red;
            if (promptResult > numberToGuess) Console.WriteLine("Le nombre à deviner est + petit");
            if (promptResult < numberToGuess) Console.WriteLine("Le nombre à deviner est + grand");
            Console.ForegroundColor = ConsoleColor.Black;
        }

        // Found correct number
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Vous avez trouvé le nombre correct !");
        Console.ForegroundColor = ConsoleColor.Black;

        // Suggest to play again
        bool shouldPlayAgain = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Voulez-vous rejouer ?")
            .AddChoices(["Oui", "Non"])
        )
            == "Oui";

        if (shouldPlayAgain) Play();
        return;
    }
}