using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCGAssets.Models.Yugioh
{
    public class Card
    {
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string desc { get; set; }
        public string race { get; set; }
        public string archetype { get; set; }
        public IEnumerable<CardSet> card_sets { get; set; }
        public IEnumerable<CardImage> card_images { get; set; }
        public CardPrice card_prices { get; set; }
    }

    public class CardSet
    {
        public string set_name { get; set; }
        public string set_code { get; set; }
        public string set_rarity { get; set; }
        public string set_price { get; set; }
    }

    public class CardImage
    {
        public string id { get; set; }
        public string image_url { get; set; }
        public string image_url_small { get; set; }
        public string set_price { get; set; }
    }

    public class CardPrice
    {
        public string cardmarket_price { get; set; }
        public string tcgplayer_price { get; set; }
        public string ebay_price { get; set; }
        public string amazon_price { get; set; }
    }

}