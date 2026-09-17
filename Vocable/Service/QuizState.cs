using System;
using System.Collections.Generic;
using System.Text;

namespace Vocable.Service
{
    public class QuizState
    {
        public List<Adverb_Question>? Questions { get; set; }
        public int Current { get; set; }
        public int Lives { get; set; }
        public int CorrectCount { get; set; }
        public bool ShowEnglishHint { get; set; }
        public bool ShowSentenceHint { get; set; }
        public bool ShowCorrectAnimation { get; set; }
        public string Message { get; set; }
        public string MessageClass { get; set; }
        public bool GameStarted { get; set; }
        public bool GameEnded { get; set; }
        public string FinalMessage { get; set; } 


        public QuizState()
        {
            Questions = new List<Adverb_Question>();
            Current = 0;
            Lives = 0;
            CorrectCount = 0;
            ShowEnglishHint = false;
            ShowSentenceHint = false;
            ShowCorrectAnimation = false;
            Message = string.Empty;
            MessageClass = string.Empty;
            GameStarted = false;
            GameEnded = false;
            FinalMessage = string.Empty;
            
        }

    }
}
