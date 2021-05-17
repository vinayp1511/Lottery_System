using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Lottery_System.Models;

namespace Lottery_System.API
{    
    public class API
    {
        string connection = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;

        public long UserID
        {
            get
            {
                return long.Parse(HttpContext.Current.Session["UserID"].ToString());
            }
            set
            {
                HttpContext.Current.Session["UserID"] = value;
            }
        }
        
        //public string UserName;
        public string UserName
        {
            get
            {
                return (HttpContext.Current.Session["UserName"].ToString());
            }
            set
            {
                HttpContext.Current.Session["UserName"] = value;
            }
        }        

        //public string Email;
        public string Email
        {
            get
            {
                return (HttpContext.Current.Session["Email"].ToString());
            }
            set
            {
                HttpContext.Current.Session["Email"] = value;
            }
        }

        //public bool IsActive;
        public string IsActive
        {
            get
            {
                return (HttpContext.Current.Session["IsActive"].ToString());
            }
            set
            {
                HttpContext.Current.Session["IsActive"] = value;
            }
        }

        public DateTime CreatedDate
        {
            get
            {
                return Convert.ToDateTime(HttpContext.Current.Session["CreatedDate"].ToString());
            }
            set
            {
                HttpContext.Current.Session["CreatedDate"] = value;
            }
        }

        public bool Login(Login login)
        {
            bool IsValid = false;            
            try
            {
                DataSet ds = new DataSet();
                SqlConnection sqlcon = new SqlConnection(connection);
                string storedprocedure = "Lottery_01_VerifyUser";

                SqlCommand sqlcmd = new SqlCommand(storedprocedure, sqlcon);

                if (!string.IsNullOrEmpty(login.Email))
                {
                    sqlcmd.Parameters.AddWithValue("@Email", login.Email);
                }
                else
                {
                    sqlcmd.Parameters.AddWithValue("@Email", DBNull.Value);
                }
                if (!string.IsNullOrEmpty(login.Password))
                {
                    sqlcmd.Parameters.AddWithValue("@Password", login.Password);
                }
                else
                {
                    sqlcmd.Parameters.AddWithValue("@Password", DBNull.Value);
                }
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcon.Open();

                SqlDataAdapter adapter = new SqlDataAdapter(sqlcmd);
                adapter.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    this.UserID = Convert.ToInt64(ds.Tables[0].Rows[0]["UserID"].ToString());
                    this.Email = (ds.Tables[0].Rows[0]["Email"].ToString());
                    this.IsActive = (ds.Tables[0].Rows[0]["IsActive"].ToString());
                    this.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                    IsValid = true;
                }
                else
                {
                    IsValid = false;
                }

                sqlcon.Close();
            }
            catch(Exception ex)
            {
                IsValid = false;
            }

            return IsValid;
        }

        public bool RegisterUser(Users user)
        {
            bool IsValid = false;
            try
            {
                DataSet ds = new DataSet();
                SqlConnection sqlcon = new SqlConnection(connection);
                string storedprocedure = "Lottery_01_RegisterUser";

                SqlCommand sqlcmd = new SqlCommand(storedprocedure, sqlcon);

                if (!string.IsNullOrEmpty(user.Email))
                {
                    sqlcmd.Parameters.AddWithValue("@Email", user.Email);
                }
                else
                {
                    sqlcmd.Parameters.AddWithValue("@Email", DBNull.Value);
                }
                if (!string.IsNullOrEmpty(user.Password))
                {
                    sqlcmd.Parameters.AddWithValue("@Password", user.Password);
                }
                else
                {
                    sqlcmd.Parameters.AddWithValue("@Password", DBNull.Value);
                }
                
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcon.Open();                
                int suceess = sqlcmd.ExecuteNonQuery();
                //if(suceess>1)
                IsValid = true;
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                IsValid = false;
            }

            return IsValid;
        }

