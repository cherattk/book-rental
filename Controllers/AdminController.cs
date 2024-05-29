using System.Data.SqlClient;
using System.Data;
using System.Web.Mvc;
using BookRental.Models;

namespace BookRental.Controllers
{
	public class AdminController : Controller
	{
		// Recuprer la valeur de <connectionStrings> dans web.config
		private string db_connection = BookRental.Services.Utilitaire.GetDBConnectionString();

		private const string Role = "admin";

		public ActionResult Index()
		{
			return RedirectToAction("Livres");
		}

		public ActionResult Membres()
		{
			UserProfile profile = BookRental.Services.Utilitaire.GetUserProfileFromSession(Session);
			if (profile.Role != AdminController.Role)
			{
				return RedirectToAction("Index", "Login");
			}

			DataTable tabMembre = new DataTable();
			using (SqlConnection dbConnection = new SqlConnection(db_connection))
			{
				dbConnection.Open();
				string sqlQuery = "select * from membre;";
				SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlQuery, dbConnection);
				dataAdapter.Fill(tabMembre);
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

			return View(tabMembre);

		}

		public ActionResult Livres()
		{
			UserProfile profile = BookRental.Services.Utilitaire.GetUserProfileFromSession(Session);
			if (profile.Role != AdminController.Role)
			{
				return RedirectToAction("Index", "Login");
			}

			DataTable tabLivres = new DataTable();
			using (SqlConnection dbConnection = new SqlConnection(db_connection))
			{
				dbConnection.Open();
				string sqlQuery = "select * from livres;";
				SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlQuery, dbConnection);
				dataAdapter.Fill(tabLivres);
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

			return View(tabLivres);
		}

		// GET: Prets
		/// <summary>
		/// Retourne la liste des prets
		/// </summary>
		public ActionResult Prets()
		{			
			UserProfile profile = BookRental.Services.Utilitaire.GetUserProfileFromSession(Session);
			if (profile.Role != AdminController.Role)
			{
				return RedirectToAction("Index", "Login");
			}

			// update la table des retard
			BookRental.Services.Utilitaire.UpdateTableRetard();

			DataTable tabPret = new DataTable();
			using (SqlConnection dbConnection = new SqlConnection(db_connection))
			{
				dbConnection.Open();
				string sqlQuery = "select membre.nom , membre.prenom , pret.id as pretId , livres.image , livres.titre , ";
				sqlQuery += "pret.premier_jour , pret.jour_retour_prevue , pret.jour_retour_effectif , ";
				sqlQuery += "pret.nbr_jour_retard from  pret ";
				sqlQuery += "left join membre on pret.membre = membre.id ";
				sqlQuery += "left join livres on pret.livre = livres.id ;";
				SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlQuery, dbConnection);
				dataAdapter.Fill(tabPret);
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

			ViewBag.FraisRetard = BookRental.Services.Utilitaire.FraisRetard;

			return View(tabPret);
		}


		/*
		public ActionResult ChercherMembre(string nom)
		{
			List<UserProfile> membreList = new List<UserProfile>();
			
			//UserProfile profile = BookRental.Services.Utilitaire.GetUserProfileFromSession(Session);
			//if (profile.Role != AdminController.Role)
			//{
				//return membreList;
			//}
			var sqlQuery = "select id , nom , prenom from membre where lower(nom)=@Nom or lower(prenom)=@Nom ;";
			using (SqlConnection dbConnection = new SqlConnection(db_connection))
			{
				dbConnection.Open();
				SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlQuery, dbConnection);
				dataAdapter.SelectCommand.Parameters.AddWithValue("@Nom", nom);
				using (SqlDataReader reader = dataAdapter.SelectCommand.ExecuteReader())
				{
					if (reader.HasRows)
					{
						while (reader.Read())
						{
							membreList.Add(new UserProfile()
							{
								Id = reader.GetInt32(0),
								Name = reader.GetString(1),
								Prenom = reader.GetString(2)
							});
						}
					}
					reader.Close();
				}

				dbConnection.Close();
			}
			return this.Json(membreList, JsonRequestBehavior.AllowGet);
		}
		//*/
	}
}