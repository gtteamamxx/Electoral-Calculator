using ElectoralCalculator.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ElectoralCalculator.Models.Stats
{
    public class StatsViewGenerator
    {
        public async Task<UserControl> GenerateStatsViewAsync()
        {
            try
            {
                var stats = await DatabaseManager.Instance.GetStatsAsync();
                var content = new UserControl()
                    .AddBasicGridToControl()
                    .AddTabControlToContent()
                        .AddTabItemToTabControl("Numeric stats", GenerateNumericStatsContent(stats))
                        .AddTabItemToTabControl("Graph stats", GenerateGraphStatsContent(stats))
                        .GetUserControlParent();

                SetStatsToUserControl(content, stats);
                return content;
            }
            catch
            {
                return null;
            }

            void SetStatsToUserControl(UserControl userControl, StatsNonDb stats)
                => userControl.DataContext = stats;
        }

        public UserControl GenerateNumericStatsToFileContent(StatsNonDb stats)
        {
            try
            {
                var content = new UserControl()
                  .AddBasicScrollViewer()
                      .AddBasicStackPanelToControl()
                          .GetContentFromStackPanel<StackPanel>()
                              .AddNumericInfoVoteFromStats(stats.Candidates, "Candidates")
                              .AddNumericInfoVoteFromStats(stats.Parties, "Parties")
                              .AddTotalVotesInfoFromStats(stats)
                          .GetUserControlParent();
                return content;
            }
            catch
            {
                return null;
            }
        }

        private UserControl GenerateNumericStatsContent(StatsNonDb stats)
        {
            var content = new UserControl()
                .AddBasicScrollViewer()
                    .AddBasicStackPanelToControl()
                        .GetContentFromStackPanel<StackPanel>()
                            .AddNumericInfoVoteFromStats(stats.Candidates, "Candidates")
                            .AddNumericInfoVoteFromStats(stats.Parties, "Parties")
                            .AddNumericInfoBadVotesFromStats(stats)
                        .GetUserControlParent();

            return content;
        }

        private UserControl GenerateGraphStatsContent(StatsNonDb stats)
        {
            var content = new UserControl()
                .AddBasicScrollViewer()
                    .AddBasicStackPanelToControl()
                        .GetContentFromStackPanel<StackPanel>()
                            .CreateChartForItemsFromStats(
                                new List<(string, double)>(
                                    stats.Candidates.Select(p => 
                                        (p.Name, (double)p.VotesNum))
                                ), 
                                "Candidates")
                            .CreateChartForItemsFromStats(new List<(string, double)>(
                                    stats.Parties.Select(p =>
                                        (p.Name, (double)p.VotesNum))
                                ), "Parties")
                            .CreateChartForItemsFromStats(new List<(string, double)>()
                                {
                                    ("Unvalid votes", (double)stats.UnvalidVotesNun),
                                }, "")
                        .GetUserControlParent();

            return content;
        }
    }
}
