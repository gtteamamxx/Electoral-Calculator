using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectoralCalculator.Models.DTO;
using System.Text.RegularExpressions;

namespace ElectoralCalculator.Models.Vote
{
    public class UserFullnameValidator
    {
        public bool ValidateFullName(string firstName, string surName)
        {
            if (!ValidateText(firstName))
                return false;
            if (!ValidateText(surName))
                return false;
            return true;
        }

        private bool ValidateText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;
            if (string.IsNullOrWhiteSpace(text))
                return false;
            if (text.Length <= 1)
                return false;
            if (!Regex.Match(text.Trim(), @"^[a-źA-Ź]+$").Success)
                return false;
            return true;
        }
    }
}
