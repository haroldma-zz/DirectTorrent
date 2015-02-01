﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace DirectTorrent.Data.Yify.Models
{
    public interface IDataModel
    {
    }

    #region Enums
    // Enums enforcing the parameter value ranges.

    /// <summary>
    /// Represents the format of the response.
    /// </summary>
    public enum Format
    {
        JSON,
        JSONP,
        XML
    }


    /// <summary>
    /// Represents the quality of the video.
    /// </summary>
    public enum Quality
    {
        HD,
        FHD,
        ThreeD,
        ALL
    }

    /// <summary>
    /// Represents the sorting criteria by which the results will be sorted.
    /// </summary>
    public enum Sort
    {
        Title,
        Year,
        Rating,
        Peers,
        Seeds,
        DownloadCount,
        LikeCount,
        DateAdded
    }

    /// <summary>
    /// Represents the order in which the results will be sorted.
    /// </summary>
    public enum Order
    {
        Ascending,
        Descending
    }
    #endregion

    /// <summary>
    /// Represents the data associated to a torrent.
    /// </summary>
    public class Torrent : ICloneable
    {

        /// <summary>
        /// Gets the url of the torrent.
        /// </summary>
        public Uri Url { get; private set; }
        /// <summary>
        /// Gets the hash of the torrent.
        /// </summary>
        public string Hash { get; private set; }
        /// <summary>
        /// Gets the quality of the movie torrent.
        /// </summary>
        public Quality Quality { get; private set; }
        /// <summary>
        /// Gets the amount of seeds.
        /// </summary>
        public int Seeds { get; private set; }
        /// <summary>
        /// Gets the amount of peers.
        /// </summary>
        public int Peers { get; private set; }
        /// <summary>
        /// Gets the size of the movie.
        /// </summary>
        public string Size { get; private set; }
        /// <summary>
        /// Gets the size of the movie represented in bytes.
        /// </summary>
        public long SizeBytes { get; private set; }
        /// <summary>
        /// Gets the date when the torrent was uploaded.
        /// </summary>
        public DateTime DateUploaded { get; private set; }
        /// <summary>
        /// Gets the date when the torrent was uploaded as a unix timestamp.
        /// </summary>
        public int DateUploadedUnix { get; private set; }

        [JsonConstructor]
        internal Torrent(string url, string hash, string quality, int seeds, int peers, string size, long size_bytes,
            string date_uploaded, int date_uploaded_unix)
        {
            this.Url = new Uri(url);
            this.Hash = hash;
            this.Quality = ParseQuality(quality);
            this.Seeds = seeds;
            this.Peers = peers;
            this.Size = size;
            this.SizeBytes = size_bytes;
            this.DateUploaded = DateTime.Parse(date_uploaded);
            this.DateUploadedUnix = date_uploaded_unix;
        }

        // Parser method for the quality enum.
        private Quality ParseQuality(string qual)
        {
            switch (qual)
            {
                case "3D":
                    return Quality.ThreeD;
                case "1080p":
                    return  Quality.FHD;
                case "720p":
                    return Quality.HD;
            }
            throw new ArgumentOutOfRangeException("qual");
        }

        public object Clone()
        {
            Torrent temp = (Torrent)this.MemberwiseClone();
            temp.Url = new Uri(this.Url.OriginalString);

            return (object) temp;
        }
    }

    /// <summary>
    /// Represents the data associated to the movie.
    /// </summary>
    public class Movie : ICloneable
    {
        /// <summary>
        /// Gets the ID of the movie.
        /// </summary>
        public int Id { get; private set; }
        /// <summary>
        /// Gets the url of the movie.
        /// </summary>
        public Uri Url { get; private set; }
        /// <summary>
        /// Gets the imdb code of the movie.
        /// </summary>
        public string ImdbCode { get; private set; }
        /// <summary>
        /// Gets the title of movie.
        /// </summary>
        public string Title { get; private set; }
        /// <summary>
        /// Gets the long title of the movie.
        /// </summary>
        public string TitleLong { get; private set; }
        /// <summary>
        /// Gets the release year of the movie.
        /// </summary>
        public int Year { get; private set; }
        /// <summary>
        /// Gets the movie rating.
        /// </summary>
        public double Rating { get; private set; }
        /// <summary>
        /// Gets the movie runtime.
        /// </summary>
        public int Runtime { get; private set; }
        /// <summary>
        /// Gets the list of genres to which the movie belongs.
        /// </summary>
        public List<string> Genres { get; private set; }
        /// <summary>
        /// Gets the language of the movie.
        /// </summary>
        public string Language { get; private set; }
        /// <summary>
        /// Gets the mpa rating of the movie.
        /// </summary>
        public string MpaRating { get; private set; }
        public string SmallCoverImage { get; private set; }
        public string MediumCoverImage { get; private set; }
        /// <summary>
        /// Gets the current movie state.
        /// </summary>
        public string State { get; private set; }
        /// <summary>
        /// Gets the list of torrents associated to the movie.
        /// </summary>
        public List<Torrent> Torrents { get; private set; }
        /// <summary>
        /// Gets the date when the movie was uploaded.
        /// </summary>
        public DateTime DateUploaded { get; private set; }
        /// <summary>
        /// Gets the date when the movie was uploaded as a unix timestamp.
        /// </summary>
        public int DateUploadedUnix { get; private set; }

        [JsonConstructor]
        internal Movie(int id, string url, string imdb_code, string title, string title_long, int year, double rating,
            int runtime, List<string> genres, string language, string mpa_rating, string small_cover_image,
            string medium_cover_image, string state, List<Torrent> torrents, string date_uploaded,
            int date_uploaded_unix)
        {
            this.Id = id;
            this.Url = new Uri(url);
            this.ImdbCode = imdb_code;
            this.Title = title;
            this.TitleLong = title_long;
            this.Year = year;
            this.Rating = rating;
            this.Runtime = runtime;
            this.Genres = genres;
            this.Language = language;
            this.MpaRating = mpa_rating;
            // TODO: Convert to images?
            this.SmallCoverImage = small_cover_image;
            this.MediumCoverImage = medium_cover_image;
            this.State = state;
            this.Torrents = torrents.Select(x => (Torrent)x.Clone()).ToList();
            this.DateUploaded = DateTime.Parse(date_uploaded);
            this.DateUploadedUnix = date_uploaded_unix;
        }

        public object Clone()
        {
            Movie temp = (Movie)this.MemberwiseClone();
            temp.Url = new Uri(this.Url.OriginalString);
            temp.Genres = new List<string>(this.Genres);
            temp.Torrents = this.Torrents.Select(x => (Torrent) x.Clone()).ToList();
            return (object) temp;

        }
    }

    /// <summary>
    /// Represents the data returned by the list_movies API
    /// </summary>
    public class ListMoviesData : IDataModel
    {
        /// <summary>
        /// Gets the total number of movies.
        /// </summary>
        public int MovieCount { get; private set; }
        /// <summary>
        /// Gets the amount of movies limited by the query.
        /// </summary>
        public int Limit { get; private set; }
        /// <summary>
        /// Gets the page of movies queried.
        /// </summary>
        public int PageNumber { get; private set; }
        /// <summary>
        /// Gets the movies returned by the query.
        /// </summary>
        public List<Movie> Movies { get; private set; }

        [JsonConstructor]
        internal ListMoviesData(int movie_count, int limit, int page_number, List<Movie> movies)
        {
            this.MovieCount = movie_count;
            this.Limit = limit;
            this.PageNumber = page_number;
            // TODO: Deep copy
            this.Movies = movies.Select(x => (Movie)x.Clone()).ToList();
        }
    }

    public class UpcomingMoviesData : IDataModel
    {
        //._. TODO: When there's data to model
    }

    /// <summary>
    /// Represents the data associated to the director.
    /// </summary>
    public class Director : ICloneable
    {
        /// <summary>
        /// Gets the director's name.
        /// </summary>
        public string Name { get; private set; }
        public string SmallImage { get; private set; }
        public string MediumImage { get; private set; }

        [JsonConstructor]
        internal Director(string name, string small_image, string medium_image)
        {
            this.Name = name;
            // TODO: Convert to images?
            this.SmallImage = small_image;
            this.MediumImage = medium_image;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    /// <summary>
    /// Represents the the data associated to the actor.
    /// </summary>
    public class Actor : ICloneable
    {
        /// <summary>
        /// Gets the actor's name.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Gets the actor's character name.
        /// </summary>
        public string CharacterName { get; private set; }
        public string SmallImage { get; private set; }
        public string MediumImage { get; private set; }

        [JsonConstructor]
        internal Actor(string name, string character_name, string small_image, string medium_image)
        {
            this.Name = name;
            this.CharacterName = character_name;
            // TODO: Convert to images?
            this.SmallImage = small_image;
            this.MediumImage = medium_image;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    /// <summary>
    /// Represents the set of images associated to the movie.
    /// </summary>
    public class Images
    {
        public string BackgroundImage { get; private set; }
        public string SmallCoverImage { get; private set; }
        public string MediumCoverImage { get; private set; }
        public string LargeCoverImage { get; private set; }
        public string MediumScreenshotImage1 { get; private set; }
        public string MediumScreenshotImage2 { get; private set; }
        public string MediumScreenshotImage3 { get; private set; }
        public string LargeScreenshotImage1 { get; private set; }
        public string LargeScreenshotImage2 { get; private set; }
        public string LargeScreenshotImage3 { get; private set; }

        [JsonConstructor]
        internal Images(string background_image, string small_cover_image, string medium_cover_image,
            string large_cover_image, string medium_screenshot_image1, string medium_screenshot_image2,
            string medium_screenshot_image3, string large_screenshot_image1, string large_screenshot_image2,
            string large_screenshot_image3)
        {
            // TODO: Convert to images?
            this.BackgroundImage = background_image;
            this.SmallCoverImage = small_cover_image;
            this.LargeCoverImage = large_cover_image;
            this.MediumCoverImage = medium_cover_image;
            this.MediumScreenshotImage1 = medium_screenshot_image1;
            this.MediumScreenshotImage2 = medium_screenshot_image2;
            this.MediumScreenshotImage3 = medium_screenshot_image3;
            this.LargeScreenshotImage1 = large_screenshot_image1;
            this.LargeScreenshotImage2 = large_screenshot_image2;
            this.LargeScreenshotImage3 = large_screenshot_image3;
        }
    }

    /// <summary>
    /// Represents the data returned by the movie_details API
    /// </summary>
    public class MovieDetailsData : IDataModel
    {
        /// <summary>
        /// Gets the ID of the movie.
        /// </summary>
        public int Id { get; private set; }
        /// <summary>
        /// Gets the url of the movie.
        /// </summary>
        public Uri Url { get; private set; }
        /// <summary>
        /// Gets the imdb code of the movie.
        /// </summary>
        public string ImdbCode { get; private set; }
        /// <summary>
        /// Gets the title of the movie.
        /// </summary>
        public string Title { get; private set; }
        /// <summary>
        /// Gets the long title of the movie.
        /// </summary>
        public string TitleLong { get; private set; }
        /// <summary>
        /// Gets the year in which the movie was released.
        /// </summary>
        public int Year { get; private set; }
        /// <summary>
        /// Gets the rating of the movie.
        /// </summary>
        public double Rating { get; private set; }
        /// <summary>
        /// Gets the runtime of the movie.
        /// </summary>
        public int Runtime { get; private set; }
        /// <summary>
        /// Gets the genres to which the movie belongs.
        /// </summary>
        public List<string> Genres { get; private set; }
        /// <summary>
        /// Gets the language of the movie.
        /// </summary>
        public string Language { get; private set; }
        /// <summary>
        /// Gets the mpa rating of the movie.
        /// </summary>
        public string MpaRating { get; private set; }
        /// <summary>
        /// Gets the download count of the movie.
        /// </summary>
        public int DownloadCount { get; private set; }
        /// <summary>
        /// Gets the like count of the movie.
        /// </summary>
        public int LikeCount { get; private set; }
        /// <summary>
        /// Gets the RottenTomatoes critics score.
        /// </summary>
        public int RtCriticsScore { get; private set; }
        /// <summary>
        /// Gets the RottenTomatoes critics rating.
        /// </summary>
        public string RtCriticsRating { get; private set; }
        /// <summary>
        /// Gets the RottenTomatoes audience score.
        /// </summary>
        public int RtAudienceScore { get; private set; }
        /// <summary>
        /// Gets the RottenTomatoes audience rating.
        /// </summary>
        public string RtAudienceRating { get; private set; }
        /// <summary>
        /// Gets the abstract of the description.
        /// </summary>
        public string DescriptionIntro { get; private set; }
        /// <summary>
        /// Gets the full description of the movie.
        /// </summary>
        public string DescriptionFull { get; private set; }
        /// <summary>
        /// Gets the YouTube trailer code of the movie.
        /// </summary>
        public string YtTrailerCode { get; private set; }
        /// <summary>
        /// Gets the date when the movie was uploaded.
        /// </summary>
        public DateTime DateUploaded { get; private set; }
        /// <summary>
        /// Gets the date when the movie was uploaded as a unix timestamp.
        /// </summary>
        public int DateUploadedUnix { get; private set; }
        /// <summary>
        /// Gets the set of images associated to the movie.
        /// </summary>
        public Images Images { get; private set; }
        /// <summary>
        /// Gets the list of torrents associated to the movie.
        /// </summary>
        public List<Torrent> Torrents { get; private set; }
        /// <summary>
        /// Gets the list of directors associated to the movie.
        /// </summary>
        public List<Director> Directors { get; private set; }
        /// <summary>
        /// Gets the list of actors associated to the movie.
        /// </summary>
        public List<Actor> Actors { get; private set; }

        [JsonConstructor]
        internal MovieDetailsData(int id, string url, string imdb_code, string title, string title_long, int year,
            double rating, int runtime, List<string> genres, string language, string mpa_rating, int download_count,
            int like_count, int rt_critics_score, string rt_critics_rating, int rt_audience_score,
            string rt_audience_rating, string description_intro, string description_full, string yt_trailer_code,
            string date_uploaded, int date_uploaded_unix, Images images, List<Torrent> torrents,
            List<Director> directors, List<Actor> actors)
        {
            this.Id = id;
            this.Url = new Uri(url);
            this.ImdbCode = imdb_code;
            this.Title = title;
            this.TitleLong = title_long;
            this.Year = year;
            this.Rating = rating;
            this.Runtime = runtime;
            this.Genres = new List<string>(genres);
            this.Language = language;
            this.MpaRating = mpa_rating;
            this.DownloadCount = download_count;
            this.LikeCount = like_count;
            this.RtCriticsScore = rt_critics_score;
            this.RtCriticsRating = rt_critics_rating;
            this.RtAudienceScore = rt_audience_score;
            this.RtAudienceRating = rt_audience_rating;
            this.DescriptionIntro = description_intro;
            this.DescriptionFull = description_full;
            this.YtTrailerCode = yt_trailer_code;
            this.DateUploaded = DateTime.Parse(date_uploaded);
            this.DateUploadedUnix = date_uploaded_unix;
            this.Images = images;
            this.Torrents = torrents.Select(x => (Torrent)x.Clone()).ToList();
            this.Directors = directors;
            this.Actors = actors;
        }
    }
}