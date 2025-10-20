using System.Text.Json.Serialization;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response
{
    public class CnbcSpecialReportsResponse
    {
        [JsonPropertyName("data")]
        public CnbcSpecialData? Data { get; set; }
    }

    public class CnbcSpecialData
    {
        [JsonPropertyName("specialReportsEntries")]
        public CnbcSpecialReportsEntries? SpecialReportsEntries { get; set; }
    }

    public class CnbcSpecialReportsEntries
    {
        [JsonPropertyName("__typename")]
        public string? Typename { get; set; }

        [JsonPropertyName("results")]
        public List<CnbcSpecialReportItem>? Results { get; set; }
    }

    public class CnbcSpecialReportItem
    {
        [JsonPropertyName("__typename")]
        public string? Typename { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("headline")]
        public string? Headline { get; set; }

        [JsonPropertyName("shorterHeadline")]
        public string? ShorterHeadline { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("pageName")]
        public string? PageName { get; set; }

        [JsonPropertyName("relatedTagsFilteredFormatted")]
        public string? RelatedTagsFilteredFormatted { get; set; }

        [JsonPropertyName("dateFirstPublished")]
        public string? DateFirstPublished { get; set; }

        [JsonPropertyName("dateLastPublished")]
        public string? DateLastPublished { get; set; }

        [JsonPropertyName("shortDateFirstPublished")]
        public string? ShortDateFirstPublished { get; set; }

        [JsonPropertyName("shortDateLastPublished")]
        public string? ShortDateLastPublished { get; set; }

        [JsonPropertyName("sectionHierarchyFormatted")]
        public string? SectionHierarchyFormatted { get; set; }

        [JsonPropertyName("authorFormatted")]
        public string? AuthorFormatted { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("premium")]
        public bool Premium { get; set; }

        [JsonPropertyName("promoImage")]
        public CnbcPromoImage? PromoImage { get; set; }

        [JsonPropertyName("section")]
        public CnbcSection? Section { get; set; }

        [JsonPropertyName("featuredMedia")]
        public CnbcFeaturedMedia? FeaturedMedia { get; set; }

        [JsonPropertyName("duration")]
        public int? Duration { get; set; }

        [JsonPropertyName("playbackURL")]
        public string? PlaybackURL { get; set; }

        [JsonPropertyName("vcpsId")]
        public long? VcpsId { get; set; }
    }

    public class CnbcPromoImage
    {
        [JsonPropertyName("__typename")]
        public string? Typename { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }
    }

    public class CnbcSection
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

    public class CnbcFeaturedMedia
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
}
