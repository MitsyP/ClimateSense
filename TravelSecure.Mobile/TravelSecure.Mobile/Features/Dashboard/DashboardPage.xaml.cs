using System.Text.Json;

namespace TravelSecure.Mobile.Features.Dashboard;

public partial class DashboardPage : ContentPage
{
    private const string WeatherApiKey = "cf99dc2801a290f703d3c742963c1d14";
    private const string WeatherUrl = "https://api.openweathermap.org/data/2.5/weather";

    public DashboardPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarClima();
        await CargarAlertasRecientes();
    }

    private async Task CargarClima()
    {
        try
        {
            var location = await Geolocation.GetLastKnownLocationAsync();
            if (location == null) throw new Exception("Sin ubicación");

            using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(8) };
            var url = $"{WeatherUrl}?lat={location.Latitude}&lon={location.Longitude}" +
                      $"&appid={WeatherApiKey}&units=metric&lang=es";

            var response = await client.GetStringAsync(url);
            var data = JsonDocument.Parse(response);

            var temp = data.RootElement.GetProperty("main").GetProperty("temp").GetDouble();
            var desc = data.RootElement.GetProperty("weather")[0]
                           .GetProperty("description").GetString() ?? "despejado";

            LblTempClima.Text = $"{temp:F0}°";
            LblDescClima.Text = System.Globalization.CultureInfo.CurrentCulture
                                      .TextInfo.ToTitleCase(desc);

            var cond = desc.ToLower();
            if (cond.Contains("lluvia") || cond.Contains("tormenta") || cond.Contains("niebla"))
            {
                LblEstadoRuta.Text = "⚠️ Precaución en ruta";
                LblEstadoRuta.TextColor = Color.FromArgb("#F59E0B");
            }
            else
            {
                LblEstadoRuta.Text = "✅ Ruta Segura";
                LblEstadoRuta.TextColor = Color.FromArgb("#22C55E");
            }
        }
        catch
        {
            LblTempClima.Text = "19°";
            LblDescClima.Text = "Parcialmente nublado";
            LblEstadoRuta.Text = "✅ Ruta Segura";
            LblEstadoRuta.TextColor = Color.FromArgb("#22C55E");
        }
    }

    private async Task CargarAlertasRecientes()
    {
        await Task.Delay(900);

        LoadingAlertas.IsRunning = false;
        LoadingAlertas.IsVisible = false;

        var alertas = new[]
        {
            new { Icono = "🚗", Color = "#EF4444", Tipo = "Accidente registrado",
                  Lugar = "Carretera Central, KM 124", Tiempo = "Hace 3 min", Nivel = "ALTO" },
            new { Icono = "❄️", Color = "#3B82F6", Tipo = "Clima extremo",
                  Lugar = "Paso Ticlio – Carretera Central", Tiempo = "Hace 11 min", Nivel = "MEDIO" },
            new { Icono = "🚧", Color = "#F59E0B", Tipo = "Obras en la vía",
                  Lugar = "Av. Javier Prado Este, cuadra 18", Tiempo = "Hace 38 min", Nivel = "BAJO" },
            new { Icono = "⛰️", Color = "#8B5CF6", Tipo = "Derrumbe parcial",
                  Lugar = "Variante de Pasamayo", Tiempo = "Hace 1 h", Nivel = "ALTO" },
        };

        foreach (var alerta in alertas)
        {
            var fila = new Border
            {
                BackgroundColor = Color.FromArgb("#111827"),
                StrokeThickness = 0,
                Padding = new Thickness(16, 12),
                Margin = new Thickness(4, 2),
            };
            fila.StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle
            { CornerRadius = new CornerRadius(12) };

            var grid = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = 44 },
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto }
                }
            };

            var iconoBorder = new Border
            {
                BackgroundColor = Color.FromArgb(alerta.Color + "22"),
                WidthRequest = 38,
                HeightRequest = 38,
                StrokeThickness = 0,
                VerticalOptions = LayoutOptions.Center
            };
            iconoBorder.StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle
            { CornerRadius = new CornerRadius(10) };
            iconoBorder.Content = new Label
            {
                Text = alerta.Icono,
                FontSize = 18,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            var info = new VerticalStackLayout { Spacing = 2, VerticalOptions = LayoutOptions.Center };
            info.Add(new Label
            {
                Text = alerta.Tipo,
                FontSize = 13,
                TextColor = Colors.White,
                FontAttributes = FontAttributes.Bold
            });
            info.Add(new Label
            {
                Text = alerta.Lugar,
                FontSize = 11,
                TextColor = Color.FromArgb("#6B7280")
            });

            var right = new VerticalStackLayout
            {
                Spacing = 4,
                HorizontalOptions = LayoutOptions.End,   // ← sin EndAndExpand
                VerticalOptions = LayoutOptions.Center
            };
            var nivelColor = alerta.Nivel == "ALTO" ? "#EF4444"
                           : alerta.Nivel == "MEDIO" ? "#F59E0B" : "#22C55E";
            var badge = new Border
            {
                BackgroundColor = Color.FromArgb(alerta.Color + "22"),
                Padding = new Thickness(6, 2),
                StrokeThickness = 0
            };
            badge.StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle
            { CornerRadius = new CornerRadius(4) };
            badge.Content = new Label
            {
                Text = alerta.Nivel,
                FontSize = 9,
                TextColor = Color.FromArgb(alerta.Color),
                FontAttributes = FontAttributes.Bold
            };
            right.Add(badge);
            right.Add(new Label
            {
                Text = alerta.Tiempo,
                FontSize = 10,
                TextColor = Color.FromArgb("#374151"),
                HorizontalOptions = LayoutOptions.End
            });

            Grid.SetColumn(iconoBorder, 0);
            Grid.SetColumn(info, 1);
            Grid.SetColumn(right, 2);
            grid.Add(iconoBorder);
            grid.Add(info);
            grid.Add(right);

            fila.Content = grid;
            ListaAlertas.Add(fila);
        }
    }

    private async void OnBuscarRuta(object sender, EventArgs e)
    {
        await DisplayAlertAsync("Próximamente 🗺️",
            "La búsqueda de rutas estará disponible en el siguiente avance.", "Entendido");
    }

    private async void OnNavReporte(object sender, EventArgs e)
        => await Shell.Current.GoToAsync("//main/incidents/ReportePage");
    private async void OnNavAlertas(object sender, EventArgs e)
        => await Shell.Current.GoToAsync("//main/alerts/AlertsPage");
    private async void OnNavPerfil(object sender, EventArgs e)
        => await Shell.Current.GoToAsync("//main/profile/ProfilePage");
}
