using ElectoralCalculator.Models.Database;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace ElectoralCalculator.Models.Stats
{
    public static class StatsViewGeneratorExtension
    {
        public static T AddBasicGridToControl<T>(this T element) where T : ContentControl
        {
            element.Content = new Grid();
            return element;
        }

        public static T AddBasicStackPanelToControl<T>(this T element) where T : ContentControl
        {
            element.Content = new StackPanel();
            return element;
        }

        public static UserControl GetUserControlParent(this object item)
        {
            var parent = ((dynamic)item).Parent;
            if ((parent is UserControl))
                return parent;
            else
                return GetUserControlParent(parent);
        }

        public static TabControl AddTabItemToTabControl(this TabControl tabControl, string header, UserControl content)
        {
            tabControl.Items.Add(new TabItem()
            {
                Header = header,
                Content = content
            });

            return tabControl;
        }

        public static TabControl AddTabControlToContent(this UserControl userControl)
        {
            var tabControl = new TabControl();
            ((Grid)userControl.Content).Children.Add(tabControl);
            return tabControl;
        }

        public static ScrollViewer AddBasicScrollViewer(this UserControl userControl)
        {
            var scrollViewer = new ScrollViewer();
            userControl.Content = scrollViewer;
            return scrollViewer;
        }

        public static T GetContentFromStackPanel<T>(this ScrollViewer scrollViewer)
        {
            if (!(scrollViewer.Content is T))
                throw new Exception($"Content is not {typeof(T)}!");
            return (T)scrollViewer.Content;
        }

        public static T AddNumericInfoBadVotesFromStats<T>(this T element, StatsNonDb stats) where T : Panel
        {
            var votesTextBlock = new TextBlock();

            votesTextBlock.Inlines.Add(new Run
            {
                Text = $"1. Bad votes: {stats.UnvalidVotesNun}",
                FontSize = 16
            });

            var stackPanel = new StackPanel()
                                    .Add<UIElementCollection>(CreateHeaderTextBlock("Bad votes", Colors.Red))
                                    .Add<UIElementCollection>(votesTextBlock);
            element.Children.Add(stackPanel);
            return element;
        }

        public static T1 AddNumericInfoVoteFromStats<T1, T2>(this T1 element, List<T2> items, string header) where T1 : Panel
        {
            var voteListTextBlock = new TextBlock();
            int itemNumber = 1;

            foreach (dynamic item in items)
            {
                voteListTextBlock.Inlines.Add(new Run()
                {
                    Text = $"{itemNumber++}. {item.Name} - {item.VotesNum} " +
                    $"{GetValidEndOfNum("vote", item.VotesNum)}{Environment.NewLine}",
                    FontSize = 16
                });
            }
            var stackPanel = new StackPanel()
                                    .Add<UIElementCollection>(CreateHeaderTextBlock(header, Colors.Red))
                                    .Add<UIElementCollection>(voteListTextBlock);

            element.Children.Add(stackPanel);
            return element;
        }

        public static T1 CreateChartForItemsFromStats<T1>(this T1 element, List<(string, double)> items, string header) where T1 : Panel
        {
            var columnChart = new CartesianChart()
            {
                LegendLocation = LegendLocation.Left,
                AxisX = new AxesCollection()
                {
                    new Axis()
                    {
                        Separator = new LiveCharts.Wpf.Separator()
                        {
                            IsEnabled = true,
                            Step = 1
                        },
                        Title = header,
                        Labels = items.Select(p => p.Item1).ToList(),
                        FontSize = 16,
                        LabelsRotation = items.Count * 13 - 13
                    }
                },
                AxisY = new AxesCollection()
                {
                    new Axis()
                    {
                        Separator = new LiveCharts.Wpf.Separator()
                        {
                            IsEnabled = true,
                            Step = 1
                        }
                    }
                },
                Series = new SeriesCollection()
                {
                    new ColumnSeries
                    {
                        Title = "Votes num",
                        Values = new ChartValues<double>(items.Select(p => p.Item2)),
                        FontSize = 16,
                        DataLabels = true,
                        LabelsPosition = BarLabelPosition.Perpendicular
                    }
                },
                Height = 400,
                BorderBrush = new SolidColorBrush(Colors.Gray),
                BorderThickness = new Thickness(1),
                Margin = new Thickness(3),
                Padding = new Thickness(3)
            };
            element.Children.Add(columnChart);

            return element;
        }

        private static TextBlock CreateHeaderTextBlock(string text, System.Windows.Media.Color color)
        {
            return new TextBlock
            {
                FontSize = 20,
                HorizontalAlignment = HorizontalAlignment.Center,
                Text = text,
                Foreground = new SolidColorBrush(color)
            };
        }

        public static T AddTotalVotesInfoFromStats<T>(this T element, StatsNonDb stats) where T : Panel
        {
            int totalVotesNum = stats.ValidVotesNum + stats.UnvalidVotesNun;
            var votesTextBlock = new TextBlock();

            votesTextBlock.Inlines.Add(new Run
            {
                Text = $"1. Good votes: {stats.ValidVotesNum} / {totalVotesNum}{Environment.NewLine}"
                    + $"2. Bad votes: {stats.UnvalidVotesNun} / {totalVotesNum}{Environment.NewLine}"
                    + $"3. Without law tries to vote: {stats.WithoutLawTries}",
                FontSize = 16
            });

            var stackPanel = new StackPanel()
                                    .Add<UIElementCollection>(CreateHeaderTextBlock("Total votes", Colors.Red))
                                    .Add<UIElementCollection>(votesTextBlock);
            element.Children.Add(stackPanel);
            return element;
        }

        public static StackPanel Add<T>(this StackPanel collection, UIElement element)
        {
            collection.Children.Add(element);
            return collection;
        }

        private static string GetValidEndOfNum(string text, int num)
        {
            return $"{text}{(num > 1 || num == 0 ? "s" : "")}";
        }
    }
}
