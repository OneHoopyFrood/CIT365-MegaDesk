using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MegaDesk.Resources
{
    public partial class AddQuote : Form
    {
        private Desk desk;

        private ErrorProvider widthErrorProvider;
        private ErrorProvider depthErrorProvider;

        public AddQuote()
        {
            InitializeComponent();

            materialOptions.DataSource = Enum.GetValues(typeof(Desk.DeskMaterial));
            List<int> possiblePeriods = new List<int> { 3, 5, 7 };
            rushOptions.DataSource = (from numDays in possiblePeriods
                                     select numDays).ToList();

            desk = new Desk(24, 12, 2, Desk.DeskMaterial.Pine);

            widthErrorProvider = new ErrorProvider();
            widthErrorProvider.SetIconAlignment(this.widthInput, ErrorIconAlignment.MiddleRight);
            widthErrorProvider.SetIconPadding(this.widthInput, 2);

            depthErrorProvider = new ErrorProvider();
            depthErrorProvider.SetIconAlignment(this.depthInput, ErrorIconAlignment.MiddleRight);
            depthErrorProvider.SetIconPadding(this.depthInput, 2);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void ReturnToMain()
        {
            var mainMenu = (MegaDesk)Tag;
            mainMenu.Show();
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            ReturnToMain();
        }

        private void widthInput_Validating(object sender, CancelEventArgs e)
        {
            TextBox input = (TextBox)sender;
            try
            {
                int newWidth;
                if (Int32.TryParse(input.Text, out newWidth))
                {
                    desk.Width = newWidth;
                    widthErrorProvider.SetError(this.widthInput, String.Empty);
                    return;
                }
                else
                {
                    widthErrorProvider.SetError(this.widthInput, "Enter a number");
                }
            }
            catch (Exception ex)
            {
                widthErrorProvider.SetError(this.widthInput, "Must be between 24-96, inclusive");
            }
        }

        private void depthInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox input = (TextBox)sender;
            try
            {
                char newKey = e.KeyChar;
                if (char.IsControl(newKey))
                {
                    return; // Don't validate, just ignore
                }

                int newDepth;
                if (char.IsDigit(newKey) && Int32.TryParse(input.Text + newKey, out newDepth))
                {
                    desk.Depth = newDepth;
                    depthErrorProvider.SetError(this.depthInput, String.Empty);
                    return;
                }
                else
                {
                    depthErrorProvider.SetError(this.depthInput, "Enter a number");
                }
            }
            catch (Exception ex)
            {
                depthErrorProvider.SetError(this.depthInput, "Must be between 12-48, inclusive");
            }
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            try
            {
                Desk.DeskMaterial newMaterial = (Desk.DeskMaterial)materialOptions.SelectedValue;

                int prodDays = 14;
                if (rushRadioButton.Checked && rushOptions.SelectedValue != null)
                {
                    prodDays = (int)rushOptions.SelectedValue;
                }
                DeskQuote newQuote = new DeskQuote(customerName.Text, int.Parse(widthInput.Text), int.Parse(depthInput.Text), (int)numDrawersInput.Value, newMaterial, prodDays);
                newQuote.SaveToCSV("DeskQuotes.csv");
                MessageBox.Show("Save Successful!");
                ReturnToMain();
                ViewQuote view = new ViewQuote(newQuote);
                view.Show();
            }
            catch
            {
                MessageBox.Show("Failed to save!");
            }
        }

        private void rushRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            rushOptions.Enabled = rushRadioButton.Checked;
        }
    }
}
