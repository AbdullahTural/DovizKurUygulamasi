using Doviz.Core;
using Doviz.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Doviz.WinApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Doviz.Core.BusinessLogicLayer BLL = new Doviz.Core.BusinessLogicLayer();
            BLL.KurBilgileriniGuncelle();
        }

        private void btn_jsonkurgu_Click(object sender, EventArgs e)
        {
            Doviz.Core.BusinessLogicLayer BLL = new Doviz.Core.BusinessLogicLayer();
            BLL.KurBilgileriniGuncelle();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Doviz.Core.BusinessLogicLayer Bll = new Doviz.Core.BusinessLogicLayer();
            List<ParaBirimi> ParaBirimleri = Bll.ParaBirimiListesi();  
            List<Kur> KurBilgileri = Bll.KurListe(); // Kur listesi

            Kur Dolar = KurBilgileri.FirstOrDefault(I => I.ParaBirimiID == ParaBirimleri.FirstOrDefault(x => x.Code == "USD").ID);

            lbl_DolarAlis.Text = Dolar.Alis.ToString();
            lbl_DolarSatis.Text = Dolar.Satis.ToString();

            Kur Euro = KurBilgileri.FirstOrDefault(I => I.ParaBirimiID == ParaBirimleri.FirstOrDefault(x => x.Code == "EUR").ID);

            lbl_EuroAlis.Text = Euro.Alis.ToString();
            lbl_EuroSatis.Text = Euro.Satis.ToString();

            Kur Sterlin = KurBilgileri.FirstOrDefault(I => I.ParaBirimiID == ParaBirimleri.FirstOrDefault(x => x.Code == "GBP").ID);

            lbl_SterlinAlis.Text = Sterlin.Alis.ToString();
            lbl_SterlinSatis.Text = Sterlin.Satis.ToString();

            grd_kurgecmis.DataSource = Bll.KurGecmisGoruntule();
        }
    }
}
