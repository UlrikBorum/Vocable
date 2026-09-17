using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Vocable.Service;
using Vocable;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;

namespace Vocable_web.Pages
{
    public class IndexModel : PageModel
    {
        #region instance fields
        private readonly AdverbsService _adverbsService;
        #endregion

        #region constructor

        public IndexModel(AdverbsService adverbsService)
        {
            _adverbsService = adverbsService;
         
        }
        #endregion


        #region properties
       
        [BindProperty]
        public string Answer { get; set; } = string.Empty;

        [BindProperty]
        public GenericRepo<Adverb_Question> Questions { get; set; } = new GenericRepo<Adverb_Question>();

        [BindProperty]
        public QuizState QS { get; set; } = new QuizState();



        #endregion

        public void OnGet()
        {
            // initial GET - nothing to do
        }

        // Start using the DI-registered service from Program.cs
        public IActionResult OnPostStart()
        {
            // Service already populated in Program.cs as singleton
            var aq = _adverbsService.GetRandomAdverbQuestions(5);

            Questions = aq;
            QS.Current = 0;
            QS.Lives = 3;
            QS.CorrectCount = 0;
            QS.ShowEnglishHint = false;
            QS.ShowSentenceHint = false;
            QS.ShowCorrectAnimation = false;

            
            QS.GameStarted = true;
            QS.GameEnded = false;

            // Persist questions between POST requests so hint actions can reload them
            SaveStateToTemp();

            return Page();
        }

   
        public IActionResult OnPostSubmit()
        {
            // Reload persisted state (forms don't post the full repo)
            LoadStateFromTemp();

            var aq = Questions;

            if (aq.ReadAll().Count == 0)
            {
                QS.Message = "No questions available.";
                QS.MessageClass = string.Empty;
                QS.GameStarted = false;
                return Page();
            }

            // ensure bounds
            if (QS.Current < 0) QS.Current = 0;
            if (QS.Current >= aq.ReadAll().Count)
            {
                EndGame();
                return Page();
            }

            var q = aq.ReadAll()[QS.Current];
            var given = (Answer ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(given))
            {
                QS.Message = "Please enter an answer.";
                QS.MessageClass = "message-wrong";
                QS.GameStarted = true;
                // ensure no lingering correct-animation flag when user hasn't answered
                QS.ShowCorrectAnimation = false;
                return Page();
            }

            if (string.Equals(given, q.Question ?? string.Empty, StringComparison.OrdinalIgnoreCase))
            {
                QS.CorrectCount++;
                QS.Message = "Correct! Moving to next question.";
                QS.MessageClass = "message-correct";
                // show short animation between questions
                QS.ShowCorrectAnimation = true;
                QS.Current++;
                    // clear the answer for the next question and remove any ModelState entry so the tag helper
                    // renders the updated empty value (ModelState values take precedence over the property)
                    Answer = string.Empty;
                    ModelState.Remove(nameof(Answer));
                QS.Lives = 3;
                QS.ShowEnglishHint = false;
                QS.ShowSentenceHint = false;
            }
            else
            {
                QS.Lives--;
                if (QS.Lives <= 0)
                {
                    QS.Message = $"No lives left. The answer was {q.Question}! Moving to next question.";
                    QS.MessageClass = "message-wrong";
                    QS.Current++;
                    // clear the answer when advancing after losing all lives and remove ModelState entry
                    Answer = string.Empty;
                    ModelState.Remove(nameof(Answer));
                    QS.Lives = 3;
                    QS.ShowEnglishHint = false;
                    QS.ShowSentenceHint = false;
                    // ensure no animation when advancing due to losing lives
                    QS.ShowCorrectAnimation = false;
                }
                else
                {
                    QS.Message = $"Wrong. {QS.Lives} lives remaining.";
                    QS.MessageClass = "message-wrong";
                    // don't show correct animation on wrong answer
                    QS.ShowCorrectAnimation = false;
                }
            }

            if (QS.Current >= aq.ReadAll().Count)
            {
                EndGame();
            }

            QS.GameStarted = !QS.GameEnded;

            // Persist updated state so the next POST can reload the same questions
            if (!QS.GameEnded)
            {
                SaveStateToTemp();
            }

            return Page();
        }

        public IActionResult OnPostRevealEnglish()
        {
            // reload state so page can render helper text (if needed)
            LoadStateFromTemp();

            QS.ShowEnglishHint = true;
            QS.GameStarted = true;
            // ensure animation flag is cleared when revealing hints (only answers should trigger it)
            QS.ShowCorrectAnimation = false;
            // persist hint state
            SaveStateToTemp();
            return Page();
        }

        public IActionResult OnPostRevealSentence()
        {
            // reload state so page can render helper text (if needed)
            LoadStateFromTemp();

            QS.ShowSentenceHint = true;
            QS.GameStarted = true;
            // ensure animation flag is cleared when revealing hints (only answers should trigger it)
            QS.ShowCorrectAnimation = false;
            // persist hint state
            SaveStateToTemp();
            return Page();
        }

        public IActionResult OnPostPlayAgain()
        {
            // clear any persisted questions and start fresh
            TempData.Remove(TempKey);
            return RedirectToPage();
        }

        private void EndGame()
        {
            QS.GameEnded = true;
            QS.GameStarted = false;

            

            QS.FinalMessage = $"You answered {QS.CorrectCount} of {Questions.ReadAll().Count} correctly.";

            // clear persisted questions when game ends
            TempData.Remove(TempKey);
        }

        private const string TempKey = "QuizStateJson";


        private void SaveStateToTemp()
        {
           
            QS.Questions = Questions?.ReadAll() ?? new List<Adverb_Question>();
            
             // preserve animation flag briefly so view can show animation then client-side script will revert
            QS.ShowCorrectAnimation = QS.ShowCorrectAnimation;

            var json = JsonSerializer.Serialize(QS);
            TempData[TempKey] = json;
        }

        private void LoadStateFromTemp()
        {
            if (!TempData.ContainsKey(TempKey)) return;

            var json = TempData.Peek(TempKey) as string;
            if (string.IsNullOrEmpty(json)) return;

            try
            {
                var state = JsonSerializer.Deserialize<QuizState>(json);
                if (state == null) return;
                
                Questions = new GenericRepo<Adverb_Question>(state.Questions ?? new List<Adverb_Question>());
                QS.Current = state.Current;
                QS.Lives = state.Lives;
                QS.CorrectCount = state.CorrectCount;
                QS.ShowEnglishHint = state.ShowEnglishHint;
                QS.ShowSentenceHint = state.ShowSentenceHint;
                QS.ShowCorrectAnimation = state.ShowCorrectAnimation;
            }
            catch
            {
                // ignore deserialization errors
            }
        }

       
    }
}
