using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarConfigurator.Models
{
    public class CarConfiguration
    {
        public string ModelName { get; set; } = "";
        public decimal BasePrice { get; set; }
        public string EngineName { get; set; } = "";
        public decimal EnginePrice { get; set; }
        public string ColorName { get; set; } = "";
        public decimal ColorPrice { get; set; }
        public List<CarOption> SelectedOptions { get; set; } = new List<CarOption>();
        public int DownPaymentPercent { get; set; } = 20;
        public int LoanMonths { get; set; } = 60;
        public string CustomerName { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Email { get; set; } = "";
        public decimal OptionsTotal
        {
            get { return SelectedOptions.Sum(o => o.Price); }
        }
        public decimal TotalPrice
        {
            get
            {
                return BasePrice + EnginePrice + ColorPrice + OptionsTotal;
            }
        }
        public decimal DownPayment
        {
            get
            {
                return TotalPrice * DownPaymentPercent / 100;
            }
        }
        public decimal CreditAmount
        {
            get
            {
                return TotalPrice - DownPayment;
            }
        }
        public decimal MonthlyPayment
        {
            get
            {
                return LoanMonths > 0 ? CreditAmount / LoanMonths : 0;
            }
        }
    }
    public class CarOption
    {
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
    }
}
