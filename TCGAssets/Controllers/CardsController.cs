using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using TCGAssets.Models.Yugioh;
using RouteAttribute = System.Web.Http.RouteAttribute;

namespace TCGAssets.Controllers
{
    public class CardsController : ApiController
    {
        public string[] Directories { get; set; }
        private static List<Card> cards = new List<Card>();
        private static string tcg_name = "";
        private static int tcg = 2;

        public CardsController()
        {
            switch (tcg)
            {
                case 0:
                    tcg_name = "MTG";
                    break;
                case 1:
                    tcg_name = "Pokemon";
                    break;
                case 2:
                    tcg_name = "Yugioh";
                    break;
            }

            Directories = new string[] { "Monsters", "Skills", "Spells", "Tokens", "Traps" };
            JArray array = JArray.Parse(File.ReadAllText(HttpContext.Current.Server.MapPath("../../Assets/") + tcg_name + "/JSON/Data.json"));
            foreach (JObject obj in array)
            {
                Card card = JsonConvert.DeserializeObject<Card>(obj.ToString());
                card.id = string.Format("{0:00000000}", int.Parse(card.id));
                if(card.attribute == null)
                {
                    card.attribute = "";
                }
                foreach (CardImage img in card.card_images)
                {
                    img.id = string.Format("{0:00000000}", int.Parse(img.id));
                }
                cards.Add(card);
            }
        }

        [Route("api/cards/properties/")]
        public List<string> GetProperties([FromUri]
            string id = "", string name = "", string type = "", string desc = "", string atk = "", string def = "", string level = "", string race = "", string attribute = "", string archetype = ""
        )
        {
            Func<Card, bool> query = card =>
              !string.IsNullOrEmpty(id) ? card.id.Contains(id)
            : !string.IsNullOrEmpty(name) ? card.name.ToUpper().Contains(name.ToUpper())
            : !string.IsNullOrEmpty(type) ? card.type.ToUpper().Contains(type.ToUpper())
            : !string.IsNullOrEmpty(desc) ? card.desc.ToUpper().Contains(desc.ToUpper())
            : !string.IsNullOrEmpty(atk) ? card.atk.Contains(atk)
            : !string.IsNullOrEmpty(def) ? card.def.Contains(def)
            : !string.IsNullOrEmpty(level) ? card.level.Contains(level)
            : !string.IsNullOrEmpty(race) ? card.race.ToUpper().Contains(race.ToUpper())
            : !string.IsNullOrEmpty(attribute) ? card.attribute.ToUpper().Contains(attribute.ToUpper())
            : !string.IsNullOrEmpty(archetype) ? card.archetype.ToUpper().Contains(archetype.ToUpper())
            : false;

            List<Card> filtered_set = cards.Where(query).Distinct().ToList();

            List<string> result = new List<string>();
            
            foreach(Card card in filtered_set)
            {
                foreach (CardImage img in card.card_images)
                {
                    if (!result.Contains(img.id))
                    {
                        if (card.type.Contains("Monster"))
                        {
                            result.Add(string.Format("http://localhost:62717/Assets/Yugioh/Img/Monsters/{0:00000000}.jpg", img.id));
                        }
                        if (card.type.Contains("Skill"))
                        {
                            result.Add(string.Format("http://localhost:62717/Assets/Yugioh/Img/Skills/{0:00000000}.jpg", img.id));
                        }
                        if (card.type.Contains("Spell"))
                        {
                            result.Add(string.Format("http://localhost:62717/Assets/Yugioh/Img/Spells/{0:00000000}.jpg", img.id));
                        }
                        if (card.type.Contains("Token"))
                        {
                            result.Add(string.Format("http://localhost:62717/Assets/Yugioh/Img/Tokens/{0:00000000}.jpg", img.id));
                        }
                        if (card.type.Contains("Trap"))
                        {
                            result.Add(string.Format("http://localhost:62717/Assets/Yugioh/Img/Traps/{0:00000000}.jpg", img.id));
                        }
                    }
                }
            }

            return result;
        }
    }
}