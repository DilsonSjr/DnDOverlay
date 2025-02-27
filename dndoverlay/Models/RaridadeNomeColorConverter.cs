using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DnDOverlay.Models
{
    public class RaridadeNomeColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string raridade = value as string;

            // Verifica se o valor é nulo ou vazio
            if (string.IsNullOrEmpty(raridade))
                return new LinearGradientBrush(
                    new GradientStopCollection
                    {
                        new GradientStop(Color.FromRgb(20, 20, 20), 0),
                        new GradientStop(Color.FromRgb(29, 29, 29), 1)
                    });

            switch (raridade.ToLower())
            {
                case "comum":
                    return new LinearGradientBrush(
                        new GradientStopCollection
                        {
                            new GradientStop(Color.FromRgb(20, 20, 20), 0),
                            new GradientStop(Color.FromRgb(29, 29, 29), 0.5)
                        });
                case "incomum":
                    return new LinearGradientBrush(
                        new GradientStopCollection
                        {
                            new GradientStop(Color.FromRgb(29, 29, 29), 0),
                            new GradientStop(Color.FromRgb(39, 47, 39), 0.5),
                        });
                case "raro":
                    return new LinearGradientBrush(
                        new GradientStopCollection
                        {
                            new GradientStop(Color.FromRgb(29, 29, 29), 0),
                            new GradientStop(Color.FromRgb(28, 37, 49), 0.5),
                        });
                case "epico":
                    return new LinearGradientBrush(
                        new GradientStopCollection
                        {
                            new GradientStop(Color.FromRgb(29, 29, 29), 0),
                            new GradientStop(Color.FromRgb(66, 33, 72), 0.5),
                        });
                case "lendario":
                    return new LinearGradientBrush(
                        new GradientStopCollection
                        {
                            new GradientStop(Color.FromRgb(29, 29, 29), 0),
                            new GradientStop(Color.FromRgb(97, 74, 26), 0.5),
                        });
                default:
                    return new LinearGradientBrush(
                        new GradientStopCollection
                        {
                            new GradientStop(Color.FromRgb(29, 29, 29), 0),
                            new GradientStop(Colors.Gray, 1)
                        });
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null; // Não precisa de implementação para conversão reversa
        }
    }
}
