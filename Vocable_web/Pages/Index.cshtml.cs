using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Vocable.Service;
using Vocable;
using System.Linq;
using System.Text.Json;

namespace Vocable_web.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {

        }

        // Return 5 random questions as JSON for the client-side quiz
        public IActionResult OnGetQuestions()
        {
            var svc = new AdverbsService();
            // This method populates the internal repo with placeholder questions
            svc.AddAnAdverbQuestionRepo(null);
            var questions = svc.GetRandomAdverbQuestions(5);

            // Return only the fields needed by the UI
            var payload = questions.Select(q => new
            {
                q.Id,
                q.Question,
                q.HelpEnglish,
                q.HelpSentence,
                q.HelpImagePath,

            }).ToArray();

            return new JsonResult(payload);
        }
    }
}
