using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaDesk
{
    public class DeskQuote
    {
        public Desk Desk;
        private string customerName;
        public string CustomerName { get => customerName; set => customerName = value; }
        public decimal QuoteAmount
        {
            get;
            private set;
        }
        public DateTime QuoteDate
        {
            get;
            set;
        }
        public int ProductionDays
        {
            get;
            private set;
        }

        public DeskQuote(string customerName, Desk desk, int productionDays)
        {
            this.CustomerName = customerName;
            this.Desk = desk;
            this.ProductionDays = productionDays;

            CalculateQuote();
            QuoteDate = DateTime.Now;
        }

        public DeskQuote(string customerName, int width, int depth, int numDrawers, Desk.DeskMaterial material, int productionDays) 
            : this(customerName, new Desk(width, depth, numDrawers, material), productionDays)
        {

        }

        private void CalculateQuote()
        {
            decimal cost = 200;
            int surfaceArea = this.Desk.GetSurfaceArea();
            if (surfaceArea > 1000)
                cost += (surfaceArea - 1000);
            cost += Desk.NumDrawers * 50;

            switch (this.Desk.Material)
            {
                case Desk.DeskMaterial.Pine:
                    cost += 50;
                    break;
                case Desk.DeskMaterial.Laminate:
                    cost += 100;
                    break;
                case Desk.DeskMaterial.Veneer:
                    cost += 125;
                    break;
                case Desk.DeskMaterial.Oak:
                    cost += 200;
                    break;
                case Desk.DeskMaterial.Rosewood:
                    cost += 300;
                    break;
            }

            if (IsRush())
            {
                switch (this.ProductionDays)
                {
                    case 3:
                        cost += 60;
                        cost += (surfaceArea % 1000) * 10;
                        break;
                    case 5:
                        cost += 40;
                        cost += (surfaceArea % 1000) * 10;
                        break;
                    case 7:
                        cost += 30;
                        cost += (surfaceArea % 1000) * 5;
                        break;
                }
            }

            this.QuoteAmount = cost;
        }

        private bool IsRush()
        {
            return ProductionDays < 14;
        }

        public void SaveToCSV(string filename)
        {
            File.AppendAllText(filename, 
                "\n" + String.Join(",", this.customerName, this.Desk.Width, this.Desk.Depth, this.Desk.NumDrawers, 
                (int)this.Desk.Material, this.ProductionDays, this.QuoteDate.ToShortDateString()));
        }
    }
}
