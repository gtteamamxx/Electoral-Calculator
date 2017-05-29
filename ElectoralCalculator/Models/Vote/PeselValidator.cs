using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectoralCalculator.Models.Vote
{
    public class PeselValidator
    {
        /* Class from internet */
        public bool ValidatePesel(string pesel, out DateTime dateTime)
        {
            dateTime = new DateTime();

            if (string.IsNullOrWhiteSpace(pesel))
                return false;

            byte[] controlBits = new byte[10] { 9, 7, 3, 1, 9, 7, 3, 1, 9, 7 };
            byte[] controlBIts = new byte[] { 48, 49, 50, 51, 52, 53, 54, 55, 56, 57 };
            bool result = false;
            int sum = 0;
            int controlSum = 0;

            pesel = pesel.Trim();

            if (pesel.Length != 11)
                return false;

            foreach (char l in pesel)
            {
                byte b = Convert.ToByte(l);
                if (Array.IndexOf(controlBIts, Convert.ToByte(l)) == -1)
                    return false;
            }

            controlSum = Convert.ToInt32(pesel[10].ToString());

            for (int i = 0; i < 10; i++)
                sum += controlBits[i] * Convert.ToInt32(pesel[i].ToString());

            result = ((sum % 10) == controlSum);

            if (!result)
                return false;

            int year = 0;
            int month = 0;
            int day = Convert.ToInt32(pesel[4].ToString()) * 10 + Convert.ToInt32(pesel[5].ToString());

            if (pesel[2] == '0' || pesel[2] == '1')
            {
                year = 1900;
                month = Convert.ToInt32(pesel[2].ToString()) * 10 + Convert.ToInt32(pesel[3].ToString());
            }
            else if (pesel[2] == '2' || pesel[2] == '3')
            {
                year = 2000;
                month = (Convert.ToInt32(pesel[2].ToString()) * 10 + Convert.ToInt32(pesel[3].ToString()) - 20);
            }
            else if (pesel[2] == '4' || pesel[2] == '5')
            {
                year = 2100;
                month = (Convert.ToInt32(pesel[2].ToString()) * 10 + Convert.ToInt32(pesel[3].ToString()) - 40);
            }
            else if (pesel[2] == '6' || pesel[2] == '7')
            {
                year = 2200;
                month = (Convert.ToInt32(pesel[2].ToString()) * 10 + Convert.ToInt32(pesel[3].ToString()) - 60);
            }
            else if (pesel[2] == '8' || pesel[2] == '9')
            {
                year = 1800;
                month = (Convert.ToInt32(pesel[2].ToString()) * 10 + Convert.ToInt32(pesel[3].ToString()) - 80);
            }
            year += Convert.ToInt32(pesel[0].ToString()) * 10 + Convert.ToInt32(pesel[1].ToString());
            String szDate = year.ToString() + "-" + (month < 10 ? "0" + month.ToString() : month.ToString()) + "-" + (day < 10 ? "0" + day.ToString() : day.ToString());
            result = DateTime.TryParse(szDate, out dateTime);
            return result;
        }
    }
}