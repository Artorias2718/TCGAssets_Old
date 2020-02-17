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
            JArray array = JArray.Parse(File.ReadAllText("C:/Users/artur/OneDrive/Desktop/GIT/TCG/TCGAssets/TCGAssets/Assets/" + tcg_name + "/JSON/Data.json"));

            foreach (JObject obj in array)
            {
                Card card = JsonConvert.DeserializeObject<Card>(obj.ToString());
                card.id = string.Format("{0:00000000}", int.Parse(card.id));
                foreach (CardImage img in card.card_images)
                {
                    img.id = string.Format("{0:00000000}", int.Parse(img.id));
                }
                cards.Add(card);
            }
        }

        //GET: Cards

        public List<string> GetIds(int id)
        {
            throw new NotImplementedException();
        }

        //GET: Card
        public List<string> GetId(string id)
        {
            List<Card> filter_by_id = cards.Where(card => card.id.Contains(id)).Distinct().ToList();
            List<string> result = new List<string>();
            
            foreach(Card card in filter_by_id)
            {
                foreach(CardImage img in card.card_images)
                {
                    if (!result.Contains(img.id))
                    {
                        if(card.type.Contains("Monster"))
                        {
                            result.Add(string.Format("http://localhost:62717/Assets/Yugioh/Img/Monsters/{0:00000000}.jpg", img.id));
                        }
                        if(card.type.Contains("Skill"))
                        {
                            result.Add(string.Format("http://localhost:62717/Assets/Yugioh/Img/Skills/{0:00000000}.jpg", img.id));
                        }
                        if(card.type.Contains("Spell"))
                        {
                            result.Add(string.Format("http://localhost:62717/Assets/Yugioh/Img/Spells/{0:00000000}.jpg", img.id));
                        }
                        if(card.type.Contains("Token"))
                        {
                            result.Add(string.Format("http://localhost:62717/Assets/Yugioh/Img/Tokens/{0:00000000}.jpg", img.id));
                        }
                        if(card.type.Contains("Trap"))
                        {
                            result.Add(string.Format("http://localhost:62717/Assets/Yugioh/Img/Traps/{0:00000000}.jpg", img.id));
                        }
                    }
                }
            }

            return result.Distinct().ToList();
        }

        // GET: Card
        public List<string> GetName(string id)
        {
            List<string> id_list = cards.Where(card => card.name.ToUpper().Contains(id.ToUpper())).Select(card => card.id).ToList();
            List<string> result = new List<string>();

            foreach (string numeric_id in id_list)
            {
                List<Card> filter_by_id = cards.Where(card => card.id.Contains(numeric_id)).Distinct().ToList();

                foreach (Card card in filter_by_id)
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
            }

            return result.Distinct().ToList();
        }
    }
}