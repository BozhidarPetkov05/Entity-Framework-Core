namespace MusicHub
{
    using System;
    using System.Data.SqlTypes;
    using System.Text;
    using Castle.DynamicProxy.Generators;
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            //Test your solutions here

            Console.WriteLine(ExportAlbumsInfo(context, 9));
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albums = context.Albums
                .AsEnumerable()
                .Where(a => a.ProducerId == producerId)
                .Select(a => new
                {
                    a.ProducerId,
                    a.Price,
                    a.Songs,
                    a.Name,
                    ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy"),
                    ProducerName = a.Producer.Name,

                })
                .OrderByDescending(a => a.Price)
                .ToList();

           

            StringBuilder sb = new StringBuilder();

            foreach (var album in albums)
            {
                var songs = album.Songs
                    .OrderByDescending(s => s.Name)
                    .ThenBy(s => s.Writer)
                    .ToList();

                sb.AppendLine($"-AlbumName: {album.Name}");
                sb.AppendLine($"-ReleaseDate: {album.ReleaseDate}");
                sb.AppendLine($"-ProducerName: {album.ProducerName}");
                sb.AppendLine($"-Songs:");

                int songCount = 0;
                foreach (var song in songs)
                {
                    songCount++;
                    
                    sb.AppendLine($"---#{songCount}");
                    sb.AppendLine($"---SongName: {song.Name}");
                    sb.AppendLine($"---Price: {song.Price:f2}");
                    sb.AppendLine($"---Writer: {song.Writer.Name}");
                }

                sb.AppendLine($"-AlbumPrice: {album.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songs = context.Songs
                .AsEnumerable()
                .Where(s => s.Duration.TotalSeconds > duration)
                .Select(s => new
                {
                    SongName = s.Name,
                    PeformersNames = s.SongPerformers
                        .Select(sp => $"{sp.Performer.FirstName} {sp.Performer.LastName}")
                        .OrderBy(n => n).ToList(),
                    WriterName = s.Writer.Name,
                    AlbumProducerName = s.Album.Producer.Name,
                    Duration = s.Duration.ToString("c")
                })
                .OrderBy(s => s.SongName)
                .ThenBy(s => s.WriterName)
                .ToList();

            StringBuilder sb = new StringBuilder();

            int songCount = 0;

            foreach (var song in songs)
            {
                songCount++;
                
                sb.AppendLine($"-Song #{songCount}");
                sb.AppendLine($"---SongName: {song.SongName}");
                sb.AppendLine($"---Writer: {song.WriterName}");

                if (song.PeformersNames.Any())
                {
                    foreach (var performer in song.PeformersNames)
                    {
                        sb.AppendLine($"---Performer: {performer}");
                    }
                }

                sb.AppendLine($"---AlbumProducer: {song.AlbumProducerName}");
                sb.AppendLine($"---Duration: {song.Duration}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
