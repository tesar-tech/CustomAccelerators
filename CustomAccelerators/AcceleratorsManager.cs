using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Security.Cryptography.Certificates;
using Windows.Storage;
using Windows.System;

namespace CustomAccelerators
{
    public class AcceleratorsManager
    {
        internal static List<CustomAccelerator> HangingAccelerators { get; set; } = new List<CustomAccelerator>();

        /// <summary>
        /// this is called when customAcc is constructed in xaml
        /// </summary>
        /// <param name="ca"></param>
        internal static void AddToList(CustomAccelerator ca)
        {
            if (AcceleratorsDefinitions.Count == 0)
            {//when there is no definition (xaml in constructed before accelerators definiton 
                //(which is right because it time consuming operation) it will add to hanging accelerator)
                HangingAccelerators.Add(ca);
            }
            else
            {  //for examle accelerator from non-default page is added
                AddCustomAcceleratorToDefinition(ca);
            }
        }

        /// <summary>
        /// Pairing accelerator to definitions.
        /// </summary>
        /// <param name="ca"></param>
        private static void AddCustomAcceleratorToDefinition(CustomAccelerator ca)
        {
            AcceleratorDefinition definitionForCa;

            try
            {  //xaml accelerator and accelerators definition is paired by identity
                definitionForCa = AcceleratorsDefinitions.SingleOrDefault(x => x.Identity == ca.Identity);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException($"There is probably multiple definitions of same identity({ca.Identity})", ex);
            }

            if (definitionForCa == null)
            {
                Debug.WriteLine($"There is no definition for accelerator {ca.Identity}");
            }
            else
            {
                if(ca.GetType()== typeof(CustomAcceleratorSecondary))
                    definitionForCa.CustomAcceleratorSecondary = ca;
                else
                definitionForCa.CustomAccelerator = ca;
                ca.SetKeyModifierLabel(definitionForCa.Key, definitionForCa.Modifiers, definitionForCa.Label);
            }

        }

        private static bool _isLoaded = false;

        /// <summary>
        ///for every definition try to load key and modifiers from local setting
        ///if not set, use the default value.
        ///Also add hanging accelerators (previously constructed by xaml)
        /// </summary>
        /// <param name="defaults"></param>
        public static void AddDefaultsAndLoadFromStorage(IEnumerable<(string identity, string label, VirtualKey key, VirtualKeyModifiers modifiers)> defaults, ResourceLoader resourceLoader = null)
        {
            if (_isLoaded)//Let it construct just once (dont want to add same accelerator when user returns to main page)
            {
                return;
            }
            foreach (var (identity, label, keyFromDefinition, modifiersFromDefinition) in defaults)
            {

                AcceleratorDefinition accDef = new AcceleratorDefinition(identity, label);
                var keyFromStorageSetting = LocalSettingsStorage.GetStoredKeyForIdentity(identity);
                accDef.Key = keyFromStorageSetting != VirtualKey.None ? keyFromStorageSetting : keyFromDefinition;

                var (wasSet, modifiersFromStorageSetting) = LocalSettingsStorage.GetStoredModifiersForIdentity(identity);
                accDef.Modifiers = wasSet ? modifiersFromStorageSetting : modifiersFromDefinition;

                AcceleratorsDefinitions.Add(accDef);
            }
            if (HangingAccelerators.Any())
            {
                HangingAccelerators.ForEach(ca => AddCustomAcceleratorToDefinition(ca));
                HangingAccelerators.Clear();//it is not necessary (will not be used in future)
            }
            _isLoaded = true;
        }

        /// <summary>
        /// collection for Edit control
        /// </summary>
        public static ObservableCollection<AcceleratorDefinition> AcceleratorsDefinitions { get; set; } = new ObservableCollection<AcceleratorDefinition>();


    }

}
