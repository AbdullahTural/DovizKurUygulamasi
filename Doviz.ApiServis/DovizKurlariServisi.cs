using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Doviz.ApiServis
{
    partial class DovizKurlariServisi : ServiceBase
    {
        public Timer t; // Timer nesnesi oluşturuldu.
        public DovizKurlariServisi()
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
            t.Start(); // Timer başlatıldı.
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
