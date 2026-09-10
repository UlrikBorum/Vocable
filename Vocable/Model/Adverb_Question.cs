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
        #endregion

        #region properties
        public string Question { get { return _question; } set { _question = value; } }
        public int Id { get { return _id; } set { _id = value; } }
        #endregion

        #region constructors
        public Adverb_Question() { }
        #endregion


    }
}
