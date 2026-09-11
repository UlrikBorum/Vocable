using Vocable;



GenericRepo<Adverb_Question> adverbRepo = new GenericRepo<Adverb_Question>();

// Creating 10 questions
adverbRepo.Create(new Adverb_Question { Id = 1, Question = "hablar", HelpImagePath = "", HelpEnglish = "to speak", HelpSentence = "Yo hablo español." });
adverbRepo.Create(new Adverb_Question { Id = 2, Question = "venir", HelpImagePath = "", HelpEnglish = "to come", HelpSentence = "¿Puedes venir mañana?" });
adverbRepo.Create(new Adverb_Question { Id = 3, Question = "tener", HelpImagePath = "", HelpEnglish = "to have", HelpSentence = "Tengo dos libros." });
adverbRepo.Create(new Adverb_Question { Id = 4, Question = "hacer", HelpImagePath = "", HelpEnglish = "to do/make", HelpSentence = "Ella hace la tarea." });
adverbRepo.Create(new Adverb_Question { Id = 5, Question = "decir", HelpImagePath = "", HelpEnglish = "to say/tell", HelpSentence = "Él dice la verdad." });
adverbRepo.Create(new Adverb_Question { Id = 6, Question = "ver", HelpImagePath = "", HelpEnglish = "to see", HelpSentence = "Veo la película." });
adverbRepo.Create(new Adverb_Question { Id = 7, Question = "saber", HelpImagePath = "", HelpEnglish = "to know", HelpSentence = "Sé la respuesta." });
adverbRepo.Create(new Adverb_Question { Id = 8, Question = "comer", HelpImagePath = "", HelpEnglish = "to eat", HelpSentence = "Ella come manzanas." });
adverbRepo.Create(new Adverb_Question { Id = 9, Question = "vivir", HelpImagePath = "", HelpEnglish = "to live", HelpSentence = "Nosotros vivimos aquí." });
adverbRepo.Create(new Adverb_Question { Id = 10, Question = "ir", HelpImagePath = "", HelpEnglish = "to go", HelpSentence = "Voy al mercado." });

// Creating an array and taking out 5 random questions 
Adverb_Question[] FiveQ = adverbRepo.ToArray();
Random.Shared.Shuffle(FiveQ);

Array.Resize(ref FiveQ, 5);

GameTestStart();


void GameTestStart()
{

    Console.WriteLine("Testing a queue of Questions");

    for (int n = 0; n <= 4; n++)
    {
        string? playerGuess = null;
        int tries = 3;

        while (playerGuess == null || playerGuess != FiveQ[n].Question)
        {

            Console.WriteLine($"\nGuess the spanish word for {FiveQ[n].HelpImagePath}");
            Console.WriteLine($"Write 1 for english word, write 2 for a sentence");

            playerGuess = Console.ReadLine();
            if (playerGuess == FiveQ[n].Question)
            {
                Console.WriteLine("¡muy bien!");
                break;
            }
            else 
            {
                if (playerGuess == "1")
                {
                    int helpNr = int.Parse(playerGuess);
                    Console.WriteLine($"The english word is '{FiveQ[n].HelpEnglish}'");
                }
                if (playerGuess == "2")
                {
                    int helpNr = int.Parse(playerGuess);
                    Console.WriteLine($"The sentence is '{FiveQ[n].HelpSentence}'");
                }

                if (tries > 0)
                {
                    tries = tries - 1;
                    if (tries == 0)
                    {
                        Console.WriteLine($"sorry you didn't get it! It was {FiveQ[n].Question}!");
                        break;
                    }
                    Console.WriteLine($"Please try again! You have {tries} left");
                }                
            }                
        }
    }
}




