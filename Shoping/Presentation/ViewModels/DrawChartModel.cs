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

        public CartesianChart DrawMultipleColumnChartByTime(List<List<ChartItemDTO>> values, string X_Title, List<string> X_Labels, string Y_Title)
        {
            CartesianChart chart = new();

            SeriesCollection series = [];
            foreach (var productsByTime in values)
            {
                SeriesCollection section = [];
                foreach (var product in productsByTime)
                {
                    List<int> quantity = [product.Quantity];
                    section.Add(new ColumnSeries
                    {
                        Title = product.ColumnName,
                        Values = new ChartValues<int>(quantity)
                    });
                }
                foreach(var sec in section)
                {
                    series.Add(sec);
                }
            }

            chart.Series = series;

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

            SeriesCollection series = [];
            foreach (var product in values)
            {
                List<int> quantity = [product.Quantity];
                series.Add(new ColumnSeries
                {
                    Title = product.ColumnName,
                    Values = new ChartValues<int>(quantity)
                });
            }

            chart.Series = series;

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
