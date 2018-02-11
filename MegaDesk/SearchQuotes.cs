using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MegaDesk
{
    public partial class SearchQuotes : Form
    {
        private List<DeskQuote> allQuotes;
        private List<DeskQuote> searchResults;

        public SearchQuotes()
        {
            InitializeComponent();

            searchResultsListView.Columns.Add("Customer", 100);
            searchResultsListView.Columns.Add("Material", 80);
            searchResultsListView.Columns.Add("Price", 100);
            searchResultsListView.Items.Add("No Data");

            materialComboBox.DataSource = Enum.GetValues(typeof(Desk.DeskMaterial));

            try
            {
                var text = File.ReadAllText("DeskQuotes.json");
                this.allQuotes = JsonConvert.DeserializeObject<List<DeskQuote>>(text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error! Failed to read file!");
            }
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            return_to_main();
        }

        private void return_to_main()
        {
            var mainMenu = (MegaDesk)Tag;
            mainMenu.Show();
            Close();
        }

        private void materialComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (allQuotes == null)
                return;

            var searchForMaterial = (Desk.DeskMaterial)materialComboBox.SelectedValue;
            searchResults = allQuotes.FindAll(q => q.Desk.Material == searchForMaterial);

            searchResultsListView.Items.Clear();
            if (searchResults.Count < 1)
            {
                searchResultsListView.Items.Add("No Data");
            }
            else
            {
                foreach (var result in searchResults)
                {
                    searchResultsListView.Items.Add(new ListViewItem(new[] { result.CustomerName, result.Desk.Material.ToString(), result.QuoteAmount.ToString("C") }));
                }
            }
        }

        private void searchResultsListView_ItemActivate(object sender, EventArgs e)
        {
            ListView list = (ListView)sender;
            DeskQuote quote = searchResults[list.FocusedItem.Index];
            ViewQuote viewQuoteForm = new ViewQuote(quote);
            viewQuoteForm.Tag = this;
            viewQuoteForm.Show(this);
        }
    }
}
