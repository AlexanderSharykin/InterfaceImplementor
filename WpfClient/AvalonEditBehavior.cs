using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interactivity;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;

namespace WpfClient
{
    /// <summary>
    /// Wrapper for binding AvalonEdit.TextEditor properties
    /// </summary>
    /// <see cref="http://stackoverflow.com/a/18969007/1506454"/>
    /// <remarks>Author: leolorenzoluis, http://stackoverflow.com/users/493389 </remarks>
    public sealed class AvalonEditBehaviour : Behavior<TextEditor>
    {
        public static readonly DependencyProperty EditorTextProperty =
            DependencyProperty.Register("EditorText", typeof(string), typeof(AvalonEditBehaviour),
            new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, EditorTextChangedCallback));

        public string EditorText
        {
            get { return (string)GetValue(EditorTextProperty); }
            set { SetValue(EditorTextProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject != null)
                AssociatedObject.TextChanged += EditorOnTextChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (AssociatedObject != null)
                AssociatedObject.TextChanged -= EditorOnTextChanged;
        }

        private void EditorOnTextChanged(object sender, EventArgs eventArgs)
        {
            var textEditor = sender as TextEditor;
            if (textEditor != null)
            {
                if (textEditor.Document != null && EditorText != textEditor.Document.Text)
                {
                    EditorText = textEditor.Document.Text;
                }
            }
        }

        private static void EditorTextChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs dp)
        {
            var behavior = (AvalonEditBehaviour)obj;
            if (behavior.AssociatedObject != null)
            {
                var editor = behavior.AssociatedObject;
                if (editor.Document != null)
                {
                    string code = dp.NewValue.ToString();
                    if (editor.TextArea.IndentationStrategy != null)
                    {
                        var doc = new TextDocument {Text = code};
                        editor.TextArea.IndentationStrategy.IndentLines(doc, 0, doc.Lines.Count - 1);
                        code = doc.Text;
                    }
                    editor.Document.Text = code;
                }
            }
        }
    }
}
