using HarmonyLib;
using UnityEngine;
using UnityEngine.Pool;
using VentVigilante.Implementation.Common;
using VentVigilante.Implementation.Renderers;

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

            Utilities.Log("Player Threw Coin");
        }
    }

    [HarmonyPatch(typeof(InteractableController), "OnCollisionEnter")]
    public class InteractableControllerOnCollisionEnterHook
    {
        [HarmonyPostfix]
        public static void Postfix(InteractableController __instance)
        {
            if (!IsPlayerThrownCoin(__instance))
            {
                return;
            }

            ObjectPool<>
            Utilities.Log("Coin Impact");

            // We're going to do a lot of early outs here to make sure a coin vibrating in the corner doesn't spam echolocation pulses.
            if (__instance.GetInstanceID() != LastCoinThrown)
            {
                Utilities.Log("Coin Impact EO1");
                return;
            }
            
            if (__instance.rb.velocity.magnitude < IMPACT_THRESHOLD_VELOCITY)
            {
                Utilities.Log("Coin Impact EO2");
                return;
            }

            Vector3 coinPosition = __instance.transform.position;

            if (LastCoinImpactPosition != Vector3.zero && Vector3.Distance(LastCoinImpactPosition, coinPosition) < IMPACT_THRESHOLD_DISTANCE)
            {
                Utilities.Log("Coin Impact EO3");
                return;
            }

            LastCoinImpactPosition = coinPosition;
            AirDuctGroup.AirDuctSection startDuct = VentHelpers.PositionToAirDuct(coinPosition);

            if (startDuct == null)
            {
                Utilities.Log("Coin Impact EO4");
                return;
            }

            GameObject pulse = new GameObject("Pulse");
            EcholocationPulse p = pulse.AddComponent<EcholocationPulse>();
            p.StartPulse(startDuct);
        }
    }
}