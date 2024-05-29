using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using BookRental.Models;

namespace BookRental.Controllers
{
  public class PretController : Controller
  {
    // GET : Pret/Reserver
    public ActionResult Reserver(string livreId)
    {
      if (Session["user"] == null)
      {
        return RedirectToAction("Index", "Website");
      }

      UserProfile profile = Session["user"] as UserProfile;
      int membreID = profile.Id;

      /*if (profile.Role == "admin")
      {
          membreID = membreId;
      }*/

      try
      {

        string db_connection = BookRental.Services.Utilitaire.GetDBConnectionString();

        using (SqlConnection dbConnection = new SqlConnection(db_connection))
        {
          dbConnection.Open();

          // Verifer si le client a atteind sa limite de livre par pret
          // recuperation liste des prets
          string requeteSqlSelect = "SELECT livre FROM pret where pret.membre=@membreId;";
          SqlDataAdapter sda1 = new SqlDataAdapter(requeteSqlSelect, dbConnection);
          sda1.SelectCommand.Parameters.AddWithValue("@membreId", membreID);
          DataTable tabPret = new DataTable();
          sda1.Fill(tabPret);
          sda1.Dispose();
          // maximum livre par pret par client
          if (tabPret.Rows.Count < BookRental.Services.Utilitaire.GetLimitPretParClient())
          {
            // client peut encore ajouter un livre						
            string requetSqlInsert = "insert into pret(membre , livre , premier_jour , jour_retour_prevue) ";
            requetSqlInsert += "values(@membreID , @livreID , GETDATE() , DATEADD(day , @maxJourPret , GETDATE() ));";
            SqlCommand cmd = new SqlCommand(requetSqlInsert, dbConnection);
            cmd.Parameters.AddWithValue("@membreID", membreID);
            cmd.Parameters.AddWithValue("@livreID", Convert.ToInt32(livreId));
            // total de jours de prets => 1 semaine
            cmd.Parameters.AddWithValue("@maxJourPret", BookRental.Services.Utilitaire.GetMaxJourPret());
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            DataTable tabLivre = new DataTable();
            string selectTitreLivre = "SELECT titre FROM livres where id=@livreId;";
            SqlDataAdapter dataAdapter = new SqlDataAdapter(selectTitreLivre, dbConnection);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@livreId", Convert.ToInt32(livreId));
            dataAdapter.Fill(tabLivre);
            TempData["info"] = "Le livre suivant  a été ajouté à votre résérvation : \""
                                + tabLivre.Rows[0][0] + "\"";
          }
          else
          {
            TempData["erreur"] = "Vous avez atteind la limite de votre résérvation de livres";
          }


        }
      }
      catch (System.Exception exc)
      {
        System.Diagnostics.Debug.WriteLine(exc.Message);
        TempData["erreur"] = "Erreur lors de la reservation du livre ";
      }
      return RedirectToAction("Index", "Website");


    }



    // GET: Pret/Retour/Id
    public ActionResult Retour(int id /* id du livre */)
    {
      if (Session["user"] == null)
      {
        return RedirectToAction("Index", "Website");
      }

      UserProfile profile = Session["user"] as UserProfile;
      if (profile.Role != "admin")
      {
        TempData["erreur"] = "Vous devez etre Administrateur pour retourner le livre";
        return RedirectToAction("Index", "Website");
      }

      try
      {
        string db_connection = BookRental.Services.Utilitaire.GetDBConnectionString();
        using (SqlConnection dbConnection = new SqlConnection(db_connection))
        {
          dbConnection.Open();
          string requeteSqlUpdate = "update pret set jour_retour_effectif=GETDATE() ";
          requeteSqlUpdate += "where pret.id=@pretID;";
          SqlCommand cmd = new SqlCommand(requeteSqlUpdate, dbConnection);
          cmd.Parameters.AddWithValue("@pretID", id);
          cmd.ExecuteNonQuery();
        }

        TempData["info"] = "Le livre a été retourné. Merci!";
      }
      catch (System.Exception exc)
      {
        System.Diagnostics.Debug.WriteLine(exc.Message);
        TempData["erreur"] = "Erreur dans le retour du livre";
      }

      return RedirectToAction("Prets", "Admin");
    }

  }
}
