using Doviz.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Doviz.Core
{
    public class DatabaseLogicLayer : Base
    {
        SqlConnection connection;
        SqlCommand cmd;
        SqlDataReader reader;

        public DatabaseLogicLayer()
        {
            connection = new SqlConnection("Data Source=ABDULLAH\\SQLEXPRESS01;Initial Catalog=Doviz;Integrated Security=True;");
        }

        public void BaglantiIslemleri()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
            else
            {
                connection.Open();
            }
        }

        public SqlDataReader ParaBirimiListesi() // para birmi listesi
        {

            TryCatchKullan(() =>
            {
                BaglantiIslemleri();
                cmd = new SqlCommand("SELECT * FROM ParaBirimi", connection);
                reader = cmd.ExecuteReader(); // Eğer hata alacak isek burada alacağımız için reader'ı burada açıyoruz ki hata
                // catch bloğuna düşsün.
            });

            return reader;

        }

        public SqlDataReader KurListe() // Kur listesi
        {
            TryCatchKullan(() => 
            { 
                cmd = new SqlCommand("SELECT * FROM Kur", connection);
                BaglantiIslemleri();
                reader = cmd.ExecuteReader();

            });
            return reader;

        }

        public SqlDataReader KurListe(Guid ParaBirimiID) // böylece sadece belirli bir para biriminin kurunu alabiliriz.
        {
            TryCatchKullan(() =>
            {
                cmd = new SqlCommand("SELECT * FROM Kur where ParaBirimiID = @ParaBirimiID", connection);
                cmd.Parameters.AddWithValue("@ParaBirimiID", System.Data.SqlDbType.UniqueIdentifier).Value = ParaBirimiID;
                BaglantiIslemleri();
                reader = cmd.ExecuteReader();

            });
            return reader;

        }

        public SqlDataReader KurGecmisListe() // Kur geçmişi listesi
        {
            TryCatchKullan(() =>
            {
                cmd = new SqlCommand("SELECT * FROM KurGecmis", connection);
                BaglantiIslemleri();
                reader = cmd.ExecuteReader();

            }); 
            return reader;

        }

        public SqlDataReader KurGecmisListe(Guid ParaBirimiID) // böylece sadece belirli bir para biriminin geçmişini alabiliriz.
        {
            TryCatchKullan(() =>
            {
                cmd = new SqlCommand("SELECT * FROM KurGecmis where ParaBirimiID = @ParaBirimiID", connection);
                cmd.Parameters.Add("@ParaBirimiID", System.Data.SqlDbType.UniqueIdentifier).Value = ParaBirimiID;
                BaglantiIslemleri();
                reader = cmd.ExecuteReader();

            });
            return reader;

        }

        public void KurKayitEkle(Kur kur)
        {
            TryCatchKullan(() => 
            {
                cmd = new SqlCommand("KurKayitEKLE", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID" , System.Data.SqlDbType.UniqueIdentifier).Value = kur.ID;
                cmd.Parameters.Add("@ParaBirimiID", System.Data.SqlDbType.UniqueIdentifier).Value = kur.ParaBirimiID;
                cmd.Parameters.Add("@Alis", System.Data.SqlDbType.Decimal).Value = kur.Alis;
                cmd.Parameters.Add("@Satis", System.Data.SqlDbType.Decimal).Value = kur.Satis;
                cmd.Parameters.Add("@OlusturmaTarih", System.Data.SqlDbType.DateTime).Value = kur.OlusturmaTarih;
                BaglantiIslemleri();
                cmd.ExecuteNonQuery(); // ExecuteNonQuery, sorgunun sonucunu döndürmez, sadece etki alanını döndürür.
                // Örneğin, INSERT, UPDATE veya DELETE sorguları için kullanılır.
                // ExecuteReader, sorgunun sonucunu döndürür ve SqlDataReader nesnesi ile okunabilir.
                // Örneğin, SELECT sorguları için kullanılır.

            });
            BaglantiIslemleri();
        }
    }
}
