using Doviz.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Doviz.Core
{
    public class BusinessLogicLayer : Base
    {
        DatabaseLogicLayer DLL;

        public BusinessLogicLayer()
        {
            DLL = new DatabaseLogicLayer();
        }

        public List<ParaBirimi> ParaBirimiListesi()
        {
            List<ParaBirimi> ParaBirimleri = new List<ParaBirimi>();
            SqlDataReader reader = DLL.ParaBirimiListesi();
            while (reader.Read())
            {
                ParaBirimleri.Add(new ParaBirimi()
                {// bu yazım stili sayesinde sql'de boş değerler DBNull olarak geleceği için bu kontrol yapılmazsa,
                 // reader.Get...() metodları DBNull ile karşılaştığında exception fırlatabilir (InvalidCastException).
                    ID = reader.IsDBNull(0) ? Guid.Empty : reader.GetGuid(0),
                    Code = reader.IsDBNull(1) ? string.Empty : reader.GetString(1), //Hataların önüne geçer (null değerlerden dolayı çökme olmaz).
                    Tanim = reader.IsDBNull(2) ? string.Empty : reader.GetString(2), //Kod daha güvenli (robust) hale gelir.
                    UyariLimit = reader.IsDBNull(3) ? 0 : reader.GetDecimal(3),
                    Email = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    AdSoyad = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
                });
            }
            reader.Close();
            DLL.BaglantiIslemleri();
            return ParaBirimleri;
        }

        public List<Kur> KurListe()
        {
            List<Kur> KurDegerleri = new List<Kur>();

            SqlDataReader reader = DLL.KurListe();
            while (reader.Read())
            {
                KurDegerleri.Add(new Kur()
                {
                    ID = reader.IsDBNull(0) ? Guid.Empty : reader.GetGuid(0),
                    ParaBirimiID = reader.IsDBNull(1) ? Guid.Empty : reader.GetGuid(1),
                    Alis = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2),
                    Satis = reader.IsDBNull(3) ? 0 : reader.GetDecimal(3),
                    OlusturmaTarih = reader.IsDBNull(4) ? DateTime.MinValue : reader.GetDateTime(4),
                });
            }
            reader.Close();
            DLL.BaglantiIslemleri();

            return KurDegerleri;
        }


        public Kur KurListe(Guid ParaBirimiID)
        {
            Kur Kur = new Kur();

            SqlDataReader reader = DLL.KurListe(ParaBirimiID);
            while (reader.Read())
            {
                Kur = new Kur()
                {
                    ID = reader.IsDBNull(0) ? Guid.Empty : reader.GetGuid(0),
                    ParaBirimiID = reader.IsDBNull(1) ? Guid.Empty : reader.GetGuid(1),
                    Alis = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2),
                    Satis = reader.IsDBNull(3) ? 0 : reader.GetDecimal(3),
                    OlusturmaTarih = reader.IsDBNull(4) ? DateTime.MinValue : reader.GetDateTime(4),
                };
            }
            reader.Close();
            DLL.BaglantiIslemleri();

            return Kur;
        }

        public List<KurGecmis> KurGecmisListe()
        {
            List<KurGecmis> kurGecmisdegerleri = new List<KurGecmis>();
            SqlDataReader reader = DLL.KurGecmisListe();
            while (reader.Read())
            {
                kurGecmisdegerleri.Add(new KurGecmis()
                {
                    ID = reader.IsDBNull(0) ? Guid.Empty : reader.GetGuid(0),
                    KurID = reader.IsDBNull(1) ? Guid.Empty : reader.GetGuid(1),
                    ParaBirimiID = reader.IsDBNull(2) ? Guid.Empty : reader.GetGuid(2),
                    Alis = reader.IsDBNull(3) ? 0 : reader.GetDecimal(3),
                    Satis = reader.IsDBNull(4) ? 0 : reader.GetDecimal(4),
                    OlusturmaTarih = reader.IsDBNull(5) ? DateTime.MinValue : reader.GetDateTime(5)

                });

            }
            reader.Close();
            DLL.BaglantiIslemleri();
            return kurGecmisdegerleri;
        }

        public KurGecmis KurGecmisListe(Guid ParaBirimiID)
        {
            KurGecmis kurGecmis = new KurGecmis();
            SqlDataReader reader = DLL.KurGecmisListe(ParaBirimiID);
            while (reader.Read())
            {
                kurGecmis = new KurGecmis()
                {
                    ID = reader.IsDBNull(0) ? Guid.Empty : reader.GetGuid(0),
                    KurID = reader.IsDBNull(1) ? Guid.Empty : reader.GetGuid(1),
                    ParaBirimiID = reader.IsDBNull(2) ? Guid.Empty : reader.GetGuid(2),
                    Alis = reader.IsDBNull(3) ? 0 : reader.GetDecimal(3),
                    Satis = reader.IsDBNull(4) ? 0 : reader.GetDecimal(4),
                    OlusturmaTarih = reader.IsDBNull(5) ? DateTime.MinValue : reader.GetDateTime(5)

                };

            }
            reader.Close();
            DLL.BaglantiIslemleri();
            return kurGecmis;
        }

        public void KurKayitEkle(Guid ID, Guid ParaBirmiID, decimal Alis, decimal Satis, DateTime OlusturmaTarih)
        {
            if (ID != Guid.Empty && ParaBirmiID != Guid.Empty && Alis != 0 && Satis != 0 && OlusturmaTarih > DateTime.MinValue)
            {
                Kur kur = new Kur()
                {
                    ID = ID,
                    ParaBirimiID = ParaBirmiID,
                    Alis = Alis,
                    Satis = Satis,
                    OlusturmaTarih = OlusturmaTarih
                };

                DLL.KurKayitEkle(kur);
            }
            else
            {
                throw new Exception("Kur bilgileri eksik");
            }
        }

        public void KurBilgileriniGuncelle()
        {
            WebClient webClient = new WebClient();
            string jsonDataTxt = webClient.DownloadString("https://api.exchangerate-api.com/v4/latest/USD");
            JsonDataType dovizKurBilgileri = JsonConvert.DeserializeObject<JsonDataType>(jsonDataTxt);

            List<ParaBirimi> paraBirimleri = ParaBirimiListesi(); // Mevcut para birimleri listesi

            foreach (var item in dovizKurBilgileri.rates)
            {
                Console.WriteLine($"Kur Kodu: {item.Key}, Değer: {item.Value}");
                string code = item.Key;
                decimal satisKuru = item.Value;
                decimal alisKuru = satisKuru * 0.98m; // Örnek: alış kuru satışın %98’i

                var paraBirimi = paraBirimleri.FirstOrDefault(p => p.Code == code);

                if (paraBirimi != null)
                {
                    // Kur verisi veritabanına kaydedilir
                    KurKayitEkle(
                        Guid.NewGuid(),
                        paraBirimi.ID,
                        alisKuru,
                        satisKuru,
                        DateTime.Now
                    );

                    // Eğer satış kuru uyarı limitini geçtiyse ve e-posta adresi doluysa mail gönder
                    if (satisKuru <= paraBirimi.UyariLimit)
                    {
                        if (!string.IsNullOrWhiteSpace(paraBirimi.Email))
                        {
                            EmailGonder(
                                paraBirimi.Email,
                                paraBirimi.AdSoyad,
                                paraBirimi.Tanim,
                                paraBirimi.UyariLimit
                            );
                        }
                        else
                        {
                            Console.WriteLine($"⚠️ E-posta adresi tanımlı değil: {paraBirimi.Tanim}");
                        }
                    }
                }
            }
        }


        public DataTable KurGecmisGoruntule()  // Geri dönüş değerimiz DataTable
        {
            DataTable Dt = new DataTable();   // DataTable nesnesi oluşturduk.
            Dt.Columns.Add("Doviz Tanım", typeof(string));
            Dt.Columns.Add("Doviz Kod", typeof(string));
            Dt.Columns.Add("Alis", typeof(string));
            Dt.Columns.Add("Satis", typeof(string));
            Dt.Columns.Add("OlusturmaTarih", typeof(string));

            List<KurGecmis> KurgecmisList = KurGecmisListe(); // Kur geçmişi listesini alıyoruz.
            List<ParaBirimi> ParaBirimiList = ParaBirimiListesi(); // Para birimi listesini alıyoruz.

            for (int i = 0; i < KurgecmisList.Count; i++)
            {
                Dt.Rows.Add(
                    ParaBirimiList.FirstOrDefault(x => x.ID == KurgecmisList[i].ParaBirimiID).Tanim,
                    ParaBirimiList.FirstOrDefault(x => x.ID == KurgecmisList[i].ParaBirimiID).Code,
                    KurgecmisList[i].Alis.ToString(),
                    KurgecmisList[i].Satis.ToString(),
                    KurgecmisList[i].OlusturmaTarih.ToString("dd/MM/yyyy HH:mm") // Tarih formatı
                );
            }

            return Dt; // DataTable nesnesini döndürüyoruz.
        }

        static void EmailGonder(string EmailAdres, string EmailAdresIsimSoyisim, string ParaBirimiAdi, decimal UyariLimiti)
        {
            Encoding encode = Encoding.GetEncoding("windows-1254");

            MailMessage email = new MailMessage();
            MailAddress from = new MailAddress("karneynim@gmail.com", "Kur Takip Sistemi", encode);
            MailAddress to = new MailAddress(EmailAdres, EmailAdresIsimSoyisim, encode);

            email.To.Add(to);
            email.From = from;

            email.Subject = $"{ParaBirimiAdi} - {UyariLimiti} değerine ulaştı";
            email.Body = $"{ParaBirimiAdi} kuru, {UyariLimiti} seviyesine {DateTime.Now:dd.MM.yyyy HH:mm} tarihinde ulaştı. Alım yapabilirsiniz.";
            email.IsBodyHtml = false;

            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587;
            string kullaniciAdi = "karneynim@gmail.com";       // Gönderici adresi
            string uygulamaSifresi = "duaparzjujqhqlhq";    // Google'dan aldığın 16 karakterlik uygulama şifresi (boşluksuz yaz!)

            using (SmtpClient smtp = new SmtpClient(smtpServer, smtpPort))
            {
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential(kullaniciAdi, uygulamaSifresi); // Normal şifre değil, uygulama şifresi!

                try
                {
                    smtp.Send(email);
                    Console.WriteLine("✅ E-posta başarıyla gönderildi.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("❌ E-posta gönderme hatası: " + ex.Message);
                }
            }
        }
    }
}
