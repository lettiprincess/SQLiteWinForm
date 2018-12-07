using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormDB
{
    public partial class Form1 : Form
    {
        SQLiteConnection conn;

        public Form1()
        {
            InitializeComponent();
            conn = new SQLiteConnection(ConfigurationManager.ConnectionStrings["connString"].ConnectionString);
            conn.Open();
            CreateTables();

            //opcionális
            FormClosed += (sender, e) => conn.Close();

            //TesztAdat();
            AdatKiir();
        }

        public void CreateTables() {
            var command = conn.CreateCommand();
            command.CommandText = @"CREATE TABLE IF NOT EXISTS csoportok (id INTEGER PRIMARY KEY AUTOINCREMENT ,
                                                                letrejott DATE NOT NULL);";
            command.ExecuteNonQuery();

            var command2 = conn.CreateCommand();
            command2.CommandText = @"CREATE TABLE IF NOT EXISTS halak (id INTEGER PRIMARY KEY AUTOINCREMENT ,
                                                                nev VARCHAR (1000) NOT NULL,
                                                                csoport_id INTEGER NOT NULL,
                                                                FOREIGN KEY (csoport_id) REFERENCES csoportok(id));";
            command2.ExecuteNonQuery();
        }

        void AdatKiir() {
            listBox1.Items.Clear();
            string formatString = "{0} ({1}. csoport, {2} alakult)";
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT h.nev nev, cs.id id, cs.letrejott letrejott FROM halak h INNER JOIN csoportok cs ON h.csoport_id = cs.id ORDER BY h.nev";

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    listBox1.Items.Add(string.Format(formatString, reader["nev"], reader["id"], reader["letrejott"]));

                }
            }
        }

        void TesztAdat() {
            var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO csoportok (letrejott) VALUES ('2018-12-07')";
            cmd.ExecuteNonQuery();
            var cmd2 = conn.CreateCommand();
            cmd2.CommandText = @"INSERT INTO halak (nev,csoport_id) VALUES ('aranyhal',1),('párduc',1)";
            cmd2.ExecuteNonQuery();
        }

    }
}
