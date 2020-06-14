using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.IO;

namespace DanbooruDBProvider
{
    public class SQLiteProvider
    {
        private readonly string connectionString;

        public SQLiteProvider(string newDbPath)
        {
            if (string.IsNullOrWhiteSpace(newDbPath))
            {
                newDbPath = "sqllite.db";
            }
            this.connectionString = "Version=3;cache=shared;Data Source=" + newDbPath;

            if (!File.Exists(newDbPath))
            {
                File.WriteAllBytes(newDbPath, new byte[0]);
            }
        }

        private readonly string CREATE_SQL = @"
CREATE TABLE IF NOT EXISTS `downloaded_files` (
    `id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
    `provider_name` TEXT NOT NULL,
    `post_id` TEXT NOT NULL,
    `filename` TEXT NOT NULL,
    `path` TEXT NOT NULL,
    `download_timestamp` TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP
)
";

        public bool Create()
        {
            var result = -1;
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                var cmd = connection.CreateCommand();
                cmd.CommandText = CREATE_SQL;
                result = cmd.ExecuteNonQuery();
            }

            return result > -1 ? true : false;
        }

        private readonly string INSERT_SQL = @"
INSERT INTO downloaded_files (provider_name, post_id, filename, path)
VALUES (@provider_name, @post_id, @filename, @path)
";

        public bool Insert(string providerName, string postId, string filename, string path)
        {
            var result = -1;
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                var cmd = connection.CreateCommand();
                cmd.CommandText = INSERT_SQL;

                cmd.Parameters.Add(new SQLiteParameter("@provider_name", providerName));
                cmd.Parameters.Add(new SQLiteParameter("@post_id", postId));
                cmd.Parameters.Add(new SQLiteParameter("@filename", filename));
                cmd.Parameters.Add(new SQLiteParameter("@path", path));
                result = cmd.ExecuteNonQuery();
            }

            return result > -1 ? true : false;
        }

        private readonly string SELECT_SQL = @"
SELECT * FROM downloaded_files
WHERE provider_name = @provider_name and post_id = @post_id
";

        public List<DownloadedFile> GetDownloadedFileByProviderAndPostId(string providerName, string postId)
        {
            var result = new List<DownloadedFile>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                var cmd = connection.CreateCommand();
                cmd.CommandText = SELECT_SQL;

                cmd.Parameters.Add(new SQLiteParameter("@provider_name", providerName));
                cmd.Parameters.Add(new SQLiteParameter("@post_id", postId));
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var downloadedFile = new DownloadedFile()
                    {
                        Id = reader.GetInt32(0),
                        ProviderName = reader.GetString(1),
                        PostId = reader.GetString(2),
                        Filename = reader.GetString(3),
                        Path = reader.GetString(4),
                        Timestamp = DateTime.ParseExact(reader.GetString(5), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                    };
                    result.Add(downloadedFile);
                }
            }
            return result;
        }
    }
}