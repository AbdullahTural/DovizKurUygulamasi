using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;


namespace DovizApiServis
{
    public partial class Service1 : ServiceBase
    {
        private Timer t;
        public Service1()
        {
            InitializeComponent();
            t = new Timer(120000); //2 dk olarak ayarlandı.
            t.Elapsed += T_Elapsed; // Timer her 2 dakikada bir tetiklenecek.
        }

        private void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            Doviz.Core.BusinessLogicLayer BLL = new Doviz.Core.BusinessLogicLayer();
            BLL.KurBilgileriniGuncelle(); // Kur bilgilerini güncelle.
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                t.Start();
            }
            catch (Exception ex)
            {
                // Hata mesajını dosyaya yaz
                File.WriteAllText("C:\\ServiceError.log", ex.ToString());
            }
        }

        protected override void OnStop()
        {
            t.Stop(); // Timer durduruldu.
        }

        protected override void OnContinue()
        {
            t.Start();
        }


        protected override void OnPause()
        {
            t.Stop();
        }

        protected override void OnShutdown()
        {
            t.Stop();
        }
    }
}
