using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ViewModels
{
    /// <summary>
    /// ViewModel for MessageBoxes
    /// </summary>
    public class MessageVm
    {
        /// <summary>
        /// Gets or sets message text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets message topic
        /// </summary>
        public string Caption { get; set; }
    }
}
