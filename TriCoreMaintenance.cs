using System;
using HCSS.DomainModel.Core;
using HCSS.DomainModel.Fueler;
using HCSS.Framework;
using HCSS.Framework.DataLayer;
using HCSS.Framework.Services;

namespace TriCoreMaintenance {
    public class TriCoreMaintenance : ILibrary
    {
        public string Name
        {
            get { return "TriCoreMaintenance"; }
        }

        public void Initialize()
        {
            App.Current.LoadCommands(Utility.GetTextResource("TriCoreMaintenance.DataCommand.xml"));

            var timer = new System.Timers.Timer() {
                Enabled = true,
                Interval = 60000
            };
            
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            try {
                var target = sender as System.Timers.Timer;
                target.Stop();

                var transfers = new FluidTransferList();
                transfers.Load(Database.Query("TriCoreMaintenance.GetAllTransfersForPetrolCanada"));
                transfers.Delete();

                Database.Execute("TriCoreMaintenance.Cleanup");
            }
            catch(Exception ex) {
                App.Current.ExceptionManager.Publish(ex);
            }
        }
    }
}
