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
    public partial class ViewAllQuotes : Form
    {
        public ViewAllQuotes()
        {
            InitializeComponent();
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
    }
}
