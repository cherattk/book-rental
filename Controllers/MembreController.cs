using System.Data.SqlClient;
using System.Data;
using System.Web.Mvc;
using BookRental.Models;

namespace BookRental.Controllers
{
	public class MembreController : Controller
	{
		// Recuprer la valeur de <connectionStrings> dans web.config
		private string db_connection = BookRental.Services.Utilitaire.GetDBConnectionString();
		private const string Role = "membre";

		// GET: User
		public ActionResult Index()
		{
			UserProfile profile = BookRental.Services.Utilitaire.GetUserProfileFromSession(Session);

			if (profile.Role != MembreController.Role)
			{
				return RedirectToAction("Index", "Index");
			}

			// update la table des retard
			BookRental.Services.Utilitaire.UpdateTableRetard(profile.Id);

			DataTable tabPret = new DataTable();
			using (SqlConnection dbConnection = new SqlConnection(db_connection))
			{
				dbConnection.Open();
				string sqlQuery = "select pret.id as pretId , livres.image , livres.titre , ";
				sqlQuery += "pret.premier_jour , pret.jour_retour_prevue , pret.jour_retour_effectif , ";
				sqlQuery += "retard.nbr_jour_retard , retard.montant ";
				sqlQuery += "from  pret ";
				sqlQuery += "left join retard on pret.id = retard.pret ";
				sqlQuery += "left join livres on pret.livre = livres.id ";
				sqlQuery += "where pret.membre = @membreID;";
				SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlQuery, dbConnection);
				dataAdapter.SelectCommand.Parameters.AddWithValue("@membreID", profile.Id);
				dataAdapter.Fill(tabPret);
			}
			return View(tabPret);
		}

	}
}
