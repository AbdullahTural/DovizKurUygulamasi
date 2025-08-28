using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doviz.Core
{
    public  class Base
    {
        public void TryCatchKullan(Action _action)
        {
            try
            {
                _action();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata oluştu: " + ex.Message);

                throw;
            }
        }

    }
}
