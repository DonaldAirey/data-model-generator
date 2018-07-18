// <copyright file="LoadStateEventArgs.cs" company="Gamma Four, Inc.">
//     Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.Navigation
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Class used to hold the event data required when a page attempts to load state.
    /// </summary>
    public class LoadStateEventArgs : EventArgs
    {
        /// <summary>
        /// The parameter value passed to navigation method when this page was initially requested.
        /// </summary>
        private object navigationParameterField;

        /// <summary>
        /// A dictionary of state preserved by this page during an earlier session.  This will be null the first time a page is visited.
        /// </summary>
        private Dictionary<string, object> pageStateField;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadStateEventArgs"/> class.
        /// </summary>
        /// <param name="navigationParameter">
        /// The parameter value passed to navigation method when this page was initially requested.
        /// </param>
        /// <param name="pageState">
        /// A dictionary of state preserved by this page during an earlier session.  This will be null the first time a page is visited.
        /// </param>
        public LoadStateEventArgs(object navigationParameter, Dictionary<string, object> pageState)
            : base()
        {
            // Initialize the object.
            this.navigationParameterField = navigationParameter;
            this.pageStateField = pageState;
        }

        /// <summary>
        /// Gets the parameter value passed to navigation method when this page was initially requested.
        /// </summary>
        public object NavigationParameter
        {
            get
            {
                return this.navigationParameterField;
            }
        }

        /// <summary>
        /// Gets a dictionary of state preserved by this page during an earlier session.  This will be null the first time a page is visited.
        /// </summary>
        public Dictionary<string, object> PageState
        {
            get
            {
                return this.pageStateField;
            }
        }
    }
}