using CGC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.Funkcje.CutFuncFolder.CutBase
{
    public class CutBaseReturn
    {
        private Connect connect = new Connect();

        private static CutBaseReturn m_oInstance = null;
        private static readonly object m_oPadLock = new object();

        public static CutBaseReturn Instace
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new CutBaseReturn();
                    }
                    return m_oInstance;
                }
            }
        }

        public List<Cut_Project> GetCut_Project(string company)
        {
            List<Cut_Project> temp = new List<Cut_Project>();

            SqlCommand command = new SqlCommand("SELECT * FROM [Cut_Project] WHERE Company = @Company;", connect.cnn);
            connect.cnn.Open();

            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Cut_Project cut_Project = new Cut_Project();
                cut_Project.Cut_id = sqlDataReader["Cut_id"].ToString();
                cut_Project.Order_id = sqlDataReader["Order_id"].ToString();
                cut_Project.Status = sqlDataReader["Status"].ToString();

                temp.Add(cut_Project);
            }

            //foreach(Cut_Project cut in temp)
            //{
            //    command = new SqlCommand("SELECT * FROM [Order] WHERE Company = @Company AND Order_id = @Order_id;", connect.cnn);
            //    connect.cnn.Open();

            //    command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;
            //    command.Parameters.Add("@Order_id", SqlDbType.VarChar, 40).Value = cut.Order_id;

            //    sqlDataReader = command.ExecuteReader();

            //    while (sqlDataReader.Read())
            //    {
            //        cut.Owner = sqlDataReader["Owner"].ToString();
            //    }
            //}

            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();
            return temp;
        }

        public List<Cut_Project> Get_Cut_Project(string status, string company)
        {
            List<Cut_Project> temp = new List<Cut_Project>();

            SqlCommand command = new SqlCommand("SELECT * FROM [Cut_Project] WHERE Status = @Status AND Company = @Company;", connect.cnn);

            command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = status;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

            connect.cnn.Open();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Cut_Project cut_Project = new Cut_Project();
                cut_Project.Cut_id = sqlDataReader["Cut_id"].ToString();
                cut_Project.Order_id = sqlDataReader["Order_id"].ToString();
                cut_Project.Status = sqlDataReader["Status"].ToString();

                temp.Add(cut_Project);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();

            foreach(Cut_Project cut in temp)
            {
                command = new SqlCommand("SELECT * FROM [Order] WHERE Id_Order = @Id_Order AND Company = @Company;", connect.cnn);

                command.Parameters.Add("@Id_Order", SqlDbType.VarChar, 40).Value = cut.Order_id;
                command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

                connect.cnn.Open();

                sqlDataReader = command.ExecuteReader();
                while (sqlDataReader.Read())
               {
                    cut.Priority = sqlDataReader["Priority"].ToString();
                    cut.Deadline = sqlDataReader["Deadline"].ToString();
                    cut.Status = sqlDataReader["Status"].ToString();
                    cut.Owner = sqlDataReader["Owner"].ToString();
                }
                sqlDataReader.Close();
                command.Dispose();
                connect.cnn.Close();
            }

            return temp;
        }

        public List<Cut_Project> Get_Cut_Project(string status, string status2, string company)
        {
            List<Cut_Project> temp = new List<Cut_Project>();

            SqlCommand command = new SqlCommand("SELECT * FROM [Cut_Project] WHERE (Status = @Status OR Status = @Status2) AND Company = @Company;", connect.cnn);

            command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = status;
            command.Parameters.Add("@Status2", SqlDbType.VarChar, 40).Value = status2;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

            connect.cnn.Open();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Cut_Project cut_Project = new Cut_Project();
                cut_Project.Cut_id = sqlDataReader["Cut_id"].ToString();
                cut_Project.Order_id = sqlDataReader["Order_id"].ToString();
                cut_Project.Status = sqlDataReader["Status"].ToString();

                temp.Add(cut_Project);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();

            foreach (Cut_Project cut in temp)
            {
                command = new SqlCommand("SELECT * FROM [Order] WHERE Id_Order = @Id_Order AND Company = @Company;", connect.cnn);

                command.Parameters.Add("@Id_Order", SqlDbType.VarChar, 40).Value = cut.Order_id;
                command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

                connect.cnn.Open();

                sqlDataReader = command.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    cut.Priority = sqlDataReader["Priority"].ToString();
                    cut.Deadline = sqlDataReader["Deadline"].ToString();
                    cut.Owner = sqlDataReader["Owner"].ToString();
                }
                sqlDataReader.Close();
                command.Dispose();
                connect.cnn.Close();
            }

            return temp;
        }

        public List<Cut_Project> GetLastGlobalIdCutProject(string company)
        {
            List<Cut_Project> temp = new List<Cut_Project>();
            SqlCommand command = new SqlCommand("Select TOP(1) Global_id From [Cut_Project] ORDER BY convert(int, Global_id) DESC", connect.cnn);
            connect.cnn.Open();

            //command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Cut_Project cut_Project = new Cut_Project();
                cut_Project.Global_Id = Convert.ToInt32(sqlDataReader["Global_id"]) + 1;

                temp.Add(cut_Project);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();

            if (temp.Count == 0)
            {
                temp.Add(new Cut_Project { Global_Id = 1 });
            }


            return temp;
        }
    }
}
