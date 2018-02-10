﻿using System;
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
        private static Dictionary<int, int[]> rushAdditionalCost = new Dictionary<int, int[]>();
        private string customerName;
        public string CustomerName { get => customerName; set => customerName = value; }
        private decimal? _quoteAmount = null;
        public decimal QuoteAmount
        {
            get
            {
                if (_quoteAmount == null)
                    CalculateQuote();
                return _quoteAmount.Value;
            }
            private set
            {
                _quoteAmount = value;
            }
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

            if (!rushAdditionalCost.Any())
            {
                var rushOrderPrices = File.ReadAllLines("rushOrderPrices.txt").Select(l => int.Parse(l)).ToArray();

                rushAdditionalCost.Add(3, new int[] { rushOrderPrices[0], rushOrderPrices[1], rushOrderPrices[2] });
                rushAdditionalCost.Add(5, new int[] { rushOrderPrices[3], rushOrderPrices[4], rushOrderPrices[5] });
                rushAdditionalCost.Add(7, new int[] { rushOrderPrices[6], rushOrderPrices[7], rushOrderPrices[8] });
            }
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
                var sa = Desk.GetSurfaceArea();
                if (sa < 1000)
                {
                    cost += (decimal)rushAdditionalCost[ProductionDays][0];
                }
                else if(sa < 2001)
                {
                    cost += (decimal)rushAdditionalCost[ProductionDays][1];
                }
                else
                {
                    cost += (decimal)rushAdditionalCost[ProductionDays][2];
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
