using BookRental.Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;

namespace BookRental.Services
{
	public class Utilitaire
	{
		public const double FraisRetard = 0.50;

		public static int GetMaxJourPret(){
			return 7; // total de jours de prets => 1 semaine
		}

		public static int GetLimitPretParClient()
		{
			return 10; // maximum livre par pret par client
		}

		public static void UpdateTableRetard(int? UserId = null)
		{
			string db_connection = GetDBConnectionString();
			using (SqlConnection dbConnection = new SqlConnection(db_connection))
			{
				dbConnection.Open();
				// creer la requete pour mettre ajour la table des retards
				//*
				string sqlQuery = @"update pret 
									set nbr_jour_retard = case  
										when pret.jour_retour_effectif is null 
												then DATEDIFF(day, pret.jour_retour_prevue, GETDATE())
									else 
										DATEDIFF(day, pret.jour_retour_prevue , pret.jour_retour_effectif )
									end";

				if(UserId != null){
					// executer pour 1 utilisateur seulement
					sqlQuery += " where pret.membre=@UserId;";
				}
				else{
					sqlQuery += ";";
				}				

				SqlCommand cmd = new SqlCommand(sqlQuery , dbConnection);
				if (UserId != null)
				{
					cmd.Parameters.AddWithValue("@UserId", UserId);
				}
					
				cmd.ExecuteNonQuery();				

			}
		}

		public static string GetDBConnectionString(){

			string connectionString = ConfigurationManager.ConnectionStrings["DBLivres"].ConnectionString;
			return connectionString;
		}

		public static UserProfile GetUserProfileFromSession(HttpSessionStateBase session)
		{
			UserProfile profile = new UserProfile();
			if (session["user"] != null)
			{
				profile = session["user"] as UserProfile;			
			}
			return profile;
		}
	}
}