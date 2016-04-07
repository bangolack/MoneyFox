using System;
using Android.App;
using Android.Support.V7.Preferences;
using MoneyFox.Shared.Interfaces;

namespace MoneyFox.Droid
{
    public class RoamingSettings : IRoamingSettings
    {
        public void AddOrUpdateValue(string key, object value)
        {
            var prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
            var editor = prefs.Edit();
            editor.PutString(key, Convert.ToString(value));
            editor.Apply();
        }

        public TValueType GetValueOrDefault<TValueType>(string key, TValueType defaultValue)
        {
            var prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
            var value = prefs.GetString(key, null);

            return value != null
                ? (TValueType) Convert.ChangeType(value, typeof (TValueType))
                : defaultValue;
        }
    }
}