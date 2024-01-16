using DiscographyViewerAPI.Models.Dto;
using DiscographyViewerAPI.Models.ViewModels;
using DiscographyViewerAPI.Services;

namespace DiscographyViewerAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddScoped<IDiscographyService, DiscographyService>();
            var app = builder.Build();

            app.MapGet("/{name}", async (string name, IDiscographyService discographyClient) =>
            {
                DiscographyDto discography = await discographyClient.GetDiscographyAsync(name.ToLower());

                DiscographyViewModel result = new DiscographyViewModel()
                {
                    Album = discography.Album.Select(a => new AlbumViewModel()
                    {
                        Name = a.StrAlbum,
                        YearReleased = a.IntYearReleased,
                    }).ToList(),
                };
               
                return Results.Json(result);
            });

            app.Run();
        }
    }
}
