using System;
using System.Collections.Generic;
using System.Text;

namespace Vocable
{
    public class Adverb_Question : IContainId
    {
        #region instance fields

        private int _id;
        private string _question;
        private string _helpEnglish;
        private string _helpSentence;
        private string _helpImagePath;
        #endregion

        #region properties
        public string Question { get { return _question; } set { _question = value; } }
        public int Id { get { return _id; } set { _id = value; } }
        public string HelpEnglish { get { return _helpEnglish; } set { _helpEnglish = value; } }
        public string HelpSentence { get { return _helpSentence; } set { _helpSentence = value; } }
        public string HelpImagePath { get { return _helpImagePath; } set { _helpImagePath = value; } }
        #endregion

        #region constructors
        public Adverb_Question() 
        {
            _id = 0;
            _question = "Placeholder";
            _helpImagePath = "Image-path";
            _helpEnglish = "Placeholder";
            _helpSentence = "This is a placeholder.";
        }

        public Adverb_Question(int id, string question, string helpImage, string helpEng, string help, string sentence)
        {
            _id = id;
            _question = question;
            _helpImagePath = helpImage;
            _helpEnglish = helpEng;
            _helpSentence = sentence;
        }
        #endregion

        #region methods
        public override string ToString()
        {
            return _helpImagePath;
        }
        #endregion

    }
}
