using LiveCharts;
using LiveCharts.Wpf;
using Shoping.Data_Access.DTOs;
using System.Windows.Controls.DataVisualization;

namespace Shoping.Presentation.ViewModels
{
    public class DrawChartModel
    {
        public CartesianChart DrawDoubleLineChartByTime(List<int> values1, List<int> values2, string title1, string title2, string X_Title, List<string> X_Labels, string Y_Title)
        {
            CartesianChart chart = new();
            chart.Series = [
                new LineSeries
                {
                    Title = title1,
                    Values = new ChartValues<int>(values1),
                },
                new LineSeries
                {
                    Title = title2,
                    Values = new ChartValues<int>(values2),
                }
            ];

            int max = values1.Count != 0 ? values1.Max() : 0;
            int min = values2.Count != 0 ? values2.Min() : 0;

            if(max != min)
            {
                chart.AxisY.Add(new Axis()
                {
                    Title = Y_Title,
                    MaxValue = max,
                    MinValue = min,
                    Separator = new Separator() { Step = (max - min) / 10},
                });
            } else if(max > 0)
            {
                chart.AxisY.Add(new Axis()
                {
                    Title = Y_Title,
                    MinValue = 0,
                    Separator = new Separator() { Step = max / 10 },
                });
            } else if(max < 0)
            {
                chart.AxisY.Add(new Axis()
                {
                    Title = Y_Title,
                    MaxValue = 0,
                    Separator = new Separator() { Step = max / 10 },
                });
            }else
            {
                chart.AxisY.Add(new Axis()
                {
                    Title = Y_Title,
                    MinValue = 0,
                    MaxValue = 10,
                });
            }

            chart.AxisX.Add(new Axis()
            {
                Title = X_Title,
                Labels = X_Labels,
                Separator = new Separator { Step = 1 },
            });

            return chart;
        }

        public CartesianChart DrawStackedColumnChartByTime(List<List<ChartItemDTO>> values, string X_Title, List<string> X_Labels, string Y_Title)
        {
            CartesianChart chart = new();

            List<(int, ChartItemDTO)> all_datas = [];
            foreach(var item in values)
            {
                foreach(var value in item)
                {
                    all_datas.Add((values.IndexOf(item), value));
                }
            }
            if (all_datas.Count() == 0)
            {
                chart.Series.Add(new StackedColumnSeries()
                {
                    Values = new ChartValues<int>(Enumerable.Repeat(0, X_Labels.Count()).ToList()),
                });
            }

            var datasByTime = all_datas.GroupBy(x => x.Item2.ColumnName);
            foreach(var data in datasByTime)
            {
                List<int> columnValues = Enumerable.Repeat(0, X_Labels.Count()).ToList();
                foreach(var column in data)
                {
                    columnValues[column.Item1] = column.Item2.Quantity;
                }
                if(columnValues.All(x => x == 0))
                {
                    continue;
                }
                chart.Series.Add(new StackedColumnSeries()
                {
                    Title = data.Key,
                    Values = new ChartValues<int>(columnValues),
                });
            }

            if (values.Count == 0)
            {
                chart.AxisY.Add(new Axis()
                {
                    Title = Y_Title,
                    MaxValue = 10,
                    MinValue = 0,
                });
            } else
            {
                chart.AxisY.Add(new Axis()
                {
                    Title = Y_Title,
                    MinValue = 0,
                });
            }

            chart.AxisX.Add(new Axis()
            {
                Title = X_Title,
                Labels = X_Labels,
                Separator = new Separator { Step = 1 },
            });

            return chart;
        }
        public CartesianChart DrawColumnChartByTime(List<ChartItemDTO> values, string X_Title, List<string> X_Labels, string Y_Title)
        {
            CartesianChart chart = new();

            foreach(var item in values)
            {
                List<int> quantities = Enumerable.Repeat(0, X_Labels.Count()).ToList();
                quantities[values.IndexOf(item)] = item.Quantity;
                if(quantities.All(x => x == 0))
                {
                    continue;
                }
                chart.Series.Add(new StackedColumnSeries
                {
                    Title = item.ColumnName,
                    Values = new ChartValues<int>(quantities)
                });
            }

            if (values.Count == 0)
            {
                chart.AxisY.Add(new Axis()
                {
                    Title = Y_Title,
                    MaxValue = 10,
                    MinValue = 0,
                });
            } else
            {
                chart.AxisY.Add(new Axis()
                {
                    Title = Y_Title,
                    MinValue = 0,
                });
            }

            chart.AxisX.Add(new Axis()
            {
                Title = X_Title,
                Labels = X_Labels,
                Separator = new Separator { Step = 1 },
            });

            return chart;
        }
    }
}
