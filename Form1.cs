/*
Author: Jay Patel, 000881881
Date: 15/11/2023
Purpose: Lab 4 Part B
I, Jay Patel, 000881881 certify that this material is my original work.  No other person's work has been used without due acknowledgement.
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace Lab4B
{
    public partial class Form1 : Form
    {
        private string selectedFilePath;
        private string filename;
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Click event of the "Load File" menu item.
        /// Opens a file dialog to load an HTML file.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void loadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Clear the list box before loading a new file
            listBox1.Items.Clear();

            // Open file dialog to select an HTML file
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "HTML Files(*.html;*.htm) | *.html;*.htm";

            // If the user selects a file, enable the "Check Tags" menu item
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                checkTagsToolStripMenuItem.Enabled = true;
                selectedFilePath = openFileDialog.FileName;
                filename = Path.GetFileName(selectedFilePath);

                // Display the loaded filename in the label
                label1.Text = ($"Loaded: {filename}");
            }
        }

        /// <summary>
        /// Handles the Click event of the "Check Tags" menu item.
        /// Checks if the HTML file has balanced tags.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void checkTagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Check if the HTML tags are balanced
            bool tagsBalanced = CheckTags(selectedFilePath);

            // Update the label based on the result of tag checking
            if (tagsBalanced)
            {
                label1.Text = ($"{filename} has balanced tags.");
            }
            else
            {
                label1.Text = ($"{filename} does not have balanced tags.");
            }
        }

        /// <summary>
        /// Checks if the HTML file has balanced tags.
        /// </summary>
        /// <param name="filePath">The path to the HTML file.</param>
        /// <returns>True if tags are balanced; otherwise, false.</returns>
        private bool CheckTags(string filePath)
        {
            try
            {
                // Read the HTML file content
                string htmlContent = File.ReadAllText(filePath);

                // Initialize counters for opening and closing tags
                int openingTagCount = 0;
                int closingTagCount = 0;

                // Define a stack to keep track of opening tags and their nesting level
                Stack<string> tagStack = new Stack<string>();

                // Define a regular expression to match HTML tags
                Regex tagRegex = new Regex(@"<[^>]+>");

                // Match all HTML tags in the content
                MatchCollection tagMatches = tagRegex.Matches(htmlContent);

                // Process each matched tag
                foreach (Match match in tagMatches)
                {
                    string tag = match.Value;

                    // Check if the tag is an opening tag
                    if (tag.StartsWith("<") && !tag.StartsWith("</"))
                    {
                        // Ignore certain tags
                        if (!IsIgnoredTag(tag))
                        {
                            // Increment the opening tag counter
                            openingTagCount++;

                            // Get the tag name without attributes
                            string tagName = GetTagName(tag).ToLower();

                            // Indentation for display
                            string indentation = new string(' ', tagStack.Count * 2);

                            listBox1.Items.Add($"{indentation}Found opening tag: <{tagName}>");

                            // Push the opening tag onto the stack
                            tagStack.Push(tagName);
                        }
                        else
                        {
                            // If it's an ignored tag, use the same indentation
                            string indentation = new string(' ', tagStack.Count * 2);
                            string tagName = GetTagName(tag).ToLower();
                            listBox1.Items.Add($"{indentation}Found opening tag: <{tagName}>");
                        }
                    }
                    else if (tag.StartsWith("</"))
                    {
                        // Increment the closing tag counter
                        closingTagCount++;

                        // Get the tag name without attributes
                        string tagName = GetTagName(tag).ToLower();

                        // If the stack is not empty, check if the last opening tag corresponds
                        if (tagStack.Count > 0)
                        {
                            var poppedTag = tagStack.Pop();

                            // Indentation for display
                            string indentation = new string(' ', tagStack.Count * 2);

                            listBox1.Items.Add($"{indentation}Found closing tag: <{tagName}>");
                        }
                        else
                        {
                            // If the stack is empty, there is no corresponding opening tag
                            return false;
                        }
                    }
                }

                // If both counters are equal, all tags are balanced
                if (openingTagCount == closingTagCount)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                label1.Text = (ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Checks if a tag should be ignored.
        /// </summary>
        /// <param name="tag">The tag to check.</param>
        /// <returns>True if the tag should be ignored; otherwise, false.</returns>
        private bool IsIgnoredTag(string tag)
        {
            // Define a list of tags to be ignored
            List<string> ignoredTags = new List<string> { "<img", "<hr", "<br" };

            // Check if the tag is in the list of ignored tags
            return ignoredTags.Any(ignoredTag => tag.StartsWith(ignoredTag));
        }

        /// <summary>
        /// Gets the tag name without attributes.
        /// </summary>
        /// <param name="tag">The full tag.</param>
        /// <returns>The tag name.</returns>
        private string GetTagName(string tag)
        {
            // Remove attributes from the tag to get only the tag name
            int indexOfSpace = tag.IndexOf(' ');
            if (indexOfSpace != -1)
            {
                return tag.Substring(1, indexOfSpace - 1);
            }
            else
            {
                // If no space is found, remove the closing angle bracket
                return tag.Substring(1, tag.Length - 2);
            }
        }


        // Exits the application
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}
