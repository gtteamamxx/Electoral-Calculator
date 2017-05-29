using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectoralCalculator.Models.Database;
using System.IO;
using Microsoft.Win32;
using System.Windows;
using ElectoralCalculator.Models.Application;

namespace ElectoralCalculator.Models.Save
{
    public class CSVSaverMethod : ISaveMethod
    {
        public void Save(StatsNonDb stats)
        {
            string csv = GenerateCSVFromStats(stats);
            if(csv == null)
            {
                ShowUnableToSaveFileMessage();
                return;
            }
            SaveToFile(csv);
        }

        private void SaveToFile(string csv)
        {
            var saveFileDialog = new SaveFileDialog()
            {
                FileName = "stats",
                DefaultExt = ".csv",
                Filter = "CSV Files (.csv)|*.csv"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    using (var fileStream = File.Create(saveFileDialog.FileName))
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(csv);
                        fileStream.Write(bytes, 0, bytes.Length);
                    }
                    MessageBoxHelper.Show("File was saved succesfully", MessageBoxHelper.MessageType.Information);
                }
                catch
                {
                    ShowUnableToSaveFileMessage();
                }
            }
        }

        private void ShowUnableToSaveFileMessage()
            => MessageBoxHelper.Show("Unable to save file", MessageBoxHelper.MessageType.Error);

        private string GenerateCSVFromStats(StatsNonDb stats)
        {
            try
            {
                StringBuilder csvBuilder = new StringBuilder();
                csvBuilder.AppendLine($"valueName,value");

                csvBuilder.AppendLine($"Candidates,{stats.ValidVotesNum}");
                foreach (var candidate in stats.Candidates)
                    csvBuilder.AppendLine($"\"{getUTF8String(candidate.Name)}\",{candidate.VotesNum}");

                csvBuilder.AppendLine($"Parties,{stats.ValidVotesNum}");
                foreach (var party in stats.Parties)
                    csvBuilder.AppendLine($"\"{getUTF8String(party.Name)}\",{party.VotesNum}");

                csvBuilder.AppendLine($"Votes,{stats.UnvalidVotesNun + stats.ValidVotesNum}");
                csvBuilder.AppendLine($"Good,{stats.ValidVotesNum}");
                csvBuilder.AppendLine($"Bad,{stats.UnvalidVotesNun}");
                csvBuilder.AppendLine($"\"Without law tries\",{stats.WithoutLawTries}");
                return csvBuilder.ToString();
            }
            catch
            {
                return null;
            }
        }

        string getUTF8String(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
