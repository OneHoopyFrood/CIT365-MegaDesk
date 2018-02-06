using MegaDesk_ColePanike.Resources;
using System;
using System.Windows.Forms;

namespace MegaDesk_ColePanike
{
    public partial class MegaDesk : Form
    {
        public MegaDesk()
        {
            InitializeComponent();
        }

        private void addQuoteButton_Click(object sender, EventArgs e)
        {
            AddQuote addNewQuoteForm = new AddQuote();
            addNewQuoteForm.Tag = this;
            addNewQuoteForm.Show(this);
            Hide();
        }

        private void viewQuotesButton_Click(object sender, EventArgs e)
        {
            ViewAllQuotes viewQuotesForm = new ViewAllQuotes();
            viewQuotesForm.Tag = this;
            viewQuotesForm.Show(this);
            Hide();
        }

        private void searchQuotesButton_Click(object sender, EventArgs e)
        {
            SearchQuotes searchQuotesForm = new SearchQuotes();
            searchQuotesForm.Tag = this;
            searchQuotesForm.Show(this);
            Hide();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
