using HarmonyLib;
using UnityEngine;
using VentVigilante.Implementation.Common;
using VentVigilante.Implementation.Disks;
using VentVigilante.Implementation.Mapping;

namespace VentVigilante.Hooks;

public class CoinThrowingHooks
{
    private static int LastCoinThrown;
    private static Vector3 LastCoinImpactPosition;

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

            LastCoinThrown = __instance.GetInstanceID();
            LastCoinImpactPosition = Vector3.zero;
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
            if (__instance.GetInstanceID() != LastCoinThrown)
            {
                return;
            }
            
            if (__instance.rb.velocity.magnitude < IMPACT_THRESHOLD_VELOCITY)
            {
                return;
            }

            Vector3 coinPosition = __instance.transform.position;

            if (LastCoinImpactPosition != Vector3.zero && Vector3.Distance(LastCoinImpactPosition, coinPosition) < IMPACT_THRESHOLD_DISTANCE)
            {
                return;
            }

            LastCoinImpactPosition = coinPosition;
            AirDuctGroup.AirDuctSection startDuct = VentHelpers.PositionToAirDuct(coinPosition);

            if (startDuct == null)
            {
                return;
            }

            EcholocationPulse pulse = EcholocationPulsePool.Instance.CheckoutPoolObject();
            pulse.StartPulse(startDuct);
        }
    }
}