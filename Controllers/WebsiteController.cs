/*
 * Cheratt Karim
 * TP2 - IFT-1148
 */

using BookRental.Models;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Web.Mvc;

namespace BookRental.Controllers
{
  public class WebsiteController : Controller
  {
    private string db_connection = BookRental.Services.Utilitaire.GetDBConnectionString();

    // GET: Index
    public ActionResult Index()
    {
      DataTable tabLivre = new DataTable();
      using (SqlConnection dbConnection = new SqlConnection(db_connection))
      {
        dbConnection.Open();
        string sqlQuery = "SELECT * FROM livres;";
        SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlQuery, dbConnection);
        
        dataAdapter.Fill(tabLivre);

        if (Session["user"] != null)
        {
          UserProfile profile = Session["user"] as UserProfile;
          int membreID = profile.Id;
          // liste des prets du membre
          string sqlPrets = "SELECT livre FROM pret where membre=@membreId;";
          SqlDataAdapter da = new SqlDataAdapter(sqlPrets, dbConnection);
          da.SelectCommand.Parameters.AddWithValue("@membreId", membreID);
          DataTable tabPrets = new DataTable();
          da.Fill(tabPrets);
          //
          ViewBag.userPrets = "";
          DataRow[] listPret = tabPrets.Select();
          foreach (var item in listPret)
          {
            ViewBag.userPrets += item["livre"]+",";
          }
            
        }

      }

      // message d'erreur depuis un autre controller
      if (TempData["erreur"] != null)
      {
        ViewBag.Erreur = TempData["erreur"];
      }

      // message informatif depuis un autre controller
      if (TempData["info"] != null)
      {
        ViewBag.Message = TempData["info"];
      }

      return View(tabLivre);
    }

  }

}