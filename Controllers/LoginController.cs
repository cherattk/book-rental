using BookRental.Models;
using System;
using System.Data.SqlClient;
using System.Data;
using System.Web.Mvc;

namespace BookRental.Controllers
{
	public class LoginController : Controller
	{
		private string db_connection = BookRental.Services.Utilitaire.GetDBConnectionString();

		// GET /Login/Index
		// Login Form
		// public ActionResult Index(string user)
    public ActionResult Index()
    {
      ///////////////////////////////////////////////////
      /// Dev Only
      /*UserProfile profile = new UserProfile();
			if (user == "admin") {
				profile.Id = 1;
				profile.Name = "karim";
				profile.Prenom = "cheratt";
				profile.Role = "admin";

			}
			else
			{
				profile.Id = 2;
				profile.Name = "john";
				profile.Prenom = "Doe";
				profile.Role = "membre";
			}

			Session["user"] = profile;
			return RedirectToAction("Index", "Website");*/
			///////////////////////////////////////////////////
			
			return View();

		}

		// POSt /Index/Login
		[HttpPost]
		public ActionResult Login(FormCollection authData)
		{
			//////////////////////////////////////////////////
			try
			{
				string UserEmail = authData["user_email"];
				string UserPass = authData["user_pass"];

				DataTable userData = new DataTable();
				using (SqlConnection dbConnection = new SqlConnection(db_connection))
				{
					dbConnection.Open();
					string sqlQuery = "SELECT id , nom , prenom , role FROM membre WHERE courriel=@email and password=@password;";
					SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlQuery, dbConnection);
					dataAdapter.SelectCommand.Parameters.AddWithValue("@email", UserEmail);
					dataAdapter.SelectCommand.Parameters.AddWithValue("@password", UserPass);
					dataAdapter.Fill(userData);
				}

				if (userData.Rows.Count == 1)
				{
					UserProfile profile = new UserProfile()
					{
						Id = Convert.ToInt32(userData.Rows[0][0].ToString()),
						Name = userData.Rows[0][1].ToString(),
						Prenom = userData.Rows[0][2].ToString(),
						Role = userData.Rows[0][3].ToString()
					};

					Session["user"] = profile;
					if (profile.Role == "admin")
					{
						return RedirectToAction("Index", "Admin");
					}
					else
					{
						return RedirectToAction("Index", "Website");
					}
				}
				else
				{
					ViewBag.Error = "Utilisateur introuvable";
					return View("Index");
				}

			}
			catch (System.Exception exc)
			{
				System.Diagnostics.Debug.WriteLine(exc.Message);
				ViewBag.Error = "Erreur de connection";
				return View("Index");
			}



		}

		public ActionResult Logout()
		{
			Session["user"] = null;
			return RedirectToAction("Index", "Website");
		}
	}
}