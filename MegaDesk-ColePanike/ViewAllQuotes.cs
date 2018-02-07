using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MegaDesk_ColePanike
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
                var lines = File.ReadAllLines("DeskQuotes.csv")
                    .Select(dq => dq.Split(',')).ToList();


                this.AllQuotes = lines.Select(line => {
                    Desk.DeskMaterial material;
                    Enum.TryParse<Desk.DeskMaterial>(line[4], out material);
                    Desk desk = new Desk(int.Parse(line[1]), int.Parse(line[2]), int.Parse(line[3]), material);
                    DeskQuote loadedQuote = new DeskQuote(line[0], desk, String.IsNullOrEmpty(line[5]) ? 14 : int.Parse(line[5]));
                    loadedQuote.QuoteDate = DateTime.Parse(line[6]);
                    return loadedQuote;
                }).ToList();
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
