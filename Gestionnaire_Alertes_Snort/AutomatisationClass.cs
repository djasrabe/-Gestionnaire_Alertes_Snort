using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Gestionnaire_Alertes_Snort
{

    public delegate void ModeAutoActiveEventHandler (object sender,EventArgs e);
   

    public class AutomatisationClass:EventArgs
    {
        public event ModeAutoActiveEventHandler ModeAutoActive;
       
        public void LeveeEvenement()
        {
            if (ModeAutoActive != null)
                ModeAutoActive(this, EventArgs.Empty);
        }
        
    }
}
