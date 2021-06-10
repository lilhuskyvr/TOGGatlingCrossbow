using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TOGModFramework;
using UnityEngine;

namespace TOGGatlingCrossbow
{
    public class TOGGatlingCrossbowPatch : MonoBehaviour
    {
        public string modName = "TOGGatlingCrossbow";
        public bool isSemiAuto;
        private bool isCrossbowBeingReloaded;

        public void Inject()
        {
            EventManager.OnCrossbowBoltFired += EventManagerOnonCrossbowBoltFired;

            var json = FileManager.GetModJsonData(modName);
            var data = JsonConvert.DeserializeObject<Settings>(json);
            isSemiAuto = data.isSemiAuto;
        }

        public IEnumerator ReloadCrossbow(CrossBowScript crossBowScript)
        {
            isCrossbowBeingReloaded = true;
            var currentHandController = crossBowScript.wp.isHeldRight
                ? Player.local.controlInput.RightController
                : Player.local.controlInput.LeftController;

            if (isSemiAuto)
            {
                while (currentHandController.triggerPressed)
                {
                    yield return new WaitForFixedUpdate();
                }
            }

            if (!crossBowScript.isLoaded)
            {
                crossBowScript.isLoaded = true;
                crossBowScript.isStringPulled = true;
                crossBowScript.Bolt.GetComponent<MeshRenderer>().enabled = true;
                crossBowScript.EquipControl.SetCrossBowLoaded(true);
                crossBowScript.BowString.localPosition =
                    new Vector3(0.0f, crossBowScript.BowString.localPosition.y, 0.008f);
            }

            isCrossbowBeingReloaded = false;

            yield return null;
        }

        private void EventManagerOnonCrossbowBoltFired(CrossBowScript crossBowScript)
        {
            if (!isCrossbowBeingReloaded)
                ConfigManager.local.StartCoroutine(ReloadCrossbow(crossBowScript));
        }
    }
}