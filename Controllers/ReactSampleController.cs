using System;
using log4net;
using Microsoft.AspNetCore.Mvc;

namespace reactAzure.Controllers
{
    /// <summary>
    /// Base API Controller - all others derive from it
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ReactSampleController<T> : ControllerBase where T : ControllerBase
    {
        #region Properties
        protected ILog Logger
        {
            get;
            private set;
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Common logger
        /// </summary>
        protected ReactSampleController()
        {
            this.Logger = LogManager.GetLogger(typeof(T).FullName);
        }
        #endregion
    }
}
