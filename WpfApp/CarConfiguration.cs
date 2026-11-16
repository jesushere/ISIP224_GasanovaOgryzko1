using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    public class CarConfiguration
    {
        public string ModelName { get; set; }
        public string EngineType { get; set; }
        public string Color { get; set; }

        public decimal BasePrice { get; set; }
        public decimal EnginePrice { get; set; }
        public decimal ColorPrice { get; set; }
        public decimal OptionsPrice { get; set; }

        public List<string> SelectedOptions { get; set; }

        public int DownPayment { get; set; }
        public int LoanMonths { get; set; }

        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public string ClientPhone { get; set; }

        public decimal TotalPrice //общая стоимость автомобиля
        {
            get
            {
                return BasePrice + EnginePrice + ColorPrice + OptionsPrice;
            }
        }

        public decimal DownPaymentAmount //сумма первоначального взноса
        {
            get
            {
                return TotalPrice * DownPayment / 100;
            }
        }

        public decimal MonthlyPayment //ежемесячный платеж
        {
            get
            {
                if (LoanMonths == 0)
                {
                    return 0;
                }
                else
                {
                    return CreditAmount/LoanMonths;
                }
            }
        }

        public decimal CreditAmount //сумма кредита
        {
            get
            {
                return TotalPrice - DownPaymentAmount;
            }
        }

        public CarConfiguration()
        {
            SelectedOptions = new List<string>();
            DownPayment = 20;
            LoanMonths = 36;
        }
    }
}
