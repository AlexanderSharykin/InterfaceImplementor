using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ViewModels
{
    /// <summary>
    /// ViewModel for OpenFileDialog
    /// </summary>
    public class OpenFileVm
    {
        /// <summary>
        /// Gets or sets dialog title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or setw file types filter
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// Gets or sets select file name
        /// </summary>
        public string FileName { get; set; }        
    }
}