        public bool SaveBet(List<Bet> bets)
        {
            bool IsValid = false;
            try
            {
                DataSet ds = new DataSet();
                SqlConnection sqlcon = new SqlConnection(connection);
                string storedprocedure = "Lottery_01_SaveBet";

                SqlCommand sqlcmd = new SqlCommand(storedprocedure, sqlcon);

                for(int i = 0; i< bets.Count; i++)
                {
                    sqlcmd.Parameters.AddWithValue("@UserID", this.UserID);
                    if (Convert.ToInt32(bets[i].BetNumber) > 0)
                    {
                        sqlcmd.Parameters.AddWithValue("@BetNumber", bets[i].BetNumber);
                    }
                    else
                    {
                        sqlcmd.Parameters.AddWithValue("@Betnumber", DBNull.Value);
                    }
                    if (Convert.ToInt32(bets[i].BetAmount) > 0)
                    {
                        sqlcmd.Parameters.AddWithValue("@BetAmount", bets[i].BetAmount);
                    }
                    else
                    {
                        sqlcmd.Parameters.AddWithValue("@BetAmount", DBNull.Value);
                    }

                    if (Convert.ToInt32(bets[i].BetAmount) > 0)
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcon.Open();
                        int suceess = sqlcmd.ExecuteNonQuery();
                    }
                }
                
                //if(suceess>1)
                IsValid = true;
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                IsValid = false;
            }

            return IsValid;
        }

        public Bet GetWinner()
        {
            Bet winner = new Bet();

            try
            {
                DataSet ds = new DataSet();
                SqlConnection sqlcon = new SqlConnection(connection);
                string storedprocedure = "Lottery_01_GetWinner";
                SqlCommand sqlcmd = new SqlCommand(storedprocedure, sqlcon);                
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcon.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sqlcmd);

                sqlcmd.Parameters.AddWithValue("@UserID", this.UserID);                
                adapter.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    winner.UserID = Convert.ToInt64(ds.Tables[0].Rows[0]["UserID"].ToString());
                    winner.BetNumber = Convert.ToInt32(ds.Tables[0].Rows[0]["BetNumber"].ToString());
                    //winner.BetAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["BetAmount"].ToString());                    
                    winner.TotalBetAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["TotalBetAmount"].ToString());                    
                }                

                sqlcon.Close();
            }
            catch(Exception ex)
            {

            }
            return winner;
        }

        public List<Country> GetCountryList()
        {
            List<Country> listCountry = new List<Country>();

            try
            {
                DataSet ds = new DataSet();
                SqlConnection sqlcon = new SqlConnection(connection);
                string storedprocedure = "Lottery_01_GetCountryList";
                SqlCommand sqlcmd = new SqlCommand(storedprocedure, sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcon.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sqlcmd);
                
                adapter.Fill(ds);

                for(int i=0;i< ds.Tables[0].Rows.Count;i++)
                {
                    Country country = new Country();

                    country.CountryId = Convert.ToInt64(ds.Tables[0].Rows[i]["CountryId"].ToString());
                    country.CountryName = Convert.ToString(ds.Tables[0].Rows[i]["CountryName"].ToString());

                    listCountry.Add(country);
                }

                sqlcon.Close();
            }
            catch (Exception ex)
            {

            }
            return listCountry;
        }

        public List<State> GetStateList(long CountryId)
        {
            List<State> lstState = new List<State>();

            try
            {
                DataSet ds = new DataSet();
                SqlConnection sqlcon = new SqlConnection(connection);
                string storedprocedure = "Lottery_01_GetStateList";
                SqlCommand sqlcmd = new SqlCommand(storedprocedure, sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcon.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sqlcmd);

                sqlcmd.Parameters.AddWithValue("@CountryId", CountryId);
                adapter.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    State state = new State();

                    state.StateId = Convert.ToInt64(ds.Tables[0].Rows[i]["StateId"].ToString());
                    state.StateName = Convert.ToString(ds.Tables[0].Rows[i]["StateName"].ToString());

                    lstState.Add(state);
                }

                sqlcon.Close();
            }
            catch (Exception ex)
            {

            }
            return lstState;
        }
    }
}