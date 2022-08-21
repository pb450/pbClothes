using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace uiClothes.Server
{
    internal class ServerScript : BaseScript
    {
        public ServerScript()
        {
            EventHandlers["uiClothes:giveClothes"] += new Action<Player, string, string, string>(giveClothes);
        }

        void giveClothes([FromSource]Player ply, string type, string gender, string configuration)
        {
            var id = int.Parse(ply.Handle);

            //var message = $"{id} requests to get {type} in conf: {configuration}";
            //Console.WriteLine(message);

            Dictionary<string, string> config = new Dictionary<string, string>();
            config.Add("gender", gender);
            config.Add("config", configuration);

            Exports["ox_inventory"].AddItem(id, type, 1, config);
        }
    }
}
