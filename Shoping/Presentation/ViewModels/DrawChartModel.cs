﻿using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.VisualBasic;
using Shoping.Data_Access.DTOs;

namespace Shoping.Presentation.ViewModels
{
    public class DrawChartModel
    {
        public CartesianChart DrawDoubleLineChartByTime(List<int> values1, List<int> values2, string title1, string title2, string X_Title, List<string> X_Labels)
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
                    MaxValue = max,
                    MinValue = min,
                });
            } else
            {
                chart.AxisY.Add(new Axis()
                {
                    MaxValue = max,
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

        public CartesianChart DrawColumnChartByTime(List<List<ChartItemDTO>> values, string title, string X_Title, List<string> X_Labels, CartesianChart MyChart)
        {
            CartesianChart chart = new()
            {
                Series = [
                    new ColumnSeries
                    {
                        Title = title,
                        Values = new ChartValues<List<ChartItemDTO>>(values),
                    }
                ]
            };

            if (values.Count == 0)
            {
                chart.AxisY.Add(new Axis()
                {
                    MaxValue = 10,
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
