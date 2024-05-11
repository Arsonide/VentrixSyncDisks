using HarmonyLib;
using UnityEngine;
using VentrixSyncDisks.Implementation.Common;
using VentrixSyncDisks.Implementation.Disks;
using VentrixSyncDisks.Implementation.Mapping;

namespace VentrixSyncDisks.Hooks;

public class MappingHooks
{
    public static PhysicsProfile CoinImpactPhysicsProfile;

    private static int _lastCoinThrown;
    private static Vector3 _lastCoinImpactPosition;

    private const string COIN_NAME = "WorldCoin";
    private const float IMPACT_THRESHOLD_DISTANCE = 1f;
    private const float IMPACT_THRESHOLD_VELOCITY = 0.1f;

    private static bool IsPlayerThrownCoin(InteractableController controller)
    {
        if (controller == null || controller.thrownBy != Player.Instance)
        {
            return false;
        }

        return controller.gameObject.name.StartsWith(COIN_NAME);
    }
    
    [HarmonyPatch(typeof(InteractableController), "DropThis")]
    public class InteractableControllerDropThisHook
    {
        [HarmonyPostfix]
        public static void Postfix(InteractableController __instance, bool throwThis)
        {
            if (!throwThis || !IsPlayerThrownCoin(__instance))
            {
                return;
            }

            _lastCoinThrown = __instance.GetInstanceID();
            _lastCoinImpactPosition = Vector3.zero;
        }
    }

    [HarmonyPatch(typeof(InteractableController), "OnCollisionEnter")]
    public class InteractableControllerOnCollisionEnterHook
    {
        [HarmonyPostfix]
        public static void Postfix(InteractableController __instance)
        {
            if (DiskRegistry.MappingDisk.Level <= 0)
            {
                return;
            }
            
            if (!IsPlayerThrownCoin(__instance))
            {
                return;
            }
            
            // We're going to do a lot of early outs here to make sure a coin vibrating in the corner doesn't spam echolocation pulses.
            if (__instance.GetInstanceID() != _lastCoinThrown)
            {
                return;
            }
            
            if (__instance.rb.velocity.magnitude < IMPACT_THRESHOLD_VELOCITY)
            {
                return;
            }

            if (CoinImpactPhysicsProfile == null)
            {
                CoinImpactPhysicsProfile = __instance.interactable.preset.GetPhysicsProfile();
            }
            
            Vector3 coinPosition = __instance.transform.position;

            if (_lastCoinImpactPosition != Vector3.zero && Vector3.Distance(_lastCoinImpactPosition, coinPosition) < IMPACT_THRESHOLD_DISTANCE)
            {
                return;
            }

            _lastCoinImpactPosition = coinPosition;
            AirDuctGroup.AirDuctSection startDuct = Utilities.PositionToAirDuct(coinPosition);

            if (startDuct == null)
            {
                return;
            }

            EcholocationPulse pulse = EcholocationPulsePool.Instance.CheckoutPoolObject();
            pulse.StartPulse(startDuct);
        }
    }
}