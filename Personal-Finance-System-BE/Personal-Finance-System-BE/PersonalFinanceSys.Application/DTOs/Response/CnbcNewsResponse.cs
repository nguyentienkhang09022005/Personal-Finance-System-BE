using System.Text.Json.Serialization;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response
{
    public class CnbcNewsResponse
    {
        [JsonPropertyName("data")]
        public CnbcSectionsData? Data { get; set; }
    }

    public class CnbcSectionsData
    {
        [JsonPropertyName("sectionsEntries")]
        public List<CnbcSectionEntry>? SectionsEntries { get; set; }
    }

    public class CnbcSectionEntry
    {
        [JsonPropertyName("__typename")]
        public string? Typename { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("subType")]
        public string? SubType { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("headline")]
        public string? Headline { get; set; }

        [JsonPropertyName("shortestHeadline")]
        public string? ShortestHeadline { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("datePublished")]
        public string? DatePublished { get; set; }

        [JsonPropertyName("dateFirstPublished")]
        public string? DateFirstPublished { get; set; }

        [JsonPropertyName("dateLastPublished")]
        public string? DateLastPublished { get; set; }

        [JsonPropertyName("premium")]
        public bool Premium { get; set; }

        [JsonPropertyName("projectTeamContentFormatted")]
        public string? ProjectTeamContentFormatted { get; set; }

        [JsonPropertyName("sectionHierarchyFormatted")]
        public string? SectionHierarchyFormatted { get; set; }

        [JsonPropertyName("additionalSectionContentFormatted")]
        public string? AdditionalSectionContentFormatted { get; set; }

        [JsonPropertyName("sourceOrganizationFormatted")]
        public string? SourceOrganizationFormatted { get; set; }

        [JsonPropertyName("relatedTagsFilteredFormatted")]
        public string? RelatedTagsFilteredFormatted { get; set; }

        [JsonPropertyName("hier1Formatted")]
        public string? Hier1Formatted { get; set; }

        [JsonPropertyName("tagName")]
        public string? TagName { get; set; }

        [JsonPropertyName("tagNameFormattedFull")]
        public string? TagNameFormattedFull { get; set; }

        [JsonPropertyName("usageRule")]
        public string? UsageRule { get; set; }

        [JsonPropertyName("dateline")]
        public bool? Dateline { get; set; }

        [JsonPropertyName("section")]
        public CnbcSubSection? Section { get; set; }

        [JsonPropertyName("assets")]
        public List<CnbcSectionAsset>? Assets { get; set; }
    }

    public class CnbcSectionAsset
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
        public CnbcPromoImageResponse? PromoImage { get; set; }

        [JsonPropertyName("section")]
        public CnbcSubSection? Section { get; set; } 

        [JsonPropertyName("featuredMedia")]
        public CnbcFeaturedMediaResponse? FeaturedMedia { get; set; }
    }

    public class CnbcSubSection
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

    public class CnbcPromoImageResponse
    {
        [JsonPropertyName("__typename")]
        public string? Typename { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }
    }

    public class CnbcFeaturedMediaResponse
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
