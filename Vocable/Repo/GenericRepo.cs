using System;
using System.Collections.Generic;
using System.Text;


namespace Vocable
{
    public class GenericRepo<T> : IRepo where T : IContainId
    {
        #region Instance fields

        private List<T> _items = new List<T>();


        #endregion
        #region Properties
        #endregion

        #region Constructors
        public GenericRepo() { }
        #endregion




    }
}
