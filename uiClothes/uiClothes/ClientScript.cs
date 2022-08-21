using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace uiClothes
{
    internal class ClientScript : BaseScript
    {
        public Dictionary<int, string> drawableTypes;
        public JObject defaults;

        public ClientScript()
        {
            EventHandlers["onClientResourceStart"] += new Action<string>(onClientResourceStart);
        }

        void onClientResourceStart(string resourceName)
        {
            if (resourceName != API.GetCurrentResourceName())
            {
                return;
            }

            LoadDefaults();

            EventHandlers["uiClothes:takeOff"] += new Action<string>(uiClothes_takeOff);
            EventHandlers["uiClothes:putOnClothes"] += new Action<string, string, string>(uiClothes_putOnClothes);
        }

        void LoadDefaults()
        {
            var defaultsString = API.LoadResourceFile(API.GetCurrentResourceName(), "defaults.json");
            defaults = JObject.Parse(defaultsString);
        }

        int GetDefault(DrawableType dt)
        {
            var gender = Game.PlayerPed.Model.Hash == API.GetHashKey("mp_m_freemode_01") ? "m" : "w";
            var key = defaults[dt.ToString()][gender].ToObject<int>();

            return key;
        }


        void uiClothes_putOnClothes(string type, string config, string gender)
        {
            var expectedModel = gender == "m" ? "mp_m_freemode_01" : "mp_f_freemode_01";
            var possible = Game.PlayerPed.Model.Hash == API.GetHashKey(expectedModel);

            if (possible)
            {
                if (type != "pb_top")
                {
                    var w = JsonConvert.DeserializeObject<DrawableConfiguration>(config);
                    if (w.type == 0)
                    {
                        var drawableType = (DrawableType)w.drawableType;
                        var a = w.index;
                        var b = w.texture;
                        var c = w.pallete;

                        SetDrawable(drawableType, a, b, c);
                    } else
                    {
                        var propType = (PropType)w.drawableType;

                        var a = w.index;
                        var b = w.texture;

                        API.SetPedPropIndex(Game.PlayerPed.Handle, w.drawableType, a, b, true);
                        TriggerEvent("pbClothes:onPropChange");
                    }
                   
                } else
                {
                    var w = JsonConvert.DeserializeObject<List<DrawableConfiguration>>(config);

                    foreach (var item in w)
                    {
                        var drawableType = (DrawableType)item.drawableType;
                        var a = item.index;
                        var b = item.texture;
                        var c = item.pallete;

                        SetDrawable(drawableType, a, b, c);
                    }
                }
            } else
            {
                Exports["ox_lib"].notify(new
                {
                    title = "Error",
                    description = "This clothes don't fit on you",
                    type = "error"
                });
            }
        }

        void uiClothes_takeOff(string type)
        {
            if (Game.PlayerPed.Model.Hash == API.GetHashKey("mp_m_freemode_01") || Game.PlayerPed.Model.Hash == API.GetHashKey("mp_f_freemode_01"))
            {
                var gender = Game.PlayerPed.Model.Hash == API.GetHashKey("mp_m_freemode_01") ? "m" : "f";

                if (type == "top")
                {
                    var torso = GetDrawable(DrawableType.Torso);
                    var cloth = GetDrawable(DrawableType.Tops);
                    var under = GetDrawable(DrawableType.Undershirts);

                    var _default_cloth = GetDefault(DrawableType.Tops);
                    var _default_under = GetDefault(DrawableType.Undershirts);

                    if (_default_cloth != cloth.index || _default_under != under.index)
                    {

                        SetDrawable(DrawableType.Torso, GetDefault(DrawableType.Torso), 0, 0);
                        SetDrawable(DrawableType.Tops, GetDefault(DrawableType.Tops), 0, 0);
                        SetDrawable(DrawableType.Undershirts, GetDefault(DrawableType.Undershirts), 0, 0);

                        var w = new List<DrawableConfiguration>() { torso, cloth, under };
                        TriggerServerEvent("uiClothes:giveClothes", "pb_top", gender, JsonConvert.SerializeObject(w));
                    }
                }

                if (type == "mask")
                {

                    var _default = GetDefault(DrawableType.Mask);
                    var mask = GetDrawable(DrawableType.Mask);

                    if (mask.index != _default) {

                        SetDrawable(DrawableType.Mask, _default, 0, 0);

                        var w = JsonConvert.SerializeObject(mask);
                        TriggerServerEvent("uiClothes:giveClothes", "pb_mask", gender, w);
                    }
                }

                if (type == "shoes")
                {
                    var shoes = GetDrawable(DrawableType.Shoes);
                    var _default = GetDefault(DrawableType.Shoes);

                    if (shoes.index != _default)
                    {
                        SetDrawable(DrawableType.Shoes, GetDefault(DrawableType.Shoes), 0, 0);

                        var w = JsonConvert.SerializeObject(shoes);
                        TriggerServerEvent("uiClothes:giveClothes", "pb_shoes", gender, w);
                    }
                }

                if (type == "pants")
                {
                    var pants = GetDrawable(DrawableType.Legs);
                    var _default = GetDefault(DrawableType.Legs);

                    if (pants.index != _default)
                    {
                        SetDrawable(DrawableType.Legs, GetDefault(DrawableType.Legs), 0, 0);

                        var w = JsonConvert.SerializeObject(pants);
                        TriggerServerEvent("uiClothes:giveClothes", "pb_pants", gender, w);
                    }
                }

                if (type == "hat")
                {
                    var hat = GetDrawable(PropType.Hats);

                    if (hat.index > 0)
                    {
                        ClearProp(PropType.Hats);

                        var w = JsonConvert.SerializeObject(hat);
                        TriggerServerEvent("uiClothes:giveClothes", "pb_head", gender, w);
                    }
                }
                
                
                if (type == "watch")
                {
                    var watch = GetDrawable(PropType.Watches);

                    if (watch.index > 0)
                    {
                        ClearProp(PropType.Watches);

                        var w = JsonConvert.SerializeObject(watch);
                        TriggerServerEvent("uiClothes:giveClothes", "pb_watch", gender, w);
                    }
                }

                
                if (type == "glass")
                {
                    var glass = GetDrawable(PropType.Glasses);

                    if (glass.index > 0)
                    {
                        ClearProp(PropType.Glasses);

                        var w = JsonConvert.SerializeObject(glass);
                        TriggerServerEvent("uiClothes:giveClothes", "pb_glasses", gender, w);
                    }
                }

                //PropType.Bracelets
                if (type == "bracelets")
                {
                    var brace = GetDrawable(PropType.Bracelets);

                    if (brace.index > 0)
                    {
                        ClearProp(PropType.Bracelets);

                        var w = JsonConvert.SerializeObject(brace);
                        TriggerServerEvent("uiClothes:giveClothes", "pb_bracelet", gender, w);
                    }
                }

                //PropType.Ears
                if (type == "ears")
                {
                    var earrings = GetDrawable(PropType.Ears);

                    if (earrings.index > 0)
                    {
                        ClearProp(PropType.Ears);

                        var w = JsonConvert.SerializeObject(earrings);
                        TriggerServerEvent("uiClothes:giveClothes", "pb_earring", gender, w);
                    }
                }

                if(type == "armor")
                {
                    var armor = GetDrawable(DrawableType.BodyArmors);
                    var _default = GetDefault(DrawableType.BodyArmors);

                    if (armor.index != _default)
                    {
                        SetDrawable(DrawableType.BodyArmors, 0, 0, 0);

                        var w = JsonConvert.SerializeObject(armor);
                        TriggerServerEvent("uiClothes:giveClothes", "pb_armor", gender, w);
                    }

                }

                if (type == "bag")
                {
                    var bag = GetDrawable(DrawableType.BagsandParachutes);
                    var _default = GetDefault(DrawableType.BagsandParachutes);

                    if (bag.index != _default)
                    {
                        SetDrawable(DrawableType.BagsandParachutes, GetDefault(DrawableType.BagsandParachutes), 0, 0);

                        var w = JsonConvert.SerializeObject(bag);
                        TriggerServerEvent("uiClothes:giveClothes", "pb_backpack", gender, w);
                    }
                }
            } else
            {
                Exports["ox_lib"].notify(new {
                    title = "Error",
                    description = "You can't takeoff/put on clothes from this model",
                    type = "error"
                });
            }
        }

        public void SetDrawable(DrawableType dt, int t, int tx, int v)
        {
            API.SetPedComponentVariation(Game.PlayerPed.Handle, (int)dt, t, tx, v);
            TriggerEvent("pbClothes:onDrawableChange");
        }

        public void ClearProp(PropType pt)
        {
            API.ClearPedProp(Game.PlayerPed.Handle, (int)pt);
            TriggerEvent("pbClothes:onPropChange");
        }

        public DrawableConfiguration GetDrawable(DrawableType dt)
        {
            var pped = Game.PlayerPed.Handle;
            var id = API.GetPedDrawableVariation(pped, (int)dt);
            var texture = API.GetPedTextureVariation(pped, (int)dt);
            var pallete = API.GetPedPaletteVariation(pped, (int)dt);

            return new DrawableConfiguration((int)dt, 0, id, texture, pallete);
        }

        public DrawableConfiguration GetDrawable(PropType pt)
        {
            var pped = Game.PlayerPed.Handle;

            var id = API.GetPedPropIndex(pped, (int)pt);
            var texture = API.GetPedPropTextureIndex(pped, (int)pt);
            var pallete = 0;

            return new DrawableConfiguration((int)pt, 100, id, texture, pallete);
        }

        public class DrawableConfiguration
        {
            public int drawableType;
            public int type;
            public int index;
            public int texture;
            public int pallete;

            public DrawableConfiguration(int dwt, int commonType, int i, int t, int p)
            {
                drawableType = dwt;
                index = i;
                texture = t;
                pallete = p;
                type = commonType;
            }
        } 

        public class Animation
        {
            public string dict;
            public string clip;

            public Animation(string d, string c)
            {
                dict = d;
                clip = c;
            }
        }

        public enum PropType
        {
            Hats = 0,
            Glasses,
            Ears,
            Watches = 6,
            Bracelets = 7

        }

        public enum DrawableType
        {
            Head = 0,
            Mask,
            HairStyles,
            Torso,
            Legs,
            BagsandParachutes,
            Shoes,
            Accessories,
            Undershirts,
            BodyArmors,
            Decals,
            Tops
        }

        void cloth(object data, object slot)
        {
            var data_json = JsonConvert.SerializeObject(data);
            var slot_json = JsonConvert.SerializeObject(slot);

            Debug.WriteLine("Data");
            Debug.WriteLine(data_json);
            Debug.WriteLine("Slot");
            Debug.WriteLine(slot_json);
        }

        void PlayAnimation(Animation am, int amount)
        {

            Game.PlayerPed.Task.PlayAnimation(am.dict, am.clip, 1f, amount, AnimationFlags.None);
        }
    }
}
