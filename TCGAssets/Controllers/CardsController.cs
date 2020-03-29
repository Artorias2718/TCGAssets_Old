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
        public string[] YugiohDirectories { get; set; }
        private static List<Card> cards = new List<Card>();

        public CardsController()
        {
            YugiohDirectories = new string[] { "Monsters", "Skills", "Spells", "Tokens", "Traps" };
            JArray array = JArray.Parse(File.ReadAllText(HttpContext.Current.Server.MapPath("../../Assets/Yugioh/JSON/Data.json")));
            foreach (JObject obj in array)
            {
                Card card = JsonConvert.DeserializeObject<Card>(obj.ToString());
                card.id = string.Format("{0:00000000}", int.Parse(card.id));
                if (card.attribute == null)
                {
                    card.attribute = "";
                }

                if (card.atk == null)
                {
                    card.atk = "";
                }

                if (card.def == null)
                {
                    card.def = "";
                }

                if (card.level == null)
                {
                    card.level = "";
                }

                foreach (CardImage img in card.card_images)
                {
                    img.id = string.Format("{0:00000000}", int.Parse(img.id));
                }
                cards.Add(card);
            }
        }

        [Route("api/cards/yugioh_properties/")]
        public List<string> GetProperties([FromUri]
            string id = "", string name = "", string type = "", string desc = "", string atk = "", string def = "", string level = "", string race = "", string attribute = "", string archetype = ""
        )
        {
            List<Card> filtered_set = cards;

            if (!string.IsNullOrEmpty(id))
            {
                filtered_set = (from card in filtered_set
                                where card.id.Contains(id)
                                select card).Distinct().ToList();
            }

            if (!string.IsNullOrEmpty(name))
            {
                filtered_set = (from card in filtered_set
                                where card.name.ToUpper().Contains(name.ToUpper())
                                select card).Distinct().ToList();
            }

            if (!string.IsNullOrEmpty(type))
            {
                filtered_set = (from card in filtered_set
                                where card.type.ToUpper().Contains(type.ToUpper())
                                select card).Distinct().ToList();
            }

            if (!string.IsNullOrEmpty(desc))
            {
                filtered_set = (from card in filtered_set
                                where card.desc.ToUpper().Contains(desc.ToUpper())
                                select card).Distinct().ToList();
            }

            if (!string.IsNullOrEmpty(atk))
            {
                filtered_set = (from card in filtered_set
                                where card.atk.Contains(atk)
                                select card).Distinct().ToList();
            }

            if (!string.IsNullOrEmpty(def))
            {
                filtered_set = (from card in filtered_set
                                where card.def.Contains(def)
                                select card).Distinct().ToList();
            }

            if (!string.IsNullOrEmpty(level))
            {
                filtered_set = (from card in filtered_set
                                where card.level.Contains(level)
                                select card).Distinct().ToList();
            }

            if (!string.IsNullOrEmpty(race))
            {
                filtered_set = (from card in filtered_set
                                where card.race.ToUpper().Contains(race.ToUpper())
                                select card).Distinct().ToList();
            }

            if (!string.IsNullOrEmpty(attribute))
            {
                filtered_set = (from card in filtered_set
                                where card.attribute.ToUpper().Contains(attribute.ToUpper())
                                select card).Distinct().ToList();
            }

            if (!string.IsNullOrEmpty(archetype))
            {
                filtered_set = (from card in filtered_set
                                where card.archetype.ToUpper().Contains(archetype.ToUpper())
                                select card).Distinct().ToList();
            }

            List<int> monster = new List<int>();
            List<int> pendulum = new List<int>();
            List<int> skill = new List<int>();
            List<int> spell = new List<int>();
            List<int> token = new List<int>();
            List<int> trap = new List<int>();

            List<string> result = new List<string>();

            foreach (Card card in filtered_set)
            {
                foreach (CardImage img in card.card_images)
                {
                    if (!result.Contains(img.id))
                    {
                        if (card.type.Contains("Monster"))
                        {
                            if (card.type.Contains("Pendulum"))
                            {
                                pendulum.Add(int.Parse(img.id));
                            }
                            else
                            {
                                monster.Add(int.Parse(img.id));
                            }
                        }
                        if (card.type.Contains("Skill"))
                        {
                            skill.Add(int.Parse(img.id));
                        }
                        if (card.type.Contains("Spell"))
                        {
                            spell.Add(int.Parse(img.id));
                        }
                        if (card.type.Contains("Token"))
                        {
                            token.Add(int.Parse(img.id));
                        }
                        if (card.type.Contains("Trap"))
                        {
                            trap.Add(int.Parse(img.id));
                        }
                    }
                }
            }

            if (monster.Count > 0)
            {
                monster = monster.Distinct().ToList();
                monster.Sort();
            }
            if (pendulum.Count > 0)
            {
                pendulum = pendulum.Distinct().ToList();
                pendulum.Sort();
            }
            if (skill.Count > 0)
            {
                skill = skill.Distinct().ToList();
                skill.Sort();
            }
            if (spell.Count > 0)
            {
                spell = spell.Distinct().ToList();
                spell.Sort();
            }
            if (token.Count > 0)
            {
                token = token.Distinct().ToList();
                token.Sort();
            }
            if (trap.Count > 0)
            {
                trap = trap.Distinct().ToList();
                trap.Sort();
            }

            foreach (int m_id in monster)
            {
                result.Add(string.Format("http://localhost:62717/Assets/Yugioh/Img/Monsters/{0:00000000}.jpg", m_id));
            }
            foreach (int p_id in pendulum)
            {
                result.Add(string.Format("http://localhost:62717/Assets/Yugioh/Img/Monsters/Pendulum/{0:00000000}.jpg", p_id));
            }
            foreach (int s_id in skill)
            {
                result.Add(string.Format("http://localhost:62717/Assets/Yugioh/Img/Skills/{0:00000000}.jpg", s_id));
            }
            foreach (int s_id in spell)
            {
                result.Add(string.Format("http://localhost:62717/Assets/Yugioh/Img/Spells/{0:00000000}.jpg", s_id));
            }
            foreach (int t_id in token)
            {
                result.Add(string.Format("http://localhost:62717/Assets/Yugioh/Img/Tokens/{0:00000000}.jpg", t_id));
            }
            foreach (int t_id in trap)
            {
                result.Add(string.Format("http://localhost:62717/Assets/Yugioh/Img/Traps/{0:00000000}.jpg", t_id));
            }

            return result;
        }
    }
}