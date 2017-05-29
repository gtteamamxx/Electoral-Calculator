using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectoralCalculator.Models.Database;
using System.Windows.Controls;
using Microsoft.Win32;
using System.IO;
using System.IO.Packaging;
using System.Windows.Xps.Packaging;
using ElectoralCalculator.Models.Stats;
using System.Windows;
using ElectoralCalculator.Models.Application;

namespace ElectoralCalculator.Models.Save
{
    public class PDFSaverMethod : ISaveMethod
    {
        public void Save(StatsNonDb stats)
        {
            var content = new StatsViewGenerator().GenerateNumericStatsToFileContent(stats);
            if(content == null)
            {
                ShowUnableToSaveFileMessage();
                return;
            }
            content.HorizontalContentAlignment = HorizontalAlignment.Center;
            SaveToPdf(content);
        }

        private void SaveToPdf(UserControl content)
        {
            var saveFileDialog = new SaveFileDialog()
            {
                FileName = "stats",
                DefaultExt = ".pdf",
                Filter = "PDF Documents (.pdf)|*.pdf"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                using (var xpsMemorySteam = new MemoryStream())
                {
                    try
                    {
                        using (var xpsPackage = Package.Open(xpsMemorySteam, FileMode.Create))
                        {
                            using (XpsDocument doc = new XpsDocument(xpsPackage))
                                XpsDocument.CreateXpsDocumentWriter(doc).Write(content);
                        }
                        PdfSharp.Xps.XpsConverter.Convert(
                            PdfSharp.Xps.XpsModel.XpsDocument.Open(xpsMemorySteam), saveFileDialog.FileName, 0);
                        MessageBox.Show("File was saved succesfully", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch
                    {
                        ShowUnableToSaveFileMessage();
                    }
                }
            }
        }

        private void ShowUnableToSaveFileMessage()
            => MessageBoxHelper.Show("Unable to save file", MessageBoxHelper.MessageType.Error);

    }
}
