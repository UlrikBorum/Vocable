using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Vocable.Service
{
    public class AdverbsService
    {
        #region instance fields
        private IRepo<Adverb_Question> _adverbQuestionsRepo;
        private Random r = new Random();
        #endregion

        public AdverbsService(IRepo<Adverb_Question> adverbQuestionsRepo)
        {
            _adverbQuestionsRepo = adverbQuestionsRepo;
        }

        public AdverbsService()
        {
            _adverbQuestionsRepo = new GenericRepo<Adverb_Question>();
        }

        public IRepo<Adverb_Question> AdverbQuestionRepo { get  { return _adverbQuestionsRepo; } set {  _adverbQuestionsRepo = value;  }   }
        

        public void AddAnAdverbQuestionRepo()
        {

            // Implementation for adding a new adverb question to the database or collection      

            // placeolder for actual implementation
            // Creating 10 questions
            _adverbQuestionsRepo.Create(new Adverb_Question { Id = 1, Question = "hablar", HelpImagePath = "/images/correr_image.gif", HelpEnglish = "to speak", HelpSentence = "Yo hablo español." });
            _adverbQuestionsRepo.Create(new Adverb_Question { Id = 2, Question = "venir", HelpImagePath = "/images/correr_image.gif", HelpEnglish = "to come", HelpSentence = "¿Puedes venir mañana?" });
            _adverbQuestionsRepo.Create(new Adverb_Question { Id = 3, Question = "tener", HelpImagePath = "/images/correr_image.gif", HelpEnglish = "to have", HelpSentence = "Tengo dos libros." });
            _adverbQuestionsRepo.Create(new Adverb_Question { Id = 4, Question = "hacer", HelpImagePath = "/images/correr_image.gif", HelpEnglish = "to do/make", HelpSentence = "Ella hace la tarea." });
            _adverbQuestionsRepo.Create(new Adverb_Question { Id = 5, Question = "decir", HelpImagePath = "/images/correr_image.gif", HelpEnglish = "to say/tell", HelpSentence = "Él dice la verdad." });
            _adverbQuestionsRepo.Create(new Adverb_Question { Id = 6, Question = "ver", HelpImagePath = "/images/correr_image.gif", HelpEnglish = "to see", HelpSentence = "Veo la película." });
            _adverbQuestionsRepo.Create(new Adverb_Question { Id = 7, Question = "saber", HelpImagePath = "/images/correr_image.gif", HelpEnglish = "to know", HelpSentence = "Sé la respuesta." });
            _adverbQuestionsRepo.Create(new Adverb_Question { Id = 8, Question = "comer", HelpImagePath = "/images/correr_image.gif", HelpEnglish = "to eat", HelpSentence = "Ella come manzanas." });
            _adverbQuestionsRepo.Create(new Adverb_Question { Id = 9, Question = "vivir", HelpImagePath = "/images/correr_image.gif", HelpEnglish = "to live", HelpSentence = "Nosotros vivimos aquí." });
            _adverbQuestionsRepo.Create(new Adverb_Question { Id = 10, Question = "ir", HelpImagePath = "/images/correr_image.gif", HelpEnglish = "to go", HelpSentence = "Voy al mercado." });
        }


        // Take 5 random questions from the repo and return them as a shuffled Generic List of Adverb_Question objects
        public GenericRepo<Adverb_Question> GetRandomAdverbQuestions(int numberOfQuestions)
        {
            var allQuestions = _adverbQuestionsRepo.ReadAll();
            var xQuestionsArray = allQuestions.ToArray();
            ShuffleArray(r, xQuestionsArray);
            xQuestionsArray = xQuestionsArray.Take(numberOfQuestions).ToArray();
            GenericRepo<Adverb_Question> genericList = new GenericRepo<Adverb_Question>(xQuestionsArray.ToList());
           

       
            return genericList;
        }


        public static void ShuffleArray(Random rng, Adverb_Question[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = rng.Next(n--);
                Adverb_Question temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }

        }
    }
}
