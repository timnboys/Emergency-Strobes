﻿namespace Emergency_Strobes
{
    // RPH
    using Rage;
    using Rage.Native;

    [System.Flags]
    internal enum VehicleLight
    {
        LeftHeadlight = 1 << 0,
        RightHeadlight = 1 << 1,

        LeftTailLight = 1 << 2,
        RightTailLight = 1 << 3,

        LeftFrontIndicatorLight = 1 << 4,
        RightFrontIndicatorLight = 1 << 5,
        LeftRearIndicatorLight = 1 << 6,
        RightRearIndicatorLight = 1 << 7,

        LeftBrakeLight = 1 << 8,
        RightBrakeLight = 1 << 9,
        MiddleBrakeLight = 1 << 10,

        All = LeftHeadlight | RightHeadlight | LeftTailLight | RightTailLight | LeftBrakeLight | RightBrakeLight | LeftFrontIndicatorLight | RightFrontIndicatorLight | LeftRearIndicatorLight | RightRearIndicatorLight | MiddleBrakeLight,
    }

    internal static class Extensions
    {
        private static int BrokenLightsOffset;
        private static int LightMultiplierOffset;
        private static int ShouldRenderBrokenLightsOffset;
        private static int ShouldRenderBrokenSirenLightsOffset;

        static Extensions()
        {
            switch (Game.ProductVersion.Build)
            {
                default:
                case 944:
                    BrokenLightsOffset = 0x07BC;
                    LightMultiplierOffset = 0x0954;
                    ShouldRenderBrokenLightsOffset = 0x07C4;
                    ShouldRenderBrokenSirenLightsOffset = 0x07C5;
                    break;
                case 877:
                    BrokenLightsOffset = 0x079C;
                    LightMultiplierOffset = 0x092C;
                    ShouldRenderBrokenLightsOffset = 0x07A4;
                    ShouldRenderBrokenSirenLightsOffset = 0x07A5;
                    break;
                case 791:
                    BrokenLightsOffset = 0x077C;
                    LightMultiplierOffset = 0x090C;
                    ShouldRenderBrokenLightsOffset = 0x0784;
                    ShouldRenderBrokenSirenLightsOffset = 0x0785;
                    break;
            }
        }

        public static unsafe void SetLightBroken(this Vehicle v, VehicleLight light, bool broken)
        {
            int mask = (int)light;

            if (broken)
            {
                *(int*)(v.MemoryAddress.ToInt64() + BrokenLightsOffset) |= mask;
            }
            else
            {
                *(int*)(v.MemoryAddress.ToInt64() + BrokenLightsOffset) &= ~mask;
            }
        }

        public static unsafe bool IsLightBroken(this Vehicle v, VehicleLight light)
        {
            return (*(int*)(v.MemoryAddress.ToInt64() + BrokenLightsOffset) & (int)light) == (int)light;
        }

        public static Vector3 GetDeformationAt(this Vehicle v, Vector3 offset)
        {
            return NativeFunction.Natives.GetVehicleDeformationAtPos<Vector3>(v, offset.X, offset.Y, offset.Z);
        }

        public static unsafe void SetLightMultiplier(this Vehicle v, float multiplier)
        {
            //NativeFunction.Natives.SetVehicleLightMultiplier(v, multiplier);
            *(float*)(v.MemoryAddress + LightMultiplierOffset) = multiplier;
        }

        public static unsafe float GetLightMultiplier(this Vehicle v)
        {
            return *(float*)(v.MemoryAddress + LightMultiplierOffset);
        }

        public static unsafe void SetBrokenLightsRenderedAsBroken(this Vehicle v, bool broken)
        {
            *(byte*)(v.MemoryAddress + ShouldRenderBrokenLightsOffset) = (byte)(broken ? 1 : 0);
        }

        public static unsafe bool AreBrokenLightsRenderedAsBroken(this Vehicle v)
        {
            byte value = *(byte*)(v.MemoryAddress + ShouldRenderBrokenLightsOffset);
            return value == 1;
        }

        public static unsafe void SetBrokenSirenLightsRenderedAsBroken(this Vehicle v, bool broken)
        {
            *(byte*)(v.MemoryAddress + ShouldRenderBrokenSirenLightsOffset) = (byte)(broken ? 1 : 0);
        }

        public static unsafe bool AreBrokenSirenLightsRenderedAsBroken(this Vehicle v)
        {
            byte value = *(byte*)(v.MemoryAddress + ShouldRenderBrokenSirenLightsOffset);
            return value == 1;
        }
    }
}
