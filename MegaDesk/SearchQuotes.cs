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
                var lines = File.ReadAllLines("DeskQuotes.csv")
                    .Select(dq => dq.Split(',')).ToList();

                this.allQuotes = lines.Select(line => {
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
            var searchResults = allQuotes.FindAll(q => q.Desk.Material == searchForMaterial);

            searchResultsListView.Items.Clear();
            if (searchResults.Count < 1)
            {
                searchResultsListView.Items.Add("No Data");
            }
            else
            {
                foreach (var result in searchResults)
                {
                    searchResultsListView.Items.Add(new ListViewItem(new[] { result.CustomerName, result.Desk.Material.ToString(), result.QuoteAmount.ToString() }));
                }
            }
        }
    }
}
