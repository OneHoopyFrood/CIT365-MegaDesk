using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MegaDesk_ColePanike
{
    public partial class ViewQuote : Form
    {
        public ViewQuote(DeskQuote displayQuote)
        {
            InitializeComponent();

            DisplayQuote(displayQuote);
        }

        private void DisplayQuote(DeskQuote displayQuote)
        {
            customerName.Text = displayQuote.CustomerName;
            quoteAmount.Text = displayQuote.QuoteAmount.ToString("C");
            width.Text = displayQuote.Desk.Width.ToString();
            depth.Text = displayQuote.Desk.Depth.ToString();
            numDrawers.Text = displayQuote.Desk.NumDrawers.ToString();
            material.Text = displayQuote.Desk.Material.ToString();
            daysToDeliver.Text = displayQuote.ProductionDays.ToString() + (displayQuote.ProductionDays < 14 ? " (Rush)" : "");
            quoteDate.Text = displayQuote.QuoteDate.ToShortDateString();
        }
    }
}
