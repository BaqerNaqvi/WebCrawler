using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebCrawler.Views
{
    public partial class test : System.Web.UI.Page
    {
        MySqlConnection con;
        MySqlCommand cmd;
        string str;

        protected void Page_Load(object sender, EventArgs e)
        {
            //con = new MySqlConnection("Data Source=qlumi-database11.catnt1gon99v.us-west-2.rds.amazonaws.com;Database=qlumi;User ID=db_user;Password=purelife");
            //con.Open();
            //Response.Write("connect");

            //str = "select * from Vendor";
            //cmd = new MySqlCommand(str, con);
            //DataTable dt = new DataTable();
            //dt.Load(cmd.ExecuteReader());

            //return jsSerializer.Serialize(dt); 
        }
    }
}