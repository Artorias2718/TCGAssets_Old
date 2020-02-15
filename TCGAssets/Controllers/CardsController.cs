using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace TCGAssets.Controllers
{
    public class CardsController : ApiController
    {
        public string[] Directories { get; set; }

        // GET: Card 
        public string Get(int id, int tcg = 2)
        //public ActionResult Get(int id, int tcg = 2)
        {
            string tcg_name = "";

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

            // I have no idea how to pull this without using an absoulte path; relative paths seem to be failing.
            // This likely means I will have to update this when it's time to host it on a live server.
            Directories = Directory.GetDirectories("C:/Users/artur/OneDrive/Desktop/GIT/TCG/TCGAssets/TCGAssets/Assets/" + tcg_name + "/Img/", "*", SearchOption.AllDirectories);

            for(int i = 0; i < Directories.Length; ++i)
            {
                Directories[i] = Directories[i].Substring(Directories[i].LastIndexOf('/') + 1);
            }

            string path = "";

            for (int j = 0; j < Directories.Length; ++j)
            {
                path = string.Format("http://localhost:62717/Assets/{0}/{1}/{2}/{3:00000000}.jpg", tcg_name , "Img", Directories[j], id);
                try
                {
                    System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(new Uri(path));
                    request.Method = "HEAD";
                    System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
                    response.Close();
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        break;
                    }
                }
                catch (Exception e)
                {

                }
            }

            return path;
            //return View("Card", Json(path));
        }
    }
}