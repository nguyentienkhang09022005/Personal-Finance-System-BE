using System.Text.Json.Serialization;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response
{
    public class CnbcTrendingNewsResponse
    {
        [JsonPropertyName("data")]
        public CnbcData? Data { get; set; }
    }

    public class CnbcData
    {
        [JsonPropertyName("mostPopularEntries")]
        public CnbcMostPopular? MostPopularEntries { get; set; }
    }

    public class CnbcMostPopular
    {
        [JsonPropertyName("__typename")]
        public string? Typename { get; set; }

        [JsonPropertyName("assets")]
        public List<CnbcNewsStory>? Assets { get; set; }
    }

    public class CnbcNewsStory
    {
        [JsonPropertyName("__typename")]
        public string? Typename { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("headline")]
        public string? Headline { get; set; }

        [JsonPropertyName("shorterHeadline")]
        public string? ShorterHeadline { get; set; }

        [JsonPropertyName("dateLastPublished")]
        public string? DateLastPublished { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("pageName")]
        public string? PageName { get; set; }

        [JsonPropertyName("relatedTagsFilteredFormatted")]
        public string? RelatedTagsFilteredFormatted { get; set; }

        [JsonPropertyName("dateFirstPublished")]
        public string? DateFirstPublished { get; set; }

        [JsonPropertyName("sectionHierarchyFormatted")]
        public string? SectionHierarchyFormatted { get; set; }

        [JsonPropertyName("authorFormatted")]
        public string? AuthorFormatted { get; set; }

        [JsonPropertyName("shortDateFirstPublished")]
        public string? ShortDateFirstPublished { get; set; }

        [JsonPropertyName("shortDateLastPublished")]
        public string? ShortDateLastPublished { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("premium")]
        public bool Premium { get; set; }

        [JsonPropertyName("promoImage")]
        public PromoImage? PromoImage { get; set; }

        [JsonPropertyName("featuredMedia")]
        public FeaturedMedia? FeaturedMedia { get; set; }

        [JsonPropertyName("section")]
        public Section? Section { get; set; }
    }

    public class PromoImage
    {
        [JsonPropertyName("__typename")]
        public string? Typename { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }
    }

    public class FeaturedMedia
    {
        [JsonPropertyName("__typename")]
        public string? Typename { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("duration")]
        public int? Duration { get; set; }

        [JsonPropertyName("playbackURL")]
        public string? PlaybackURL { get; set; }

        [JsonPropertyName("vcpsId")]
        public long? VcpsId { get; set; }
    }

    public class Section
    {
        [JsonPropertyName("__typename")]
        public string? Typename { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("shortestHeadline")]
        public string? ShortestHeadline { get; set; }

        [JsonPropertyName("tagName")]
        public string? TagName { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("premium")]
        public bool Premium { get; set; }
    }
}
