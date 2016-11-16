// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by the Game Data Editor.
//
//      Changes to this file will be lost if the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using UnityEngine;
using System;
using System.Collections.Generic;

using GameDataEditor;

namespace GameDataEditor
{
    public class GDEBombData : IGDEData
    {
        private static string costKey = "cost";
		private int _cost;
        public int cost
        {
            get { return _cost; }
            set {
                if (_cost != value)
                {
                    _cost = value;
                    GDEDataManager.SetInt(_key+"_"+costKey, _cost);
                }
            }
        }

        private static string numberKey = "number";
		private int _number;
        public int number
        {
            get { return _number; }
            set {
                if (_number != value)
                {
                    _number = value;
                    GDEDataManager.SetInt(_key+"_"+numberKey, _number);
                }
            }
        }

        private static string nameKey = "name";
		private string _name;
        public string name
        {
            get { return _name; }
            set {
                if (_name != value)
                {
                    _name = value;
                    GDEDataManager.SetString(_key+"_"+nameKey, _name);
                }
            }
        }

        private static string thumbKey = "thumb";
		private string _thumb;
        public string thumb
        {
            get { return _thumb; }
            set {
                if (_thumb != value)
                {
                    _thumb = value;
                    GDEDataManager.SetString(_key+"_"+thumbKey, _thumb);
                }
            }
        }

        private static string descKey = "desc";
		private string _desc;
        public string desc
        {
            get { return _desc; }
            set {
                if (_desc != value)
                {
                    _desc = value;
                    GDEDataManager.SetString(_key+"_"+descKey, _desc);
                }
            }
        }

        public GDEBombData()
		{
			_key = string.Empty;
		}

		public GDEBombData(string key)
		{
			_key = key;
		}
		
        public override void LoadFromDict(string dataKey, Dictionary<string, object> dict)
        {
            _key = dataKey;

			if (dict == null)
				LoadFromSavedData(dataKey);
			else
			{
                dict.TryGetInt(costKey, out _cost);
                dict.TryGetInt(numberKey, out _number);
                dict.TryGetString(nameKey, out _name);
                dict.TryGetString(thumbKey, out _thumb);
                dict.TryGetString(descKey, out _desc);
                LoadFromSavedData(dataKey);
			}
		}

        public override void LoadFromSavedData(string dataKey)
		{
			_key = dataKey;
			
            _cost = GDEDataManager.GetInt(_key+"_"+costKey, _cost);
            _number = GDEDataManager.GetInt(_key+"_"+numberKey, _number);
            _name = GDEDataManager.GetString(_key+"_"+nameKey, _name);
            _thumb = GDEDataManager.GetString(_key+"_"+thumbKey, _thumb);
            _desc = GDEDataManager.GetString(_key+"_"+descKey, _desc);
         }

        public void Reset_cost()
        {
            GDEDataManager.ResetToDefault(_key, costKey);

            Dictionary<string, object> dict;
            GDEDataManager.Get(_key, out dict);
            dict.TryGetInt(costKey, out _cost);
        }

        public void Reset_number()
        {
            GDEDataManager.ResetToDefault(_key, numberKey);

            Dictionary<string, object> dict;
            GDEDataManager.Get(_key, out dict);
            dict.TryGetInt(numberKey, out _number);
        }

        public void Reset_name()
        {
            GDEDataManager.ResetToDefault(_key, nameKey);

            Dictionary<string, object> dict;
            GDEDataManager.Get(_key, out dict);
            dict.TryGetString(nameKey, out _name);
        }

        public void Reset_thumb()
        {
            GDEDataManager.ResetToDefault(_key, thumbKey);

            Dictionary<string, object> dict;
            GDEDataManager.Get(_key, out dict);
            dict.TryGetString(thumbKey, out _thumb);
        }

        public void Reset_desc()
        {
            GDEDataManager.ResetToDefault(_key, descKey);

            Dictionary<string, object> dict;
            GDEDataManager.Get(_key, out dict);
            dict.TryGetString(descKey, out _desc);
        }

        public void ResetAll()
        {
            GDEDataManager.ResetToDefault(_key, nameKey);
            GDEDataManager.ResetToDefault(_key, costKey);
            GDEDataManager.ResetToDefault(_key, thumbKey);
            GDEDataManager.ResetToDefault(_key, descKey);
            GDEDataManager.ResetToDefault(_key, numberKey);


            Dictionary<string, object> dict;
            GDEDataManager.Get(_key, out dict);
            LoadFromDict(_key, dict);
        }
    }
}
