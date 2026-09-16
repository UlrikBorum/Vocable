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
        private readonly AdverbsService _adverbsService;

        private readonly GenericRepo<Adverb_Question> _repo;
    

        public IndexModel(AdverbsService adverbsService, GenericRepo<Adverb_Question> repo)
        {
            _adverbsService = adverbsService;
            _repo = repo;
        }


        #region properties
        [BindProperty]
        public int Current { get; set; }

        [BindProperty]
        public int Lives { get; set; }

        [BindProperty]
        public int CorrectCount { get; set; }

        [BindProperty]
        public bool ShowEnglishHint { get; set; }

        [BindProperty]
        public bool ShowSentenceHint { get; set; }

        [BindProperty]
        public bool ShowCorrectAnimation { get; set; }

        [BindProperty]
        public string Answer { get; set; } = string.Empty;

        [BindProperty]
        public GenericRepo<Adverb_Question> Questions { get; set; } = new GenericRepo<Adverb_Question>();
        




        public string Message { get; set; } = string.Empty;
        public string MessageClass { get; set; } = string.Empty;
        public bool GameStarted { get; set; }
        public bool GameEnded { get; set; }
        public string FinalMessage { get; set; } = string.Empty;


        #endregion

        public void OnGet()
        {
            // initial GET - nothing to do
        }

        // Start using the DI-registered service from Program.cs
        public IActionResult OnPostStart()
        {
            // Service already populated in Program.cs as singleton
            var qs = _adverbsService.GetRandomAdverbQuestions(5);

            Questions = qs;
            Current = 0;
            Lives = 3;
            CorrectCount = 0;
            ShowEnglishHint = false;
            ShowSentenceHint = false;
            ShowCorrectAnimation = false;
            Message = string.Empty;
            GameStarted = true;
            GameEnded = false;

            // Persist questions between POST requests so hint actions can reload them
            SaveStateToTemp();

            return Page();
        }

   
        public IActionResult OnPostSubmit()
        {
            // Reload persisted state (forms don't post the full repo)
            LoadStateFromTemp();

            var qs = Questions;

            if (qs.ReadAll().Count == 0)
            {
                Message = "No questions available.";
                MessageClass = string.Empty;
                GameStarted = false;
                return Page();
            }

            // ensure bounds
            if (Current < 0) Current = 0;
            if (Current >= qs.ReadAll().Count)
            {
                EndGame();
                return Page();
            }

            var q = qs.ReadAll()[Current];
            var given = (Answer ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(given))
            {
                Message = "Please enter an answer.";
                MessageClass = "message-wrong";
                GameStarted = true;
                // ensure no lingering correct-animation flag when user hasn't answered
                ShowCorrectAnimation = false;
                return Page();
            }

            if (string.Equals(given, q.Question ?? string.Empty, StringComparison.OrdinalIgnoreCase))
            {
                CorrectCount++;
                Message = "Correct! Moving to next question.";
                MessageClass = "message-correct";
                // show short animation between questions
                ShowCorrectAnimation = true;
                Current++;
                    // clear the answer for the next question and remove any ModelState entry so the tag helper
                    // renders the updated empty value (ModelState values take precedence over the property)
                    Answer = string.Empty;
                    ModelState.Remove(nameof(Answer));
                Lives = 3;
                ShowEnglishHint = false;
                ShowSentenceHint = false;
            }
            else
            {
                Lives--;
                if (Lives <= 0)
                {
                    Message = $"No lives left. The answer was {q.Question}! Moving to next question.";
                    MessageClass = "message-wrong";
                    Current++;
                    // clear the answer when advancing after losing all lives and remove ModelState entry
                    Answer = string.Empty;
                    ModelState.Remove(nameof(Answer));
                    Lives = 3;
                    ShowEnglishHint = false;
                    ShowSentenceHint = false;
                    // ensure no animation when advancing due to losing lives
                    ShowCorrectAnimation = false;
                }
                else
                {
                    Message = $"Wrong. {Lives} lives remaining.";
                    MessageClass = "message-wrong";
                    // don't show correct animation on wrong answer
                    ShowCorrectAnimation = false;
                }
            }

            if (Current >= qs.ReadAll().Count)
            {
                EndGame();
            }

            GameStarted = !GameEnded;

            // Persist updated state so the next POST can reload the same questions
            if (!GameEnded)
            {
                SaveStateToTemp();
            }

            return Page();
        }

        public IActionResult OnPostRevealEnglish()
        {
            // reload state so page can render helper text (if needed)
            LoadStateFromTemp();

            ShowEnglishHint = true;
            GameStarted = true;
            // ensure animation flag is cleared when revealing hints (only answers should trigger it)
            ShowCorrectAnimation = false;
            // persist hint state
            SaveStateToTemp();
            return Page();
        }

        public IActionResult OnPostRevealSentence()
        {
            // reload state so page can render helper text (if needed)
            LoadStateFromTemp();

            ShowSentenceHint = true;
            GameStarted = true;
            // ensure animation flag is cleared when revealing hints (only answers should trigger it)
            ShowCorrectAnimation = false;
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
            GameEnded = true;
            GameStarted = false;

            

            FinalMessage = $"You answered {CorrectCount} of {Questions.ReadAll().Count} correctly.";

            // clear persisted questions when game ends
            TempData.Remove(TempKey);
        }

        private const string TempKey = "QuizStateJson";


        private void SaveStateToTemp()
        {
            QuizState state = new QuizState
            {
                Questions = Questions?.ReadAll() ?? new List<Adverb_Question>(),
                CorrectCount = CorrectCount,
                Current = Current,  
                Lives = Lives,
                ShowEnglishHint = ShowEnglishHint,
                ShowSentenceHint = ShowSentenceHint,
                // preserve animation flag briefly so view can show animation then client-side script will revert
                ShowCorrectAnimation = ShowCorrectAnimation
            };

            var json = JsonSerializer.Serialize(state);
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
                Current = state.Current;
                Lives = state.Lives;
                CorrectCount = state.CorrectCount;
                ShowEnglishHint = state.ShowEnglishHint;
                ShowSentenceHint = state.ShowSentenceHint;
                ShowCorrectAnimation = state.ShowCorrectAnimation;
            }
            catch
            {
                // ignore deserialization errors
            }
        }

       
    }
}
