using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MegaDesk
{
    public partial class ViewAllQuotes : Form
    {
        public List<DeskQuote> AllQuotes;

        public ViewAllQuotes()
        {
            InitializeComponent();

            LoadQuotes();
            DisplayQuotes();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            ReturnToMain();
        }

        private void ReturnToMain()
        {
            var mainMenu = (MegaDesk)Tag;
            mainMenu.Show();
            Close();
        }

        private void LoadQuotes()
        {
            try
            {
                var text = File.ReadAllText("DeskQuotes.json");
                this.AllQuotes = JsonConvert.DeserializeObject<List<DeskQuote>>(text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error! Failed to read file!");
            }
        }

        private void DisplayQuotes()
        {
            AllQuoteListView.Columns.Add("Customer", 100);
            AllQuoteListView.Columns.Add("Material", 80);
            AllQuoteListView.Columns.Add("Price", 100);
            if (this.AllQuotes != null)
            {
                foreach (DeskQuote quote in this.AllQuotes)
                {
                    AllQuoteListView.Items.Add(new ListViewItem(new []{ quote.CustomerName, quote.Desk.Material.ToString(), quote.QuoteAmount.ToString("C") }));
                }
            }
        }

        private void AllQuoteListView_ItemActivate(object sender, EventArgs e)
        {
            ListView list = (ListView)sender;
            DeskQuote quote = AllQuotes[list.FocusedItem.Index];
            ViewQuote viewQuoteForm = new ViewQuote(quote);
            viewQuoteForm.Tag = this;
            viewQuoteForm.Show(this);
        }
    }
}
