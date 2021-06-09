using TOGModFramework;
using UnityEngine;

namespace TOGGatlingCrossbow
{
    public class TOGGatlingCrossbowPatch : MonoBehaviour
    {
        public void Inject()
        {
            EventManager.OnCrossbowBoltFired += EventManagerOnonCrossbowBoltFired;
        }

        private void EventManagerOnonCrossbowBoltFired(CrossBowScript crossBowScript)
        {
            if (!crossBowScript.isLoaded)
            {
                crossBowScript.isLoaded = true;
                crossBowScript.isStringPulled = true;
                crossBowScript.Bolt.GetComponent<MeshRenderer>().enabled = true;
                crossBowScript.EquipControl.SetCrossBowLoaded(true);
                crossBowScript.BowString.localPosition =
                    new Vector3(0.0f, crossBowScript.BowString.localPosition.y, 0.008f);
            }
        }
    }
}