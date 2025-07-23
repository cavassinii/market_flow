using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DTO.Mercado_Livre
{
    public class MlCategoryRootResponse : Dictionary<string, MlCategory> { }

    public class MlCategory
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = null!;

        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;

        [JsonPropertyName("picture")]
        public string? Picture { get; set; }

        [JsonPropertyName("permalink")]
        public string? Permalink { get; set; }

        [JsonPropertyName("total_items_in_this_category")]
        public int? TotalItemsInThisCategory { get; set; }

        [JsonPropertyName("path_from_root")]
        public List<PathFromRootItem> PathFromRoot { get; set; } = new();

        [JsonPropertyName("children_categories")]
        public List<ChildCategory> ChildrenCategories { get; set; } = new();

        [JsonPropertyName("attribute_types")]
        public string? AttributeTypes { get; set; }

        [JsonPropertyName("settings")]
        public Settings? Settings { get; set; }

        [JsonPropertyName("channels_settings")]
        public List<ChannelSetting> ChannelsSettings { get; set; } = new();

        [JsonPropertyName("meta_categ_id")]
        public string? MetaCategId { get; set; }

        [JsonPropertyName("attributable")]
        public bool? Attributable { get; set; }

        [JsonPropertyName("date_created")]
        public DateTime? DateCreated { get; set; }
    }

    public class PathFromRootItem
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = null!;

        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;
    }

    public class ChildCategory
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = null!;

        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;

        [JsonPropertyName("total_items_in_this_category")]
        public int? TotalItemsInThisCategory { get; set; }
    }

    public class Settings
    {
        [JsonPropertyName("adult_content")]
        public bool? AdultContent { get; set; }

        [JsonPropertyName("buying_allowed")]
        public bool? BuyingAllowed { get; set; }

        [JsonPropertyName("buying_modes")]
        public List<string> BuyingModes { get; set; } = new();

        [JsonPropertyName("catalog_domain")]
        public string? CatalogDomain { get; set; }

        [JsonPropertyName("coverage_areas")]
        public string? CoverageAreas { get; set; }

        [JsonPropertyName("currencies")]
        public List<string> Currencies { get; set; } = new();

        [JsonPropertyName("fragile")]
        public bool? Fragile { get; set; }

        [JsonPropertyName("immediate_payment")]
        public string? ImmediatePayment { get; set; }

        [JsonPropertyName("item_conditions")]
        public List<string> ItemConditions { get; set; } = new();

        [JsonPropertyName("items_reviews_allowed")]
        public bool? ItemsReviewsAllowed { get; set; }

        [JsonPropertyName("listing_allowed")]
        public bool? ListingAllowed { get; set; }

        [JsonPropertyName("max_description_length")]
        public int? MaxDescriptionLength { get; set; }

        [JsonPropertyName("max_pictures_per_item")]
        public int? MaxPicturesPerItem { get; set; }

        [JsonPropertyName("max_pictures_per_item_var")]
        public int? MaxPicturesPerItemVar { get; set; }

        [JsonPropertyName("max_sub_title_length")]
        public int? MaxSubTitleLength { get; set; }

        [JsonPropertyName("max_title_length")]
        public int? MaxTitleLength { get; set; }

        [JsonPropertyName("max_variations_allowed")]
        public int? MaxVariationsAllowed { get; set; }

        [JsonPropertyName("maximum_price")]
        public decimal? MaximumPrice { get; set; }

        [JsonPropertyName("maximum_price_currency")]
        public string? MaximumPriceCurrency { get; set; }

        [JsonPropertyName("minimum_price")]
        public decimal? MinimumPrice { get; set; }

        [JsonPropertyName("minimum_price_currency")]
        public string? MinimumPriceCurrency { get; set; }

        [JsonPropertyName("mirror_category")]
        public string? MirrorCategory { get; set; }

        [JsonPropertyName("mirror_master_category")]
        public string? MirrorMasterCategory { get; set; }

        [JsonPropertyName("mirror_slave_categories")]
        public List<object> MirrorSlaveCategories { get; set; } = new();

        [JsonPropertyName("price")]
        public string? Price { get; set; }

        [JsonPropertyName("reservation_allowed")]
        public string? ReservationAllowed { get; set; }

        [JsonPropertyName("restrictions")]
        public List<object> Restrictions { get; set; } = new();

        [JsonPropertyName("rounded_address")]
        public bool? RoundedAddress { get; set; }

        [JsonPropertyName("seller_contact")]
        public string? SellerContact { get; set; }

        [JsonPropertyName("shipping_options")]
        public List<string> ShippingOptions { get; set; } = new();

        [JsonPropertyName("shipping_profile")]
        public string? ShippingProfile { get; set; }

        [JsonPropertyName("show_contact_information")]
        public bool? ShowContactInformation { get; set; }

        [JsonPropertyName("simple_shipping")]
        public string? SimpleShipping { get; set; }

        [JsonPropertyName("stock")]
        public string? Stock { get; set; }

        [JsonPropertyName("sub_vertical")]
        public string? SubVertical { get; set; }

        [JsonPropertyName("subscribable")]
        public bool? Subscribable { get; set; }

        [JsonPropertyName("tags")]
        public List<object> Tags { get; set; } = new();

        [JsonPropertyName("vertical")]
        public string? Vertical { get; set; }

        [JsonPropertyName("vip_subdomain")]
        public string? VipSubdomain { get; set; }

        [JsonPropertyName("buyer_protection_programs")]
        public List<string> BuyerProtectionPrograms { get; set; } = new();

        [JsonPropertyName("status")]
        public string? Status { get; set; }
    }

    public class ChannelSetting
    {
        [JsonPropertyName("channel")]
        public string Channel { get; set; } = null!;

        [JsonPropertyName("settings")]
        public Dictionary<string, object> Settings { get; set; } = new();
    }
}
