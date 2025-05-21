
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SuspiciousTextScanner
{
    public partial class MainForm : Form
    {
        private readonly Dictionary<string, string> patterns = new()
        {
            { "URL", @"https?://[^\s]+" },
            { "Windows path", @"[a-zA-Z]:\\(?:[^\\\s]+\\)*[^\\\s]+" },
            { "Unix path", @"/(?:[^/\s]+/)*[^/\s]+" },
            { "IP Address", @"\b\d{1,3}(\.\d{1,3}){3}\b" },
            { "Batch command", @"\b(?:del|copy|move|shutdown|format|netstat|ping|whoami|taskkill|reg|sc)\b" },
            { "Shell command", @"\b(?:rm|mv|wget|curl|chmod|chown|nc|nmap|sudo|ssh)\b" }
        };

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new()
            {
                Filter = "Text Files|*.txt;*.log"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = dialog.FileName;
                string content = File.ReadAllText(filePath);
                listBoxResults.Items.Clear();

                foreach (var (label, pattern) in patterns)
                {
                    foreach (Match match in Regex.Matches(content, pattern, RegexOptions.IgnoreCase))
                    {
                        listBoxResults.Items.Add($"{label}: {match.Value}");
                    }
                }

                lblStatus.Text = $"Scan complete. Found {listBoxResults.Items.Count} suspicious items.";
            }
        }
    }
}
